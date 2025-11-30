namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Import;

public class ImportWialonTasksCommandValidator : AbstractValidator<ImportWialonTasksCommand>
{
        public ImportWialonTasksCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

