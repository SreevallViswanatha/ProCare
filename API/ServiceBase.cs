using ProCare.API.Core;
using ProCare.API.Core.Requests;
using ProCare.API.Core.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.API
{
    public class ServiceBase : Service
    {
        protected async Task<T2> processRequest<T1, T2>(T1 request, string apiRoute, Func<Task<T2>> actionToCallIfValid = null)
            where T1 : BaseRequest
            where T2 : BaseResponse
        {
            T2 response;
            try
            {
                if (this.Request.Items.ContainsKey("IsValidRequest") && Convert.ToBoolean(this.Request.Items["IsValidRequest"]))
                {
                    response = await actionToCallIfValid().ConfigureAwait(false);
                }
                else
                {
                    response = (T2)((ServiceStack.Host.NetCore.NetCoreResponse)this.Response).Dto;
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> exceptionIdentifiers = new Dictionary<string, string> { { "ApiRequestID", Request.Items["ApiMessageID"].ToString() } };

                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                await commonApiHelper.LogException(false, Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, exceptionIdentifiers, apiRoute).ConfigureAwait(false);
                throw ex = ex is ArgumentException || ex is TaskCanceledException ? ex : new Exception(FieldDescriptions.UnhandledExceptionError);
            }

            return response;
        }

    }
}
