namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Import;

public class ImportServiceLogsCommandValidator : AbstractValidator<ImportServiceLogsCommand>
{
        public ImportServiceLogsCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

