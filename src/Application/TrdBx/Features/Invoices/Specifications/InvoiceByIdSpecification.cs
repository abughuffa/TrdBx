using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;

/// <summary>
/// Specification class for filtering Invoices by their ID.
/// </summary>
public class InvoiceByIdSpecification : Specification<Invoice>
{
    public InvoiceByIdSpecification(int id)
    {
        Query.Where(q => q.Id == id);
    }
}
