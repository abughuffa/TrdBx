using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering SmsConfigTemplates by their ID.
/// </summary>
public class SmsConfigTemplateByModelIdSpecification : Specification<SmsConfigTemplate>
{
    public SmsConfigTemplateByModelIdSpecification(int id)
    {
       Query.Where(q => q.TrackingUnitModelId == id);
    }
}