using System.ComponentModel;

namespace CleanArchitecture.Blazor.Domain.Enums;
public enum InsMode
{
    [Description("null")] Null = 0,
    [Description("Basic")] Basic = 1,
    [Description("Advanced")] Advanced = 2,
    [Description("Advanced +")] AdvancedPlus = 3,
    [Description("Advanced ++")] AdvancedPlusPlus = 4
}
public enum WStatus
{
    [Description("Null")] Null = 0,
    [Description("Active")] Active = 1,
    [Description("Inactive")] Inactive = 2,
    [Description("All")] All = 999,
}
public enum UStatus
{
    [Description("New")] New = 0 ,
    [Description("Reserved")] Reserved = 1,
    [Description("Installed & Active Gprs")] InstalledActiveGprs = 2,
    [Description("Installed & Active Hosting")] InstalledActiveHosting = 3,
    [Description("Installed & Active")] InstalledActive = 4,
    [Description("Installed & Inactive")] InstalledInactive = 5,
    [Description("Recovered")] Recovered = 6,
    [Description("Used")] Used = 7,
    [Description("Damaged")] Damaged = 8,
    [Description("Lost")] Lost = 9,
    [Description("Null")] Null = 99,
    [Description("All")] All = 999,

}
public enum SubPackageFees
{
    [Description("Zero Fees")] ZeroFees = 0,
    [Description("Gprs Fees")] GprsFees = 1,
    [Description("Host Fees")] HostFees = 2,   
    [Description("Full Fees")] FullFees = 3
}
public enum SubPackage
{
    [Description("Active Gprs")] ActiveGprs = 1,
    [Description("Active Hosting")] ActiveHosting = 2,
    [Description("Active")] Active = 3,
}
public enum SStatus
{
    [Description("New")] New = 0,
    [Description("Installed")] Installed = 1,
    [Description("Recovered")] Recovered = 2,
    [Description("Used")] Used = 3,
    [Description("Lost")] Lost = 4
}

public enum SLStatus
{
    [Description("Active")] Active = 0,
    [Description("One-Way Block")] OneWayBlock = 1,
    [Description("Two-Way Block")] TwoWayBlock = 2,
    [Description("Frozen Block")] Frozen = 3,
    [Description("Null")] Null = 99,
    [Description("All")] All = 999

}
//public enum AStatus
//{
//    [Description("Just Added")] JustAdded = 0,
//    [Description("One TrackingUnit Installed")] OneTrackingUnitInstalled = 1,
//    [Description("Tow TrackingUnits Installed")] TowTrackingUnitsInstalled = 2,
//    [Description("TrackingUnit Recovered")] TrackingUnitRecovered = 3
//}
public enum ServiceTask
{
    [Description("All")] All = 0,
    [Description("Check")] Check = 1,
    [Description("Install New unit")] Install = 2,
    [Description("ReInstall Used unit")] ReInstall = 3,
    [Description("Recover Installed unit")] Recover = 4,
    [Description("Transfer Installed unit")] Transfer = 5,
    [Description("Replace Installed unit")] Replace = 6,
    [Description("Install SimCard card")] InstallSimCard = 7,
    [Description("Recover SimCard card")] RecoverSimCard = 8,
    [Description("Replace SimCard card")] ReplacSimCard = 9,
    [Description("Activate unit's Subscription")] ActivateUnit = 10,
    [Description("Activate unit's Subscription for GPRS")] ActivateUnitForGprs = 11,
    [Description("Activate unit's Subscription FOR Hosting")] ActivateUnitForHosting = 12,
    [Description("Deactivate unit's Subscription")] DeactivateUnit = 13,
    [Description("Renew unit's Subscription")] RenewUnitSub = 14,
    [Description("UploadedData from TrdBx")] TrdbxDataUpload = 15
}
public enum APITask
{
    [Description("Add To Wialon")] AddToWialon = 1,
    [Description("Update On Wialon")] UpdateOnWialon = 2,
    [Description("Activate On Wialon")] ActivateOnWialon = 3,
    [Description("Deactivate On Wialon")] DeactivateOnWialon = 4,
    [Description("Remove From Wialon")] RemoveFromWialon = 5
    //[Description("Deactivate On Wialon Defered")] DeactivateOnWialonDefered,
    //[Description("Remove From Wialon Defered")] RemoveFromWialonDefered,
    //[Description("Invoice Of Subscription Fees")] InvoiceOfSubscriptionFees,
    //[Description("Invoice Of Partial Subscription Fees")] InvoiceOfPartialSubscriptionFees,
}
public enum AssignedTo
{
    [Description("Null")] Null = 0,
    [Description("Client")] Client = 1,
    [Description("Customer")] Customer = 2
}
public enum BillingPlan
{
    
    [Description("Unkown")] Unkown = 0,
    [Description("Basic")] Basic = 1,
    [Description("Advanced")] Advanced = 2
}
public enum IStatus
{
    [Description("All")] All = 6, //just created invoice
    [Description("Draft")] Draft = 0, //just created invoice
    [Description("SentToTax")] SentToTax = 1, //invoice in tax process
    [Description("Ready")] Ready = 2, //invoice retrived from taxes
    [Description("Billed")] Billed = 3, //invoice sent to customer
    [Description("Paid")] Paid = 4, //invoice paid
    [Description("Canceled")] Canceled = 5 //invoice canceled
}
public enum InvoiceType
{
    [Description("All")] All = 0, //just created invoice
    [Description("Check")] Check = 1, //just created invoice
    [Description("Support")] Support = 2, //invoice in tax process
    [Description("Install")] Install = 3, //invoice retrived from taxes
    [Description("Renew")] Renew = 4, //invoice sent to customer
    [Description("Subscription")] Subscription = 5, //invoice paid
    [Description("Replace")] Replace = 6 //invoice canceled
}


