// using System.Runtime.CompilerServices;
// using System.Text.Json;
// using CleanArchitecture.Blazor.Application.Common.Interfaces;
// using CleanArchitecture.Blazor.Domain.Enums;
// using CleanArchitecture.Blazor.Infrastructure.Services.TpLink;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging;

// namespace CleanArchitecture.Blazor.Infrastructure.Services;

// public class TpLinkLteSmsReceiver : ISmsReceiver
// {
//     private readonly TpLinkLteClient _client;
//     private readonly ILogger<TpLinkLteSmsReceiver> _logger;
//     private readonly int _pollIntervalSeconds;
//     private readonly string _username;
//     private readonly string _password;

//     public SmsProvider SmsProvider => SmsProvider.TpLinkLte;

//     public TpLinkLteSmsReceiver(
//         TpLinkLteClient client,
//         IConfiguration configuration,
//         ILogger<TpLinkLteSmsReceiver> logger)
//     {
//         _client = client;
//         _logger = logger;
//         _pollIntervalSeconds = configuration.GetValue<int>(
//             "SmsSettings:TpLinkLte:PollIntervalSeconds", 15);
//         _username = configuration["SmsSettings:TpLinkLte:Username"] ?? "admin";
//         _password = configuration["SmsSettings:TpLinkLte:Password"]!;
//     }

//     public async IAsyncEnumerable<SmsReceiveResult> ListenAsync(
//         [EnumeratorCancellation] CancellationToken cancellationToken)
//     {
//         var loggedIn = await _client.LoginAsync(_username, _password, cancellationToken);
//         if (!loggedIn)
//         {
//             _logger.LogError("TP-Link LTE login failed.");
//             yield break;
//         }

//         var seen = new HashSet<string>();

//         while (!cancellationToken.IsCancellationRequested)
//         {
//             List<SmsReceiveResult> messages = new();
//             try
//             {
//                 // Check unread count.
//                 var countJson = await _client.GetUnreadCountAsync(cancellationToken);
//                 var count = ParseUnreadCount(countJson);

//                 if (count > 0)
//                 {
//                     // Set page to 1.
//                     await _client.SetPageNumberAsync(1, cancellationToken);

//                     // Get entries.
//                     var entriesJson = await _client.GetUnreadEntriesAsync(cancellationToken);
//                     messages = ParseEntries(entriesJson);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogWarning(ex, "Polling TP-Link SMS failed.");
//             }

//             foreach (var m in messages)
//             {
//                 var key = $"{m.PhoneNumber}|{m.Message}|{m.SMSDate:yyyyMMddHHmm}";
//                 if (!seen.Add(key)) continue;

//                 yield return new SmsReceiveResult(
//                     PhoneNumber: m.PhoneNumber,
//                     Message: m.Message,
//                     SMSDate: m.SMSDate,
//                     ProviderMessageId: null,
//                     PartsCount: 1,
//                     Encoding: "GSM7");
//             }

//             await Task.Delay(TimeSpan.FromSeconds(_pollIntervalSeconds), cancellationToken);
//         }
//     }

//     private static int ParseUnreadCount(string json)
//     {
//         try
//         {
//             using var doc = JsonDocument.Parse(json);
//             if (doc.RootElement.TryGetProperty("totalNumber", out var n) &&
//                 int.TryParse(n.GetString(), out var count))
//                 return count;
//         }
//         catch (JsonException) { }
//         return 0;
//     }

//     private static List<SmsReceiveResult> ParseEntries(string json)
//     {
//         var result = new List<SmsReceiveResult>();
//         try
//         {
//             using var doc = JsonDocument.Parse(json);
//             if (doc.RootElement.ValueKind == JsonValueKind.Array)
//             {
//                 foreach (var el in doc.RootElement.EnumerateArray())
//                 {
//                     var phone = el.TryGetProperty("from", out var f) ? f.GetString() ?? "" : "";
//                     var body = el.TryGetProperty("content", out var c) ? c.GetString() ?? "" : "";
//                     var timeStr = el.TryGetProperty("receivedTime", out var t) ? t.GetString() : null;
//                     var receivedAt = DateTime.TryParse(timeStr, out var dt) ? dt : DateTime.UtcNow;

//                     result.Add(new SmsReceiveResult(phone, body, receivedAt));
//                 }
//             }
//         }
//         catch (JsonException) { }
//         return result;
//     }
// }