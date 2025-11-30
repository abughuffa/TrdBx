using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;

/// <summary>
/// Specification class for filtering Invoices by their ID.
/// </summary>
public class UnBilledSubscriptionInvoiceByCustomerParentSpecification : Specification<ServiceLog>
{
    public UnBilledSubscriptionInvoiceByCustomerParentSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                         && (s.ServiceTask == ServiceTask.ActivateUnit
                             || s.ServiceTask == ServiceTask.ActivateUnitForGprs
                             || s.ServiceTask == ServiceTask.ActivateUnitForHosting
                             || s.ServiceTask == ServiceTask.DeactivateUnit)
                         && s.Customer.ParentId == Id);
    }
}

public class UnBilledSubscriptionInvoiceByCustomerChildSpecification : Specification<ServiceLog>
{
    public UnBilledSubscriptionInvoiceByCustomerChildSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                         && (s.ServiceTask == ServiceTask.ActivateUnit
                             || s.ServiceTask == ServiceTask.ActivateUnitForGprs
                             || s.ServiceTask == ServiceTask.ActivateUnitForHosting
                             || s.ServiceTask == ServiceTask.DeactivateUnit)
                         && s.CustomerId == Id);
    }
}




