using CleanArchitecture.Blazor.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.CheckSession;

public record CheckSessionAsyncCommand : IRequest<bool>;

public class CheckSessionAsyncCommandHandler : IRequestHandler<CheckSessionAsyncCommand, bool>
{
    private readonly IWialonService _wialonService;

    public CheckSessionAsyncCommandHandler(IWialonService wialonService)
    {
        _wialonService = wialonService;
    }

    public Task<bool> Handle(CheckSessionAsyncCommand request, CancellationToken cancellationToken)
    {
        return _wialonService.IsSessionValidAsync();
    }
}