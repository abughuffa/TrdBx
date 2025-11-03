using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of ActivateHostingTestCases.
/// </summary>
public class ActivateHostingTestCaseAdvancedSpecification : Specification<ActivateHostingTestCase>
{
    public ActivateHostingTestCaseAdvancedSpecification(ActivateHostingTestCaseAdvancedFilter filter)
    {
        Query.Where(q => q.SNo != null)
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword));
       
    }
}
