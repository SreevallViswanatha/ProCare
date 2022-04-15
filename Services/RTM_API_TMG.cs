using Newtonsoft.Json;
using ProCare.API.Core;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.PBM.Services
{
    public class RTM_API_TMG : IRTM_API
    {
        public async Task<RTM_API_Response> SendRTMData(Dictionary<string, string> apiSettings, RTMRecordDTO rtmRecord, string clientCode)
        {

            Guid guid = Guid.NewGuid();
            var methodName = $"PBMAPI/ScheduledTaskServices/ProcessSendRTMDataTask";

            var username = apiSettings["AccountID"];
            var password = apiSettings["AccessKey"];
            var url = "";

            //--Prepare request with fields to post 
            var request = new { };

            //--Prepare response object
            var response = new {};

            //--log request
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Request, JsonConvert.SerializeObject(request), null, identifier1: clientCode, identifier2: url).ConfigureAwait(false);

            //--Peform Api Post Invocation
            APIResponse<object> apiResponse = await ApiHelper.ApiBasicAuthPost<object, object>(url, request, username, password).ConfigureAwait(false);

            //--log response                
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Response, JsonConvert.SerializeObject(apiResponse), null, identifier1: clientCode, identifier2: url).ConfigureAwait(false);

            //--Parse Response
            response = JsonConvert.DeserializeAnonymousType(JsonConvert.SerializeObject(apiResponse.Response), response);
            RTM_API_Response output = new RTM_API_Response()
            {
                Status = RTM_APIResponseStatus.Sucess,
                ALTID = ""
            };

            return output;
        }
    }
}
