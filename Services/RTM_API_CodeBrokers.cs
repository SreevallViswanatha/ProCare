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
    public class RTM_API_CodeBrokers : IRTM_API
    {
        public async Task<RTM_API_Response> SendRTMData(Dictionary<string, string> apiSettings, RTMRecordDTO rtmRecord, string clientCode)
        {
            RTM_API_Response output = new RTM_API_Response();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            try
            {

                Guid guid = Guid.NewGuid();
                var methodName = $"PBMAPI/ScheduledTaskServices/ProcessSendRTMDataTask";

                var username = apiSettings["AccountID"];
                var password = apiSettings["AccessKey"];
                var url = rtmRecord.Fields.First(x => x.Name == "TRANSCODE").Value?.ToString() == "2" ? apiSettings["ClaimReversalUrl"] : apiSettings["ClaimUrl"];

                //--Prepare request with fields to post to CodeBrokers
                var request = new
                {
                    code = new
                    {
                        type = apiSettings["OfferCodeType"],
                        channel = apiSettings["OfferCodeChannel"],
                        text = rtmRecord.Fields.First(x => x.Name == "CARDID").Value,
                        date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss +0000"),
                    },
                    claim = new
                    {
                        id = rtmRecord.Fields.First(x => x.Name == "NDCREF").Value,
                        pharmacy = new { npi = rtmRecord.Fields.First(x => x.Name == "PHANPI").Value },
                        script = new
                        {
                            id = rtmRecord.Fields.First(x => x.Name == "RXNO").Value,
                            ndc = rtmRecord.Fields.First(x => x.Name == "NDC").Value,
                            days = Convert.ToInt32(rtmRecord.Fields.First(x => x.Name == "DAYSUP").Value),
                            physician = new { npi = rtmRecord.Fields.First(x => x.Name == "PrescriberNPI").Value }
                        }
                    }
                };

                //--Prepare CodeBrokers response object
                var response = new { status = new { id = "", code = -1, details = "" } };

                //--log request
                await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Request, JsonConvert.SerializeObject(request), null, identifier1: clientCode, identifier2: url).ConfigureAwait(false);

                //--Peform Api Post Invocation
                APIResponse<object> apiResponse = await ApiHelper.ApiBasicAuthPost<object, object>(url, request, username, password).ConfigureAwait(false);

                //--Parse Response
                if (apiResponse != null)
                {
                    if (apiResponse.Response != null)
                    {
                        response = JsonConvert.DeserializeAnonymousType(JsonConvert.SerializeObject(apiResponse.Response), response);
                        output.Status = response.status.code == 0 ? RTM_APIResponseStatus.Sucess : RTM_APIResponseStatus.Fail;
                        output.ALTID = response.status.id;

                        //--log response                
                        await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Response, JsonConvert.SerializeObject(apiResponse), null, identifier1: output.Status.ToString(), identifier2: output.ALTID).ConfigureAwait(false);
                    }
                }
                else
                {
                    output.Status = RTM_APIResponseStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                output.Status = RTM_APIResponseStatus.Fail;
                await commonApiHelper.LogException(false, ApplicationSource.PBMAPI, ex.Message, ex.StackTrace,
                    new Dictionary<string, string> { { "ClientCode", clientCode },{ "RTMRecord", JsonConvert.SerializeObject(rtmRecord) } }, ApiRoutes.ProcessSendRTMDataTask).ConfigureAwait(false);
            }

            return output;
        }
    }
}
