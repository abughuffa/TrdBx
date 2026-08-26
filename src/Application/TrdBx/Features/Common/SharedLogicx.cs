using System.Text;
using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Domain.Enums;

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
    private static readonly int[] APCC = { 384	,489,	425	,490	,426	,491	,427	,469,	405	,470	,406,	471	,407,	457,	393	,458,	394	,459	,395,	453	,389	,454,	390	,455,	391	,461	,397	,462	,398	,463	,399}; //	All			
    private static readonly int[] AHPCC = { 256	,341	,277,	342	,278	,343,	279	,381	,317	,382,	318,	383,	319	,329,	265,	330,	266,	331,	267,	325	,261	,326	,262	,327,	263	,333	,269	,334	,270	,335	,271 };//	All		
    private static readonly int[] AGPCC = {128	,233	,169	,234,	17,	342,	278,	343	,279	,381	,317,	382,	318	,383	,319,	329,	265,	330,	266	,331	,267	,325	,261	,326	,262	,327	,263	,333	,269	,334	,270	,335	,271};//All								
    private static readonly int[] DPCC = { 105,	41,	106	,42	,107,	43	,85,	21	,86,	22	,87,	23,	125	,61,	126,	62,	127,	63};
    private static readonly int[] RPCC = {26496, 31257, 26841, 31326, 26910, 31395, 26979, 31533, 27117, 31602, 27186, 31671, 27255, 31809, 27393, 31878, 27462, 31947, 27531, 1920, 2265, 1945, 2270, 1950, 2275, 1955, 2285, 1965, 2290, 1970, 2295, 1975, 2305, 1985, 2310, 1990, 2315, 1995, 26880, 31710, 27230, 31780, 27300, 31850, 27370, 31990, 27510, 32060, 27580, 32130, 27650, 32270, 27790, 32340, 27860, 32410, 27930, 2304, 2718, 2334, 2724, 2340, 2730, 2346, 2742, 2358, 2748, 2364, 2754, 2370, 2766, 2382, 2772, 2388, 2778, 2394, 27264, 32163, 27619, 32234, 27690, 32305, 27761, 32447, 27903, 32518, 27974, 32589, 28045, 32731, 28187, 32802, 28258, 32873, 28329, 2688, 3171, 2723, 3178, 2730, 3185, 2737, 3199, 2751, 3206, 2758, 3213, 2765, 3227, 2779, 3234, 2786, 3241, 2793, 28032, 33069, 28397, 33142, 28470, 33215, 28543, 33361, 28689, 33434, 28762, 33507, 28835, 33653, 28981, 33726, 29054, 33799, 29127, 3456, 4077, 3501, 4086, 3510, 4095, 3519, 4113, 3537, 4122, 3546, 4131, 3555, 4149, 3573, 4158, 3582, 4167, 3591, 28416, 33522, 28786, 33596, 28860, 33670, 28934, 33818, 29082, 33892, 29156, 33966, 29230, 34114, 29378, 34188, 29452, 34262, 29526, 3840, 4530, 3890, 4540, 3900, 4550, 3910, 4570, 3930, 4580, 3940, 4590, 3950, 4610, 3970, 4620, 3980, 4630, 3990, 28800, 33975, 29175, 34050, 29250, 34125, 29325, 34275, 29475, 34350, 29550, 34425, 29625, 34575, 29775, 34650, 29850, 34725, 29925, 4224, 4983, 4279, 4994, 4290, 5005, 4301, 5027, 4323, 5038, 4334, 5049, 4345, 5071, 4367, 5082, 4378, 5093, 4389, 29568, 34881, 29953, 34958, 30030, 35035, 30107, 35189, 30261, 35266, 30338, 35343, 30415, 35497, 30569, 35574, 30646, 35651, 30723, 4992, 5889, 5057, 5902, 5070, 5915, 5083, 5941, 5109, 5954, 5122, 5967, 5135, 5993, 5161, 6006, 5174, 6019, 5187, 29952, 35334, 30342, 35412, 30420, 35490, 30498, 35646, 30654, 35724, 30732, 35802, 30810, 35958, 30966, 36036, 31044, 36114, 31122, 5376, 6342, 5446, 6356, 5460, 6370, 5474, 6398, 5502, 6412, 5516, 6426, 5530, 6454, 5558, 6468, 5572, 6482, 5586, 30336, 35787, 30731, 35866, 30810, 35945, 30889, 36103, 31047, 36182, 31126, 36261, 31205, 36419, 31363, 36498, 31442, 36577, 31521, 5760, 6795, 5835, 6810, 5850, 6825, 5865, 6855, 5895, 6870, 5910, 6885, 5925, 6915, 5955, 6930, 5970, 6945, 5985, 32640, 38505, 33065, 38590, 33150, 38675, 33235, 38845, 33405, 38930, 33490, 39015, 33575, 39185, 33745, 39270, 33830, 39355, 33915, 8064, 9513, 8169, 9534, 8190, 9555, 8211, 9597, 8253, 9618, 8274, 9639, 8295, 9681, 8337, 9702, 8358, 9723, 8379, 33024, 38958, 33454, 39044, 33540, 39130, 33626, 39302, 33798, 39388, 33884, 39474, 33970, 39646, 34142, 39732, 34228, 39818, 34314, 8448, 9966, 8558, 9988, 8580, 10010, 8602, 10054, 8646, 10076, 8668, 10098, 8690, 10142, 8734, 10164, 8756, 10186, 8778, 33408, 39411, 33843, 39498, 33930, 39585, 34017, 39759, 34191, 39846, 34278, 39933, 34365, 40107, 34539, 40194, 34626, 40281, 34713, 8832, 10419, 8947, 10442, 8970, 10465, 8993, 10511, 9039, 10534, 9062, 10557, 9085, 10603, 9131, 10626, 9154, 10649, 9177, 40320, 47565, 40845, 47670, 40950, 47775, 41055, 47985, 41265, 48090, 41370, 48195, 41475, 48405, 41685, 48510, 41790, 48615, 41895, 15744, 18573, 15949, 18614, 15990, 18655, 16031, 18737, 16113, 18778, 16154, 18819, 16195, 18901, 16277, 18942, 16318, 18983, 16359, 40704, 48018, 41234, 48124, 41340, 48230, 41446, 48442, 41658, 48548, 41764, 48654, 41870, 48866, 42082, 48972, 42188, 49078, 42294, 16128, 19026, 16338, 19068, 16380, 19110, 16422, 19194, 16506, 19236, 16548, 19278, 16590, 19362, 16674, 19404, 16716, 19446, 16758, 41088, 48471, 41623, 48578, 41730, 48685, 41837, 48899, 42051, 49006, 42158, 49113, 42265, 49327, 42479, 49434, 42586, 49541, 42693, 16512, 19479, 16727, 19522, 16770, 19565, 16813, 19651, 16899, 19694, 16942, 19737, 16985, 19823, 17071, 19866, 17114, 19909, 17157, 48000, 56625, 48625, 56750, 48750, 56875, 48875, 57125, 49125, 57250, 49250, 57375, 49375, 57625, 49625, 57750, 49750, 57875, 49875, 23424, 27633, 23729, 27694, 23790, 27755, 23851, 27877, 23973, 27938, 24034, 27999, 24095, 28121, 24217, 28182, 24278, 28243, 24339, 48384, 57078, 49014, 57204, 49140, 57330, 49266, 57582, 49518, 57708, 49644, 57834, 49770, 58086, 50022, 58212, 50148, 58338, 50274, 23808, 28086, 24118, 28148, 24180, 28210, 24242, 28334, 24366, 28396, 24428, 28458, 24490, 28582, 24614, 28644, 24676, 28706, 24738, 48768, 57531, 49403, 57658, 49530, 57785, 49657, 58039, 49911, 58166, 50038, 58293, 50165, 58547, 50419, 58674, 50546, 58801, 50673, 24192, 28539, 24507, 28602, 24570, 28665, 24633, 28791, 24759, 28854, 24822, 28917, 24885, 29043, 25011, 29106, 25074, 29169, 25137};

    internal static string Activate(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)

    {
        //bool applyChangesToDatabaseFlag = applyChangesToDatabase;
        var currentSubscription = unit.Subscriptions?.MaxBy(x => x.Id);
        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 384);
        var r = currentSubscription;
        //var CSeDt = currentSubscription.SeDate;

        int[] osG1 = {463, 399, 490, 426, 470, 406, 457, 393, 458, 394, 453, 389, 454, 390, 461, 397, 462, 398, 384};//*	*	*	*	*
        int[] osG2 = {471, 455};//Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"												
        int[] osG3 = {491, 459};//Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"												
        int[] osG4 = {469, 405};//Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"										
        int[] osG5 = {407, 391};//Desc10014	G	*	TsDt	"تحديث - قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"										
        int[] osG6 = {489, 425};//Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"										
        int[] osG7 = {427, 395};//Desc10015	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
													
        int[] rsG1 = {463, 399};//*	*	*	*	*																
        int[] rsG2 = {407, 391, 427, 395};//Desc10021	H+G	TsDt	SeDt	"دورة اشتراك ف3 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"													
        int[] rsG3 = {469, 405, 489, 425, 490, 426, 470,	406,	457,	393,	458	, 394, 453, 389, 454, 390, 461, 397, 462, 398};//Desc10020	H+G	TsDt	LD	"دورة اشتراك ف3 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
        int[] rsG4 = {384};//Desc10009	H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف3 بمدة سنة واحدة من تاريخ التركيب"																	
        int[] rsG5 = {491, 459};//Desc10006	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 السابق"																
        int[] rsG6 = {471, 455};//Desc10005	H+G	TsDt	CAL	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 السابق"
                                                                        
        int[] osLFPG1 = {490, 426, 470, 406, 457, 393, 458, 394, 453, 389, 454, 390, 461, 397, 462, 398, 384};//	*
        int[] osLFPG2 = {407, 391, 469, 405, 471, 455};//	G									
        int[] osLFPG3 = {427, 395, 489, 425, 491, 459};//	H										
        int[] osLFPG4 = {};//	H+G

        int[] rsLFPG1 = {463,399,490,426,470,406,457,393,458,394,453,389,454,390,461,397,462,398,384}; //	*									
        int[] rsLFPG2 = {407,391,427,395,469,405,489,425,490,426,470,406,457,393,458,394,453,389,454,390,461,397,462,398,384,491,459,471,455}; //	H+G	

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

                            var startDate = currentSubscription.SsDate;
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

                            var startDate = currentSubscription.SsDate;
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

                            var startDate = (DateOnly)caseProfile.SeDt;
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

                            var startDate = currentSubscription.SsDate;
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

                            var startDate = (DateOnly)caseProfile.SeDt;
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

                            var startDate = currentSubscription.SsDate;
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
                            var endDate =  (DateOnly)caseProfile.SeDt;
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
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

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
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

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

                if (servcieLog.Subscriptions.Count == 384) servcieLog.IsDeserved = false;

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

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10010, startDate, endDate));

                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"												
    
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10011, startDate, endDate));

                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"										

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10022, startDate, endDate));

                            break;
                        }
                    case int code when osG5.Contains(code):
                        {
                            //Desc10014	G	*	TsDt	"تحديث - قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"										


                            var startDate = currentSubscription.SsDate;
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
                            var startDate = currentSubscription.SsDate;
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
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;
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
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;
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
    internal static string ActivateForHosting(TrackingUnit unit, ServiceLog servcieLog, DateOnly tsDate, CPrice price, bool applyChangesToDatabase)
    {
        //bool ApplyChangesToDatabaseFlag = applyChangesToDatabase;

        var currentSubscription = unit.Subscriptions?.MaxBy(x => x.Id);

        var caseProfile = new SubscriptionCaseProfile(unit.UStatus, currentSubscription, tsDate, 256);

        int[] osG1 = {256, 342, 278, 382, 318, 329, 265, 330, 266, 331, 325, 261, 326, 262, 333, 269, 334, 270};//	*	*	*	*
        int[] osG2 = {343, 327};//Desc10010	G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"
        int[] osG3 = {383, 335};//Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
        int[] osG4 = {279, 263};//Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
        int[] osG5 = {267};//Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
        int[] osG6 = {319, 271};//Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
        int[] osG7 = {341, 277};//Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"
        int[] osG8 = {381, 317};//Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
        
        int[] rsG1 = {331};//	*	*	*	*
        int[] rsG2 = {343, 327};//Desc10003	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 السابق"
        int[] rsG3 = {383, 335};//Desc10004	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 السابق"
        int[] rsG4 = {256};//Desc10008	H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف2 بمدة سنة واحدة من تاريخ التركيب"
        int[] rsG5 = {342, 278, 382, 318, 329, 265, 330, 266, 325, 261, 326, 262, 333, 269, 334, 270, 341, 277, 381, 317};//Desc10018	H	TsDt	LD	"دورة اشتراك ف2 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
        int[] rsG6 = {279, 263, 267, 319, 271};//Desc10019	H	TsDt	SeDt	"دورة اشتراك ف2 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"


        int[] osLFPG1 = {256, 342, 278, 382, 318, 329, 265, 330, 266, 331, 325, 261, 326, 262, 333, 269, 334, 270};//	*
        int[] osLFPG2 = {343, 327, 279, 263, 341, 277};//	G
        int[] osLFPG3 = {267};//	H
        int[] osLFPG4 = {383, 335, 319, 271, 381, 317};//	H+G

        int[] rsLFPG1 = {331};//	*
        int[] rsLFPG2 = {343, 327, 383, 335, 256, 342, 278, 382, 318, 329, 265, 330, 266, 325, 261, 326, 262, 333, 269, 334, 270, 341, 277, 381, 317, 279, 263, 267, 319, 271};//	H


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

                            var startDate = currentSubscription.SsDate;
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
                            var startDate = currentSubscription.SsDate;
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

                            var startDate = currentSubscription.SsDate;
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

                            var startDate = currentSubscription.SsDate;
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
                            var startDate = currentSubscription.SsDate;
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

                            
                            var startDate = (DateOnly)caseProfile.SeDt;
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

                             var startDate = (DateOnly)caseProfile.SeDt;
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
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

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
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

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
                            var endDate = (DateOnly)caseProfile.SeDt;
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
                if (servcieLog.Subscriptions.Count == 256) servcieLog.IsDeserved = false;



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
          

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10010, startDate, endDate));

                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                                 //Desc10012	H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
                
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host + price.Gprs) / 365, 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10012, startDate, endDate));

                            break;
                        }
                    case int x when osG4.Contains(x):
                        {
                            //Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
            
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10013, startDate, endDate));

                            break;
                        }
                    case int x when osG5.Contains(x):
                        {
                            //Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
               

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10014, startDate, endDate));

                            break;
                        }
                    case int x when osG6.Contains(x):
                        {
                             //Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
                
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round(((price.Host + price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10015, startDate, endDate));

                            break;
                        }
                    case int x when osG7.Contains(x):
                        {
                            //Desc10022	G	SeDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"

                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10022, startDate, endDate));

                            break;
                        }
                        case int x when osG8.Contains(x):
                        {
                            //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
                
                            var startDate = (DateOnly)caseProfile.SeDt;
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
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10003, startDate, endDate));

                            break;
                        }
                    case int x when rsG3.Contains(x):
                        {
                            //Desc10004	H	TsDt	CAL	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 السابق"
                
                             var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Gprs + price.Host) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;

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
                            var endDate = (DateOnly)caseProfile.SeDt;
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

                            var startDate = currentSubscription.SsDate;
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

                            var startDate = currentSubscription.SsDate;
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


                            var startDate = currentSubscription.SsDate;
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

                            var startDate = currentSubscription.SsDate;
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

                             var startDate = currentSubscription.SsDate;
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

                            var startDate = (DateOnly)caseProfile.SeDt;
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
                            var startDate = (DateOnly)caseProfile.SeDt;
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
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

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
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

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
                            var endDate = (DateOnly)caseProfile.SeDt;
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

                if (servcieLog.Subscriptions.Count == 128) servcieLog.IsDeserved = false;

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

                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs + price.Host) / 365, 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10012, startDate, endDate));

                            break;
                        }
                    case int x when osG3.Contains(x):
                        {
                                        //Desc10011	H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"
     
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                              var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10011, startDate, endDate));

                            break;

                        }
                    case int x when osG4.Contains(x):
                        {
                                       //Desc10013	G	*	TsDt	"تحديث – قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
              
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10013, startDate, endDate));

                            break;
                        }
                    case int x when osG5.Contains(x):
                        {
                              //Desc10014	H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
       
                            var startDate = currentSubscription.SsDate;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10014, startDate, endDate));

                            break;
                        }
                    case int x when osG6.Contains(x):
                        {
                                     //Desc10015	H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"

                                    var startDate = currentSubscription.SsDate;
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
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Host) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10001, startDate, endDate));

                            break;
                        }
                    case int code when rsG3.Contains(code):
                        {
                                //Desc10002	G	TsDt	CAL	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 السابق"

                           var startDate = caseProfile.TsDt;
                            
                            int daysDifference = ((DateOnly)caseProfile.SeDt).DayNumber - caseProfile.TsDt.DayNumber;

                            var ldailyFees = Math.Round(((price.Host+price.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                            var subdays = (int)((daysDifference * ldailyFees)/(Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero)));

                            var endDate = caseProfile.TsDt.AddDays(subdays);
                            
                            var dailyFees = 0.0m;


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
                            var endDate = (DateOnly)caseProfile.SeDt;
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
                            
                            var startDate = (DateOnly)caseProfile.SeDt;
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
                            
                            var startDate = (DateOnly)caseProfile.SeDt;
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

                            var startDate = (DateOnly)caseProfile.SeDt;
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

  

                if (servcieLog.Subscriptions.Count == 0) servcieLog.IsDeserved = false;

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


                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10022, startDate, endDate));

                            break;
                        }
                    case int code when osG3.Contains(code):
                        {
                            //Desc10023	H	SeDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
   
                            var startDate = (DateOnly)caseProfile.SeDt;
                            var endDate = caseProfile.TsDt;
                            var dailyFees = Math.Round((price.Host / 365), 3, MidpointRounding.AwayFromZero);

                            stringBuilder.AppendLine(string.Format(SubscriptionDescs.Desc10023, startDate, endDate));

                            break;
                        }
                    case int code when osG4.Contains(code):
                        {
                            //Desc10024	H+G	SeDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
                            var startDate = (DateOnly)caseProfile.SeDt;
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
    internal static string MixSubscriptions(TrackingUnit runit,TrackingUnit sunit, ServiceLog servcieLog, DateOnly tsDate, List<CPrice> prices,int targetStatus, bool applyChangesToDatabase)
    {
        var rcurrentSubscription = runit.Subscriptions?.MaxBy(x => x.Id);
        var scurrentSubscription = sunit.Subscriptions?.MaxBy(x => x.Id);

        //catch sub end date of the units
        DateOnly? rsubEndDate = rcurrentSubscription?.SeDate;
        DateOnly? ssubEndDate = scurrentSubscription?.SeDate;

        var rCaseCode = new SubscriptionCaseProfile(runit.UStatus, rcurrentSubscription, tsDate, 0);
        var sCaseCode = new SubscriptionCaseProfile(sunit.UStatus, scurrentSubscription, tsDate, targetStatus);
        
        // Get Mixed caseCode
        var caseCode = rCaseCode.CaseCode * sCaseCode.CaseCode;

        #region Static arrays Region
        
            #region Replaced unit's observed sub. case codes
            // *	*	*	*
            int[] RuOsG0 = {26496, 31257, 26841, 31326, 26910, 31395, 26979, 31533, 27117, 31602, 27186, 31671, 27255, 31809, 27393, 31878, 27462, 31947, 27531, 1920, 2265, 1945, 2270, 1950, 2275, 1955, 2285, 1965, 2290, 1970, 2295, 1975, 2305, 1985, 2310, 1990, 2315, 1995, 26880, 31710, 27230, 31780, 27300, 31850, 27370, 31990, 27510, 32060, 27580, 32130, 27650, 32270, 27790, 32340, 27860, 32410, 27930, 2304, 2718, 2334, 2724, 2340, 2730, 2346, 2742, 2358, 2748, 2364, 2754, 2370, 2766, 2382, 2772, 2388, 2778, 2394, 28032, 33069, 28397, 33142, 28470, 33215, 28543, 33361, 28689, 33434, 28762, 33507, 28835, 33653, 28981, 33726, 29054, 33799, 29127, 3456, 4077, 3501, 4086, 3510, 4095, 3519, 4113, 3537, 4122, 3546, 4131, 3555, 4149, 3573, 4158, 3582, 4167, 3591, 28416, 33522, 28786, 33596, 28860, 33670, 28934, 33818, 29082, 33892, 29156, 33966, 29230, 34114, 29378, 34188, 29452, 34262, 29526, 3840, 4530, 3890, 4540, 3900, 4550, 3910, 4570, 3930, 4580, 3940, 4590, 3950, 4610, 3970, 4620, 3980, 4630, 3990, 29568, 34881, 29953, 34958, 30030, 35035, 30107, 35189, 30261, 35266, 30338, 35343, 30415, 35497, 30569, 35574, 30646, 35651, 30723, 4992, 5889, 5057, 5902, 5070, 5915, 5083, 5941, 5109, 5954, 5122, 5967, 5135, 5993, 5161, 6006, 5174, 6019, 5187, 29952, 35334, 30342, 35412, 30420, 35490, 30498, 35646, 30654, 35724, 30732, 35802, 30810, 35958, 30966, 36036, 31044, 36114, 31122, 5376, 6342, 5446, 6356, 5460, 6370, 5474, 6398, 5502, 6412, 5516, 6426, 5530, 6454, 5558, 6468, 5572, 6482, 5586, 33024, 38958, 33454, 39044, 33540, 39130, 33626, 39302, 33798, 39388, 33884, 39474, 33970, 39646, 34142, 39732, 34228, 39818, 34314, 8448, 9966, 8558, 9988, 8580, 10010, 8602, 10054, 8646, 10076, 8668, 10098, 8690, 10142, 8734, 10164, 8756, 10186, 8778, 40704, 48018, 41234, 48124, 41340, 48230, 41446, 48442, 41658, 48548, 41764, 48654, 41870, 48866, 42082, 48972, 42188, 49078, 42294, 16128, 19026, 16338, 19068, 16380, 19110, 16422, 19194, 16506, 19236, 16548, 19278, 16590, 19362, 16674, 19404, 16716, 19446, 16758, 48384, 57078, 49014, 57204, 49140, 57330, 49266, 57582, 49518, 57708, 49644, 57834, 49770, 58086, 50022, 58212, 50148, 58338, 50274, 23808, 28086, 24118, 28148, 24180, 28210, 24242, 28334, 24366, 28396, 24428, 28458, 24490, 28582, 24614, 28644, 24676, 28706, 24738, 17664, 22425, 18009, 22494, 18078, 22563, 18147, 22701, 18285, 22770, 18354, 22839, 18423, 22977, 18561, 23046, 18630, 23115, 18699, 1280, 1625, 1305, 1630, 1310, 1635, 1315, 1645, 1325, 1650, 1330, 1655, 1335, 1665, 1345, 1670, 1350, 1675, 1355, 17920, 22750, 18270, 22820, 18340, 22890, 18410, 23030, 18550, 23100, 18620, 23170, 18690, 23310, 18830, 23380, 18900, 23450, 18970, 1536, 1950, 1566, 1956, 1572, 1962, 1578, 1974, 1590, 1980, 1596, 1986, 1602, 1998, 1614, 2004, 1620, 2010, 1626, 18688, 23725, 19053, 23798, 19126, 23871, 19199, 24017, 19345, 24090, 19418, 24163, 19491, 24309, 19637, 24382, 19710, 24455, 19783, 2304, 2925, 2349, 2934, 2358, 2943, 2367, 2961, 2385, 2970, 2394, 2979, 2403, 2997, 2421, 3006, 2430, 3015, 2439, 18944, 24050, 19314, 24124, 19388, 24198, 19462, 24346, 19610, 24420, 19684, 24494, 19758, 24642, 19906, 24716, 19980, 24790, 20054, 2560, 3250, 2610, 3260, 2620, 3270, 2630, 3290, 2650, 3300, 2660, 3310, 2670, 3330, 2690, 3340, 2700, 3350, 2710, 19712, 25025, 20097, 25102, 20174, 25179, 20251, 25333, 20405, 25410, 20482, 25487, 20559, 25641, 20713, 25718, 20790, 25795, 20867, 3328, 4225, 3393, 4238, 3406, 4251, 3419, 4277, 3445, 4290, 3458, 4303, 3471, 4329, 3497, 4342, 3510, 4355, 3523, 19968, 25350, 20358, 25428, 20436, 25506, 20514, 25662, 20670, 25740, 20748, 25818, 20826, 25974, 20982, 26052, 21060, 26130, 21138, 3584, 4550, 3654, 4564, 3668, 4578, 3682, 4606, 3710, 4620, 3724, 4634, 3738, 4662, 3766, 4676, 3780, 4690, 3794, 22016, 27950, 22446, 28036, 22532, 28122, 22618, 28294, 22790, 28380, 22876, 28466, 22962, 28638, 23134, 28724, 23220, 28810, 23306, 5632, 7150, 5742, 7172, 5764, 7194, 5786, 7238, 5830, 7260, 5852, 7282, 5874, 7326, 5918, 7348, 5940, 7370, 5962, 27136, 34450, 27666, 34556, 27772, 34662, 27878, 34874, 28090, 34980, 28196, 35086, 28302, 35298, 28514, 35404, 28620, 35510, 28726, 10752, 13650, 10962, 13692, 11004, 13734, 11046, 13818, 11130, 13860, 11172, 13902, 11214, 13986, 11298, 14028, 11340, 14070, 11382, 32256, 40950, 32886, 41076, 33012, 41202, 33138, 41454, 33390, 41580, 33516, 41706, 33642, 41958, 33894, 42084, 34020, 42210, 34146, 15872, 20150, 16182, 20212, 16244, 20274, 16306, 20398, 16430, 20460, 16492, 20522, 16554, 20646, 16678, 20708, 16740, 20770, 16802, 8832, 13593, 9177, 13662, 9246, 13731, 9315, 13869, 9453, 13938, 9522, 14007, 9591, 14145, 9729, 14214, 9798, 14283, 9867, 640, 985, 665, 990, 670, 995, 675, 1005, 685, 1010, 690, 1015, 695, 1025, 705, 1030, 710, 1035, 715, 8960, 13790, 9310, 13860, 9380, 13930, 9450, 14070, 9590, 14140, 9660, 14210, 9730, 14350, 9870, 14420, 9940, 14490, 10010, 768, 1182, 798, 1188, 804, 1194, 810, 1206, 822, 1212, 828, 1218, 834, 1230, 846, 1236, 852, 1242, 858, 9344, 14381, 9709, 14454, 9782, 14527, 9855, 14673, 10001, 14746, 10074, 14819, 10147, 14965, 10293, 15038, 10366, 15111, 10439, 1152, 1773, 1197, 1782, 1206, 1791, 1215, 1809, 1233, 1818, 1242, 1827, 1251, 1845, 1269, 1854, 1278, 1863, 1287, 9472, 14578, 9842, 14652, 9916, 14726, 9990, 14874, 10138, 14948, 10212, 15022, 10286, 15170, 10434, 15244, 10508, 15318, 10582, 1280, 1970, 1330, 1980, 1340, 1990, 1350, 2010, 1370, 2020, 1380, 2030, 1390, 2050, 1410, 2060, 1420, 2070, 1430, 9856, 15169, 10241, 15246, 10318, 15323, 10395, 15477, 10549, 15554, 10626, 15631, 10703, 15785, 10857, 15862, 10934, 15939, 11011, 1664, 2561, 1729, 2574, 1742, 2587, 1755, 2613, 1781, 2626, 1794, 2639, 1807, 2665, 1833, 2678, 1846, 2691, 1859, 9984, 15366, 10374, 15444, 10452, 15522, 10530, 15678, 10686, 15756, 10764, 15834, 10842, 15990, 10998, 16068, 11076, 16146, 11154, 1792, 2758, 1862, 2772, 1876, 2786, 1890, 2814, 1918, 2828, 1932, 2842, 1946, 2870, 1974, 2884, 1988, 2898, 2002, 11008, 16942, 11438, 17028, 11524, 17114, 11610, 17286, 11782, 17372, 11868, 17458, 11954, 17630, 12126, 17716, 12212, 17802, 12298, 2816, 4334, 2926, 4356, 2948, 4378, 2970, 4422, 3014, 4444, 3036, 4466, 3058, 4510, 3102, 4532, 3124, 4554, 3146, 13568, 20882, 14098, 20988, 14204, 21094, 14310, 21306, 14522, 21412, 14628, 21518, 14734, 21730, 14946, 21836, 15052, 21942, 15158, 5376, 8274, 5586, 8316, 5628, 8358, 5670, 8442, 5754, 8484, 5796, 8526, 5838, 8610, 5922, 8652, 5964, 8694, 6006, 16128, 24822, 16758, 24948, 16884, 25074, 17010, 25326, 17262, 25452, 17388, 25578, 17514, 25830, 17766, 25956, 17892, 26082, 18018, 7936, 12214, 8246, 12276, 8308, 12338, 8370, 12462, 8494, 12524, 8556, 12586, 8618, 12710, 8742, 12772, 8804, 12834, 8866};
            
            // G	RseDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"
            int[] RuOsG1 = {32640, 38505, 33065, 38590, 33150, 38675, 33235, 38845, 33405, 38930, 33490, 39015, 33575, 39185, 33745, 39270, 33830, 39355, 33915, 8064, 9513, 8169, 9534, 8190, 9555, 8211, 9597, 8253, 9618, 8274, 9639, 8295, 9681, 8337, 9702, 8358, 9723, 8379, 21760, 27625, 22185, 27710, 22270, 27795, 22355, 27965, 22525, 28050, 22610, 28135, 22695, 28305, 22865, 28390, 22950, 28475, 23035, 5376, 6825, 5481, 6846, 5502, 6867, 5523, 6909, 5565, 6930, 5586, 6951, 5607, 6993, 5649, 7014, 5670, 7035, 5691, 10880, 16745, 11305, 16830, 11390, 16915, 11475, 17085, 11645, 17170, 11730, 17255, 11815, 17425, 11985, 17510, 12070, 17595, 12155, 2688, 4137, 2793, 4158, 2814, 4179, 2835, 4221, 2877, 4242, 2898, 4263, 2919, 4305, 2961, 4326, 2982, 4347, 3003};
            // H	RseDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
            int[] RuOsG2 = {40320, 47565, 40845, 47670, 40950, 47775, 41055, 47985, 41265, 48090, 41370, 48195, 41475, 48405, 41685, 48510, 41790, 48615, 41895, 15744, 18573, 15949, 18614, 15990, 18655, 16031, 18737, 16113, 18778, 16154, 18819, 16195, 18901, 16277, 18942, 16318, 18983, 16359, 26880, 34125, 27405, 34230, 27510, 34335, 27615, 34545, 27825, 34650, 27930, 34755, 28035, 34965, 28245, 35070, 28350, 35175, 28455, 10496, 13325, 10701, 13366, 10742, 13407, 10783, 13489, 10865, 13530, 10906, 13571, 10947, 13653, 11029, 13694, 11070, 13735, 11111, 13440, 20685, 13965, 20790, 14070, 20895, 14175, 21105, 14385, 21210, 14490, 21315, 14595, 21525, 14805, 21630, 14910, 21735, 15015, 5248, 8077, 5453, 8118, 5494, 8159, 5535, 8241, 5617, 8282, 5658, 8323, 5699, 8405, 5781, 8446, 5822, 8487, 5863};
            // H+G	RseDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"
            int[] RuOsG3 = {48000, 56625, 48625, 56750, 48750, 56875, 48875, 57125, 49125, 57250, 49250, 57375, 49375, 57625, 49625, 57750, 49750, 57875, 49875, 23424, 27633, 23729, 27694, 23790, 27755, 23851, 27877, 23973, 27938, 24034, 27999, 24095, 28121, 24217, 28182, 24278, 28243, 24339, 32000, 40625, 32625, 40750, 32750, 40875, 32875, 41125, 33125, 41250, 33250, 41375, 33375, 41625, 33625, 41750, 33750, 41875, 33875, 15616, 19825, 15921, 19886, 15982, 19947, 16043, 20069, 16165, 20130, 16226, 20191, 16287, 20313, 16409, 20374, 16470, 20435, 16531, 16000, 24625, 16625, 24750, 16750, 24875, 16875, 25125, 17125, 25250, 17250, 25375, 17375, 25625, 17625, 25750, 17750, 25875, 17875, 7808, 12017, 8113, 12078, 8174, 12139, 8235, 12261, 8357, 12322, 8418, 12383, 8479, 12505, 8601, 12566, 8662, 12627, 8723};
  
            // G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"
            int[] RuOsG4 = {8832, 10419, 8947, 10442, 8970, 10465, 8993, 10511, 9039, 10534, 9062, 10557, 9085, 10603, 9131, 10626, 9154, 10649, 9177, 1792, 2275, 1827, 2282, 1834, 2289, 1841, 2303, 1855, 2310, 1862, 2317, 1869, 2331, 1883, 2338, 1890, 2345, 1897, 5888, 7475, 6003, 7498, 6026, 7521, 6049, 7567, 6095, 7590, 6118, 7613, 6141, 7659, 6187, 7682, 6210, 7705, 6233, 896, 1379, 931, 1386, 938, 1393, 945, 1407, 959, 1414, 966, 1421, 973, 1435, 987, 1442, 994, 1449, 1001, 2944, 4531, 3059, 4554, 3082, 4577, 3105, 4623, 3151, 4646, 3174, 4669, 3197, 4715, 3243, 4738, 3266, 4761, 3289};
            // H	*	TsDt	"تحديث - إنهاء دورة اشتراك ف2 السابقة"
            int[] RuOsG5 = {16512, 19479, 16727, 19522, 16770, 19565, 16813, 19651, 16899, 19694, 16942, 19737, 16985, 19823, 17071, 19866, 17114, 19909, 17157, 2816, 3575, 2871, 3586, 2882, 3597, 2893, 3619, 2915, 3630, 2926, 3641, 2937, 3663, 2959, 3674, 2970, 3685, 2981, 11008, 13975, 11223, 14018, 11266, 14061, 11309, 14147, 11395, 14190, 11438, 14233, 11481, 14319, 11567, 14362, 11610, 14405, 11653, 1408, 2167, 1463, 2178, 1474, 2189, 1485, 2211, 1507, 2222, 1518, 2233, 1529, 2255, 1551, 2266, 1562, 2277, 1573, 5504, 8471, 5719, 8514, 5762, 8557, 5805, 8643, 5891, 8686, 5934, 8729, 5977, 8815, 6063, 8858, 6106, 8901, 6149}; 
            // H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
            int[] RuOsG6 = {3840, 4875, 3915, 4890, 3930, 4905, 3945, 4935, 3975, 4950, 3990, 4965, 4005, 4995, 4035, 5010, 4050, 5025, 4065, 16128, 20475, 16443, 20538, 16506, 20601, 16569, 20727, 16695, 20790, 16758, 20853, 16821, 20979, 16947, 21042, 17010, 21105, 17073, 1920, 2955, 1995, 2970, 2010, 2985, 2025, 3015, 2055, 3030, 2070, 3045, 2085, 3075, 2115, 3090, 2130, 3105, 2145, 8064, 12411, 8379, 12474, 8442, 12537, 8505, 12663, 8631, 12726, 8694, 12789, 8757, 12915, 8883, 12978, 8946, 13041, 9009};
            
            // G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة الغير مسددة"
            int[] RuOsG7 = {2688, 3171, 2723, 3178, 2730, 3185, 2737, 3199, 2751, 3206, 2758, 3213, 2765, 3227, 2779, 3234, 2786, 3241, 2793};
            // H	*	TsDt	"تحديث - إنهاء دورة اشتراك ف2 السابقة الغير مسددة"
            int[] RuOsG8 = {4224, 4983, 4279, 4994, 4290, 5005, 4301, 5027, 4323, 5038, 4334, 5049, 4345, 5071, 4367, 5082, 4378, 5093, 4389};
            // H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة الغير مسددة"
            int[] RuOsG9 = {5760, 6795, 5835, 6810, 5850, 6825, 5865, 6855, 5895, 6870, 5910, 6885, 5925, 6915, 5955, 6930, 5970, 6945, 5985, 24192, 28539, 24507, 28602, 24570, 28665, 24633, 28791, 24759, 28854, 24822, 28917, 24885, 29043, 25011, 29106, 25074, 29169, 25137};
           
            // G	*	TsDt	""تحديث – نقل بقية الاشتراك ف1 المسدد إلى الوحدة البديلة""
            int[] RuOsG10 = {27264, 32163, 27619, 32234, 27690, 32305, 27761, 32447, 27903, 32518, 27974, 32589, 28045, 32731, 28187, 32802, 28258, 32873, 28329, 33408, 39411, 33843, 39498, 33930, 39585, 34017, 39759, 34191, 39846, 34278, 39933, 34365, 40107, 34539, 40194, 34626, 40281, 34713, 18176, 23075, 18531, 23146, 18602, 23217, 18673, 23359, 18815, 23430, 18886, 23501, 18957, 23643, 19099, 23714, 19170, 23785, 19241, 22272, 28275, 22707, 28362, 22794, 28449, 22881, 28623, 23055, 28710, 23142, 28797, 23229, 28971, 23403, 29058, 23490, 29145, 23577, 9088, 13987, 9443, 14058, 9514, 14129, 9585, 14271, 9727, 14342, 9798, 14413, 9869, 14555, 10011, 14626, 10082, 14697, 10153, 11136, 17139, 11571, 17226, 11658, 17313, 11745, 17487, 11919, 17574, 12006, 17661, 12093, 17835, 12267, 17922, 12354, 18009, 12441};      
            // H	*	TsDt	""تحديث – نقل بقية الاشتراك ف2 المسدد إلى الوحدة البديلة""
            int[] RuOsG11 = {28800, 33975, 29175, 34050, 29250, 34125, 29325, 34275, 29475, 34350, 29550, 34425, 29625, 34575, 29775, 34650, 29850, 34725, 29925, 41088, 48471, 41623, 48578, 41730, 48685, 41837, 48899, 42051, 49006, 42158, 49113, 42265, 49327, 42479, 49434, 42586, 49541, 42693, 19200, 24375, 19575, 24450, 19650, 24525, 19725, 24675, 19875, 24750, 19950, 24825, 20025, 24975, 20175, 25050, 20250, 25125, 20325, 27392, 34775, 27927, 34882, 28034, 34989, 28141, 35203, 28355, 35310, 28462, 35417, 28569, 35631, 28783, 35738, 28890, 35845, 28997, 9600, 14775, 9975, 14850, 10050, 14925, 10125, 15075, 10275, 15150, 10350, 15225, 10425, 15375, 10575, 15450, 10650, 15525, 10725, 13696, 21079, 14231, 21186, 14338, 21293, 14445, 21507, 14659, 21614, 14766, 21721, 14873, 21935, 15087, 22042, 15194, 22149, 15301};
            // H+G	*	TsDt	""تحديث – نقل بقية الاشتراك ف3 المسدد إلى الوحدة البديلة""
            int[] RuOsG12 = {30336, 35787, 30731, 35866, 30810, 35945, 30889, 36103, 31047, 36182, 31126, 36261, 31205, 36419, 31363, 36498, 31442, 36577, 31521, 48768, 57531, 49403, 57658, 49530, 57785, 49657, 58039, 49911, 58166, 50038, 58293, 50165, 58547, 50419, 58674, 50546, 58801, 50673, 20224, 25675, 20619, 25754, 20698, 25833, 20777, 25991, 20935, 26070, 21014, 26149, 21093, 26307, 21251, 26386, 21330, 26465, 21409, 32512, 41275, 33147, 41402, 33274, 41529, 33401, 41783, 33655, 41910, 33782, 42037, 33909, 42291, 34163, 42418, 34290, 42545, 34417, 10112, 15563, 10507, 15642, 10586, 15721, 10665, 15879, 10823, 15958, 10902, 16037, 10981, 16195, 11139, 16274, 11218, 16353, 11297, 16256, 25019, 16891, 25146, 17018, 25273, 17145, 25527, 17399, 25654, 17526, 25781, 17653, 26035, 17907, 26162, 18034, 26289, 18161};
               
#endregion
            
            #region  Selected unit's observed sub. case codes
            // *	*	*	*
            int[] SuOsG0 = {26496, 31257, 26841, 31326, 26910, 31533, 27117, 31602, 27186, 31809, 27393, 31878, 27462, 31947, 1920, 2265, 1945, 2270, 1950, 2285, 1965, 2290, 1970, 2305, 1985, 2310, 1990, 2315, 26880, 31710, 27230, 31780, 27300, 31990, 27510, 32060, 27580, 32270, 27790, 32340, 27860, 32410, 2304, 2718, 2334, 2724, 2340, 2742, 2358, 2748, 2364, 2766, 2382, 2772, 2388, 2778, 27264, 32163, 27619, 32234, 27690, 32447, 27903, 32518, 27974, 32731, 28187, 32802, 28258, 32873, 2688, 3171, 2723, 3178, 2730, 3199, 2751, 3206, 2758, 3227, 2779, 3234, 2786, 3241, 28032, 33069, 28397, 33142, 28470, 33361, 28689, 33434, 28762, 33653, 28981, 33726, 29054, 33799, 3456, 4077, 3501, 4086, 3510, 4113, 3537, 4122, 3546, 4149, 3573, 4158, 3582, 4167, 28416, 33522, 28786, 33596, 28860, 33818, 29082, 33892, 29156, 34114, 29378, 34188, 29452, 34262, 3840, 4530, 3890, 4540, 3900, 4570, 3930, 4580, 3940, 4610, 3970, 4620, 3980, 4630, 28800, 33975, 29175, 34050, 29250, 34275, 29475, 34350, 29550, 34575, 29775, 34650, 29850, 34725, 4224, 4983, 4279, 4994, 4290, 5027, 4323, 5038, 4334, 5071, 4367, 5082, 4378, 5093, 29568, 34881, 29953, 34958, 30030, 35189, 30261, 35266, 30338, 35497, 30569, 35574, 30646, 35651, 4992, 5889, 5057, 5902, 5070, 5941, 5109, 5954, 5122, 5993, 5161, 6006, 5174, 6019, 29952, 35334, 30342, 35412, 30420, 35646, 30654, 35724, 30732, 35958, 30966, 36036, 31044, 36114, 5376, 6342, 5446, 6356, 5460, 6398, 5502, 6412, 5516, 6454, 5558, 6468, 5572, 6482, 30336, 35787, 30731, 35866, 30810, 36103, 31047, 36182, 31126, 36419, 31363, 36498, 31442, 36577, 5760, 6795, 5835, 6810, 5850, 6855, 5895, 6870, 5910, 6915, 5955, 6930, 5970, 6945, 32640, 38505, 33065, 38590, 33150, 38845, 33405, 38930, 33490, 39185, 33745, 39270, 33830, 39355, 8064, 9513, 8169, 9534, 8190, 9597, 8253, 9618, 8274, 9681, 8337, 9702, 8358, 9723, 33024, 38958, 33454, 39044, 33540, 39302, 33798, 39388, 33884, 39646, 34142, 39732, 34228, 39818, 8448, 9966, 8558, 9988, 8580, 10054, 8646, 10076, 8668, 10142, 8734, 10164, 8756, 10186, 33408, 39411, 33843, 39498, 33930, 39759, 34191, 39846, 34278, 40107, 34539, 40194, 34626, 40281, 8832, 10419, 8947, 10442, 8970, 10511, 9039, 10534, 9062, 10603, 9131, 10626, 9154, 10649, 40320, 47565, 40845, 47670, 40950, 47985, 41265, 48090, 41370, 48405, 41685, 48510, 41790, 48615, 15744, 18573, 15949, 18614, 15990, 18737, 16113, 18778, 16154, 18901, 16277, 18942, 16318, 18983, 40704, 48018, 41234, 48124, 41340, 48442, 41658, 48548, 41764, 48866, 42082, 48972, 42188, 49078, 16128, 19026, 16338, 19068, 16380, 19194, 16506, 19236, 16548, 19362, 16674, 19404, 16716, 19446, 41088, 48471, 41623, 48578, 41730, 48899, 42051, 49006, 42158, 49327, 42479, 49434, 42586, 49541, 16512, 19479, 16727, 19522, 16770, 19651, 16899, 19694, 16942, 19823, 17071, 19866, 17114, 19909, 48000, 56625, 48625, 56750, 48750, 57125, 49125, 57250, 49250, 57625, 49625, 57750, 49750, 57875, 23424, 27633, 23729, 27694, 23790, 27877, 23973, 27938, 24034, 28121, 24217, 28182, 24278, 28243, 48384, 57078, 49014, 57204, 49140, 57582, 49518, 57708, 49644, 58086, 50022, 58212, 50148, 58338, 23808, 28086, 24118, 28148, 24180, 28334, 24366, 28396, 24428, 28582, 24614, 28644, 24676, 28706, 48768, 57531, 49403, 57658, 49530, 58039, 49911, 58166, 50038, 58547, 50419, 58674, 50546, 58801, 24192, 28539, 24507, 28602, 24570, 28791, 24759, 28854, 24822, 29043, 25011, 29106, 25074, 29169, 17664, 22425, 18009, 22494, 18078, 22701, 18285, 22770, 18354, 22839, 22977, 18561, 23046, 18630, 1280, 1625, 1305, 1630, 1310, 1645, 1325, 1650, 1330, 1655, 1665, 1345, 1670, 1350, 17920, 22750, 18270, 22820, 18340, 23030, 18550, 23100, 18620, 23170, 23310, 18830, 23380, 18900, 1536, 1950, 1566, 1956, 1572, 1974, 1590, 1980, 1596, 1986, 1998, 1614, 2004, 1620, 18176, 23075, 18531, 23146, 18602, 23359, 18815, 23430, 18886, 23501, 23643, 19099, 23714, 19170, 1792, 2275, 1827, 2282, 1834, 2303, 1855, 2310, 1862, 2317, 2331, 1883, 2338, 1890, 18688, 23725, 19053, 23798, 19126, 24017, 19345, 24090, 19418, 24163, 24309, 19637, 24382, 19710, 2304, 2925, 2349, 2934, 2358, 2961, 2385, 2970, 2394, 2979, 2997, 2421, 3006, 2430, 18944, 24050, 19314, 24124, 19388, 24346, 19610, 24420, 19684, 24494, 24642, 19906, 24716, 19980, 2560, 3250, 2610, 3260, 2620, 3290, 2650, 3300, 2660, 3310, 3330, 2690, 3340, 2700, 19200, 24375, 19575, 24450, 19650, 24675, 19875, 24750, 19950, 24825, 24975, 20175, 25050, 20250, 2816, 3575, 2871, 3586, 2882, 3619, 2915, 3630, 2926, 3641, 3663, 2959, 3674, 2970, 19712, 25025, 20097, 25102, 20174, 25333, 20405, 25410, 20482, 25487, 25641, 20713, 25718, 20790, 3328, 4225, 3393, 4238, 3406, 4277, 3445, 4290, 3458, 4303, 4329, 3497, 4342, 3510, 19968, 25350, 20358, 25428, 20436, 25662, 20670, 25740, 20748, 25818, 25974, 20982, 26052, 21060, 3584, 4550, 3654, 4564, 3668, 4606, 3710, 4620, 3724, 4634, 4662, 3766, 4676, 3780, 20224, 25675, 20619, 25754, 20698, 25991, 20935, 26070, 21014, 26149, 26307, 21251, 26386, 21330, 3840, 4875, 3915, 4890, 3930, 4935, 3975, 4950, 3990, 4965, 4995, 4035, 5010, 4050, 21760, 27625, 22185, 27710, 22270, 27965, 22525, 28050, 22610, 28135, 28305, 22865, 28390, 22950, 5376, 6825, 5481, 6846, 5502, 6909, 5565, 6930, 5586, 6951, 6993, 5649, 7014, 5670, 22016, 27950, 22446, 28036, 22532, 28294, 22790, 28380, 22876, 28466, 28638, 23134, 28724, 23220, 5632, 7150, 5742, 7172, 5764, 7238, 5830, 7260, 5852, 7282, 7326, 5918, 7348, 5940, 22272, 28275, 22707, 28362, 22794, 28623, 23055, 28710, 23142, 28797, 28971, 23403, 29058, 23490, 5888, 7475, 6003, 7498, 6026, 7567, 6095, 7590, 6118, 7613, 7659, 6187, 7682, 6210, 26880, 34125, 27405, 34230, 27510, 34545, 27825, 34650, 27930, 34755, 34965, 28245, 35070, 28350, 10496, 13325, 10701, 13366, 10742, 13489, 10865, 13530, 10906, 13571, 13653, 11029, 13694, 11070, 27136, 34450, 27666, 34556, 27772, 34874, 28090, 34980, 28196, 35086, 35298, 28514, 35404, 28620, 10752, 13650, 10962, 13692, 11004, 13818, 11130, 13860, 11172, 13902, 13986, 11298, 14028, 11340, 27392, 34775, 27927, 34882, 28034, 35203, 28355, 35310, 28462, 35417, 35631, 28783, 35738, 28890, 11008, 13975, 11223, 14018, 11266, 14147, 11395, 14190, 11438, 14233, 14319, 11567, 14362, 11610, 32000, 40625, 32625, 40750, 32750, 41125, 33125, 41250, 33250, 41375, 41625, 33625, 41750, 33750, 15616, 19825, 15921, 19886, 15982, 20069, 16165, 20130, 16226, 20191, 20313, 16409, 20374, 16470, 32256, 40950, 32886, 41076, 33012, 41454, 33390, 41580, 33516, 41706, 41958, 33894, 42084, 34020, 15872, 20150, 16182, 20212, 16244, 20398, 16430, 20460, 16492, 20522, 20646, 16678, 20708, 16740, 32512, 41275, 33147, 41402, 33274, 41783, 33655, 41910, 33782, 42037, 42291, 34163, 42418, 34290, 16128, 20475, 16443, 20538, 16506, 20727, 16695, 20790, 16758, 20853, 20979, 16947, 21042, 17010, 8832, 13593, 9177, 13662, 9246, 13731, 13869, 9453, 13938, 9522, 14145, 9729, 14214, 9798, 640, 985, 665, 990, 670, 995, 1005, 685, 1010, 690, 1025, 705, 1030, 710, 8960, 13790, 9310, 13860, 9380, 13930, 14070, 9590, 14140, 9660, 14350, 9870, 14420, 9940, 768, 1182, 798, 1188, 804, 1194, 1206, 822, 1212, 828, 1230, 846, 1236, 852, 9088, 13987, 9443, 14058, 9514, 14129, 14271, 9727, 14342, 9798, 14555, 10011, 14626, 10082, 896, 1379, 931, 1386, 938, 1393, 1407, 959, 1414, 966, 1435, 987, 1442, 994, 9344, 14381, 9709, 14454, 9782, 14527, 14673, 10001, 14746, 10074, 14965, 10293, 15038, 10366, 1152, 1773, 1197, 1782, 1206, 1791, 1809, 1233, 1818, 1242, 1845, 1269, 1854, 1278, 9472, 14578, 9842, 14652, 9916, 14726, 14874, 10138, 14948, 10212, 15170, 10434, 15244, 10508, 1280, 1970, 1330, 1980, 1340, 1990, 2010, 1370, 2020, 1380, 2050, 1410, 2060, 1420, 9600, 14775, 9975, 14850, 10050, 14925, 15075, 10275, 15150, 10350, 15375, 10575, 15450, 10650, 1408, 2167, 1463, 2178, 1474, 2189, 2211, 1507, 2222, 1518, 2255, 1551, 2266, 1562, 9856, 15169, 10241, 15246, 10318, 15323, 15477, 10549, 15554, 10626, 15785, 10857, 15862, 10934, 1664, 2561, 1729, 2574, 1742, 2587, 2613, 1781, 2626, 1794, 2665, 1833, 2678, 1846, 9984, 15366, 10374, 15444, 10452, 15522, 15678, 10686, 15756, 10764, 15990, 10998, 16068, 11076, 1792, 2758, 1862, 2772, 1876, 2786, 2814, 1918, 2828, 1932, 2870, 1974, 2884, 1988, 10112, 15563, 10507, 15642, 10586, 15721, 15879, 10823, 15958, 10902, 16195, 11139, 16274, 11218, 1920, 2955, 1995, 2970, 2010, 2985, 3015, 2055, 3030, 2070, 3075, 2115, 3090, 2130, 10880, 16745, 11305, 16830, 11390, 16915, 17085, 11645, 17170, 11730, 17425, 11985, 17510, 12070, 2688, 4137, 2793, 4158, 2814, 4179, 4221, 2877, 4242, 2898, 4305, 2961, 4326, 2982, 11008, 16942, 11438, 17028, 11524, 17114, 17286, 11782, 17372, 11868, 17630, 12126, 17716, 12212, 2816, 4334, 2926, 4356, 2948, 4378, 4422, 3014, 4444, 3036, 4510, 3102, 4532, 3124, 11136, 17139, 11571, 17226, 11658, 17313, 17487, 11919, 17574, 12006, 17835, 12267, 17922, 12354, 2944, 4531, 3059, 4554, 3082, 4577, 4623, 3151, 4646, 3174, 4715, 3243, 4738, 3266, 13440, 20685, 13965, 20790, 14070, 20895, 21105, 14385, 21210, 14490, 21525, 14805, 21630, 14910, 5248, 8077, 5453, 8118, 5494, 8159, 8241, 5617, 8282, 5658, 8405, 5781, 8446, 5822, 13568, 20882, 14098, 20988, 14204, 21094, 21306, 14522, 21412, 14628, 21730, 14946, 21836, 15052, 5376, 8274, 5586, 8316, 5628, 8358, 8442, 5754, 8484, 5796, 8610, 5922, 8652, 5964, 13696, 21079, 14231, 21186, 14338, 21293, 21507, 14659, 21614, 14766, 21935, 15087, 22042, 15194, 5504, 8471, 5719, 8514, 5762, 8557, 8643, 5891, 8686, 5934, 8815, 6063, 8858, 6106, 16000, 24625, 16625, 24750, 16750, 24875, 25125, 17125, 25250, 17250, 25625, 17625, 25750, 17750, 7808, 12017, 8113, 12078, 8174, 12139, 12261, 8357, 12322, 8418, 12505, 8601, 12566, 8662, 16128, 24822, 16758, 24948, 16884, 25074, 25326, 17262, 25452, 17388, 25830, 17766, 25956, 17892, 7936, 12214, 8246, 12276, 8308, 12338, 12462, 8494, 12524, 8556, 12710, 8742, 12772, 8804, 16256, 25019, 16891, 25146, 17018, 25273, 25527, 17399, 25654, 17526, 26035, 17907, 26162, 18034, 8064, 12411, 8379, 12474, 8442, 12537, 12663, 8631, 12726, 8694, 12915, 8883, 12978, 8946};
            // G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"
            int[] SuOsG1 = {31395, 2275, 31850, 2730, 32305, 3185, 33215, 4095, 33670, 4550, 34125, 5005, 35035, 5915, 35490, 6370, 35945, 6825, 38675, 9555, 39130, 10010, 39585, 10465, 47775, 18655, 48230, 19110, 48685, 19565, 56875, 27755, 57330, 28210, 57785, 28665, 22563, 1635, 22890, 1962, 23217, 2289, 23871, 2943, 24198, 3270, 24525, 3597, 25179, 4251, 25506, 4578, 25833, 4905, 27795, 6867, 28122, 7194, 28449, 7521, 34335, 13407, 34662, 13734, 34989, 14061, 40875, 19947, 41202, 20274, 41529, 20601};
            // H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"
            int[] SuOsG2 = {31671, 2295, 32130, 2754, 32589, 3213, 33507, 4131, 33966, 4590, 34425, 5049, 35343, 5967, 35802, 6426, 36261, 6885, 39015, 9639, 39474, 10098, 39933, 10557, 48195, 18819, 48654, 19278, 49113, 19737, 57375, 27999, 57834, 28458, 58293, 28917, 14007, 1015, 14210, 1218, 14413, 1421, 14819, 1827, 15022, 2030, 15225, 2233, 15631, 2639, 15834, 2842, 16037, 3045, 17255, 4263, 17458, 4466, 17661, 4669, 21315, 8323, 21518, 8526, 21721, 8729, 25375, 12383, 25578, 12586, 25781, 12789};                      
            // H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"
            int[] SuOsG3 = {23115, 1675, 23450, 2010, 23785, 2345, 24455, 3015, 24790, 3350, 25125, 3685, 25795, 4355, 26130, 4690, 26465, 5025, 28475, 7035, 28810, 7370, 29145, 7705, 35175, 13735, 35510, 14070, 35845, 14405, 41875, 20435, 42210, 20770, 42545, 21105, 14283, 1035, 14490, 1242, 14697, 1449, 15111, 1863, 15318, 2070, 15525, 2277, 15939, 2691, 16146, 2898, 16353, 3105, 17595, 4347, 17802, 4554, 18009, 4761, 21735, 8487, 21942, 8694, 22149, 8901, 25875, 12627, 26082, 12834, 26289, 13041};
            // G	*	TsDt	"تحديث - قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"
            int[] SuOsG4 = {26979, 1955, 27370, 2346, 27761, 2737, 28543, 3519, 28934, 3910, 29325, 4301, 30107, 5083, 30498, 5474, 30889, 5865, 33235, 8211, 33626, 8602, 34017, 8993, 41055, 16031, 41446, 16422, 41837, 16813, 48875, 23851, 49266, 24242, 49657, 24633,18147, 1315, 18410, 1578, 18673, 1841, 19199, 2367, 19462, 2630, 19725, 2893, 20251, 3419, 20514, 3682, 20777, 3945, 22355, 5523, 22618, 5786, 22881, 6049, 27615, 10783, 27878, 11046, 28141, 11309, 32875, 16043, 33138, 16306, 33401, 16569, 9315, 675, 9450, 810, 9585, 945, 9855, 1215, 9990, 1350, 10125, 1485, 10395, 1755, 10530, 1890, 10665, 2025, 11475, 2835, 11610, 2970, 11745, 3105, 14175, 5535, 14310, 5670, 14445, 5805, 16875, 8235, 17010, 8370, 17145, 8505};
           // H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
            int[] SuOsG5 = {27255, 1975, 27650, 2370, 28045, 2765, 28835, 3555, 29230, 3950, 29625, 4345, 30415, 5135, 30810, 5530, 31205, 5925, 33575, 8295, 33970, 8690, 34365, 9085, 41475, 16195, 41870, 16590, 42265, 16985, 49375, 24095, 49770, 24490, 50165, 24885, 18423, 1335, 18690, 1602, 18957, 1869, 19491, 2403, 19758, 2670, 20025, 2937, 20559, 3471, 20826, 3738, 21093, 4005, 22695, 5607, 22962, 5874, 23229, 6141, 28035, 10947, 28302, 11214, 28569, 11481, 33375, 16287, 33642, 16554, 33909, 16821, 9591, 695, 9730, 834, 9869, 973, 10147, 1251, 10286, 1390, 10425, 1529, 10703, 1807, 10842, 1946, 10981, 2085, 11815, 2919, 11954, 3058, 12093, 3197, 14595, 5699, 14734, 5838, 14873, 5977, 17375, 8479, 17514, 8618, 17653, 8757};
            // H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
            int[] SuOsG6 = {27531, 1995, 27930, 2394, 28329, 2793, 29127, 3591, 29526, 3990, 29925, 4389, 30723, 5187, 31122, 5586, 31521, 5985, 33915, 8379, 34314, 8778, 34713, 9177, 41895, 16359, 42294, 16758, 42693, 17157, 49875, 24339, 50274, 24738, 50673, 25137, 18699, 1355, 18970, 1626, 19241, 1897, 19783, 2439, 20054, 2710, 20325, 2981, 20867, 3523, 21138, 3794, 21409, 4065, 23035, 5691, 23306, 5962, 23577, 6233, 28455, 11111, 28726, 11382, 28997, 11653, 33875, 16531, 34146, 16802, 34417, 17073, 9867, 715, 10010, 858, 10153, 1001, 10439, 1287, 10582, 1430, 10725, 1573, 11011, 1859, 11154, 2002, 11297, 2145, 12155, 3003, 12298, 3146, 12441, 3289, 15015, 5863, 15158, 6006, 15301, 6149, 17875, 8723, 18018, 8866, 18161, 9009};
            #endregion

            #region  Selected unit's Recuired sub. case codes
            // *	*	*	*
            int[] SuRsG0 = {31947, 2315, 32410, 2778, 27264, 32163, 27619, 32234, 27690, 27761, 32447, 27903, 32518, 27974, 28045, 32731, 28187, 32802, 28258, 32873, 28329, 3241, 33799, 4167, 34262, 4630, 28800, 33975, 29175, 34050, 29250, 29325, 34275, 29475, 34350, 29550, 29625, 34575, 29775, 34650, 29850, 34725, 29925, 5093, 35651, 6019, 36114, 6482, 30336, 35787, 30731, 35866, 30810, 30889, 36103, 31047, 36182, 31126, 31205, 36419, 31363, 36498, 31442, 36577, 31521, 6945, 39355, 9723, 39818, 10186, 33408, 39411, 33843, 39498, 33930, 34017, 39759, 34191, 39846, 34278, 34365, 40107, 34539, 40194, 34626, 40281, 34713, 10649, 48615, 18983, 49078, 19446, 41088, 48471, 41623, 48578, 41730, 41837, 48899, 42051, 49006, 42158, 42265, 49327, 42479, 49434, 42586, 49541, 42693, 19909, 57875, 28243, 58338, 28706, 48768, 57531, 49403, 57658, 49530, 49657, 58039, 49911, 58166, 50038, 50165, 58547, 50419, 58674, 50546, 58801, 50673, 29169, 22839, 1655, 23170, 1986, 18176, 23075, 18531, 23146, 18602, 18673, 23359, 18815, 23430, 18886, 23501, 18957, 23643, 19099, 23714, 19170, 19241, 2317, 24163, 2979, 24494, 3310, 19200, 24375, 19575, 24450, 19650, 19725, 24675, 19875, 24750, 19950, 24825, 20025, 24975, 20175, 25050, 20250, 20325, 3641, 25487, 4303, 25818, 4634, 20224, 25675, 20619, 25754, 20698, 20777, 25991, 20935, 26070, 21014, 26149, 21093, 26307, 21251, 26386, 21330, 21409, 4965, 28135, 6951, 28466, 7282, 22272, 28275, 22707, 28362, 22794, 22881, 28623, 23055, 28710, 23142, 28797, 23229, 28971, 23403, 29058, 23490, 23577, 7613, 34755, 13571, 35086, 13902, 27392, 34775, 27927, 34882, 28034, 28141, 35203, 28355, 35310, 28462, 35417, 28569, 35631, 28783, 35738, 28890, 28997, 14233, 41375, 20191, 41706, 20522, 32512, 41275, 33147, 41402, 33274, 33401, 41783, 33655, 41910, 33782, 42037, 33909, 42291, 34163, 42418, 34290, 34417, 20853, 13731, 995, 13930, 1194, 9088, 13987, 9443, 14058, 9514, 14129, 9585, 14271, 9727, 14342, 9798, 9869, 14555, 10011, 14626, 10082, 10153, 1393, 14527, 1791, 14726, 1990, 9600, 14775, 9975, 14850, 10050, 14925, 10125, 15075, 10275, 15150, 10350, 10425, 15375, 10575, 15450, 10650, 10725, 2189, 15323, 2587, 15522, 2786, 10112, 15563, 10507, 15642, 10586, 15721, 10665, 15879, 10823, 15958, 10902, 10981, 16195, 11139, 16274, 11218, 11297, 2985, 16915, 4179, 17114, 4378, 11136, 17139, 11571, 17226, 11658, 17313, 11745, 17487, 11919, 17574, 12006, 12093, 17835, 12267, 17922, 12354, 12441, 4577, 20895, 8159, 21094, 8358, 13696, 21079, 14231, 21186, 14338, 21293, 14445, 21507, 14659, 21614, 14766, 14873, 21935, 15087, 22042, 15194, 15301, 8557, 24875, 12139, 25074, 12338, 16256, 25019, 16891, 25146, 17018, 25273, 17145, 25527, 17399, 25654, 17526, 17653, 26035, 17907, 26162, 18034, 18161, 12537};
            // G	TsDt	CAL11	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 السابق"
            int[] SuRsG1 = {14007, 1015, 14210, 1218, 14413, 1421, 14819, 1827, 15022, 2030, 15225, 2233, 15631, 2639, 15834, 2842, 16037, 3045, 17255, 4263, 17458, 4466, 17661, 4669, 21315, 8323, 21518, 8526, 21721, 8729, 25375, 12383, 25578, 12586, 25781, 12789};
            // G	TsDt	CAL12	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 السابق"
            int[] SuRsG2 = {14283, 1035, 14490, 1242, 14697, 1449, 15111, 1863, 15318, 2070, 15525, 2277, 15939, 2691, 16146, 2898, 16353, 3105, 17595, 4347, 17802, 4554, 18009, 4761, 21735, 8487, 21942, 8694, 22149, 8901, 25875, 12627, 26082, 12834, 26289, 13041};
            // H	TsDt	CAL13	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 السابق"
            int[] SuRsG3 = {22563, 1635, 22890, 1962, 23217, 2289, 23871, 2943, 24198, 3270, 24525, 3597, 25179, 4251, 25506, 4578, 25833, 4905, 27795, 6867, 28122, 7194, 28449, 7521, 34335, 13407, 34662, 13734, 34989, 14061, 40875, 19947, 41202, 20274, 41529, 20601};
            // H	TsDt	CAL15	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 السابق"
            int[] SuRsG4 = {23115, 1675, 23450, 2010, 23785, 2345, 24455, 3015, 24790, 3350, 25125, 3685, 25795, 4355, 26130, 4690, 26465, 5025, 28475, 7035, 28810, 7370, 29145, 7705, 35175, 13735, 35510, 14070, 35845, 14405, 41875, 20435, 42210, 20770, 42545, 21105};
            // H+G	TsDt	CAL16	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 السابق"
            int[] SuRsG5 = {31395, 2275, 31850, 2730, 32305, 3185, 33215, 4095, 33670, 4550, 34125, 5005, 35035, 5915, 35490, 6370, 35945, 6825, 38675, 9555, 39130, 10010, 39585, 10465, 47775, 18655, 48230, 19110, 48685, 19565, 56875, 27755, 57330, 28210, 57785, 28665};
            // H+G	TsDt	CAL17	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 السابق"
            int[] SuRsG6 = {31671, 2295, 32130, 2754, 32589, 3213, 33507, 4131, 33966, 4590, 34425, 5049, 35343, 5967, 35802, 6426, 36261, 6885, 39015, 9639, 39474, 10098, 39933, 10557, 48195, 18819, 48654, 19278, 49113, 19737, 57375, 27999, 57834, 28458, 58293, 28917};
            // G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف1 بمدة سنة واحدة من تاريخ التركيب"
            int[] SuRsG7 = {8832, 640, 8960, 768, 896, 9344, 1152, 9472, 1280, 1408, 9856, 1664, 9984, 1792, 1920, 10880, 2688, 11008, 2816, 2944, 13440, 5248, 13568, 5376, 5504, 16000, 7808, 16128, 7936, 8064};
            // H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف2 بمدة سنة واحدة من تاريخ التركيب"
            int[] SuRsG8 = {17664, 1280, 17920, 1536, 1792, 18688, 2304, 18944, 2560, 2816, 19712, 3328, 19968, 3584, 3840, 21760, 5376, 22016, 5632, 5888, 26880, 10496, 27136, 10752, 11008, 32000, 15616, 32256, 15872, 16128};
            // H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف3 بمدة سنة واحدة من تاريخ التركيب"
            int[] SuRsG9 = {26496, 1920, 26880, 2304, 2688, 28032, 3456, 28416, 3840, 4224, 29568, 4992, 29952, 5376, 5760, 32640, 8064, 33024, 8448, 8832, 40320, 15744, 40704, 16128, 16512, 48000, 23424, 48384, 23808, 24192};
            // G	TsDt	LD	"دورة اشتراك ف1 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
            int[] SuRsG10 = {13593, 9177, 13662, 9246, 13869, 9453, 13938, 9522, 14145, 9729, 14214, 9798, 985, 665, 990, 670, 1005, 685, 1010, 690, 1025, 705, 1030, 710, 13790, 9310, 13860, 9380, 14070, 9590, 14140, 9660, 14350, 9870, 14420, 9940, 1182, 798, 1188, 804, 1206, 822, 1212, 828, 1230, 846, 1236, 852, 1379, 931, 1386, 938, 1407, 959, 1414, 966, 1435, 987, 1442, 994, 14381, 9709, 14454, 9782, 14673, 10001, 14746, 10074, 14965, 10293, 15038, 10366, 1773, 1197, 1782, 1206, 1809, 1233, 1818, 1242, 1845, 1269, 1854, 1278, 14578, 9842, 14652, 9916, 14874, 10138, 14948, 10212, 15170, 10434, 15244, 10508, 1970, 1330, 1980, 1340, 2010, 1370, 2020, 1380, 2050, 1410, 2060, 1420, 2167, 1463, 2178, 1474, 2211, 1507, 2222, 1518, 2255, 1551, 2266, 1562, 15169, 10241, 15246, 10318, 15477, 10549, 15554, 10626, 15785, 10857, 15862, 10934, 2561, 1729, 2574, 1742, 2613, 1781, 2626, 1794, 2665, 1833, 2678, 1846, 15366, 10374, 15444, 10452, 15678, 10686, 15756, 10764, 15990, 10998, 16068, 11076, 2758, 1862, 2772, 1876, 2814, 1918, 2828, 1932, 2870, 1974, 2884, 1988, 2955, 1995, 2970, 2010, 3015, 2055, 3030, 2070, 3075, 2115, 3090, 2130, 16745, 11305, 16830, 11390, 17085, 11645, 17170, 11730, 17425, 11985, 17510, 12070, 4137, 2793, 4158, 2814, 4221, 2877, 4242, 2898, 4305, 2961, 4326, 2982, 16942, 11438, 17028, 11524, 17286, 11782, 17372, 11868, 17630, 12126, 17716, 12212, 4334, 2926, 4356, 2948, 4422, 3014, 4444, 3036, 4510, 3102, 4532, 3124, 4531, 3059, 4554, 3082, 4623, 3151, 4646, 3174, 4715, 3243, 4738, 3266, 20685, 13965, 20790, 14070, 21105, 14385, 21210, 14490, 21525, 14805, 21630, 14910, 8077, 5453, 8118, 5494, 8241, 5617, 8282, 5658, 8405, 5781, 8446, 5822, 20882, 14098, 20988, 14204, 21306, 14522, 21412, 14628, 21730, 14946, 21836, 15052, 8274, 5586, 8316, 5628, 8442, 5754, 8484, 5796, 8610, 5922, 8652, 5964, 8471, 5719, 8514, 5762, 8643, 5891, 8686, 5934, 8815, 6063, 8858, 6106, 24625, 16625, 24750, 16750, 25125, 17125, 25250, 17250, 25625, 17625, 25750, 17750, 12017, 8113, 12078, 8174, 12261, 8357, 12322, 8418, 12505, 8601, 12566, 8662, 24822, 16758, 24948, 16884, 25326, 17262, 25452, 17388, 25830, 17766, 25956, 17892, 12214, 8246, 12276, 8308, 12462, 8494, 12524, 8556, 12710, 8742, 12772, 8804, 12411, 8379, 12474, 8442, 12663, 8631, 12726, 8694, 12915, 8883, 12978, 8946};
            // H	TsDt	LD	"دورة اشتراك ف2 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
            int[] SuRsG11 = {22425, 18009, 22494, 18078, 22701, 18285, 22770, 18354, 22977, 18561, 23046, 18630, 1625, 1305, 1630, 1310, 1645, 1325, 1650, 1330, 1665, 1345, 1670, 1350, 22750, 18270, 22820, 18340, 23030, 18550, 23100, 18620, 23310, 18830, 23380, 18900, 1950, 1566, 1956, 1572, 1974, 1590, 1980, 1596, 1998, 1614, 2004, 1620, 2275, 1827, 2282, 1834, 2303, 1855, 2310, 1862, 2331, 1883, 2338, 1890, 23725, 19053, 23798, 19126, 24017, 19345, 24090, 19418, 24309, 19637, 24382, 19710, 2925, 2349, 2934, 2358, 2961, 2385, 2970, 2394, 2997, 2421, 3006, 2430, 24050, 19314, 24124, 19388, 24346, 19610, 24420, 19684, 24642, 19906, 24716, 19980, 3250, 2610, 3260, 2620, 3290, 2650, 3300, 2660, 3330, 2690, 3340, 2700, 3575, 2871, 3586, 2882, 3619, 2915, 3630, 2926, 3663, 2959, 3674, 2970, 25025, 20097, 25102, 20174, 25333, 20405, 25410, 20482, 25641, 20713, 25718, 20790, 4225, 3393, 4238, 3406, 4277, 3445, 4290, 3458, 4329, 3497, 4342, 3510, 25350, 20358, 25428, 20436, 25662, 20670, 25740, 20748, 25974, 20982, 26052, 21060, 4550, 3654, 4564, 3668, 4606, 3710, 4620, 3724, 4662, 3766, 4676, 3780, 4875, 3915, 4890, 3930, 4935, 3975, 4950, 3990, 4995, 4035, 5010, 4050, 27625, 22185, 27710, 22270, 27965, 22525, 28050, 22610, 28305, 22865, 28390, 22950, 6825, 5481, 6846, 5502, 6909, 5565, 6930, 5586, 6993, 5649, 7014, 5670, 27950, 22446, 28036, 22532, 28294, 22790, 28380, 22876, 28638, 23134, 28724, 23220, 7150, 5742, 7172, 5764, 7238, 5830, 7260, 5852, 7326, 5918, 7348, 5940, 7475, 6003, 7498, 6026, 7567, 6095, 7590, 6118, 7659, 6187, 7682, 6210, 34125, 27405, 34230, 27510, 34545, 27825, 34650, 27930, 34965, 28245, 35070, 28350, 13325, 10701, 13366, 10742, 13489, 10865, 13530, 10906, 13653, 11029, 13694, 11070, 34450, 27666, 34556, 27772, 34874, 28090, 34980, 28196, 35298, 28514, 35404, 28620, 13650, 10962, 13692, 11004, 13818, 11130, 13860, 11172, 13986, 11298, 14028, 11340, 13975, 11223, 14018, 11266, 14147, 11395, 14190, 11438, 14319, 11567, 14362, 11610, 40625, 32625, 40750, 32750, 41125, 33125, 41250, 33250, 41625, 33625, 41750, 33750, 19825, 15921, 19886, 15982, 20069, 16165, 20130, 16226, 20313, 16409, 20374, 16470, 40950, 32886, 41076, 33012, 41454, 33390, 41580, 33516, 41958, 33894, 42084, 34020, 20150, 16182, 20212, 16244, 20398, 16430, 20460, 16492, 20646, 16678, 20708, 16740, 20475, 16443, 20538, 16506, 20727, 16695, 20790, 16758, 20979, 16947, 21042, 17010};
            // H+G	TsDt	LD	"دورة اشتراك ف3 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
            int[] SuRsG12 = {31257, 26841, 31326, 26910, 31533, 27117, 31602, 27186, 31809, 27393, 31878, 27462, 2265, 1945, 2270, 1950, 2285, 1965, 2290, 1970, 2305, 1985, 2310, 1990, 31710, 27230, 31780, 27300, 31990, 27510, 32060, 27580, 32270, 27790, 32340, 27860, 2718, 2334, 2724, 2340, 2742, 2358, 2748, 2364, 2766, 2382, 2772, 2388, 3171, 2723, 3178, 2730, 3199, 2751, 3206, 2758, 3227, 2779, 3234, 2786, 33069, 28397, 33142, 28470, 33361, 28689, 33434, 28762, 33653, 28981, 33726, 29054, 4077, 3501, 4086, 3510, 4113, 3537, 4122, 3546, 4149, 3573, 4158, 3582, 33522, 28786, 33596, 28860, 33818, 29082, 33892, 29156, 34114, 29378, 34188, 29452, 4530, 3890, 4540, 3900, 4570, 3930, 4580, 3940, 4610, 3970, 4620, 3980, 4983, 4279, 4994, 4290, 5027, 4323, 5038, 4334, 5071, 4367, 5082, 4378, 34881, 29953, 34958, 30030, 35189, 30261, 35266, 30338, 35497, 30569, 35574, 30646, 5889, 5057, 5902, 5070, 5941, 5109, 5954, 5122, 5993, 5161, 6006, 5174, 35334, 30342, 35412, 30420, 35646, 30654, 35724, 30732, 35958, 30966, 36036, 31044, 6342, 5446, 6356, 5460, 6398, 5502, 6412, 5516, 6454, 5558, 6468, 5572, 6795, 5835, 6810, 5850, 6855, 5895, 6870, 5910, 6915, 5955, 6930, 5970, 38505, 33065, 38590, 33150, 38845, 33405, 38930, 33490, 39185, 33745, 39270, 33830, 9513, 8169, 9534, 8190, 9597, 8253, 9618, 8274, 9681, 8337, 9702, 8358, 38958, 33454, 39044, 33540, 39302, 33798, 39388, 33884, 39646, 34142, 39732, 34228, 9966, 8558, 9988, 8580, 10054, 8646, 10076, 8668, 10142, 8734, 10164, 8756, 10419, 8947, 10442, 8970, 10511, 9039, 10534, 9062, 10603, 9131, 10626, 9154, 47565, 40845, 47670, 40950, 47985, 41265, 48090, 41370, 48405, 41685, 48510, 41790, 18573, 15949, 18614, 15990, 18737, 16113, 18778, 16154, 18901, 16277, 18942, 16318, 48018, 41234, 48124, 41340, 48442, 41658, 48548, 41764, 48866, 42082, 48972, 42188, 19026, 16338, 19068, 16380, 19194, 16506, 19236, 16548, 19362, 16674, 19404, 16716, 19479, 16727, 19522, 16770, 19651, 16899, 19694, 16942, 19823, 17071, 19866, 17114, 56625, 48625, 56750, 48750, 57125, 49125, 57250, 49250, 57625, 49625, 57750, 49750, 27633, 23729, 27694, 23790, 27877, 23973, 27938, 24034, 28121, 24217, 28182, 24278, 57078, 49014, 57204, 49140, 57582, 49518, 57708, 49644, 58086, 50022, 58212, 50148, 28086, 24118, 28148, 24180, 28334, 24366, 28396, 24428, 28582, 24614, 28644, 24676, 28539, 24507, 28602, 24570, 28791, 24759, 28854, 24822, 29043, 25011, 29106, 25074};
            // G	TsDt	SseDt	"دورة اشتراك ف1 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"
            int[] SuRsG13 = {9315, 9591, 9867, 675, 695, 715, 9450, 9730, 10010, 810, 834, 858, 945, 973, 1001, 9855, 10147, 10439, 1215, 1251, 1287, 9990, 10286, 10582, 1350, 1390, 1430, 1485, 1529, 1573, 10395, 10703, 11011, 1755, 1807, 1859, 10530, 10842, 11154, 1890, 1946, 2002, 2025, 2085, 2145, 11475, 11815, 12155, 2835, 2919, 3003, 11610, 11954, 12298, 2970, 3058, 3146, 3105, 3197, 3289, 14175, 14595, 15015, 5535, 5699, 5863, 14310, 14734, 15158, 5670, 5838, 6006, 5805, 5977, 6149, 16875, 17375, 17875, 8235, 8479, 8723, 17010, 17514, 18018, 8370, 8618, 8866, 8505, 8757, 9009};
            // H	TsDt	SseDt	"دورة اشتراك ف2 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"
            int[] SuRsG14 = {18147, 18423, 18699, 1315, 1335, 1355, 18410, 18690, 18970, 1578, 1602, 1626, 1841, 1869, 1897, 19199, 19491, 19783, 2367, 2403, 2439, 19462, 19758, 20054, 2630, 2670, 2710, 2893, 2937, 2981, 20251, 20559, 20867, 3419, 3471, 3523, 20514, 20826, 21138, 3682, 3738, 3794, 3945, 4005, 4065, 22355, 22695, 23035, 5523, 5607, 5691, 22618, 22962, 23306, 5786, 5874, 5962, 6049, 6141, 6233, 27615, 28035, 28455, 10783, 10947, 11111, 27878, 28302, 28726, 11046, 11214, 11382, 11309, 11481, 11653, 32875, 33375, 33875, 16043, 16287, 16531, 33138, 33642, 34146, 16306, 16554, 16802, 16569, 16821, 17073};
            // H+G	TsDt	SseDt	"دورة اشتراك ف3 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"
            int[] SuRsG15 = {26979, 27255, 27531, 1955, 1975, 1995, 27370, 27650, 27930, 2346, 2370, 2394, 2737, 2765, 2793, 28543, 28835, 29127, 3519, 3555, 3591, 28934, 29230, 29526, 3910, 3950, 3990, 4301, 4345, 4389, 30107, 30415, 30723, 5083, 5135, 5187, 30498, 30810, 31122, 5474, 5530, 5586, 5865, 5925, 5985, 33235, 33575, 33915, 8211, 8295, 8379, 33626, 33970, 34314, 8602, 8690, 8778, 8993, 9085, 9177, 41055, 41475, 41895, 16031, 16195, 16359, 41446, 41870, 42294, 16422, 16590, 16758, 16813, 16985, 17157, 48875, 49375, 49875, 23851, 24095, 24339, 49266, 49770, 50274, 24242, 24490, 24738, 24633, 24885, 25137};
            #endregion

            #region  Selected unit's Transfered sub. case codes
            // *	*	*	*
            int[] SuTsG0 = {31947, 2315, 32410, 2778, 3241, 33799, 4167, 34262, 4630, 5093, 35651, 6019, 36114, 6482, 6945, 39355, 9723, 39818, 10186, 10649, 48615, 18983, 49078, 19446, 19909, 57875, 28243, 58338, 28706, 29169, 22839, 1655, 23170, 1986, 2317, 24163, 2979, 24494, 3310, 3641, 25487, 4303, 25818, 4634, 4965, 28135, 6951, 28466, 7282, 7613, 34755, 13571, 35086, 13902, 14233, 41375, 20191, 41706, 20522, 20853, 13731, 995, 13930, 1194, 1393, 14527, 1791, 14726, 1990, 2189, 15323, 2587, 15522, 2786, 2985, 16915, 4179, 17114, 4378, 4577, 20895, 8159, 21094, 8358, 8557, 24875, 12139, 25074, 12338, 12537, 14007, 1015, 14210, 1218, 1421, 14819, 1827, 15022, 2030, 2233, 15631, 2639, 15834, 2842, 3045, 17255, 4263, 17458, 4466, 4669, 21315, 8323, 21518, 8526, 8729, 25375, 12383, 25578, 12586, 12789, 14283, 1035, 14490, 1242, 1449, 15111, 1863, 15318, 2070, 2277, 15939, 2691, 16146, 2898, 3105, 17595, 4347, 17802, 4554, 4761, 21735, 8487, 21942, 8694, 8901, 25875, 12627, 26082, 12834, 13041, 22563, 1635, 22890, 1962, 2289, 23871, 2943, 24198, 3270, 3597, 25179, 4251, 25506, 4578, 4905, 27795, 6867, 28122, 7194, 7521, 34335, 13407, 34662, 13734, 14061, 40875, 19947, 41202, 20274, 20601, 23115, 1675, 23450, 2010, 2345, 24455, 3015, 24790, 3350, 3685, 25795, 4355, 26130, 4690, 5025, 28475, 7035, 28810, 7370, 7705, 35175, 13735, 35510, 14070, 14405, 41875, 20435, 42210, 20770, 21105, 31395, 2275, 31850, 2730, 3185, 33215, 4095, 33670, 4550, 5005, 35035, 5915, 35490, 6370, 6825, 38675, 9555, 39130, 10010, 10465, 47775, 18655, 48230, 19110, 19565, 56875, 27755, 57330, 28210, 28665, 31671, 2295, 32130, 2754, 3213, 33507, 4131, 33966, 4590, 5049, 35343, 5967, 35802, 6426, 6885, 39015, 9639, 39474, 10098, 10557, 48195, 18819, 48654, 19278, 19737, 57375, 27999, 57834, 28458, 28917, 13593, 9177, 13662, 9246, 13869, 9453, 13938, 9522, 14145, 9729, 14214, 9798, 985, 665, 990, 670, 1005, 685, 1010, 690, 1025, 705, 1030, 710, 13790, 9310, 13860, 9380, 14070, 9590, 14140, 9660, 14350, 9870, 14420, 9940, 1182, 798, 1188, 804, 1206, 822, 1212, 828, 1230, 846, 1236, 852, 1379, 931, 1386, 938, 1407, 959, 1414, 966, 1435, 987, 1442, 994, 14381, 9709, 14454, 9782, 14673, 10001, 14746, 10074, 14965, 10293, 15038, 10366, 1773, 1197, 1782, 1206, 1809, 1233, 1818, 1242, 1845, 1269, 1854, 1278, 14578, 9842, 14652, 9916, 14874, 10138, 14948, 10212, 15170, 10434, 15244, 10508, 1970, 1330, 1980, 1340, 2010, 1370, 2020, 1380, 2050, 1410, 2060, 1420, 2167, 1463, 2178, 1474, 2211, 1507, 2222, 1518, 2255, 1551, 2266, 1562, 15169, 10241, 15246, 10318, 15477, 10549, 15554, 10626, 15785, 10857, 15862, 10934, 2561, 1729, 2574, 1742, 2613, 1781, 2626, 1794, 2665, 1833, 2678, 1846, 15366, 10374, 15444, 10452, 15678, 10686, 15756, 10764, 15990, 10998, 16068, 11076, 2758, 1862, 2772, 1876, 2814, 1918, 2828, 1932, 2870, 1974, 2884, 1988, 2955, 1995, 2970, 2010, 3015, 2055, 3030, 2070, 3075, 2115, 3090, 2130, 16745, 11305, 16830, 11390, 17085, 11645, 17170, 11730, 17425, 11985, 17510, 12070, 4137, 2793, 4158, 2814, 4221, 2877, 4242, 2898, 4305, 2961, 4326, 2982, 16942, 11438, 17028, 11524, 17286, 11782, 17372, 11868, 17630, 12126, 17716, 12212, 4334, 2926, 4356, 2948, 4422, 3014, 4444, 3036, 4510, 3102, 4532, 3124, 4531, 3059, 4554, 3082, 4623, 3151, 4646, 3174, 4715, 3243, 4738, 3266, 20685, 13965, 20790, 14070, 21105, 14385, 21210, 14490, 21525, 14805, 21630, 14910, 8077, 5453, 8118, 5494, 8241, 5617, 8282, 5658, 8405, 5781, 8446, 5822, 20882, 14098, 20988, 14204, 21306, 14522, 21412, 14628, 21730, 14946, 21836, 15052, 8274, 5586, 8316, 5628, 8442, 5754, 8484, 5796, 8610, 5922, 8652, 5964, 8471, 5719, 8514, 5762, 8643, 5891, 8686, 5934, 8815, 6063, 8858, 6106, 24625, 16625, 24750, 16750, 25125, 17125, 25250, 17250, 25625, 17625, 25750, 17750, 12017, 8113, 12078, 8174, 12261, 8357, 12322, 8418, 12505, 8601, 12566, 8662, 24822, 16758, 24948, 16884, 25326, 17262, 25452, 17388, 25830, 17766, 25956, 17892, 12214, 8246, 12276, 8308, 12462, 8494, 12524, 8556, 12710, 8742, 12772, 8804, 12411, 8379, 12474, 8442, 12663, 8631, 12726, 8694, 12915, 8883, 12978, 8946, 22425, 18009, 22494, 18078, 22701, 18285, 22770, 18354, 22977, 18561, 23046, 18630, 1625, 1305, 1630, 1310, 1645, 1325, 1650, 1330, 1665, 1345, 1670, 1350, 22750, 18270, 22820, 18340, 23030, 18550, 23100, 18620, 23310, 18830, 23380, 18900, 1950, 1566, 1956, 1572, 1974, 1590, 1980, 1596, 1998, 1614, 2004, 1620, 2275, 1827, 2282, 1834, 2303, 1855, 2310, 1862, 2331, 1883, 2338, 1890, 23725, 19053, 23798, 19126, 24017, 19345, 24090, 19418, 24309, 19637, 24382, 19710, 2925, 2349, 2934, 2358, 2961, 2385, 2970, 2394, 2997, 2421, 3006, 2430, 24050, 19314, 24124, 19388, 24346, 19610, 24420, 19684, 24642, 19906, 24716, 19980, 3250, 2610, 3260, 2620, 3290, 2650, 3300, 2660, 3330, 2690, 3340, 2700, 3575, 2871, 3586, 2882, 3619, 2915, 3630, 2926, 3663, 2959, 3674, 2970, 25025, 20097, 25102, 20174, 25333, 20405, 25410, 20482, 25641, 20713, 25718, 20790, 4225, 3393, 4238, 3406, 4277, 3445, 4290, 3458, 4329, 3497, 4342, 3510, 25350, 20358, 25428, 20436, 25662, 20670, 25740, 20748, 25974, 20982, 26052, 21060, 4550, 3654, 4564, 3668, 4606, 3710, 4620, 3724, 4662, 3766, 4676, 3780, 4875, 3915, 4890, 3930, 4935, 3975, 4950, 3990, 4995, 4035, 5010, 4050, 27625, 22185, 27710, 22270, 27965, 22525, 28050, 22610, 28305, 22865, 28390, 22950, 6825, 5481, 6846, 5502, 6909, 5565, 6930, 5586, 6993, 5649, 7014, 5670, 27950, 22446, 28036, 22532, 28294, 22790, 28380, 22876, 28638, 23134, 28724, 23220, 7150, 5742, 7172, 5764, 7238, 5830, 7260, 5852, 7326, 5918, 7348, 5940, 7475, 6003, 7498, 6026, 7567, 6095, 7590, 6118, 7659, 6187, 7682, 6210, 34125, 27405, 34230, 27510, 34545, 27825, 34650, 27930, 34965, 28245, 35070, 28350, 13325, 10701, 13366, 10742, 13489, 10865, 13530, 10906, 13653, 11029, 13694, 11070, 34450, 27666, 34556, 27772, 34874, 28090, 34980, 28196, 35298, 28514, 35404, 28620, 13650, 10962, 13692, 11004, 13818, 11130, 13860, 11172, 13986, 11298, 14028, 11340, 13975, 11223, 14018, 11266, 14147, 11395, 14190, 11438, 14319, 11567, 14362, 11610, 40625, 32625, 40750, 32750, 41125, 33125, 41250, 33250, 41625, 33625, 41750, 33750, 19825, 15921, 19886, 15982, 20069, 16165, 20130, 16226, 20313, 16409, 20374, 16470, 40950, 32886, 41076, 33012, 41454, 33390, 41580, 33516, 41958, 33894, 42084, 34020, 20150, 16182, 20212, 16244, 20398, 16430, 20460, 16492, 20646, 16678, 20708, 16740, 20475, 16443, 20538, 16506, 20727, 16695, 20790, 16758, 20979, 16947, 21042, 17010, 31257, 26841, 31326, 26910, 31533, 27117, 31602, 27186, 31809, 27393, 31878, 27462, 2265, 1945, 2270, 1950, 2285, 1965, 2290, 1970, 2305, 1985, 2310, 1990, 31710, 27230, 31780, 27300, 31990, 27510, 32060, 27580, 32270, 27790, 32340, 27860, 2718, 2334, 2724, 2340, 2742, 2358, 2748, 2364, 2766, 2382, 2772, 2388, 3171, 2723, 3178, 2730, 3199, 2751, 3206, 2758, 3227, 2779, 3234, 2786, 33069, 28397, 33142, 28470, 33361, 28689, 33434, 28762, 33653, 28981, 33726, 29054, 4077, 3501, 4086, 3510, 4113, 3537, 4122, 3546, 4149, 3573, 4158, 3582, 33522, 28786, 33596, 28860, 33818, 29082, 33892, 29156, 34114, 29378, 34188, 29452, 4530, 3890, 4540, 3900, 4570, 3930, 4580, 3940, 4610, 3970, 4620, 3980, 4983, 4279, 4994, 4290, 5027, 4323, 5038, 4334, 5071, 4367, 5082, 4378, 34881, 29953, 34958, 30030, 35189, 30261, 35266, 30338, 35497, 30569, 35574, 30646, 5889, 5057, 5902, 5070, 5941, 5109, 5954, 5122, 5993, 5161, 6006, 5174, 35334, 30342, 35412, 30420, 35646, 30654, 35724, 30732, 35958, 30966, 36036, 31044, 6342, 5446, 6356, 5460, 6398, 5502, 6412, 5516, 6454, 5558, 6468, 5572, 6795, 5835, 6810, 5850, 6855, 5895, 6870, 5910, 6915, 5955, 6930, 5970, 38505, 33065, 38590, 33150, 38845, 33405, 38930, 33490, 39185, 33745, 39270, 33830, 9513, 8169, 9534, 8190, 9597, 8253, 9618, 8274, 9681, 8337, 9702, 8358, 38958, 33454, 39044, 33540, 39302, 33798, 39388, 33884, 39646, 34142, 39732, 34228, 9966, 8558, 9988, 8580, 10054, 8646, 10076, 8668, 10142, 8734, 10164, 8756, 10419, 8947, 10442, 8970, 10511, 9039, 10534, 9062, 10603, 9131, 10626, 9154, 47565, 40845, 47670, 40950, 47985, 41265, 48090, 41370, 48405, 41685, 48510, 41790, 18573, 15949, 18614, 15990, 18737, 16113, 18778, 16154, 18901, 16277, 18942, 16318, 48018, 41234, 48124, 41340, 48442, 41658, 48548, 41764, 48866, 42082, 48972, 42188, 19026, 16338, 19068, 16380, 19194, 16506, 19236, 16548, 19362, 16674, 19404, 16716, 19479, 16727, 19522, 16770, 19651, 16899, 19694, 16942, 19823, 17071, 19866, 17114, 56625, 48625, 56750, 48750, 57125, 49125, 57250, 49250, 57625, 49625, 57750, 49750, 27633, 23729, 27694, 23790, 27877, 23973, 27938, 24034, 28121, 24217, 28182, 24278, 57078, 49014, 57204, 49140, 57582, 49518, 57708, 49644, 58086, 50022, 58212, 50148, 28086, 24118, 28148, 24180, 28334, 24366, 28396, 24428, 28582, 24614, 28644, 24676, 28539, 24507, 28602, 24570, 28791, 24759, 28854, 24822, 29043, 25011, 29106, 25074, 10842, 19491, 27370, 8602, 16031, 9315, 9591, 9867, 675, 695, 715, 9450, 9730, 10010, 810, 834, 858, 945, 973, 1001, 9855, 10147, 10439, 1215, 1251, 1287, 9990, 10286, 10582, 1350, 1390, 1430, 1485, 1529, 1573, 10395, 10703, 11011, 1755, 1807, 1859, 10530, 11154, 1890, 1946, 2002, 2025, 2085, 2145, 11475, 11815, 12155, 2835, 2919, 3003, 11610, 11954, 12298, 2970, 3058, 3146, 3105, 3197, 3289, 14175, 14595, 15015, 5535, 5699, 5863, 14310, 14734, 15158, 5670, 5838, 6006, 5805, 5977, 6149, 16875, 17375, 17875, 8235, 8479, 8723, 17010, 17514, 18018, 8370, 8618, 8866, 8505, 8757, 9009, 18147, 18423, 18699, 1315, 1335, 1355, 18410, 18690, 18970, 1578, 1602, 1626, 1841, 1869, 1897, 19199, 19783, 2367, 2403, 2439, 19462, 19758, 20054, 2630, 2670, 2710, 2893, 2937, 2981, 20251, 20559, 20867, 3419, 3471, 3523, 20514, 20826, 21138, 3682, 3738, 3794, 3945, 4005, 4065, 22355, 22695, 23035, 5523, 5607, 5691, 22618, 22962, 23306, 5786, 5874, 5962, 6049, 6141, 6233, 27615, 28035, 28455, 10783, 10947, 11111, 27878, 28302, 28726, 11046, 11214, 11382, 11309, 11481, 11653, 32875, 33375, 33875, 16043, 16287, 16531, 33138, 33642, 34146, 16306, 16554, 16802, 16569, 16821, 17073, 26979, 27255, 27531, 1955, 1975, 1995, 27650, 27930, 2346, 2370, 2394, 2737, 2765, 2793, 28543, 28835, 29127, 3519, 3555, 3591, 28934, 29230, 29526, 3910, 3950, 3990, 4301, 4345, 4389, 30107, 30415, 30723, 5083, 5135, 5187, 30498, 30810, 31122, 5474, 5530, 5586, 5865, 5925, 5985, 33235, 33575, 33915, 8211, 8295, 8379, 33626, 33970, 34314, 8690, 8778, 8993, 9085, 9177, 41055, 41475, 41895, 16195, 16359, 41446, 41870, 42294, 16422, 16590, 16758, 16813, 16985, 17157, 48875, 49375, 49875, 23851, 24095, 24339, 49266, 49770, 50274, 24242, 24490, 24738, 24633, 24885, 25137, 8832, 640, 8960, 768, 896, 9344, 1152, 9472, 1280, 1408, 9856, 1664, 9984, 1792, 1920, 10880, 2688, 11008, 2816, 2944, 13440, 5248, 13568, 5376, 5504, 16000, 7808, 16128, 7936, 8064, 17664, 1280, 17920, 1536, 1792, 18688, 2304, 18944, 2560, 2816, 19712, 3328, 19968, 3584, 3840, 21760, 5376, 22016, 5632, 5888, 26880, 10496, 27136, 10752, 11008, 32000, 15616, 32256, 15872, 16128, 26496, 1920, 26880, 2304, 2688, 28032, 3456, 28416, 3840, 4224, 29568, 4992, 29952, 5376, 5760, 32640, 8064, 33024, 8448, 8832, 40320, 15744, 40704, 16128, 16512, 48000, 23424, 48384, 23808, 24192};
            // G	CAL11	CAL11	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
            int[] SuTsG1 = {9600, 14775, 9975, 14850, 10050, 10125, 15075, 10275, 15150, 10350, 10425, 15375, 10575, 15450, 10650, 10725, 13696, 21079, 14231, 21186, 14338, 14445, 21507, 14659, 21614, 14766, 14873, 21935, 15087, 22042, 15194, 15301};
            // G	CAL11	CAL2	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
            int[] SuTsG2 = {15225, 21721};
            // G	CAL12	CAL2	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
            int[] SuTsG3 = {15525, 22149};
            // G	CAL2	CAL2	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
            int[] SuTsG4 = {14925, 21293};
            // G	CAL12	CAL12	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
            int[] SuTsG5 = {10112, 15563, 10507, 15642, 10586, 10665, 15879, 10823, 15958, 10902, 10981, 16195, 11139, 16274, 11218, 11297, 16256, 25019, 16891, 25146, 17018, 17145, 25527, 17399, 25654, 17526, 17653, 26035, 17907, 26162, 18034, 18161};
            // G	CAL11	CAL3	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
            int[] SuTsG6 = {16037, 25781};
            // G	CAL12	CAL3	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
            int[] SuTsG7 = {16353, 26289};
            // G	CAL3	CAL3	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
            int[] SuTsG8 = {15721, 25273};
            // G	CAL1	CAL1	"إنشاء دورة اشتراك ف1 منقول من الوحدة المستبدلة"
            int[] SuTsG9 = {14129, 17313};
            // G	CAL11	CAL1	"إنشاء دورة اشتراك ف1 منقول من الوحدة المستبدلة"
            int[] SuTsG10 = {14413, 17661};
            // G	CAL12	CAL1	"إنشاء دورة اشتراك ف1 منقول من الوحدة المستبدلة"
            int[] SuTsG11 = {14697, 18009};
            // G	CAL10	CAL10	"إنشاء دورة اشتراك ف1 منقول من الوحدة المستبدلة"
            int[] SuTsG12 = {9088, 13987, 9443, 14058, 9514, 9585, 14271, 9727, 14342, 9798, 9869, 14555, 10011, 14626, 10082, 10153, 11136, 17139, 11571, 17226, 11658, 11745, 17487, 11919, 17574, 12006, 12093, 17835, 12267, 17922, 12354, 12441};
            // H	CAL13	CAL13	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"
            int[] SuTsG13 = {18176, 23075, 18531, 23146, 18602, 18673, 23359, 18815, 23430, 18886, 18957, 23643, 19099, 23714, 19170, 19241, 22272, 28275, 22707, 28362, 22794, 22881, 28623, 23055, 28710, 23142, 23229, 28971, 23403, 29058, 23490, 23577};
            // H	CAL13	CAL4	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"
            int[] SuTsG14 = {23217, 28449};
            // H	CAL15	CAL4	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"
            int[] SuTsG15 = {23785, 29145};
            // H	CAL4	CAL4	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"
            int[] SuTsG16 = {23501, 28797};
            // H	CAL15	CAL15	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
            int[] SuTsG17 = {20224, 25675, 20619, 25754, 20698, 20777, 25991, 20935, 26070, 21014, 21093, 26307, 21251, 26386, 21330, 21409, 32512, 41275, 33147, 41402, 33274, 33401, 41783, 33655, 41910, 33782, 33909, 42291, 34163, 42418, 34290, 34417};
            // H	CAL13	CAL6	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
            int[] SuTsG18 = {25833, 41529};
            // H	CAL15	CAL6	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
            int[] SuTsG19 = {26465, 42545};
            // H	CAL6	CAL6	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
            int[] SuTsG20 = {26149, 42037};
            // H	CAL14	CAL14	"إنشاء دورة اشتراك ف2 منقول من الوحدة المستبدلة"
            int[] SuTsG21 = {19200, 24375, 19575, 24450, 19650, 19725, 24675, 19875, 24750, 19950, 20025, 24975, 20175, 25050, 20250, 20325, 27392, 34775, 27927, 34882, 28034, 28141, 35203, 28355, 35310, 28462, 28569, 35631, 28783, 35738, 28890, 28997};
            // H	CAL13	CAL5	"إنشاء دورة اشتراك ف2 منقول من الوحدة المستبدلة"
            int[] SuTsG22 = {24525, 34989};
            // H	CAL15	CAL5	"إنشاء دورة اشتراك ف2 منقول من الوحدة المستبدلة"
            int[] SuTsG23 = {25125, 35845};
            // H	CAL5	CAL5	"إنشاء دورة اشتراك ف2 منقول من الوحدة المستبدلة"
            int[] SuTsG24 = {24825, 35417};
            // H+G	CAL16	CAL16	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"
            int[] SuTsG25 = {27264, 32163, 27619, 32234, 27690, 27761, 32447, 27903, 32518, 27974, 28045, 32731, 28187, 32802, 28258, 28329, 33408, 39411, 33843, 39498, 33930, 34017, 39759, 34191, 39846, 34278, 34365, 40107, 34539, 40194, 34626, 34713};
            // H+G	CAL16	CAL7	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"
            int[] SuTsG26 = {32305, 39585};
            // H+G	CAL17	CAL7	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"
            int[] SuTsG27 = {32589, 39933};
            // H+G	CAL7	CAL7	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"
            int[] SuTsG28 = {32873, 40281};
            // H+G	CAL17	CAL17	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
            int[] SuTsG29 = {28800, 33975, 29175, 34050, 29250, 29325, 34275, 29475, 34350, 29550, 29625, 34575, 29775, 34650, 29850, 29925, 41088, 48471, 41623, 48578, 41730, 41837, 48899, 42051, 49006, 42158, 42265, 49327, 42479, 49434, 42586, 42693};
            // H+G	CAL16	CAL8	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
            int[] SuTsG30 = {34125, 48685};
            // H+G	CAL17	CAL8	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
            int[] SuTsG31 = {34425, 49113};
            // H+G	CAL8	CAL8	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
            int[] SuTsG32 = {34725, 49541};
            // H+G	CAL18	CAL18	"إنشاء دورة اشتراك ف3 منقول من الوحدة المستبدلة"
            int[] SuTsG33 = {30336, 35787, 30731, 35866, 30810, 30889, 36103, 31047, 36182, 31126, 31205, 36419, 31363, 36498, 31442, 31521, 48768, 57531, 49403, 57658, 49530, 49657, 58039, 49911, 58166, 50038, 50165, 58547, 50419, 58674, 50546, 50673};
            // H+G	CAL16	CAL9	"إنشاء دورة اشتراك ف3 منقول من الوحدة المستبدلة"
            int[] SuTsG34 = {35945, 57785};
            // H+G	CAL17	CAL9	"إنشاء دورة اشتراك ف3 منقول من الوحدة المستبدلة"
            int[] SuTsG35 = {36261, 58293};
            // H+G	CAL9	CAL9	"إنشاء دورة اشتراك ف3 منقول من الوحدة المستبدلة"
            int[] SuTsG36 = {36577, 58801};
            #endregion

        #endregion


        if (RPCC.Contains(caseCode))
        {
            if (applyChangesToDatabase)//ApplyChangesToDatabaseFlag
            {

                var rprice = prices.FirstOrDefault(x => x.TrackingUnitModelId == runit.TrackingUnitModelId);
                var sprice = prices.FirstOrDefault(x => x.TrackingUnitModelId == sunit.TrackingUnitModelId);

                var rHfees= Math.Round((rprice.Host / 365), 3, MidpointRounding.AwayFromZero);
                var rGfees= Math.Round((rprice.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                var rFfees= Math.Round(((rprice.Host+rprice.Gprs) / 365), 3, MidpointRounding.AwayFromZero);
                var sHfees= Math.Round((sprice.Host / 365), 3, MidpointRounding.AwayFromZero);
                var sGfees= Math.Round((sprice.Gprs / 365), 3, MidpointRounding.AwayFromZero);
                var sFfees= Math.Round(((sprice.Host+sprice.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                //RuOs 
                switch (caseCode)
                    {
                                    case int code when RuOsG0.Contains(code):
                                        {
                                            // *	*	*	*
                                            break;
                                        }

                                    case int code when RuOsG1.Contains(code):
                                        {
                                            // G	RseDt	TsDt	"قيمة اشتراك ف1 مستحقة عن الدورة السابقة"
                                                var startDate = (DateOnly)rcurrentSubscription.SeDate;
                                                                var endDate = tsDate;
                                                                //var dailyFees = Math.Round((rprice.Gprs / 365), 3, MidpointRounding.AwayFromZero);

                                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                {
                                                                    LastPaidFees = SubPackageFees.GprsFees,
                                                                    CaseCode = caseCode,
                                                                    Description = string.Format(SubscriptionDescs.Desc10022, startDate, endDate),
                                                                    TrackingUnitId = runit.Id,
                                                                    SsDate = startDate,
                                                                    SeDate = endDate,
                                                                    DailyFees = rGfees
                                                                });
                                            break;
                                        }

                                    case int code when RuOsG2.Contains(code):
                                        {
                                            // H	RseDt	TsDt	"قيمة اشتراك ف2 مستحقة عن الدورة السابقة"
                                            var startDate = (DateOnly)rcurrentSubscription.SeDate;
                                                                var endDate = tsDate;
                                                
                                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                {
                                                                    LastPaidFees = SubPackageFees.HostFees,
                                                                    CaseCode = caseCode,
                                                                    Description = string.Format(SubscriptionDescs.Desc10023, startDate, endDate),
                                                                    TrackingUnitId = runit.Id,
                                                                    SsDate = startDate,
                                                                    SeDate = endDate,
                                                                    DailyFees = rHfees
                                                                });
                                            break;
                                        }
                                    
                                    case int code when RuOsG3.Contains(code):
                                        {
                                            // H+G	RseDt	TsDt	"قيمة اشتراك ف3 مستحقة عن الدورة السابقة"

                                            var startDate = (DateOnly)rcurrentSubscription.SeDate;
                                                                var endDate = tsDate;
                                                                
                                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                {
                                                                    LastPaidFees = SubPackageFees.FullFees,
                                                                    CaseCode = caseCode,
                                                                    Description = string.Format(SubscriptionDescs.Desc10024, startDate, endDate),
                                                                    TrackingUnitId = runit.Id,
                                                                    SsDate = startDate,
                                                                    SeDate = endDate,
                                                                    DailyFees = rFfees
                                                                });
                                            break;
                                        }

                                    case int code when RuOsG4.Contains(code):
                                        {
                                            // G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"
                                                    var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.GprsFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10010, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rGfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));
                                            break;
                                        }
                                    case int code when RuOsG5.Contains(code):
                                        {
                                            // H	*	TsDt	"تحديث - إنهاء دورة اشتراك ف2 السابقة"
                                            var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                                                            
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.HostFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10011, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rHfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));
                                            break;
                                        }
                                    case int code when RuOsG6.Contains(code):
                                        {
                                            // H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"

                                                                var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                                                
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.FullFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10012, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rFfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));
                                            break;
                                        }

                                    case int code when RuOsG9.Contains(code):
                                        {
                                            // G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة الغير مسددة"
                                                            var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                                                        
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.GprsFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10034, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rGfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));
                                            break;
                                        }

                                    case int code when RuOsG8.Contains(code):
                                        {
                                            // H	*	TsDt	"تحديث - إنهاء دورة اشتراك ف2 السابقة الغير مسددة"
                                                    var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                                                        
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.HostFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10035, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rHfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));
                                            break;
                                        }               

                                    case int code when RuOsG9.Contains(code):
                                        {
                                            // H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة الغير مسددة"

                                            var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                                                        
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.FullFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10036, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rFfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));
                                            break;
                                        }

                                    case int code when RuOsG10.Contains(code):
                                        {
                                            // G	*	TsDt	"تحديث – نقل بقية الاشتراك ف1 المسدد إلى الوحدة البديلة"

                                                    var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                                                            
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.GprsFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10037, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rGfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));
                                            break;
                                        }

                                    case int code when RuOsG11.Contains(code):
                                        {
                                            // H	*	TsDt	"تحديث – نقل بقية الاشتراك ف2 المسدد إلى الوحدة البديلة"

                                            var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.HostFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10038, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rHfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));
                                            break;
                                        }

                                    case int code when RuOsG12.Contains(code):
                                        {
                                            // H+G	*	TsDt	"تحديث – نقل بقية الاشتراك ف3 المسدد إلى الوحدة البديلة"

                                            var startDate = rcurrentSubscription.SsDate;
                                                                var endDate = tsDate;
                                                
                                                                rcurrentSubscription.LastPaidFees = SubPackageFees.FullFees;
                                                                rcurrentSubscription.CaseCode = caseCode;
                                                                rcurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10039, startDate, endDate);
                                                                rcurrentSubscription.SeDate = endDate;
                                                                rcurrentSubscription.DailyFees = rFfees;
                                                                rcurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(rcurrentSubscription));

                                            break;
                                        }
                    }
                //SuOs
                switch (caseCode)
                    {
                                case int code when SuOsG0.Contains(code):
                                    {
                                        // *	*	*	*
                                        break;
                                    }
                                
                                case int code when SuOsG1.Contains(code):
                                    {
                                        // G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف1 السابقة"

                                                        var startDate = scurrentSubscription.SsDate;
                                                            var endDate = tsDate;
                                                    
                                                            scurrentSubscription.LastPaidFees = SubPackageFees.GprsFees;
                                                            scurrentSubscription.CaseCode = caseCode;
                                                            scurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10010, startDate, endDate);
                                                            scurrentSubscription.SeDate = endDate;
                                                            scurrentSubscription.DailyFees = sGfees;
                                                            scurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(scurrentSubscription));
                                        break;
                                    }

                                case int code when SuOsG2.Contains(code):
                                    {
                                        // H	*	TsDt	"تحديث – إنهاء دورة اشتراك ف2 السابقة"


                                                var startDate = scurrentSubscription.SsDate;
                                                            var endDate = tsDate;
                                
                                                            scurrentSubscription.LastPaidFees = SubPackageFees.HostFees;
                                                            scurrentSubscription.CaseCode = caseCode;
                                                            scurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10011, startDate, endDate);
                                                            scurrentSubscription.SeDate = endDate;
                                                            scurrentSubscription.DailyFees = sHfees;
                                                            scurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(scurrentSubscription));
                                        break;
                                    }

                                case int code when SuOsG3.Contains(code):
                                    {
                                        // H+G	*	TsDt	"تحديث - إنهاء دورة اشتراك ف3 السابقة"

                                        var startDate = scurrentSubscription.SsDate;
                                                            var endDate = tsDate;
                                                            var dailyFees = Math.Round(((sprice.Host + sprice.Gprs) / 365), 3, MidpointRounding.AwayFromZero);

                                                            scurrentSubscription.LastPaidFees = SubPackageFees.FullFees;
                                                            scurrentSubscription.CaseCode = caseCode;
                                                            scurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10012, startDate, endDate);
                                                            scurrentSubscription.SeDate = endDate;
                                                            scurrentSubscription.DailyFees = dailyFees;
                                                            scurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(scurrentSubscription));
                                        break;
                                    }
                                
                                case int code when SuOsG4.Contains(code):
                                    {
                                        // G	*	TsDt	"تحديث - قيمة اشتراك ف1 مستحقة عن دورة سابقة غير مسددة"

                                                var startDate = scurrentSubscription.SsDate;
                                                            var endDate = tsDate;

                                                            scurrentSubscription.LastPaidFees = SubPackageFees.GprsFees;
                                                            scurrentSubscription.CaseCode = caseCode;
                                                            scurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10013, startDate, endDate);
                                                            scurrentSubscription.SeDate = endDate;
                                                            scurrentSubscription.DailyFees = sGfees;
                                                            scurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(scurrentSubscription));

                                        break;
                                    }

                                case int code when SuOsG5.Contains(code):
                                    {
                                        // H	*	TsDt	"تحديث – قيمة اشتراك ف2 مستحقة عن دورة سابقة غير مسددة"
                                                var startDate = scurrentSubscription.SsDate;
                                                            var endDate = tsDate;
                                                        //  var dailyFees = Math.Round((sprice.Host / 365), 3, MidpointRounding.AwayFromZero);

                                                            scurrentSubscription.LastPaidFees = SubPackageFees.HostFees;
                                                            scurrentSubscription.CaseCode = caseCode;
                                                            scurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10014, startDate, endDate);
                                                            scurrentSubscription.SeDate = endDate;
                                                            scurrentSubscription.DailyFees = sHfees;
                                                            scurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(scurrentSubscription));
                                        break;
                                    }
                            
                                case int code when SuOsG6.Contains(code):
                                    {
                                        // H+G	*	TsDt	"تحديث – قيمة اشتراك ف3 مستحقة عن دورة سابقة غير مسددة"
                                                        var startDate = scurrentSubscription.SsDate;
                                                            var endDate = tsDate;

                                                            scurrentSubscription.LastPaidFees = SubPackageFees.FullFees;
                                                            scurrentSubscription.CaseCode = caseCode;
                                                            scurrentSubscription.Description = string.Format(SubscriptionDescs.Desc10015, startDate, endDate);
                                                            scurrentSubscription.SeDate = endDate;
                                                            scurrentSubscription.DailyFees = sFfees;
                                                            scurrentSubscription.AddDomainEvent(new SubscriptionUpdatedEvent(scurrentSubscription));
                                        break;
                                    }
                    }
                //SuRs
                switch (caseCode)
                    {
                                        case int code when SuRsG0.Contains(code):
                                            {
                                                // *	*	*	*
                                                break;
                                            }
                                        case int code when SuRsG1.Contains(code):
                                            {
                                                // G	TsDt	CAL11	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 السابق"

                                                var startDate = tsDate;
                                                int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                                                var RSFees = days * rHfees;
                                                var ODays = (int)Math.Ceiling(RSFees / sGfees);
                                                var endDate = tsDate.AddDays(ODays);

                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10001, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sGfees
                                                                    });

                                        
                                                break;
                                            }
                                        case int code when SuRsG2.Contains(code):
                                            {
                                                // G	TsDt	CAL12	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 السابق"

                                                var startDate = tsDate;
                                                int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                                                var RSFees = days * rFfees;
                                                var ODays = (int)Math.Ceiling(RSFees / sGfees);
                                                var endDate = tsDate.AddDays(ODays);

                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10002, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sGfees
                                                                    });


                                                break;
                                            }
                                        case int code when SuRsG3.Contains(code):
                                            {
                                                // H	TsDt	CAL13	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 السابق"


                                                var startDate = tsDate;
                                                int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                                                var RSFees = days * rGfees;
                                                var ODays = (int)Math.Ceiling(RSFees / sHfees);
                                                var endDate = tsDate.AddDays(ODays);


                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.HostFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10003, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sHfees
                                                                    });


                                                break;
                                            }
                                        case int code when SuRsG4.Contains(code):
                                            {
                                                // H	TsDt	CAL15	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 السابق"

                                                var startDate = tsDate;
                                                int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                                                var RSFees = days * rFfees;
                                                var ODays = (int)Math.Ceiling(RSFees / sHfees);
                                                var endDate = tsDate.AddDays(ODays);


                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.HostFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10004, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sHfees
                                                                    });

                                                break;
                                            }
                                        case int code when SuRsG5.Contains(code):
                                            {
                                                // H+G	TsDt	CAL16	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 السابق"

                                                var startDate = tsDate;
                                                int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                                                var RSFees = days * rGfees;
                                                var ODays = (int)Math.Ceiling(RSFees / sFfees);
                                                var endDate = tsDate.AddDays(ODays);

                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.FullFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10005, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sFfees
                                                                    });

                                                break;
                                            }
                                        case int code when SuRsG6.Contains(code):
                                            {
                                                // H+G	TsDt	CAL17	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 السابق"

                                                var startDate = tsDate;
                                                int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                                                var RSFees = days * rHfees;
                                                var ODays = (int)Math.Ceiling(RSFees / sFfees);
                                                var endDate = tsDate.AddDays(ODays);


                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.FullFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10006, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sFfees
                                                                    });

                                                break;
                                            }
                                        case int code when SuRsG7.Contains(code):
                                            {
                                                // G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف1 بمدة سنة واحدة من تاريخ التركيب"

                                                var startDate = tsDate;
                                                                    var endDate = tsDate.AddDays(365);
                
                                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10007, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sGfees
                                                                    });
                                                break;
                                            }
                                        case int code when SuRsG8.Contains(code):
                                            {
                                                // H	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف2 بمدة سنة واحدة من تاريخ التركيب"

                                                var startDate = tsDate;
                                                                    var endDate = tsDate.AddDays(365);

                                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.HostFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10008, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sHfees
                                                                    });

                                                break;
                                            }
                                        case int code when SuRsG9.Contains(code):
                                            {
                                                // H+G	TsDt	TsDt + 1 Y	"أول دورة اشتراك للوحدة – اشتراك ف3 بمدة سنة واحدة من تاريخ التركيب"
                                                var startDate = tsDate;
                                                                    var endDate = tsDate.AddDays(365);

                                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.FullFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10009, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sFfees
                                                                    });
                                                break;
                                            }
                                        case int code when SuRsG10.Contains(code):
                                            {
                                                // G	TsDt	LD	"دورة اشتراك ف1 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"

                                                var startDate = tsDate;
                                                                    var endDate = DateOnly.FromDateTime(new DateTime(tsDate.Year, 12, 31));
                                    
                                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10016, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sGfees
                                                                    });
                                                break;
                                            }
                                        case int code when SuRsG11.Contains(code):
                                            {
                                                // H	TsDt	LD	"دورة اشتراك ف2 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                                                var startDate = tsDate;
                                                                    var endDate = DateOnly.FromDateTime(new DateTime(tsDate.Year, 12, 31));

                                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.HostFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10018, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sHfees
                                                                    });
                                                break;
                                            }
                                        case int code when SuRsG12.Contains(code):
                                            {
                                                // H+G	TsDt	LD	"دورة اشتراك ف3 من تاريخ التفعيل حتى آخر يوم بالسنة الحالية"
                                                    var startDate = tsDate;
                                                                    var endDate = DateOnly.FromDateTime(new DateTime(tsDate.Year, 12, 31));

                                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.FullFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10020, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sFfees
                                                                    });
                                                break;
                                            }
                                        case int code when SuRsG13.Contains(code):
                                            {
                                                // G	TsDt	SseDt	"دورة اشتراك ف1 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"

                                        var startDate = tsDate;
                                                                    var endDate = (DateOnly)ssubEndDate;
                    
                                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10017, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sGfees
                                                                    });
                                                
                                                break;
                                            }                     
                                        case int code when SuRsG14.Contains(code):
                                            {
                                                // H	TsDt	SseDt	"دورة اشتراك ف2 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"

                                                        var startDate = tsDate;
                                                                    var endDate = (DateOnly)ssubEndDate;

                                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                                    {
                                                                        LastPaidFees = SubPackageFees.HostFees,
                                                                        CaseCode = caseCode,
                                                                        Description = string.Format(SubscriptionDescs.Desc10019, startDate, endDate),
                                                                        TrackingUnitId = sunit.Id,
                                                                        SsDate = startDate,
                                                                        SeDate = endDate,
                                                                        DailyFees = sHfees
                                                                    });
                                                break;
                                            }                       
                                        case int code when SuRsG15.Contains(code):
                                        {
                                            // H+G	TsDt	SseDt	"دورة اشتراك ف3 من تاريخ التفعيل حتى تاريخ نهاية الدورة السابقة الغير مسددة"

                                            var startDate = tsDate;
                                            var endDate = (DateOnly)ssubEndDate;
                                                                
                                                                servcieLog.Subscriptions?.Add(new Subscription
                                                                {
                                                                    LastPaidFees = SubPackageFees.FullFees,
                                                                    CaseCode = caseCode,
                                                                    Description = string.Format(SubscriptionDescs.Desc10021, startDate, endDate),
                                                                    TrackingUnitId = sunit.Id,
                                                                    SsDate = startDate,
                                                                    SeDate = endDate,
                                                                    DailyFees = sFfees
                                                                });
                                                                
                                            break;
                                        }
                    }
                //SuTs
                switch (caseCode)
    {
            case int code when SuTsG0.Contains(code):
                {
                    // *	*	*	*
                    break;
                }
            case int code when SuTsG1.Contains(code):
                {
                    // G	CAL11	CAL11	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"

                    var startDate = tsDate;
                    int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rHfees;
                    var ODays = (int)Math.Ceiling(RSFees / sGfees);
                    var endDate = tsDate.AddDays(ODays);

                                servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10025, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });


                    break;
                }
            case int code when SuTsG2.Contains(code):
                {
                    // G	CAL11	CAL2	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"

                    //CAL11
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rHfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sGfees);
                    var startDate = tsDate.AddDays(xODays);

                    //CAL2
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rHfees;
                    var ODays = (int)Math.Ceiling(RSFees / sGfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);

                                servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10025, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });
                    break;
                }
            case int code when SuTsG3.Contains(code):
                {
                    // G	CAL12	CAL2	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"


                    //CAL12
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rFfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sGfees);
                    var startDate = tsDate.AddDays(xODays);


                    //CAL2
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rHfees;
                    var ODays = (int)Math.Ceiling(RSFees / sGfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10025, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });

                    break;
                }
            case int code when SuTsG4.Contains(code):
                {
                    // G	CAL2	CAL2	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"

                    var startDate = ssubEndDate;
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rHfees;
                    var ODays = (int)Math.Ceiling(RSFees / sGfees);
                    var endDate = ((DateOnly)ssubEndDate).AddDays(ODays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10025, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });

                    break;
                }
            case int code when SuTsG5.Contains(code):
                {
                    // G	CAL12	CAL12	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"

                    var startDate = tsDate;
                    int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rFfees;
                    var ODays = (int)Math.Ceiling(RSFees / sGfees);
                    var endDate = tsDate.AddDays(ODays);

                                                                        servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10026, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });

                break;
                }
            case int code when SuTsG6.Contains(code):
                {
                    // G	CAL11	CAL3	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"

                    //  CAL11
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rHfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sGfees);
                    var startDate = tsDate.AddDays(xODays);


                    //  CAL3
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rFfees;
                    var ODays = (int)Math.Ceiling(RSFees / sGfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10026, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });


                    break;
                }
            case int code when SuTsG7.Contains(code):
                {
                    // G	CAL12	CAL3	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"

                    //CAL12
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rFfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sGfees);
                    var startDate = tsDate.AddDays(xODays);

                    //  CAL3
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rFfees;
                    var ODays = (int)Math.Ceiling(RSFees / sGfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10026, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });
                    break;
                }
            case int code when SuTsG8.Contains(code):
                {
                    // G	CAL3	CAL3	"إنشاء دورة اشتراك ف1 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"

                    var startDate = ssubEndDate;
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rFfees;
                    var ODays = (int)Math.Ceiling(RSFees / sGfees);
                    var endDate = ((DateOnly)ssubEndDate).AddDays(ODays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10026, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });

                break;
                }
            case int code when SuTsG9.Contains(code):
                {
                    // G	CAL1	CAL1	"إنشاء دورة اشتراك ف1 منقول من الوحدة المستبدلة"

                    var startDate = ssubEndDate;
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)ssubEndDate).AddDays(Odays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10027, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });
                    break;
                }
            case int code when SuTsG10.Contains(code):
                {
                    // G	CAL11	CAL1	"إنشاء دورة اشتراك ف1 منقول من الوحدة المستبدلة"

                    //  CAL11
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rHfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sGfees);
                    var startDate = tsDate.AddDays(xODays);

                    //CAL1
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)startDate).AddDays(Odays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10027, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });

                    break;
                }
            case int code when SuTsG11.Contains(code):
                {
                    // G	CAL12	CAL1	"إنشاء دورة اشتراك ف1 منقول من الوحدة المستبدلة"

                    //CAL12
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rFfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sGfees);
                    var startDate = tsDate.AddDays(xODays);

                    //CAL1
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)startDate).AddDays(Odays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10027, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });

                    break;
                }
            case int code when SuTsG12.Contains(code):
                {
                    // G	CAL10	CAL10	"إنشاء دورة اشتراك ف1 منقول من الوحدة المستبدلة"

                    var startDate = tsDate;
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = tsDate.AddDays(Odays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.GprsFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10027, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sGfees
                                                    });


                    break;
                }
            case int code when SuTsG13.Contains(code):
                {
                    // H	CAL13	CAL13	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"


                    var startDate = tsDate;
                    int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rGfees;
                    var ODays = (int)Math.Ceiling(RSFees / sHfees);
                    var endDate = tsDate.AddDays(ODays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10028, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });


                    break;
                }
            case int code when SuTsG14.Contains(code):
                {
                    // H	CAL13	CAL4	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"

                    //CAL14
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rGfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sHfees);
                    var startDate = tsDate.AddDays(xODays);

                    //CAL4
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rGfees;
                    var ODays = (int)Math.Ceiling(RSFees / sHfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);


                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10028, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });

                    break;
                }
            case int code when SuTsG15.Contains(code):
                {
                    // H	CAL15	CAL4	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"

                    //	CAL15
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rFfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sHfees);
                    var startDate = tsDate.AddDays(xODays);

                    //	CAL4
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rGfees;
                    var ODays = (int)Math.Ceiling(RSFees / sHfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);



                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10028, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });
                    break;
                }
            case int code when SuTsG16.Contains(code):
                {
                    // H	CAL4	CAL4	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"

                    var startDate = ssubEndDate;
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rGfees;
                    var ODays = (int)Math.Ceiling(RSFees / sHfees);
                    var endDate = ((DateOnly)ssubEndDate).AddDays(ODays);




                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10028, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });
                    break;
                }
            case int code when SuTsG17.Contains(code):
                {
                    // H	CAL15	CAL15	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"

                    var startDate = tsDate;
                    int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rFfees;
                    var ODays = (int)Math.Ceiling(RSFees / sHfees);
                    var endDate = tsDate.AddDays(ODays);


                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                         Description = string.Format(SubscriptionDescs.Desc10029, startDate, endDate),
                                                         TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });

                    break;
                }
            case int code when SuTsG18.Contains(code):
                {
                    // H	CAL13	CAL6	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"

                    //CAL13
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rGfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sHfees);
                    var startDate = tsDate.AddDays(xODays);
                    //CAL6
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rFfees;
                    var ODays = (int)Math.Ceiling(RSFees / sHfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);



                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                         Description = string.Format(SubscriptionDescs.Desc10029, startDate, endDate),
                                                         TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });


                    break;
                }
            case int code when SuTsG19.Contains(code):
                {
                    // H	CAL15	CAL6	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"
                    
                    //CAL15
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rFfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sHfees);
                    var startDate = tsDate.AddDays(xODays);

                    //CAL6
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rFfees;
                    var ODays = (int)Math.Ceiling(RSFees / sHfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);

                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.HostFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10029, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sHfees
                    });

                    break;
                }
            case int code when SuTsG20.Contains(code):
                {
                    // H	CAL6	CAL6	"إنشاء دورة اشتراك ف2 بناء على القيمة المتبقية من اشتراك ف3 المنقول من الوحدة المستبدلة"

                    var startDate = ssubEndDate;
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rFfees;
                    var ODays = (int)Math.Ceiling(RSFees / sHfees);
                    var endDate = ((DateOnly)ssubEndDate).AddDays(ODays);

                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10029, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });

                    break;
                }
            case int code when SuTsG21.Contains(code):
                {
                    // H	CAL14	CAL14	"إنشاء دورة اشتراك ف2 منقول من الوحدة المستبدلة"

                    var startDate = tsDate;
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)tsDate).AddDays(Odays);


                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10030, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });
                    break;
                }
            case int code when SuTsG22.Contains(code):
                {
                    // H	CAL13	CAL5	"إنشاء دورة اشتراك ف2 منقول من الوحدة المستبدلة"

                    //CAL13
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rGfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sHfees);
                    var startDate = tsDate.AddDays(xODays);

                    //CAL5
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)startDate).AddDays(Odays);


                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10030, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });

                    break;
                }
            case int code when SuTsG23.Contains(code):
                {
                    // H	CAL15	CAL5	"إنشاء دورة اشتراك ف2 منقول من الوحدة المستبدلة"

                    //CAL15
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rFfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sHfees);
                    var startDate = tsDate.AddDays(xODays);

                    //CAL5
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)startDate).AddDays(Odays);



                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                        Description = string.Format(SubscriptionDescs.Desc10030, startDate, endDate),
                                                        TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });
                    break;
                }
            case int code when SuTsG24.Contains(code):
                {
                    // H	CAL5	CAL5	"إنشاء دورة اشتراك ف2 منقول من الوحدة المستبدلة"


                    var startDate = ssubEndDate;
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)ssubEndDate).AddDays(Odays);





                                                    servcieLog.Subscriptions?.Add(new Subscription
                                                    {
                                                        LastPaidFees = SubPackageFees.HostFees,
                                                        CaseCode = caseCode,
                                                         Description = string.Format(SubscriptionDescs.Desc10030, startDate, endDate),
                                                         TrackingUnitId = sunit.Id,
                                                        SsDate = (DateOnly)startDate,
                                                        SeDate = endDate,
                                                        DailyFees = sHfees
                                                    });
                    break;
                }
            case int code when SuTsG25.Contains(code):
                {
                    // H+G	CAL16	CAL16	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"

                    var startDate = tsDate;
                    int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rGfees;
                    var ODays = (int)Math.Ceiling(RSFees / sFfees);
                    var endDate = tsDate.AddDays(ODays);



                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10031, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });
                    break;
                }
            case int code when SuTsG26.Contains(code):
                {
                    // H+G	CAL16	CAL7	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"

                    //	CAL16
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rGfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sFfees);
                    var startDate = tsDate.AddDays(xODays);

                    //	CAL7
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rGfees;
                    var ODays = (int)Math.Ceiling(RSFees / sFfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);



                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                         Description = string.Format(SubscriptionDescs.Desc10031, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });

                    break;
                }
            case int code when SuTsG27.Contains(code):
                {
                    // H+G	CAL17	CAL7	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"

                    //	CAL17
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rHfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sFfees);
                    var startDate = tsDate.AddDays(xODays);

                    //	CAL7
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rGfees;
                    var ODays = (int)Math.Ceiling(RSFees / sFfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);


                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                         Description = string.Format(SubscriptionDescs.Desc10031, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });


                    break;
                }
            case int code when SuTsG28.Contains(code):
                {
                    // H+G	CAL7	CAL7	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف1 المنقول من الوحدة المستبدلة"

                    var startDate = ssubEndDate;
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rGfees;
                    var ODays = (int)Math.Ceiling(RSFees / sFfees);
                    var endDate = ((DateOnly)ssubEndDate).AddDays(ODays);


                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                         Description = string.Format(SubscriptionDescs.Desc10031, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });

                    break;
                }
            case int code when SuTsG29.Contains(code):
                {
                    // H+G	CAL17	CAL17	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"

                    var startDate = tsDate;
                    int days = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rHfees;
                    var ODays = (int)Math.Ceiling(RSFees / sFfees);
                    var endDate = tsDate.AddDays(ODays);


                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                         Description = string.Format(SubscriptionDescs.Desc10032, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });

                    break;
                }
            case int code when SuTsG30.Contains(code):
                {
                    // H+G	CAL16	CAL8	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"

                    //	CAL16
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rGfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sFfees);
                    var startDate = tsDate.AddDays(xODays);
                    //	CAL8
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rHfees;
                    var ODays = (int)Math.Ceiling(RSFees / sFfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);


                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10032, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });


                    break;
                }
            case int code when SuTsG31.Contains(code):
                {
                    // H+G	CAL17	CAL8	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"

                    //	CAL17
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rHfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sFfees);
                    var startDate = tsDate.AddDays(xODays);

                    //	CAL8
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rHfees;
                    var ODays = (int)Math.Ceiling(RSFees / sFfees);
                    var endDate = ((DateOnly)startDate).AddDays(ODays);


                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10032, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });

                    break;
                }
            case int code when SuTsG32.Contains(code):
                {
                    // H+G	CAL8	CAL8	"إنشاء دورة اشتراك ف3 بناء على القيمة المتبقية من اشتراك ف2 المنقول من الوحدة المستبدلة"
                    var	startDate = ssubEndDate;
                    int days = (((DateOnly)ssubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var RSFees = days * rHfees;
                    var ODays = (int)Math.Ceiling(RSFees / sFfees);
                    var endDate = ((DateOnly)ssubEndDate).AddDays(ODays);


                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10032, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });

                    break;
                }
            case int code when SuTsG33.Contains(code):
                {
                    // H+G	CAL18	CAL18	"إنشاء دورة اشتراك ف3 منقول من الوحدة المستبدلة"

                    var startDate = tsDate;
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)tsDate).AddDays(Odays);


                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10033, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });
                    
                    break;
                }
            case int code when SuTsG34.Contains(code):
                {
                    // H+G	CAL16	CAL9	"إنشاء دورة اشتراك ف3 منقول من الوحدة المستبدلة"

                    //	CAL16
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rGfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sFfees);
                    var startDate = tsDate.AddDays(xODays);
                    //	CAL9
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)startDate).AddDays(Odays);



                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10033, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });

                    break;
                }
            case int code when SuTsG35.Contains(code):
                {
                    // H+G	CAL17	CAL9	"إنشاء دورة اشتراك ف3 منقول من الوحدة المستبدلة"

                    //	CAL17
                    int xdays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var xRSFees = xdays * rHfees;
                    var xODays = (int)Math.Ceiling(xRSFees / sFfees);
                    var startDate = tsDate.AddDays(xODays);

                    //	CAL9
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)startDate).AddDays(Odays);


                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10033, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });

                    break;
                }
            case int code when SuTsG36.Contains(code):
                {
                    // H+G	CAL9	CAL9	"إنشاء دورة اشتراك ف3 منقول من الوحدة المستبدلة"

                    var startDate = ssubEndDate;
                    int Odays = (((DateOnly)rsubEndDate).ToDateTime(TimeOnly.MinValue) - tsDate.ToDateTime(TimeOnly.MinValue)).Days;
                    var endDate = ((DateOnly)ssubEndDate).AddDays(Odays);
                    
                    servcieLog.Subscriptions?.Add(new Subscription
                    {
                        LastPaidFees = SubPackageFees.FullFees,
                        CaseCode = caseCode,
                        Description = string.Format(SubscriptionDescs.Desc10033, startDate, endDate),
                        TrackingUnitId = sunit.Id,
                        SsDate = (DateOnly)startDate,
                        SeDate = endDate,
                        DailyFees = sFfees
                    });
                    break;
                }
    }

                //AddWialonTasks
                if (!(bool)runit.IsOnWialon && !(bool)sunit.IsOnWialon)
                    {
                        servcieLog.WialonTasks.Add(new WialonTask()
                            {
                                TrackingUnitId = sunit.Id,
                                WialonAPIAction = WialonAPIAction.RemoveFromWialon,
                                Description = string.Format("أضف الوحدة ({0}) إلى منصة ويلون.", sunit.SNo),
                                ExcDate = tsDate,
                                IsExecuted = false,
                            });
                    }
                else if (!(bool)runit.IsOnWialon && (bool)sunit.IsOnWialon)
                        {
                            servcieLog.WialonTasks.Add(new WialonTask()
                                    {
                                        TrackingUnitId = sunit.Id,
                                        WialonAPIAction = WialonAPIAction.UpdateOnWialon,
                                        Description = string.Format(" حدث بيانات الوحدة البديلة ({0}) ببيانات الوحدة المستبدلة  ({1}) على منصة ويلون.", sunit.SNo, runit.SNo),
                                        ExcDate = tsDate,
                                        IsExecuted = false,
                                    });
                        }
                else if ((bool)runit.IsOnWialon && !(bool)sunit.IsOnWialon)
                        {
                            servcieLog.WialonTasks.Add(new WialonTask()
                                    {
                                        TrackingUnitId = runit.Id,
                                        WialonAPIAction = WialonAPIAction.RemoveFromWialon,
                                        Description = string.Format(" حذف الوحدة ({0}) من منصة ويلون.", runit.SNo),
                                        ExcDate = tsDate,
                                        IsExecuted = true,
                                    });
                            servcieLog.WialonTasks.Add(new WialonTask()
                                    {
                                        TrackingUnitId = sunit.Id,
                                        WialonAPIAction = WialonAPIAction.RemoveFromWialon,
                                        Description = string.Format("أضف الوحدة ({0}) إلى منصة ويلون.", sunit.SNo),
                                        ExcDate = tsDate,
                                        IsExecuted = true,
                                    });
                            servcieLog.WialonTasks.Add(new WialonTask()
                                    {
                                        TrackingUnitId = runit.Id,
                                        WialonAPIAction = WialonAPIAction.UpdateOnWialon,
                                        Description = string.Format(" حدث بيانات الوحدة المستبدلة ({0}) ببيانات الوحدة  البديلة ({1}) على منصة ويلون.", runit.SNo, sunit.SNo),
                                        ExcDate = tsDate,
                                        IsExecuted = false,
                                    });
                        }
                else
                        {
                            servcieLog.WialonTasks.Add(new WialonTask()
                                    {
                                        TrackingUnitId = runit.Id,
                                        WialonAPIAction = WialonAPIAction.RemoveFromWialon,
                                        Description = string.Format(" حذف الوحدة ({0}) من منصة ويلون.", runit.SNo),
                                        ExcDate = tsDate,
                                        IsExecuted = true,
                                    });
                            servcieLog.WialonTasks.Add(new WialonTask()
                                    {
                                        TrackingUnitId = runit.Id,
                                        WialonAPIAction = WialonAPIAction.UpdateOnWialon,
                                        Description = string.Format(" حدث بيانات الوحدة المستبدلة ({0}) ببيانات الوحدة  البديلة ({1}) على منصة ويلون.", runit.SNo, sunit.SNo),
                                        ExcDate = tsDate,
                                        IsExecuted = false,
                                    });
                        }
                                        
                if (targetStatus == 384 || targetStatus == 128)
                            {
                                servcieLog.WialonTasks.Add(new WialonTask()
                                                {
                                                    TrackingUnitId = sunit.Id,
                                                    WialonAPIAction = WialonAPIAction.ActivateOnWialon,
                                                    Description = string.Format(" فعل الوحدة ({0}) على منصة ويلون.", sunit.SNo),
                                                    ExcDate = tsDate,
                                                    IsExecuted = false,
                                                });
                            }
                else
            {
                 servcieLog.WialonTasks.Add(new WialonTask()
                                {
                                    TrackingUnitId = sunit.Id,
                                    WialonAPIAction = WialonAPIAction.DeactivateOnWialon,
                                    Description = string.Format(" إلغاء تفعيل الوحدة ({0}) على منصة ويلون.", sunit.SNo),
                                    ExcDate = tsDate,
                                    IsExecuted = false,
                                });
            }
                           

        
                
                
                return caseCode.ToString();
            }
            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(string.Format("العملية - {0} - كود الحالة {1}", servcieLog.Description, caseCode.ToString()));

                return stringBuilder.ToString();
            }
        }
        else
            throw new NotImplementedException($"{caseCode} Not Implemented Case code");
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
                    var lastInvoice = await cnx.Invoices.Where(i => i.InvoiceNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.InvoiceNo).FirstOrDefaultAsync();
                    if (lastInvoice != null)
                    {
                        var match = Regex.Match(lastInvoice.InvoiceNo, @$"^{prefix}(\d+)$");
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
