using CleanArchitecture.Blazor.Application.Features.SmsMessages.Caching;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.Delete;

public class DeleteSmsMessageCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => SmsMessageCacheKey.Tags;
    public DeleteSmsMessageCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteSmsMessageCommandHandler :
             IRequestHandler<DeleteSmsMessageCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    public DeleteSmsMessageCommandHandler(
       IApplicationDbContextFactory dbContextFactory
    )
    {
       _dbContextFactory = dbContextFactory;
       //_mapper = mapper;
    }

    public async ValueTask<Result> Handle(DeleteSmsMessageCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await context.SmsMessages.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
           // raise a delete domain event
           item.AddDomainEvent(new SmsMessageDeletedEvent(item));
           context.SmsMessages.Remove(item);
        }
        await context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

