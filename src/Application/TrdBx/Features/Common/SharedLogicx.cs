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
        //return sp == null ? 0.0m : sp.Price;
        return sp?.Price ?? 0.0m;
        
    }
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
    #region
    private static readonly int[] APCC = { 384	,489,	425	,490	,426	,491	,427	,469,	405	,470	,406,	471	,407,	457,	393	,458,	394	,459	,395,	453	,389	,454,	390	,455,	391	,461	,397	,462	,398	,463	,399}; //	All			
    private static readonly int[] AHPCC = { 256	,341	,277,	342	,278	,343,	279	,381	,317	,382,	318,	383,	319	,329,	265,	330,	266,	331,	267,	325	,261	,326	,262	,327,	263	,333	,269	,334	,270	,335	,271 };//	All		
    private static readonly int[] AGPCC = {128	,233	,169	,234,	17,	342,	278,	343	,279	,381	,317,	382,	318	,383	,319,	329,	265,	330,	266	,331	,267	,325	,261	,326	,262	,327	,263	,333	,269	,334	,270	,335	,271};//All								
    private static readonly int[] DPCC = { 105,	41,	106	,42	,107,	43	,85,	21	,86,	22	,87,	23,	125	,61,	126,	62,	127,	63};
    private static readonly int[] RPCC = {26496, 31257, 26841, 31326, 26910, 31395, 26979, 31533, 27117, 31602, 27186, 31671, 27255, 31809, 27393, 31878, 27462, 31947, 27531, 1920, 2265, 1945, 2270, 1950, 2275, 1955, 2285, 1965, 2290, 1970, 2295, 1975, 2305, 1985, 2310, 1990, 2315, 1995, 26880, 31710, 27230, 31780, 27300, 31850, 27370, 31990, 27510, 32060, 27580, 32130, 27650, 32270, 27790, 32340, 27860, 32410, 27930, 2304, 2718, 2334, 2724, 2340, 2730, 2346, 2742, 2358, 2748, 2364, 2754, 2370, 2766, 2382, 2772, 2388, 2778, 2394, 27264, 32163, 27619, 32234, 27690, 32305, 27761, 32447, 27903, 32518, 27974, 32589, 28045, 32731, 28187, 32802, 28258, 32873, 28329, 2688, 3171, 2723, 3178, 2730, 3185, 2737, 3199, 2751, 3206, 2758, 3213, 2765, 3227, 2779, 3234, 2786, 3241, 2793, 28032, 33069, 28397, 33142, 28470, 33215, 28543, 33361, 28689, 33434, 28762, 33507, 28835, 33653, 28981, 33726, 29054, 33799, 29127, 3456, 4077, 3501, 4086, 3510, 4095, 3519, 4113, 3537, 4122, 3546, 4131, 3555, 4149, 3573, 4158, 3582, 4167, 3591, 28416, 33522, 28786, 33596, 28860, 33670, 28934, 33818, 29082, 33892, 29156, 33966, 29230, 34114, 29378, 34188, 29452, 34262, 29526, 3840, 4530, 3890, 4540, 3900, 4550, 3910, 4570, 3930, 4580, 3940, 4590, 3950, 4610, 3970, 4620, 3980, 4630, 3990, 28800, 33975, 29175, 34050, 29250, 34125, 29325, 34275, 29475, 34350, 29550, 34425, 29625, 34575, 29775, 34650, 29850, 34725, 29925, 4224, 4983, 4279, 4994, 4290, 5005, 4301, 5027, 4323, 5038, 4334, 5049, 4345, 5071, 4367, 5082, 4378, 5093, 4389, 29568, 34881, 29953, 34958, 30030, 35035, 30107, 35189, 30261, 35266, 30338, 35343, 30415, 35497, 30569, 35574, 30646, 35651, 30723, 4992, 5889, 5057, 5902, 5070, 5915, 5083, 5941, 5109, 5954, 5122, 5967, 5135, 5993, 5161, 6006, 5174, 6019, 5187, 29952, 35334, 30342, 35412, 30420, 35490, 30498, 35646, 30654, 35724, 30732, 35802, 30810, 35958, 30966, 36036, 31044, 36114, 31122, 5376, 6342, 5446, 6356, 5460, 6370, 5474, 6398, 5502, 6412, 5516, 6426, 5530, 6454, 5558, 6468, 5572, 6482, 5586, 30336, 35787, 30731, 35866, 30810, 35945, 30889, 36103, 31047, 36182, 31126, 36261, 31205, 36419, 31363, 36498, 31442, 36577, 31521, 5760, 6795, 5835, 6810, 5850, 6825, 5865, 6855, 5895, 6870, 5910, 6885, 5925, 6915, 5955, 6930, 5970, 6945, 5985, 32640, 38505, 33065, 38590, 33150, 38675, 33235, 38845, 33405, 38930, 33490, 39015, 33575, 39185, 33745, 39270, 33830, 39355, 33915, 8064, 9513, 8169, 9534, 8190, 9555, 8211, 9597, 8253, 9618, 8274, 9639, 8295, 9681, 8337, 9702, 8358, 9723, 8379, 33024, 38958, 33454, 39044, 33540, 39130, 33626, 39302, 33798, 39388, 33884, 39474, 33970, 39646, 34142, 39732, 34228, 39818, 34314, 8448, 9966, 8558, 9988, 8580, 10010, 8602, 10054, 8646, 10076, 8668, 10098, 8690, 10142, 8734, 10164, 8756, 10186, 8778, 33408, 39411, 33843, 39498, 33930, 39585, 34017, 39759, 34191, 39846, 34278, 39933, 34365, 40107, 34539, 40194, 34626, 40281, 34713, 8832, 10419, 8947, 10442, 8970, 10465, 8993, 10511, 9039, 10534, 9062, 10557, 9085, 10603, 9131, 10626, 9154, 10649, 9177, 40320, 47565, 40845, 47670, 40950, 47775, 41055, 47985, 41265, 48090, 41370, 48195, 41475, 48405, 41685, 48510, 41790, 48615, 41895, 15744, 18573, 15949, 18614, 15990, 18655, 16031, 18737, 16113, 18778, 16154, 18819, 16195, 18901, 16277, 18942, 16318, 18983, 16359, 40704, 48018, 41234, 48124, 41340, 48230, 41446, 48442, 41658, 48548, 41764, 48654, 41870, 48866, 42082, 48972, 42188, 49078, 42294, 16128, 19026, 16338, 19068, 16380, 19110, 16422, 19194, 16506, 19236, 16548, 19278, 16590, 19362, 16674, 19404, 16716, 19446, 16758, 41088, 48471, 41623, 48578, 41730, 48685, 41837, 48899, 42051, 49006, 42158, 49113, 42265, 49327, 42479, 49434, 42586, 49541, 42693, 16512, 19479, 16727, 19522, 16770, 19565, 16813, 19651, 16899, 19694, 16942, 19737, 16985, 19823, 17071, 19866, 17114, 19909, 17157, 48000, 56625, 48625, 56750, 48750, 56875, 48875, 57125, 49125, 57250, 49250, 57375, 49375, 57625, 49625, 57750, 49750, 57875, 49875, 23424, 27633, 23729, 27694, 23790, 27755, 23851, 27877, 23973, 27938, 24034, 27999, 24095, 28121, 24217, 28182, 24278, 28243, 24339, 48384, 57078, 49014, 57204, 49140, 57330, 49266, 57582, 49518, 57708, 49644, 57834, 49770, 58086, 50022, 58212, 50148, 58338, 50274, 23808, 28086, 24118, 28148, 24180, 28210, 24242, 28334, 24366, 28396, 24428, 28458, 24490, 28582, 24614, 28644, 24676, 28706, 24738, 48768, 57531, 49403, 57658, 49530, 57785, 49657, 58039, 49911, 58166, 50038, 58293, 50165, 58547, 50419, 58674, 50546, 58801, 50673, 24192, 28539, 24507, 28602, 24570, 28665, 24633, 28791, 24759, 28854, 24822, 28917, 24885, 29043, 25011, 29106, 25074, 29169, 25137};

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
