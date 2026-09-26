using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering SmsMessages by their ID.
/// </summary>
public class SmsMessageByIdSpecification : Specification<SmsMessage>
{
    public SmsMessageByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}