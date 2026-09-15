using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CleanArchitecture.Blazor.Infrastructure.Services.TpLink;

/// <summary>
/// Client for TP-Link TL-MR6400 v5.3 firmware 1.6.0.
/// Uses the GDPR encryption protocol (AES + RSA) as documented by the
/// @hertzg/tplink-api library: https://jsr.io/@hertzg/tplink-api
/// 
/// ACT constants from the library:
///   GET: number  (read settings)
///   SET: number  (write settings)
///   GS:  number  (get settings with specific fields)
/// </summary>
public class TpLinkLteClient
{
    private readonly HttpClient _http;
    private string? _sessionId;
    private string? _tokenId;
    private byte[]? _aesKey;
    private byte[]? _aesIv;

    // ACT action type constants (from @hertzg/tplink-api)
    private const int ACT_GET = 1;
    private const int ACT_SET = 2;
    private const int ACT_GS = 3;

    public TpLinkLteClient(HttpClient http) => _http = http;

    public async Task<bool> LoginAsync(string username, string password, CancellationToken ct)
    {
        // STEP 1: GET / to fetch the login page and extract initial parameters.
        var loginPage = await _http.GetStringAsync("/", ct);

        // STEP 2: GET /cgi/getParm to obtain the RSA public key (modulus n, exponent e)
        //         and the sequence number.
        // Reference: 0xf15h/tp_link_gdpr shows RSA n: E700... and RSA e: 010001
        var parmJson = await _http.GetStringAsync("/cgi/getParm", ct);

        // TODO: Parse parmJson to extract RSA modulus and exponent.
        // The response format is typically: {"n":"<hex>","e":"<hex>","seq":<number>}
        // var rsaModulus = ...;
        // var rsaExponent = ...;
        // var sequence = ...;

        // STEP 3: Generate random AES key and IV.
        using var aes = Aes.Create();
        aes.KeySize = 128;
        aes.GenerateKey();
        aes.GenerateIV();
        _aesKey = aes.Key;
        _aesIv = aes.IV;

        // STEP 4: Build login payload.
        // The payload is a JSON object with username and password.
        var loginPayload = new Dictionary<string, string>
        {
            ["username"] = username,
            ["password"] = password
        };
        var payloadJson = JsonSerializer.Serialize(loginPayload);

        // STEP 5: Encrypt payload with AES-CBC.
        var encryptedData = EncryptAes(payloadJson, _aesKey, _aesIv);

        // STEP 6: Build and encrypt the sign string.
        // Format: "key=<hex_key>&iv=<hex_iv>&h=<md5(username+password)>&s=<seq+dataLength>"
        // Reference: 0xf15h/tp_link_gdpr shows: "key=...&iv=...&h=...&s=..."
        var keyHex = Convert.ToHexString(_aesKey).ToLowerInvariant();
        var ivHex = Convert.ToHexString(_aesIv).ToLowerInvariant();
        var md5Input = username + password;
        var md5Hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(md5Input))).ToLowerInvariant();
        // var sValue = sequence + encryptedData.Length; // Use parsed sequence
        var signPlain = $"key={keyHex}&iv={ivHex}&h={md5Hash}&s=0"; // TODO: replace s=0

        // TODO: Encrypt signPlain with the RSA public key.
        // var encryptedSign = RsaEncrypt(signPlain, rsaModulus, rsaExponent);

        // STEP 7: POST to /cgi/login with the encrypted envelope.
        // The request body is a JSON object: { "data": "...", "sign": "..." }
        // var loginRequest = new { data = encryptedData, sign = encryptedSign };
        // var resp = await _http.PostAsJsonAsync("/cgi/login", loginRequest, ct);

        // TODO: Parse the response to extract sessionId and tokenId.
        // The response is typically: {"success":true,"data":{"sessionId":"...","tokenId":"..."}}
        // _sessionId = ...;
        // _tokenId = ...;

        throw new NotImplementedException(
            "Complete the login flow using @hertzg/tplink-api as reference.");
    }

    private static string EncryptAes(string plaintext, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var inputBytes = Encoding.UTF8.GetBytes(plaintext);
        var outputBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        return Convert.ToBase64String(outputBytes);
    }

    /// <summary>
    /// Executes an action on the router.
    /// Actions are tuples: [actionType, actionName, parameters?]
    /// Examples from @hertzg/tplink-api:
    ///   [ACT.GET, "LTE_SMS_UNREADMSGBOX", ["totalNumber"]]
    ///   [ACT.SET, "LTE_SMS_UNREADMSGBOX", { pageNumber: "1" }]
    ///   [ACT.GS, "LTE_SMS_UNREADMSGENTRY", ["index", "from", "content", "receivedTime"]]
    /// </summary>
    public async Task<string> ExecuteActionAsync(
        int actionType, string actionName, object? parameters, CancellationToken ct)
    {
        // TODO: Build the encrypted request envelope.
        // The actions array is serialized to JSON, then encrypted with AES and signed with RSA.
        // POST to /cgi?2 and decrypt the response with AES.
        // Reference: @hertzg/tplink-api execute() function.
        throw new NotImplementedException(
            "Implement action execution based on @hertzg/tplink-api source.");
    }

    public Task<string> GetUnreadCountAsync(CancellationToken ct)
    {
        return ExecuteActionAsync(ACT_GET, "LTE_SMS_UNREADMSGBOX",
            new[] { "totalNumber" }, ct);
    }

    public Task<string> SetPageNumberAsync(int pageNumber, CancellationToken ct)
    {
        return ExecuteActionAsync(ACT_SET, "LTE_SMS_UNREADMSGBOX",
            new Dictionary<string, string> { ["pageNumber"] = pageNumber.ToString() }, ct);
    }

    public Task<string> GetUnreadEntriesAsync(CancellationToken ct)
    {
        return ExecuteActionAsync(ACT_GS, "LTE_SMS_UNREADMSGENTRY",
            new[] { "index", "from", "content", "receivedTime" }, ct);
    }
}