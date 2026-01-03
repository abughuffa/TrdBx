using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Commands.Delete;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Commands.Import;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Commands.Import;

public class ImportAGTestCasesCommand : IRequest<Result<bool>>
{
    internal string FileName { get; set; }
    internal byte[] Data { get; set; }
    internal IMediator Mediator;
    public ImportAGTestCasesCommand(string fileName, byte[] data, IMediator mediator)
    {
        FileName = fileName;
        Data = data;
        Mediator = mediator;
    }
}

public class ImportAGTestCasesCommandHandler :
             IRequestHandler<ImportAGTestCasesCommand, Result<bool>>
{
    public ImportAGTestCasesCommandHandler() { }
#nullable disable warnings
    public async Task<Result<bool>> Handle(ImportAGTestCasesCommand request, CancellationToken cancellationToken)
    {

        var deleteXDataCommand = await request.Mediator.Send(new DeleteDataCommand());
        if (deleteXDataCommand.Succeeded == true)
        {

            var importActivateGprsTestCasesCommand = await request.Mediator.Send(new ImportActivateGprsTestCasesCommand(request.FileName, request.Data));
            if (importActivateGprsTestCasesCommand.Succeeded)
            {

                var importDataCommand = await request.Mediator.Send(new ImportDataCommand(request.FileName, request.Data, request.Mediator));

                if (importDataCommand.Succeeded)
                {

                    return await Result<bool>.SuccessAsync(true);

                }

                else return await Result<bool>.FailureAsync("Faild to Import Data");
            }
            else return await Result<bool>.FailureAsync("Faild to Import ActivateGprsTestCases");
        }
        else return await Result<bool>.FailureAsync("Faild to reset database...");


    }

}

