using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;

/// <summary>
/// Specification class for filtering Invoices by their ID.
/// </summary>


public class UnBilledRenewInvoiceByCustomerParentSpecification : Specification<ServiceLog>
{
    public UnBilledRenewInvoiceByCustomerParentSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                          && s.ServiceTask == ServiceTask.RenewUnitSub
                         && s.Customer.ParentId == Id);
    }
}

public class UnBilledRenewInvoiceByCustomerChildSpecification : Specification<ServiceLog>
{
    public UnBilledRenewInvoiceByCustomerChildSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                        && s.ServiceTask == ServiceTask.RenewUnitSub
                         && s.CustomerId == Id);
    }
}