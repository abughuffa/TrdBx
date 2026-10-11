using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Commands.Delete;

public class DeleteSmsConfigTemplateCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => SmsConfigTemplateCacheKey.Tags;
    public DeleteSmsConfigTemplateCommand(int[] id)
    {
        Id = id;
    }

}

public class DeleteSmsConfigTemplateCommandHandler : IRequestHandler<DeleteSmsConfigTemplateCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    public DeleteSmsConfigTemplateCommandHandler(
       IApplicationDbContextFactory dbContextFactory
    )
    {
       _dbContextFactory = dbContextFactory;
       //_mapper = mapper;
    }

    // private readonly IApplicationDbContext _context;
    // public DeleteSmsConfigTemplateCommandHandler(
    //     IApplicationDbContext context
    // )
    // {
    //     _context = context;
    // }
    public async ValueTask<Result> Handle(DeleteSmsConfigTemplateCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await context.SmsConfigTemplates.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
           // raise a delete domain event
           item.AddDomainEvent(new SmsConfigTemplateDeletedEvent(item));
           context.SmsConfigTemplates.Remove(item);
        }
        await context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();


    }

}

