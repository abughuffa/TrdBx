using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;

/// <summary>
/// Specification class for filtering Invoices by their ID.
/// </summary>
public class UnBilledCheckInvoiceByCustomerParentSpecification : Specification<ServiceLog>
{
    public UnBilledCheckInvoiceByCustomerParentSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                         && s.ServiceTask == ServiceTask.Check
                         && s.Customer.ParentId == Id);
    }
}

public class UnBilledCheckInvoiceByCustomerChildSpecification : Specification<ServiceLog>
{

    public UnBilledCheckInvoiceByCustomerChildSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                         && s.ServiceTask == ServiceTask.Check
                         && s.CustomerId == Id);
    }
}