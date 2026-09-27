using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using Mediator;

namespace CleanArchitecture.Blazor.Server.UI.Endpoints;

public static class SmsWebhookEndpoints
{
    public static IEndpointRouteBuilder MapSmsWebhook(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sms/webhook", async (
            HttpRequest req,
            IConfiguration cfg,
            IMediator mediator,
            ILoggerFactory lf) =>
        {
            var log = lf.CreateLogger("SmsWebhook");

            using var reader = new StreamReader(req.Body);
            var raw = await reader.ReadToEndAsync();

            var secret = cfg["SmsGateway:WebhookSecret"];
            if (!string.IsNullOrWhiteSpace(secret))
            {
                var sig = req.Headers["X-Signature"].ToString();
                if (!VerifyHmac(raw, secret, sig))
                {
                    log.LogWarning("Rejected SMS webhook: bad signature");
                    return Results.Unauthorized();
                }
            }

            InboundPayload? payload;
            try
            {
                payload = JsonSerializer.Deserialize<InboundPayload>(
                    raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "Bad SMS webhook payload");
                return Results.BadRequest();
            }

            if (payload is null || string.IsNullOrWhiteSpace(payload.From))
                return Results.BadRequest();

            var result = await mediator.Send(new PersistInboundSmsCommand(
                new InboundSmsDto(payload.Id, payload.From, payload.Body ?? "", payload.Ts)));

            return result.Succeeded
                ? Results.Ok(new { stored = result.Data })
                : Results.StatusCode(500);
        })
        .AllowAnonymous()
        .WithName("SmsWebhook");

        return app;
    }

    private static bool VerifyHmac(string body, string key, string hex)
    {
        if (string.IsNullOrEmpty(hex)) return false;
        using var h = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var computed = Convert.ToHexString(h.ComputeHash(Encoding.UTF8.GetBytes(body)))
                              .ToLowerInvariant();
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computed),
            Encoding.UTF8.GetBytes(hex.ToLowerInvariant()));
    }

    private record InboundPayload(long Id, string From, string? Body, long Ts);
}