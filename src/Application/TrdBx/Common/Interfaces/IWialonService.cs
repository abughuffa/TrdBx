using Newtonsoft.Json.Linq;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface IWialonService
{
    Task<JObject> CreateResource(int creatorId, string name, string dataFlags, bool skipCreatorCheck);
    Task<JObject> CreateAcount(int itemId, string plan);
    Task<JObject> CreateUnitGroup(int creatorId, string name, string dataFlags);
    Task<JObject> CreateUser(int creatorId, string name, string password, string dataFlags);
    Task<JObject> CreateUnit(int creatorId, string name, int hwTypeId, string dataFlags);
    Task<JObject> AddRemoveUnitToUnitGroup(int itemId, int[] units);
    Task<JObject> ActivateUnit(int itemId, string active);
    Task<JObject> DeleteItem(int itemId);
    Task<JObject> UpdateUnitDeviceTypeUniqueId(int itemId, int deviceTypeId, string uniqueId);
    Task<JObject> UpdateHardwareParameters(string hwId, string action);
    Task<JObject> UpdateUnitPhoneNumber(int itemId, string phoneNumber);
    Task<JObject> UpdateUnitAccessPassword(int itemId, string accessPassword);
}

