using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Infrastructure.Services.TpLink;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public class TpLinkLteSmsSender : ISmsSender
{
    private readonly TpLinkLteClient _client;
    private readonly ILogger<TpLinkLteSmsSender> _logger;
    private readonly string _username;
    private readonly string _password;

    public SmsProvider SmsProvider => SmsProvider.TpLinkLte;

    public TpLinkLteSmsSender(
        TpLinkLteClient client,
        IConfiguration configuration,
        ILogger<TpLinkLteSmsSender> logger)
    {
        _client = client;
        _logger = logger;
        _username = configuration["SmsSettings:TpLinkLte:Username"] ?? "admin";
        _password = configuration["SmsSettings:TpLinkLte:Password"]!;
    }

    public async Task<SmsSendResult> SendAsync(
        string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.LoginAsync(_username, _password, cancellationToken);
            // TODO: Implement send using LTE_SMS_SENDNEWMSG action
            // var result = await _client.ExecuteActionAsync(ACT_SET, "LTE_SMS_SENDNEWMSG",
            //     new Dictionary<string, string> { ["phoneNumber"] = phoneNumber, ["message"] = message },
            //     cancellationToken);
            return new SmsSendResult(Success: false, ErrorMessage: "Send not yet implemented.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TP-Link SMS send failed");
            return new SmsSendResult(Success: false, ErrorMessage: ex.Message);
        }
    }
}