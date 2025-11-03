using CleanArchitecture.Blazor.Application.Features.TestCases.Commands;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Commands.Import;

public class ImportAHTestCasesCommand : IRequest<Result<bool>>
{
    internal string FileName { get; set; }
    internal byte[] Data { get; set; }
    internal IMediator Mediator;
    public ImportAHTestCasesCommand(string fileName, byte[] data, IMediator mediator)
    {
        FileName = fileName;
        Data = data;
        Mediator = mediator;
    }
}

public class ImportAHTestCasesCommandHandler :
             IRequestHandler<ImportAHTestCasesCommand, Result<bool>>
{
    public ImportAHTestCasesCommandHandler() { }
#nullable disable warnings
    public async Task<Result<bool>> Handle(ImportAHTestCasesCommand request, CancellationToken cancellationToken)
    {

        var deleteXDataCommand = await request.Mediator.Send(new DeleteAllDataCommand());
        if (deleteXDataCommand.Succeeded == true)
        {

            var importActivateHostingTestCasesCommand = await request.Mediator.Send(new ImportActivateHostingTestCasesCommand(request.FileName, request.Data));
            if (importActivateHostingTestCasesCommand.Succeeded)
            {

                var importDataCommand = await request.Mediator.Send(new ImportDataCommand(request.FileName, request.Data, request.Mediator));

                if (importDataCommand.Succeeded)
                {

                    return await Result<bool>.SuccessAsync(true);

                }

                else return await Result<bool>.FailureAsync("Faild to Import Data");
            }
            else return await Result<bool>.FailureAsync("Faild to Import ActivateHostingTestCases");
        }
        else return await Result<bool>.FailureAsync("Faild to reset database...");


    }

}

