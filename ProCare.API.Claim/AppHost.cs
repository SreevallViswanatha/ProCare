using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ServiceStack;
using ServiceStack.Validation;
using Funq;
using ProCare.API.Claims.Validators;
using ProCare.API.Core.Requests;
using FluentValidation;
using ServiceStack.FluentValidation.Results;
using ProCare.API.Core.Requests;
using System.Net;
using ProCare.API.Claims.Messages.Response;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.Claims
{
    public class AppHost : ProCareAppHost
    {
        #region Constructors

        public AppHost() : base("ProCare.API.Claims", typeof(ClaimServices).Assembly)
        {
        }

        #endregion

        #region Public Methods

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            base.Configure(container);
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false)
            });

            Plugins.Add(new AuthFeature(() => new CustomAuthUserSession(),
                                        new ServiceStack.Auth.IAuthProvider[] {
                                            new CustomBasicAuthProvider()
                                        }));

            // Register repositories
            container.Register(c => new TransmissionValidator());
            container.Register(c => new ClaimReversalValidator());
            container.Register(c => new ClaimEligibilityValidator());

            this.GlobalRequestFilters.Add(requestFilter);


            this.GlobalResponseFilters.Add(async (req, res, requestDto) =>
            {
                await responseFilter(req, res, requestDto).ConfigureAwait(false);
            });
        }

        private void requestFilter(ServiceStack.Web.IRequest req, ServiceStack.Web.IResponse resp, object dto)
        {
            if (dto != null && dto is BaseRequest)
            {
                Guid messageID = Guid.NewGuid();
                req.Items.Add(Logging.Text_ApiMessageID, messageID);
                req.Items.Add(Logging.Text_StartTime, DateTime.Now);

                ServiceStack.FluentValidation.IValidator validator = ValidatorCache.GetValidator(req, dto.GetType());

                if (validator == null)
                {
                    req.Items.Add(Logging.Text_IsValidRequest, Boolean.TrueString);
                }
                else
                {
                    ValidationResult result = validator.Validate(dto);

                    if (result.IsValid)
                    {
                        req.Items.Add(Logging.Text_IsValidRequest, Boolean.TrueString);
                    }
                    else
                    {
                        req.Items.Add(Logging.Text_IsValidRequest, Boolean.FalseString);
                        object errorResponse = DtoUtils.CreateErrorResponse(dto, ValidationResultExtensions.ToErrorResult(result));
                        resp.Dto = ((ServiceStack.HttpError)errorResponse).Response;
                        resp.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
            }
        }

        private async Task responseFilter(ServiceStack.Web.IRequest req, ServiceStack.Web.IResponse resp, object dto)
        {
            try
            {
                ProCare.API.Core.Responses.BaseResponse apiMethodResponse = null;

                if (resp is ServiceStack.Host.NetCore.NetCoreResponse)
                {
                    ServiceStack.Host.NetCore.NetCoreResponse rawResponse = (ServiceStack.Host.NetCore.NetCoreResponse)resp;

                    if (rawResponse.Dto is ProCare.API.Core.Responses.BaseResponse)
                    {
                        apiMethodResponse = (ProCare.API.Core.Responses.BaseResponse)rawResponse.Dto;
                    }
                    else if (rawResponse.Dto is ServiceStack.HttpError && ((ServiceStack.HttpError)rawResponse.Dto).Response is ProCare.API.Core.Responses.BaseResponse)
                    {
                        apiMethodResponse = (ProCare.API.Core.Responses.BaseResponse)((ServiceStack.HttpError)rawResponse.Dto).Response;
                    }
                }

                if (apiMethodResponse != null)
                {
                    Guid messageID = Guid.Parse(req.Items[Logging.Text_ApiMessageID].ToString());
                    DateTime startDT = (DateTime)req.Items[Logging.Text_StartTime];
                    double executionTime = (DateTime.Now - startDT).TotalMilliseconds;

                    apiMethodResponse.ApiRequestID = messageID;
                    apiMethodResponse.TimeToProcess = (long)executionTime;
                    dto = apiMethodResponse;

                    // Log Api Request
                    await logAPIRequest(messageID, req.Dto, req).ConfigureAwait(false);
                    // Log Api Response
                    await logAPIResponse(messageID, dto, req).ConfigureAwait(false);
                }
            }
            catch { }
        }

        private async Task logAPIRequest(Guid messageID, object message, ServiceStack.Web.IRequest req)
        {
            string identifier1, identifier2, identifier3, identifier4;
            identifier1 = identifier2 = identifier3 = identifier4 = null;
            DateTime startDT = (DateTime)req.Items[Logging.Text_StartTime];

            if (req.Items.ContainsKey(Logging.Text_ReqIdentifier1))
            {
                identifier1 = Convert.ToString(req.Items[Logging.Text_ReqIdentifier1]);
            }
            if (req.Items.ContainsKey(Logging.Text_ReqIdentifier2))
            {
                identifier2 = Convert.ToString(req.Items[Logging.Text_ReqIdentifier2]);
            }
            if (req.Items.ContainsKey(Logging.Text_ReqIdentifier3))
            {
                identifier3 = Convert.ToString(req.Items[Logging.Text_ReqIdentifier3]);
            }
            if (req.Items.ContainsKey(Logging.Text_ReqIdentifier4))
            {
                identifier4 = Convert.ToString(req.Items[Logging.Text_ReqIdentifier4]);
            }

            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            await commonApiHelper.LogApiMessage(ApiType.ClaimApi, messageID, req.OriginalPathInfo, ApiMessageType.Request, message,
                  identifier1: identifier1, identifier2: identifier2, identifier3: identifier3, identifier4: identifier4, MessageTimeStamp: startDT).ConfigureAwait(false);

        }

        private async Task logAPIResponse(Guid messageID, object message, ServiceStack.Web.IRequest req)
        {
            string identifier1, identifier2, identifier3, identifier4;
            identifier1 = identifier2 = identifier3 = identifier4 = null;

            if (req.Items.ContainsKey(Logging.Text_RespIdentifier1))
            {
                identifier1 = Convert.ToString(req.Items[Logging.Text_RespIdentifier1]);
            }
            if (req.Items.ContainsKey(Logging.Text_RespIdentifier2))
            {
                identifier2 = Convert.ToString(req.Items[Logging.Text_RespIdentifier2]);
            }
            if (req.Items.ContainsKey(Logging.Text_RespIdentifier3))
            {
                identifier3 = Convert.ToString(req.Items[Logging.Text_RespIdentifier3]);
            }
            if (req.Items.ContainsKey(Logging.Text_RespIdentifier4))
            {
                identifier4 = Convert.ToString(req.Items[Logging.Text_RespIdentifier4]);
            }

            if (message is NCPDPDetailResponse)
            {
                try
                {
                    ((NCPDPDetailResponse)message).NcpdpRequestString = new string(((NCPDPDetailResponse)message).NcpdpRequestString.Where(c => !char.IsControl(c)).ToArray());
                }
                catch(Exception)
                {
                    ((NCPDPDetailResponse)message).NcpdpRequestString = "";
                }

                try
                {
                    ((NCPDPDetailResponse)message).NcpdpResponseString = new string(((NCPDPDetailResponse)message).NcpdpResponseString.Where(c => !char.IsControl(c)).ToArray());
                }
                catch(Exception)
                {
                    ((NCPDPDetailResponse)message).NcpdpResponseString = "";
                    if (string.IsNullOrWhiteSpace(identifier3))
                    {
                        identifier3 = "X";
                    }
                }
            }
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            await commonApiHelper.LogApiMessage(ApiType.ClaimApi, messageID, req.OriginalPathInfo, ApiMessageType.Response, message,
                identifier1: identifier1, identifier2: identifier2, identifier3: identifier3, identifier4: identifier4).ConfigureAwait(false);
        }


        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets a set of configured connection strings
        /// </summary>
        /// <returns>Dictionary(string, string) representing the set of configured connection strings</returns>
        private Dictionary<string, string> getConnectionStrings()
        {
            Dictionary<string, string> output = null;

            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            Task<Dictionary<string, string>> t = commonApiHelper.GetSetting(ConfigSetttingKey.ConnectionStrings);
            t.Wait(ApplicationSettings.GetCancellationToken());
            output = t.Result;

            return output;
        }

        #endregion
    }
}
