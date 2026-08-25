using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Documents.DTOs;
using CleanArchitecture.Blazor.Application.Features.Identity.DTOs;
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;
using CleanArchitecture.Blazor.Domain.Identity;
using Mapster;

namespace CleanArchitecture.Blazor.Application.Common.Mappings;

public static partial class MapsterConfiguration
{
    public static TypeAdapterConfig Create()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<Document, DocumentDto>()
            .Map(dest => dest.TenantName, src => src.Tenant == null ? null : src.Tenant.Name);

        config.NewConfig<ApplicationUser, ApplicationUserDto>()
            .Ignore(dest => dest.LocalTimeOffset)
            .Map(dest => dest.AssignedRoles, src => src.UserRoles.Select(role => role.Role.Name).ToArray())
            .Map(dest => dest.Tenants, src => src.TenantUsers.Select(tenantUser => tenantUser.Tenant))
            .Map(dest => dest.Superior, src => src.Superior == null
                ? null
                : new ApplicationUserDto
                {
                    Id = src.Superior.Id,
                    UserName = src.Superior.UserName,
                    DisplayName = src.Superior.DisplayName,
                    Email = src.Superior.Email,
                    PhoneNumber = src.Superior.PhoneNumber,
                    ProfilePictureDataUrl = src.Superior.ProfilePictureDataUrl,
                    IsActive = src.Superior.IsActive,
                    TenantId = src.Superior.TenantId,
                    TimeZoneId = src.Superior.TimeZoneId,
                    LanguageCode = src.Superior.LanguageCode
                });


      config.NewConfig<CusPrice, CusPriceDto>()
            .Map(dest => dest.TrackingUnitModel, src => src.TrackingUnitModel == null ? null : src.TrackingUnitModel.Name)
            .Map(dest => dest.Customer, src => src.Customer == null ? null : src.Customer.Name);

        config.NewConfig<Customer, CustomerDto>()
            .Map(dest => dest.Parent, src => src.Parent == null ? null : src.Parent.Name);

        config.NewConfig<TrackingUnit, TrackingUnitDto>()
            .Map(dest => dest.TrackingUnitModel, src => src.TrackingUnitModel == null ? null : src.TrackingUnitModel.Name)
            .Map(dest => dest.Customer, src => src.Customer == null ? null : src.Customer.Name)
            .Map(dest => dest.TrackedAsset, src => src.TrackedAsset == null ? null : src.TrackedAsset.TrackedAssetNo)
            .Map(dest => dest.SimCard, src => src.SimCard == null ? null : src.SimCard.SimCardNo);

        config.NewConfig<SimCard, SimCardDto>()
            .Map(dest => dest.SPackage, src => src.SPackage == null ? null : src.SPackage.Name);

        config.NewConfig<Ticket, TicketDto>()
            .Map(dest => dest.TrackingUnit, src => src.TrackingUnit == null ? null : src.TrackingUnit.SNo);

        config.NewConfig<ServiceLog, ServiceLogDto>()
            .Map(dest => dest.Customer, src => src.Customer == null ? null : src.Customer.Name);

        config.NewConfig<Subscription, SubscriptionDto>()
            .Map(dest => dest.TrackingUnit, src => src.TrackingUnit == null ? null : src.TrackingUnit.SNo)
            .Map(dest => dest.ServiceLog, src => src.ServiceLog == null ? null : src.ServiceLog.ServiceNo);

       config.NewConfig<WialonTask, WialonTaskDto>()
            .Map(dest => dest.TrackingUnit, src => src.TrackingUnit == null ? null : src.TrackingUnit.SNo)
            .Map(dest => dest.ServiceLog, src => src.ServiceLog == null ? null : src.ServiceLog.ServiceNo);

        config.NewConfig<Invoice, InvoiceDto>()
            .Map(dest => dest.Customer, src => src.Customer == null ? null : src.Customer.Name);


        return config;
    }
}
