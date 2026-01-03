using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.Customers.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.SPackages.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.SProviders.Commands.Import;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Commands.Import;

public class ImportDataCommand : IRequest<Result<bool>>
{
    internal string FileName { get; set; }
    internal byte[] Data { get; set; }

    internal IMediator Mediator;
    public ImportDataCommand(string fileName, byte[] data, IMediator mediator)
    {
        FileName = fileName;
        Data = data;
        Mediator = mediator;
    }
}

public class ImportDataCommandHandler :
             IRequestHandler<ImportDataCommand, Result<bool>>
{
    public ImportDataCommandHandler() { }
#nullable disable warnings
    public async Task<Result<bool>> Handle(ImportDataCommand request, CancellationToken cancellationToken)
    {


        var importInstallersCommand = await request.Mediator.Send(new ImportSProvidersCommand(request.FileName, request.Data));
        if (importInstallersCommand.Succeeded)
        {

            var importTrackingUnitModelsCommand = await request.Mediator.Send(new ImportTrackingUnitModelsCommand(request.FileName, request.Data));
            if (importTrackingUnitModelsCommand.Succeeded)
            {
                var importSPackagesCommand = await request.Mediator.Send(new ImportSPackagesCommand(request.FileName, request.Data));
                if (importSPackagesCommand.Succeeded)
                {
                    var importSimCardCardCardsCommand = await request.Mediator.Send(new ImportSimCardsCommand(request.FileName, request.Data));
                    if (importSimCardCardCardsCommand.Succeeded)
                    {
                        var importAssetsCommand = await request.Mediator.Send(new ImportTrackedAssetsCommand(request.FileName, request.Data));
                        if (importAssetsCommand.Succeeded)
                        {
                            var importClientsCommand = await request.Mediator.Send(new ImportCustomersCommand(request.FileName, request.Data));
                            if (importClientsCommand.Succeeded)
                            {
                                var importCusPricesCommand = await request.Mediator.Send(new ImportCusPricesCommand(request.FileName, request.Data));
                                if (importCusPricesCommand.Succeeded)
                                {
                                    var importTrackingUnitsCommand = await request.Mediator.Send(new ImportTrackingUnitsCommand(request.FileName, request.Data));
                                    if (importTrackingUnitsCommand.Succeeded)
                                    {
                                        var importServiceLogsCommand = await request.Mediator.Send(new ImportServiceLogsCommand(request.FileName, request.Data));
                                        if (importServiceLogsCommand.Succeeded)
                                        {
                                            var importSubscriptionsCommand = await request.Mediator.Send(new ImportSubscriptionsCommand(request.FileName, request.Data));
                                            if (importSubscriptionsCommand.Succeeded)
                                            {

                                            var importWialonTasksCommand = await request.Mediator.Send(new ImportWialonTasksCommand(request.FileName, request.Data));
                                            if (importWialonTasksCommand.Succeeded)
                                            {

                                                return await Result<bool>.SuccessAsync(true);

                                            }
                                            else return await Result<bool>.FailureAsync("Faild to Import WialonTasks");

                                        }
                                            else return await Result<bool>.FailureAsync("Faild to Import Subscriptions");
                                        }
                                        else return await Result<bool>.FailureAsync("Faild to Import ServiceLogs");
                                    }
                                    else return await Result<bool>.FailureAsync("Faild to Import TrackingUnits");
                                }
                                else return await Result<bool>.FailureAsync("Faild to Import CusPrices");
                            }
                            else return await Result<bool>.FailureAsync("Faild to Import Ccs");
                        }
                        else return await Result<bool>.FailureAsync("Faild to Import Assets");
                    }
                    else return await Result<bool>.FailureAsync("Faild to Import SimCardCardCards");
                }
                else return await Result<bool>.FailureAsync("Faild to Import SPackages");
            }
            else return await Result<bool>.FailureAsync("Faild to Import TrackingUnitModels");

        }
        else return await Result<bool>.FailureAsync("Faild to Import SProviders");
    }
}

