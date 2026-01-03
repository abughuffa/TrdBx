using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Commands.Delete;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Commands.Import;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Commands.Import;

public class ImportATestCasesCommand : IRequest<Result<bool>>
{
    internal string FileName { get; set; }
    internal byte[] Data { get; set; }
    internal IMediator Mediator;
    public ImportATestCasesCommand(string fileName, byte[] data, IMediator mediator)
    {
        FileName = fileName;
        Data = data;
        Mediator = mediator;
    }
}

public class ImportATestCasesCommandHandler :
             IRequestHandler<ImportATestCasesCommand, Result<bool>>
{
    public ImportATestCasesCommandHandler() { }
#nullable disable warnings
    public async Task<Result<bool>> Handle(ImportATestCasesCommand request, CancellationToken cancellationToken)
    {

        var deleteXDataCommand = await request.Mediator.Send(new DeleteDataCommand());
        if (deleteXDataCommand.Succeeded == true)
        {

            var importActivateTestCasesCommand = await request.Mediator.Send(new ImportActivateTestCasesCommand(request.FileName, request.Data));
            if (importActivateTestCasesCommand.Succeeded)
            {

                var importDataCommand = await request.Mediator.Send(new ImportDataCommand(request.FileName, request.Data, request.Mediator));

                if (importDataCommand.Succeeded)
                {

                    return await Result<bool>.SuccessAsync(true);

                }

                else return await Result<bool>.FailureAsync("Faild to Import Data");
            }
            else return await Result<bool>.FailureAsync("Faild to Import ActivateTestCases");
        }
        else return await Result<bool>.FailureAsync("Faild to reset database...");


    }

}

