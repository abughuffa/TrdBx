using CleanArchitecture.Blazor.Application.Common.Interfaces;
// using MediatR;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.CheckSession;

public record CheckSessionAsyncCommand : IRequest<Result<bool>>;

public class CheckSessionAsyncCommandHandler : IRequestHandler<CheckSessionAsyncCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;

    public CheckSessionAsyncCommandHandler(IWialonService wialonService)
    {
        _wialonService = wialonService;
    }

    public async ValueTask<Result<bool>> Handle(CheckSessionAsyncCommand request, CancellationToken cancellationToken)
    {
       
        var x = await _wialonService.IsSessionValidAsync();

        return await Result<bool>.SuccessAsync(x);
    }
}