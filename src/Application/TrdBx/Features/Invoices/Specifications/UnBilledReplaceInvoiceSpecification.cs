using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
/// <summary>
/// Specification class for filtering Invoices by their ID.
/// </summary>



public class UnBilledReplaceInvoiceByCustomerParentSpecification : Specification<ServiceLog>
{
    public UnBilledReplaceInvoiceByCustomerParentSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                         && s.ServiceTask == ServiceTask.Replace
                         && s.Customer.ParentId == Id);
    }
}

public class UnBilledReplaceInvoiceByCustomerChildSpecification : Specification<ServiceLog>
{
    public UnBilledReplaceInvoiceByCustomerChildSpecification(int Id)
    {
        Query.Where(s => s.IsDeserved && !s.IsBilled
                        && s.ServiceTask == ServiceTask.Replace
                         && s.CustomerId == Id);
    }
}