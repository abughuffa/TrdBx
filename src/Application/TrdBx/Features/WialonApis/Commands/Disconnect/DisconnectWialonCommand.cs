namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.Disconnect;

public record DisconnectWialonCommand : IRequest<bool>
{
}

public class DisconnectWialonCommandHandler : IRequestHandler<DisconnectWialonCommand, bool>

{
    private readonly IWialonWrapper _wialonWrapper;

    public DisconnectWialonCommandHandler(IWialonWrapper wialonWrapper)
    {
        _wialonWrapper = wialonWrapper;
    }
    public async Task<bool> Handle(DisconnectWialonCommand request, CancellationToken cancellationToken)
    {
        return await _wialonWrapper.TryDisconnect();
    }
}

