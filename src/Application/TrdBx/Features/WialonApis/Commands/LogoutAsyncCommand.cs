using CleanArchitecture.Blazor.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.Logout;

public record LogoutAsyncCommand : IRequest<bool>;

public class LogoutAsyncCommandHandler : IRequestHandler<LogoutAsyncCommand, bool>
{
    private readonly IWialonService _wialonService;

    public LogoutAsyncCommandHandler(IWialonService wialonService)
    {
        _wialonService = wialonService;
    }

    public Task<bool> Handle(LogoutAsyncCommand request, CancellationToken cancellationToken)
    {
        return _wialonService.LogoutAsync(cancellationToken);
    }
}