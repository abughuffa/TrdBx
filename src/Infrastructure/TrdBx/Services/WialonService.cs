using CleanArchitecture.Blazor.Infrastructure.Services.Wialon;
using Newtonsoft.Json.Linq;



namespace CleanArchitecture.Blazor.Infrastructure.Services

{
    public class WialonService : IWialonService
    {
        private readonly ILogger<WialonService> _logger;
        private readonly IWialonSession _wialonSession;

        public WialonService(IWialonSession wialonSession, ILogger<WialonService> logger)
        {
            _logger = logger;
            _wialonSession = wialonSession;
        }

        #region wialon Api
        /// <summary>
        /// Create Resource
        /// </summary>
        /// <param name="creatorId">ID of a user who will be assigned a creator for a new resource</param>
        /// <param name="name">name of a new resource(at least 4 characters)</param>
        /// <param name="dataFlags">data flags for the response (see Data format: Resources)</param>
        /// <param name="skipCreatorCheck">special flag(see below), 1 - enable</param>
        /// <returns>JObject</returns>

        public async Task<JObject> CreateResource(int creatorId, string name, string dataFlags, bool skipCreatorCheck)
        {
            //svc = core / create_resource &params={"creatorId":< long >,"name":< text >,"dataFlags":< uint >,"skipCreatorCheck":< bool >}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("creatorId", creatorId.ToString()),
                        new("name", name),
                        new("dataFlags", dataFlags),
                        new("skipCreatorCheck", "1")
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("core/create_resource", param.ToArray());
        }

        /// <summary>
        /// Create Acount
        /// </summary>
        /// <param name="itemId">resource Id</param>
        /// <param name="plan">BillingPlan</param>
        /// <returns></returns>
        public async Task<JObject> CreateAcount(int itemId, string plan)
        {
            //svc = account / create_account &params={"itemId":< long >,"plan":< text >}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("itemId", itemId.ToString()),
                        new("plan", plan)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("account/create_account", param.ToArray());
        }
        /// <summary>
        /// Create UnitGroup
        /// </summary>
        /// <param name="creatorId">ID of a user who will be assigned a creator for a new unit group</param>
        /// <param name="name">new unit group name(at least 4 characters)</param>
        /// <param name="dataFlags">data flags for the response (see Data format: Unit group)</param>
        /// <returns></returns>
        public async Task<JObject> CreateUnitGroup(int creatorId, string name, string dataFlags)
        {
            //svc = core / create_unit_group &params={"creatorId":< long >,"name":< text >,"dataFlags":< uint >}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("creatorId", creatorId.ToString()),
                        new("name", name),
                        new("dataFlags", dataFlags)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("core/create_unit_group", param.ToArray());
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="creatorId">ID of a user who will be assigned a creator for a new user</param>
        /// <param name="name">new user name(at least 4 characters)</param>
        /// <param name="password">new user password</param>
        /// <param name="dataFlags">data flags for the response (see Data format: Users)</param>
        /// <returns></returns>
        public async Task<JObject> CreateUser(int creatorId, string name, string password, string dataFlags)
        {
            //svc = core / create_user &params={"creatorId":< long >,"name":< text >,"password":< text >,"dataFlags":< uint >}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("creatorId", creatorId.ToString()),
                        new("name", name),
                        new("password", password),
                        new("dataFlags", dataFlags)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("core/create_user", param.ToArray());
        }

        /// <summary>
        /// Create Unit
        /// </summary>
        /// <param name="creatorId">ID of a user who will be assigned a creator for a new unit</param>
        /// <param name="name">new unit name(at least 4 characters)</param>
        /// <param name="hwTypeId">device(hardware) ID</param>
        /// <param name="dataFlags">data flags for the response (see Data format: Units)</param>
        /// <returns></returns>
        public async Task<JObject> CreateUnit(int creatorId, string name, int hwTypeId, string dataFlags)
        {
            //svc = core / create_unit &params={"creatorId":< long >,"name":< text >,"hwTypeId":< long >,"dataFlags":< long >} 

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("creatorId", creatorId.ToString()),
                        new("name", name),
                        new("hwTypeId", hwTypeId.ToString()),
                        new("dataFlags", dataFlags)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("core/create_unit", param.ToArray());
        }



        /// <summary>
        /// Activate Unit
        /// </summary>
        /// <param name="itemId">unit ID</param>
        /// <param name="active">0 - deactivation, 1 - activation</param>
        /// <returns></returns>
        public async Task<JObject> ActivateUnit(int itemId,string active)
        {
            //svc=unit/set_active&params={"itemId":<long>,"active":<bool>} 

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("itemId", itemId.ToString()),
                        new("active", active)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("unit/set_active", param.ToArray());
        }

        /// <summary>
        /// Delete Item
        /// </summary>
        /// <param name="itemId">item ID</param>
        /// <returns></returns>
        public async Task<JObject> DeleteItem(int itemId)
        {
            //svc = item / delete_item &params={ "itemId":< long >}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("itemId", itemId.ToString())
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("item/delete_item", param.ToArray());
        }

        /// <summary>
        /// Update Unit's DeviceType & UniqueId
        /// </summary>
        /// <param name="itemId">unit ID</param>
        /// <param name="deviceTypeId">new device type</param>
        /// <param name="uniqueId">new unique ID</param>
        /// <returns></returns>
        public async Task<JObject> UpdateUnitDeviceTypeUniqueId(int itemId, int deviceTypeId,string uniqueId)
        {
            //svc = unit / update_device_type &params={ "itemId":< long >,"deviceTypeId":< long >,"uniqueId":< text >}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("itemId", itemId.ToString()),
                        new("deviceTypeId", deviceTypeId.ToString()),
                        new("uniqueId", uniqueId)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("unit/update_device_type", param.ToArray());
        }

        /// <summary>
        /// Update Hardware Parameters
        /// </summary>
        /// <param name="hwId">hardware type ID</param>
        /// <param name="action">set - set configuration</param>
        /// <param name="action">check_config - check if hardware has parameters for configuring</param>
        /// <param name="action">download_file - download configuration file</param>
        /// <param name="action">get - get configuration</param>
        /// <returns></returns>
        public async Task<JObject> UpdateHardwareParameters(string hwId, string action)
        {
            //svc=unit/update_hw_params&params={"hwId":<text>, "action":<text>, ...}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("hwId", hwId),
                        new("action", action)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("unit/update_hw_params", param.ToArray());
        }

        /// <summary>
        /// Update Unit Phone Number
        /// </summary>
        /// <param name="itemId">unit ID</param>
        /// <param name="phoneNumber">new phone number</param>
        /// <returns></returns>
        public async Task<JObject> UpdateUnitPhoneNumber(int itemId, string phoneNumber)
        {
            //svc = unit / update_phone &params={"itemId":< long >,"phoneNumber":< text >}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("itemId", itemId.ToString()),
                        new("phoneNumber", phoneNumber)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("unit/update_phone", param.ToArray());
        }

        /// <summary>
        /// Update Unit Access Password
        /// </summary>
        /// <param name="itemId">unit ID</param>
        /// <param name="accessPassword">new access password</param>
        /// <returns></returns>
        public async Task<JObject> UpdateUnitAccessPassword(int itemId, string accessPassword)
        {
            //svc = unit / update_access_password &params={"itemId":< long >,"accessPassword":< text >}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("itemId", itemId.ToString()),
                        new("accessPassword", accessPassword)
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("unit/update_access_password", param.ToArray());
        }


/// <summary>
/// Add  Remove Units To UnitGroup
/// </summary>
/// <param name="itemId">unit group ID</param>
/// <param name="units">array of units IDs</param>
/// <returns></returns>
public async Task<JObject> AddRemoveUnitToUnitGroup(int itemId, int[] units)
        {
            // svc=unit_group/update_units&params={"itemId":<long>, "units":[<long>]}

            //Prepare API recuest here
            List<KeyValuePair<string, string>> param = new()
                      {
                        new("itemId", itemId.ToString()),
                        new("units", units.ToString())
                      };

            //Execute API request and get bool results

            return await _wialonSession.SendRequest("unit_group/update_units", param.ToArray());
        } 
        #endregion


    }
}



