//using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
//using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;



//namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Assign;

//public class AssignTicketCommand : ICacheInvalidatorRequest<Result<int>>
//{
//    [Description("Id")] public int Id { get; set; }
//    [Description("TaDate")] public DateOnly TaDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
//    //[Description("InstallerId")] public string InstallerId { get; set; } = string.Empty;

//     public IEnumerable<string> Tags => TicketCacheKey.Tags;
//    //private class Mapping : Profile
//    //{
//    //    public Mapping()
//    //    {
//    //        CreateMap<TicketDto, AssignTicketCommand>(MemberList.None)
//    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
//    //            .ForMember(dest => dest.TaDate, opt => opt.Ignore())
//    //            .ForMember(dest => dest.InstallerId, opt => opt.Ignore());
//    //    }
//    //}
//}
//public class AssignTicketCommandHandler : IRequestHandler<AssignTicketCommand, Result<int>>
//{
//    //private readonly IApplicationDbContextFactory _dbContextFactory;
//    //public AssignTicketCommandHandler(
//    //    IApplicationDbContextFactory dbContextFactory
//    //)
//    //{
//    //    _dbContextFactory = dbContextFactory;
//    //}

//    private readonly IApplicationDbContext _context;
//    public AssignTicketCommandHandler(
//        IApplicationDbContext context
//    )
//    {
//        _context = context;
//    }
//    public async Task<Result<int>> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
//    {

//        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
//        var ticket = await _context.Tickets.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");

//        if (!(ticket.TicketStatus == TicketStatus.Approved || ticket.TicketStatus == TicketStatus.Released))
//        {
//            return await Result<int>.FailureAsync("Ticket Status should be Approved Or Released to Assign it.");
//        }

//        ticket.TicketStatus = TicketStatus.Assigned;
//        ticket.TaDate = request.TaDate;
//        //ticket.InstallerId = request.InstallerId;
//        ticket.Note = string.Empty;




//        ticket.AddDomainEvent(new TicketUpdatedEvent(ticket));

//        var result = await _context.SaveChangesAsync(cancellationToken);

//        if (result > 0)
//        {
//            return await Result<int>.SuccessAsync(ticket.Id);
//        }
//        else
//            return await Result<int>.FailureAsync("Ticket Assignment Faild!");

//    }
//}

