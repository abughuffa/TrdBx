using CleanArchitecture.Blazor.Application.Features.TestCases.Commands;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Commands.Import;

public class ImportDTestCasesCommand : IRequest<Result<bool>>
{
    internal string FileName { get; set; }
    internal byte[] Data { get; set; }

    internal IMediator Mediator;
    public ImportDTestCasesCommand(string fileName, byte[] data, IMediator mediator)
    {
        FileName = fileName;
        Data = data;
        Mediator = mediator;
    }
}

public class ImportDTestCasesCommandHandler :
             IRequestHandler<ImportDTestCasesCommand, Result<bool>>
{
    public ImportDTestCasesCommandHandler() { }
#nullable disable warnings
    public async Task<Result<bool>> Handle(ImportDTestCasesCommand request, CancellationToken cancellationToken)
    {

        var deleteXDataCommand = await request.Mediator.Send(new DeleteAllDataCommand());
        if (deleteXDataCommand.Succeeded == true)
        {

            var importDeactivateTestCasesCommand = await request.Mediator.Send(new ImportDeactivateTestCasesCommand(request.FileName, request.Data));
            if (importDeactivateTestCasesCommand.Succeeded)
            {

                var importDataCommand = await request.Mediator.Send(new ImportDataCommand(request.FileName, request.Data, request.Mediator));

                if (importDataCommand.Succeeded)
                {

                    return await Result<bool>.SuccessAsync(true);

                }

                else return await Result<bool>.FailureAsync("Faild to Import Data");
            }
            else return await Result<bool>.FailureAsync("Faild to Import DeactivateTestCases");
        }
        else return await Result<bool>.FailureAsync("Faild to reset database...");


    }

}

