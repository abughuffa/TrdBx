


//using CleanArchitecture.Blazor.Application.Features.TestCases.Commands;

//namespace CleanArchitecture.Blazor.Application.Features.TestCases.RenewTestCases.Commands.Import;

//public class ImportRTestCasesCommand : IRequest<Result<bool>>
//{
//    internal string FileName { get; set; }
//    internal byte[] Data { get; set; }
//    internal IMediator Mediator;
//    public ImportRTestCasesCommand(string fileName, byte[] data, IMediator mediator)
//    {
//        FileName = fileName;
//        Data = data;
//        Mediator = mediator;
//    }
//}

//public class ImportRTestCasesCommandHandler :
//             IRequestHandler<ImportRTestCasesCommand, Result<bool>>
//{
//    public ImportRTestCasesCommandHandler() { }
//#nullable disable warnings
//    public async Task<Result<bool>> Handle(ImportRTestCasesCommand request, CancellationToken cancellationToken)
//    {

//        var deleteXDataCommand = await request.Mediator.Send(new DeleteAllDataCommand());
//        if (deleteXDataCommand.Succeeded == true)
//        {

//            var importRenewTestCasesCommand = await request.Mediator.Send(new ImportRenewTestCasesCommand(request.FileName, request.Data));
//            if (importRenewTestCasesCommand.Succeeded)
//            {

//                var importDataCommand = await request.Mediator.Send(new ImportDataCommand(request.FileName, request.Data, request.Mediator));

//                if (importDataCommand.Succeeded)
//                {

//                    return await Result<bool>.SuccessAsync(true);

//                }

//                else return await Result<bool>.FailureAsync("Faild to Import Data");
//            }
//            else return await Result<bool>.FailureAsync("Faild to Import RenewTestCases");
//        }
//        else return await Result<bool>.FailureAsync("Faild to reset database...");


//    }

//}

