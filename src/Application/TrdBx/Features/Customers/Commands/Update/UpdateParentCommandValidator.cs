namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;

public class UpdateParentCommandValidator : AbstractValidator<UpdateParentCommand>
{
    public UpdateParentCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();

        RuleFor(v => v.UserName).MaximumLength(50);


    }

}

