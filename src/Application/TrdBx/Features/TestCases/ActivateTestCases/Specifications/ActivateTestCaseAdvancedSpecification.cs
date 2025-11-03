
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.ActivateTestCases.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of ActivateTestCases.
/// </summary>
public class ActivateTestCaseAdvancedSpecification : Specification<ActivateTestCase>
{
    public ActivateTestCaseAdvancedSpecification(ActivateTestCaseAdvancedFilter filter)
    {
        Query.Where(q => q.SNo != null)
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword));
       
    }
}
