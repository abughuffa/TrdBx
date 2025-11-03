using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;

/// <summary>
/// Specification class for filtering Invoices by their ID.
/// </summary>



public class UnBilledSupportInvoiceByCustomerParentSpecification : Specification<ServiceLog>
{
    public UnBilledSupportInvoiceByCustomerParentSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                        && (s.ServiceTask == ServiceTask.Recover
                             || s.ServiceTask == ServiceTask.ReInstall
                             || s.ServiceTask == ServiceTask.Transfer
                             || s.ServiceTask == ServiceTask.InstallSimCard
                             || s.ServiceTask == ServiceTask.RecoverSimCard
                             || s.ServiceTask == ServiceTask.ReplacSimCard)
                         && s.Customer.ParentId == Id);
    }
}

public class UnBilledSupportInvoiceByCustomerChildSpecification : Specification<ServiceLog>
{
    public UnBilledSupportInvoiceByCustomerChildSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                        && (s.ServiceTask == ServiceTask.Recover
                             || s.ServiceTask == ServiceTask.ReInstall
                             || s.ServiceTask == ServiceTask.Transfer
                             || s.ServiceTask == ServiceTask.InstallSimCard
                             || s.ServiceTask == ServiceTask.RecoverSimCard
                             || s.ServiceTask == ServiceTask.ReplacSimCard)
                         && s.CustomerId == Id);
    }
}