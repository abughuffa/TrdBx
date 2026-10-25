using System.Text;
using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Common;

public partial class SubscriptionSharedLogic : PriceSharedLogic 
{

    internal static string ActivateForHosting(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)
    {
        //bool ApplyChangesToDatabaseFlag = applyChangesToDatabase;

        var currentSubscription = unit.Subscriptions?.MaxBy(x => x.Id);

        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 256);


        int[] osLFPG1 = {256, 342, 278, 382, 318, 329, 265, 330, 266, 331, 325, 261, 326, 262, 333, 269, 334, 270};//	*
        int[] osLFPG2 = {343, 327, 279, 263, 341, 277};//	G
        int[] osLFPG3 = {267};//	H
        int[] osLFPG4 = {383, 335, 319, 271, 381, 317};//	H+G

        int[] osG1 = {256, 342, 278, 382, 318, 329, 265, 330, 266, 331, 325, 261, 326, 262, 333, 269, 334, 270};//	*	*	*	*
        int[] osG2 = {343, 327};//Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"
        int[] osG3 = {383, 335};//Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
        int[] osG4 = {279, 263};//Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
        int[] osG5 = {267};//Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
        int[] osG6 = {319, 271};//Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
        int[] osG7 = {341, 277};//Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"
        int[] osG8 = {381, 317};//Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"


        int[] rsLFPG1 = {331};//	*
        int[] rsLFPG2 = {343, 327, 383, 335, 256, 342, 278, 382, 318, 329, 265, 330, 266, 325, 261, 326, 262, 333, 269, 334, 270, 341, 277, 381, 317, 279, 263, 267, 319, 271};//	H

        
        int[] rsG1 = {331};//	*	*	*	*
        int[] rsG2 = {343, 327};//Desc10003	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 السابق"
        int[] rsG3 = {383, 335};//Desc10004	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 السابق"
        int[] rsG4 = {256};//Desc10008	H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف2 بمدة سنة واحدة من تاريخ التركيب"
        int[] rsG5 = {342, 278, 382, 318, 329, 265, 330, 266, 325, 261, 326, 262, 333, 269, 334, 270, 341, 277, 381, 317};//Desc10018	H	TsDt	LD	"دورة اشتراك ف2 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
        int[] rsG6 = {279, 263, 267, 319, 271};//Desc10019	H	TsDt	SeDt	"دورة اشتراك ف2 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"




        if (AHPCC.Contains(caseProfile.CaseCode))
        {
            // Get the observed sub. last paid fees by CaseCode



            var oLF = caseProfile.CaseCode switch
            {
                var x when osLFPG1.Contains(x) => SubPackageFees.ZeroFees, //	*
                var x when osLFPG2.Contains(x) => SubPackageFees.GprsFees, //	G	
                var x when osLFPG3.Contains(x) => SubPackageFees.GprsFees, //	H
                var x when osLFPG4.Contains(x) => SubPackageFees.FullFees, //	H+G
                _ => SubPackageFees.ZeroFees // Default case when none match
            };

            // Get the required sub. last paid fees by CaseCode
            var rLF = caseProfile.CaseCode switch
            {
                var x when rsLFPG1.Contains(x) => SubPackageFees.ZeroFees, //	*
                var x when rsLFPG2.Contains(x) => SubPackageFees.HostFees,  //	H	
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

                //	*	*	*	*
                //Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"
                //Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
                //Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
                //Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
                //Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
                //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"
                //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
        
                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10010, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));

                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                            //Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host + price.Gprs) / 365, 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10012, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int x when osG4.Contains(x):
                        {
                             //Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10013, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int x when osG5.Contains(x):
                        {
                            //Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);
                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10014, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int x when osG6.Contains(x):
                        {
                            //Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10015, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int x when osG7.Contains(x):
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
                    case int x when osG8.Contains(x):
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

                //Create the REQUIRED sub. record by CaseCode

                //	*	*	*	*
                //Desc10003	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 السابق"
                //Desc10004	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 السابق"
                //Desc10008	H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف2 بمدة سنة واحدة من تاريخ التركيب"
                //Desc10018	H	TsDt	LD	"دورة اشتراك ف2 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                //Desc10019	H	TsDt	SeDt	"دورة اشتراك ف2 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"

                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*	
                            break;
                        }
                    case int x when rsG2.Contains(x):
                        {
                            //Desc10003	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 السابق"
                            
                            var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;

                            
                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10003, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when rsG3.Contains(x):
                        {
                            //Desc10004	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 السابق"
                            var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Gprs + price.Host) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,

                                Description = string.Format(SubscriptionDescs.Desc10004, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when rsG4.Contains(x):
                        {
                            //Desc10008	H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف2 بمدة سنة واحدة من تاريخ التركيب"
                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,

                                Description = string.Format(SubscriptionDescs.Desc10008, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when rsG5.Contains(x):
                        {
                            //Desc10018	H	TsDt	LD	"دورة اشتراك ف2 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,

                                Description = string.Format(SubscriptionDescs.Desc10018, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when rsG6.Contains(x):
                        {
                            //Desc10019	H	TsDt	SeDt	"دورة اشتراك ف2 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"
                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt!;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,

                                Description = string.Format(SubscriptionDescs.Desc10019, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                }

                //Add operator task to deactivate the unit on wialon

                //	Deactivate
                if (unit.WStatus != WStatus.Inactive)
                            {
                                //Add operator task to Activate unit on wialon
                                servcieLog.WialonTasks.Add(new WialonTask()
                                {
                                    TrackingUnitId = unit.Id,
                                    WialonAPIAction = WialonAPIAction.DeactivateOnWialon,
                                    Description = string.Format("إلغاء تفعيل الوحدة ({0}) على منصة ويلون.", unit.SNo),
                                    ExcDate = caseProfile.TsDt,
                                    IsExecuted = false,
                                });

                            }



                //TODO: Subscriptions.Count SHOULD BE (Subscriptions.where(s => s.TrackingUnitId = Unit.Id) == 256) 
                if (servcieLog.Subscriptions!.Count == 256) servcieLog.IsDeserved = false;



                //Update Sub. Statuses
                unit.UStatus = UStatus.InstalledActiveHosting;

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

                //Create the observed sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                                            //	*	*	*	*
                
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
                    case int x when osG3.Contains(x):
                        {
                                 //Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
                
                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host + price.Gprs) / 365, 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10012, startDate, endDate));

                            break;
                        }
                    case int x when osG4.Contains(x):
                        {
                            //Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
            
                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10013, startDate, endDate));

                            break;
                        }
                    case int x when osG5.Contains(x):
                        {
                            //Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
               

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10014, startDate, endDate));

                            break;
                        }
                    case int x when osG6.Contains(x):
                        {
                             //Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
                
                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10015, startDate, endDate));

                            break;
                        }
                    case int x when osG7.Contains(x):
                        {
                            //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10022, startDate, endDate));

                            break;
                        }
                        case int x when osG8.Contains(x):
                        {
                            //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
                
                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            
                            var dailyFees = Math.Round((price.Host + price.Gprs) / 365, 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10024, startDate, endDate));

                            break;
                        }

                        
                }

                //Create the REQUIRED sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                            //	*	*	*	*

                            break;
                        }
                    case int x when rsG2.Contains(x):
                        {
                            //Desc10003	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 السابق"

                           var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            //var dailyFees = 0.0m;

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10003, startDate, endDate));

                            break;
                        }
                    case int x when rsG3.Contains(x):
                        {
                            //Desc10004	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 السابق"
                
                             var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Gprs + price.Host) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            //var dailyFees = 0.0m;

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10004, startDate, endDate));

                            break;
                        }
                    case int x when rsG4.Contains(x):
                        {
                            //Desc10008	H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف2 بمدة سنة واحدة من تاريخ التركيب"

                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10008, startDate, endDate));

                            break;
                        }

                                        

                   case int x when rsG5.Contains(x):
                        {
                            //Desc10018	H	TsDt	LD	"دورة اشتراك ف2 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10018, startDate, endDate));

                            break;
                        }

                    case int x when rsG6.Contains(x):
                        {
                            
                           //Desc10019	H	TsDt	SeDt	"دورة اشتراك ف2 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt!;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10019, startDate, endDate));

                            break;
                        }
                }

                //Add operator task to deactivate the unit on wialon

       if (unit.WStatus != WStatus.Inactive)
                            {
                                stringBuilder.AppendLine(string.Format("إلغاء تفعيل الوحدة ({0}) على منصة ويلون.", unit.SNo));
                            }

                //if (servcieLog.Subscriptions.Count == 256) servcieLog.IsDeserved = false;

                return stringBuilder.ToString();
            }

        }
        else
            throw new NotImplementedException($"{caseProfile.CaseCode.ToString()} Not Implemented Case code");
    }


}

