using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Domain;
using MediatR;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.Login;

public record LoginAsyncCommand : IRequest<WialonLoginResult>;

public class LoginAsyncCommandHandler : IRequestHandler<LoginAsyncCommand, WialonLoginResult>
{
    private readonly IWialonService _wialonService;

    public LoginAsyncCommandHandler(IWialonService wialonService)
    {
        _wialonService = wialonService;
    }

    public Task<WialonLoginResult> Handle(LoginAsyncCommand request, CancellationToken cancellationToken)
    {
        return _wialonService.LoginAsync(cancellationToken);
    }
}