using System.Text;
using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Blazor.Application.Features.Common;
public class PriceSharedLogic : SerialForSharedLogic
{
    #region MyRegion
    public static async Task<decimal>  GetSPrice(IApplicationDbContext cnx, ServiceTask serviceTask)
    {
        // Get the Ccs record
        var sp = await cnx.ServicePrices.SingleOrDefaultAsync(c => c.ServiceTask == serviceTask);
        return sp == null ? 0.0m : sp.Price;
    }

    //public static List<CPrice> GetCPrices(IApplicationDbContext cnx, int customerId)
    //{
    //    // Get the Ccs record
    //    var cc = cnx.Customers.SingleOrDefault(c => c.Id == customerId) ?? throw new Exception("Customer record not found");
    //    // Determine which Id to use based on the BillingPlan
    //    var ccId = cc.BillingPlan == BillingPlan.Advanced ? (int)cc.ParentId : cc.Id;
    //    // Get the GPS unit models
    //    var gpsUnitModels = cnx.TrackingUnitModels.ToList();
    //    // Prepare the result list
    //    var result = new List<CPrice>();

    //    foreach (var gpsUnitModel in gpsUnitModels)
    //    {
    //        var cusPrice = cnx.CusPrices
    //            .SingleOrDefault(cp => cp.CustomerId == ccId && cp.TrackingUnitModelId == gpsUnitModel.Id);

    //        if (cusPrice != null)
    //        {
    //            result.Add(new CPrice
    //            {
    //                TrackingUnitModelId = gpsUnitModel.Id,
    //                Price = cusPrice.Price,
    //                Host = cusPrice.Host,
    //                Gprs = cusPrice.Gprs
    //            });
    //        }
    //        else
    //        {
    //            result.Add(new CPrice
    //            {
    //                TrackingUnitModelId = gpsUnitModel.Id,
    //                Price = gpsUnitModel.DefualtPrice,
    //                Host = gpsUnitModel.DefualtHost,
    //                Gprs = gpsUnitModel.DefualtGprs
    //            });
    //        }
    //    }

    //    return result;
    //}

    public static async Task<CPrice>  GetCPrice(IApplicationDbContext cnx, int customerId, int umId)
    {
        // Get the Ccs record
        var cc = await cnx.Customers.SingleOrDefaultAsync(c => c.Id == customerId) ?? throw new Exception("Cc record not found");
        var um = await cnx.TrackingUnitModels.SingleOrDefaultAsync(c => c.Id == umId) ?? throw new Exception("TrackingUnitModel record not found");
        // Determine which Id to use based on the BillingPlan
        var _customerId = cc.BillingPlan == BillingPlan.Advanced ? (int)cc.ParentId : cc.Id;

        var cusPrice = await cnx.CusPrices
            .SingleOrDefaultAsync(cp => cp.CustomerId == _customerId && cp.TrackingUnitModelId == umId);

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
                Price = um.DefualtPrice,
                Host = um.DefualtHost,
                Gprs = um.DefualtGprs
            };
        }
    }
    #endregion
}
public class SubscriptionSharedLogic : PriceSharedLogic 
{
    #region
    private static readonly int[] APCC = { 384, 489, 425, 490, 426, 491, 427, 469, 405, 470, 406, 471, 407, 493, 429, 494, 430, 495, 431, 477, 413, 478, 414, 479, 415, 457, 393, 458, 394, 459, 395, 453, 389, 454, 390, 455, 391, 461, 397, 462, 398, 463, 399 }; //	All			
    private static readonly int[] AHPCC = { 256, 341, 277, 342, 278, 343, 279, 381, 317, 382, 318, 383, 319, 349, 285, 350, 286, 351, 287, 329, 265, 330, 266, 331, 267, 325, 261, 326, 262, 327, 263, 333, 269, 334, 270, 335, 271 };//All					
    private static readonly int[] AGPCC = { 128, 233, 169, 234, 170, 235, 171, 253, 189, 254, 190, 255, 191, 237, 173, 238, 174, 239, 175, 201, 137, 202, 138, 203, 139, 197, 133, 198, 134, 199, 135, 205, 141, 206, 142, 207, 143 };//	All					
    private static readonly int[] DPCC = { 105, 41, 106, 42, 107, 43, 85, 21, 86, 22, 87, 23, 125, 61, 126, 62, 127, 63, 109, 45, 110, 46, 111, 47, 93, 29, 94, 30, 95, 31 };
    //public SubscriptionServices() { }
    internal static string Activate(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)

    {
        //bool applyChangesToDatabaseFlag = applyChangesToDatabase;
        var currentSubscription = unit.Subscriptions?.OrderBy(x => x.Id).LastOrDefault();
        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 384);
        var r = currentSubscription;

        int[] osLFPG1 = { 384, 490, 426, 470, 406, 494, 430, 495, 431, 478, 414, 479, 415, 457, 393, 458, 394, 453, 389, 454, 390, 461, 397, 462, 398, 463, 399 };//	*					
        int[] osLFPG2 = { 405, 469, 407, 477, 413, 391 }; //	G					
        int[] osLFPG3 = { 489, 425, 427, 493, 429, 395 }; //	H					
        int[] osLFPG4 = { 491, 471, 459, 455 }; //	H+G					

        int[] rsLFPG1 = { 491, 471, 495, 431, 479, 415, 459, 455, 463, 399 }; //	*					
        int[] rsLFPG2 = { 384, 489, 425, 490, 426, 427, 469, 405, 470, 406, 407, 493, 429, 494, 430, 477, 413, 478, 414, 457, 393, 458, 394, 395, 453, 389, 454, 390, 391, 461, 397, 462, 398 }; //	H+G					

        //int[] tG1 = { 384, 489, 425, 490, 426, 491, 427, 469, 405, 470, 406, 471, 407, 493, 429, 494, 430, 495, 431, 477, 413, 478, 414, 479, 415, 457, 393, 458, 394, 459, 395, 453, 389, 454, 390, 455, 391, 461, 397, 462, 398, 463, 399 }; //	Activate					

        int[] osG1 = { 384, 490, 426, 470, 406, 494, 430, 495, 431, 478, 414, 479, 415, 457, 393, 458, 394, 453, 389, 454, 390, 461, 397, 462, 398, 463, 399 }; //	NOTHING	*	*	*	*	*
        int[] osG2 = { 407, 391 }; //	UPDATE	OSLFP	G	*	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة"
        int[] osG3 = { 427, 395 }; //	UPDATE	OSLFP	H	*	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة"
        int[] osG4 = { 469, 405, 477, 413 }; //	CREATE	OSLFP	G	SeDt	TsDt	"قيمة اشتراك تمديد مستحقة عن الدورة السابقة"
        int[] osG5 = { 489, 425, 493, 429 }; //	CREATE	OSLFP	H	SeDt	TsDt	"قيمة اشتراك استضافة مستحقة عن الدورة السابقة"
        int[] osG6 = { 491, 459 }; //	CREATE	OSLFP	G	TsDt	SeDt	"تغيير طبيعة الاشتراك السابق من اشتراك للاستضافة إلى اشتراك كامل"
        int[] osG7 = { 471, 455 };//	CREATE	OSLFP	H	TsDt	SeDt	"تغيير طبيعة الاشتراك السابق من اشتراك للتمديد إلى اشتراك كامل"

        int[] rsG1 = { 491, 471, 495, 431, 479, 415, 459, 455, 463, 399 }; //	CREATE	*	*	*	*	*
        int[] rsG2 = { 384 }; //	CREATE	RSLFP	H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك كامل بمدة سنة واحدة من تاريخ التركيب"
        int[] rsG3 = { 489, 425, 490, 426, 469, 405, 470, 406, 493, 429, 494, 430, 477, 413, 478, 414, 457, 393, 458, 394, 453, 389, 454, 390, 461, 397, 462, 398 }; //	CREATE	RSLFP	H+G	TsDt	LD	"دورة اشتراك كامل من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
        int[] rsG4 = { 427, 407, 395, 391 }; //	CREATE	RSLFP	H+G	TsDt	SeDt	"دورة اشتراك كامل من تاريخ التفعيل حتى نهاية الدورة السابقة الغير مسددة"

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
                        APITask = APITask.AddToWialon,
                        Desc = string.Format("اضف الوحدة ({0}) الى منصة ويلون.", unit.SNo),
                        ExcDate = caseProfile.TsDt,
                        IsExecuted = false,
                    });
                }

                //Create the observed sub. record by CaseCode
                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //	UPDATE	OSLFP	G	*	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة حتى تاريخ التفعيل"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                            //var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Desc = string.Format(SubscriptionDescs.Desc20, startDate, endDate);
                            //currentSubscription.SsDate -- Not Affected here
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //	UPDATE	OSLFP	H	*	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة حتى تاريخ التفعيل"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Desc = string.Format(SubscriptionDescs.Desc16, startDate, endDate);
                            //currentSubscription.SsDate -- Not Affected here
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //CREATE OSLFP   G SeDt    TsDt    "قيمة اشتراك تمديد مستحقة عن الدورة السابقة حتى تاريخ التفعيل"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);


                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc18, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });

                            break;
                        }
                    case int code when osG5.Contains(code):
                        {
                            //CREATE OSLFP   H SeDt    TsDt    "قيمة اشتراك استضافة مستحقة عن الدورة السابقة حتى تاريخ التفعيل"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc13, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });



                            break;
                        }
                    case int code when osG6.Contains(code):
                        {
                            //	CREATE	OSLFP	G	TsDt	SeDt	"تغيير طبيعة الاشتراك السابق من اشتراك استضافة إلى اشتراك كامل"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc04, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees

                            });


                            break;
                        }
                    case int code when osG7.Contains(code):
                        {
                            //	CREATE	OSLFP	H	TsDt	SeDt	"تغيير طبيعة الاشتراك السابق من اشتراك للتمديد إلى اشتراك كامل"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);
                            //var dailyFees = price.Host / 365;
                            //var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc06, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees

                            });

                            break;
                        }
                }

                //Create the REQUIRED sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                            //	CREATE	*	*	*	*	*	
                            break;
                        }
                    case int code when rsG2.Contains(code):
                        {
                            //	CREATE	RSLFP	H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك كامل بمدة سنة واحدة من تاريخ التركيب"

                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc01, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });

                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                            //	CREATE	RSLFP	H+G	TsDt	LD	"دورة اشتراك كامل من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc08, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });

                            break;
                        }
                    case int code when rsG4.Contains(code):
                        {
                            //CREATE RSLFP   H + G TsDt SeDt    "دورة اشتراك كامل من تاريخ التفعيل حتى نهاية الدورة السابقة الغير مسددة"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc09, startDate, endDate),
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
                        APITask = APITask.ActivateOnWialon,
                        Desc = string.Format("فعل الوحدة ({0}) على منصة ويلون.", unit.SNo),
                        ExcDate = caseProfile.TsDt,
                        IsExecuted = false,
                    });

                }

                if (servcieLog.Subscriptions.Count == 384) servcieLog.IsDeserved = false;

                //Update Sub. Statuses
                unit.UStatus = UStatus.InstalledActive;

                return caseProfile.CaseCode.ToString();
            }

            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(string.Format("العملية - {0} - كود الحالة {1}", servcieLog.Desc, caseProfile.CaseCode.ToString()));

                if (unit.IsOnWialon == false)
                {
                    stringBuilder.AppendLine(string.Format("اضف الوحدة ({0}) الى منصة ويلون.", unit.SNo));
                }


                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //	UPDATE	OSLFP	G	*	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة حتى تاريخ التفعيل"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc20, startDate, endDate));

                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //	UPDATE	OSLFP	H	*	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة حتى تاريخ التفعيل"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc16, startDate, endDate));

                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //CREATE OSLFP   G SeDt    TsDt    "قيمة اشتراك تمديد مستحقة عن الدورة السابقة حتى تاريخ التفعيل"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc18, startDate, endDate));

                            break;
                        }
                    case int code when osG5.Contains(code):
                        {

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc13, startDate, endDate));



                            break;
                        }
                    case int code when osG6.Contains(code):
                        {

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc04, startDate, endDate));


                            break;
                        }
                    case int code when osG7.Contains(code):
                        {
                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc06, startDate, endDate));

                            break;
                        }
                }

                //Create the REQUIRED sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                            //	CREATE	*	*	*	*	*	
                            break;
                        }
                    case int code when rsG2.Contains(code):
                        {


                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc01, startDate, endDate));

                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                            //	CREATE	RSLFP	H+G	TsDt	LD	"دورة اشتراك كامل من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc08, startDate, endDate));

                            break;
                        }
                    case int code when rsG4.Contains(code):
                        {
                            //CREATE RSLFP   H + G TsDt SeDt    "دورة اشتراك كامل من تاريخ التفعيل حتى نهاية الدورة السابقة الغير مسددة"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc09, startDate, endDate));
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
    internal static string ActivateForHosting(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)
    {
        //bool ApplyChangesToDatabaseFlag = applyChangesToDatabase;

        var currentSubscription = unit.Subscriptions?.OrderBy(x => x.Id).LastOrDefault();

        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 256);

        int[] osLFPG1 = { 256, 342, 278, 382, 318, 383, 350, 286, 351, 329, 265, 330, 266, 331, 325, 261, 326, 262, 333, 269, 334, 270, 335 }; //	*					
        int[] osLFPG2 = { 341, 277, 279, 349, 285, 287, 263 };  //	G					
        int[] osLFPG3 = { 267 }; //	H					
        int[] osLFPG4 = { 343, 381, 317, 319, 327, 271 }; //	H+G					

        int[] rsLFPG1 = { 343, 383, 351, 331, 327, 335 }; //	*					
        int[] rsLFPG2 = { 256, 341, 277, 342, 278, 279, 381, 317, 382, 318, 319, 349, 285, 350, 286, 287, 329, 265, 330, 266, 267, 325, 261, 326, 262, 263, 333, 269, 334, 270, 271 }; //	H					

        int[] tG1 = { 256, 341, 277, 342, 278, 279, 381, 317, 382, 318, 319, 349, 285, 350, 286, 287, 329, 265, 330, 266, 331, 267, 325, 261, 326, 262, 263, 333, 269, 334, 270, 271 }; //	Deactivate					
        int[] tG2 = { 343, 383, 351, 327, 335 }; //	Defered Deactivate					

        int[] osG1 = { 256, 342, 278, 382, 318, 383, 350, 286, 351, 329, 265, 330, 266, 331, 325, 261, 326, 262, 333, 269, 334, 270, 335 }; //	NOTHING	*	*	*	*	*
        int[] osG2 = { 279, 287, 263 }; //	UPDATE	OSLFP	G	*	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة"
        int[] osG3 = { 267 }; //	UPDATE	OSLFP	H	*	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة"
        int[] osG4 = { 319, 271 }; //	UPDATE	OSLFP	H+G	*	TsDt	"قيمة اشتراك كامل مستحقة عن دورة سابقة غير مسددة"
        int[] osG5 = { 341, 277, 349, 285 }; //	CREATE	OSLFP	G	SeDt	TsDt	"قيمة اشتراك تمديد مستحقة عن الدورة السابقة"
        int[] osG6 = { 381, 317 }; //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة"
        int[] osG7 = { 343, 327 };  //	CREATE	OSLFP	H	TsDt	SeDt	"تغيير طبيعة الاشتراك السابق من اشتراك للتمديد إلى اشتراك للاستضافة"

        int[] rsG1 = { 343, 383, 351, 331, 327, 335 }; //	NOTHING	*	*	*	*	*
        int[] rsG2 = { 256 }; //	CREATE	RSLFP	H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك للاستضافة بمدة سنة واحدة من تاريخ التركيب"
        int[] rsG3 = { 341, 277, 342, 278, 381, 317, 382, 318, 349, 285, 350, 286, 329, 265, 330, 266, 325, 261, 326, 262, 333, 269, 334, 270 }; //	CREATE	RSLFP	H	TsDt	LD	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى آخر يوم بالسنة الحالية"
        int[] rsG4 = { 279, 319, 287, 267, 263, 271 }; //	CREATE	RSLFP	H	TsDt	SeDt	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"

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
                        APITask = APITask.AddToWialon,
                        Desc = string.Format("اضف الوحدة ({0}) الى منصة ويلون.", unit.SNo),
                        ExcDate = caseProfile.TsDt,
                        IsExecuted = false,
                    });
                }

                //Create the observed sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //	UPDATE	OSLFP	G	*	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Desc = string.Format(SubscriptionDescs.Desc21, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));

                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                            //	UPDATE	OSLFP	H	*	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة"
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Desc = string.Format(SubscriptionDescs.Desc17, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int x when osG4.Contains(x):
                        {
                            //	UPDATE	OSLFP	H+G	*	TsDt	"قيمة اشتراك كامل مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Desc = string.Format(SubscriptionDescs.Desc25, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int x when osG5.Contains(x):
                        {
                            //	CREATE	OSLFP	G	SeDt	TsDt	"قيمة اشتراك تمديد مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc19, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when osG6.Contains(x):
                        {
                            //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc24, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when osG7.Contains(x):
                        {
                            //	CREATE	OSLFP	H	TsDt	SeDt	

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc07, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                }

                //Create the REQUIRED sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*	
                            break;
                        }
                    case int x when rsG2.Contains(x):
                        {
                            //	CREATE	RSLFP	H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك للاستضافة بمدة سنة واحدة من تاريخ التركيب"
                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc02, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when rsG3.Contains(x):
                        {
                            //	CREATE	RSLFP	H	TsDt	LD	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى آخر يوم بالسنة الحالية"
                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,

                                Desc = string.Format(SubscriptionDescs.Desc10, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when rsG4.Contains(x):
                        {
                            //	CREATE	RSLFP	H	TsDt	SeDt	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"
                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,

                                Desc = string.Format(SubscriptionDescs.Desc11, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                }

                //Add operator task to deactivate the unit on wialon

                switch (caseProfile.CaseCode)
                {
                    case int x when tG1.Contains(x):
                        {
                            //	Deactivate
                            if (unit.WStatus != WStatus.Inactive)
                            {
                                //Add operator task to Activate unit on wialon
                                servcieLog.WialonTasks.Add(new WialonTask()
                                {
                                    TrackingUnitId = unit.Id,
                                    APITask = APITask.DeactivateOnWialon,
                                    Desc = string.Format("إلغاء تفعيل الوحدة ({0}) على منصة ويلون.", unit.SNo),
                                    ExcDate = caseProfile.TsDt,
                                    IsExecuted = false,
                                });

                            }
                            break;
                        }
                    case int code when tG2.Contains(code):
                        {
                            //	Defered Deactivate

                            if (unit.WStatus != WStatus.Inactive)
                            {
                                //Add operator task to Activate unit on wialon
                                servcieLog.WialonTasks.Add(new WialonTask()
                                {
                                    TrackingUnitId = unit.Id,
                                    APITask = APITask.DeactivateOnWialon,
                                    Desc = string.Format("مهمة مؤجلة - إلغاء تفعيل الوحدة ({0}) على منصة ويلون.", unit.SNo),
                                    ExcDate = (DateOnly)caseProfile.SeDt,
                                    IsExecuted = false,
                                });

                            }
                            break;

                        }
                }

                if (servcieLog.Subscriptions.Count == 256) servcieLog.IsDeserved = false;

                //Update Sub. Statuses
                unit.UStatus = UStatus.InstalledActiveHosting;

                return caseProfile.CaseCode.ToString();
            }

            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(string.Format("العملية - {0} - كود الحالة {1}", servcieLog.Desc, caseProfile.CaseCode.ToString()));

                if (unit.IsOnWialon == false)
                {
                    stringBuilder.AppendLine(string.Format("اضف الوحدة ({0}) الى منصة ويلون.", unit.SNo));
                }

                //Create the observed sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //	UPDATE	OSLFP	G	*	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc21, startDate, endDate));

                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                            //	UPDATE	OSLFP	H	*	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة"
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc17, startDate, endDate));

                            break;
                        }
                    case int x when osG4.Contains(x):
                        {
                            //	UPDATE	OSLFP	H+G	*	TsDt	"قيمة اشتراك كامل مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc25, startDate, endDate));

                            break;
                        }
                    case int x when osG5.Contains(x):
                        {
                            //	CREATE	OSLFP	G	SeDt	TsDt	"قيمة اشتراك تمديد مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc19, startDate, endDate));

                            break;
                        }
                    case int x when osG6.Contains(x):
                        {
                            //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc24, startDate, endDate));

                            break;
                        }
                    case int x when osG7.Contains(x):
                        {
                            //	CREATE	OSLFP	H	TsDt	SeDt	

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc07, startDate, endDate));

                            break;
                        }
                }

                //Create the REQUIRED sub. record by CaseCode

                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*	
                            break;
                        }
                    case int x when rsG2.Contains(x):
                        {
                            //	CREATE	RSLFP	H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك للاستضافة بمدة سنة واحدة من تاريخ التركيب"
                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc02, startDate, endDate));

                            break;
                        }
                    case int x when rsG3.Contains(x):
                        {
                            //	CREATE	RSLFP	H	TsDt	LD	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى آخر يوم بالسنة الحالية"
                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10, startDate, endDate));

                            break;
                        }
                    case int x when rsG4.Contains(x):
                        {
                            //	CREATE	RSLFP	H	TsDt	SeDt	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"
                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc11, startDate, endDate));

                            break;
                        }
                }

                //Add operator task to deactivate the unit on wialon

                switch (caseProfile.CaseCode)
                {
                    case int x when tG1.Contains(x):
                        {
                            //	Deactivate
                            if (unit.WStatus != WStatus.Inactive)
                            {
                                stringBuilder.AppendLine(string.Format("إلغاء تفعيل الوحدة ({0}) على منصة ويلون.", unit.SNo));
                            }
                            break;
                        }
                    case int code when tG2.Contains(code):
                        {
                            //	Defered Deactivate

                            if (unit.WStatus != WStatus.Inactive)
                            {
                                stringBuilder.AppendLine(string.Format("مهمة مؤجلة - إلغاء تفعيل الوحدة ({0}) على منصة ويلون.", unit.SNo));
                            }
                            break;

                        }
                }

                //if (servcieLog.Subscriptions.Count == 256) servcieLog.IsDeserved = false;

                return stringBuilder.ToString();
            }

        }
        else
            throw new NotImplementedException($"{caseProfile.CaseCode.ToString()} Not Implemented Case code");
    }
    internal static string ActivateForGprs(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)
    {
        //bool ApplyChangesToDatabaseFlag = applyChangesToDatabase;

        var currentSubscription = unit.Subscriptions?.OrderBy(x => x.Id).LastOrDefault();

        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 128);


        int[] osLFPG1 = { 128, 234, 170, 254, 190, 255, 238, 174, 239, 201, 137, 202, 138, 197, 133, 198, 134, 199, 205, 141, 206, 142, 207 }; //	*					
        int[] osLFPG2 = { 135 };  //	G					
        int[] osLFPG3 = { 233, 169, 171, 237, 173, 175, 139 };  //	H					
        int[] osLFPG4 = { 235, 253, 189, 191, 203, 143 }; //	H+G					

        int[] rsLFPG1 = { 235, 255, 239, 203, 199, 207 }; //	*					
        int[] rsLFPG2 = { 128, 233, 169, 234, 170, 171, 253, 189, 254, 190, 191, 237, 173, 238, 174, 175, 201, 137, 202, 138, 139, 197, 133, 198, 134, 135, 205, 141, 206, 142, 143 }; //	G					

        //int[] tG1 = { 128, 233, 169, 234, 170, 235, 171, 253, 189, 254, 190, 255, 191, 237, 173, 238, 174, 239, 175, 201, 137, 202, 138, 203, 139, 197, 133, 198, 134, 199, 135, 205, 141, 206, 142, 207, 143 }; //	Activate					

        int[] osG1 = { 128, 234, 170, 254, 190, 255, 238, 174, 239, 201, 137, 202, 138, 197, 133, 198, 134, 199, 205, 141, 206, 142, 207 }; //	NOTHING	*	*	*	*	*
        int[] osG2 = { 235, 203 }; //	CREATE	OSLFP	G	TsDt	SeDt	"تغيير طبيعة الاشتراك السابق من اشتراك للاستضافة إلى اشتراك للتمديد"
        int[] osG3 = { 135 }; //	UPDATE	OSLFP	G	SeDt	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة"
        int[] osG4 = { 233, 169, 237, 173 }; //	CREATE	OSLFP	H	SeDt	TsDt	"قيمة اشتراك استضافة مستحقة عن الدورة السابقة"
        int[] osG5 = { 171, 175, 139 }; //	UPDATE	OSLFP	H	SeDt	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة"
        int[] osG6 = { 253, 189 }; //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة"
        int[] osG7 = { 191, 143 }; //	UPDATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن دورة سابقة غير مسددة"

        int[] rsG1 = { 235, 255, 239, 203, 199, 207 }; //	NOTHING	*	*	*	*	*
        int[] rsG2 = { 128 }; //	CREATE	RSLFP	G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك للتمديد بمدة سنة واحدة من تاريخ التركيب"
        int[] rsG3 = { 233, 169, 234, 170, 253, 189, 254, 190, 237, 173, 238, 174, 201, 137, 202, 138, 197, 133, 198, 134, 205, 141, 206, 142 }; //	CREATE	RSLFP	G	TsDt	LD	"دورة اشتراك للتمديد من تاريخ تفعيل التمديد حتى آخر يوم بالسنة الحالية"
        int[] rsG4 = { 139, 171, 191, 175, 135, 143 }; //	CREATE	RSLFP	G	TsDt	SeDt	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"

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
                        APITask = APITask.AddToWialon,
                        Desc = string.Format("اضف الوحدة ({0}) الى منصة ويلون.", unit.SNo),
                        ExcDate = caseProfile.TsDt,
                        IsExecuted = false,
                    });
                }

                //Create the observed sub. record by CaseCode
                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            // * ****
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //	CREATE	OSLFP	G	TsDt	SeDt	"تغيير طبيعة الاشتراك السابق من اشتراك للاستضافة إلى اشتراك للتمديد"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc05, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });

                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                            //	UPDATE	OSLFP	G	*	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = price.Gprs / 365;

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Desc = string.Format(SubscriptionDescs.Desc21, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;

                        }
                    case int x when osG4.Contains(x):
                        {
                            //	CREATE	OSLFP	H	SeDt	TsDt	"قيمة اشتراك استضافة مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc15, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when osG5.Contains(x):
                        {
                            //	UPDATE	OSLFP	H	*	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Desc = string.Format(SubscriptionDescs.Desc17, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                    case int x when osG6.Contains(x):
                        {
                            //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc24, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int x when osG7.Contains(x):
                        {
                            //	UPDATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن دورة سابقة غير مسددة"
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            currentSubscription.LastPaidFees = (SubPackageFees)oLF;
                            currentSubscription.CaseCode = caseProfile.CaseCode;
                            currentSubscription.Desc = string.Format(SubscriptionDescs.Desc25, startDate, endDate);
                            currentSubscription.SeDate = endDate;
                            currentSubscription.DailyFees = dailyFees;
                            currentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(currentSubscription));
                            break;
                        }
                }

                //Create the REQUIRED sub. record by CaseCode
                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*	
                            break;
                        }
                    case int code when rsG2.Contains(code):
                        {
                            //	CREATE	RSLFP	G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك للتمديد بمدة سنة واحدة من تاريخ التركيب"

                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc03, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                            //	CREATE	RSLFP	G	TsDt	LD	"دورة اشتراك للتمديد من تاريخ تفعيل التمديد حتى آخر يوم بالسنة الحالية"

                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc12, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when rsG4.Contains(code):
                        {
                            //	CREATE	RSLFP	G	TsDt	SeDt	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                            //var days = (((endDate).ToDateTime(TimeOnly.MinValue)) - (startDate).ToDateTime(TimeOnly.MinValue)).Days;

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)rLF,
                                CaseCode = caseProfile.CaseCode,
                                //Desc = "دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة",
                                Desc = string.Format(SubscriptionDescs.Desc11, startDate, endDate),
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
                        APITask = APITask.ActivateOnWialon,
                        Desc = string.Format("فعل الوحدة ({0}) على منصة ويلون.", unit.SNo),
                        ExcDate = caseProfile.TsDt,
                        IsExecuted = false,
                    });

                }

                if (servcieLog.Subscriptions.Count == 128) servcieLog.IsDeserved = false;

                //Update Sub. Statuses
                unit.UStatus = UStatus.InstalledActiveGprs;

                return caseProfile.CaseCode.ToString();
            }

            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(string.Format("العملية - {0} - كود الحالة {1}", servcieLog.Desc, caseProfile.CaseCode.ToString()));

                if (unit.IsOnWialon == false)
                {
                    stringBuilder.AppendLine(string.Format("اضف الوحدة ({0}) الى منصة ويلون.", unit.SNo));
                }

                //Create the observed sub. record by CaseCode
                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            // * ****
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //	CREATE	OSLFP	G	TsDt	SeDt	"تغيير طبيعة الاشتراك السابق من اشتراك للاستضافة إلى اشتراك للتمديد"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc05, startDate, endDate));

                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                            //	UPDATE	OSLFP	G	*	TsDt	"قيمة اشتراك تمديد مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = price.Gprs / 365;

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc21, startDate, endDate));

                            break;

                        }
                    case int x when osG4.Contains(x):
                        {
                            //	CREATE	OSLFP	H	SeDt	TsDt	"قيمة اشتراك استضافة مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc15, startDate, endDate));

                            break;
                        }
                    case int x when osG5.Contains(x):
                        {
                            //	UPDATE	OSLFP	H	*	TsDt	"قيمة اشتراك استضافة مستحقة عن دورة سابقة غير مسددة"

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc17, startDate, endDate));

                            break;
                        }
                    case int x when osG6.Contains(x):
                        {
                            //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc24, startDate, endDate));

                            break;
                        }
                    case int x when osG7.Contains(x):
                        {
                            //	UPDATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن دورة سابقة غير مسددة"
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc25, startDate, endDate));

                            break;
                        }
                }

                //Create the REQUIRED sub. record by CaseCode
                switch (caseProfile.CaseCode)
                {
                    case int x when rsG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*	
                            break;
                        }
                    case int code when rsG2.Contains(code):
                        {
                            //	CREATE	RSLFP	G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك للتمديد بمدة سنة واحدة من تاريخ التركيب"

                            var startDate = caseProfile.TsDt;
                            var endDate = caseProfile.TsDt.AddDays(365);
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc03, startDate, endDate));

                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                            //	CREATE	RSLFP	G	TsDt	LD	"دورة اشتراك للتمديد من تاريخ تفعيل التمديد حتى آخر يوم بالسنة الحالية"

                            var startDate = caseProfile.TsDt;
                            var endDate = DateOnly.FromDateTime(new DateTime(caseProfile.TsDt.Year, 12, 31));
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);


                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc12, startDate, endDate));

                            break;
                        }
                    case int code when rsG4.Contains(code):
                        {
                            //	CREATE	RSLFP	G	TsDt	SeDt	"دورة اشتراك للاستضافة من تاريخ تفعيل الاستضافة حتى تاريخ نهاية الدورة السابقة الغير مسددة"

                            var startDate = caseProfile.TsDt;
                            var endDate = (DateOnly)caseProfile.SeDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc11, startDate, endDate));

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
    internal static string Deactivate(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)
    {

        //bool ApplyChangesToDatabaseFlag = applyChangesToDatabase;

        var currentSubscription = unit.Subscriptions?.OrderBy(x => x.Id).LastOrDefault();

        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 0);

        int[] osLFPG1 = { 106, 42, 107, 43, 86, 22, 87, 23, 126, 62, 127, 63, 110, 46, 111, 47, 94, 30, 95, 31 }; //	*						
        int[] osLFPG2 = { 85, 21, 93, 29 }; //	G						
        int[] osLFPG3 = { 105, 41, 109, 45 }; //	H						
        int[] osLFPG4 = { 125, 61 }; //	H+G						

        int[] tG1 = { 105, 41, 106, 42, 85, 21, 86, 22, 125, 61, 126, 62, 109, 45, 110, 46, 93, 29, 94, 30 }; //	Remove						
        int[] tG2 = { 107, 43, 87, 23, 127, 63, 111, 47, 95, 31 }; //	Defered Remove						

        int[] osG1 = { 106, 42, 107, 43, 86, 22, 87, 23, 126, 62, 127, 63, 110, 46, 111, 47, 94, 30, 95, 31 }; //	NOTHING	*	*	*	*	*	
        int[] osG2 = { 85, 21, 93, 29 }; //	CREATE	OSLFP	G	SeDt	TsDt	"قيمة اشتراك تمديد مستحقة عن الدورة السابقة"	CREATE
        int[] osG3 = { 105, 41, 109, 45 }; //	CREATE	OSLFP	H	SeDt	TsDt	"قيمة اشتراك استضافة مستحقة عن الدورة السابقة"	CREATE
        int[] osG4 = { 125, 61 }; //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة"	CREATE

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
                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*	
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //	CREATE	OSLFP	G	SeDt	TsDt	"قيمة اشتراك تمديد مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc23, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //	CREATE	OSLFP	H	SeDt	TsDt	"قيمة اشتراك استضافة مستحقة عن الدورة السابقة حتى تاريخ إلغاء التفعيل"
                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc14, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees


                            });
                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة حتى تاريخ إلغاء التفعيل"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            servcieLog.Subscriptions?.Add(new Subscription
                            {
                                LastPaidFees = (SubPackageFees)oLF,
                                CaseCode = caseProfile.CaseCode,
                                Desc = string.Format(SubscriptionDescs.Desc26, startDate, endDate),
                                TrackingUnitId = unit.Id,
                                SsDate = startDate,
                                SeDate = endDate,
                                DailyFees = dailyFees
                            });
                            break;
                        }
                }

                switch (caseProfile.CaseCode)
                {
                    case int x when tG1.Contains(x):
                        {
                            //RemoveFromWialon
                            if ((bool)unit.IsOnWialon)
                            {
                                servcieLog.WialonTasks.Add(new WialonTask()
                                {
                                    TrackingUnitId = unit.Id,
                                    APITask = APITask.RemoveFromWialon,
                                    Desc = string.Format("حذف الوحدة ({0}) من منصة ويلون.", unit.SNo),
                                    ExcDate = caseProfile.TsDt,
                                    IsExecuted = false,
                                });


                            }

                            break;
                        }
                    case int code when tG2.Contains(code):
                        {
                            //Defered-RemoveFromWialon
                            if ((bool)unit.IsOnWialon)
                            {
                                servcieLog.WialonTasks.Add(new WialonTask()
                                {
                                    TrackingUnitId = unit.Id,
                                    APITask = APITask.RemoveFromWialon,
                                    Desc = string.Format("مهمة مؤجلة - احذف الوحدة ({0}) من منصة ويلون.", unit.SNo),
                                    ExcDate = (DateOnly)caseProfile.SeDt,
                                    IsExecuted = false,
                                });
                            }

                            break;
                        }
                }

                if (servcieLog.Subscriptions.Count == 0) servcieLog.IsDeserved = false;

                //Update Sub. Statuses
                unit.UStatus = UStatus.InstalledInactive;

                return caseProfile.CaseCode.ToString();
            }

            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(string.Format("العملية - {0} - كود الحالة {1}", servcieLog.Desc, caseProfile.CaseCode.ToString()));

                switch (caseProfile.CaseCode)
                {
                    case int x when osG1.Contains(x):
                        {
                            //	NOTHING	*	*	*	*	*	
                            break;
                        }
                    case int x when osG2.Contains(x):
                        {
                            //	CREATE	OSLFP	G	SeDt	TsDt	"قيمة اشتراك تمديد مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc23, startDate, endDate));

                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //	CREATE	OSLFP	H	SeDt	TsDt	"قيمة اشتراك استضافة مستحقة عن الدورة السابقة حتى تاريخ إلغاء التفعيل"
                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc14, startDate, endDate));

                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //	CREATE	OSLFP	H+G	SeDt	TsDt	"قيمة اشتراك كامل مستحقة عن الدورة السابقة حتى تاريخ إلغاء التفعيل"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc26, startDate, endDate));

                            break;
                        }
                }

                switch (caseProfile.CaseCode)
                {
                    case int x when tG1.Contains(x):
                        {
                            stringBuilder.AppendLine(string.Format("حذف الوحدة ({0}) من منصة ويلون.", unit.SNo));
                            break;
                        }
                    case int code when tG2.Contains(code):
                        {
                            //Defered-RemoveFromWialon
                            if ((bool)unit.IsOnWialon)
                            {
                                stringBuilder.AppendLine(string.Format("مهمة مؤجلة - احذف الوحدة ({0}) من منصة ويلون.", unit.SNo));

                            }

                            break;
                        }
                }

                //if (servcieLog.Subscriptions.Count == 0) servcieLog.IsDeserved = false;

                return stringBuilder.ToString();
            }


        }
        else
            throw new NotImplementedException($"{caseProfile.CaseCode} Not Implemented Case code");




    }

    #endregion

}

public class SerialForSharedLogic
{


    #region MyRegion
    internal static async Task<string> GenSerialNo(IApplicationDbContext cnx, string serialFor, DateOnly? date)
    {
        var now = date is null ? DateOnly.FromDateTime(DateTime.Now) : date;
        var prefix = $"{now:yyyyMM}-";
        var sequenceNumber = 1;
        var serialNo = string.Empty;

        switch (serialFor)
        {
            case "TrackedAsset":
                {
                    // Get latest asset number for current month
                    var lastTrackedAsset = await cnx.TrackedAssets.Where(i => i.TrackedAssetNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.TrackedAssetNo).FirstOrDefaultAsync();
                    if (lastTrackedAsset != null)
                    {
                        var match = Regex.Match(lastTrackedAsset.TrackedAssetNo, @$"^{prefix}(\d+)$");
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
                        {
                            sequenceNumber = lastSequence + 1;
                        }
                    }
                    serialNo = $"{prefix}{sequenceNumber:D3}";
                    break;
                }

            case "ServiceLog":
                {

                    // Get latest serviceLog number for current month
                    var lastserviceLog = await cnx.ServiceLogs.Where(i => i.ServiceNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.ServiceNo).FirstOrDefaultAsync();
                    if (lastserviceLog != null)
                    {
                        var match = Regex.Match(lastserviceLog.ServiceNo, @$"^{prefix}(\d+)$");
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
                        {
                            sequenceNumber = lastSequence + 1;
                        }
                    }
                    serialNo = $"{prefix}{sequenceNumber:D3}";
                    break;
                }

            case "Invoice":
                {
                    // Get latest invoice number for current month
                    var lastInvoice = await cnx.Invoices.Where(i => i.InvNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.InvNo).FirstOrDefaultAsync();
                    if (lastInvoice != null)
                    {
                        var match = Regex.Match(lastInvoice.InvNo, @$"^{prefix}(\d+)$");
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
                        {
                            sequenceNumber = lastSequence + 1;
                        }
                    }
                    serialNo = $"{prefix}{sequenceNumber:D3}";
                    break;
                }
            case "Ticket":
                {
                    // Get latest ticket number for current month
                    var lastTicket = await cnx.Tickets.Where(i => i.TicketNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.TicketNo).FirstOrDefaultAsync();
                    if (lastTicket != null)
                    {
                        var match = Regex.Match(lastTicket.TicketNo, @$"^{prefix}(\d+)$");
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
                        {
                            sequenceNumber = lastSequence + 1;
                        }
                    }
                    serialNo = $"{prefix}{sequenceNumber:D3}";
                    break;
                }


            default:
                {
                    throw new NotImplementedException($"Couldn't create serial number for Object {serialFor}, which passed!");
                }
        }
        return serialNo;
    }
    #endregion



}
