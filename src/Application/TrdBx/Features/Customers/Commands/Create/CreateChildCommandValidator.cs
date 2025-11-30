namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;

public class CreateChildCommandValidator : AbstractValidator<CreateChildCommand>
{
    public CreateChildCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
        RuleFor(v => v.UserName).MaximumLength(50).NotEmpty();
        RuleFor(v => v.ParentId).NotNull();
        RuleFor(v => v.UserName).MaximumLength(50);

    }

}

