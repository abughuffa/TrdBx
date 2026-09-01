using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Domain.Enums;
public enum InsMode
{
    [Display(Name = "null")] Null = 0,
    [Display(Name = "Basic")] Basic = 1,
    [Display(Name = "Advanced")] Advanced = 2,
    [Display(Name = "Advanced +")] AdvancedPlus = 3,
    [Display(Name = "Advanced ++")] AdvancedPlusPlus = 4
}
public enum WStatus
{
    [Display(Name = "Active")] Active = 1,
    [Display(Name = "Inactive")] Inactive = 2,
    
}
public enum UStatus
{
    [Display(Name = "New")] New = 0 ,
    [Display(Name = "Reserved")] Reserved = 1,
    [Display(Name = "Installed & Active Gprs")] InstalledActiveGprs = 2,
    [Display(Name = "Installed & Active Hosting")] InstalledActiveHosting = 3,
    [Display(Name = "Installed & Active")] InstalledActive = 4,
    [Display(Name = "Installed & Inactive")] InstalledInactive = 5,
    [Display(Name = "Recovered")] Recovered = 6,
    [Display(Name = "Used")] Used = 7,
    [Display(Name = "Damaged")] Damaged = 8,
    [Display(Name = "Lost")] Lost = 9,


}
public enum SubPackageFees
{
    [Display(Name = "Zero Fees")] ZeroFees = 0,
    [Display(Name = "Gprs Fees")] GprsFees = 1,
    [Display(Name = "Host Fees")] HostFees = 2,   
    [Display(Name = "Full Fees")] FullFees = 3
}
public enum SubPackage
{
    [Display(Name = "Active Gprs")] ActiveGprs = 1,
    [Display(Name = "Active Hosting")] ActiveHosting = 2,
    [Display(Name = "Active")] Active = 3,
}
public enum SStatus
{
    [Display(Name = "New")] New = 0,
    [Display(Name = "Installed")] Installed = 1,
    [Display(Name = "Recovered")] Recovered = 2,
    [Display(Name = "Used")] Used = 3,
    [Display(Name = "Lost")] Lost = 4
}

public enum SLStatus
{
    [Display(Name = "Active")] Active = 0,
    [Display(Name = "One-Way Block")] OneWayBlock = 1,
    [Display(Name = "Two-Way Block")] TwoWayBlock = 2,
    [Display(Name = "Frozen Block")] Frozen = 3,
    [Display(Name = "Inactive")] Inactive = 4,

}
public enum TicketStatus
{
    //All = 0,
    [Display(Name = "Opened")] Opened =1,
    [Display(Name = "Accepted")] Accepted =2,
    [Display(Name = "Rejected")] Rejected =3,
    [Display(Name = "Closed")] Closed =4
}

// public enum TicketTask
// {
//     [Display(Name = "Check")] Check = 10, //just created invoice

//     [Display(Name = "Support")] Support_Recover = 20, //invoice in tax process
//     [Display(Name = "Support")] Support_ReInstall = 21, //invoice in tax process
//     [Display(Name = "Support")] Support_Transfer = 22, //invoice in tax process
//     [Display(Name = "Support")] Support_Replace = 23, //invoice in tax process

//     [Display(Name = "Support")] Support_InstallSimCard = 24, //invoice in tax process
//     [Display(Name = "Support")] Support_RecoverSimCard = 25, //invoice in tax process
//     [Display(Name = "Support")] Support_ReplacSimCard = 26, //invoice in tax process

//     [Display(Name = "Install")] Install_New = 31, //invoice retrived from taxes
//     [Display(Name = "Install")] ReInstall_Used = 32, //invoice retrived from taxes

//     [Display(Name = "Renew")] Renew_RenewUnitSub = 41, //invoice sent to customer

//     [Display(Name = "Subscription")] Subscription_ActivateUnit = 51, //invoice paid
//     [Display(Name = "Subscription")] Subscription_ActivateUnitForGprs = 52, //invoice paid
//     [Display(Name = "Subscription")] Subscription_ActivateUnitForHosting = 53, //invoice paid
//     [Display(Name = "Subscription")] Subscription_DeactivateUnit = 54, //invoice paid

// }

public enum ServiceTask
{
    [Display(Name = "Check")] Check = 1,

    [Display(Name = "Install New unit")] Install = 2,
    [Display(Name = "ReInstall Used unit")] ReInstall = 3,
    [Display(Name = "Recover Installed unit")] Recover = 4,
    [Display(Name = "Transfer Installed unit")] Transfer = 5,
    [Display(Name = "Replace Installed unit")] Replace = 6,

    [Display(Name = "Install SimCard card")] InstallSimCard = 7,
    [Display(Name = "Recover SimCard card")] RecoverSimCard = 8,
    [Display(Name = "Replace SimCard card")] ReplacSimCard = 9,

    [Display(Name = "Activate unit's Subscription")] ActivateUnit = 10,
    [Display(Name = "Activate unit's Subscription for GPRS")] ActivateUnitForGprs = 11,
    [Display(Name = "Activate unit's Subscription FOR Hosting")] ActivateUnitForHosting = 12,
    [Display(Name = "Deactivate unit's Subscription")] DeactivateUnit = 13,

    [Display(Name = "Renew unit's Subscription")] RenewUnitSub = 14,
    
    [Display(Name = "UploadedData from TrdBx")] TrdbxDataUpload = 15,
    [Display(Name = "Status Update")] StatusUpdate = 16
}


public enum WialonAPIAction
{
    [Display(Name = "Check on Wialon")] CheckOnWialon = 0,
    [Display(Name = "Add To Wialon")] AddToWialon = 1,
    [Display(Name = "Update On Wialon")] UpdateOnWialon = 2,
    [Display(Name = "Activate On Wialon")] ActivateOnWialon = 3,
    [Display(Name = "Deactivate On Wialon")] DeactivateOnWialon = 4,
    [Display(Name = "Remove From Wialon")] RemoveFromWialon = 5,
}

public enum BillingPlan
{
    
    [Display(Name = "Unkown")] Unkown = 0,
    [Display(Name = "Basic")] Basic = 1,
    [Display(Name = "Advanced")] Advanced = 2
}
public enum IStatus
{
    [Display(Name = "Draft")] Draft = 1, //just created invoice
    [Display(Name = "SentToTax")] SentToTax = 2, //invoice in tax process
    [Display(Name = "Ready")] Ready = 3, //invoice retrived from taxes
    [Display(Name = "Billed")] Billed = 4, //invoice sent to customer
    [Display(Name = "Partailly Paid")] PartaillyPaid = 5, //invoice paid
    [Display(Name = "Paid")] Paid = 6, //invoice paid
    [Display(Name = "Canceled")] Canceled = 7 //invoice canceled
}
public enum InvoiceType
{
    [Display(Name = "Check")] Check = 1, //just created invoice
    [Display(Name = "Support")] Support = 2, //invoice in tax process
    [Display(Name = "Install")] Install = 3, //invoice retrived from taxes
    [Display(Name = "Renew")] Renew = 4, //invoice sent to customer
    [Display(Name = "Subscription")] Subscription = 5, //invoice paid
    [Display(Name = "Replace")] Replace = 6 //invoice canceled
}


