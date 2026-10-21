using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.Queries;

public record GetSmsGatewaySettingsQuery : IRequest<Result<SmsGatewaySettingsDto>>;

public class GetSmsGatewaySettingsQueryHandler
    : IRequestHandler<GetSmsGatewaySettingsQuery, Result<SmsGatewaySettingsDto>>
{
    private readonly ISmsGatewaySettingsProvider _provider;

    public GetSmsGatewaySettingsQueryHandler(ISmsGatewaySettingsProvider provider)
        => _provider = provider;

    public async ValueTask<Result<SmsGatewaySettingsDto>> Handle(
        GetSmsGatewaySettingsQuery req, CancellationToken ct)
    {
        var dto = await _provider.GetAsync(ct);
        return await Result<SmsGatewaySettingsDto>.SuccessAsync(dto);
    }
}