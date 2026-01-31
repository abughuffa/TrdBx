using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Create;

public class CreateTicketCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int[] Id { get; }
    [Description("ServiceTask")] public ServiceTask ServiceTask { get; set; } = ServiceTask.Check;
    [Description("TcDate")] public DateOnly TcDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public CreateTicketCommand(int[] id)
    {
        Id = id;
    }

    public string CacheKey => TicketCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => TicketCacheKey.Tags;




    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<CreateTicketCommand, Ticket>(MemberList.None);
    //    }
    //}
}

public class CreateTicketCommandHandler : SerialForSharedLogic, IRequestHandler<CreateTicketCommand, Result<int>>
{

    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public CreateTicketCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public CreateTicketCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var incompletedTickets = await _context.Tickets.Where(x => request.Id.Contains(x.TrackingUnitId) && x.TicketStatus != TicketStatus.Closed).ToListAsync(cancellationToken);

        if (incompletedTickets.Any())
        {
            return await Result<int>.FailureAsync("There is a Tracking Unit has incompleted Tickets.");
        }

        var desc = string.Empty;;
        var ticketNo = GenSerialNo(_context, "Ticket", request.TcDate).Result;

        var items = await _context.TrackingUnits.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);



        switch (request.ServiceTask)
        {
            case ServiceTask.Check:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive || u.UStatus == UStatus.InstalledInactive))
                    {
                        return await Result<int>.FailureAsync("All Tracking Unit status should be Installed to Check it.");
                    }

                    desc = "كشف على الوحدة ({0}).";
                    break;
                }
            case ServiceTask.Install:
                {
                    if (!items.Any(u => u.UStatus == UStatus.New || u.UStatus == UStatus.Reserved))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit status is Not statisfy Install Process!.");
                    }
                    desc = "تركيب الوحدة ({0}).";
                    break;
                }
            case ServiceTask.ReInstall:
                {
                    if (!items.Any(u => u.UStatus == UStatus.Used))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit status is Not statisfy ReInstall Process!.");
                    }
                    desc = "إعادة تركيب الوحدة ({0}).";
                    break;
                }

            case ServiceTask.Transfer:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive || u.UStatus == UStatus.InstalledInactive))
                    {
                        return await Result<int>.FailureAsync("All Tracking Unit status should be Installed to Transfer it.");
                    }
                    desc = "نقل الوحدة ({0}).";
                    break;
                }

            case ServiceTask.Replace:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive || u.UStatus == UStatus.InstalledInactive))
                    {
                        return await Result<int>.FailureAsync("All Tracking Unit status should be Installed to Replace it.");
                    }
                    desc = "استبدال الوحدة ({0}).";
                    break;
                }

            case ServiceTask.Recover:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive || u.UStatus == UStatus.InstalledInactive))
                    {
                        return await Result<int>.FailureAsync("All Tracking Unit status should be Installed to Recover it.");
                    }
                    desc = "استرجاع الوحدة ({0}).";
                    break;
                }

            case ServiceTask.ActivateUnit:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledInactive))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit status is Not statisfy Activate Process!.");
                    }
                    desc = "تفعيل الوحدة ({0}).";
                    break;
                }

            case ServiceTask.ActivateUnitForGprs:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActive || u.UStatus == UStatus.InstalledInactive))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit status is Not statisfy ActivateUnitForGprs Process!.");
                    }
                    desc = "تفعيل الوحدة ({0}) للتمديد.";
                    break;
                }

            case ServiceTask.ActivateUnitForHosting:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive || u.UStatus == UStatus.InstalledInactive))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit status is Not statisfy ActivateUnitForHosting Process!.");
                    }
                    desc = "تفعيل الوحدة ({0}) للإستضافة.";
                    break;
                }

            case ServiceTask.DeactivateUnit:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit status is Not statisfy Deactivate Process!.");
                    }
                    desc = "إلغاء تفعيل الوحدة ({0}).";
                    break;
                }

            case ServiceTask.RenewUnitSub:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit status is Not statisfy Renew Process!.");
                    }
                    desc = "تجديد اشتراك الوحدة ({0}).";
                    break;
                }

            case ServiceTask.ReplacSimCard:
                {
                    if (!items.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit status is Not statisfy ReplacSimCard Process!.");
                    }
                    desc = "استبدال شريحة الوحدة ({0}).";
                    break;
                }

            case ServiceTask.InstallSimCard:
                {
                    if (!items.Any(u => u.SimCardId is null))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit has sim Assigned!.");
                    }
                    desc = "تركيب شريحة للوحدة ({0}).";
                    break;
                }

            case ServiceTask.RecoverSimCard:
                {
                    if (items.Any(u => u.SimCardId is null))
                    {
                        return await Result<int>.FailureAsync("One of the selected Tracking Unit not has sim Assigned!.");
                    }
                    desc = "استرجاع شريحة من الوحدة ({0}).";
                    break;
                }
        }


        foreach (var item in items)
        {
            var ticket = new Ticket()
            {
                TrackingUnitId = item.Id,
                Desc = string.Format(desc, item.SNo),
                TicketNo = ticketNo,
                ServiceTask = request.ServiceTask,
                TicketStatus = TicketStatus.Opened,
                TcDate = request.TcDate,

            };

            ticket.AddDomainEvent(new TicketCreatedEvent(ticket));
            _context.Tickets.Add(ticket);

            var match = Regex.Match(ticketNo, @"^(\d{6}-)(\d{3})$");
            var prefix = match.Groups[1].Value;
            var sequence = int.Parse(match.Groups[2].Value);
            ticketNo = $"{prefix}{sequence + 1:D3}";

        }



        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result<int>.SuccessAsync(result);
        }
        else
            return await Result<int>.FailureAsync("Ticket creation Faild!");

    }
}



