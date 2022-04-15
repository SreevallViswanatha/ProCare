using Funq;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ProCare.API.Core.Requests;
using ProCare.API.PBM.Repository;
using ProCare.API.PBM.RequestValidator;
using ProCare.API.PBM.RequestValidator.Member;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using ProCare.Common.Data.SQL;
using ServiceStack;
using ServiceStack.FluentValidation.Results;
using ServiceStack.Validation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.PBM
{
    public class AppHost : ProCareAppHost
    {
        #region Constructors

        public AppHost() : base("ProCare.API.PBM", typeof(PBMServices).Assembly) { }

        #endregion

        #region Public Methods

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            base.Configure(container);
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
                UseHttpsLinks = false
                //UseHttpsLinks = !AppSettings.Get(nameof(HostConfig.DebugMode), false)
            });

                Plugins.Add(new AuthFeature(() => new CustomAuthUserSession(),
                new ServiceStack.Auth.IAuthProvider[] {
                    new CustomBasicAuthProvider() 
                }));

            // Register repositories
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Register repositories
            Dictionary<string, string> globalConnections = getConnectionStrings();
            Dictionary<string, string> pbmConnections = getPBMAPIConnectionStrings();

            IDataAccessHelper adsPBMDataHelper = new AdsHelper(globalConnections[ConfigSetttingKey.ConnectionStrings_Poets]);
            IDataAccessHelper DrugDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.ConnectionStrings_PrxDW]);
            IDataAccessHelper MediSpanDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_MediSpan]);
            IDataAccessHelper memberEligibilityDataHelper = new SQLHelper(globalConnections[ConfigSetttingKey.ConnectionStrings_MemberEligibility]);
            IDataAccessHelper preImportDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_Admit24]);
            IDataAccessHelper pharmacyDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.ConnectionStrings_PrxDW]);
            IDataAccessHelper prescriberDataHelper = new AdsHelper(pbmConnections[ConfigSetttingKey.ConnectionStrings_NDDF]);
            IDataAccessHelper securityDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_Admit24]);
            IDataAccessHelper innovationDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_Innovation]);
            IDataAccessHelper retroLICSDataHelper = new AdsHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_RetroLICS_eProcare]);
            IDataAccessHelper pharmacyLocatorDataHelper = new AdsHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_PharmacyLocator_PBME]);
            IDataAccessHelper dataWarehouseDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_DataWarehouse]);
            IDataAccessHelper memberPortalDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_MemberPortal]);
            IDataAccessHelper hospiceDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_Admit24]);
            IDataAccessHelper pbmSchedulerDataHelper = new SQLHelper(pbmConnections[ConfigSetttingKey.PBMAPI_ConnectionStrings_PBMScheduler]);

            // Register our Repository for Dependency 
            var pbmRepository = new PbmRepository(adsPBMDataHelper);
            var prescriberRepository = new PrescriberRepository(prescriberDataHelper);
            var memberRepository = new MemberRepository(adsPBMDataHelper);
            container.Register<IPbmRepository>(c => pbmRepository);
            container.Register<IDrugRepository>(c => new DrugRepository(DrugDataHelper));
            container.Register<IMediSpanRepository>(c => new MediSpanRepository(MediSpanDataHelper));
            container.Register<IEligibilityRepository>(c => new EligibilityRepository(memberEligibilityDataHelper));
            container.Register<IPreImportRepository>(c => new PreImportRepository(preImportDataHelper));
            container.Register<IPharmacyRepository>(c => new PharmacyRepository(pharmacyDataHelper));
            container.Register<IPharmacyRepository>(c => new PharmacyRepository(pharmacyLocatorDataHelper));
            container.Register<IPrescriberRepository>(c => prescriberRepository);
            container.Register<ISecurityRepository>(c => new SecurityRepository(securityDataHelper));
            container.Register<IInnovationRepository>(c => new InnovationRepository(innovationDataHelper));
            container.Register<IRetroLICSRepository>(c => new RetroLICSRepository(retroLICSDataHelper));
            container.Register<IDataWarehouseRepository>(c => new DataWarehouseRepository(dataWarehouseDataHelper));
            container.Register<IMemberPortalRepository>(c => new MemberPortalRepository(memberPortalDataHelper));
            container.Register(c => MediSpanDataHelper);
            container.Register(c => memberEligibilityDataHelper);
            container.Register(c => pharmacyDataHelper);
            container.Register(c => prescriberDataHelper);
            container.Register(c => prescriberDataHelper);
            container.Register<IHospiceRepository>(c => new HospiceRepository(hospiceDataHelper));
            container.Register<IPBMSchedulerTaskRepository>(c => new PBMSchedulerTaskRepository(pbmSchedulerDataHelper));
            container.Register<IMemberRepository>(c => memberRepository);
            container.Register<IMemberRuleRepository>(c => new MemberRuleRepository(adsPBMDataHelper));
            
            var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            container.Register<ICommonApiHelper>(c => commonApiHelper);
            container.RegisterValidator(typeof(EligibilityMemberValidator));
            container.RegisterValidator(typeof(EligibilityImportRequestValidator));
            container.RegisterValidator(typeof(AccumulatorMemberValidator));
            container.RegisterValidator(typeof(AccumulatorImportRequestValidator));
            container.RegisterValidator(typeof(ACPImportRequestValidator));
            container.RegisterValidator(typeof(HospiceSecurityCodeLookupRequestValidator));
            container.RegisterValidator(typeof(MemberSearchRequestValidator));
            container.RegisterValidator(typeof(PharmacySearchRequestValidator));
            container.RegisterValidator(typeof(PrescriberSearchRequestValidator));
            container.RegisterValidator(typeof(PrescriberDetailRequestValidator));
            container.RegisterValidator(typeof(PaidClaimSearchRequestValidator));
            container.RegisterValidator(typeof(PaidClaimDetailsRequestValidator));
            container.RegisterValidator(typeof(MemberDetailsRequestValidator));
            container.RegisterValidator(typeof(DrugMonographRequestValidator));
            container.RegisterValidator(typeof(TestClaimRequestValidator));
            container.RegisterValidator(typeof(DynamicPARequestValidator));
            container.RegisterValidator(typeof(RetroLICSRebuildAccumulatorsTaskRequestValidator));
            container.RegisterValidator(typeof(MemberPortalLoginRequestValidator));
            container.RegisterValidator(typeof(MemberPortalRegistrationRequestValidator));
            container.RegisterValidator(typeof(HospiceImportInsertRecordRequestValidator));
            container.RegisterValidator(typeof(MemberPhysicianLockDetailsRequestValidator));
            container.RegisterValidator(typeof(MemberPhysicianLockUpdateRequestValidator));
            container.RegisterValidator(typeof(MemberTerminationRequestValidator));
            container.RegisterValidator(typeof(MemberRuleRequestValidator));
            container.RegisterValidator(typeof(UnverifiedEpisodeListRequestValidator));
            container.RegisterValidator(typeof(ProcessDetectAndCreateNewEpisodeTaskRequestValidator));
            container.RegisterValidator(typeof(ClientSiteConfigurationRequestValidator));
            container.RegisterValidator(typeof(AppUserInfoListRequestValidator));
            container.RegisterValidator(typeof(UsersWithPermissionsListRequestValidator));
            container.RegisterValidator(typeof(PermissionRequestValidator));
            container.RegisterValidator(typeof(SelfAssignEpisodeRequestValidator));
            container.RegisterValidator(typeof(ReAssignEpisodeRequestValidator));
            container.RegisterValidator(typeof(MemberDetailVQRequestValidator));
            container.RegisterValidator(typeof(UsersWithPlansListRequestValidator));
            container.RegisterValidator(typeof(WorkersCompDetailVQRequestValidator));
            container.RegisterValidator(typeof(NoteListRequestValidator));
            container.RegisterValidator(typeof(EpisodeClaimHistoryRequestValidator));
            container.RegisterValidator(typeof(InsertNoteRequestValidator));
            container.RegisterValidator(typeof(CodedEntityRequestValidator));
            container.RegisterValidator(typeof(MemberPlanIDUpdateRequestValidator));
            container.RegisterValidator(typeof(UpdateCardholderIDRequestValidator));
            container.RegisterValidator(typeof(MemberDetailVQUpdateRequestValidator));
            container.RegisterValidator(typeof(WorkersCompDetailVQUpdateRequestValidator));
            container.RegisterValidator(typeof(AddressExtendedValidator));
            container.RegisterValidator(typeof(UserPlanListSearchRequestValidator));
            container.RegisterValidator(typeof(ChangeLogListRequestValidator));
            container.RegisterValidator(typeof(EpisodeStatusUpdateRequestValidator));
            container.RegisterValidator(typeof(EpisodeGetNextForUserRequestValidator));
            container.RegisterValidator(typeof(MemberPortalPasswordResetRequestValidator));
            container.RegisterValidator(typeof(MemberPortalPasswordChangeRequestValidator));
            container.RegisterValidator(typeof(MemberMedicationDeleteRequestValidator));
            container.RegisterValidator(typeof(MemberContactSaveRequestValidator));
            container.RegisterValidator(typeof(MemberMedicationSaveRequestValidator));
            container.RegisterValidator(typeof(MemberMedicationsGetRequestValidator));
            container.RegisterValidator(typeof(MemberContactDeleteRequestValidator));
            container.RegisterValidator(typeof(GetMemberContactsRequestValidator));

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
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, messageID, req.OriginalPathInfo, ApiMessageType.Request, message,
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

            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, messageID, req.OriginalPathInfo, ApiMessageType.Response, message,
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

        /// <summary>
        /// Gets a set of configured PBM connection strings
        /// </summary>
        /// <returns>Dictionary(string, string) representing the set of configured PBM connection strings</returns>
        private Dictionary<string, string> getPBMAPIConnectionStrings()
        {
            return getConnectionStringsByName(ConfigSetttingKey.PBMAPIConnectionStrings);
        }

        /// <summary>
        /// Gets a set of connection strings configured under the given name
        /// </summary>
        /// <returns>Dictionary(string, string) representing the set of configured connection strings for the given connection set name</returns>
        private Dictionary<string, string> getConnectionStringsByName(string name)
        {
            Dictionary<string, string> output = null;

            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            Task<Dictionary<string, string>> t = commonApiHelper.GetSetting(name);
            t.Wait(ApplicationSettings.GetCancellationToken());
            output = t.Result;

            return output;
        }

        #endregion
    }
}
