using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Tickets by their ID.
/// </summary>
public class TicketByIdSpecification : Specification<Ticket>
{
    public TicketByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}