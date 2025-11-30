namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.Connect;

public record ConnectWialonCommand : IRequest<bool>
{
}

public class ConnectWialonCommandHandler : IRequestHandler<ConnectWialonCommand, bool>

{
    private readonly IWialonWrapper _wialonWrapper;

    public ConnectWialonCommandHandler(IWialonWrapper wialonWrapper)
    {
        _wialonWrapper = wialonWrapper;
    }
    public async Task<bool> Handle(ConnectWialonCommand request, CancellationToken cancellationToken)
    {
        return await _wialonWrapper.TryConnect();
    }
}

