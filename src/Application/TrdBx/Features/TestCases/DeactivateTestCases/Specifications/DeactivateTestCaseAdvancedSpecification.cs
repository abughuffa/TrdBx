using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of DeactivateTestCases.
/// </summary>
public class DeactivateTestCaseAdvancedSpecification : Specification<DeactivateTestCase>
{
    public DeactivateTestCaseAdvancedSpecification(DeactivateTestCaseAdvancedFilter filter)
    {
        Query.Where(q => q.SNo != null)
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword));
       
    }
}
