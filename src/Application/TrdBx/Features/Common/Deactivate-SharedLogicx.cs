using System.Text;
using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Common;
public partial class SubscriptionSharedLogic : PriceSharedLogic 
{
     internal static string Deactivate(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)
    {

        //bool ApplyChangesToDatabaseFlag = applyChangesToDatabase;

        var currentSubscription = unit.Subscriptions?.MaxBy(x => x.Id);

        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 0);

        int[] osG1 = {106, 42, 107, 43, 86, 22, 87, 23, 126, 62, 127, 63};//*	*	*	*	*
        int[] osG2 = {85, 21};//Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"
        int[] osG3 = {105, 41};//Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
        int[] osG4 = {125, 61};//Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
        

        int[] osLFPG1 = {106, 42, 107, 43, 86, 22, 87, 23, 126, 62, 127, 63};//	*
        int[] osLFPG2 = {85, 21};//	G
        int[] osLFPG3 = {105, 41};//	H
        int[] osLFPG4 = {125, 61};//	H+G

        if (DPCC.Contains(caseProfile.CaseCode))
        {
            // Get the observed sub. last paid fees by CaseCode
            var oLF = caseProfile.CaseCode switch
            {
                var x when osLFPG1.Contains(x) => SubPackageFees.ZeroFees, //	*
                var x when osLFPG2.Contains(x) => SubPackageFees.GprsFees, //	G	
                var x when osLFPG3.Contains(x) => SubPackageFees.HostFees, //	H
                var x when osLFPG4.Contains(x) => SubPackageFees.FullFees, //	H+G
                _ => SubPackageFees.ZeroFees // Default case when none match

            };

            if (applyChangesToDatabase)//ApplyChangesToDatabaseFlag
            {
                //Create the observed sub. record by CaseCode

                //*	*	*	*	*
                //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"
                //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
                //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"

                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //*	*	*	*	*
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"
                            
                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10022, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
                            
                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10023, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees


                            });
                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10024, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                }

               //RemoveFromWialon
               if ((bool)unit.IsOnWialon)
                            {
                                servcieLog.WialonTasks.Add(new WialonTask()
                                {
                                    TrackingUnitId = unit.Id,
                                    WialonAPIAction = WialonAPIAction.RemoveFromWialon,
                                    Description = string.Format("حذف الوحدة ({0}) من منصة ويلون.", unit.SNo),
                                    ExcDate = caseProfile.TsDt,
                                    IsExecuted = false,
                                });


                            }

  

                servcieLog.IsDeserved = servcieLog.Subscriptions?.Count > 0;
                //Update Sub. Statuses
                unit.UStatus = UStatus.InstalledInactive;

                return caseProfile.CaseCode.ToString();
            }

            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(string.Format("العملية - {0} - كود الحالة {1}", servcieLog.Description, caseProfile.CaseCode.ToString()));

                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //*	*	*	*	*

                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                           //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"


                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10022, startDate, endDate));

                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
   
                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10023, startDate, endDate));

                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10024, startDate, endDate));

                            break;
                        }
                }

                if ((bool)unit.IsOnWialon)
                            {
                               stringBuilder.AppendLine(string.Format("حذف الوحدة ({0}) من منصة ويلون.", unit.SNo));

                            }

                //if (servcieLog.Subscriptions.Count == 0) servcieLog.IsDeserved = false;

                return stringBuilder.ToString();
            }


        }
        else
            throw new NotImplementedException($"{caseProfile.CaseCode} Not Implemented Case code");




    }

}
