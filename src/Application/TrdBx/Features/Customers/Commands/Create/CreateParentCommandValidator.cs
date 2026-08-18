namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;

public class CreateParentCommandValidator : AbstractValidator<CreateParentCommand>
{
    public CreateParentCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
        RuleFor(v => v.Account).MaximumLength(50).NotEmpty();
        RuleFor(v => v.UserName).MaximumLength(50);

    }

}

