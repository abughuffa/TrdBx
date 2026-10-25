using System.Text;
using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Common;
public class PriceSharedLogic : SerialForSharedLogic
{
    #region MyRegion


    public static async Task<CPrice>  GetCPrice(IApplicationDbContext cnx, int customerId, int umId)
    {
        // Get the Ccs record
        var cc = await cnx.Customers.SingleOrDefaultAsync(c => c.Id == customerId) ?? throw new Exception("Cc record not found");
        var um = await cnx.TrackingUnitModels.SingleOrDefaultAsync(c => c.Id == umId) ?? throw new Exception("TrackingUnitModel record not found");
        // Determine which Id to use based on the BillingPlan
        var cId = cc.BillingPlan == BillingPlan.Advanced ? (int)cc.ParentId! : cc.Id;

        var cusPrice = await cnx.CusPrices
            .SingleOrDefaultAsync(cp => cp.CustomerId == cId && cp.TrackingUnitModelId == umId);

        if (cusPrice != null)
        {
            return new CPrice
            {
                TrackingUnitModelId = cusPrice.TrackingUnitModelId,
                Price = cusPrice.Price,
                Host = cusPrice.Host,
                Gprs = cusPrice.Gprs
            };
        }
        else
        {
            return new CPrice
            {
                TrackingUnitModelId = um.Id,
                Price = um.DefaultPrice,
                Host = um.DefaultHost,
                Gprs = um.DefaultGprs
            };
        }
    }
    #endregion
}
public partial class SubscriptionSharedLogic : PriceSharedLogic 
{

    internal static string Activate(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)

    {
        //bool applyChangesToDatabaseFlag = applyChangesToDatabase;
        var currentSubscription = unit.Subscriptions?.MaxBy(x => x.Id);
        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 384);
        var r = currentSubscription;
        //var CSeDt = currentSubscription.SeDate;

        int[] osLFPG1 = {490, 426, 470, 406, 457, 393, 458, 394, 453, 389, 454, 390, 461, 397, 462, 398, 384};//	*
        int[] osLFPG2 = {407, 391, 469, 405, 471, 455};//	G									
        int[] osLFPG3 = {427, 395, 489, 425, 491, 459};//	H										
        int[] osLFPG4 = {};//	H+G

        int[] osG1 = {463, 399, 490, 426, 470, 406, 457, 393, 458, 394, 453, 389, 454, 390, 461, 397, 462, 398, 384};//*	*	*	*	*
        int[] osG2 = {471, 455};//Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"												
        int[] osG3 = {491, 459};//Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"												
        int[] osG4 = {469, 405};//Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"										
        int[] osG5 = {407, 391};//Desc10014	G	*	TsDt	"تحديث - قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"										
        int[] osG6 = {489, 425};//Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"										
        int[] osG7 = {427, 395};//Desc10015	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"

        int[] rsLFPG1 = {463,399,490,426,470,406,457,393,458,394,453,389,454,390,461,397,462,398,384}; //	*									
        int[] rsLFPG2 = {407,391,427,395,469,405,489,425,490,426,470,406,457,393,458,394,453,389,454,390,461,397,462,398,384,491,459,471,455}; //	H+G	
													
        int[] rsG1 = {463, 399};//*	*	*	*	*																
        int[] rsG2 = {407, 391, 427, 395};//Desc10021	H+G	TsDt	SeDt	"دورة اشتراك ف3 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"													
        int[] rsG3 = {469, 405, 489, 425, 490, 426, 470,	406,	457,	393,	458	, 394, 453, 389, 454, 390, 461, 397, 462, 398};//Desc10020	H+G	TsDt	LD	"دورة اشتراك ف3 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
        int[] rsG4 = {384};//Desc10009	H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف3 بمدة سنة واحدة من تاريخ التركيب"																	
        int[] rsG5 = {491, 459};//Desc10006	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 السابق"																
        int[] rsG6 = {471, 455};//Desc10005	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 السابق"
                                                                        



        if (APCC.Contains(caseProfile.CaseCode))
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
            // Get the required sub. last paid fees by CaseCode
            var rLF = caseProfile.CaseCode switch
                {
                    var x when rsLFPG1.Contains(x) => SubPackageFees.ZeroFees, //	*
                    var x when rsLFPG2.Contains(x) => SubPackageFees.FullFees,  //	H+G	
                    _ => SubPackageFees.ZeroFees // Default case when none match
                };

            if (applyChangesToDatabase)//ApplyChangesToDatabaseFlag
            {
                if (unit.IsOnWialon == false)
                {
                    //Add operator task to add the unit record to wialon
                    servcieLog.WialonTasks.Add(new WialonTask()
                    {
                        TrackingUnitId = unit.Id,
                        WialonAPIAction = WialonAPIAction.AddToWialon,
                        Description = string.Format("اضف الوحدة ({0}) الى منصة ويلون.", unit.SNo),
                        ExcDate = caseProfile.TsDt,
                        IsExecuted = false,
                    });
                }

                //Create the observed sub. record by CaseCode

                //*	*	*	*	*
                //Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"												
                //Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"												
                //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"										
                //Desc10014	G	*	TsDt	"تحديث - قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"										
                //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"										
                //Desc10015	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
		
                switch (caseProfile.CaseCode)
                {
                    case int code when osG1.Contains(code):
                        {
                            //	NOTHING	*	*	*	*	*
                            break;
                        }
                    case int code when osG2.Contains(code):
                        {
                            //Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"	

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                            //var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10010, startDate, endDate);
                            //currentSubscription.SsDate -- Not Affected here
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                             //Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"	

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10011, startDate, endDate);
                            //currentSubscription.SsDate -- Not Affected here
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int code when osG4.Contains(code):
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
                    case int code when osG5.Contains(code):
                        {
                             //Desc10014	G	*	TsDt	"تحديث - قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"	

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10014, startDate, endDate);
                            //currentSubscription.SsDate -- Not Affected here
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int code when osG6.Contains(code):
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
                    case int code when osG7.Contains(code):
                        {
                             //Desc10015	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10015, startDate, endDate);
                            //currentSubscription.SsDate -- Not Affected here
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                }

                //Create the REQUIRED sub. record by CaseCode

                //*	*	*	*	*																
                //Desc10021	H+G	TsDt	SeDt	"دورة اشتراك ف3 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"													
                //Desc10020	H+G	TsDt	LD	"دورة اشتراك ف3 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                //Desc10009	H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف3 بمدة سنة واحدة من تاريخ التركيب"																	
                //Desc10006	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 السابق"																
                //Desc10005	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 السابق"
          

                switch (caseProfile.CaseCode)
                {
                    case int code when rsG1.Contains(code):
                        {
                             //*	*	*	*	*	
                            break;
                        }
                    case int code when rsG2.Contains(code):
                        {
                            //Desc10021	H+G	TsDt	SeDt	"دورة اشتراك ف3 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"


                            var startDate = caseProfile.TsDt;
                            var endDate =  (DateOnly)caseProfile.SeDt!;
                            var dailyFees = Math.Round(((price.Gprs + price.Host) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10021, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });

                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                            //Desc10020	H+G	TsDt	LD	"دورة اشتراك ف3 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                            
                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10020, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });

                            break;
                        }
                    case int code when rsG4.Contains(code):
                        {
                            //Desc10009	H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف3 بمدة سنة واحدة من تاريخ التركيب"


                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10009, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when rsG5.Contains(code):
                        {
                           //Desc10006	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 السابق"	

                            var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Host) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees) / Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero));

                            var endDate = caseProfile.TsDt.AddDays(subdays);

                            var dailyFees = 0.0m;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10006, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                                
                            });
                            break;
                        }
                    case int code when rsG6.Contains(code):
                        {
                           //Desc10005	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 السابق"
                              var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10005, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                }

                if (unit.WStatus != WStatus.Active)
                {
                    //Add operator task to activate the unit on wialon

                    servcieLog.WialonTasks.Add(new WialonTask()
                    {
                        TrackingUnitId = unit.Id,
                        WialonAPIAction = WialonAPIAction.ActivateOnWialon,
                        Description = string.Format("فعل الوحدة ({0}) على منصة ويلون.", unit.SNo),
                        ExcDate = caseProfile.TsDt,
                        IsExecuted = false,
                    });

                }

                if (servcieLog.Subscriptions!.Count == 384) servcieLog.IsDeserved = false;

                //Update Sub. Statuses
                unit.UStatus = UStatus.InstalledActive;

                return caseProfile.CaseCode.ToString();
            }

            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(string.Format("العملية - {0} - كود الحالة {1}", servcieLog.Description, caseProfile.CaseCode.ToString()));

                if (unit.IsOnWialon == false)
                {
                    stringBuilder.AppendLine(string.Format("اضف الوحدة ({0}) الى منصة ويلون.", unit.SNo));
                }


                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                             //*	*	*	*	*

                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"												

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10010, startDate, endDate));

                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"												
    
                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10011, startDate, endDate));

                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"										

                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10022, startDate, endDate));

                            break;
                        }
                    case int code when osG5.Contains(code):
                        {
                            //Desc10014	G	*	TsDt	"تحديث - قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"										


                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10014, startDate, endDate));



                            break;
                        }
                    case int code when osG6.Contains(code):
                        {
                            //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"										

                            var startDate = caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10023, startDate, endDate));


                            break;
                        }
                    case int code when osG7.Contains(code):
                        {
                            //Desc10015	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10015, startDate, endDate));

                            break;
                        }
                }

                //Create the REQUIRED sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                             //*	*	*	*	*																
               
                            break;
                        }
                    case int code when rsG2.Contains(code):
                        {

                             //Desc10021	H+G	TsDt	SeDt	"دورة اشتراك ف3 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"													

                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Host + price.Gprs) / 365, 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10021, startDate, endDate));

                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                            //Desc10020	H+G	TsDt	LD	"دورة اشتراك ف3 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                
                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10020, startDate, endDate));

                            break;
                        }
                    case int code when rsG4.Contains(code):
                        {
                            //Desc10009	H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف3 بمدة سنة واحدة من تاريخ التركيب"																	
                
                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10009, startDate, endDate));
                            break;
                        }
                    case int code when rsG5.Contains(code):
                        {
                            //Desc10006	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 السابق"																
                
                            var startDate = caseProfile.TsDt;
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;
                            var ldailyFees = Math.Round(((price.Host) / 365), 3, MidpointRounding.AwayFromZero);
                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero)));
                            var endDate = caseProfile.TsDt.AddDays(subdays);    

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10006, startDate, endDate));
                            break;
                        }
                    case int code when rsG6.Contains(code):
                        {
                            //Desc10005	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 السابق"	

                            var startDate = caseProfile.TsDt;
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;
                            var ldailyFees = Math.Round(((price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);
                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero)));
                            var endDate = caseProfile.TsDt.AddDays(subdays);                  
                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10005, startDate, endDate));
                            break;
                        }



                        
                }

                if (unit.WStatus != WStatus.Active)
                {
                    stringBuilder.AppendLine(string.Format("فعل الوحدة ({0}) على منصة ويلون.", unit.SNo));
                }

                //if (servcieLog.Subscriptions.Count == 384) servcieLog.IsDeserved = false;

                return stringBuilder.ToString();
            }


        }
        else
            throw new NotImplementedException($"{caseProfile.CaseCode} Not Implemented Case code");
    }


}