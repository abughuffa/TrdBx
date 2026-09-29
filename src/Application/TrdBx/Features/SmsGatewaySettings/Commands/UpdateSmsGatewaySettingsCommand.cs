using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;
using FluentValidation;

namespace CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.Commands;

public class UpdateSmsGatewaySettingsCommand : IRequest<Result<SmsGatewaySettingsDto>>
{
    public string  BaseUrl               { get; set; } = null!;
    public string? ApiKey                { get; set; }
    public bool    EnableInboundPolling  { get; set; }
    public int     PollingIntervalSeconds { get; set; }
    public string? WebhookPublicUrl      { get; set; }
    public string? WebhookSecret         { get; set; }
    public int     TimeoutSeconds        { get; set; }
}

public class UpdateSmsGatewaySettingsCommandValidator
    : AbstractValidator<UpdateSmsGatewaySettingsCommand>
{
    public UpdateSmsGatewaySettingsCommandValidator()
    {
        RuleFor(x => x.BaseUrl)
            .NotEmpty()
            .Must(u => Uri.TryCreate(u, UriKind.Absolute, out var uri)
                    && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("BaseUrl must be an absolute http/https URL.");

        RuleFor(x => x.PollingIntervalSeconds).InclusiveBetween(5, 3600);
        RuleFor(x => x.TimeoutSeconds).InclusiveBetween(1, 120);

        RuleFor(x => x.WebhookPublicUrl)
            .Must(u => string.IsNullOrWhiteSpace(u)
                    || (Uri.TryCreate(u, UriKind.Absolute, out var uri)
                        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)))
            .WithMessage("WebhookPublicUrl must be an absolute http/https URL or empty.");

        RuleFor(x => x.ApiKey).MaximumLength(128);
        RuleFor(x => x.WebhookSecret).MaximumLength(256);
    }
}

public class UpdateSmsGatewaySettingsCommandHandler
    : IRequestHandler<UpdateSmsGatewaySettingsCommand, Result<SmsGatewaySettingsDto>>
{
    private readonly ISmsGatewaySettingsProvider _provider;

    public UpdateSmsGatewaySettingsCommandHandler(
        ISmsGatewaySettingsProvider provider)
    {
        _provider    = provider;
    }

    public async ValueTask<Result<SmsGatewaySettingsDto>> Handle(
        UpdateSmsGatewaySettingsCommand req, CancellationToken ct)
    {
        var current = await _provider.GetAsync(ct);

        var updated = new SmsGatewaySettingsDto
        {
            Id                    = current.Id,
            BaseUrl               = req.BaseUrl.Trim(),
            ApiKey                = string.IsNullOrWhiteSpace(req.ApiKey) ? null : req.ApiKey.Trim(),
            EnableInboundPolling  = req.EnableInboundPolling,
            PollingIntervalSeconds = req.PollingIntervalSeconds,
            WebhookPublicUrl      = string.IsNullOrWhiteSpace(req.WebhookPublicUrl) ? null : req.WebhookPublicUrl.Trim(),
            WebhookSecret         = string.IsNullOrWhiteSpace(req.WebhookSecret) ? null : req.WebhookSecret,
            TimeoutSeconds        = req.TimeoutSeconds,
        };

        await _provider.UpdateAsync(updated, ct);
        return await Result<SmsGatewaySettingsDto>.SuccessAsync(updated);
    }
}