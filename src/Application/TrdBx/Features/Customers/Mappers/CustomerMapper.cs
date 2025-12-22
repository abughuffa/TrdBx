
using CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{

    [MapProperty(
       nameof(Customer.Parent),
       nameof(CustomerDto.Parent),
       Use = nameof(MapParentToParentName)
   )]
    public static partial CustomerDto ToDto(Customer source);
    public static partial Customer FromDto(CustomerDto dto);

    public static partial Customer FromCreateChildCommand(CreateChildCommand command);
    public static partial Customer FromCreateParentCommand(CreateParentCommand command);
    public static partial UpdateChildCommand ToUpdateChildCommand(CustomerDto dto);
    public static partial UpdateParentCommand ToUpdateParentCommand(CustomerDto dto);
    public static partial void ApplyChangesFrom(UpdateChildCommand source, Customer target);
    public static partial void ApplyChangesFrom(UpdateParentCommand source, Customer target);
    public static partial IQueryable<CustomerDto> ProjectTo(this IQueryable<Customer> q);

 private static string? MapParentToParentName(Customer? parent)
    {
        return parent?.Name;
    }
}