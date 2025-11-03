using CleanArchitecture.Blazor.Infrastructure.Services.Wialon.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CleanArchitecture.Blazor.Infrastructure.Services.Wialon;
public class WialonSession : IWialonSession, IAsyncDisposable
{
    private JObject _jsonConnectionInfo;
    private readonly HttpClient _httpClient;
    private readonly ILogger<WialonSession> _logger;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    private string _cachedToken;
    

    public WialonSession(HttpClient httpClient, IOptionsSnapshot<WialonSettings> settings, ILogger<WialonSession> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cachedToken = settings.Value.Token;
    }

    public async Task<bool> TryConnect() => await TryConnect(_cachedToken);

    private async Task<bool> TryConnect(string token)
    {

        await _tokenLock.WaitAsync();

        try
        {
            // Create form data
            var formData = new Dictionary<string, string>
            {
                ["svc"] = "token/login",
                ["params"] = JsonConvert.SerializeObject(new { token })
            };

            // Send request
            var response = await _httpClient.PostAsync("", new FormUrlEncodedContent(formData));

            // Read response content as string
            string responseContent = await response.Content.ReadAsStringAsync();

            // Parse JSON response
            _jsonConnectionInfo = JObject.Parse(responseContent);

            // Check for errors
            var error = CheckError(_jsonConnectionInfo);

            if (!string.IsNullOrWhiteSpace(error)) throw new Exception(error);

            // Check for 'reason' field in response
            if (_jsonConnectionInfo["reason"] != null)
            {
                string reason = _jsonConnectionInfo["reason"]!.Value<string>()!;

                if (!string.IsNullOrWhiteSpace(reason))
                {
                    throw new Exception($"Подключение установлено, но получена следующая ошибка от сервера: {reason}");
                }
            }

            _logger.LogDebug("App Connected!");

            return true;

        }
        catch (Exception ex)
        {
            _logger.LogDebug("Faild to connect: {Params}", ex.Message);
            return false;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    public async Task<bool> TryDisconnect()
    {
        await _tokenLock.WaitAsync();

        try
        {
            var formData = new Dictionary<string, string>
            {
                ["svc"] = "core/logout",
                ["params"] = "{}",
                ["sid"] = _cachedToken
            };

            var response = await _httpClient.PostAsync("", new FormUrlEncodedContent(formData));

            string responseContent = await response.Content.ReadAsStringAsync();

            var jsonResponse = JObject.Parse(responseContent);

            // Check for errors
            var error = CheckError(_jsonConnectionInfo);

            if (!string.IsNullOrWhiteSpace(error)) throw new Exception(error);

            // Check for 'reason' field in response
            if (_jsonConnectionInfo["reason"] != null)
            {
                string reason = _jsonConnectionInfo["reason"]!.Value<string>()!;

                if (!string.IsNullOrWhiteSpace(reason))
                {
                    throw new Exception($"The connection was terminated, but the following error was received from the server: {reason}");
                }
            }

            _logger.LogDebug("App Disconnected!");

            return true;

        }
        catch (Exception ex)
        {
            _logger.LogDebug("Faild to disconnect: {Params}", ex.Message);
            return false;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    public async Task<JObject> SendRequest(string path, params KeyValuePair<string, string>[] parameters)
    {
        return await SendRequest(path, BuildJsonFromParameters(parameters));
    }

    public async Task<JObject> SendRequest(string path, string param = null!)
    {
        string responseContent = null!;

        await _tokenLock.WaitAsync();

        try
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(_cachedToken)) throw new InvalidOperationException("Token is required");

            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path cannot be empty", nameof(path));

            // Create form data
            var formData = new Dictionary<string, string>
            {
                ["svc"] = path,
                ["sid"] = _cachedToken,
                ["params"] = param ?? "{}"
            };

            // Send request
            var response = await _httpClient.PostAsync("", new FormUrlEncodedContent(formData));

            // Check HTTP status
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"HTTP error: {response.StatusCode} for {path}");
                return null;
            }

            // Read and parse response
            responseContent = await response.Content.ReadAsStringAsync();

            var result = JObject.Parse(responseContent);

            // Check for API-level errors
            var apiError = CheckError(result);

            if (!string.IsNullOrWhiteSpace(apiError))
            {
                throw new Exception($"API error in {path}: {apiError}");
            }

            return result;
        }
        catch (JsonReaderException jex) when (responseContent != null)
        {
            _logger.LogError(jex, $"Error in Json Reader ({path}, {param})");

            return HandleJsonParseException(jex, responseContent, path, param);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in SendRequest ({path}, {param})");

            //throw  HandleWialonException($"Request failed: {path}", ex);
            return null;
        }

        finally
        {
            _tokenLock.Release();
        }
    }

    private JObject HandleJsonParseException(JsonReaderException ex, string responseContent, string path, string param)
    {
        try
        {
            // Try parsing as array
            var arr = JArray.Parse(responseContent);
            var result = new JObject();

            for (var i = 0; i < arr.Count; i++)
            {
                result[i.ToString()] = arr[i];
            }

            _logger.LogInformation($"Array response converted to JObject for {path}");
            return result;
        }
        catch
        {
            _logger.LogError(ex, $"JSON parse failed for {path} with param {param}");
            _logger.LogDebug($"Response content: {Truncate(responseContent, 200)}");
            return null;
        }
    }

    private string BuildJsonFromParameters(KeyValuePair<string, string>[] parameters)
    {
        if (parameters == null || parameters.Length == 0)
            return "{}";

        var paramObject = new JObject();
        foreach (var kvp in parameters)
        {
            paramObject[kvp.Key] = kvp.Value;
        }
        return paramObject.ToString(Formatting.None);
    }

    private string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value[..maxLength] + "...";
    }

    private string CheckError(JObject jObject)
    {
        try
        {
            var errorToken = jObject["error"];
            if (errorToken == null) return null;

            return errorToken.Type switch
            {
                JTokenType.Integer => WialonExceptions.GetErrorMsg(errorToken.Value<int>()),
                JTokenType.String => WialonExceptions.GetErrorMsg(Convert.ToInt32(errorToken.Value<string>())),
                _ => $"Unknown error format: {errorToken.ToString()}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking failed");
            return "Error processing error response";
        }
    }


    public async ValueTask DisposeAsync()
    {
        await TryDisconnect();
        _tokenLock.Dispose();
        GC.SuppressFinalize(this);
    }

    public void UpdateToken(string newToken)
    {
        _tokenLock.WaitAsync();

        try
        {
            _cachedToken = newToken;
        }
        finally
        {
            _tokenLock.Release();
        }
    }
}

