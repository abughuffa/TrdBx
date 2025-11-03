namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Update;

public class UpdateServiceLogCommandValidator : AbstractValidator<UpdateServiceLogCommand>
{
        public UpdateServiceLogCommandValidator()
        {
           RuleFor(v => v.Id).NotNull();
             
    RuleFor(v => v.Desc).MaximumLength(255).NotEmpty();

          
        }
    
}

