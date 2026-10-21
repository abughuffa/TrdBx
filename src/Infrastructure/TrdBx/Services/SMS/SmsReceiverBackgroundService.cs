// using CleanArchitecture.Blazor.Application.Common.Interfaces;
// using Mediator;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;

// namespace CleanArchitecture.Blazor.Infrastructure.Services;

// public class SmsReceiverBackgroundService : BackgroundService
// {
//     private readonly IServiceScopeFactory _scopeFactory;
//     private readonly ILogger<SmsReceiverBackgroundService> _logger;

//     public SmsReceiverBackgroundService(
//         IServiceScopeFactory scopeFactory,
//         ILogger<SmsReceiverBackgroundService> logger)
//     {
//         _scopeFactory = scopeFactory;
//         _logger = logger;
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         _logger.LogInformation("SMS receiver background service starting.");

//         while (!stoppingToken.IsCancellationRequested)
//         {
//             try
//             {
//                 using var scope = _scopeFactory.CreateScope();
//                 var receiver = scope.ServiceProvider.GetRequiredService<ISmsReceiver>();
//                 var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

//                 await foreach (var incoming in receiver.ListenAsync(stoppingToken))
//                 {
//                     var cmd = new CreateIncomingSmsMessageCommand
//                     {
//                         PhoneNumber = incoming.PhoneNumber,
//                         Message = incoming.Message,
//                         SmsProvider = receiver.SmsProvider,
//                         ProviderMessageId = incoming.ProviderMessageId,
//                         SMSDate = incoming.SMSDate,
//                         PartsCount = incoming.PartsCount,
//                         Encoding = incoming.Encoding
//                     };

//                     var result = await mediator.Send(cmd, stoppingToken);
//                     if (result.Succeeded)
//                         _logger.LogInformation("Stored incoming SMS from {Phone}", incoming.PhoneNumber);
//                     else
//                         _logger.LogWarning("Failed to store incoming SMS.");
//                 }
//             }
//             catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
//             {
//                 break;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "SMS receiver loop crashed; retrying in 10s.");
//                 await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
//             }
//         }

//         _logger.LogInformation("SMS receiver background service stopped.");
//     }
// }