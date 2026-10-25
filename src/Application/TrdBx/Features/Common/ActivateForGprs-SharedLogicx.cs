using System.Text;
using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Common;

public partial class SubscriptionSharedLogic : PriceSharedLogic 
{
    internal static string ActivateForGprs(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)
    {
        //bool ApplyChangesToDatabaseFlag = applyChangesToDatabase;

        var currentSubscription = unit.Subscriptions?.MaxBy(x => x.Id);

        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 128);

        int[] osG1 = {128, 234, 170, 254, 190, 201, 137, 202, 138, 197, 133, 198, 134, 205, 141, 206, 142, 199};//*	*	*	*	*
        int[] osG2 = {255, 207};//Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
        int[] osG3 = {235, 203};//Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"
        int[] osG4 = {135};//Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
        int[] osG5 = {171, 139};//Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
        int[] osG6 = {191, 143};//Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
        int[] osG7 = {233, 169};//Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
        int[] osG8 = {253, 189};//Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"

        int[] rsG1 = {199};//*	*	*	*	*
        int[] rsG2 = {235, 203};//Desc10001	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 السابق"
        int[] rsG3 = {255, 207};//Desc10002	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 السابق"
        int[] rsG4 = {128};//Desc10007	G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف1 بمدة سنة واحدة من تاريخ التركيب"
        int[] rsG5 = {234, 170, 254, 190, 201, 137, 202, 138, 197, 133, 198, 134, 205, 141, 206, 142, 233, 169, 253, 189};//Desc10016	G	TsDt	LD	"دورة اشتراك ف1 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
        int[] rsG6 = {135, 171, 139, 191, 143};//Desc10017	G	TsDt	SeD	"دورة اشتراك ف1 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"


        int[] osLFPG1 = {128, 234, 170, 254, 190, 201, 137, 202, 138, 197, 133, 198, 134, 205, 141, 206, 142, 199};//	*
        int[] osLFPG2 = {135};//	G
        int[] osLFPG3 = {235, 203, 171, 139, 233, 169};//	H
        int[] osLFPG4 = {255, 207, 191, 143, 253, 189};//	H+G

        int[] rsLFPG1 = {199};//	*
        int[] rsLFPG2 = {235, 203, 255, 207, 128, 234, 170, 254, 190, 201, 137, 202, 138, 197, 133, 198, 134, 205, 141, 206, 142, 233, 169, 253, 189, 135, 171, 139, 191, 143};//	G

        var r = currentSubscription;

        if (AGPCC.Contains(caseProfile.CaseCode))
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

                var x when rsLFPG2.Contains(x) => SubPackageFees.GprsFees,  //	G	
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
                //Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
                //Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"
                //Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
                //Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
                //Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
                //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
                //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            // * ****
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                             //Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10012, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                            //Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Description = string.Format(SubscriptionDescs.Desc10011, startDate, endDate);
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
                            var dailyFees = Math.Round((price.Host + price.Gprs) / 365, 3, MidpointRounding.AwayFromZero);

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
                            //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt!;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host/ 365), 3, MidpointRounding.AwayFromZero);

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

                //*	*	*	*	*
                //Desc10001	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 السابق"
                //Desc10002	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 السابق"
                //Desc10007	G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف1 بمدة سنة واحدة من تاريخ التركيب"
                //Desc10016	G	TsDt	LD	"دورة اشتراك ف1 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                //Desc10017	G	TsDt	SeD	"دورة اشتراك ف1 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"


                switch (caseProfile.CaseCode)
                {
                    case int code when rsG1.Contains(code):
                        {
                            //	*	*	*	*	*	
                            break;
                        }
                    case int code when rsG2.Contains(code):
                        {
                            //Desc10001	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 السابق"

                                                        
                            var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Host) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;



                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10001, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                             //Desc10002	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 السابق"

                            var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;

                            var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Description = string.Format(SubscriptionDescs.Desc10002, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when rsG4.Contains(code):
                        {
                            //Desc10007	G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف1 بمدة سنة واحدة من تاريخ التركيب"

                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                            //var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                //Desc = "دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة",
                                Description = string.Format(SubscriptionDescs.Desc10007, startDate, endDate),
                                //Desc = string.Format("{2} للفترة من {0} حتى {1}.", startDate, endDate, "دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when rsG5.Contains(code):
                        {
                            //Desc10016	G	TsDt	LD	"دورة اشتراك ف1 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"

                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                            //var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                //Desc = "دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة",
                                Description = string.Format(SubscriptionDescs.Desc10016, startDate, endDate),
                                //Desc = string.Format("{2} للفترة من {0} حتى {1}.", startDate, endDate, "دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when rsG6.Contains(code):
                        {
                           //Desc10017	G	TsDt	SeD	"دورة اشتراك ف1 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt!;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                            //var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                //Desc = "دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة",
                                Description = string.Format(SubscriptionDescs.Desc10017, startDate, endDate),
                                //Desc = string.Format("{2} للفترة من {0} حتى {1}.", startDate, endDate, "دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                }

                //Add operator task to activate the unit on wialon

                if (unit.WStatus != WStatus.Active)
                {
                    //Add operator task to Activate unit on wialon
                    servcieLog.WialonTasks.Add(new WialonTask()
                    {
                        TrackingUnitId = unit.Id,
                        WialonAPIAction = WialonAPIAction.ActivateOnWialon,
                        Description = string.Format("فعل الوحدة ({0}) على منصة ويلون.", unit.SNo),
                        ExcDate = caseProfile.TsDt,
                        IsExecuted = false,
                    });

                }

                if (servcieLog.Subscriptions?.Count == 128) servcieLog.IsDeserved = false;

                //Update Sub. Statuses
                unit.UStatus = UStatus.InstalledActiveGprs;

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
                             //*	*	*	*	*
          
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                              //Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"

                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs + price.Host) / 365, 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10012, startDate, endDate));

                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                                        //Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"
     
                            var startDate = currentSubscription!.SsDate;
                            var endDate = caseProfile.TsDt;
                              var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10011, startDate, endDate));

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
                                            //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
              
                           var startDate = caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10023, startDate, endDate));

                            break;
                        }
                    case int x when osG8.Contains(x):
                        {
                                             //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
              
                            var startDate = caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10024, startDate, endDate));

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
                            //Desc10001	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 السابق"
           
                           var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Host) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                           // var dailyFees = 0.0m;

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10001, startDate, endDate));

                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                                //Desc10002	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 السابق"

                           var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt!).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Host+price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            //var dailyFees = 0.0m;


                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10002, startDate, endDate));

                            break;
                        }
                    case int code when rsG4.Contains(code):
                        {
                            //Desc10007	G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف1 بمدة سنة واحدة من تاريخ التركيب"
 
                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10007, startDate, endDate));

                            break;
                        }
                    case int code when rsG5.Contains(code):
                        {
                            //Desc10016	G	TsDt	LD	"دورة اشتراك ف1 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
               
                            var startDate = caseProfile.TsDt;
                             var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10016, startDate, endDate));

                            break;
                        }
                    case int code when rsG6.Contains(code):
                        {
                           //Desc10017	G	TsDt	SeD	"دورة اشتراك ف1 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"	
                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt!;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10017, startDate, endDate));

                            break;
                        }


                }

                //Add operator task to activate the unit on wialon

                if (unit.WStatus != WStatus.Active)
                {
                    stringBuilder.AppendLine(string.Format("فعل الوحدة ({0}) على منصة ويلون.", unit.SNo));

                }

                //if (servcieLog.Subscriptions.Count == 128) servcieLog.IsDeserved = false;

                return stringBuilder.ToString();
            }

        }
        else
            throw new NotImplementedException($"{caseProfile.CaseCode} Not Implemented Case code");
    }

}
