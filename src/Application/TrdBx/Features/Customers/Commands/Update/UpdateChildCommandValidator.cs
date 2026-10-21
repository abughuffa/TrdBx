using CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;

public class UpdateChildCommandValidator : AbstractValidator<UpdateChildCommand>
{
    public UpdateChildCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
        RuleFor(v => v.ParentId).NotNull();
        RuleFor(v => v.UserName).MaximumLength(50);


    }

}

