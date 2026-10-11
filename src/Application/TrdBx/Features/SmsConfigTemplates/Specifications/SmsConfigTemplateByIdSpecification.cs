using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering SmsConfigTemplates by their ID.
/// </summary>
public class SmsConfigTemplateByIdSpecification : Specification<SmsConfigTemplate>
{
    public SmsConfigTemplateByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}