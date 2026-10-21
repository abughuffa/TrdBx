using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;

/// <summary>
/// Specification class for filtering Invoices by their ID.
/// </summary>


public class UnBilledPaymentInvoiceByCustomerParentSpecification : Specification<ServiceLog>
{
    public UnBilledPaymentInvoiceByCustomerParentSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                          && s.ServiceTask == ServiceTask.Install
                          && s.Customer.ParentId == Id);
    }
}

public class UnBilledPaymentInvoiceByCustomerChildSpecification : Specification<ServiceLog>
{
    public UnBilledPaymentInvoiceByCustomerChildSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                         && (s.ServiceTask == ServiceTask.Install)
                         && s.CustomerId == Id);
    }
}