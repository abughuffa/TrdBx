namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Commands.Delete;

public class DeleteSmsConfigTemplateCommandValidator : AbstractValidator<DeleteSmsConfigTemplateCommand>
{
        public DeleteSmsConfigTemplateCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

