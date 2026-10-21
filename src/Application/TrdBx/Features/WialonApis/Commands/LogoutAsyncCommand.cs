using CleanArchitecture.Blazor.Application.Common.Interfaces;
// using MediatR;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.Logout;

public record LogoutAsyncCommand : IRequest<Result<bool>>;

public class LogoutAsyncCommandHandler : IRequestHandler<LogoutAsyncCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;

    public LogoutAsyncCommandHandler(IWialonService wialonService)
    {
        _wialonService = wialonService;
    }

    public async ValueTask<Result<bool>> Handle(LogoutAsyncCommand request, CancellationToken cancellationToken)
    {
        return await Result<bool>.SuccessAsync(await _wialonService.LogoutAsync(cancellationToken));

    }
}