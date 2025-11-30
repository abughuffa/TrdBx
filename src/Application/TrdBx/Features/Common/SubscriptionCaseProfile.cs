using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Common;
internal class SubscriptionCaseProfile
{


    internal SubscriptionCaseProfile(UStatus us, Subscription? subscription, DateOnly TsDate, int CodeIncrement)
    {
        TsDt = TsDate;
        if (subscription != null)
        {
            SeDt = subscription.SeDate;
            LPF = subscription.LastPaidFees;
            IsBilled = subscription.ServiceLog.IsBilled;   /// TODO: YOU HAVE TO BIND SERVICELOG.ISBILLED VALUE HERE

            CaseCode = CodeIncrement + (IsBilled is null ? 0 : (bool)IsBilled ? 64 : 0) +
                                        (us == UStatus.InstalledActive ? 48 : us == UStatus.InstalledActiveHosting ? 32 : us == UStatus.InstalledActiveGprs ? 16 : us == UStatus.InstalledInactive ? 0 : 0) +
                                        (LPF == SubPackageFees.FullFees ? 12 : LPF == SubPackageFees.HostFees ? 8 : LPF == SubPackageFees.GprsFees ? 4 : 0) +
                                        (SeDt < TsDt ? 1 : SeDt == TsDt ? 2 : SeDt > TsDt ? 3 : 0);
        }
        else
            CaseCode = CodeIncrement;
    }

    internal int CaseCode { get; private set; } = 0;
    internal DateOnly TsDt { get; private set; }
    internal DateOnly? SeDt { get; private set; } = null;
    internal SubPackageFees? LPF { get; private set; } = null;
    internal bool? IsBilled { get; private set; } = null;

}
