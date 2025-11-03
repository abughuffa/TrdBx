using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of DeactivateTestCases.
/// </summary>
public class ActivateGprsTestCaseAdvancedSpecification : Specification<ActivateGprsTestCase>
{
    public ActivateGprsTestCaseAdvancedSpecification(ActivateGprsTestCaseAdvancedFilter filter)
    {
        Query.Where(q => q.SNo != null)
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword));

    }
}