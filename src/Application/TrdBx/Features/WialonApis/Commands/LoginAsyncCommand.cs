using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Domain;
// using MediatR;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.Login;

public record LoginAsyncCommand : IRequest<Result<WialonLoginResult>>;

public class LoginAsyncCommandHandler : IRequestHandler<LoginAsyncCommand, Result<WialonLoginResult>>
{
    private readonly IWialonService _wialonService;

    public LoginAsyncCommandHandler(IWialonService wialonService)
    {
        _wialonService = wialonService;
    }

    public async ValueTask<Result<WialonLoginResult>> Handle(LoginAsyncCommand request, CancellationToken cancellationToken)
    {
        var x = await _wialonService.LoginAsync(cancellationToken);

        return await Result<WialonLoginResult>.SuccessAsync(x);
    }
}