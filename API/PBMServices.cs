using log4net;

using Newtonsoft.Json;

using ProCare.API.Claims.Messages.Request;
using ProCare.API.Claims.Messages.Response;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ProCare.API.Core.DTO;
using ProCare.API.Core.Requests;
using ProCare.API.Core.Responses;
using ProCare.API.PBM.Helpers;
using ProCare.API.PBM.Messages;
using ProCare.API.PBM.Messages.MediSpan.Request;
using ProCare.API.PBM.Messages.MediSpan.Response;
using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Request.Eligibility;
using ProCare.API.PBM.Messages.Response;
using ProCare.API.PBM.Messages.Response.Eligibility;
using ProCare.API.PBM.Messages.Response.MediSpan;
using ProCare.API.PBM.Repository;
using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.DTO.Drug;
using ProCare.API.PBM.Repository.DTO.Eligibility;
using ProCare.API.PBM.Repository.DTO.MemberPortal;
using ProCare.API.PBM.Repository.Helpers;
using ProCare.Common;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using ProCare.Common.Data.SQL;
using ProCare.NCPDP.Telecom.Request;

using ServiceStack;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using static ProCare.API.Core.Requests.Enums;
using static ProCare.API.PBM.Messages.Shared.Enums;

using Address = ProCare.API.PBM.Messages.Response.Address;


namespace ProCare.API.PBM
{
    public class PBMServices : Service
    {
        #region Public Properties
        /// <summary>
        /// Dependency Injected Automatically by ServiceStack
        /// </summary>
        public IDataAccessHelper DataHelper { get; set; }

        public IPbmRepository Repository { get; set; }

        public IMediSpanRepository MediSpanRepository { get; set; }
        public IDrugRepository DrugRepository { get; set; }

        public IEligibilityRepository EligibilityRepository { get; set; }

        public IPharmacyRepository PharmacyRepository { get; set; }

        public IPharmacyRepository PharmacyLocatorRepository { get; set; }

        public IPrescriberRepository PrescriberRepository { get; set; }

        public ISecurityRepository SecurityRepository { get; set; }

        public IInnovationRepository InnovationRepository { get; set; }

        public IDataWarehouseRepository DateWarehouseRepository { get; set; }

        public IMemberPortalRepository MemberPortalRepository { get; set; }

        public IHospiceRepository HospiceRepository { get; set; }
        public IPBMSchedulerTaskRepository PBMSchedulerTaskRepository { get; set; }

        public IVerificationQueueRepository VerificationQueueRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        #endregion

        #region Private Variables

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Private Variables

        #region Public Methods

        #region Api Version

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PBMVersion + "|" + ApiRoutes.PBMVersion)]
        public ApiVersionResponse Get(ApiVersionRequest request)
        {
            var response = new ApiVersionResponse()
            {
                ApiVersion = VersionHelper.GetVersion()
            };

            return response;
        }

        #endregion

        #region Lookup

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PBMEntityTypeLookup + "|" + ApiRoutes.PBMEntityTypeLookup)]
        public List<EntityLookupResponse> Get(EntityLookupRequest request)
        {
            List<EntityLookupResponse> response = new List<EntityLookupResponse>()
            {
                new EntityLookupResponse()
                {
                    ID = "001",
                    Description = "Test"
                }
            };

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ClientSiteConfiguration + "|" + ApiRoutes.ClientSiteConfiguration)]
        public async Task<ClientSiteConfigurationResponse> Get(ClientSiteConfigurationRequest request)
        {
            ClientSiteConfigurationResponse response = new ClientSiteConfigurationResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.ClientSiteConfiguration, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                List<ADSServerDatabaseDTO> adsServerDatabaseDTOs = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);

                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabaseDTOs.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());

                string connectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);

                ClientSiteConfigurationDTO results = new ClientSiteConfigurationDTO();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    results = Repository.GetClientSiteConfiguration(request.ClientSiteConfigurationID, connectionString);

                    if (results != null && results.ClientSiteConfigurationID > 0)
                    {
                        response = results.ConvertTo<ClientSiteConfigurationResponse>();
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid SiteID: {request.ClientSiteConfigurationID}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Invalid ClientID: {request.ClientID}");
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.CodedEntity + "|" + ApiRoutes.CodedEntity)]
        public async Task<CodedEntityResponse> Get(CodedEntityRequest request)
        {
            CodedEntityResponse response = new CodedEntityResponse();
            response = await processRequest(request, ApiRoutes.CodedEntity, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration

                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                string connectionString = string.Empty;
                List<CodedEntityDTO> codedEntityDTOs = new List<CodedEntityDTO>();

                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);
                List<ADSServerDatabaseDTO> adsServerDatabaseDTOs = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);
                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabaseDTOs.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());
                connectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    codedEntityDTOs = Repository.ReadCodedEntities(request.CodedEntityTypeID, connectionString);
                    response.CodedEntities = new List<CodedEntity>();
                    codedEntityDTOs = codedEntityDTOs.OrderBy(x => x.SortOrderID).ToList();
                    codedEntityDTOs?.ForEach(x => response.CodedEntities.Add(x.ConvertTo<CodedEntity>()));
                }
                else
                {
                    throw new ArgumentException(Constants.ConnectionInfoNotFound);
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        #endregion

        #region Drug

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.DrugNameSearch + "|" + ApiRoutes.DrugNameSearch)]
        public async Task<DrugNameSearchResponse> Get(DrugNameSearchRequest request)
        {
            DrugNameSearchResponse response = await processRequest(request, ApiRoutes.DrugNameSearch, async () =>
            {
                response = new DrugNameSearchResponse();
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                response.FullDrugNamesList = await DrugRepository
                                                    .LookupFullDrugNamesByPartialDrugName(request.PartialDrugName).ConfigureAwait(false);
                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.GetDrugStrengthsByDrugName + "|" + ApiRoutes.GetDrugStrengthsByDrugName)]
        public async Task<DrugStrengthsByDrugNameResponse> Get(DrugStrengthsByDrugNameRequest request)
        {
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
            string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_MemberPortal];
            PRXUserSSODetailsDTO item = await DrugRepository.GetPRXUserSSOAPSENRPLN(connectionString, request.Token);

            DrugStrengthsByDrugNameResponse response = await processRequest(request, ApiRoutes.GetDrugStrengthsByDrugName, async () =>
            {
                response = new DrugStrengthsByDrugNameResponse();
                List<DrugStrengthsDTO> dtos = await DrugRepository
                                                    .LookupDrugStrengthsByDrugName(item.ClientID, item.FRMID, request.DrugName, item.FORMULARY == "C", false).ConfigureAwait(false);
                dtos.ForEach(x =>
                {
                    response.DrugStrengths.Add(new DrugStrengths
                    {
                        DrugType = x.DrugType,
                        DrugID = x.DrugID,
                        DrugName = x.DrugName,
                        NDC = x.NDC
                    });
                });

                return response;
            }).ConfigureAwait(false);

            return response;
        }


        #endregion


        #region MediSpan

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PBMMediSpanLookup + "|" + ApiRoutes.PBMMediSpanLookup)]
        public async Task<MediSpanResponse> Get(MediSpanNDCRequest request)
        {
            MediSpanResponse response = await processRequest(request, ApiRoutes.PBMMediSpanLookup, async () =>
            {
                response = new MediSpanResponse();

                DatasetDTO dataset = await MultipleConnectionsHelper.GetClientDataset(request.ClientGuid, request.Client).ConfigureAwait(false);

                string connectionString = MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path);

                List<MediSpanDrugDTO> dtos =
                    await MediSpanRepository.LookupDrugByNDC(connectionString, request.NDC).ConfigureAwait(false);
                dtos.ForEach(x => { response.MediSpanResults.Add(x.ConvertTo<MediSpanResult>()); });

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PBMMediSpanSearch + "|" + ApiRoutes.PBMMediSpanSearch)]
        public async Task<MediSpanSearchResponse> Get(MediSpanSearchRequest request)
        {
            MediSpanSearchResponse response = await processRequest(request, ApiRoutes.PBMMediSpanSearch, async () =>
            {
                response = new MediSpanSearchResponse();

                DatasetDTO dataset = await MultipleConnectionsHelper.GetClientDataset(request.ClientGuid, request.Client).ConfigureAwait(false);

                string connectionString = MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path);

                List<MediSpanDrugDTO> dtos = await MediSpanRepository
                                                    .SearchMedifil(connectionString, request.NDC, request.DrugName, request.GPI,
                                                                    request.MSCode).ConfigureAwait(false);
                dtos.ForEach(x => { response.MediSpanSearchResults.Add(x.ConvertTo<MediSpanResult>()); });

                return response;
            }).ConfigureAwait(false);

            return response;
        }


        #endregion

        #region Hospice

        #region HospiceSecurityCodeLookup
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.HospiceSecurityCodeLookupRequest + "|" + ApiRoutes.HospiceSecurityCodeLookupRequest)]
        public async Task<HospiceSecurityCodeLookupResponse> Post(HospiceSecurityCodeLookupRequest request)
        {
            long requestId = -1;
            HospiceSecurityCodeLookupResponse response = await processRequest(request, ApiRoutes.HospiceSecurityCodeLookupRequest, async () =>
            {
                requestId = await InnovationRepository.AddAdmitCCRequest(ConfigSetttingKey.HSCLookup, JsonConvert.SerializeObject(request))
                                                      .ConfigureAwait(false);

                response = new HospiceSecurityCodeLookupResponse();

                HospiceLookupDTO dto = await SecurityRepository.LookupHospiceByHospiceSecurityCode(request.HospiceSecurityCode).ConfigureAwait(false);
                response = dto.ConvertTo<HospiceSecurityCodeLookupResponse>();

                if (response.HospiceID.GetValueOrDefault() > 0 && !string.IsNullOrWhiteSpace(response.HospiceName))
                {
                    response.IsHospiceSecurityCodeValid = true;
                }
                else
                {
                    response.IsHospiceSecurityCodeValid = false;
                }

                return response;
            }).ConfigureAwait(false);

            response.ApiRequestID = (Guid)Request.Items["ApiMessageID"];
            long responseId = await InnovationRepository.AddAdmitCCResponse(requestId, JsonConvert.SerializeObject(response)).ConfigureAwait(false);

            return response;
        }
        #endregion HospiceSecurityCodeLookup

        #region Hospice Import
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.HospiceImportInsertRecordRequest + "|" + ApiRoutes.HospiceImportInsertRecordRequest)]
        public async Task<HospiceImportInsertRecordResponse> Post(HospiceImportInsertRecordRequest request)
        {
            HospiceImportInsertRecordResponse response = await processRequest(request, ApiRoutes.HospiceImportInsertRecordRequest, async () =>
            {
                response = await ProcessHospiceImportRecord(request.VendorID ?? 0, Request.Headers, request.PatientRecord).ConfigureAwait(false);

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.HospiceImportInsertRawRecordRequest + "|" + ApiRoutes.HospiceImportInsertRawRecordRequest)]
        public async Task<HospiceImportInsertRecordResponse> Post(HospiceImportInsertRawRecordRequest request)
        {
            HospiceImportInsertRecordResponse response = await processRequest(request, ApiRoutes.HospiceImportInsertRecordRequest, async () =>
            {
                string rawImportRecord = Request.GetRawBody();
                response = await ProcessHospiceImportRecord(0, Request.Headers, rawImportRecord).ConfigureAwait(false);

                return response;
            }).ConfigureAwait(false);

            return response;
        }
        #endregion Hospice Import
        #endregion Hospice

        #region Member
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberSearchRequest + "|" + ApiRoutes.MemberSearchRequest)]
        public async Task<MemberSearchResponse> Get(MemberSearchRequest request)
        {
            SearchTypeOperator? lastNameOperator = getSearchType(request.LastName, request.LastNameOperator);
            SearchTypeOperator? firstNameOperator = getSearchType(request.FirstName, request.FirstNameOperator);
            SearchTypeOperator? memberIdOperator = getSearchType(request.MemberId, request.MemberIdOperator);

            MemberSearchResponse response = new MemberSearchResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.MemberSearchRequest, async () =>
            {
                DateTime? dob = null;

                if (!string.IsNullOrWhiteSpace(request.DateOfBirth))
                {
                    dob = DateTime.ParseExact(request.DateOfBirth, "yyyyMMdd", null);
                }

                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + request.ClientGuid).ConfigureAwait(false);
                Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MemberSearchConfig).ConfigureAwait(false);

                List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);

                if (!string.IsNullOrWhiteSpace(request.Client))
                {
                    datasets = datasets.Where(x => x.Name.EqualsIgnoreCase(request.Client)).ToList();
                }

                int maxRecords = Convert.ToInt32(config[ConfigSetttingKey.ApiMaxNumberOfRecordsSetting_KeyValue]);
                int maxCountThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxCountThreads]);
                int maxSearchThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxSearchThreads]);

                if (request.ItemsPerPage == null)
                {
                    request.ItemsPerPage = maxRecords;
                }

                if (request.PageNumber == null)
                {
                    request.PageNumber = 1;
                }

                // Perform Member search, retrieve results and populate response
                int totalNumberOfRecords = await memberSearchGetCounts(datasets, maxCountThreads, request.MemberId, memberIdOperator,
                                                                       request.LastName, lastNameOperator, request.FirstName,
                                                                       firstNameOperator, dob).ConfigureAwait(false);

                //Get search results and count of historical duplicates that were removed
                Tuple<List<MemberSearchDTO>, int> searchResultInfo = await memberSearchClientDatasets(datasets, maxSearchThreads, request.MemberId, memberIdOperator,
                                                                                       request.LastName, lastNameOperator, request.FirstName,
                                                                                       firstNameOperator, dob)
                                                                                       .ConfigureAwait(false);

                //Only return top max records, FamilyId is CardID + CardID2
                List<MemberSearchDTO> searchResults = searchResultInfo.Item1.OrderBy(x => x.FamilyId).Take(maxRecords).ToList();

                //Subtract any duplicates from the count
                totalNumberOfRecords -= searchResultInfo.Item2;

                List<MemberSearchResult> MemberSearchResults = new List<MemberSearchResult>();
                PagingOptionsResponse pagingOption = new PagingOptionsResponse();

                response.MemberSearchResults = MemberSearchResults;

                MemberSearchDTO firstItem = searchResults.FirstOrDefault();

                if (request.ItemsPerPage >= totalNumberOfRecords)
                {
                    //Add all
                    searchResults.ForEach(x =>
                    {
                        MemberSearchResult result = convertToMemberSearchResult(x);
                        MemberSearchResults.Add(result);
                    });
                }
                else
                {
                    //Add the current page
                    searchResults
                        .Skip(request.ItemsPerPage.Value * (request.PageNumber.Value - 1)) //Skip previous pages
                        .Take(request.ItemsPerPage.Value) //Get the current page of results
                        .ToList()
                        .ForEach(x => { MemberSearchResults.Add(convertToMemberSearchResult(x)); });
                }


                if (firstItem != null)
                {
                    pagingOption.PageNumber = request.PageNumber;
                    pagingOption.ItemsPerPage = request.ItemsPerPage;
                    pagingOption.TotalNumberOfPages = totalNumberOfRecords / request.ItemsPerPage;
                    if (totalNumberOfRecords % request.ItemsPerPage > 0)
                    {
                        pagingOption.TotalNumberOfPages++;
                    }
                    pagingOption.TotalNumberOfRecords = totalNumberOfRecords;
                }
                else
                {
                    pagingOption.PageNumber = 1;
                    pagingOption.ItemsPerPage = 0;
                    pagingOption.TotalNumberOfPages = 1;
                    pagingOption.TotalNumberOfRecords = 0;
                }

                response.PagingOptions = pagingOption;

                return response;
            }).ConfigureAwait(false);

            return response;
        }


        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberDetailsRequest + "|" + ApiRoutes.MemberDetailsRequest)]
        public async Task<MemberDetailsResponse> Get(MemberDetailsRequest request)
        {
            MemberDetailsResponse response = new MemberDetailsResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.MemberDetailsRequest, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + request.ClientGuid).ConfigureAwait(false);
                Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MemberDetailsConfig).ConfigureAwait(false);

                List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);
                int maxSearchThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxSearchThreads]);

                if (!string.IsNullOrWhiteSpace(request.Client))
                {
                    datasets = datasets.Where(x => x.Name.EqualsIgnoreCase(request.Client)).ToList();
                }

                ClientConfigHelper.ValidateClientAccessLevels(ref datasets, null, request.OrganizationID, request.GroupID, request.PlanID);
                datasets = datasets.Where(x => x.ClientHasAccess).ToList();

                // Perform Member search, retrieve results and populate response
                List<MemberDetailsResultDTO> results =
                    await getMemberDetails(datasets, maxSearchThreads, request.OrganizationID, request.GroupID, request.PlanID, request.MemberID,
                                           request.Person).ConfigureAwait(false);

                //Now that we know which results are from ENR, we want to get the distinct result list regardless of table
                results = results.GroupBy(x => new { x.MemberDetail.MemberEnrolleeID, x.MemberDetail.PlanID, PlanEffectiveDate = x.PlanInfo.EffectiveDate, PlanTerminationDate = x.PlanInfo.TerminationDate, MemberEffectiveDate = x.MemberCoverage.EffectiveDate, MemberTerminationDate = x.MemberCoverage.TerminationDate })
                            .Select(x => x.OrderByDescending(y => y.TableName)) //Prefer APSENR
                            .OrderByDescending(x => x.First().PlanInfo.EffectiveDate).ThenByDescending(x => x.First().MemberCoverage.EffectiveDate) //Order groups by plan effective date desc, member effective date desc
                            .Select(x => x.First()).ToList(); //Take first from each group, this will prefer APSENR in the case of dupes

                List<MemberDetailsResultDTO> enrResults = results.Where(x => x.TableName.EqualsIgnoreCase("APSENR")).ToList();

                //Filter to first result if not a multi-segent response
                if (!request.MultiSegmentResponseIndicator && results.Count > 1)
                {
                    if (enrResults.Count > 0)
                    {
                        results = new List<MemberDetailsResultDTO> { enrResults.FirstOrDefault() };
                    }
                    else
                    {
                        results = new List<MemberDetailsResultDTO> { results.FirstOrDefault() };
                    }
                }

                results = await getMemberDetailDiagnoses(datasets, maxSearchThreads, results).ConfigureAwait(false);

                List<MemberDetailsResult> MemberDetails = new List<MemberDetailsResult>();

                response.MemberDetailsResults = MemberDetails;

                results.ForEach(x =>
                {
                    MemberDetailsResult result = convertToMemberDetailsResult(x);
                    MemberDetails.Add(result);
                });

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberPhysicianLockDetailsRequest + "|" + ApiRoutes.MemberPhysicianLockDetailsRequest)]
        public async Task<MemberPhysicianLockDetailsResponse> Get(MemberPhysicianLockDetailsRequest request)
        {
            MemberPhysicianLockDetailsResponse response = new MemberPhysicianLockDetailsResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.MemberPhysicianLockDetailsRequest, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + request.ClientGuid).ConfigureAwait(false);
                Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MultithreadDefaults);
                Dictionary<string, string> physreqDescriptions = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_PHYSREQ_Descriptions).ConfigureAwait(false);

                List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);
                int maxSearchThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxSearchThreads]);
                if (!string.IsNullOrWhiteSpace(request.Client))
                {
                    datasets = datasets.Where(x => x.Name.EqualsIgnoreCase(request.Client)).ToList();
                }

                ClientConfigHelper.ValidateClientAccessLevels(ref datasets, null, null, null, request.PlanID);
                datasets = datasets.Where(x => x.ClientHasAccess).ToList();
                List<DatasetEnrolleeIDDTO> datasetEnrolleeIds = await getDatasetEnrolleeIds(datasets, request.PlanID, request.MemberID, request.Person).ConfigureAwait(false);

                if (datasetEnrolleeIds.Count == 0)
                {
                    throw new ArgumentException($"A Member {request.MemberID} with Person Code {request.Person} was not found in Plan {request.PlanID}");
                }

                // Retrieve results and populate response
                List<MemberPhysicianLockDetailsResultDTO> results =
                await getMemberPhysicianLockDetails_ByMember(datasetEnrolleeIds, maxSearchThreads, request.PlanID, request.MemberID, request.Person).ConfigureAwait(false);

                if (results.Count > 0)
                {
                    var memberInfo = results.First();
                    string physreq = string.IsNullOrWhiteSpace(memberInfo.PHYSREQ) ? "" : memberInfo.PHYSREQ.ToUpper();

                    response.Client = memberInfo.Client;
                    response.OrganizationID = memberInfo.ORGID;
                    response.GroupID = memberInfo.GRPID;
                    response.PlanID = request.PlanID.ToUpper().Trim();
                    response.MemberID = request.MemberID.ToUpper().Trim();
                    response.MemberEnrolleeID = memberInfo.ENRID;
                    response.PersonCode = request.Person.Trim();
                    response.MemberLockInStatus = $"{physreq} - {physreqDescriptions[physreq]}";

                    List<MemberPhysicianLockPhysician> PhysicianLocks = new List<MemberPhysicianLockPhysician>();
                    response.PhysicianLocks = PhysicianLocks;

                    if (memberInfo.EffectiveDate.HasValue)
                    {
                        results.ForEach(x =>
                        {
                            MemberPhysicianLockPhysician result = convertToMemberPhysicianLockPhysician(x);
                            PhysicianLocks.Add(result);
                        });
                    }
                    else if (string.IsNullOrWhiteSpace(physreq))
                    {
                        throw new ArgumentException($"No physician lock records found for Member {request.MemberID} with Person Code {request.Person} in Plan {request.PlanID}");
                    }
                }
                else
                {
                    throw new ArgumentException("Member information was not loaded in a valid format.");
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberMedicationDeleteRequest + "|" + ApiRoutes.MemberMedicationDeleteRequest)]
        public async Task<MemberMedicationDeleteResponse> Post(MemberMedicationDeleteRequest request)
        {
            MemberMedicationDeleteResponse response = new MemberMedicationDeleteResponse();
            response = await processRequest(request, ApiRoutes.MemberMedicationDeleteRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_MemberPortal];
                PRXUserSSODetailsDTO item = await DrugRepository.GetPRXUserSSOAPSENRPLN(connectionString, request.Token);
                if (item == null)
                {
                    throw new ArgumentException("Member authentication token is expired or invalid");
                }
                var result = await MemberPortalRepository.GetMemberFavoriteMedicationByMemberMedicationID(connectionString, request.MemberMedicationID, item.UserID);
                if (result.UserID != default)
                {
                    await MemberPortalRepository.DeleteMemberFavoriteMedication(item.UserID, request.MemberMedicationID);
                }

                else
                {
                    throw new ArgumentException($"A Member Favorite Medication with MemberMedicationID '{request.MemberMedicationID}' was not found for User { request.Token}");
                }
                return response;

            }).ConfigureAwait(false);

            return response;
        }


        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberMedicationSaveRequest + "|" + ApiRoutes.MemberMedicationSaveRequest)]
        public async Task<MemberMedicationSaveResponse> Post(MemberMedicationSaveRequest request)
        {
            MemberMedicationSaveResponse response = new MemberMedicationSaveResponse();
            response = await processRequest(request, ApiRoutes.MemberMedicationSaveRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_MemberPortal];


                PRXUserSSODetailsDTO tokenData = await MemberPortalRepository.GetPRXUserSSOAPSENRPLN(connectionString, request.Token);

                validateUserToken(tokenData);

                if (request.MemberMedicationTypeID == (int)MemberMedicationType.SavedMedication)
                {
                    bool ndcExists = await MemberPortalRepository.NDCExists(connectionString, request.EntityIdentifier.ToString());

                    if (!ndcExists)
                    {
                        throw new ArgumentException("No drug was found for the requested NDC");
                    }
                }

                if (request.MemberMedicationID > 0)
                {
                    var memberMedication = await MemberPortalRepository.GetMemberFavoriteMedicationByMemberMedicationID(connectionString, (long)request.MemberMedicationID, tokenData.UserID);

                    if (memberMedication == null)
                    {
                        throw new ArgumentException($"A medication for MemberMedicationID {request.MemberMedicationID} was not found for User {request.Token}");
                    }
                }

                response.MemberMedicationID = await MemberPortalRepository.SaveMemberFavoriteMedication(connectionString, tokenData.UserID, request.MemberMedicationID, request.MemberMedicationTypeID, request.EntityIdentifier, request.MedicationName).ConfigureAwait(false);

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.GetMemberMedications + "|" + ApiRoutes.GetMemberMedications)]
        public async Task<MemberMedicationGetResponse> Get(MemberMedicationGetRequest request)
        {
            MemberMedicationGetResponse response = new MemberMedicationGetResponse();
            response = await processRequest(request, ApiRoutes.MemberMedicationSaveRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_MemberPortal];

                PRXUserSSODetailsDTO tokenData = await MemberPortalRepository.GetPRXUserSSOAPSENRPLN(connectionString, request.Token);

                validateUserToken(tokenData);

                List<MemberFavoriteMedicationDTO> memberFavoriteMedicationDTO = await MemberPortalRepository.GetMemberFavoriteMedicationByUserId(connectionString, tokenData.UserID).ConfigureAwait(false);

                response.MemberMedications = new List<MemberMedication>();
                foreach (var medication in memberFavoriteMedicationDTO)
                {
                    string entityIdentifier = medication.EntityIdentifier.ToString();
                    if (medication.MemberMedicationTypeID == (int)MemberMedicationType.SavedMedication)
                    {
                        entityIdentifier = string.Format("{0:00000000000}", Convert.ToDecimal(medication.EntityIdentifier));
                    }
                    response.MemberMedications.Add(new MemberMedication
                    {
                        EntityIdentifier = entityIdentifier,
                        MedicationName = medication.MedicationName,
                        MemberMedicationID = medication.MemberMedicationID,
                        MemberMedicationTypeID = medication.MemberMedicationTypeID
                    });
                }


                response.UserID = tokenData.UserID;

                return response;
            }).ConfigureAwait(false);
            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.DeleteMemberContact + "|" + ApiRoutes.DeleteMemberContact)]
        public async Task<MemberContactDeleteResponse> Post(MemberContactDeleteRequest request)
        {
            MemberContactDeleteResponse response = new MemberContactDeleteResponse();
            response = await processRequest(request, ApiRoutes.DeleteMemberContact, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_MemberPortal];
                PRXUserSSODetailsDTO tokenData = await MemberPortalRepository.GetPRXUserSSOAPSENRPLN(connectionString, request.Token);

                validateUserToken(tokenData);

                var MemberFavoriteContact = await MemberPortalRepository.GetMemberFavoriteContactByMemberContactID(tokenData.ClientID, request.MemberContactID);
                if (MemberFavoriteContact == null || MemberFavoriteContact.UserID == 0)
                {
                    throw new ArgumentException("No MemberContactID was found for the requested UserId, MemberContactID and ClientID");
                }
                else
                {
                    await MemberPortalRepository.MemberFavoriteContactDelete(tokenData.UserID, request.MemberContactID);
                }
                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberContactSaveRequest + "|" + ApiRoutes.MemberContactSaveRequest)]
        public async Task<MemberContactSaveResponse> Post(MemberContactSaveRequest request)
        {
            MemberContactSaveResponse response = new MemberContactSaveResponse();
            response = await processRequest(request, ApiRoutes.MemberContactSaveRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_MemberPortal];
                PRXUserSSODetailsDTO tokenData = await DrugRepository.GetPRXUserSSOAPSENRPLN(connectionString, request.Token);
                validateUserToken(tokenData);
                if (request.MemberContactTypeID == (int)MemberContactType.SavedPharmacy)
                {
                    var recordExists = await MemberPortalRepository.PharmacyByClientIDPHAIDExists(tokenData.ClientID, request.EntityIdentifier?.ToString("0000000"));
                    if (!recordExists)
                    {
                        throw new ArgumentException("No Pharmacy was found for the requested Pharmacy ID/EntityIdentifier and ClientID");
                    }
                }
                if (request.MemberContactID != null && request.MemberContactID >= 0)
                {
                    var MemberFavoriteContact = await MemberPortalRepository.GetMemberFavoriteContactByMemberContactID(tokenData.ClientID, (long)request.MemberContactID);
                    if (MemberFavoriteContact == null || MemberFavoriteContact.UserID == 0)
                    {
                        throw new ArgumentException("No MemberContactID was found for the requested UserId, MemberContactID and ClientID");
                    }
                }
                var isDuplicate = false;
                var MemberFavoriteUser = await MemberPortalRepository.GetMemberFavoriteContactsByUserID(tokenData.ClientID, tokenData.UserID);
                if (request.MemberContactID == null)
                {
                    if (request.MemberContactTypeID == (int)MemberContactType.AddedByMember)
                    {
                        isDuplicate = MemberFavoriteUser.Any(a => a.MemberContactTypeID == request.MemberContactTypeID && (a.ContactPhone == request.ContactPhone || a.ContactName == request.ContactName || a.ContactAddress == request.ContactAddress));
                    }
                    else if (request.MemberContactTypeID == (int)MemberContactType.SavedPharmacy)
                    {
                        isDuplicate = MemberFavoriteUser.Any(a => a.MemberContactTypeID == request.MemberContactTypeID && a.EntityIdentifier == request.EntityIdentifier);
                    }
                }
                else
                {
                    if (request.MemberContactTypeID == (int)MemberContactType.AddedByMember)
                    {
                        isDuplicate = MemberFavoriteUser.Any(a => a.MemberContactTypeID == request.MemberContactTypeID && a.MemberContactID != request.MemberContactID && (a.ContactPhone == request.ContactPhone || a.ContactName == request.ContactName || a.ContactAddress == request.ContactAddress));
                    }
                    else if (request.MemberContactTypeID == (int)MemberContactType.SavedPharmacy)
                    {
                        isDuplicate = MemberFavoriteUser.Any(a => a.MemberContactID != request.MemberContactID && a.MemberContactTypeID == request.MemberContactTypeID && a.EntityIdentifier == request.EntityIdentifier);
                    }
                }
                if (isDuplicate)
                {
                    throw new ArgumentException($"A duplicate record cannot be inserted because a Member Favorite Contact record with UserID '{tokenData.UserID}', MemberContactTypeID '{request.MemberContactTypeID}' and EntityIdentifier '{request.EntityIdentifier}' already exists for User { request.Token}");
                }
                var resp = await MemberPortalRepository.SaveMemberFavoriteContact(tokenData.UserID, request.MemberContactTypeID, request.EntityIdentifier?.ToString("0000000"), request.ContactName, request.ContactAddress, request.ContactPhone, request.MemberContactID);
                response.MemberContactID = resp;
                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.GetMemberContacts + "|" + ApiRoutes.GetMemberContacts)]
        public async Task<GetMemberContactsResponse> Get(GetMemberContactsRequest request)
        {
            GetMemberContactsResponse response = new GetMemberContactsResponse();
            response = await processRequest(request, ApiRoutes.GetMemberContacts, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_MemberPortal];
                PRXUserSSODetailsDTO tokenData = await DrugRepository.GetPRXUserSSOAPSENRPLN(connectionString, request.Token);
                validateUserToken(tokenData);
                List<MemberFavoriteContactDTO> memberFavoriteContactDTO = await MemberPortalRepository.GetMemberFavoriteContactsByUserID(tokenData.ClientID, tokenData.UserID).ConfigureAwait(false);
                response.MemberContacts = new List<MemberContact>();
                foreach (var favContact in memberFavoriteContactDTO)
                {
                    string entityIdentifier = favContact.EntityIdentifier.ToString();
                    if (favContact.MemberContactTypeID == (int)MemberContactType.SavedPharmacy)
                    {
                        entityIdentifier = favContact.EntityIdentifier.ToString("0000000");
                    }
                    response.MemberContacts.Add(new MemberContact
                    {
                        EntityIdentifier = entityIdentifier,
                        ContactName = favContact.ContactName,
                        MemberContactID = (int)favContact.MemberContactID,
                        MemberContactTypeID = favContact.MemberContactTypeID,
                        ContactAddress = favContact.ContactAddress,
                        ContactPhone = favContact.ContactPhone
                    });
                }
                response.UserID = tokenData.UserID;
                return response;
            }).ConfigureAwait(false);
            return response;
        }



        #endregion Member

        #region Claim

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PaidClaimSearchRequest + "|" + ApiRoutes.PaidClaimSearchRequest)]
        public async Task<PaidClaimSearchResponse> Get(PaidClaimSearchRequest request)
        {
            PaidClaimSearchResponse response = new PaidClaimSearchResponse();

            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            response = await processRequest(request, ApiRoutes.PaidClaimSearchRequest, async () =>
            {
                List<ClaimSearchDTO> searchResults = new List<ClaimSearchDTO>();
                List<DatasetDTO> datasets;
                int maxRecords, maxSearchThreads;
                string clientGuid = ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + request.ClientGuid;
                int totalNumberOfRecords = 0;

                //Get Claim Search configurations
                List<string> configKeys = new List<string> { clientGuid, ConfigSetttingKey.PBMAPI_ClaimSearchConfig };
                List<Configuration> configurations = await commonApiHelper.GetConfigurations(configKeys).ConfigureAwait(false);
                GetClaimSearchConfigurations(configurations, clientGuid, out datasets, out maxRecords, out maxSearchThreads);

                if (!string.IsNullOrWhiteSpace(request.Client))
                {
                    datasets = datasets.Where(x => x.Name.EqualsIgnoreCase(request.Client)).ToList();
                }

                //Search daily Claims
                SearchTypeOperator searchTypeOperator = (SearchTypeOperator)getSearchType(request.MemberId, request.MemberIdOperator);
                Tuple<List<ClaimSearchDTO>, int> dailyClaimSearchResults = await DailyClaimSearchClientDatasets(datasets, maxSearchThreads,
                                                                      request.MemberId, searchTypeOperator,
                                                                      request.FillDateFrom, request.FillDateTo).ConfigureAwait(false);
                //Search paid claims
                Tuple<List<ClaimSearchDTO>, int> paidClaimSearchResults = await PaidClaimSearchClientDatasets(datasets, maxSearchThreads,
                                                                        request.MemberId, searchTypeOperator,
                                                                        request.FillDateFrom, request.FillDateTo).ConfigureAwait(false);

                //Subtract any duplicates from the count
                totalNumberOfRecords -= dailyClaimSearchResults.Item2;
                totalNumberOfRecords -= paidClaimSearchResults.Item2;

                searchResults.AddRange(dailyClaimSearchResults.Item1);
                searchResults.AddRange(paidClaimSearchResults.Item1);

                if (searchResults.Count > 0)
                {
                    //Only return top max records configured
                    searchResults = searchResults.OrderByDescending(x => x.ClaimNumber).Take(maxRecords).ToList();

                    //Claim Search Resutls Paging
                    totalNumberOfRecords = await PaidClaimSearchResutlsCount(datasets, maxSearchThreads,
                                                                             request.MemberId, searchTypeOperator, request.FillDateFrom, request.FillDateTo).ConfigureAwait(false) +
                                           await DailyClaimSearchResutlsCount(datasets, maxSearchThreads,
                                                                              request.MemberId, searchTypeOperator, request.FillDateFrom, request.FillDateTo).ConfigureAwait(false);
                }

                response.ClaimSearchResults = ClaimSearchResutlsPaging(request, response, searchResults, maxRecords, maxSearchThreads, totalNumberOfRecords);

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PaidClaimDetailsRequest + "|" + ApiRoutes.PaidClaimDetailsRequest)]
        public async Task<PaidClaimDetailsResponse> Get(PaidClaimDetailsRequest request)
        {
            PaidClaimDetailsResponse response = new PaidClaimDetailsResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.PaidClaimDetailsRequest, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + request.ClientGuid).ConfigureAwait(false);
                Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_PaidClaimDetailsConfig).ConfigureAwait(false);

                List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);
                int maxSearchThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxSearchThreads]);

                if (!string.IsNullOrWhiteSpace(request.Client))
                {
                    datasets = datasets.Where(x => x.Name.EqualsIgnoreCase(request.Client)).ToList();
                }

                // Perform Prescriber search, retrieve results and populate response
                PaidClaimDetailsDTO dailyResult = await getPaidClaimDailyDetails(datasets, maxSearchThreads / 2, request.ClaimNumber).ConfigureAwait(false);
                PaidClaimDetailsDTO historyResult = await getPaidClaimHistoryDetails(datasets, maxSearchThreads / 2, request.ClaimNumber).ConfigureAwait(false);

                PaidClaimDetailsDTO finalResult = dailyResult ?? historyResult;

                if (finalResult != null)
                {
                    DatasetDTO dataset = datasets.First(x => x.Name == finalResult.Client);

                    ClientConfigHelper.ValidateClientAccessLevels(dataset, finalResult.ActualMember.ParentID, finalResult.ActualMember.OrganizationID,
                                                  finalResult.ActualMember.GroupID, finalResult.ActualMember.PlanID);

                    if (finalResult != null)
                    {
                        response = convertToPaidClaimDetailsResult(finalResult, datasets.First(x => x.Name.EqualsIgnoreCase(finalResult.Client)).MemberIdType);
                    }
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ExpandedTestClaimRequest + "|" + ApiRoutes.ExpandedTestClaimRequest)]
        public async Task<ExpandedTestClaimResponse> Post(ExpandedTestClaimRequest request)
        {
            ExpandedTestClaimResponse response = new ExpandedTestClaimResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);


            response = await processRequest(request, ApiRoutes.ExpandedTestClaimRequest, async () =>
            {
                Dictionary<string, string> clientSetting = await ClientConfigHelper.GetClientConfigs(request.ClientGuid).ConfigureAwait(false);
                List<DatasetDTO> datasets = ClientConfigHelper.GetClientDatasets(clientSetting, request.Client, null, null, request.TestClaim.Insurance.GroupId, null);

                //Get configurations
                List<string> configKeys = new List<string> { ConfigSetttingKey.PBMAPI_ClaimsAPI_ConnectionInfo, ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaimDefaultValues };
                List<Configuration> configurations = await commonApiHelper.GetConfigurations(configKeys).ConfigureAwait(false);

                //Get test claim default values
                Dictionary<string, string> defaultValues = configurations.First(x => x.ConfigurationKey == ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaimDefaultValues).ConfigurationValues;

                TestClaimSubmissionRequest requestWithDefaults = populateMissingTestClaimFieldsWithDefaults(request.TestClaim, defaultValues);

                //Get claim api credentials
                var claimApiCredentials = configurations.First(x => x.ConfigurationKey == ConfigSetttingKey.PBMAPI_ClaimsAPI_ConnectionInfo).ConfigurationValues;
                string userName = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Username];
                string password = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Password];
                string url = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_BaseUrl] + ApiRoutes.ClaimSubmissionRequest;

                var output = await ApiHelper.ApiBasicAuthPost<TestClaimSubmissionRequest, TestClaimSubmissionResponse>(url, requestWithDefaults, userName, password).ConfigureAwait(false);

                //Check response
                if (!output.IsSuccessfulStatusCode)
                {
                    if (output.Response.ResponseStatus != null
                        && output.Response.ResponseStatus.ErrorCode.Equals("ArgumentException"))
                    {
                        throw new ArgumentException($"Validation error from Claims API: {output.Response.ResponseStatus.Message}");
                    }
                    else
                    {
                        throw new ApplicationException($"Unexpected error in Claims API");
                    }
                }

                TestClaimSubmissionResponse testClaimResponse = output.Response;

                response.Header = testClaimResponse.Header;
                response.Message = testClaimResponse.Message;
                response.Insurance = testClaimResponse.Insurance;
                response.Patient = testClaimResponse.Patient;
                response.Claim = testClaimResponse.Claim;
                response.COB = testClaimResponse.COB;
                response.DUR = testClaimResponse.DUR;
                response.Pricing = testClaimResponse.Pricing;
                response.Status = testClaimResponse.Status;
                response.NcpdpRequestString = testClaimResponse.NcpdpRequestString;
                response.NcpdpResponseString = testClaimResponse.NcpdpResponseString;
                response.ResponseStatus = testClaimResponse.ResponseStatus;

                if (request.IncludeFeeSchedule)
                {
                    if (!string.IsNullOrWhiteSpace(testClaimResponse.Status.AuthorizationNumber))
                    {
                        string eProcareConnectionString = MultipleConnectionsHelper.GetEProcareConnectionString(datasets.FirstOrDefault().Path);
                        FeeScheduleDTO feeSchedule = await Repository.GetFeeScheduleByNDCREF(eProcareConnectionString, testClaimResponse.Status.AuthorizationNumber).ConfigureAwait(false);
                        mapFeeSchedule(testClaimResponse, feeSchedule);
                    }
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.TestClaimRequest + "|" + ApiRoutes.TestClaimRequest)]
        public async Task<TestClaimResponse> Post(TestClaimRequest request)
        {
            TestClaimResponse response = new TestClaimResponse();


            response = await processRequest(request, ApiRoutes.TestClaimRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                //Get configurations
                List<string> configKeys = new List<string> { ConfigSetttingKey.PBMAPI_ClaimsAPI_ConnectionInfo, ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaimDefaultValues };
                List<Configuration> configurations = await commonApiHelper.GetConfigurations(configKeys).ConfigureAwait(false);

                //Get test claim default values
                Dictionary<string, string> defaultValues = configurations.First(x => x.ConfigurationKey == ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaimDefaultValues).ConfigurationValues;

                //Map default values
                ClaimSubmissionRequest claimRequest = new ClaimSubmissionRequest();
                claimRequest.Header = JsonConvert.DeserializeObject<RequestHeader>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Header]);
                claimRequest.Claim = JsonConvert.DeserializeObject<RequestClaim>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Claim]);
                claimRequest.Compound = JsonConvert.DeserializeObject<RequestCompound>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Compound]);
                claimRequest.DUR = JsonConvert.DeserializeObject<RequestDrugUtilizationReview>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Dur]);
                claimRequest.Prescriber = JsonConvert.DeserializeObject<RequestPrescriber>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Prescriber]);
                claimRequest.Pricing = JsonConvert.DeserializeObject<RequestPricing>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Pricing]);

                //Map client defined values
                claimRequest.Claim.DaysSupply = request.Claim.DaysSupply;
                claimRequest.Claim.ProductIdQualifier = request.Claim.ProductIdQualifier;
                claimRequest.Claim.QuantityDispensed = request.Claim.QuantityDispensed;
                claimRequest.Claim.ProductId = request.Claim.ProductId;
                claimRequest.Header.BinNumber = request.Header.BinNumber;
                claimRequest.Header.DateOfService = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                if (request.Header.DateOfService.HasValue)
                {
                    claimRequest.Header.DateOfService = request.Header.DateOfService.Value;
                }
                if (!string.IsNullOrWhiteSpace(request.Claim.DispenseAsWritten))
                {
                    claimRequest.Claim.DispenseAsWritten = request.Claim.DispenseAsWritten[0];
                }
                claimRequest.Header.ServiceProviderId = request.Header.ServiceProviderId;
                claimRequest.Header.ServiceProviderIdQualifier = request.Header.ServiceProviderIdQualifier;
                if (!string.IsNullOrWhiteSpace(request.Header.ProcessorControlNumber))
                {
                    claimRequest.Header.ProcessorControlNumber = request.Header.ProcessorControlNumber;
                }
                claimRequest.Insurance.CardholderId = request.Insurance.CardholderId;
                claimRequest.Insurance.GroupId = request.Insurance.GroupId;
                claimRequest.Insurance.PersonCode = request.Insurance.PersonCode;
                claimRequest.Insurance.PatientRelationshipCode = request.Insurance.PatientRelationshipCode;
                claimRequest.Patient.DateOfBirth = request.Patient.DateOfBirth;
                claimRequest.Patient.FirstName = request.Patient.FirstName;
                claimRequest.Patient.LastName = request.Patient.LastName;
                claimRequest.Patient.Gender = request.Patient.Gender;
                claimRequest.IsInternalClaim = request.IsInternalClaim;

                //Call claims api to test claim
                response = await SubmitTestClaim(claimRequest, commonApiHelper, configurations).ConfigureAwait(false);

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        #endregion

        #region Pharmacy
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PharmacySearchRequest + "|" + ApiRoutes.PharmacySearchRequest)]
        public async Task<PharmacySearchResponse> Get(PharmacySearchRequest request)
        {
            PharmacySearchResponse response = new PharmacySearchResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.PharmacySearchRequest, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + request.ClientGuid).ConfigureAwait(false);
                Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_PharmacySearchConfig).ConfigureAwait(false);

                List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);

                if (!string.IsNullOrWhiteSpace(request.Client))
                {
                    datasets = datasets.Where(x => x.Name.EqualsIgnoreCase(request.Client)).ToList();
                }

                int maxRecords = Convert.ToInt32(config[ConfigSetttingKey.ApiMaxNumberOfRecordsSetting_KeyValue]);
                int maxSearchThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxSearchThreads]);
                int defaultMiles = Convert.ToInt32(config[ConfigSetttingKey.APIPBM_PharmacySearch_DefaultMiles]);

                if (request.ItemsPerPage == null)
                {
                    request.ItemsPerPage = maxRecords;
                }

                if (request.PageNumber == null)
                {
                    request.PageNumber = 1;
                }

                if (request.WithinMiles == null)
                {
                    request.WithinMiles = defaultMiles;
                }

                //Filter by distance, all possible clientIDs, 24 hour, and optional PlanID
                Task<List<PharmacyNetworkDTO>> pharmacyNetworkResultsTask =
                    pharmacySearchGetBatchWithinDistance(datasets, maxSearchThreads / 2, request.Zip, request.WithinMiles.GetValueOrDefault(),
                                                         request.Open24Hours, request.Flex90, request.PlanID);

                //Call get count sproc
                Task<int> totalNumberOfRecordsTask = pharmacySearchGetCount(datasets, maxSearchThreads / 2, request.Zip,
                                                                            request.WithinMiles.GetValueOrDefault(), request.Open24Hours, request.Flex90,
                                                                            request.PlanID);

                List<PharmacyNetworkDTO> pharmacyNetworkResults = await pharmacyNetworkResultsTask.ConfigureAwait(false);
                int totalNumberOfRecords = await totalNumberOfRecordsTask.ConfigureAwait(false);

                var groupedSearchResults = pharmacyNetworkResults.GroupBy(x => new { x.Pharmacy.PharmacyID, x.Pharmacy.Client }).ToList();

                List<PharmacySearchDTO> searchResults = new List<PharmacySearchDTO>();

                groupedSearchResults.ForEach(x =>
                {
                    searchResults.Add(new PharmacySearchDTO
                    {
                        Pharmacy = x.First().Pharmacy,
                        Networks = x.Select(y => y.Network).OrderBy(y => y.NetworkID).ToList()
                    });
                });

                searchResults = searchResults.OrderBy(x => x.Pharmacy.Distance).ToList();

                List<PharmacySearchResult> PharmacySearchResults = new List<PharmacySearchResult>();
                PagingOptionsResponse pagingOption = new PagingOptionsResponse();

                response.PharmacySearchResults = PharmacySearchResults;

                PharmacySearchDTO firstItem = searchResults.FirstOrDefault();


                if (request.ItemsPerPage >= totalNumberOfRecords)
                {
                    //Add all
                    searchResults.ForEach(x =>
                    {
                        PharmacySearchResult result = convertToPharmacySearchResult(x);
                        PharmacySearchResults.Add(result);
                    });
                }
                else
                {
                    //Add the current page
                    searchResults
                        .Skip(request.ItemsPerPage.Value * (request.PageNumber.Value - 1)) //Skip previous pages
                        .Take(request.ItemsPerPage.Value) //Get the current page of results
                        .ToList()
                        .ForEach(x => { PharmacySearchResults.Add(convertToPharmacySearchResult(x)); });
                }


                if (firstItem != null)
                {
                    if (PharmacySearchResults.Count < request.ItemsPerPage && PharmacySearchResults.Count < maxRecords)
                    {
                        totalNumberOfRecords = PharmacySearchResults.Count;
                    }

                    pagingOption.PageNumber = request.PageNumber;
                    pagingOption.ItemsPerPage = request.ItemsPerPage;
                    pagingOption.TotalNumberOfPages = totalNumberOfRecords / request.ItemsPerPage;
                    if (totalNumberOfRecords % request.ItemsPerPage > 0)
                    {
                        pagingOption.TotalNumberOfPages++;
                    }
                    pagingOption.TotalNumberOfRecords = totalNumberOfRecords;
                }
                else
                {
                    pagingOption.PageNumber = 1;
                    pagingOption.ItemsPerPage = 0;
                    pagingOption.TotalNumberOfPages = 1;
                    pagingOption.TotalNumberOfRecords = 0;
                }

                response.PagingOptions = pagingOption;

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PharmacyLocatorRequest + "|" + ApiRoutes.PharmacyLocatorRequest)]
        public async Task<PharmacyLocatorResponse> Get(PharmacyLocatorRequest request)
        {
            PharmacyLocatorResponse response = new PharmacyLocatorResponse();

            response = await processRequest(request, ApiRoutes.PharmacyLocatorRequest, async () =>
            {
                List<PharmacySearchDTO> pharmacySearchResults =
                    await PharmacyLocatorRepository.GetTop10WithinDistance(request.Zip, request.WithinMiles.GetValueOrDefault()).ConfigureAwait(false);

                pharmacySearchResults
                    .ForEach(x => { response.PharmacySearchResults.Add(convertToPharmacySearchResult(x)); });

                return response;
            }).ConfigureAwait(false);

            return response;
        }
        #endregion Pharmacy

        #region Prescriber
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PrescriberSearchRequest + "|" + ApiRoutes.PrescriberSearchRequest)]
        public async Task<PrescriberSearchResponse> Get(PrescriberSearchRequest request)
        {
            SearchTypeOperator? lastNameOperator = getSearchType(request.LastName, request.LastNameOperator);
            SearchTypeOperator? firstNameOperator = getSearchType(request.FirstName, request.FirstNameOperator);

            PrescriberSearchResponse response = new PrescriberSearchResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.PrescriberSearchRequest, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_PrescriberSearchConfig).ConfigureAwait(false);

                int maxRecords = Convert.ToInt32(config[ConfigSetttingKey.ApiMaxNumberOfRecordsSetting_KeyValue]);

                if (request.ItemsPerPage == null)
                {
                    request.ItemsPerPage = maxRecords;
                }

                if (request.PageNumber == null)
                {
                    request.PageNumber = 1;
                }

                if (request.IncludeDeactivatedPrescribers == null)
                {
                    request.IncludeDeactivatedPrescribers = false;
                }

                // Perform Prescriber search, retrieve results and populate response
                int totalNumberOfRecords = await PrescriberSearchGetCount(request.PrescriberId,
                                                                       request.LastName, lastNameOperator, request.FirstName,
                                                                       firstNameOperator, request.IncludeDeactivatedPrescribers.Value).ConfigureAwait(false);

                List<PrescriberSearchDTO> searchResults = await PrescriberSearch(request.PrescriberId,
                                                                                       request.LastName, lastNameOperator, request.FirstName,
                                                                                       firstNameOperator, request.IncludeDeactivatedPrescribers.Value)
                                                                                       .ConfigureAwait(false);

                //Only return top max records, FamilyId is CardID + CardID2
                searchResults = searchResults.OrderBy(x => x.PrescriberId).Take(maxRecords).ToList();

                List<PrescriberSearchResult> PrescriberSearchResults = new List<PrescriberSearchResult>();
                PagingOptionsResponse pagingOption = new PagingOptionsResponse();

                response.PrescriberSearchResults = PrescriberSearchResults;

                PrescriberSearchDTO firstItem = searchResults.FirstOrDefault();

                if (request.ItemsPerPage >= totalNumberOfRecords)
                {
                    //Add all
                    searchResults.ForEach(x =>
                    {
                        PrescriberSearchResult result = convertToPrescriberSearchResult(x);
                        PrescriberSearchResults.Add(result);
                    });
                }
                else
                {
                    //Add the current page
                    searchResults
                        .Skip(request.ItemsPerPage.Value * (request.PageNumber.Value - 1)) //Skip previous pages
                        .Take(request.ItemsPerPage.Value) //Get the current page of results
                        .ToList()
                        .ForEach(x => { PrescriberSearchResults.Add(convertToPrescriberSearchResult(x)); });
                }


                if (firstItem != null)
                {
                    pagingOption.PageNumber = request.PageNumber;
                    pagingOption.ItemsPerPage = request.ItemsPerPage;
                    pagingOption.TotalNumberOfPages = totalNumberOfRecords / request.ItemsPerPage;
                    if (totalNumberOfRecords % request.ItemsPerPage > 0)
                    {
                        pagingOption.TotalNumberOfPages++;
                    }
                    pagingOption.TotalNumberOfRecords = totalNumberOfRecords;
                }
                else
                {
                    pagingOption.PageNumber = 1;
                    pagingOption.ItemsPerPage = 0;
                    pagingOption.TotalNumberOfPages = 1;
                    pagingOption.TotalNumberOfRecords = 0;
                }

                response.PagingOptions = pagingOption;

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PrescriberDetailRequest + "|" + ApiRoutes.PrescriberDetailRequest)]
        public async Task<PrescriberDetailResponse> Get(PrescriberDetailRequest request)
        {
            PrescriberDetailResponse response = new PrescriberDetailResponse();

            string prescriberID = request.PrescriberID;
            PhysicianQualifier qualifier = request.Qualifier;

            response = await processRequest(request, ApiRoutes.PrescriberDetailRequest, async () =>
            {
                if (request.Qualifier == PhysicianQualifier.DEA)
                {
                    //Find NPI
                    string npi = await PrescriberRepository.GetPrescriberNPI(request.PrescriberID).ConfigureAwait(false);
                    if (!(string.IsNullOrEmpty(npi)))
                    {
                        //Reset paramaters to get physician info by npi
                        prescriberID = npi;
                        qualifier = PhysicianQualifier.NPI;
                    }
                }

                //Prescriber Detail
                PrescriberDetailDTO dto = await PrescriberRepository.GetPrescriberDetails(prescriberID, qualifier).ConfigureAwait(false);
                response = convertToPrescriberDetailResponse(dto);
                response.PrescriberID = request.PrescriberID;

                //Prescriber Alternate Addresses
                response.AlternateAddresses = new List<PrescriberAddress>();
                List<PrescriberAddressDTO> prescriberAddressDtos = await PrescriberRepository.GetPrescriberAlternateAddress(prescriberID, qualifier).ConfigureAwait(false);
                prescriberAddressDtos.ForEach(x =>
                {
                    response.AlternateAddresses.Add(x.ConvertTo<PrescriberAddress>());
                });

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        #endregion Prescriber

        #region Rule

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.DynamicPARequest + "|" + ApiRoutes.DynamicPARequest)]
        public async Task<DynamicPAResponse> Post(DynamicPARequest request)
        {
            DynamicPAResponse response = new DynamicPAResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.DynamicPARequest, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + request.ClientGuid).ConfigureAwait(false);

                List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);
                DatasetDTO dataset = datasets.FirstOrDefault(x => x.Name.EqualsIgnoreCase(request.Client));
                ClientConfigHelper.ValidateClientAccessLevels(dataset, null, null, request.GroupID, request.PlanID);

                string clientConnectionString = getDatasetConnectionString(dataset);
                string userID = ClientConfigHelper.GetUserIDFromClientGuid(request.ClientGuid);

                RuleTemplateDTO ruleTemplate = await getRuleTemplate(clientConnectionString, dataset?.Name, request).ConfigureAwait(false);

                validatePharmacy(clientConnectionString, request.PharmacyID, ruleTemplate.PPNID);

                List<string> enrolleeIds = await getEnrolleeId(clientConnectionString, dataset.MemberIdType, request).ConfigureAwait(false);

                string enrolleeId = enrolleeIds.First();

                if (!isValidMember(clientConnectionString, request.PlanID,
                                  !string.IsNullOrWhiteSpace(request.MemberEnrolleeID) ? request.MemberEnrolleeID : enrolleeId,
                                  out string errorMessage))
                {
                    throw new ArgumentException(errorMessage);
                }

                response.MemberID = request.MemberID;
                response.MemberEnrolleeID = enrolleeId;
                response.OrganizationID = request.OrganizationID;
                response.GroupID = request.GroupID;
                response.PlanID = request.PlanID;
                response.VendorPANumber = request.VendorPANumber;
                //response.ProductIDQualifier = getProductIDQualifier(ruleTemplate.CODETYPE);
                //response.ProductIDQualifierDescription = ruleTemplate.CODETYPE;
                //response.ProductID = ruleTemplate.CODES;
                //Per Sandi, reply with what was sent instead of what was written on the rule
                response.ProductIDQualifier = request.ProductIDQualifier;
                response.ProductIDQualifierDescription = getProductIDQualifierDescription(request.ProductIDQualifier);
                response.ProductID = request.ProductID;
                response.ProductDescription = ruleTemplate.LN60;
                response.ProcarePANumber = new List<string>();

                string retailSysid = "", mailSysid = "";
                bool ruleAlreadyExists = false;

                if (dataset.ShowClaimsForAllMemberPlans)
                {
                    bool exists = false;
                    foreach (var enrid in enrolleeIds)
                    {
                        if (memberHasRule(clientConnectionString, request.PlanID, enrid, ruleTemplate.VENDTYPE,
                                            ruleTemplate.CODETYPE, ruleTemplate.CODES, dataset.ShowClaimsForAllMemberPlans,
                                            out retailSysid, out mailSysid))
                        {
                            exists = true;
                            break;
                        }
                    }

                    ruleAlreadyExists = exists;
                }
                else
                {
                    ruleAlreadyExists = memberHasRule(clientConnectionString, request.PlanID, enrolleeId, ruleTemplate.VENDTYPE,
                                                        ruleTemplate.CODETYPE, ruleTemplate.CODES, dataset.ShowClaimsForAllMemberPlans,
                                                        out retailSysid, out mailSysid);
                }

                //Check if member already has this rule
                if (ruleAlreadyExists)
                {
                    //Update rule
                    if (!string.IsNullOrEmpty(mailSysid))
                    {
                        response.ProcarePANumber.Add(request.Client.ToUpper() +
                                                        await saveMemberRule(clientConnectionString, dataset.Name, mailSysid, enrolleeId, request,
                                                                            ruleTemplate, nameof(VendorType.MAIL), userID, request.OrganizationID,
                                                                            dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
                    }

                    if (!string.IsNullOrEmpty(retailSysid) && !dataset.ShowClaimsForAllMemberPlans)
                    {
                        response.ProcarePANumber.Add(request.Client.ToUpper() +
                                                        await saveMemberRule(clientConnectionString, dataset.Name, retailSysid, enrolleeId, request,
                                                                            ruleTemplate, nameof(VendorType.RETL), userID, request.OrganizationID,
                                                                            dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
                    }
                }
                else
                {
                    //Add rule
                    if (!dataset.ShowClaimsForAllMemberPlans && ruleTemplate.VENDTYPE.EqualsIgnoreCase(nameof(VendorType.BOTH)))
                    {
                        response.ProcarePANumber.Add(request.Client.ToUpper() +
                                                        await saveMemberRule(clientConnectionString, dataset.Name, null, enrolleeId, request,
                                                                            ruleTemplate, nameof(VendorType.MAIL), userID, request.OrganizationID,
                                                                            dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
                        response.ProcarePANumber.Add(request.Client.ToUpper() +
                                                        await saveMemberRule(clientConnectionString, dataset.Name, null, enrolleeId, request,
                                                                            ruleTemplate, nameof(VendorType.RETL), userID, request.OrganizationID,
                                                                            dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
                    }
                    else
                    {
                        response.ProcarePANumber.Add(request.Client.ToUpper() +
                                                        await saveMemberRule(clientConnectionString, dataset.Name, null, enrolleeId, request,
                                                                            ruleTemplate, ruleTemplate.VENDTYPE, userID, request.OrganizationID,
                                                                            dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
                    }
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        #endregion Rule

        #region EligibilityImport

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.EligibilityImport + "|" + ApiRoutes.EligibilityImport)]
        public async Task<EligibilityImportResponse> Post(EligibilityImportRequest request)
        {
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            EligibilityImportResponse response = await processRequest(request, ApiRoutes.EligibilityImport, async () =>
            {
                response = new EligibilityImportResponse();

                string xmlRequest = generateEligibilityImportXML(request);
                ClientWithConfigurationsDTO clientWithConfigs =
                await getClientWithConfigurations(commonApiHelper, ConfigSetttingKey.PBMAPIMemberEligibilityClientConfigurations,
                                                    request.ClientID).ConfigureAwait(false);


                var import = new Import
                {
                    RawData = xmlRequest,
                    ImportStatusID = (long)ImportStatus.Open,
                    ClientID = clientWithConfigs.ClientID,
                    TransactionTypeID = clientWithConfigs.TransactionTypeID,
                    CreatedTime = DateTime.Now,
                    InsertAppUserID = clientWithConfigs.ClientInsertAppUserID,
                    InsertTime = DateTime.Now,
                    UpdateAppUserID = clientWithConfigs.ClientInsertAppUserID,
                    UpdateTime = DateTime.Now,
                };

                long importID = saveImportToDatabase(import);
                import.ImportID = importID;
                import.UpdateAppUserID = clientWithConfigs.ClientUpdateAppUserID;

                processImportRequest(request, import, clientWithConfigs);

                response.Action = import.RecordAction;
                response.ErrorMessage = import.ErrorMessage ?? string.Empty;
                response.ImportID = 0;
                response.RecordID = import.ImportID.ToString();
                response.ReturnValue = import.ReturnValue;
                response.Warnings = import.WarningMessage ?? string.Empty;

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        #endregion

        #region AccumulatorImport
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.AccumulatorImport + "|" + ApiRoutes.AccumulatorImport)]
        public async Task<AccumulatorImportResponse> Post(AccumulatorImportRequest request)
        {
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            AccumulatorImportResponse response = await processRequest(request, ApiRoutes.AccumulatorImport, async () =>
                                             {
                                                 response = new AccumulatorImportResponse();

                                                 string xmlRequest = generateAccumulatorImportXML(request);

                                                 ClientWithConfigurationsDTO clientWithConfigs =
                                                     await getClientWithConfigurations(commonApiHelper,
                                                                                       ConfigSetttingKey.PBMAPIMemberAccumulatorClientConfigurations,
                                                                                       request.ClientID).ConfigureAwait(false);

                                                 var import = new Import
                                                 {
                                                     RawData = xmlRequest,
                                                     ImportStatusID = (long)ImportStatus.Open,
                                                     ClientID = clientWithConfigs.ClientID,
                                                     TransactionTypeID = clientWithConfigs.TransactionTypeID,
                                                     RecordID = request.RecordID.ToString(),
                                                     CreatedTime = DateTime.Now,
                                                     InsertAppUserID = clientWithConfigs.ClientInsertAppUserID,
                                                     InsertTime = DateTime.Now,
                                                     UpdateAppUserID = clientWithConfigs.ClientInsertAppUserID,
                                                     UpdateTime = DateTime.Now,
                                                     ReturnValue = "",
                                                     RecordAction = ""
                                                 };

                                                 long importID = saveImportToDatabase(import);
                                                 import.ImportID = importID;
                                                 import.UpdateAppUserID = clientWithConfigs.ClientUpdateAppUserID;

                                                 processImportRequest(request, import, clientWithConfigs);

                                                 response.Action = import.RecordAction;
                                                 response.ErrorMessage = import.ErrorMessage ?? string.Empty;
                                                 response.ImportID = 0;
                                                 response.RecordID = import.ImportID.ToString();
                                                 response.ReturnValue = import.ReturnValue;
                                                 response.Warnings = import.WarningMessage ?? string.Empty;

                                                 return response;
                                             }).ConfigureAwait(false);

            return response;
        }
        #endregion

        #region ACPImport
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ACPImport + "|" + ApiRoutes.ACPImport)]
        public async Task<ACPImportResponse> Post(ACPImportRequest request)
        {
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            ACPImportResponse response = await processRequest(request, ApiRoutes.ACPImport, async () =>
            {
                response = new ACPImportResponse();

                ClientACPDTO client =
                    await getClientACPConfigurations(commonApiHelper, ConfigSetttingKey.PBMAPIMemberAccumulatorClientConfigurations, request.ClientID)
                        .ConfigureAwait(false);

                // If we have a match on the URL allow processing.
                if (client.Client_ACPID > 0)
                {
                    //Full ACP replacement if FullReplacement = 1 in Client_ACP table, update otherwise
                    TransactionType requestTransactionType =
                        client.FullReplacement ? TransactionType.ACPFull : TransactionType.ACPUpdate;

                    var import = new ACPImport
                    {
                        RawData = JsonConvert.SerializeObject(request),
                        ImportStatusID = (long)ImportStatus.Open,
                        ClientID = client.Client_ACPID,
                        TransactionTypeID = (long)requestTransactionType,
                        CreatedTime = DateTime.Now,
                        RecIdentifier = request.RecIdentifier,
                        InsertAppUserID = client.InsertAppUserID,
                        InsertTime = DateTime.Now,
                        UpdateAppUserID = client.InsertAppUserID,
                        UpdateTime = DateTime.Now
                    };

                    long importID = saveACPImportToDatabase(import);
                    import.Import_ACPID = importID;

                    if (!DateTime.TryParse(request.AdjustmentDate, out DateTime adjustmentDate))
                    {
                        adjustmentDate = DateTime.MinValue;
                    }

                    // Validate the incoming values.
                    bool planIDValid = isValidPlanID(client.ADSDatabasePath, request.PlanID, out string planErrorMessage, true);
                    bool planInDateValid = isPlanInDate(client.ADSDatabasePath, request.PlanID, adjustmentDate, out string planDateErrorMessage);
                    bool cardIDValid = isValidCardID(client.ADSDatabasePath, request.PlanID, request.CardholderID, request.Person, out string cardErrorMessage, true);

                    bool acpAmountValid = true;
                    string acpAmountErrorMessage = "";

                    if (client.FullReplacement)
                    {
                        acpAmountValid = isValidACPAmount(request.MedicalDeductible, request.MedicalMaximumBenefitAmount, request.MedicalOutOfPocketAmount, out acpAmountErrorMessage);
                    }

                    if (!planIDValid || !planInDateValid || !cardIDValid || !acpAmountValid)
                    {
                        List<string> errorMessages = new List<string>
                        {
                            planErrorMessage,
                            planDateErrorMessage,
                            cardErrorMessage,
                            acpAmountErrorMessage
                        };

                        string errorMessage = getFullErrorMessage(errorMessages, true);

                        import.ImportStatusID = (long)ImportStatus.ParseError;
                        import.ErrorMessage = errorMessage;
                        import.CompletedTime = DateTime.Now;

                        saveACPImportToDatabase(import);

                        throw new ArgumentException(errorMessage);
                    }

                    import.ImportStatusID = (long)ImportStatus.Parsed;
                    saveACPImportToDatabase(import);

                    // Do the ACP processing here...                    
                    var member = EligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(client.ADSDatabasePath, request.PlanID, getCardID(request.CardholderID), getCardID2(request.CardholderID),
                                                                                    request.Person).FirstOrDefault();

                    string enrid = member?.ENRID;
                    DateTime memberSince = member?.MBRSINCE ?? DateTime.MinValue;

                    if (memberSince == DateTime.MinValue)
                    {
                        //attempt to get MemberSince for Person 01 if none found for current Person
                        memberSince = EligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(
                            client.ADSDatabasePath, request.PlanID, getCardID(request.CardholderID),
                            getCardID2(request.CardholderID),
                            "01").FirstOrDefault()?.MBRSINCE ?? DateTime.MinValue;
                    }

                    DateTime startDate = DateTime.MinValue;
                    DateTime endDate = DateTime.MinValue;

                    populateAccumulatorDates(client.ADSDatabasePath, enrid, ref startDate, ref endDate, request.PlanID, adjustmentDate, memberSince);

                    if (client.FullReplacement)
                    {
                        try
                        {
                            EligibilityRepository.ReplaceAccumulator(client.ADSDatabasePath, adjustmentDate, startDate, endDate, request.PlanID, enrid,
                                                            request.MedicalDeductible, request.MedicalOutOfPocketAmount,
                                                            request.MedicalMaximumBenefitAmount, client.UserName);
                            response.Action = "Full";
                        }
                        catch (Exception)
                        {
                            response.ResponseError = new Response_Error();
                            response.ResponseError.Message = "Internal Processing Error";
                            response.ResponseError.Exception = "Full Update Medical Failed";
                            response.ResponseError.Code = "2";
                        }
                    }
                    else
                    {
                        try
                        {
                            EligibilityRepository.UpdateAccumulator(client.ADSDatabasePath, adjustmentDate, startDate, endDate, request.PlanID, enrid,
                                                         request.MedicalDeductible, request.MedicalOutOfPocketAmount,
                                                         request.MedicalMaximumBenefitAmount, client.UserName);
                            response.Action = "Update";
                        }
                        catch (Exception)
                        {
                            response.ResponseError = new Response_Error();
                            response.ResponseError.Message = "Internal Processing Error";
                            response.ResponseError.Exception = "Update Medical Failed";
                            response.ResponseError.Code = "4";
                        }
                    }

                    if (string.IsNullOrWhiteSpace(response.ResponseError?.Message))
                    {
                        response.ReturnValue = "Success";
                        try
                        {
                            response.NewBalance =
                                EligibilityRepository.GetNewBalance(client.ADSDatabasePath, adjustmentDate, request.PlanID, enrid);
                        }
                        catch (Exception e)
                        {
                            response.Warnings = "Cannot calculate new balance value: " + e.Message;
                        }
                    }

                    response.ImportID = import.Import_ACPID.ToString();
                    response.RecIdentifier = request.RecIdentifier;

                    if (String.IsNullOrEmpty(response.ErrorMessage) && response.ResponseError == null)
                    {
                        import.ImportStatusID = (long)ImportStatus.Closed;
                    }
                    else
                    {
                        import.ImportStatusID = (long)ImportStatus.ProcessError;
                    }

                    saveACPImportToDatabase(import);

                    // Then close out the record...
                    import.ReturnValue = response.ReturnValue;
                    import.WarningMessage = response.Warnings;
                    import.ErrorMessage = response.ErrorMessage;
                    //import.RecIdentifier = request.RecIdentifier;
                    import.RecordAction = response.Action;
                    import.NewBalance = response.NewBalance;
                    import.CompletedTime = DateTime.Now;

                    saveACPImportToDatabase(import);

                    //response.RecIdentifier = request.RecIdentifier;
                }
                else
                {
                    response.ResponseError = new Response_Error();
                    response.ResponseError.Code = "InboundRecord";
                    response.ResponseError.Message = "The client ID is invalid.";
                    response.ResponseError.Exception = "No record corresponding to the clientID could be found.";
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }
        #endregion ACPImport

        #region Drug

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.DrugMonographRequest + "|" + ApiRoutes.DrugMonographRequest)]
        public async Task<DrugMonographResponse> Get(DrugMonographRequest request)
        {
            DrugMonographResponse response = new DrugMonographResponse();


            response = await processRequest(request, ApiRoutes.DrugMonographRequest, async () =>
            {

                MonographDTO dto = new MonographDTO();
                dto = await MediSpanRepository.GetDrugMonograph((int)request.ProductQualifier, request.Productid, request.LanguageCode).ConfigureAwait(false);

                response.Monograph = string.Empty;
                response.Copyright = dto.Copyright;

                if (dto.Monograph != null)
                {
                    switch (request.LanguageCode)
                    {
                        case LanguageCode.English:
                            response.Monograph = Convert.ToBase64String(Encoding.ASCII.GetBytes(dto.Monograph));
                            break;
                        case LanguageCode.Spanish:
                            response.Monograph = Convert.ToBase64String(Encoding.GetEncoding("windows-1252").GetBytes(dto.Monograph));
                            break;
                        default:
                            break;
                    }
                }

                switch (request.ProductQualifier)
                {
                    case ProductQualifier.NDC:
                        response.NDC = request.Productid;
                        break;
                    case ProductQualifier.GPI:
                        response.GPI14 = request.Productid;
                        break;
                    default:
                        break;
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }
        #endregion

        #region MemberPortal

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberPortalRegistrationRequest + "|" + ApiRoutes.MemberPortalRegistrationRequest)]
        public async Task<MemberPortalRegistrationResponse> Post(MemberPortalRegistrationRequest request)
        {
            MemberPortalRegistrationResponse response = new MemberPortalRegistrationResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            response = await processRequest(request, ApiRoutes.MemberPortalRegistrationRequest, async () =>
            {
                response = new MemberPortalRegistrationResponse();
                int domainID;
                string clientHostUrl;
                Dictionary<string, string> emailConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_DefaultEmailConfig).ConfigureAwait(false);
                Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MemberPortal_RegistrationConfig).ConfigureAwait(false);

                string smtpServer = emailConfig[ConfigSetttingKey.PBMAPI_DefaultEmailConfig_SMTPServer];
                int minLoginAge = int.Parse(config[ConfigSetttingKey.PBMAPI_MemberPortal_RegistrationConfig_MinimumLoginAge]);
                ClientInfoDTO clientInfo = await DateWarehouseRepository.LookupClientInfo(request.BinNumber).ConfigureAwait(false);

                //Get DomainID from DomainName if populated
                if (!string.IsNullOrWhiteSpace(request.DomainName))
                {
                    domainID = getDomainIDFromDomainName(request.DomainName);
                    clientHostUrl = request.DomainName;
                }
                //If not, we'll be using the defaults for the client
                else
                {
                    domainID = clientInfo.DefaultDomainID;
                    clientHostUrl = clientInfo.DomainName;
                }

                //Verify BIN allows online access
                verifyBINAccess(request.BinNumber);

                //Lookup potential matches for the enrollee
                List<MemberPortalEnrolleeDTO> enrolleeList = await DateWarehouseRepository.LookupEnrollee(
                    clientInfo.ClientID, domainID, request.CardID, request.DateOfBirth,
                    request.Gender.ToString(), clientInfo.ParentID, clientInfo.OrgID, clientInfo.GroupID, clientInfo.Class).ConfigureAwait(false);

                if (enrolleeList.Any())
                {
                    MemberPortalEnrolleeDTO enrollee = enrolleeList.First();

                    //Multiple matching enrollees found: Match dependent first name if present or return the options if not
                    if (enrolleeList.Count > 1)
                    {
                        enrollee = enrolleeList.FirstOrDefault(x => x.FirstName.Trim().EqualsIgnoreCase(request.DependentFirstName ?? ""));
                    }

                    //One matching enrollee found: Register
                    if (!string.IsNullOrWhiteSpace(enrollee?.EnrolleeID))
                    {
                        if (enrollee.DateOfBirth > DateTime.Today.AddYears(minLoginAge * -1) &
                           enrolleeList.Count > 1)
                        {
                            throw new ArgumentException($"You must be at least {minLoginAge} years old to register.");
                        }

                        registerMemberPortalUser(clientInfo.ClientID, domainID, request.BinNumber, enrollee.EnrolleeID, enrollee.PlanID,
                                                 request.EmailAddress, request.Username, request.VerifyQuestion, request.VerifyAnswer,
                                                 enrollee.FirstName, enrollee.LastName, clientInfo.Name, clientHostUrl, smtpServer);

                        response.ResponseStatus = new ResponseStatus { Message = "Member registered successfully.  An email with the member's temporary password has been sent." };
                    }
                    else
                    {
                        //If we couldn't narrow the dependent list down to one, return an error with dependent names to choose from
                        response.DependentFirstNameOptions = enrolleeList.Select(x => x.FirstName).OrderBy(x => x).ToList();
                        response.ResponseStatus = new ResponseStatus { Message = "Multiple dependents were found for the enrollee.  Please indicate the first name of the correct dependent to register." };
                    }
                }
                else
                {
                    //No matching enrollees found: ERROR
                    throw new ArgumentException("No enrollee with the supplied values could be found.");
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberPortalLoginRequest + "|" + ApiRoutes.MemberPortalLoginRequest)]
        public async Task<MemberPortalLoginResponse> Post(MemberPortalLoginRequest request)
        {
            MemberPortalLoginResponse response = new MemberPortalLoginResponse();

            response = await processRequest(request, ApiRoutes.MemberPortalLoginRequest, async () =>
            {
                response = new MemberPortalLoginResponse();

                int clientID = getClientIDFromDomainName(request.DomainName);

                MemberPortalUserDTO loginDto = await DateWarehouseRepository.LookupUser(clientID, request.Username, request.Password).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(loginDto.EnrolleeID))
                {
                    throw new ArgumentException("The login is not valid.");
                }

                List<MemberPortalSSOValidationDTO> ssoDtoList =
                    await DateWarehouseRepository.GetMembers(clientID, loginDto.EnrolleeID).ConfigureAwait(false);

                if (ssoDtoList.Count == 0)
                {
                    throw new ArgumentException("No enrollees found for this login.");
                }

                MemberPortalSSOValidationDTO ssoDto = ssoDtoList.FirstOrDefault(x => (x.EffectiveDate.HasValue && x.EffectiveDate.Value <= DateTime.Today) &&
                                                                            (!x.TerminationDate.HasValue || x.TerminationDate.Value > DateTime.Today));

                if (!string.IsNullOrWhiteSpace(ssoDto.CardID))
                {
                    ValidUserLoginDTO validatedUserDto = await MemberPortalRepository.Login(ssoDto.CardID, ssoDto.CardID2, "01", loginDto.EnrolleeID,
                                                                          loginDto.BinNumber, clientID, loginDto.DomainID).ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(validatedUserDto.Token))
                    {
                        throw new ArgumentException("Login failed.");
                    }

                    // 1. Generate Token for Response
                    response.Token = validatedUserDto.Token.ToUpper();
                    response.BinNumber = loginDto.BinNumber;
                    response.EnrolleeID = loginDto.EnrolleeID;
                    response.CardID1And2 = validatedUserDto.CardID1And2;
                    response.PlanID = validatedUserDto.PLNID;
                    response.GroupID = validatedUserDto.GRPID;
                    response.OrganizationID = validatedUserDto.ORGID;
                    response.ParentID = validatedUserDto.PARID;
                    response.ProcessorControlNumber = validatedUserDto.PCN;
                    response.SubgroupID = validatedUserDto.SUBGRPID;
                    response.PlanEffectiveDate = validatedUserDto.PlanEffDate;
                    response.PlanTerminationDate = validatedUserDto.PlanTermDate;
                    response.GroupEffectiveDate = validatedUserDto.GroupEffDate;
                    response.GroupTerminationDate = validatedUserDto.GroupTermDate;
                    response.FirstName = validatedUserDto.FirstName;
                    response.MiddleName = validatedUserDto.MiddleName;
                    response.LastName = validatedUserDto.LastName;
                    response.Token = response.Token.ToUpper();
                    response.DateOfBirth = validatedUserDto.DateOfBirth;
                    response.Gender = validatedUserDto.Gender.ToString();
                   
                    // 2. Login Successful
                    await DateWarehouseRepository.LoginSuccessful(loginDto.UserID);
                }
                else
                {
                    throw new ArgumentException("The enrollee is termed.");
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberPortalPasswordResetRequest + "|" + ApiRoutes.MemberPortalPasswordResetRequest)]
        public async Task<MemberPortalPasswordResetResponse> Post(MemberPortalPasswordResetRequest request)
        {
            MemberPortalPasswordResetResponse response = new MemberPortalPasswordResetResponse();
            response = await processRequest(request, ApiRoutes.MemberPortalLoginRequest, async () =>
            {

                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                int domainID;
                string clientHostUrl;

                Dictionary<string, string> emailConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_DefaultEmailConfig).ConfigureAwait(false);

                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_DataWarehouse];
                string smtpServer = emailConfig[ConfigSetttingKey.PBMAPI_DefaultEmailConfig_SMTPServer];

                domainID = getDomainIDFromDomainName(request.DomainName);
                clientHostUrl = request.DomainName;
                var prxUser = await MemberPortalRepository.GetPRXUserDetails(connectionString, domainID, request.UserName).ConfigureAwait(false);
                if (prxUser == null || prxUser.UserID == default)
                {
                    throw new ArgumentException("A Valid User not found for the submitted Domain.");
                }

                string password = generateMemberPortalPassword();

                bool isUpdated = DateWarehouseRepository.UpdatePassword(prxUser.UserID, getMD5Hash(password)).Result;

                sendMemberPortalPasswordResetEmail(smtpServer, prxUser.EmailAddress, prxUser.FNAME, prxUser.LNAME, prxUser.LoginName, password, prxUser.Name, clientHostUrl);
                response.ResponseStatus = new ResponseStatus { Message = "Your password was reset successfully.  An email with the member's temporary password has been sent." };
                return response;

            }).ConfigureAwait(false);
            return response;
        }


        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberPortalPasswordChangeRequest + "|" + ApiRoutes.MemberPortalPasswordChangeRequest)]
        public async Task<MemberPortalChangePasswordResponse> Post(MemberPortalPasswordChangeRequest request)
        {
            MemberPortalChangePasswordResponse response = new MemberPortalChangePasswordResponse();
            response = await processRequest(request, ApiRoutes.MemberPortalLoginRequest, async () =>
            {

                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                int domainID;
                string clientHostUrl;

                Dictionary<string, string> emailConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_DefaultEmailConfig).ConfigureAwait(false);

                Dictionary<string, string> pbmConnectionStrings = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPIConnectionStrings).ConfigureAwait(false);
                string connectionString = pbmConnectionStrings[ConfigSetttingKey.PBMAPI_ConnectionStrings_DataWarehouse];
                string smtpServer = emailConfig[ConfigSetttingKey.PBMAPI_DefaultEmailConfig_SMTPServer];

                domainID = getDomainIDFromDomainName(request.DomainName);
                clientHostUrl = request.DomainName;
                var prxUser = await MemberPortalRepository.GetPRXUserDetailsforUpdatePassword(connectionString, domainID, request.UserName, request.OldPassword).ConfigureAwait(false);
                if (prxUser == null || prxUser.UserID == default)
                {
                    throw new ArgumentException("A Valid User not found for the submitted Domain.");
                }

                bool isUpdated = DateWarehouseRepository.UpdatePassword(prxUser.UserID, request.NewPassword).Result;

                sendMemberPortalPasswordChangeEmail(smtpServer, prxUser.EmailAddress, prxUser.FNAME, prxUser.LNAME, prxUser.LoginName, prxUser.Name, clientHostUrl);
                response.ResponseStatus = new ResponseStatus { Message = "Your password was changed successfully.  An email with the member's updated password has been sent." };
                return response;

            }).ConfigureAwait(false);
            return response;
        }


        //[Authenticate]
        //[RequiresAnyPermission(ApiRoutes.MemberPortalLoginHelpRequest + "|" + ApiRoutes.MemberPortalLoginHelpRequest)]
        //public async Task<MemberPortalLoginHelpResponse> Post(MemberPortalLoginHelpRequest request)
        //{
        //    MemberPortalLoginHelpResponse response = new MemberPortalLoginHelpResponse();
        //    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

        //    response = await processRequest(request, ApiRoutes.MemberPortalLoginHelpRequest, async () =>
        //    {
        //        response = new MemberPortalLoginHelpResponse();
        //        int domainID;
        //        string clientHostUrl;

        //        Dictionary<string, string> emailConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_DefaultEmailConfig).ConfigureAwait(false);
        //        Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MemberPortal_RegistrationConfig).ConfigureAwait(false);

        //        string smtpServer = emailConfig[ConfigSetttingKey.PBMAPI_DefaultEmailConfig_SMTPServer];

        //        //Get Client Info
        //        ClientInfoDTO clientInfo = await DateWarehouseRepository.LookupClientInfo(request.BinNumber).ConfigureAwait(false);

        //        //Get DomainID from DomainName if populated
        //        if (!string.IsNullOrWhiteSpace(request.DomainName))
        //        {
        //            domainID = getDomainIDFromDomainName(request.DomainName);
        //            clientHostUrl = request.DomainName;
        //        }
        //        //If not, we'll be using the defaults for the client
        //        else
        //        {
        //            domainID = clientInfo.DefaultDomainID;
        //            clientHostUrl = clientInfo.DomainName;
        //        }

        //        //Verify BIN
        //        verifyBINAccess(request.BinNumber);

        //        //Lookup Enrollee                
        //        List<MemberPortalEnrolleeDTO> enrolleeList = await DateWarehouseRepository.LookupEnrollee(
        //            clientInfo.ClientID, domainID, request.CardID, request.DateOfBirth,
        //            request.Gender.ToString(), clientInfo.ParentID, clientInfo.OrgID, clientInfo.GroupID, clientInfo.Class).ConfigureAwait(false);

        //        if (enrolleeList.Any())
        //        {
        //            MemberPortalEnrolleeDTO enrollee = enrolleeList.First();

        //            //One matching enrollee found
        //            if (!string.IsNullOrWhiteSpace(enrollee?.EnrolleeID))
        //            {
        //                //Get User Access Info                        
        //                UserInfoDTO userInfo = await DateWarehouseRepository.LookupUserInfo(enrollee.EnrolleeID).ConfigureAwait(false);

        //                if (!string.IsNullOrWhiteSpace(userInfo?.LoginName))
        //                {
        //                    //Change Password & Send Email
        //                    sendLoginHelpInfo(userInfo.UserID, clientInfo.ClientID, enrollee.PlanID, userInfo.EmailAddress, enrollee.FirstName, enrollee.LastName, userInfo.LoginName, clientInfo.Name, clientHostUrl, smtpServer);

        //                    response.IsResetPassword = "true";
        //                    response.ResponseStatus = new ResponseStatus { Message = "Your password was reset successfully.  An email with the member's temporary password has been sent." };
        //                }
        //                else
        //                {
        //                    //No matching user found: ERROR
        //                    throw new ArgumentException("No user account with the supplied values could be found.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //No matching enrollees found: ERROR
        //            throw new ArgumentException("No enrollee with the supplied values could be found.");
        //        }

        //        return response;

        //    }).ConfigureAwait(false);

        //    return response;
        //}

        //[Authenticate]
        //[RequiresAnyPermission(ApiRoutes.MemberPortalChangePasswordRequest + "|" + ApiRoutes.MemberPortalChangePasswordRequest)]
        //public async Task<MemberPortalChangePasswordResponse> Post(MemberPortalChangePasswordRequest request)
        //{
        //    MemberPortalChangePasswordResponse response = new MemberPortalChangePasswordResponse();
        //    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

        //    response = await processRequest(request, ApiRoutes.MemberPortalChangePasswordRequest, async () =>
        //    {
        //        response = new MemberPortalChangePasswordResponse();
        //        string clientHostUrl;

        //        Dictionary<string, string> emailConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_DefaultEmailConfig).ConfigureAwait(false);
        //        Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MemberPortal_RegistrationConfig).ConfigureAwait(false);

        //        string smtpServer = emailConfig[ConfigSetttingKey.PBMAPI_DefaultEmailConfig_SMTPServer];

        //        //Get User Access Info                      
        //        UserInfoDTO userInfo = await DateWarehouseRepository.LookupUserInfo(request.UserID).ConfigureAwait(false);

        //        if (!string.IsNullOrWhiteSpace(userInfo?.LoginName))
        //        {
        //            //Get Client Info
        //            ClientInfoDTO clientInfo = await DateWarehouseRepository.LookupClientInfo(userInfo.BinNumber).ConfigureAwait(false);

        //            //Set DomainName
        //            if (!string.IsNullOrWhiteSpace(request.DomainName))
        //            {
        //                clientHostUrl = request.DomainName;
        //            }
        //            //If not, we'll be using the defaults for the client
        //            else
        //            {
        //                clientHostUrl = clientInfo.DomainName;
        //            }

        //            //Lookup Enrollee                
        //            List<MemberPortalEnrolleeDTO> enrolleeList = await DateWarehouseRepository.LookupEnrollee(userInfo.ENRID).ConfigureAwait(false);

        //            if (enrolleeList.Any())
        //            {
        //                MemberPortalEnrolleeDTO enrollee = enrolleeList.First();

        //                if (!string.IsNullOrWhiteSpace(userInfo?.LoginName))
        //                {
        //                    //Change Password & Send Email
        //                    sendChangePasswordInfo(userInfo.UserID, userInfo.EmailAddress, enrollee.FirstName, enrollee.LastName, userInfo.LoginName, clientInfo.Name, clientHostUrl, smtpServer);

        //                    response.IsChangePassword = "true";
        //                    response.ResponseStatus = new ResponseStatus { Message = "Your password was been changed successfully." };
        //                }
        //                else
        //                {
        //                    //No matching user found: ERROR
        //                    throw new ArgumentException("No user information could be found.");
        //                }

        //            }
        //            else
        //            {
        //                //No matching enrollees found: ERROR
        //                throw new ArgumentException("No enrollee information could be found.");
        //            }
        //        }

        //        return response;

        //    }).ConfigureAwait(false);

        //    return response;
        //}

        //[Authenticate]
        //[RequiresAnyPermission(ApiRoutes.MemberPortalAutoLogonRequest + "|" + ApiRoutes.MemberPortalAutoLogonRequest)]
        //public async Task<MemberPortalAutoLogonResponse> Post(MemberPortalAutoLogonRequest request)
        //{
        //    MemberPortalAutoLogonResponse response = new MemberPortalAutoLogonResponse();

        //    response = await processRequest(request, ApiRoutes.MemberPortalAutoLogonRequest, async () =>
        //    {
        //        response = new MemberPortalAutoLogonResponse();

        //        // 1. Get Values From Token
        //        AutoLogonDTO autoLogonSSO = await MemberPortalRepository.ReadUserSSO(request.Token).ConfigureAwait(false);

        //        if (!string.IsNullOrWhiteSpace(autoLogonSSO?.ENRID))
        //        {
        //            // 2. Validate LastLogin
        //            DateTime tokenTime = autoLogonSSO.LastLogin;  // TODO: if (DateTime.Now > tokenTime.AddMinutes(2)) 

        //            // 3. Set Initial Response Values
        //            response.EnrolleeID = autoLogonSSO.ENRID;
        //            response.ClientID = autoLogonSSO.ClientID;
        //            response.DomainID = autoLogonSSO.DomainID;
        //            response.BIN = autoLogonSSO.BIN;
        //            response.CardID = autoLogonSSO.CardId;
        //            response.CardID2 = autoLogonSSO.CardID2;

        //            // 4. Find User info on PRXUsers Table
        //            UserInfoDTO userInfo = await DateWarehouseRepository.LookupUserInfo(autoLogonSSO.ENRID).ConfigureAwait(false);

        //            if (!string.IsNullOrWhiteSpace(userInfo?.LoginName))
        //            {
        //                response.UserID = userInfo.UserID;
        //                response.LoginName = userInfo.LoginName;
        //                response.EmailAddress = userInfo.EmailAddress;
        //                response.Question = userInfo.Question;
        //                response.Answer = userInfo.Answer;
        //                response.Validated = userInfo.Validated;
        //                response.AllowOthers = userInfo.AllowOthers;
        //                response.LoginCount = userInfo.LoginCount;
        //                response.BIN = userInfo.BinNumber;
        //                response.Password = userInfo.Password;
        //            }

        //            // 5. Lookup Enrollee                
        //            List<MemberPortalEnrolleeDTO> enrolleeList = await DateWarehouseRepository.LookupEnrollee(autoLogonSSO.ENRID).ConfigureAwait(false);

        //            if (enrolleeList.Any())
        //            {
        //                MemberPortalEnrolleeDTO enrollee = enrolleeList.First();
        //                response.FirstName = enrollee.FirstName;
        //                response.LastName = enrollee.LastName;
        //            }

        //        }
        //        else
        //        {
        //            //No matching token: ERROR
        //            throw new ArgumentException("Invalid user session. Please log in again.");
        //        }

        //        return response;

        //    }).ConfigureAwait(false);

        //    return response;
        //}

        #endregion

        #region PBMSchedulerTask

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.PBMSchedulerServiceTask + "|" + ApiRoutes.PBMSchedulerServiceTask)]
        public async Task<PBMSchedulerServiceTaskListResponse> Get(PBMSchedulerServiceTaskRequest request)
        {
            // Log Request
            PBMSchedulerServiceTaskListResponse response = new PBMSchedulerServiceTaskListResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            response = await processRequest(request, ApiRoutes.PBMSchedulerServiceTask, async () =>
            {
                List<PBMSchedulerServiceTaskDTO> pbmSchedulerServiceTaskDTOs = PBMSchedulerTaskRepository.getPBMSchedulerServiceTaskItems();

                response.PBMSchedulerServiceTaskItems = new List<PBMSchedulerServiceTaskItem>();
                response.PBMSchedulerServiceTaskItems.AddRange(pbmSchedulerServiceTaskDTOs.Select(x => x.ConvertTo<PBMSchedulerServiceTaskItem>()));

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        #endregion

        #region VerificationQueue

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.UnverifiedEpisodeList + "|" + ApiRoutes.UnverifiedEpisodeList)]
        public async Task<UnverifiedEpisodeListResponse> Get(UnverifiedEpisodeListRequest request)
        {
            var response = new UnverifiedEpisodeListResponse();
            response = await processRequest(request, ApiRoutes.UnverifiedEpisodeList, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                var connectionSettings = commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_ConnectionInfo).Result;
                var pbmAPIBaseUrl = connectionSettings[ConfigSetttingKey.URLKey];
                var pbmAPIUserName = connectionSettings[ConfigSetttingKey.UsernameKey];
                var pbmAPIPassword = connectionSettings[ConfigSetttingKey.PasswordKey];

                Dictionary<string, string> apiRequest = new Dictionary<string, string>()
                {
                    {"ClientGUID", request.ClientGUID.ToString()},
                    {"ClientID", request.ClientID.ToString()},
                    {"AppUserID", request.AppUserID.ToString()},
                    {"IncludePermissions", "true"}
                };

                bool isSuccesful = tryCallPBMAPIGet<AppUserInfoListResponse>((pbmAPIBaseUrl + ApiRoutes.AppUserInfoList), pbmAPIUserName, pbmAPIPassword, ApiRoutes.AppUserInfoList, apiRequest, out AppUserInfoListResponse appUserInfoListResponse);
                if (!isSuccesful)
                {
                    throw new Exception($"Error while call API: {ApiRoutes.AppUserInfoList}");
                }

                var permissions = appUserInfoListResponse.Permissions;

                permissions = permissions.Where(x => x.CategoryID == (int)PermissionCategory.VQViewEpisodeList).ToList();

                if (permissions == null)
                {
                    response.Episodes = new List<Episode>();
                }
                else
                {
                    // Retrieve client connection string and maximum records settings from configuration
                    var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                    var adsConnectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                    if (!string.IsNullOrEmpty(adsConnectionString))
                    {
                        var results = new List<EpisodeDTO>();
                        VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(adsConnectionString));

                        var t1 = VerificationQueueRepository.GetUnverifiedEpisodeListFromAPSDLY(adsConnectionString);
                        var t2 = VerificationQueueRepository.GetUnverifiedEpisodeListFromClaimHist(adsConnectionString);
                        var t3 = VerificationQueueRepository.GetUnverifiedEpisodeListFromAPSSDB(adsConnectionString);
                        var taskList = new List<Task<List<EpisodeDTO>>>() { t1, t2, t3 };

                        await Task.WhenAll(taskList);
                        foreach (var task in taskList)
                        {
                            if (task.Exception != null)
                            {
                                Exception ex = task.Exception;
                                var exceptionIdentifiers = new Dictionary<string, string> { { "ApiRequestID", Request.Items["ApiMessageID"].ToString() } };
                                await commonApiHelper.LogException(false, Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, exceptionIdentifiers, ApiRoutes.UnverifiedEpisodeList, 100000).ConfigureAwait(false);
                            }
                            else
                            {
                                var data = await task.ConfigureAwait(false);
                                results.AddRange(data);
                            }
                        }
                        taskList.Clear();


                        var Episodes = new List<Episode>();
                        if (results?.Count > 0)
                        {
                            results = results.OrderBy(x => x.CreatedDateTime).ToList();
                            results?.ForEach(x =>
                            {
                                Episode result = x.ConvertTo<Episode>();
                                result.CreatedDateTime = convertToDateTimeString(x.CreatedDateTime);
                                result.NDCProcDateTime = convertToDateTimeString(x.NDCProcDateTime);
                                Episodes.Add(result);
                            });
                        }
                        if (Episodes?.Count > 0)
                        {
                            if (!permissions.Any(x => x.PermissionID == (int)Enums.UserPermission.EpisodeWithStatus_Active))
                            {
                                //remove Active Episodes 
                                Episodes = Episodes.Where(x => x.EpisodeStatusID != (int)EpisodeStatus.Active).ToList();
                            }
                            if (!permissions.Any(x => x.PermissionID == (int)Enums.UserPermission.EpisodeWithStatus_Unverifiable))
                            {
                                //remove Unverifiable Episodes 
                                Episodes = Episodes.Where(x => x.EpisodeStatusID != (int)EpisodeStatus.Unverifiable).ToList();
                            }
                            if (!permissions.Any(x => x.PermissionID == (int)Enums.UserPermission.EpisodeWithStatus_Pending))
                            {
                                //remove Pending  Episodes
                                Episodes = Episodes.Where(x => x.EpisodeStatusID != (int)EpisodeStatus.Pending).ToList();
                            }
                            if (!permissions.Any(x => x.PermissionID == (int)Enums.UserPermission.EpisodeWithStatus_HelpDeskComplete))
                            {
                                //remove Help Desk Complete Episodes 
                                Episodes = Episodes.Where(x => x.EpisodeStatusID != (int)EpisodeStatus.HelpDeskComplete).ToList();
                            }
                            if (!permissions.Any(x => x.PermissionID == (int)Enums.UserPermission.EpisodeWithStatus_NewPlanNeeded))
                            {
                                //remove New Plan Needed Episodes 
                                Episodes = Episodes.Where(x => x.EpisodeStatusID != (int)EpisodeStatus.NewPlanNeeded).ToList();
                            }
                            if (!permissions.Any(x => x.PermissionID == (int)Enums.UserPermission.EpisodeWithStatus_PlanAdded))
                            {
                                //remove Plan Added Episodes 
                                Episodes = Episodes.Where(x => x.EpisodeStatusID != (int)EpisodeStatus.PlanAdded).ToList();
                            }

                            var planids = VerificationQueueRepository.GetEpisodePlanIds(adsConnectionString, request.AppUserID, Episodes.Select(x => x.PlanID).Distinct().ToList().Join(","));
                            Episodes = Episodes.Where(x => planids.Contains(x.PlanID)).ToList();
                        }

                        response.Episodes = Episodes;
                    }
                    else
                    {
                        throw new Exception(Constants.ConnectionInfoNotFound);
                    }
                }
                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.EpisodeDetails + "|" + ApiRoutes.EpisodeDetails)]
        public async Task<EpisodeDetailsResponse> Get(EpisodeDetailsRequest request)
        {
            var response = new EpisodeDetailsResponse();

            response = await processRequest(request, ApiRoutes.EpisodeDetails, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                // Retrieve client connection string and maximum records settings from configuration
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    if (VerificationQueueRepository.EpisodeExists(connectionString, request.EpisodeID))
                    {
                        EpisodeDetailsDTO episodeDetailsDTO = VerificationQueueRepository.GetEpisodeDetailsFromEpisode(request.EpisodeID);

                        if (episodeDetailsDTO != null && episodeDetailsDTO.EpisodeID > 0)
                        {
                            response = episodeDetailsDTO.ConvertTo<EpisodeDetailsResponse>();
                            response.CreatedDateTime = convertToDateTimeString(episodeDetailsDTO.CreatedDateTime);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid episode id - " + request.EpisodeID);
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ReAssignEpisode + "|" + ApiRoutes.ReAssignEpisode)]
        public async Task<ReAssignEpisodeResponse> Post(ReAssignEpisodeRequest request)
        {
            ReAssignEpisodeResponse response = new ReAssignEpisodeResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.ReAssignEpisode, async () =>
            {
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                List<ADSServerDatabaseDTO> adsServerDatabases = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.GlobalPBM_Settings_ADSServerDatabases]);
                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabases.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());
                string adsConnectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);

                if (!adsConnectionString.IsNullOrEmpty())
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(adsConnectionString));
                    UserRepository = new UserRepository(new AdsHelper(adsConnectionString));
                    if (VerificationQueueRepository.EpisodeExists(adsConnectionString, request.EpisodeID))
                    {
                        List<int> userIDs = new List<int>() { request.AppUserID };

                        if (request.AssignedToAppUserID.HasValue)
                        {
                            userIDs.Add(request.AssignedToAppUserID.Value);
                        }

                        List<AppUserInfoDTO> userInfos = UserRepository.GetAppUserInfo(adsConnectionString, userIDs);
                        AppUserInfoDTO appUserInfo = userInfos.FirstOrDefault(x => x.AppUserID == request.AppUserID);
                        AppUserInfoDTO assignedUserInfo = userInfos.FirstOrDefault(x => x.AppUserID == request.AssignedToAppUserID);
                        if (appUserInfo == null)
                        {
                            throw new ArgumentException($"Invalid User ID: {request.AppUserID}");
                        }

                        if (request.AssignedToAppUserID.HasValue && assignedUserInfo == null)
                        {
                            throw new ArgumentException($"Invalid User ID: {request.AssignedToAppUserID}");
                        }

                        List<PermissionRequest> permissions = new List<PermissionRequest>()
                        {
                            new PermissionRequest(){ MinimumGrantLevel= 3, PermissionID = (int)UserPermission.Assign_To_Others},
                            new PermissionRequest(){ MinimumGrantLevel = 3, PermissionID = (int)UserPermission.Eligible_For_Assignment}
                        };

                        Dictionary<string, string> connectionSettings = commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_ConnectionInfo).Result;
                        string pbmAPIBaseUrl = connectionSettings[ConfigSetttingKey.URLKey];
                        string pbmAPIUserName = connectionSettings[ConfigSetttingKey.UsernameKey];
                        string pbmAPIPassword = connectionSettings[ConfigSetttingKey.PasswordKey];

                        UsersWithPermissionsListRequest apiRequest = new UsersWithPermissionsListRequest()
                        {
                            ClientGUID = request.ClientGUID,
                            ClientID = request.ClientID,
                            Permissions = permissions
                        };

                        bool isSuccesful = tryCallPBMAPIPost<UsersWithPermissionsListRequest, UsersWithPermissionsListResponse>((pbmAPIBaseUrl + ApiRoutes.UsersWithPermissionsList), pbmAPIUserName, pbmAPIPassword, ApiRoutes.UsersWithPermissionsList, apiRequest, out UsersWithPermissionsListResponse apiResponse);
                        if (!isSuccesful)
                        {
                            throw new Exception($"{Constants.ApiError}: {ApiRoutes.UsersWithPermissionsList}");
                        }
                        List<PermissionedUser> userPermissions = apiResponse.PermissionedUsers;
                        if (userPermissions?.FirstOrDefault(x => x.Permission.PermissionID == (int)UserPermission.Assign_To_Others)?.Users.FirstOrDefault(x => x.AppUserID == request.AppUserID) == null)
                        {
                            response.ReAssignmentResultCode = (int)ReAssignmentResultCode.Failed_Because_AppUserID;
                            return response;
                        }
                        if (request.AssignedToAppUserID.HasValue && userPermissions?.FirstOrDefault(x => x.Permission.PermissionID == (int)UserPermission.Eligible_For_Assignment)?.Users.FirstOrDefault(x => x.AppUserID == request.AssignedToAppUserID) == null)
                        {
                            response.ReAssignmentResultCode = (int)ReAssignmentResultCode.Failed_Because_AssignedToAppUserID;
                            return response;
                        }

                        VerificationQueueRepository.ReAssignEpisode(adsConnectionString, request.EpisodeID, request.AssignedToAppUserID, request.AppUserID);
                        response.ReAssignmentResultCode = (int)ReAssignmentResultCode.Success;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid Episode ID: {request.EpisodeID}");
                    }
                }
                else
                {
                    throw new ArgumentException(Constants.ConnectionInfoNotFound);
                }
                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.SelfAssignEpisode + "|" + ApiRoutes.SelfAssignEpisode)]
        public async Task<SelfAssignEpisodeResponse> Post(SelfAssignEpisodeRequest request)
        {
            SelfAssignEpisodeResponse response = new SelfAssignEpisodeResponse();
            response = await processRequest(request, ApiRoutes.SelfAssignEpisode, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                List<ADSServerDatabaseDTO> adsServerDatabases = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.GlobalPBM_Settings_ADSServerDatabases]);
                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabases.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());
                string connectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);

                if (!connectionString.IsNullOrEmpty())
                {

                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    if (!VerificationQueueRepository.EpisodeExists(connectionString, request.EpisodeID))
                    {
                        throw new ArgumentException("Invalid episode id - " + request.EpisodeID);
                    }

                    Dictionary<string, string> connectionSettings = commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_ConnectionInfo).Result;
                    string pbmAPIBaseUrl = connectionSettings[ConfigSetttingKey.URLKey];
                    string pbmAPIUserName = connectionSettings[ConfigSetttingKey.UsernameKey];
                    string pbmAPIPassword = connectionSettings[ConfigSetttingKey.PasswordKey];

                    Dictionary<string, string> apiRequest = new Dictionary<string, string>()
                        {
                            {"ClientGUID", request.ClientGUID.ToString()},
                            {"ClientID", request.ClientID.ToString()},
                            {"AppUserID", request.AssignedToAppUserID.ToString()},
                            {"IncludePermissions", "true"}
                        };

                    bool isSuccesful = tryCallPBMAPIGet<AppUserInfoListResponse>((pbmAPIBaseUrl + ApiRoutes.AppUserInfoList), pbmAPIUserName, pbmAPIPassword, ApiRoutes.AppUserInfoList, apiRequest, out AppUserInfoListResponse appUserInfoListResponse);
                    if (!isSuccesful)
                    {
                        throw new Exception($"{Constants.ApiError}: {ApiRoutes.AppUserInfoList}");
                    }

                    if (appUserInfoListResponse.AppUserID != request.AssignedToAppUserID)
                    {
                        throw new ArgumentException("Invalid AssignedToAppuser ID - " + request.AssignedToAppUserID);
                    }
                    else
                    {
                        if (!appUserInfoListResponse.Permissions.Select(x => x.PermissionID).Contains((int)UserPermission.Auto_Self_Assignment))
                        {
                            response.AssignmentResultCode = (int)AssignmentResultCode.Assignment_Failed_because_User_is_not_allowed_for_autoself_assignment;
                            response.EpisodeID = null;
                            response.AssignedToAppUserID = null;
                        }
                        else
                        {
                            SelfAssignEpisodeDTO selfAssignEpisodeDTO = VerificationQueueRepository.SelfAssignEpisode(connectionString, request.EpisodeID, request.AutoAssignmentModeID, Convert.ToInt32(appUserInfoListResponse.AppUserID));
                            if (selfAssignEpisodeDTO.AssignedToAppUserID != appUserInfoListResponse.AppUserID)
                            {
                                response.EpisodeID = null;
                                response.AssignmentResultCode = (int)AssignmentResultCode.Assignment_Failed_because_Episode_assigned_to_another_user;
                                response.AssignedToAppUserID = selfAssignEpisodeDTO.AssignedToAppUserID;
                            }
                            else
                            {
                                response.EpisodeID = request.EpisodeID;
                                response.AssignmentResultCode = (int)AssignmentResultCode.Assignment_Successful;
                                response.AssignedToAppUserID = selfAssignEpisodeDTO.AssignedToAppUserID;
                            }
                        }
                    }

                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }


                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.WorkersCompDetail + "|" + ApiRoutes.WorkersCompDetail)]
        public async Task<WorkersCompDetailVQResponse> Get(WorkersCompDetailVQRequest request)
        {
            var response = new WorkersCompDetailVQResponse();
            response = await processRequest(request, ApiRoutes.WorkersCompDetail, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!connectionString.IsNullOrEmpty())
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    var workersCompDetailDTO = VerificationQueueRepository.GetWorkersCompDetails(connectionString, request.ENRID, request.PlanID);

                    response = workersCompDetailDTO.ConvertTo<WorkersCompDetailVQResponse>();
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }


        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.EpisodeClaimHistory + "|" + ApiRoutes.EpisodeClaimHistory)]
        public async Task<EpisodeClaimHistoryResponse> Get(EpisodeClaimHistoryRequest request)
        {
            var response = new EpisodeClaimHistoryResponse();
            response = await processRequest(request, ApiRoutes.EpisodeClaimHistory, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!connectionString.IsNullOrEmpty())
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new SQLHelper(connectionString));

                    var memberExists = VerificationQueueRepository.APSENRExists(connectionString, request.ENRID, request.PlanID); ;

                    if (memberExists)
                    {
                        var episodeClaimsHistoryDTOs = VerificationQueueRepository.GetEpisodeClaims(connectionString, request.ENRID, request.PlanID);

                        response.EpisodeClaimsHistory = new List<EpisodeClaimsHistory>();

                        episodeClaimsHistoryDTOs.ForEach(x => response.EpisodeClaimsHistory.Add(x.ConvertTo<EpisodeClaimsHistory>()));
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid ENR ID: {request.ENRID} and PlanID: {request.PlanID}");
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                Request.Items[Logging.Text_ReqIdentifier1] = request.ENRID;
                Request.Items[Logging.Text_ReqIdentifier2] = request.PlanID;

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberDetailVQ + "|" + ApiRoutes.MemberDetailVQ)]
        public async Task<MemberDetailVQResponse> Get(MemberDetailVQRequest request)
        {
            var response = new MemberDetailVQResponse();
            response = await processRequest(request, ApiRoutes.MemberDetailVQ, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    var memberDetailVQDTO = VerificationQueueRepository.GetMemberDetailByENRIDAndPlanID(connectionString, request.ENRID, request.PlanID);

                    if (memberDetailVQDTO == null || memberDetailVQDTO.PlanID != request.PlanID)
                    {
                        throw new ArgumentException("Invalid ENRID / PlanID combination " + request.ENRID + " / " + request.PlanID);
                    }

                    response = memberDetailVQDTO.ConvertTo<MemberDetailVQResponse>();
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberPlanIDUpdate + "|" + ApiRoutes.MemberPlanIDUpdate)]
        public async Task<MemberPlanIDUpdateResponse> Post(MemberPlanIDUpdateRequest request)
        {
            var response = new MemberPlanIDUpdateResponse() { Errors = new List<CodedErrorResponse>() };
            response = await processRequest(request, ApiRoutes.MemberPlanIDUpdate, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    var errorMessages = VerificationQueueRepository.ValidateMemberPlanID(request.ENRID, request.PlanID, request.NewPlanID, request.AppUserID);

                    if (errorMessages != null && errorMessages.Count > 0)
                    {
                        foreach (var errorCode in errorMessages)
                        {
                            response.Errors.Add(new CodedErrorResponse { ErrorCode = errorCode.ErrorCode, ErrorMessage = errorCode.ErrorMessage });
                        }
                    }
                    else
                    {
                        VerificationQueueRepository.UpdateMemberPlanID(request.ENRID, request.PlanID, request.NewPlanID, Constants.VerificationQueue, request.AppUserID);
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }


                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.UpdateCardholderID + "|" + ApiRoutes.UpdateCardholderID)]
        public async Task<UpdateCardholderIDResponse> Post(UpdateCardholderIDRequest request)
        {
            var response = new UpdateCardholderIDResponse() { Errors = new List<CodedErrorResponse>() };
            response = await processRequest(request, ApiRoutes.UpdateCardholderID, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    var cardID = getCardID(request.CardHolderID);
                    var cardID2 = getCardID2(request.CardHolderID);

                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));

                    var errors = VerificationQueueRepository.ValidateCardID(connectionString, cardID, cardID2, request.PlanID, request.SubID);

                    if (errors?.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            response.Errors.Add(new CodedErrorResponse { ErrorCode = error.ErrorCode, ErrorMessage = error.ErrorMessage });
                        }
                    }
                    else
                    {
                        VerificationQueueRepository.UpdateCardHolderID(connectionString, cardID, cardID2, request.PlanID, request.SubID, ConfigSetttingKey.Update_CardHolderID_ChangesBY, request.AppUserID);
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberDetailVQUpdate + "|" + ApiRoutes.MemberDetailVQUpdate)]
        public async Task<MemberDetailVQUpdateResponse> Post(MemberDetailVQUpdateRequest request)
        {
            var response = new MemberDetailVQUpdateResponse() { ValidationErrors = new List<BaseErrorResponse>() };
            response = await processRequest(request, ApiRoutes.MemberDetailVQUpdate, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    var userName = VerificationQueueRepository.GetUserNameByAppUserID(request.AppUserID);

                    if (string.IsNullOrEmpty(userName))
                    {
                        response.ValidationErrors.Add(new BaseErrorResponse { ErrorMessage = $"Invalid AppUserId: {request.AppUserID}" });
                    }

                    var apsenrExists = VerificationQueueRepository.APSENRExists(connectionString, request.ENRID, request.PlanID);

                    if (!apsenrExists)
                    {
                        response.ValidationErrors.Add(new BaseErrorResponse { ErrorMessage = $"Invalid ENRID: {request.ENRID} and PlanID: {request.PlanID}" });
                    }

                    if (response?.ValidationErrors?.Count < 1)
                    {
                        var memberDetailVQUpdateDTO = request.ConvertTo<MemberDetailVQUpdateDTO>();

                        VerificationQueueRepository.UpdateMemberDetailVQ(memberDetailVQUpdateDTO, userName);
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.WorkersCompDetailUpdate + "|" + ApiRoutes.WorkersCompDetailUpdate)]
        public async Task<WorkersCompDetailVQUpdateResponse> Post(WorkersCompDetailVQUpdateRequest request)
        {
            var response = new WorkersCompDetailVQUpdateResponse() { UpdateErrors = new List<BaseErrorResponse>() };

            response = await processRequest(request, ApiRoutes.WorkersCompDetailUpdate, async () =>
            {

                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    var userName = VerificationQueueRepository.GetUserNameByAppUserID(request.AppUserID);

                    if (string.IsNullOrEmpty(userName))
                    {
                        response.UpdateErrors.Add(new BaseErrorResponse() { ErrorMessage = $"Invalid AppUserId: {request.AppUserID}" });
                    }

                    bool enrwcExists = VerificationQueueRepository.ENRWCExists(request.ENRID, request.PlanID);

                    if (!enrwcExists)
                    {
                        response.UpdateErrors.Add(new BaseErrorResponse() { ErrorMessage = $"Invalid ENRID: {request.ENRID} and PlanID: {request.PlanID}" });
                    }

                    if (response.UpdateErrors == null || response.UpdateErrors.Count == 0)
                    {
                        WorkersCompDetailVQUpdateDTO workersCompDetailVQUpdateDTO = request.ConvertTo<WorkersCompDetailVQUpdateDTO>();
                        VerificationQueueRepository.UpdateWorkersCompDetailVQ(workersCompDetailVQUpdateDTO, userName);
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.UserPlanListSearch + "|" + ApiRoutes.UserPlanListSearch)]
        public async Task<UserPlanListSearchResponse> Get(UserPlanListSearchRequest request)
        {
            var response = new UserPlanListSearchResponse() { UserPlans = new List<UserPlan>() };

            response = await processRequest(request, ApiRoutes.UserPlanListSearch, async () =>
            {

                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));

                    if (!VerificationQueueRepository.AppUserIDExists(connectionString, request.AppUserID))
                    {
                        throw new ArgumentException($"Invalid AppUserID: {request.AppUserID}");
                    }
                    var calculatePlanAccessTask = VerificationQueueRepository.CalculatePlanAccess(request.AppUserID, false);
                    var calculateGroupAccessTask = VerificationQueueRepository.CalculateGroupAccess(request.AppUserID, false);
                    var calculateOrgAccessTask = VerificationQueueRepository.CalculateOrgAccess(request.AppUserID, false);
                    var calculateParentAccessTask = VerificationQueueRepository.CalculateParentAccess(request.AppUserID, false);

                    var tasks = new List<Task> { calculatePlanAccessTask, calculateGroupAccessTask, calculateOrgAccessTask, calculateParentAccessTask };

                    await Task.WhenAll(tasks);
                    var isExceptionOccured = false;
                    foreach (Task task in tasks)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            var exceptionIdentifiers = new Dictionary<string, string> { { "ApiRequestID", Request.Items["ApiMessageID"].ToString() } };
                            await commonApiHelper.LogException(false, Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, exceptionIdentifiers, ApiRoutes.UserPlanListSearch, 100000).ConfigureAwait(false);
                            isExceptionOccured = true;
                        }
                    }

                    if (!isExceptionOccured)
                    {
                        var UserPlanDtos = new List<UserPlanDTO>();
                        SearchType searchType = (SearchType)(Convert.ToInt32(request.SearchTypeID));
                        switch (searchType)
                        {
                            case SearchType.PlanName:
                                UserPlanDtos = VerificationQueueRepository.GetUserPlanWithPlanName(request.AppUserID, request.SearchWith.ToUpper());
                                break;
                            case SearchType.GroupName:
                                UserPlanDtos = VerificationQueueRepository.GetUserPlanWithGroupName(request.AppUserID, request.SearchWith.ToUpper());
                                break;
                            case SearchType.PlanAddress:
                                UserPlanDtos = VerificationQueueRepository.GetUserPlanWithPlanAddress(request.AppUserID, request.SearchWith.ToUpper());
                                break;
                            case SearchType.PlanState:
                                UserPlanDtos = VerificationQueueRepository.GetUserPlanWithPlanState(request.AppUserID, request.SearchWith.ToUpper());
                                break;
                        }

                        UserPlanDtos?.ForEach(userPlan =>
                        {
                            response.UserPlans.Add(userPlan.ConvertTo<UserPlan>());
                        });
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ChangeLogList + "|" + ApiRoutes.ChangeLogList)]
        public async Task<ChangeLogListResponse> Get(ChangeLogListRequest request)
        {
            var response = new ChangeLogListResponse() { ChangeLogDetails = new List<ChangeLogDetail>() };

            response = await processRequest(request, ApiRoutes.ChangeLogList, async () =>
            {

                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));

                    if (!VerificationQueueRepository.APSENRExists(connectionString, request.ENRID, null))
                    {
                        throw new ArgumentException($"Invalid ENR ID: {request.ENRID}");
                    }
                    var changeLogDetailDtos = new List<ChangeLogDetailDTO>();

                    changeLogDetailDtos = VerificationQueueRepository.GetChangeLogDetail(request.ENRID);
                    changeLogDetailDtos?.ForEach(changeLogDetail =>
                    {
                        switch (changeLogDetail.LogFile.ToUpper())
                        {
                            case "APSLOG":
                                changeLogDetail.LogFile = LogFile.Member.ToString();
                                changeLogDetail.LogFileID = (int)LogFile.Member;
                                break;
                            case "ENRWCL":
                                changeLogDetail.LogFile = LogFile.MemberWorkersComp.ToString();
                                changeLogDetail.LogFileID = (int)LogFile.MemberWorkersComp;
                                break;
                            case "EPISODEL":
                                changeLogDetail.LogFile = LogFile.Episode.ToString();
                                changeLogDetail.LogFileID = (int)LogFile.Episode;
                                break;
                        }

                        changeLogDetail.ChangedDateTime = convertToDateTimeString(Convert.ToDateTime(changeLogDetail.ChangedDateTime));

                        if (changeLogDetail.ChangedField == ConfigSetttingKey.CarrierNT)
                        {
                            changeLogDetail.LogFile = LogFile.MemberWorkersComp.ToString();
                        }

                        response.ChangeLogDetails.Add(changeLogDetail.ConvertTo<ChangeLogDetail>());
                    });

                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.EpisodeStatusUpdate + "|" + ApiRoutes.EpisodeStatusUpdate)]
        public async Task<EpisodeStatusUpdateResponse> Post(EpisodeStatusUpdateRequest request)
        {
            var response = new EpisodeStatusUpdateResponse();

            response = await processRequest(request, ApiRoutes.EpisodeStatusUpdate, async () =>
            {

                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    var userName = VerificationQueueRepository.GetUserNameByAppUserID(request.AppUserID);

                    if (string.IsNullOrEmpty(userName))
                    {
                        throw new ArgumentException($"Invalid AppUser Id : {request.AppUserID}");
                    }
                    if (!VerificationQueueRepository.EpisodeExists(connectionString, request.EpisodeID))
                    {
                        throw new ArgumentException($"Invalid Episode Id : {request.EpisodeID}");
                    }

                    VerificationQueueRepository.UpdateEpisodeStatus(request.EpisodeID, request.EpisodeStatusID, Constants.VerificationQueue, userName.Length > 15 ? userName.Substring(0, 15) : userName);

                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.EpisodeGetNextForUser + "|" + ApiRoutes.EpisodeGetNextForUser)]
        public async Task<EpisodeGetNextForUserResponse> Get(EpisodeGetNextForUserRequest request)
        {
            var response = new EpisodeGetNextForUserResponse();

            response = await processRequest(request, ApiRoutes.EpisodeGetNextForUser, async () =>
            {

                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    var userName = VerificationQueueRepository.GetUserNameByAppUserID(request.AppUserID);

                    if (string.IsNullOrEmpty(userName))
                    {
                        throw new ArgumentException($"Invalid AppUserId: {request.AppUserID}");
                    }

                    bool episodeExists = VerificationQueueRepository.EpisodeExists(connectionString, request.CurrentEpisodeID);

                    if (!episodeExists)
                    {
                        throw new ArgumentException($"Episode Id - {request.CurrentEpisodeID} Details not found");
                    }

                    response.EpisodeID = VerificationQueueRepository.GetNextEpisodeForUser(request.AppUserID, request.CurrentEpisodeID);

                    response.ResultCode = response.EpisodeID == null ? (int)NextEpisodeResultCode.NoNextEpisodeIDAvailable : (int)NextEpisodeResultCode.NextEpisodeIDAvailable;
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.EpisodeListCarrierNoteUpdate + "|" + ApiRoutes.EpisodeListCarrierNoteUpdate)]
        public async Task<EpisodeListCarrierNoteUpdateResponse> Post(EpisodeListCarrierNoteUpdateRequest request)
        {
            var response = new EpisodeListCarrierNoteUpdateResponse() { };

            response = await processRequest(request, ApiRoutes.EpisodeListCarrierNoteUpdate, async () =>
            {

                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                var connectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    var userName = VerificationQueueRepository.GetUserNameByAppUserID(request.AppUserID);

                    if (string.IsNullOrEmpty(userName))
                    {
                        throw new ArgumentException($"Invalid AppUser ID {request.AppUserID}");
                    }

                    List<long> epsIDs = VerificationQueueRepository.ValidateEpisodeIDs(string.Join(",", request.EpisodeIDs));

                    if (epsIDs?.Count > 0)
                    {
                        throw new ArgumentException($"Invalid Episodes {string.Join(",", epsIDs)}");
                    }

                    VerificationQueueRepository.UpdateCarrierNote(string.Join(",", request.EpisodeIDs), request.CarrierNote, userName);
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);
            return response;
        }

        #endregion

        #region User

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.AppUserInfoList + "|" + ApiRoutes.AppUserInfoList)]
        public async Task<AppUserInfoListResponse> Get(AppUserInfoListRequest request)
        {
            AppUserInfoListResponse response = new AppUserInfoListResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.AppUserInfoList, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                List<ADSServerDatabaseDTO> adsServerDatabases = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.GlobalPBM_Settings_ADSServerDatabases]);
                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabases.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());
                string connectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);

                if (!string.IsNullOrEmpty(connectionString))
                {
                    UserRepository = new UserRepository(new AdsHelper(connectionString));
                    List<AppUserInfoDTO> userInfos = UserRepository.GetAppUserInfo(connectionString, new List<int>() { request.AppUserID });

                    if (userInfos?.Count > 0)
                    {
                        AppUserInfoDTO userInfo = userInfos.FirstOrDefault();
                        response.AppUserID = userInfo.AppUserID;
                        response.LogonID = userInfo.LogonID;
                        response.FirstName = userInfo.FirstName;
                        response.LastName = userInfo.LastName;

                        List<UserLogoBinaryDTO> userLogoBinaryDTOs = UserRepository.GetUserLogoBinaryInfo(connectionString, request.AppUserID);
                        List<UserLogoBinaryDTO> validUserLogoBinaryDTOs = userLogoBinaryDTOs.GroupBy(c => c.LogoID).Select(grp => grp.FirstOrDefault()).ToList();

                        response.UserLogoBinary = validUserLogoBinaryDTOs?.Count == 1 ? Convert.ToBase64String(validUserLogoBinaryDTOs?.FirstOrDefault()?.Image) : null;

                        if (request.IncludePermissions)
                        {
                            response.Permissions = new List<Permission>();
                            List<PermissionDTO> userAllPermissions = UserRepository.GetAppUserPermissions(connectionString, new List<int>() { request.AppUserID });
                            if (userAllPermissions?.Count > 0)
                            {
                                List<PermissionDTO> resultPermissions = filterUserPermissions(userAllPermissions);
                                resultPermissions.ForEach(x => response.Permissions.Add(x.ConvertTo<Permission>()));
                            }
                        }
                        else
                        {
                            response.Permissions = new List<Permission>();
                        }
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.UsersWithPermissionsList + "|" + ApiRoutes.UsersWithPermissionsList)]
        public async Task<UsersWithPermissionsListResponse> Post(UsersWithPermissionsListRequest request)
        {
            UsersWithPermissionsListResponse response = new UsersWithPermissionsListResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.UsersWithPermissionsList, async () =>
            {
                // Retrieve client connection string and maximum records settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                List<ADSServerDatabaseDTO> adsServerDatabases = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.GlobalPBM_Settings_ADSServerDatabases]);
                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabases.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());
                string connectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);

                List<UserPermissionDTO> results = new List<UserPermissionDTO>();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    string permissionIdList = string.Join(',', request.Permissions.Select(x => x.PermissionID));
                    UserRepository = new UserRepository(new SQLHelper(connectionString));
                    results = UserRepository.GetUsersWithPermissionsList(connectionString, permissionIdList);
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                List<UserPermissionDTO> resultPermissions = filterUserPermissions(results);

                var finalResult = new List<UserPermissionDTO>();
                request.Permissions.ForEach(x =>
                {
                    finalResult.AddRange(resultPermissions.Where(y => y.GrantLevel >= x.MinimumGrantLevel && y.PermissionID == x.PermissionID).ToList());
                });

                response.PermissionedUsers = (from f in finalResult
                                              group f by new { f.PermissionID, f.PermissionName, f.CategoryID, f.CategoryName } into fg
                                              select new PermissionedUser
                                              {
                                                  Permission = new Permission
                                                  {
                                                      PermissionID = fg.Key.PermissionID,
                                                      PermissionName = fg.Key.PermissionName,
                                                      CategoryID = fg.Key.CategoryID,
                                                      CategoryName = fg.Key.CategoryName
                                                  },
                                                  Users = fg.ToList().Select(_ => new AppUser { AppUserID = _.AppUserID, GrantLevel = _.GrantLevel, FirstName = _.FirstName, LastName = _.LastName }).ToList()
                                              }).ToList();


                var availablePermissions = response.PermissionedUsers.Select(x => x.Permission.PermissionID).ToList();
                var unAvailablePermissions = request.Permissions.Select(x => x.PermissionID).Except(availablePermissions).ToList();


                if (unAvailablePermissions.Count > 0)
                {
                    List<PermissionDTO> unavailablePermissionDetails = UserRepository.GetPermissionsByIds(connectionString, string.Join(',', unAvailablePermissions));

                    List<int> totalPermissions = availablePermissions;
                    totalPermissions.AddRange(unavailablePermissionDetails.Select(x => x.PermissionID));

                    List<int> requestedPermissions = request.Permissions.Select(X => X.PermissionID).ToList();
                    List<int> invalidPermissionIds = requestedPermissions.Except(totalPermissions).ToList();

                    if (invalidPermissionIds.Count > 0)
                    {
                        throw new ArgumentException("Invalid Permission ID's - " + invalidPermissionIds.Join(","));
                    }

                    unavailablePermissionDetails?.ForEach(permission =>
                    {
                        response.PermissionedUsers.Add(new PermissionedUser
                        {
                            Permission =
                            {
                                PermissionID = permission.PermissionID,
                                PermissionName = permission.PermissionName,
                                CategoryID = permission.CategoryID,
                                CategoryName = permission.CategoryName
                            },
                            Users = new List<AppUser>()
                        });
                    });
                }
                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.UsersWithPlansList + "|" + ApiRoutes.UsersWithPlansList)]
        public async Task<UsersWithPlansListResponse> Post(UsersWithPlansListRequest request)
        {
            UsersWithPlansListResponse response = new UsersWithPlansListResponse() { PlanUsers = new List<PlanUser>() };
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            response = await processRequest(request, ApiRoutes.UsersWithPlansList, async () =>
            {
                // Retrieve client connection string settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                List<ADSServerDatabaseDTO> adsServerDatabases = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.GlobalPBM_Settings_ADSServerDatabases]);
                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabases.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());
                string connectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);

                List<PlanUserDTO> results = new List<PlanUserDTO>();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    string planIDList = string.Join(',', request.PlanIDs);
                    UserRepository = new UserRepository(new AdsHelper(connectionString));
                    results = UserRepository.GetUsersWithPlansList(connectionString, planIDList);

                    var planUsers = from p in results
                                    group p by new { p.PlanID } into pg
                                    select new PlanUser
                                    {
                                        PlanID = pg.Key.PlanID,
                                        AppUsers = pg.ToList().Select(_ => new PlanUserInfo { AppUserID = _.AppUserID, FirstName = _.FirstName, LastName = _.LastName }).ToList()
                                    };

                    response.PlanUsers.AddRange(planUsers);
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        #endregion

        #region Note

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ReadNotes + "|" + ApiRoutes.ReadNotes)]
        public async Task<NoteListResponse> Get(NoteListRequest request)
        {
            NoteListResponse response = new NoteListResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.ReadNotes, async () =>
            {
                // Retrieve client connection string settings from configuration
                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                List<ADSServerDatabaseDTO> adsServerDatabases = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.GlobalPBM_Settings_ADSServerDatabases]);
                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabases.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());
                string connectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);
                List<NoteDTO> results = new List<NoteDTO>();
                if (!connectionString.IsNullOrEmpty())
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    Enum.TryParse(request.FileTypeID.ToString(), true, out NoteFileTypeID noteFileTypeID);
                    results = VerificationQueueRepository.GetNoteList(connectionString,
                                                                        noteFileTypeID.ToString(), request.Key.ToUpper(),
                                                                        string.IsNullOrEmpty(request.NoteType) ? null : request.NoteType.ToUpper());
                    response.Notes = new List<Note>();
                    if (!results.IsEmpty())
                    {
                        results = (from note in results
                                   group note by new { note.ID, note.Description, note.Type, note.CreateBy, note.CreateDateTime } into ng
                                   select new NoteDTO
                                   {
                                       ID = ng.Key.ID,
                                       Description = ng.Key.Description,
                                       Type = ng.Key.Type,
                                       CreateBy = ng.Key.CreateBy,
                                       CreateDateTime = ng.Key.CreateDateTime,
                                       NoteText = string.Join("", ng.ToList().Select(x => x.NoteText))
                                   }).ToList();



                        results.ForEach(x => response.Notes.Add(x.ConvertTo<Note>()));
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }


        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.InsertNote + "|" + ApiRoutes.InsertNote)]
        public async Task<InsertNoteResponse> Post(InsertNoteRequest request)
        {
            InsertNoteResponse response = new InsertNoteResponse();
            response = await processRequest(request, ApiRoutes.InsertNote, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);

                List<ADSServerDatabaseDTO> adsServerDatabases = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.GlobalPBM_Settings_ADSServerDatabases]);
                ADSServerDatabaseDTO adsServerDatabaseDTO = adsServerDatabases.FirstOrDefault(x => x.Name == request.ClientID.ToUpper());
                string connectionString = MultipleConnectionsHelper.GetADSConnectionString(adsServerDatabaseDTO);

                if (!connectionString.IsNullOrEmpty())
                {
                    VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(connectionString));
                    if (VerificationQueueRepository.AppUserExists(connectionString, request.CreatedBy.ToUpper()))
                    {
                        Enum.TryParse(request.FileTypeID.ToString(), true, out NoteFileTypeID noteFileTypeID);
                        string note = GetSubstringNote(request.Note);

                        VerificationQueueRepository.InsertNote(connectionString,
                                                               noteFileTypeID.ToString(),
                                                               request.Key.ToUpper(),
                                                               string.IsNullOrEmpty(request.Type) ? null : request.Type.ToUpper(),
                                                               string.IsNullOrEmpty(request.Description) ? null : request.Description.ToUpper(),
                                                               request.CreatedBy.Length >= 15 ? request.CreatedBy.Substring(0, 15).ToUpper() : request.CreatedBy.ToUpper(),
                                                               note);
                    }
                    else
                    {
                        throw new ArgumentException($"User { request.CreatedBy } not found.");
                    }
                }
                else
                {
                    throw new Exception(Constants.ConnectionInfoNotFound);
                }

                Request.Items[Logging.Text_ReqIdentifier1] = request.FileTypeID;
                Request.Items[Logging.Text_ReqIdentifier2] = request.Key;

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        #endregion

        #endregion


        #region Private Methods

        private async Task<T2> processRequest<T1, T2>(T1 request, string apiRoute, Func<Task<T2>> actionToCallIfValid = null)
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

        private bool tryCallPBMAPIGet<T>(string endPoint, string apiUserName, string apiPassword, string methodSource, Dictionary<string, string> request, out T response)
        {
            bool isSuccessful = true;
            response = default(T);
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            try
            {
                APIResponse<T> apiResponse = ApiHelper.APIBasicAuthGet<T>(endPoint, request, apiUserName, apiPassword).Result;

                if (apiResponse.StatusCode != HttpStatusCode.OK)
                {
                    isSuccessful = false;
                    commonApiHelper.LogException(true, Enums.ApplicationSource.PBMAPI, $"API Error Status for route: {endPoint}", JsonConvert.SerializeObject(apiResponse), methodSource: methodSource).ConfigureAwait(false);
                }
                response = apiResponse.Response;
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                commonApiHelper.LogException(true, Enums.ApplicationSource.PBMAPI, $"API Exception for route: {endPoint} Message: {ex.Message}",
                                                               ex.StackTrace, methodSource: methodSource).ConfigureAwait(false);
            }

            return isSuccessful;
        }

        private bool tryCallPBMAPIPost<T1, T2>(string endPoint, string apiUserName, string apiPassword, string methodSource, T1 request, out T2 response)
        {
            bool isSuccessful = true;
            response = default(T2);
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            try
            {
                APIResponse<T2> apiResponse = ApiHelper.ApiBasicAuthPost<T1, T2>(endPoint, request, apiUserName, apiPassword).Result;

                if (apiResponse.StatusCode != HttpStatusCode.OK)
                {
                    isSuccessful = false;
                    commonApiHelper.LogException(true, ApplicationSource.PBMAPI, $"API Error Status for EndPoint: {endPoint}", JsonConvert.SerializeObject(apiResponse), methodSource: methodSource).ConfigureAwait(false);
                }
                response = apiResponse.Response;
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                commonApiHelper.LogException(true, ApplicationSource.PBMAPI, $"API Exception for route: {endPoint} Message: {ex.Message}",
                                                               ex.StackTrace, methodSource: methodSource).ConfigureAwait(false);
            }

            return isSuccessful;
        }


        #region Rule

        private string getDatasetConnectionString(DatasetDTO dataset)
        {
            //Error if no dataset found
            if (string.IsNullOrWhiteSpace(dataset?.Path))
            {
                throw new ArgumentException("The Client is invalid or this account does not have access to the requested Client.");
            }

            return MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path);
        }

        private async Task<RuleTemplateDTO> getRuleTemplate(string clientConnectionString, string datasetName, DynamicPARequest request)
        {

            RuleTemplateDTO ruleTemplate = await Repository
                                                 .GetDynamicPARuleTemplate(clientConnectionString, datasetName, request.DynamicPACode2, request.ProductID)
                                                 .ConfigureAwait(false);

            //Error if a rule template was not found
            if (string.IsNullOrWhiteSpace(ruleTemplate.EPA_ID))
            {
                throw new ArgumentException($"Rule template not found for {request.Client} submitted Dynamic PA Code 2 ({request.DynamicPACode2}).");
            }

            if (string.IsNullOrWhiteSpace(ruleTemplate.CODES))
            {
                throw new ArgumentException($"NDC {request.ProductID} not found.");
            }

            return ruleTemplate;
        }

        private void validatePharmacy(string clientConnectionString, string requestID, string templateID)
        {
            string pharmacyID = !string.IsNullOrWhiteSpace(requestID) ? requestID : templateID;

            if (!string.IsNullOrWhiteSpace(pharmacyID))
            {
                bool pharmacyValid = isValidPharmacy(clientConnectionString, pharmacyID, out string errorMessage);

                if (!pharmacyValid)
                {
                    throw new ArgumentException(errorMessage);
                }
            }
        }

        private async Task<List<string>> getEnrolleeId(string clientConnectionString, MemberIDType memberIdType, DynamicPARequest request)
        {
            List<string> enrolleeIds = new List<string>();

            //Get member EnrolleeID if MemberEnrolleeID not provided
            if (string.IsNullOrWhiteSpace(request.MemberEnrolleeID))
            {
                enrolleeIds = await getEnrolleeId(clientConnectionString, memberIdType, request.PlanID, request.MemberID)
                                             .ConfigureAwait(false);
            }
            else
            {
                enrolleeIds.Add(request.MemberEnrolleeID);
            }

            if (!enrolleeIds.Any())
            {
                throw new ArgumentException("Invalid member.  Active member could not be found.");
            }

            return enrolleeIds;
        }

        private async Task<string> getEnrolleeId(string clientConnectionString, MemberIDType memberIdType, string planId, string memberId, string person)
        {
            List<string> enrolleeIds = await getEnrolleeId(clientConnectionString, memberIdType, planId, memberId).ConfigureAwait(false);
            string enrolleeId = enrolleeIds.First();

            return await Repository.GetMemberEnrolleeIdWithPerson(clientConnectionString, planId, enrolleeId, person).ConfigureAwait(false);
        }

        private async Task<List<DatasetEnrolleeIDDTO>> getDatasetEnrolleeIds(List<DatasetDTO> datasets, string planId, string memberId, string person)
        {
            Task<List<DatasetEnrolleeIDDTO>> t = Task.Run(() =>
            {
                List<DatasetEnrolleeIDDTO> dbResults = new List<DatasetEnrolleeIDDTO>();

                datasets.ForEach(dataset =>
                {
                    try
                    {
                        string enrolleeId = getEnrolleeId(getDatasetConnectionString(dataset), dataset.MemberIdType, planId, memberId, person).Result;

                        if (!string.IsNullOrWhiteSpace(enrolleeId))
                        {
                            dbResults.Add(new DatasetEnrolleeIDDTO(dataset, enrolleeId));
                        }
                    }
                    catch (Exception) { }
                });

                return dbResults;
            }
            );

            List<DatasetEnrolleeIDDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        private async Task<List<string>> getEnrolleeId(string clientConnectionString, MemberIDType memberIdType, string planId, string memberId)
        {
            List<string> enrolleeIds = await Repository.GetMemberEnrolleeId(clientConnectionString, memberIdType, planId, memberId)
                                             .ConfigureAwait(false);

            if (!enrolleeIds.Any())
            {
                throw new ArgumentException("Invalid member.  Active member could not be found.");
            }

            return enrolleeIds;
        }

        //private async Task<bool> isValidPhysician(string clientConnectionString, MemberPhysicianLockUpdateRequest request, string userId)
        //{
        //    List<ExistenceStatus> validStatusList = new List<ExistenceStatus> { ExistenceStatus.Exists, ExistenceStatus.ExistsExactMatch };
        //    ExistenceStatus physicianExists = await PrescriberRepository.ValidatePhysician(clientConnectionString, request.PhysicianNPI, request.PhysicianDEA,
        //                                                                    request.EffectiveDate, request.TerminationDate).ConfigureAwait(false);

        //    if (physicianExists == ExistenceStatus.InvalidExistenceState)
        //    {
        //        throw new ArgumentException($"Invalid physician: Conflict when using NPI {request.PhysicianNPI} with DEA {request.PhysicianDEA}");
        //    }
        //    else if (physicianExists == ExistenceStatus.DoesNotExist)
        //    {
        //        throw new ArgumentException($"Invalid physician: A physician using NPI {request.PhysicianNPI} or DEA {request.PhysicianDEA} could not be found; contact account manager to add to Physician File");
        //        //PrescriberRepository.AddPhysician(clientConnectionString, request.PhysicianNPI, request.PhysicianDEA,
        //        //                        request.PhysicianFirstName, request.PhysicianLastName,
        //        //                        request.EffectiveDate, request.TerminationDate, userId);
        //    }
        //    //else if (physicianExists == ExistenceStatus.ExistsNeedsUpdate)
        //    //{
        //    //    PrescriberRepository.UpdatePhysician(clientConnectionString, request.PhysicianNPI,
        //    //                            request.PhysicianFirstName, request.PhysicianLastName,
        //    //                            request.EffectiveDate, request.TerminationDate, userId);
        //    //}

        //    return validStatusList.Contains(physicianExists);
        //}

        //private async Task<ExistenceStatus> addUpdatePhysicianIfNotCurrent(string clientConnectionString, MemberPhysicianLockUpdateRequest request, string userId)
        //{
        //    ExistenceStatus physicianExists = await PrescriberRepository.ValidatePhysician(clientConnectionString, request.PhysicianNPI, request.PhysicianDEA,
        //                                                                    request.PhysicianFirstName, request.PhysicianLastName,
        //                                                                    request.EffectiveDate, request.TerminationDate).ConfigureAwait(false);

        //    if(physicianExists == ExistenceStatus.InvalidExistenceState)
        //    {
        //        throw new ArgumentException($"Invalid physician: conflict when using NPI {request.PhysicianNPI} with DEA {request.PhysicianFirstName}");
        //    }
        //    else if (physicianExists == ExistenceStatus.DoesNotExist)
        //    {
        //        PrescriberRepository.AddPhysician(clientConnectionString, request.PhysicianNPI, request.PhysicianDEA,
        //                                request.PhysicianFirstName, request.PhysicianLastName,
        //                                request.EffectiveDate, request.TerminationDate, userId);
        //    }
        //    else if(physicianExists == ExistenceStatus.ExistsNeedsUpdate)
        //    {
        //        PrescriberRepository.UpdatePhysician(clientConnectionString, request.PhysicianNPI,
        //                                request.PhysicianFirstName, request.PhysicianLastName,
        //                                request.EffectiveDate, request.TerminationDate, userId);
        //    }

        //    return physicianExists;
        //}

        private string getProductIDQualifier(string codeType)
        {
            Enum.TryParse(codeType, out ProductType parsedCodeType);

            if (parsedCodeType == ProductType.Unknown)
            {
                parsedCodeType = ProductType.Other;
            }

            return ((int)parsedCodeType).ToString().PadLeft(2, '0');
        }

        private string getProductIDQualifierDescription(string qualifier)
        {
            ProductType parsedCodeType = (ProductType)int.Parse(qualifier);

            if (parsedCodeType == ProductType.Unknown)
            {
                parsedCodeType = ProductType.Other;
            }

            return parsedCodeType.ToString();
        }

        private bool memberHasRule(string adsConnectionString, string planId, string enrolleeId, string vendType, string codeType, string codes, bool ignorePlan, out string retailSysid, out string mailSysid)
        {
            retailSysid = "";
            mailSysid = "";
            bool exists = false;

            if (ignorePlan)
            {
                retailSysid = Repository.RuleExists(adsConnectionString, null, enrolleeId, null, codeType, codes).Result;
                mailSysid = retailSysid;
            }
            else
            {
                switch (vendType)
                {
                    case nameof(VendorType.RETL):
                        retailSysid = Repository.RuleExists(adsConnectionString, planId, enrolleeId, nameof(VendorType.RETL), codeType, codes).Result;
                        break;
                    case nameof(VendorType.MAIL):
                        mailSysid = Repository.RuleExists(adsConnectionString, planId, enrolleeId, nameof(VendorType.MAIL), codeType, codes).Result;
                        break;
                    case nameof(VendorType.BOTH):
                        mailSysid = Repository.RuleExists(adsConnectionString, planId, enrolleeId, nameof(VendorType.MAIL), codeType, codes).Result;
                        retailSysid = Repository.RuleExists(adsConnectionString, planId, enrolleeId, nameof(VendorType.RETL), codeType, codes).Result;
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(retailSysid) || !string.IsNullOrEmpty(mailSysid))
            {
                exists = true;
            }

            return exists;
        }

        private async Task<string> saveMemberRule(string clientConnectionString, string clientName, string sysid, string enrolleeId, DynamicPARequest request, RuleTemplateDTO ruleTemplate, string vendType, string userName, string changedBy, bool ignorePlan)
        {
            string newSysid = string.Empty;
            int? daysSupplyMaximum = parseNullableInt(request.DaysSupplyMaximum);
            int? patientAgeMaximum = parseNullableInt(request.PatientAgeMaximum);
            int? periodQuantityDays = parseNullableInt(request.PeriodQuantityDays);
            int? numberOfFills = parseNullableInt(request.NumberOfFills);
            double? periodQuantityMaximum = parseNullableDouble(request.PeriodQuantityMaximum);
            double? amountDueMaximum = parseNullableDouble(request.AmountDueMaximum);

            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(request.RequestFromDate))
            {
                fromDate = DateTime.ParseExact(request.RequestFromDate, "yyyyMMdd", null);
            }

            if (!string.IsNullOrWhiteSpace(request.RequestToDate))
            {
                toDate = DateTime.ParseExact(request.RequestToDate, "yyyyMMdd", null);
            }
            string includeExcludeValue = "";

            if (!string.IsNullOrWhiteSpace(request.PharmacyIncludeExclude))
            {
                switch (request.PharmacyIncludeExclude.ToUpper())
                {
                    case "I":
                        includeExcludeValue = "C";
                        break;
                    case "E":
                        includeExcludeValue = "E";
                        break;
                }
            }

            DateTime? effdt = fromDate ?? ruleTemplate.EFFDT;
            DateTime? trmdt = toDate ?? ruleTemplate.TRMDT;
            int? roundedAmountDueMaximum = null, magelo, fagelo;
            string sex, magemeth, fagemeth;

            if (amountDueMaximum.HasValue)
            {
                roundedAmountDueMaximum = (int)Math.Ceiling(amountDueMaximum.Value);
            }

            if (patientAgeMaximum.HasValue)
            {
                sex = "B";
                magemeth = "<";
                fagemeth = "<";
                magelo = patientAgeMaximum;
                fagelo = patientAgeMaximum;
            }
            else
            {
                sex = ruleTemplate.SEX;
                magemeth = ruleTemplate.MAGEMETH;
                fagemeth = ruleTemplate.FAGEMETH;
                magelo = ruleTemplate.MAGELO;
                fagelo = ruleTemplate.FAGELO;
            }

            string dsgid = ruleTemplate.DSGID;
            if (daysSupplyMaximum.HasValue && daysSupplyMaximum.Value > 0)
            {
                string lookupDSGID = await Repository.LookupDSGID(clientConnectionString, daysSupplyMaximum.Value).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(lookupDSGID))
                {
                    dsgid = lookupDSGID;
                }
                else
                {
                    throw new ArgumentException("The submitted DaysSupplyMaximum value was not found.");
                }
            }

            int? maxrefills = ruleTemplate.MAXREFILLS ?? 99;
            int? maxrefmnt = numberOfFills.HasValue ? numberOfFills - 1 : ruleTemplate.MAXREFMNT;
            string pharmacyId = string.IsNullOrWhiteSpace(request.PharmacyID) ? ruleTemplate.PPNID : request.PharmacyID;
            string includeExclude = string.IsNullOrWhiteSpace(includeExcludeValue) ? ruleTemplate.PPNREQRUL : includeExcludeValue;
            int? hidollar = roundedAmountDueMaximum ?? ruleTemplate.HIDOLLAR;
            double? qtyperdys = periodQuantityMaximum ?? ruleTemplate.QTYPERDYS;
            int? qtydylmt = periodQuantityDays ?? ruleTemplate.QTYDYLMT;

            //Story 20337 - Always write blank CopayGCI/MultisourceCode
            //List<string> validMultisourceCodes = new List<string> { "M", "O", "N", "Y" };
            //string validatedMultisourceCode = validMultisourceCodes.Contains(request.MultisourceCode.ToUpper()) ? request.MultisourceCode : "";
            string multisourceCode = ""; //string.IsNullOrWhiteSpace(validatedMultisourceCode) ? ruleTemplate.COPAYGCI : validatedMultisourceCode;

            string trmDtPaidMsgText = trmdt != null ? $" thru {trmdt.Value:MM/dd/yyyy}" : "";
            string paidMsg = $"Approved{trmDtPaidMsgText}, {request.VendorPANumber}";

            if (string.IsNullOrWhiteSpace(sysid))
            {
                newSysid = await addMemberRule_DynamicPA(clientConnectionString, clientName, enrolleeId, request, ruleTemplate, vendType, effdt, trmdt, sex,
                                           magemeth, fagemeth, magelo, fagelo, dsgid, maxrefills, maxrefmnt, pharmacyId, includeExclude, hidollar,
                                           qtyperdys, qtydylmt, multisourceCode, paidMsg, userName, changedBy, ignorePlan);
            }
            else
            {
                newSysid = await updateMemberRule_DynamicPA(clientConnectionString, clientName, sysid, enrolleeId, request, ruleTemplate, vendType, effdt, trmdt, sex,
                                           magemeth, fagemeth, magelo, fagelo, dsgid, maxrefills, maxrefmnt, pharmacyId, includeExclude, hidollar,
                                           qtyperdys, qtydylmt, multisourceCode, paidMsg, userName, changedBy, ignorePlan);
            }

            return newSysid;
        }

        private int? parseNullableInt(string value)
        {
            int? parsedValue = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                parsedValue = int.Parse(value);
            }

            return parsedValue;
        }

        private double? parseNullableDouble(string value)
        {
            double? parsedValue = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                parsedValue = double.Parse(value);
            }

            return parsedValue;
        }

        private async Task<string> addMemberRule_DynamicPA(string clientConnectionString, string clientName, string enrolleeId, DynamicPARequest request,
                                                 RuleTemplateDTO ruleTemplate, string vendType, DateTime? effdt, DateTime? trmdt, string sex,
                                                 string magemeth, string fagemeth, int? magelo, int? fagelo, string dsgid, int? maxrefills,
                                                 int? maxrefmnt, string pharmacyId, string includeExclude, int? hidollar, double? qtyperdys,
                                                 int? qtydylmt, string multisourceCode, string paidMsg, string userName, string changedBy, bool ignorePlan)
        {
            return await Repository.AddMemberRule_DynamicPA(
                clientConnectionString
                , clientName
                , request.PlanID
                , request.MemberID
                , enrolleeId
                , ruleTemplate.CODES
                , ruleTemplate.EPA_ID
                , ruleTemplate.DESC
                , ruleTemplate.TYPE
                , effdt
                , trmdt
                , ruleTemplate.CODETYPE
                , vendType
                , ruleTemplate.PADENIED
                , sex
                , magemeth
                , fagemeth
                , magelo
                , ruleTemplate.MAGEHI
                , fagelo
                , ruleTemplate.FAGEHI
                , ruleTemplate.APPLYACC
                , ruleTemplate.BAPPACC
                , dsgid
                , ruleTemplate.DSGID2
                , ruleTemplate.CALCREFILL
                , ruleTemplate.REFILLDAYS
                , ruleTemplate.REFILLMETH
                , ruleTemplate.REFILLPCT
                , maxrefills
                , maxrefmnt
                , ruleTemplate.PENALTY
                , ruleTemplate.DESI
                , ruleTemplate.PHYLIMIT
                , ruleTemplate.GI_GPI
                , ruleTemplate.PPGID
                , pharmacyId
                , includeExclude
                , ruleTemplate.INCCOMP
                , ruleTemplate.BRANDDISC
                , ruleTemplate.GENONLY
                , ruleTemplate.DRUGCLASS
                , ruleTemplate.DRUGTYPE
                , ruleTemplate.DRUGSTAT
                , ruleTemplate.MAINTIND
                , ruleTemplate.COMPMAX
                , hidollar
                , qtyperdys
                , qtydylmt
                , multisourceCode
                , ruleTemplate.COPLVLASSN
                , ruleTemplate.OVRRJTADI
                , ruleTemplate.OVRRJTAGE
                , ruleTemplate.OVRRJTADD
                , ruleTemplate.OVRRJTDDC
                , ruleTemplate.OVRRJTDOT
                , ruleTemplate.OVRRJTDUP
                , ruleTemplate.OVRRJTIAT
                , ruleTemplate.OVRRJTMMA
                , ruleTemplate.OVRRJTLAC
                , ruleTemplate.OVRRJTPRG
                , ruleTemplate.ACTIVE
                , request.Note
                , null
                , request.VendorPANumber
                , paidMsg
                , ruleTemplate.REASON
                , userName
                , changedBy
                , ignorePlan).ConfigureAwait(false);
        }

        private async Task<string> updateMemberRule_DynamicPA(string clientConnectionString, string clientName, string sysid, string enrolleeId,
                                                    DynamicPARequest request, RuleTemplateDTO ruleTemplate, string vendType, DateTime? effdt,
                                                    DateTime? trmdt, string sex, string magemeth, string fagemeth, int? magelo, int? fagelo,
                                                    string dsgid, int? maxrefills, int? maxrefmnt, string pharmacyId, string includeExclude,
                                                    int? hidollar, double? qtyperdys, int? qtydylmt, string multisourceCode, string paidMsg,
                                                    string userName, string changedBy, bool ignorePlan)
        {
            return await Repository.UpdateMemberRule_DynamicPA(
                clientConnectionString
                , clientName
                , sysid
                , request.PlanID
                , request.MemberID
                , enrolleeId
                , ruleTemplate.CODES
                , ruleTemplate.EPA_ID
                , ruleTemplate.DESC
                , ruleTemplate.TYPE
                , effdt
                , trmdt
                , ruleTemplate.CODETYPE
                , vendType
                , ruleTemplate.PADENIED
                , sex
                , magemeth
                , fagemeth
                , magelo
                , ruleTemplate.MAGEHI
                , fagelo
                , ruleTemplate.FAGEHI
                , ruleTemplate.APPLYACC
                , ruleTemplate.BAPPACC
                , dsgid
                , ruleTemplate.DSGID2
                , ruleTemplate.CALCREFILL
                , ruleTemplate.REFILLDAYS
                , ruleTemplate.REFILLMETH
                , ruleTemplate.REFILLPCT
                , maxrefills
                , maxrefmnt
                , ruleTemplate.PENALTY
                , ruleTemplate.DESI
                , ruleTemplate.PHYLIMIT
                , ruleTemplate.GI_GPI
                , ruleTemplate.PPGID
                , pharmacyId
                , includeExclude
                , ruleTemplate.INCCOMP
                , ruleTemplate.BRANDDISC
                , ruleTemplate.GENONLY
                , ruleTemplate.DRUGCLASS
                , ruleTemplate.DRUGTYPE
                , ruleTemplate.DRUGSTAT
                , ruleTemplate.MAINTIND
                , ruleTemplate.COMPMAX
                , hidollar
                , qtyperdys
                , qtydylmt
                , multisourceCode
                , ruleTemplate.COPLVLASSN
                , ruleTemplate.OVRRJTADI
                , ruleTemplate.OVRRJTAGE
                , ruleTemplate.OVRRJTADD
                , ruleTemplate.OVRRJTDDC
                , ruleTemplate.OVRRJTDOT
                , ruleTemplate.OVRRJTDUP
                , ruleTemplate.OVRRJTIAT
                , ruleTemplate.OVRRJTMMA
                , ruleTemplate.OVRRJTLAC
                , ruleTemplate.OVRRJTPRG
                , ruleTemplate.ACTIVE
                , request.Note
                , null
                , request.VendorPANumber
                , paidMsg
                , ruleTemplate.REASON
                , userName
                , changedBy
                , ignorePlan).ConfigureAwait(false);
        }
        #endregion Rule

        #region Eligibility

        private async Task<ClientWithConfigurationsDTO> getClientWithConfigurations(CommonApiHelper commonApiHelper, string settingName, string userName)
        {
            var setting = await commonApiHelper.GetSetting(settingName).ConfigureAwait(false);
            var clientApiConfigurations = JsonConfigReader.Deserialize<Dictionary<string, string>>(setting[userName]);

            long clientID = long.Parse(clientApiConfigurations["ClientID"]);
            string databaseUserName = clientApiConfigurations["UserName"];

            ClientWithConfigurationsDTO clientWithConfigs = EligibilityRepository.GetClientWithClientConfiguration(clientID);

            clientWithConfigs.UserName = databaseUserName;
            clientWithConfigs.ADSConnectionString = MultipleConnectionsHelper.GetEProcareConnectionString(clientWithConfigs.ADSDatabasePath);

            return clientWithConfigs;
        }

        private async Task<ClientACPDTO> getClientACPConfigurations(CommonApiHelper commonApiHelper, string settingName, string userName)
        {
            ClientACPDTO clientACPWithConfigs = new ClientACPDTO();

            var setting = await commonApiHelper.GetSetting(settingName).ConfigureAwait(false);

            if (setting.ContainsKey(userName))
            {
                var clientACPApiConfigurations = JsonConfigReader.Deserialize<Dictionary<string, string>>(setting[userName]);

                long clientACPID = long.Parse(clientACPApiConfigurations["Client_ACPID"]);
                string databaseUserName = clientACPApiConfigurations["UserName"];

                clientACPWithConfigs = EligibilityRepository.GetClientACPConfiguration(clientACPID);

                clientACPWithConfigs.UserName = databaseUserName;
            }

            return clientACPWithConfigs;
        }

        private static string generateAccumulatorImportXML(AccumulatorImportRequest request)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(VendorACCUMTansactionRecord), "");
            var xml = "";

            VendorACCUMTansactionRecord xmlRequest = new VendorACCUMTansactionRecord(request.ClientID, request.RecordID, request.Member);

            using (var sww = new StringWriterExtensions.StringWriterWithEncoding(Encoding.UTF8))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };
                using (XmlWriter writer = XmlWriter.Create(sww, settings))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces(
                        new[] { XmlQualifiedName.Empty });

                    xsSubmit.Serialize(writer, xmlRequest, ns);
                    xml = sww.ToString();
                }
            }

            return xml;
        }

        private static string generateEligibilityImportXML(EligibilityImportRequest request)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(VendorADTTansactionRecord), "");
            var xml = "";

            VendorADTTansactionRecord xmlRequest = new VendorADTTansactionRecord(request.ClientID, request.RecordID, request.Member);

            using (var sww = new StringWriterExtensions.StringWriterWithEncoding(Encoding.UTF8))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };
                using (XmlWriter writer = XmlWriter.Create(sww, settings))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces(
                        new[] { XmlQualifiedName.Empty });

                    xsSubmit.Serialize(writer, xmlRequest, ns);
                    xml = sww.ToString();
                }
            }

            return xml;
        }

        private long saveACPImportToDatabase(ACPImport import)
        {
            long importID = 0;

            if (import.Import_ACPID > 0)
            {
                import.UpdateTime = DateTime.Now;
            }

            importID = EligibilityRepository.SaveACPImport(import.Import_ACPID, import.TransactionTypeID, import.ClientID, import.ImportStatusID, import.RawData, import.CreatedTime, import.CompletedTime, import.ReturnValue,
                                             import.WarningMessage, import.ErrorMessage, import.RecIdentifier, import.RecordAction, import.NewBalance, import.InsertAppUserID, import.UpdateAppUserID);

            return importID;
        }

        private long saveImportToDatabase(Import import)
        {
            long importID = 0;

            if (import.ImportID > 0)
            {
                import.UpdateTime = DateTime.Now;
            }

            importID = EligibilityRepository.SaveImport(import.ImportID, import.TransactionTypeID, import.ClientID, import.ImportStatusID, import.PreImportID, import.PlanID,
                                    import.PatName, import.RawData, import.CreatedTime, import.CompletedTime, import.ReturnValue,
                                    import.WarningMessage, import.ErrorMessage, import.RecordID, import.RecordAction, import.InsertAppUserID, import.UpdateAppUserID);

            return importID;
        }

        private void processImportRequest(BaseRequest request, Import import, ClientWithConfigurationsDTO clientWithConfigs)
        {
            var fileFormat = (FileFormats)clientWithConfigs.DataTypeID;

            bool canProcessRecord = true;

            switch (fileFormat)
            {
                case FileFormats.XML:
                    canProcessRecord = validateXMLStructure(import, clientWithConfigs);
                    break;
                default:
                    throw new NotSupportedException("Unsupported TransactionTypeID");
            }

            if (canProcessRecord)
            {
                try
                {
                    processRecord(request, import, clientWithConfigs);
                }
                catch (Exception e)
                {
                    import.ErrorMessage = e.Message;
                    saveImportToDatabase(import);
                }
            }
        }

        private bool validateXMLStructure(Import import, ClientWithConfigurationsDTO clientWithConfigs)
        {
            bool valid = true;

            //Load Step
            List<string> errors = EligibilityHelper.ValidateXML(import.RawData, clientWithConfigs.XSD);

            if (errors.Count > 0)
            {
                valid = false;
                import.ImportStatusID = (long)ImportStatus.ParseError;
                import.ErrorMessage = errors.ToString();
            }
            else
            {
                import.ImportStatusID = (long)ImportStatus.Parsed;
                import.PatName = "Identified";
            }

            saveImportToDatabase(import);

            //Update Pat Name step
            import.PatName = "Unknown Patient";
            saveImportToDatabase(import);

            //Process step
            if (import.ImportStatusID != (long)ImportStatus.Parsed)
            {
                valid = false;
                throw new FormatException("XML format does not match XSD");
            }

            return valid;
        }

        private void processRecord(BaseRequest request, Import import, ClientWithConfigurationsDTO clientWithConfigs)
        {
            switch ((TransactionType)import.TransactionTypeID)
            {
                case TransactionType.Eligibility:
                    EligibilityImportRequest eligibilityRequest = (EligibilityImportRequest)request;
                    import.PatName = String.Format("{0} {1}", eligibilityRequest.Member.FirstName.ToUpper(), eligibilityRequest.Member.LastName.ToUpper());
                    import.PlanID = eligibilityRequest.Member.PlanID;
                    processEligibility(eligibilityRequest, import, clientWithConfigs);
                    break;
                case TransactionType.TMGAccumulator:
                    AccumulatorImportRequest accumulatorRequest = (AccumulatorImportRequest)request;
                    import.PatName = String.Format("{0} {1}", accumulatorRequest.Member.FirstName.ToUpper(), accumulatorRequest.Member.LastName.ToUpper());
                    import.PlanID = accumulatorRequest.Member.PlanID;
                    processAccumulator(accumulatorRequest, import, clientWithConfigs);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void processEligibility(EligibilityImportRequest request, Import import, ClientWithConfigurationsDTO clientWithConfigs)
        {
            bool validRecord = true;
            string warningString = "";
            string errorString = "";

            import.ImportStatusID = (long)ImportStatus.Open;
            import.ReturnValue = "";

            validRecord = isValidPlanID(clientWithConfigs.ADSConnectionString, import.PlanID, out errorString);

            MemberDTO oldMember = EligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(clientWithConfigs.ADSConnectionString, request.Member.PlanID, getCardID(request.Member.CardID), getCardID2(request.Member.CardID),
                                                       request.Member.Person).FirstOrDefault();
            MemberDTO memberToSave = new MemberDTO();

            if (oldMember == null)
            {
                oldMember = EligibilityRepository.GetHistoryMembersByPlanIDCardIDCardID2Person(clientWithConfigs.ADSConnectionString, request.Member.PlanID, getCardID(request.Member.CardID),
                                                             getCardID2(request.Member.CardID),
                                                             request.Member.Person).FirstOrDefault();

                if (oldMember == null)
                {
                    oldMember = new MemberDTO();
                    import.RecordAction = "Inserted Record";
                }
                else
                {
                    errorString = "Card ID already on file in APSENH";
                    validRecord = false;
                }
            }
            else
            {
                memberToSave.USERNAME = clientWithConfigs.UserName;
                memberToSave = oldMember.CreateCopy();
                import.RecordAction = "Updated Record";
            }

            long reinstateSysID = 0L;

            if (validRecord)
            {
                validRecord = validEligibilityRecord(clientWithConfigs.ADSConnectionString, request, memberToSave, oldMember, clientWithConfigs, out reinstateSysID, out warningString, out errorString);
            }

            bool recordSaved = false;

            if (validRecord)
            {
                recordSaved = saveEligibilityRecordData(clientWithConfigs.ADSConnectionString, reinstateSysID, request, oldMember, memberToSave, import, clientWithConfigs);
            }

            if (!validRecord || !recordSaved)
            {
                import.RecordAction = "";
            }
            else
            {
                memberToSave = EligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(clientWithConfigs.ADSConnectionString, memberToSave.PLNID, memberToSave.CARDID, memberToSave.CARDID2,
                                                           memberToSave.PERSON).FirstOrDefault();

                import.ImportStatusID = (long)ImportStatus.Closed;
            }

            //APSSUS suspensions can be saved independently of the main APSENR record.
            if (validRecord && string.IsNullOrWhiteSpace(errorString) && !string.IsNullOrWhiteSpace(request.Member.CutOffMethod))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(import.RecordAction))
                    {
                        import.RecordAction = "Updated Record";
                    }

                    string suspensionSysid = processSuspension(clientWithConfigs.ADSConnectionString, request, clientWithConfigs.UserName);
                }
                catch (Exception e)
                {
                    errorString = e.Message;
                    import.ImportStatusID = (long)ImportStatus.ProcessError;
                }
            }

            import.RecordID = request.RecordID.ToString();

            updateImportStatus(import, warningString, errorString);
        }

        private string processSuspension(string adsConnectionString, EligibilityImportRequest request, string userName)
        {
            string tableName = "APSENR";
            string linkKey = request.Member.CardID;

            SuspensionDTO suspension = EligibilityRepository.GetSuspensionByTableNameLinkKey(adsConnectionString, tableName, linkKey).FirstOrDefault();

            if (suspension == null)
            {
                suspension = new SuspensionDTO
                {
                    TABLENAME = tableName,
                    LINKKEY = linkKey,
                    CUTOFFMETH = request.Member.CutOffMethod,
                    JOURNAL = "Y"
                };
            }

            SuspensionDTO oldSuspension = suspension.CreateCopy();
            suspension.CUTOFFMETH = request.Member.CutOffMethod;

            if (suspension.CUTOFFMETH.Equals("R"))
            {
                suspension.CUTOFFDATE = null;
            }
            else
            {
                suspension.CUTOFFDATE = DateTime.Today;
            }

            return logAndSaveSuspension(adsConnectionString, oldSuspension, suspension, userName, true);
        }

        private void processAccumulator(AccumulatorImportRequest request, Import import, ClientWithConfigurationsDTO clientWithConfigs)
        {
            string warningString = "";
            string errorString = "";

            import.ImportStatusID = (long)ImportStatus.Open;

            var member = EligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(clientWithConfigs.ADSConnectionString, request.Member.PlanID, getCardID(request.Member.CardID), getCardID2(request.Member.CardID),
                                                       request.Member.Person).FirstOrDefault();

            string enrid = member?.ENRID;
            string subid = member?.SUBID;

            BenefitYearAccumulatorDTO accumulator = null;
            BenefitYearAccumulatorDTO oldAccumulator = new BenefitYearAccumulatorDTO();

            if (string.IsNullOrEmpty(enrid))
            {
                import.ErrorMessage = "Missing Eligibility Record";
                import.ImportStatusID = (long)ImportStatus.ProcessError;
            }
            else
            {
                accumulator =
                    EligibilityRepository.GetBenefitYearAccumulatorByPlanIDEnrolleeID(clientWithConfigs.ADSConnectionString, request.Member.PlanID, enrid).FirstOrDefault();

                if (accumulator == null)
                {
                    accumulator = new BenefitYearAccumulatorDTO();
                    accumulator.PLNID = request.Member.PlanID;
                    accumulator.ENRID = enrid;
                    accumulator.SUBID = subid;
                    accumulator.PLANTYPE = "RETL";
                    accumulator.EFFDT = EligibilityHelper.ForceValidDateTime(request.Member.EffectiveDate);
                    accumulator.TRMDT = EligibilityHelper.ForceValidDateTime(request.Member.TerminationDate);
                    accumulator.CLASS = "N";
                    accumulator.JOURNAL = "Y";
                }
                else
                {
                    oldAccumulator = accumulator.CreateCopy();
                }

                import.ReturnValue = saveAccumulator(clientWithConfigs.ADSConnectionString, request, oldAccumulator, accumulator, clientWithConfigs.UserName, out string recordAction,
                                                     out long importStatus, out warningString, out errorString);
                import.RecordAction = recordAction;
                import.ImportStatusID = importStatus;
            }

            updateImportStatus(import, warningString, errorString);
        }

        private string saveAccumulator(string adsConnectionString, AccumulatorImportRequest request, BenefitYearAccumulatorDTO oldAccumulator, BenefitYearAccumulatorDTO accumulator, string userName, out string recordAction, out long importStatusId, out string warningString, out string errorString)
        {
            importStatusId = 0L;
            warningString = "";
            string sysid = "";

            DateTime EffectiveDate = EligibilityHelper.ForceValidDateTime(request.Member.EffectiveDate);
            DateTime TerminationDate = EligibilityHelper.ForceValidDateTime(request.Member.TerminationDate);

            DateTime saveEffectiveDate = accumulator.EFFDT;
            DateTime saveTerminationDate = accumulator.TRMDT;

            determineTypeOfSave(oldAccumulator.SYSID, accumulator.SYSID, EffectiveDate, TerminationDate, ref saveEffectiveDate, ref saveTerminationDate,
                                out recordAction,
                                out importStatusId, out errorString, out bool saveRecord, out bool writeLog, out bool writeAccumulator,
                                out bool writeHistory);

            accumulator.EFFDT = saveEffectiveDate;
            accumulator.TRMDT = saveTerminationDate;

            if (saveRecord)
            {
                try
                {
                    sysid = logAndSaveAccumulator(adsConnectionString, oldAccumulator, accumulator, userName, writeLog, writeAccumulator, writeHistory);
                    importStatusId = (long)ImportStatus.Closed;
                }
                catch (Exception)
                {
                    importStatusId = (long)ImportStatus.ProcessError;
                    recordAction = "";
                }
            }
            else
            {
                recordAction = "";

                if (string.IsNullOrWhiteSpace(errorString))
                {
                    importStatusId = (long)ImportStatus.NoChanges;
                    sysid = oldAccumulator.SYSID;
                }
            }

            return sysid;
        }

        private bool validEligibilityRecord(string adsConnectionString, EligibilityImportRequest request, MemberDTO memberToSave, MemberDTO oldMember, ClientWithConfigurationsDTO clientWithConfigs, out long reinstateSysID, out string warningString, out string errorString)
        {
            errorString = "";
            warningString = "";
            bool validRecord = true;
            reinstateSysID = 0L;
            int personCode = int.Parse(request.Member.Person);

            validRecord = !EligibilityHelper.IsNonDuplicateCardID(oldMember, request.Member, out errorString);

            if (validRecord && clientWithConfigs.FlexValidations)
            {
                //TMG does not have flex validations
            }

            if (validRecord)
            {
                validRecord = EligibilityHelper.IsValidDOB(EligibilityHelper.ForceValidDateTime(request.Member.DOB), clientWithConfigs.DOBPresent,
                                                            out string warningMessage, out errorString);
                if (!string.IsNullOrWhiteSpace(warningMessage))
                {
                    if (string.IsNullOrWhiteSpace(warningString))
                    {
                        warningString = warningMessage;
                    }
                    else
                    {
                        warningString = warningString + "|" + warningMessage;
                    }
                }
            }

            if (validRecord)
            {
                validRecord = EligibilityHelper.IsValidEffectiveDate(EligibilityHelper.ForceValidDateTime(request.Member.EffectiveDate), out errorString);
            }

            if (validRecord)
            {
                validRecord = EligibilityHelper.IsValidTerminationDate(EligibilityHelper.ForceValidDateTime(request.Member.TerminationDate),
                                                                        EligibilityHelper.ForceValidDateTime(request.Member.EffectiveDate), out errorString);
            }

            if (validRecord && !string.IsNullOrWhiteSpace(memberToSave.SYSID))
            {
                if (oldMember.TRMDT.HasValue && (oldMember.TRMDT != DateTime.MinValue && oldMember.TRMDT != DateTime.MaxValue))
                {
                    DateTime requestTermDate = EligibilityHelper.ForceValidDateTime(request.Member.TerminationDate);
                    if (requestTermDate == DateTime.MinValue || requestTermDate == DateTime.MaxValue)
                    {
                        reinstateSysID = long.Parse(oldMember.SYSID);
                        memberToSave.MBRSINCE = oldMember.MBRSINCE;
                    }
                }
            }

            if (validRecord && clientWithConfigs.TermDateOffset)
            {
                //TMG does not have term date offset
            }

            if (validRecord && request.Member.Relationship == 2)
            {
                validRecord = EligibilityHelper.IsValidSpouseChange(oldMember, request.Member, out errorString);

                if (validRecord)
                {
                    validRecord = EligibilityHelper.IsValidSpouseAge(EligibilityHelper.ForceValidDateTime(request.Member.DOB), clientWithConfigs.MinSpouseAge, out errorString);
                }
            }

            if (validRecord && personCode > 1)
            {
                var members = EligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(adsConnectionString, request.Member.PlanID, getCardID(request.Member.CardID),
                                             getCardID2(request.Member.CardID),
                                             "01");
                if (members.Count > 0)
                {
                    memberToSave.SUBID = members.FirstOrDefault().SUBID;
                }
                else
                {
                    validRecord = false;
                    errorString = "No Valid 01 on file";
                }
            }

            if (validRecord)
            {
                validRecord = EligibilityHelper.IsValidPersonAndRelationship(personCode, request.Member.Relationship, out errorString);
            }


            return validRecord;
        }

        private void updateImportStatus(Import import, string warningString, string errorString)
        {
            import.CompletedTime = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(errorString))
            {
                import.ErrorMessage = errorString;
            }

            if (!string.IsNullOrWhiteSpace(warningString))
            {
                import.WarningMessage = warningString;
            }

            saveImportToDatabase(import);
        }

        private bool recordChanged(MemberDTO memberToSave)
        {
            //Always returns true for TMG
            return true;
        }

        private bool saveEligibilityRecordData(string adsConnectionString, long reinstateSysID, EligibilityImportRequest request, MemberDTO oldMember, MemberDTO memberToSave, Import import, ClientWithConfigurationsDTO clientWithConfigs)
        {
            bool saved = false;
            int personCode = int.Parse(request.Member.Person);
            bool saveRecord = false;
            bool writeLog = false;
            bool writeMember = false;
            bool writeHistory = false;

            try
            {
                DateTime EffectiveDate = EligibilityHelper.ForceValidDateTime(request.Member.EffectiveDate);
                DateTime TerminationDate = EligibilityHelper.ForceValidDateTime(request.Member.TerminationDate);


                if (recordChanged(memberToSave))
                {
                    setMemberValuesFromRequestAndConfigs(ref oldMember, ref memberToSave, request, clientWithConfigs, personCode, reinstateSysID);

                    //Don't save records that haven't changed
                    if (string.IsNullOrWhiteSpace(oldMember.SYSID) || !memberToSave.Equals(oldMember))
                    {
                        DateTime saveEffectiveDate = string.IsNullOrWhiteSpace(oldMember.SYSID) ? memberToSave.EFFDT : oldMember.EFFDT;
                        DateTime saveTerminationDate = string.IsNullOrWhiteSpace(oldMember.SYSID) ? memberToSave.TRMDT.Value : oldMember.TRMDT.Value;

                        determineTypeOfSave(oldMember.SYSID, memberToSave.SYSID, EffectiveDate, TerminationDate, ref saveEffectiveDate,
                                            ref saveTerminationDate,
                                            out string recordAction,
                                            out long importStatusId, out string errorString, out saveRecord, out writeLog,
                                            out writeMember,
                                            out writeHistory);

                        memberToSave.EFFDT = saveEffectiveDate;
                        memberToSave.TRMDT = saveTerminationDate;
                        import.RecordAction = recordAction;
                        import.ImportStatusID = importStatusId;
                        if (!string.IsNullOrWhiteSpace(errorString))
                        {
                            import.ErrorMessage = errorString;
                        }
                        else
                        {
                            saveRecord = true;
                        }
                    }
                }

                if (saveRecord)
                {
                    if (reinstateSysID == 0)
                    {
                        import.ReturnValue = logAndSaveMember(adsConnectionString, oldMember, memberToSave, clientWithConfigs.UserName, writeLog,
                                                                writeMember, writeHistory);
                        saved = true;

                        if (personCode > 1 && clientWithConfigs.TerminateDependents)
                        {
                            //Dependents not terminated for TMG
                        }
                    }
                    //We shouldn't hit this case anymore now that TMG is requiring TermDates
                    else
                    {
                        DateTime reinstateDate = memberToSave.TRMDT.HasValue && EffectiveDate <= memberToSave.TRMDT
                            ? memberToSave.TRMDT.GetValueOrDefault()
                            : EffectiveDate;
                        saved = reinstateMember(adsConnectionString, oldMember, memberToSave, reinstateDate, false, true);
                    }

                    import.ImportStatusID = (long)ImportStatus.Closed;
                }
                else
                {
                    import.RecordAction = "";
                    import.ReturnValue = "";

                    if (string.IsNullOrWhiteSpace(import.ErrorMessage))
                    {
                        import.ImportStatusID = (long)ImportStatus.NoChanges;
                        import.ReturnValue = oldMember.SYSID;
                    }
                }
            }
            catch (Exception e)
            {
                import.ImportStatusID = (long)ImportStatus.ProcessError;
                import.ErrorMessage = e.Message;
            }

            return saved;
        }

        private void setMemberValuesFromRequestAndConfigs(ref MemberDTO oldMember, ref MemberDTO memberToSave, EligibilityImportRequest request, ClientWithConfigurationsDTO clientWithConfigs, int personCode, long reinstateSysID)
        {
            memberToSave.ACCMETH = "Y";
            memberToSave.ACCUM = 0;
            memberToSave.ADDEDBY = clientWithConfigs.UserName.Substring(0, 8).ToUpper();
            memberToSave.USERNAME = clientWithConfigs.UserName;
            oldMember.USERNAME = clientWithConfigs.UserName;

            if (request.Member.AddressLine1.Length > 0)
            {
                memberToSave.ADDR = request.Member.AddressLine1.ToUpper();
            }

            if (request.Member.AddressLine2.Length > 0)
            {
                memberToSave.ADDR2 = request.Member.AddressLine2.ToUpper();
            }

            memberToSave.CARDID = getCardID(request.Member.CardID);
            memberToSave.CARDID2 = getCardID2(request.Member.CardID);

            if (request.Member.City.Length > 0)
            {
                memberToSave.CITY = request.Member.City.ToUpper();
            }

            memberToSave.CRDDT = DateTime.MinValue;
            memberToSave.DOB = EligibilityHelper.ForceValidDateTime(request.Member.DOB);
            memberToSave.EFFDT = EligibilityHelper.ForceValidDateTime(request.Member.EffectiveDate);
            memberToSave.ELGCD = "03";
            memberToSave.ELGOVER = "1";
            memberToSave.EMPCD = "01";
            memberToSave.FLEX1 = request.Member.Flex1;
            memberToSave.FLEX2 = request.Member.Flex2;
            memberToSave.FLEX3 = request.Member.Flex3;
            memberToSave.FNAME = request.Member.FirstName.ToUpper();
            memberToSave.LASTUPDT = DateTime.Now;
            memberToSave.LNAME = request.Member.LastName.ToUpper();
            memberToSave.LSTDTCARD = DateTime.MinValue;
            memberToSave.MBRSINCE = memberToSave.MBRSINCE != DateTime.MinValue ? memberToSave.MBRSINCE : EligibilityHelper.ForceValidDateTime(request.Member.EffectiveDate);
            memberToSave.MNAME = request.Member.MiddleName.ToUpper();
            memberToSave.NDCUPDATE = string.IsNullOrWhiteSpace(memberToSave.SYSID) ? "A" : "C";
            memberToSave.PERSON = request.Member.Person;
            memberToSave.OLDPERSON = clientWithConfigs.UseOldPerson ? request.Member.Person : "";
            memberToSave.PHONE = request.Member.Phone;
            memberToSave.PLNID = request.Member.PlanID.ToUpper();
            memberToSave.RELCD = request.Member.Relationship.ToString();
            memberToSave.SEX = request.Member.Gender.ToUpper();
            memberToSave.STATE = request.Member.State.ToUpper();

            List<string> allowGovYValues = new List<string> { "TRUE", "1" };
            memberToSave.ALLOWGOVT = allowGovYValues.Contains(request.Member.AllowGovernment?.ToUpper()) ? "Y" : "";

            if (memberToSave.DOB == DateTime.MinValue || memberToSave.DOB == DateTime.MaxValue)
            {
                memberToSave.DOB = null;
            }

            DateTime trmDt = EligibilityHelper.ForceValidDateTime(request.Member.TerminationDate);

            if (trmDt == DateTime.MinValue || trmDt == DateTime.MaxValue)
            {
                memberToSave.TRMDT = null;
            }
            else
            {
                memberToSave.TRMDT = reinstateSysID > 0 ? DateTime.MinValue : trmDt;
            }

            memberToSave.USEELM = "N";
            memberToSave.ZIP = request.Member.Zip;
            memberToSave.ZIP4 = request.Member.Zip4;

            if (!string.IsNullOrWhiteSpace(oldMember.SUBID))
            {
                memberToSave.SUBID = oldMember.SUBID;
                memberToSave.ENRID = oldMember.ENRID;
            }

            memberToSave.COB = request.Member.COB;
            memberToSave.ELGCD = request.Member.CoverageType;

            if (personCode == 1)
            {
                memberToSave.SUBID = memberToSave.ENRID;
            }

            memberToSave.JOURNAL = "Y";

            memberToSave.LASTUPDT = DateTime.Now;
        }

        private void determineTypeOfSave(string oldSysid, string sysid, DateTime requestEffectiveDate, DateTime requestTerminationDate, ref DateTime saveEffectiveDate, ref DateTime saveTerminationDate, out string recordAction, out long importStatusID, out string errorString, out bool saveRecord, out bool saveLog, out bool writeMainRecord, out bool writeHistory)
        {
            saveRecord = true;
            saveLog = true;
            writeMainRecord = true;
            writeHistory = false;

            recordAction = "";
            importStatusID = 0L;
            errorString = "";

            if (string.IsNullOrWhiteSpace(oldSysid))
            {
                recordAction = "Inserted Record";
            }

            if (!string.IsNullOrWhiteSpace(sysid))
            {
                recordAction = "Updated Record";

                if (requestEffectiveDate == saveEffectiveDate && requestTerminationDate == saveTerminationDate)
                {
                    //#1
                    saveRecord = false;
                    importStatusID = (long)ImportStatus.NoChanges;
                }
                else
                {
                    if (saveEffectiveDate == requestEffectiveDate && saveTerminationDate != requestTerminationDate)
                    {
                        //#2 & #3
                        saveTerminationDate = requestTerminationDate;
                    }
                    else if (saveTerminationDate == requestTerminationDate && saveEffectiveDate != requestEffectiveDate)
                    {
                        if (requestEffectiveDate > saveEffectiveDate)
                        {
                            //#5
                            writeHistory = true;
                        }

                        //#4
                        saveEffectiveDate = requestEffectiveDate;
                    }
                    else if ((requestEffectiveDate > saveEffectiveDate && requestEffectiveDate < saveTerminationDate) ||
                             (requestTerminationDate > saveEffectiveDate && requestTerminationDate < saveTerminationDate) ||
                             (requestEffectiveDate < saveEffectiveDate && requestTerminationDate > saveTerminationDate))
                    {
                        //#6, #7, #10, & #13
                        saveRecord = false;
                        importStatusID = (long)ImportStatus.ProcessError;
                        errorString = "Creates BYA Overlap";
                    }
                    else if (requestTerminationDate > saveTerminationDate)
                    {
                        if (requestEffectiveDate >= saveTerminationDate)
                        {
                            //#8 & #12
                            saveEffectiveDate = requestEffectiveDate;
                            saveTerminationDate = requestTerminationDate;
                            writeHistory = true;
                        }
                    }
                    else if (requestEffectiveDate < saveEffectiveDate)
                    {
                        if (requestTerminationDate <= saveEffectiveDate)
                        {
                            //#9 & #11
                            saveEffectiveDate = requestEffectiveDate;
                            saveTerminationDate = requestTerminationDate;
                            writeMainRecord = false;
                            writeHistory = true;
                            //Not updating main record, inserting history record only
                            recordAction = "Inserted Record";
                        }
                    }
                }
            }
        }

        private bool reinstateMember(string adsConnectionString, MemberDTO oldMember, MemberDTO memberToSave, DateTime reinstateDate, bool validDateRange = true, bool logChanges = false)
        {
            bool setReinstated = true;

            memberToSave.EFFDT = reinstateDate;

            if (!memberToSave.NDCUPDATE.Equals("A"))
            {
                memberToSave.NDCUPDATE = "R";
            }

            saveHistoryMember(adsConnectionString, oldMember);

            if (logChanges)
            {
                string logSysid = logMember(adsConnectionString, oldMember);
                generateEligibilityLogDet(adsConnectionString, logSysid, oldMember, memberToSave);
            }

            saveMember(adsConnectionString, memberToSave);

            return setReinstated;
        }

        private string logAndSaveAccumulator(string adsConnectionString, BenefitYearAccumulatorDTO oldAccumulator, BenefitYearAccumulatorDTO accumulatorToSave, string userName, bool logChanges = false, bool writeAccumulator = true, bool writeHistory = false)
        {
            bool delayLog = false;
            BenefitYearAccumulatorDTO accumulatorToLog = oldAccumulator;
            string logSysid = "";
            string accumulatorSysid = "";

            if (writeHistory)
            {
                if (writeAccumulator)
                {
                    saveHistoryAccumulator(adsConnectionString, oldAccumulator);
                }
                else
                {
                    accumulatorSysid = saveHistoryAccumulator(adsConnectionString, accumulatorToSave);
                    accumulatorToLog = accumulatorToSave;
                }
            }

            if (logChanges)
            {
                if (string.IsNullOrWhiteSpace(accumulatorToLog.SYSID))
                {
                    delayLog = true;
                }

                if (!delayLog)
                {
                    accumulatorToLog.USERNAME = userName;
                    logSysid = logAccumulator(adsConnectionString, accumulatorToLog);
                }
            }

            if (writeAccumulator)
            {
                accumulatorSysid = saveAccumulator(adsConnectionString, accumulatorToSave);
            }

            if (logChanges && delayLog)
            {
                accumulatorToLog = EligibilityRepository.GetBenefitYearAccumulatorByPlanIDEnrolleeID(adsConnectionString, accumulatorToSave.PLNID, accumulatorToSave.ENRID)
                                         .FirstOrDefault();

                accumulatorToLog.USERNAME = userName;
                logSysid = logAccumulator(adsConnectionString, accumulatorToLog);
            }

            if (!string.IsNullOrWhiteSpace(logSysid))
            {
                //No LOGDET for APSBYA
            }

            return accumulatorSysid;
        }

        private string logAndSaveSuspension(string adsConnectionString, SuspensionDTO oldSuspension, SuspensionDTO suspensionToSave, string userName, bool logChanges = false)
        {
            bool delayLog = false;
            SuspensionDTO suspensionToLog = oldSuspension;

            if (logChanges)
            {
                if (string.IsNullOrWhiteSpace(suspensionToLog.SYSID))
                {
                    delayLog = true;
                }

                if (!delayLog)
                {
                    suspensionToLog.USERNAME = userName;
                    logSuspension(adsConnectionString, suspensionToLog);
                }
            }

            string suspensionSysid = saveSuspension(adsConnectionString, suspensionToSave);

            if (logChanges && delayLog)
            {
                suspensionToLog = EligibilityRepository.GetSuspensionByTableNameLinkKey(adsConnectionString, suspensionToSave.TABLENAME, suspensionToSave.LINKKEY).FirstOrDefault();

                suspensionToLog.USERNAME = userName;
                logSuspension(adsConnectionString, suspensionToLog);
            }

            //if (!string.IsNullOrWhiteSpace(logSysid))
            //{
            //    generateEligibilityLogDet(adsConnectionString, logSysid, oldSuspension, suspensionToSave);
            //}

            return suspensionSysid;
        }

        private string logAndSaveMember(string adsConnectionString, MemberDTO oldMember, MemberDTO memberToSave, string userName, bool logChanges = false, bool writeMember = true, bool writeHistory = false)
        {
            bool delayLog = false;
            MemberDTO memberToLog = oldMember;
            string logSysid = "";
            string memberSysid = "";

            if (writeHistory)
            {
                if (writeMember)
                {
                    saveHistoryMember(adsConnectionString, oldMember);
                }
                else
                {
                    memberSysid = saveHistoryMember(adsConnectionString, memberToSave);
                    memberToLog = memberToSave;
                }
            }
            if (logChanges)
            {
                if (string.IsNullOrWhiteSpace(memberToLog.SYSID))
                {
                    delayLog = true;
                }

                if (!delayLog)
                {
                    memberToLog.USERNAME = userName;
                    logSysid = logMember(adsConnectionString, memberToLog);
                }
            }

            if (writeMember)
            {
                memberSysid = saveMember(adsConnectionString, memberToSave);
            }

            if (logChanges && delayLog)
            {
                memberToLog = EligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(adsConnectionString, memberToSave.PLNID, memberToSave.CARDID, memberToSave.CARDID2,
                                                                         memberToSave.PERSON).FirstOrDefault();

                memberToLog.USERNAME = userName;
                logSysid = logMember(adsConnectionString, memberToLog);
            }

            if (!string.IsNullOrWhiteSpace(logSysid))
            {
                generateEligibilityLogDet(adsConnectionString, logSysid, oldMember, memberToSave);
            }

            return memberSysid;
        }

        private void generateEligibilityLogDet(string adsConnectionString, string logSysid, MemberDTO oldMember, MemberDTO newMember)
        {
            processLogDet(adsConnectionString, logSysid, "ENRID", oldMember.ENRID, newMember.ENRID);
            processLogDet(adsConnectionString, logSysid, "PLNID", oldMember.PLNID, newMember.PLNID);
            processLogDet(adsConnectionString, logSysid, "SUBID", oldMember.SUBID, newMember.SUBID);
            processLogDet(adsConnectionString, logSysid, "CARDID", oldMember.CARDID, newMember.CARDID);
            processLogDet(adsConnectionString, logSysid, "PERSON", oldMember.PERSON, newMember.PERSON);
            processLogDet(adsConnectionString, logSysid, "EFFDT", oldMember.EFFDT, newMember.EFFDT);
            processLogDet(adsConnectionString, logSysid, "TRMDT", oldMember.TRMDT, newMember.TRMDT);
            processLogDet(adsConnectionString, logSysid, "FLEX1", oldMember.FLEX1, newMember.FLEX1);
            processLogDet(adsConnectionString, logSysid, "FLEX2", oldMember.FLEX2, newMember.FLEX2);
            processLogDet(adsConnectionString, logSysid, "RELCD", oldMember.RELCD, newMember.RELCD);
            processLogDet(adsConnectionString, logSysid, "ELGOVER", oldMember.ELGOVER, newMember.ELGOVER);
            processLogDet(adsConnectionString, logSysid, "FNAME", oldMember.FNAME, newMember.FNAME);
            processLogDet(adsConnectionString, logSysid, "MNAME", oldMember.MNAME, newMember.MNAME);
            processLogDet(adsConnectionString, logSysid, "LNAME", oldMember.LNAME, newMember.LNAME);
            processLogDet(adsConnectionString, logSysid, "ADDR", oldMember.ADDR, newMember.ADDR);
            processLogDet(adsConnectionString, logSysid, "ADDR2", oldMember.ADDR2, newMember.ADDR2);
            processLogDet(adsConnectionString, logSysid, "CITY", oldMember.CITY, newMember.CITY);
            processLogDet(adsConnectionString, logSysid, "STATE", oldMember.STATE, newMember.STATE);
            processLogDet(adsConnectionString, logSysid, "ZIP", oldMember.ZIP, newMember.ZIP);
            processLogDet(adsConnectionString, logSysid, "ZIP4", oldMember.ZIP4, newMember.ZIP4);
            processLogDet(adsConnectionString, logSysid, "DOB", oldMember.DOB, newMember.DOB);
            processLogDet(adsConnectionString, logSysid, "SEX", oldMember.SEX, newMember.SEX);
            processLogDet(adsConnectionString, logSysid, "ELGCD", oldMember.ELGCD, newMember.ELGCD);
            processLogDet(adsConnectionString, logSysid, "EMPCD", oldMember.EMPCD, newMember.EMPCD);
            processLogDet(adsConnectionString, logSysid, "CRDDT", oldMember.CRDDT, newMember.CRDDT);
            processLogDet(adsConnectionString, logSysid, "SYSID", oldMember.SYSID, newMember.SYSID);
            processLogDet(adsConnectionString, logSysid, "LSTDTCARD", oldMember.LSTDTCARD, newMember.LSTDTCARD);
            processLogDet(adsConnectionString, logSysid, "NOUPDATE", oldMember.NOUPDATE, newMember.NOUPDATE);
            processLogDet(adsConnectionString, logSysid, "NDCUPDATE", oldMember.NDCUPDATE, newMember.NDCUPDATE);
            processLogDet(adsConnectionString, logSysid, "LASTUPDT", oldMember.LASTUPDT, newMember.LASTUPDT);
            processLogDet(adsConnectionString, logSysid, "MBRSINCE", oldMember.MBRSINCE, newMember.MBRSINCE);
            processLogDet(adsConnectionString, logSysid, "PHYID", oldMember.PHYID, newMember.PHYID);
            processLogDet(adsConnectionString, logSysid, "OLDPERSON", oldMember.OLDPERSON, newMember.OLDPERSON);
            processLogDet(adsConnectionString, logSysid, "FLEX3", oldMember.FLEX3, newMember.FLEX3);
            processLogDet(adsConnectionString, logSysid, "OTHERID", oldMember.OTHERID, newMember.OTHERID);
            processLogDet(adsConnectionString, logSysid, "DEPCODE", oldMember.DEPCODE, newMember.DEPCODE);
            processLogDet(adsConnectionString, logSysid, "MAINT", oldMember.MAINT, newMember.MAINT);
            processLogDet(adsConnectionString, logSysid, "ACCUM", oldMember.ACCUM, newMember.ACCUM);
            processLogDet(adsConnectionString, logSysid, "PATSTAT", oldMember.PATSTAT, newMember.PATSTAT);
            processLogDet(adsConnectionString, logSysid, "ENRCOPAYM", oldMember.ENRCOPAYM, newMember.ENRCOPAYM);
            processLogDet(adsConnectionString, logSysid, "ENRCOPAYR", oldMember.ENRCOPAYR, newMember.ENRCOPAYR);
            processLogDet(adsConnectionString, logSysid, "PHYSREQ", oldMember.PHYSREQ, newMember.PHYSREQ);
            processLogDet(adsConnectionString, logSysid, "USEELM", oldMember.USEELM, newMember.USEELM);
            processLogDet(adsConnectionString, logSysid, "ACCMETH", oldMember.ACCMETH, newMember.ACCMETH);
            processLogDet(adsConnectionString, logSysid, "CARDID2", oldMember.CARDID2, newMember.CARDID2);
            processLogDet(adsConnectionString, logSysid, "COB", oldMember.COB, newMember.COB);
            processLogDet(adsConnectionString, logSysid, "JOURNAL", oldMember.JOURNAL, newMember.JOURNAL);
            processLogDet(adsConnectionString, logSysid, "ADDEDBY", oldMember.ADDEDBY, newMember.ADDEDBY);
            processLogDet(adsConnectionString, logSysid, "PMGID", oldMember.PMGID, newMember.PMGID);
            processLogDet(adsConnectionString, logSysid, "PHONE", oldMember.PHONE, newMember.PHONE);
            processLogDet(adsConnectionString, logSysid, "MEDICARE", oldMember.MEDICARE, newMember.MEDICARE);
            processLogDet(adsConnectionString, logSysid, "PPNREQENR", oldMember.PPNREQENR, newMember.PPNREQENR);
            processLogDet(adsConnectionString, logSysid, "PPNID", oldMember.PPNID, newMember.PPNID);
            processLogDet(adsConnectionString, logSysid, "HICN", oldMember.HICN, newMember.HICN);
            processLogDet(adsConnectionString, logSysid, "RXBIN", oldMember.RXBIN, newMember.RXBIN);
            processLogDet(adsConnectionString, logSysid, "RXPCN", oldMember.RXPCN, newMember.RXPCN);
            processLogDet(adsConnectionString, logSysid, "RXGROUP", oldMember.RXGROUP, newMember.RXGROUP);
            processLogDet(adsConnectionString, logSysid, "RXID", oldMember.RXID, newMember.RXID);
            processLogDet(adsConnectionString, logSysid, "TRELIG", oldMember.TRELIG, newMember.TRELIG);
            processLogDet(adsConnectionString, logSysid, "PHYQUAL", oldMember.PHYQUAL, newMember.PHYQUAL);
            processLogDet(adsConnectionString, logSysid, "MMEDAYMAX", oldMember.MMEDAYMAX, newMember.MMEDAYMAX);
            processLogDet(adsConnectionString, logSysid, "ALLOWGOVT", oldMember.ALLOWGOVT, newMember.ALLOWGOVT);
        }

        private void processLogDet(string adsConnectionString, string logSysid, string fieldName, object oldValue, object newValue)
        {
            int maxValueLength = 15;
            string oldVal = oldValue?.ToString() ?? "";
            string newVal = newValue?.ToString() ?? "";

            if (oldValue is DateTime)
            {
                DateTime oldDate = (DateTime)oldValue;

                if (oldDate == DateTime.MinValue || oldDate == DateTime.MaxValue)
                {
                    oldVal = "";
                }
                else
                {
                    oldVal = ((DateTime)oldValue).ToString("MM/dd/yyyy");
                }
            }

            if (newValue is DateTime)
            {
                DateTime newDate = (DateTime)newValue;

                if (newDate == DateTime.MinValue || newDate == DateTime.MaxValue)
                {
                    newVal = "";
                }
                else
                {
                    newVal = ((DateTime)newValue).ToString("MM/dd/yyyy");
                }
            }

            oldVal = oldVal?.Substring(0, oldVal.Length < maxValueLength ? oldVal.Length : maxValueLength);
            newVal = newVal?.Substring(0, newVal.Length < maxValueLength ? newVal.Length : maxValueLength);

            if (!oldVal.Equals(newVal))
            {
                EligibilityRepository.AddLogDetail(adsConnectionString, logSysid, "APSLOG", fieldName, oldVal ?? "", newVal ?? "");
            }
        }

        private string saveSuspension(string adsConnectionString, SuspensionDTO suspensionToSave)
        {
            return EligibilityRepository.SaveSuspension(adsConnectionString, suspensionToSave.TABLENAME, suspensionToSave.LINKKEY, suspensionToSave.CUTOFFDATE,
                                             suspensionToSave.CUTOFFMETH, suspensionToSave.JOURNAL, suspensionToSave.SYSID);
        }

        private string saveMember(string adsConnectionString, MemberDTO memberToSave)
        {
            return EligibilityRepository.SaveMember(adsConnectionString, memberToSave.ENRID, memberToSave.PLNID, memberToSave.SUBID, memberToSave.CARDID,
                                  memberToSave.PERSON, memberToSave.EFFDT, memberToSave.TRMDT, memberToSave.FLEX1,
                                  memberToSave.FLEX2, memberToSave.RELCD, memberToSave.ELGOVER, memberToSave.FNAME,
                                  memberToSave.MNAME, memberToSave.LNAME, memberToSave.ADDR, memberToSave.ADDR2,
                                  memberToSave.CITY, memberToSave.STATE, memberToSave.ZIP, memberToSave.ZIP4, memberToSave.DOB,
                                  memberToSave.SEX, memberToSave.ELGCD, memberToSave.EMPCD, memberToSave.CRDDT,
                                  memberToSave.SYSID, memberToSave.LSTDTCARD, memberToSave.NOUPDATE, memberToSave.NDCUPDATE,
                                  memberToSave.LASTUPDT, memberToSave.MBRSINCE, memberToSave.PHYID, memberToSave.OLDPERSON,
                                  memberToSave.FLEX3, memberToSave.OTHERID, memberToSave.DEPCODE, memberToSave.MAINT,
                                  memberToSave.ACCUM, memberToSave.PATSTAT, memberToSave.ENRCOPAYM, memberToSave.ENRCOPAYR,
                                  memberToSave.PHYSREQ, memberToSave.USEELM, memberToSave.ACCMETH, memberToSave.CARDID2,
                                  memberToSave.COB, memberToSave.JOURNAL, memberToSave.ADDEDBY, memberToSave.PMGID,
                                  memberToSave.PHONE, memberToSave.MEDICARE, memberToSave.PPNREQENR, memberToSave.PPNID,
                                  memberToSave.HICN, memberToSave.RXBIN, memberToSave.RXPCN, memberToSave.RXGROUP,
                                  memberToSave.RXID, memberToSave.TRELIG, memberToSave.PHYQUAL, memberToSave.MMEDAYMAX, memberToSave.ALLOWGOVT);
        }

        private string saveAccumulator(string adsConnectionString, BenefitYearAccumulatorDTO accumulatorToSave)
        {
            return EligibilityRepository.SaveAccumulator(adsConnectionString, accumulatorToSave.PLNID, accumulatorToSave.ENRID, accumulatorToSave.SUBID, accumulatorToSave.ENRAMT,
                                          accumulatorToSave.YTDRX, accumulatorToSave.YTDDOLLAR, accumulatorToSave.PLANTYPE, accumulatorToSave.EFFDT,
                                          accumulatorToSave.TRMDT, accumulatorToSave.BROKERYTD, accumulatorToSave.SMOKINGYTD, accumulatorToSave.SMOKINGLT,
                                          accumulatorToSave.COPAY, accumulatorToSave.PRODSEL, accumulatorToSave.DEDUCT, accumulatorToSave.DEDMETDT,
                                          accumulatorToSave.EXCEEDMAX, accumulatorToSave.MAXMETDT, accumulatorToSave.OOPMETDT, accumulatorToSave.LIFEMAX,
                                          accumulatorToSave.FERYTDMAX, accumulatorToSave.FERLTMAX, accumulatorToSave.OCYTD, accumulatorToSave.OCLIFE,
                                          accumulatorToSave.ICYTD, accumulatorToSave.ICLIFE, accumulatorToSave.JOURNAL, accumulatorToSave.SYSID, accumulatorToSave.TIER,
                                          accumulatorToSave.NPDEDACC, accumulatorToSave.NPOOPACC, accumulatorToSave.NPMAXACC, accumulatorToSave.QTR4DEDACC,
                                          accumulatorToSave.QTR4OOPACC, accumulatorToSave.QTR4MAXACC, accumulatorToSave.MEDDEDACC, accumulatorToSave.MEDOOPACC,
                                          accumulatorToSave.MEDMAXACC, accumulatorToSave.DIAPHRDT, accumulatorToSave.NPMEDMAX, accumulatorToSave.NPMEDOOP,
                                          accumulatorToSave.NPMEDDED, accumulatorToSave.LASTCLAIM, accumulatorToSave.OTHERID, accumulatorToSave.GHYTDMAX,
                                          accumulatorToSave.GHLTMAX, accumulatorToSave.CPYSUBS, accumulatorToSave.DEDSUBS, accumulatorToSave.TROOP,
                                          accumulatorToSave.TOTPRC, accumulatorToSave.GAP_TROOP, accumulatorToSave.ENRADJ, accumulatorToSave.CLASS,
                                          accumulatorToSave.BYATROOP, accumulatorToSave.PARTBDED, accumulatorToSave.PARTBOOP, accumulatorToSave.PARTBMAX,
                                          accumulatorToSave.BDEDMETDT, accumulatorToSave.BOOPMETDT, accumulatorToSave.BMAXMETDT, accumulatorToSave.TROOPIC,
                                          accumulatorToSave.TOTPRCIC, accumulatorToSave.CPYSUBSIC, accumulatorToSave.GAPCPYSUB, accumulatorToSave.GAPTOTPRC,
                                          accumulatorToSave.TEQFEE, accumulatorToSave.SPECDED, accumulatorToSave.SPECOOP, accumulatorToSave.SPECDOLLAR);
        }

        private void logSuspension(string adsConnectionString, SuspensionDTO suspensionToLog)
        {
            EligibilityRepository.LogSuspension(adsConnectionString, suspensionToLog.TABLENAME, suspensionToLog.LINKKEY, suspensionToLog.CUTOFFDATE,
                                            suspensionToLog.CUTOFFMETH, suspensionToLog.JOURNAL, suspensionToLog.SYSID, suspensionToLog.USERNAME);
        }

        private string logMember(string adsConnectionString, MemberDTO memberToLog)
        {
            return EligibilityRepository.LogMember(adsConnectionString, memberToLog.ENRID, memberToLog.PLNID, memberToLog.SUBID, memberToLog.CARDID,
                                 memberToLog.PERSON, memberToLog.EFFDT, memberToLog.TRMDT, memberToLog.FLEX1,
                                 memberToLog.FLEX2, memberToLog.RELCD, memberToLog.ELGOVER, memberToLog.FNAME,
                                 memberToLog.MNAME, memberToLog.LNAME, memberToLog.ADDR, memberToLog.ADDR2,
                                 memberToLog.CITY, memberToLog.STATE, memberToLog.ZIP, memberToLog.ZIP4, memberToLog.DOB,
                                 memberToLog.SEX, memberToLog.ELGCD, memberToLog.EMPCD, memberToLog.CRDDT,
                                 memberToLog.SYSID, memberToLog.LSTDTCARD, memberToLog.NOUPDATE, memberToLog.NDCUPDATE,
                                 memberToLog.LASTUPDT, memberToLog.MBRSINCE, memberToLog.PHYID, memberToLog.OLDPERSON,
                                 memberToLog.FLEX3, memberToLog.OTHERID, memberToLog.DEPCODE, memberToLog.MAINT,
                                 memberToLog.ACCUM, memberToLog.PATSTAT, memberToLog.ENRCOPAYM, memberToLog.ENRCOPAYR,
                                 memberToLog.PHYSREQ, memberToLog.USEELM, memberToLog.ACCMETH, memberToLog.CARDID2,
                                 memberToLog.COB, memberToLog.JOURNAL, memberToLog.ADDEDBY, memberToLog.PMGID,
                                 memberToLog.PHONE, memberToLog.MEDICARE, memberToLog.PPNREQENR, memberToLog.PPNID,
                                 memberToLog.HICN, memberToLog.RXBIN, memberToLog.RXPCN, memberToLog.RXGROUP,
                                 memberToLog.RXID, memberToLog.TRELIG, memberToLog.PHYQUAL, memberToLog.MMEDAYMAX, memberToLog.ALLOWGOVT, memberToLog.USERNAME);
        }

        private string logAccumulator(string adsConnectionString, BenefitYearAccumulatorDTO accumulatorToLog)
        {
            return EligibilityRepository.LogAccumulator(adsConnectionString, accumulatorToLog.PLNID, accumulatorToLog.ENRID, accumulatorToLog.SUBID, accumulatorToLog.ENRAMT,
                                         accumulatorToLog.YTDRX, accumulatorToLog.YTDDOLLAR, accumulatorToLog.PLANTYPE, accumulatorToLog.EFFDT, accumulatorToLog.TRMDT,
                                         accumulatorToLog.BROKERYTD, accumulatorToLog.SMOKINGYTD, accumulatorToLog.SMOKINGLT, accumulatorToLog.COPAY,
                                         accumulatorToLog.PRODSEL, accumulatorToLog.DEDUCT, accumulatorToLog.DEDMETDT, accumulatorToLog.EXCEEDMAX,
                                         accumulatorToLog.MAXMETDT, accumulatorToLog.OOPMETDT, accumulatorToLog.LIFEMAX, accumulatorToLog.FERYTDMAX,
                                         accumulatorToLog.FERLTMAX, accumulatorToLog.OCYTD, accumulatorToLog.OCLIFE, accumulatorToLog.ICYTD, accumulatorToLog.ICLIFE,
                                         accumulatorToLog.JOURNAL, accumulatorToLog.SYSID, accumulatorToLog.TIER, accumulatorToLog.NPDEDACC, accumulatorToLog.NPOOPACC,
                                         accumulatorToLog.NPMAXACC, accumulatorToLog.QTR4DEDACC, accumulatorToLog.QTR4OOPACC, accumulatorToLog.QTR4MAXACC,
                                         accumulatorToLog.MEDDEDACC, accumulatorToLog.MEDOOPACC, accumulatorToLog.MEDMAXACC, accumulatorToLog.DIAPHRDT,
                                         accumulatorToLog.NPMEDMAX, accumulatorToLog.NPMEDOOP, accumulatorToLog.NPMEDDED, accumulatorToLog.LASTCLAIM,
                                         accumulatorToLog.OTHERID, accumulatorToLog.GHYTDMAX, accumulatorToLog.GHLTMAX, accumulatorToLog.CPYSUBS,
                                         accumulatorToLog.DEDSUBS, accumulatorToLog.TROOP, accumulatorToLog.TOTPRC, accumulatorToLog.GAP_TROOP, accumulatorToLog.ENRADJ,
                                         accumulatorToLog.CLASS, accumulatorToLog.BYATROOP, accumulatorToLog.PARTBDED, accumulatorToLog.PARTBOOP,
                                         accumulatorToLog.PARTBMAX, accumulatorToLog.BDEDMETDT, accumulatorToLog.BOOPMETDT, accumulatorToLog.BMAXMETDT,
                                         accumulatorToLog.TROOPIC, accumulatorToLog.TOTPRCIC, accumulatorToLog.CPYSUBSIC, accumulatorToLog.GAPCPYSUB,
                                         accumulatorToLog.GAPTOTPRC, accumulatorToLog.TEQFEE, accumulatorToLog.SPECDED, accumulatorToLog.SPECOOP,
                                         accumulatorToLog.SPECDOLLAR, accumulatorToLog.USERNAME);
        }

        private string saveHistoryMember(string adsConnectionString, MemberDTO oldMember)
        {
            return EligibilityRepository.SaveHistoryMember(adsConnectionString, oldMember.ENRID, oldMember.PLNID, oldMember.SUBID, oldMember.CARDID,
                                            oldMember.PERSON, oldMember.EFFDT, oldMember.TRMDT, oldMember.FLEX1,
                                            oldMember.FLEX2, oldMember.RELCD, oldMember.ELGOVER, oldMember.FNAME,
                                            oldMember.MNAME, oldMember.LNAME, oldMember.ADDR, oldMember.ADDR2,
                                            oldMember.CITY, oldMember.STATE, oldMember.ZIP, oldMember.ZIP4, oldMember.DOB,
                                            oldMember.SEX, oldMember.ELGCD, oldMember.EMPCD, oldMember.CRDDT,
                                            oldMember.SYSID, oldMember.LSTDTCARD, oldMember.NOUPDATE, oldMember.NDCUPDATE,
                                            oldMember.LASTUPDT, oldMember.MBRSINCE, oldMember.PHYID, oldMember.OLDPERSON,
                                            oldMember.FLEX3, oldMember.OTHERID, oldMember.DEPCODE, oldMember.MAINT,
                                            oldMember.ACCUM, oldMember.PATSTAT, oldMember.ENRCOPAYM, oldMember.ENRCOPAYR,
                                            oldMember.PHYSREQ, oldMember.USEELM, oldMember.ACCMETH, oldMember.CARDID2,
                                            oldMember.COB, oldMember.JOURNAL, oldMember.ADDEDBY, oldMember.PMGID,
                                            oldMember.PHONE, oldMember.MEDICARE, oldMember.PPNREQENR, oldMember.PPNID,
                                            oldMember.HICN, oldMember.RXBIN, oldMember.RXPCN, oldMember.RXGROUP,
                                            oldMember.RXID, oldMember.TRELIG, oldMember.PHYQUAL, oldMember.MMEDAYMAX, oldMember.ALLOWGOVT, oldMember.USERNAME);
        }

        private string saveHistoryAccumulator(string adsConnectionString, BenefitYearAccumulatorDTO oldAccumulator)
        {
            return EligibilityRepository.SaveHistoryAccumulator(adsConnectionString, oldAccumulator.PLNID, oldAccumulator.ENRID, oldAccumulator.SUBID, oldAccumulator.ENRAMT,
                                         oldAccumulator.YTDRX, oldAccumulator.YTDDOLLAR, oldAccumulator.PLANTYPE, oldAccumulator.EFFDT, oldAccumulator.TRMDT,
                                         oldAccumulator.BROKERYTD, oldAccumulator.SMOKINGYTD, oldAccumulator.SMOKINGLT, oldAccumulator.COPAY,
                                         oldAccumulator.PRODSEL, oldAccumulator.DEDUCT, oldAccumulator.DEDMETDT, oldAccumulator.EXCEEDMAX,
                                         oldAccumulator.MAXMETDT, oldAccumulator.OOPMETDT, oldAccumulator.LIFEMAX, oldAccumulator.FERYTDMAX,
                                         oldAccumulator.FERLTMAX, oldAccumulator.OCYTD, oldAccumulator.OCLIFE, oldAccumulator.ICYTD, oldAccumulator.ICLIFE,
                                         oldAccumulator.JOURNAL, oldAccumulator.SYSID, oldAccumulator.TIER, oldAccumulator.NPDEDACC, oldAccumulator.NPOOPACC,
                                         oldAccumulator.NPMAXACC, oldAccumulator.QTR4DEDACC, oldAccumulator.QTR4OOPACC, oldAccumulator.QTR4MAXACC,
                                         oldAccumulator.MEDDEDACC, oldAccumulator.MEDOOPACC, oldAccumulator.MEDMAXACC, oldAccumulator.DIAPHRDT,
                                         oldAccumulator.NPMEDMAX, oldAccumulator.NPMEDOOP, oldAccumulator.NPMEDDED, oldAccumulator.LASTCLAIM,
                                         oldAccumulator.OTHERID, oldAccumulator.GHYTDMAX, oldAccumulator.GHLTMAX, oldAccumulator.CPYSUBS,
                                         oldAccumulator.DEDSUBS, oldAccumulator.TROOP, oldAccumulator.TOTPRC, oldAccumulator.GAP_TROOP, oldAccumulator.ENRADJ,
                                         oldAccumulator.CLASS, oldAccumulator.BYATROOP, oldAccumulator.PARTBDED, oldAccumulator.PARTBOOP,
                                         oldAccumulator.PARTBMAX, oldAccumulator.BDEDMETDT, oldAccumulator.BOOPMETDT, oldAccumulator.BMAXMETDT,
                                         oldAccumulator.TROOPIC, oldAccumulator.TOTPRCIC, oldAccumulator.CPYSUBSIC, oldAccumulator.GAPCPYSUB,
                                         oldAccumulator.GAPTOTPRC, oldAccumulator.TEQFEE, oldAccumulator.SPECDED, oldAccumulator.SPECOOP,
                                         oldAccumulator.SPECDOLLAR, oldAccumulator.USERNAME);
        }

        private string getFullErrorMessage(List<string> messages, bool formatForACP = false)
        {
            StringBuilder errorMessages = new StringBuilder();

            foreach (string message in messages)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    if (formatForACP)
                    {
                        errorMessages.AppendFormat("{0}|", message);
                    }
                    else
                    {
                        errorMessages.AppendLine(message);
                    }
                }
            }

            return errorMessages.ToString();
        }

        private string getCardID(string fullCardID)
        {
            return fullCardID.Substring(0, 9);
        }

        private string getCardID2(string fullCardID)
        {
            return fullCardID.Substring(9);
        }

        private bool isValidPlanID(string adsConnectionString, string planID, out string errorMessage, bool useACPMessage = false)
        {
            errorMessage = "";
            bool valid = true;

            if (!EligibilityRepository.PlanExists(adsConnectionString, planID))
            {
                if (useACPMessage)
                {
                    errorMessage = "Invalid Plan ID";
                }
                else
                {
                    errorMessage = "M/I Plan ID";
                }

                valid = false;
            }

            return valid;
        }

        private bool isValidPharmacy(string adsConnectionString, string pharmacyID, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (!PharmacyRepository.PharmacyExists(adsConnectionString, pharmacyID).Result)
            {
                errorMessage = "Invalid Pharmacy";

                valid = false;
            }

            return valid;
        }

        private bool isValidMember(string adsConnectionString, string planId, string enrolleeId, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (!Repository.MemberExists(adsConnectionString, planId, enrolleeId).Result)
            {
                errorMessage = $"Member {enrolleeId} not found in Plan {planId}";

                valid = false;
            }

            return valid;
        }

        private bool isPlanInDate(string adsConnectionString, string planID, DateTime adjustmentDate, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (!EligibilityRepository.PlanInDate(adsConnectionString, planID, adjustmentDate))
            {
                errorMessage = string.Format("There was no plan effective as of {0}", adjustmentDate.ToShortDateString());

                valid = false;
            }

            return valid;
        }

        private bool isValidCardID(string adsConnectionString, string planID, string cardID, string person, out string errorMessage, bool useACPMessage = false)
        {
            errorMessage = "";
            bool valid = true;

            if (!(EligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(adsConnectionString, planID, getCardID(cardID), getCardID2(cardID), person).Count > 0))
            {
                if (useACPMessage)
                {
                    errorMessage = "PlnId+CardId(1&2)+Person, Not In ApsEnr";
                }
                else
                {
                    errorMessage = "M/I Card ID";
                }

                valid = false;
            }

            return valid;
        }

        private bool isValidACPAmount(double medicalDeductible, double medicalMaximumBenefitAmount, double medicalOutOfPocketAmount, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (medicalDeductible < 0 || medicalMaximumBenefitAmount < 0 || medicalOutOfPocketAmount < 0)
            {
                errorMessage = "Negative amounts are not allowed";
                valid = false;
            }

            return valid;
        }


        #endregion

        #region ACP

        public void populateAccumulatorDates(string connectionString, string enrId, ref DateTime dBegin, ref DateTime dEnd, string plnId,
                                             DateTime dHstDate, DateTime memberSince)
        {
            // Get Plan fields record.
            ACPPlanFieldsDTO planFields = EligibilityRepository.GetACPPlanFields(connectionString, plnId);

            if (planFields.AccumulatorPeriod == 0 && String.IsNullOrWhiteSpace(planFields.Oxford) && planFields.AnnDate == DateTime.MinValue)
            {
                // Default to 1 year.  will reject further downstream
                dBegin = DateTime.Parse("01/01/" + DateTime.Now.Year);
                dEnd = DateTime.Parse("12/31/" + DateTime.Now.Year);
                return;
            }

            DateTime dStartDate = DateTime.Parse(planFields.AnnDate.Month + "/" + planFields.AnnDate.Day + "/" + DateTime.Now.Year);

            if (planFields.AccumulatorPeriod == 0)
            {
                planFields.AccumulatorPeriod = 12;
            }

            if (planFields.Oxford.Equals("E"))
            {
                dStartDate = memberSince;
            }
            else
            {
                DateTime workDate = dStartDate;

                if (planFields.AccumulatorPeriod == 12)
                    dStartDate = planFields.AnnDate;
                else
                {
                    int i = 0;
                    for (i = 0; i < 12; i += planFields.AccumulatorPeriod)
                    {
                        workDate = workDate.AddMonths(planFields.AccumulatorPeriod);
                        if (workDate > DateTime.Now)
                            break;
                    }
                    workDate = workDate.AddMonths(-planFields.AccumulatorPeriod);
                    dStartDate = workDate;
                }
            }

            dBegin = DateTime.Parse(dStartDate.Month + "/" + dStartDate.Day + "/" + dHstDate.Year);

            // Starting date cannot be greater than the adjustment date
            if (dBegin >= dHstDate)
            {
                for (int i = 0; i < 12; i++)
                {
                    dBegin = dBegin.AddMonths(-planFields.AccumulatorPeriod);
                    if (dBegin <= dHstDate)
                        break;
                }
            }
            dEnd = dBegin.AddMonths(planFields.AccumulatorPeriod);
        }
        #endregion ACP

        #region Search Helpers

        private string convertToDateString(DateTime? value, bool emptyStringIfNull = true)
        {
            string output = null;

            if (value.HasValue)
            {
                output = value.Value.ToString("yyyyMMdd");
            }
            else if (emptyStringIfNull)
            {
                output = "";
            }

            return output;
        }

        private string convertToDateTimeString(DateTime? value, bool emptyStringIfNull = true)
        {
            string output = null;

            if (value.HasValue)
            {
                output = value.Value.ToString("MM/dd/yyyy HH:mm:ss");
            }
            else if (emptyStringIfNull)
            {
                output = "";
            }

            return output;
        }

        private SearchTypeOperator? getSearchType(string fieldValue, string operatorValue)
        {
            SearchTypeOperator? searchOperator = null;

            if (!string.IsNullOrWhiteSpace(fieldValue))
            {
                if (!string.IsNullOrWhiteSpace(operatorValue))
                {
                    searchOperator = (SearchTypeOperator)operatorValue.ToUpper()[0];
                }
                else
                {
                    searchOperator = SearchTypeOperator.StartsWith;
                }
            }

            return searchOperator;
        }

        private string getSearchTypeString(SearchTypeOperator? searchTypeOperator)
        {
            return searchTypeOperator != null ? ((char)searchTypeOperator).ToString() : null;
        }

        private static PagingOptionsResponse SetPagingOptions(int resultsCount, int? itemsPerPage, int? pageNumber, int totalNumberOfRecords)
        {
            PagingOptionsResponse pagingOption = new PagingOptionsResponse();
            if (resultsCount > 0)
            {
                pagingOption.PageNumber = pageNumber;
                pagingOption.ItemsPerPage = itemsPerPage;
                pagingOption.TotalNumberOfPages = totalNumberOfRecords / itemsPerPage;
                if (totalNumberOfRecords % itemsPerPage > 0)
                {
                    pagingOption.TotalNumberOfPages++;
                }
                pagingOption.TotalNumberOfRecords = totalNumberOfRecords;
            }
            else
            {
                pagingOption.PageNumber = 1;
                pagingOption.ItemsPerPage = 0;
                pagingOption.TotalNumberOfPages = 1;
                pagingOption.TotalNumberOfRecords = 0;
            }
            return pagingOption;
        }

        #endregion Search Helpers

        #region Member

        private MemberSearchResult convertToMemberSearchResult(MemberSearchDTO x)
        {
            MemberSearchResult result = x.ConvertTo<MemberSearchResult>();
            result.PlanEffectiveDate = convertToDateString(x.PlanEffectiveDate);
            result.DateOfBirth = convertToDateString(x.DateOfBirth);
            result.MemberEffectiveDate = convertToDateString(x.MemberEffectiveDate);
            result.MemberTerminationDate = convertToDateString(x.MemberTerminationDate);

            result.GroupEligibility = new GroupEligibility();
            result.GroupEligibility.GroupTerminationDate = convertToDateString(x.GroupEligibility.GroupTerminationDate);
            result.GroupEligibility.GroupEffectiveDate = convertToDateString(x.GroupEligibility.GroupEffectiveDate);
            result.GroupEligibility.PlanEffectiveDate = convertToDateString(x.GroupEligibility.PlanEffectiveDate);
            result.GroupEligibility.Active = x.GroupEligibility.Active;
            result.GroupEligibility.PlanId = x.GroupEligibility.PlanId;
            result.GroupEligibility.Flex1 = x.GroupEligibility.Flex1;

            //only include if populated
            if (!string.IsNullOrWhiteSpace(x.AdditionalEligibility.Flex2) || !string.IsNullOrWhiteSpace(x.AdditionalEligibility.Flex3))
            {
                result.AdditionalEligibility = new AdditionalEligibility();
                result.AdditionalEligibility.Flex2 = !string.IsNullOrWhiteSpace(x.AdditionalEligibility.Flex2) ? x.AdditionalEligibility.Flex2 : null;
                result.AdditionalEligibility.Flex3 = !string.IsNullOrWhiteSpace(x.AdditionalEligibility.Flex3) ? x.AdditionalEligibility.Flex3 : null;
            }
            else
            {
                result.AdditionalEligibility = null;
            }

            return result;
        }

        private MemberDetailsResult convertToMemberDetailsResult(MemberDetailsResultDTO x)
        {
            MemberDetailsResult result = x.ConvertTo<MemberDetailsResult>();

            x.HealthProfile.ForEach(y =>
            {
                MemberDetailsMemberDiagnosis convertedDiagnosis = y.ConvertTo<MemberDetailsMemberDiagnosis>();
                result.HealthProfile.Add(convertedDiagnosis);
            });

            result.MemberDetail.DateOfBirth = convertToDateString(x.MemberDetail.DateOfBirth);
            result.MemberDetail.OriginalFromDate = convertToDateString(x.MemberDetail.OriginalFromDate);

            result.AlternateInsurance.MedicareFromDate = convertToDateString(x.AlternateInsurance.MedicareFromDate);

            result.MemberCoverage.Status = x.MemberCoverage.Status ? "A" : "I";
            result.MemberCoverage.EffectiveDate = convertToDateString(x.MemberCoverage.EffectiveDate);
            result.MemberCoverage.TerminationDate = convertToDateString(x.MemberCoverage.TerminationDate);

            result.IDCard.CardDate = convertToDateString(x.IDCard.CardDate);

            result.PlanInfo.EffectiveDate = convertToDateString(x.PlanInfo.EffectiveDate);
            result.PlanInfo.TerminationDate = convertToDateString(x.PlanInfo.TerminationDate);

            result.MedicarePartDItems.FromDate = convertToDateString(x.MedicarePartDItems.FromDate);
            result.MedicarePartDItems.ToDate = convertToDateString(x.MedicarePartDItems.ToDate);
            result.MedicarePartDItems.CopayCategoryEffectiveDate = convertToDateString(x.MedicarePartDItems.CopayCategoryEffectiveDate);

            return result;
        }

        public MemberPhysicianLockPhysician convertToMemberPhysicianLockPhysician(MemberPhysicianLockDetailsResultDTO x)
        {
            MemberPhysicianLockPhysician result = x.ConvertTo<MemberPhysicianLockPhysician>();
            result.PhysicianDEA = x.PHYDEA;
            result.PhysicianNPI = x.PHYNPI;

            return result;
        }

        private async Task<Tuple<List<MemberSearchDTO>, int>> memberSearchClientDatasets(List<DatasetDTO> datasets, int concurrentThreadCount, string memberId,
                                                                             SearchTypeOperator? memberIdOperator, string lastName,
                                                                             SearchTypeOperator? lastNameOperator, string firstName,
                                                                             SearchTypeOperator? firstNameOperator, DateTime? dateOfBirth)
        {
            List<Task<Tuple<List<MemberSearchDTO>, int>>> taskList = new List<Task<Tuple<List<MemberSearchDTO>, int>>>();
            List<Tuple<List<MemberSearchDTO>, int>> finalStateTaskList = new List<Tuple<List<MemberSearchDTO>, int>>();

            int threadCounter = 1;

            for (int i = 0; i < datasets.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<Tuple<List<MemberSearchDTO>, int>> t = Repository.GetMemberSearchResults(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path),
                                                                                      datasets[i].Name, datasets[i].MemberIdType, memberId,
                                                                                      getSearchTypeString(memberIdOperator), lastName,
                                                                                      getSearchTypeString(lastNameOperator), firstName,
                                                                                      getSearchTypeString(firstNameOperator), dateOfBirth,
                                                                         datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<Tuple<List<MemberSearchDTO>, int>> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            Tuple<List<MemberSearchDTO>, int> taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.Add(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            Tuple<List<MemberSearchDTO>, int> finalResults = new Tuple<List<MemberSearchDTO>, int>(finalStateTaskList.SelectMany(x => x.Item1).ToList(), finalStateTaskList.Sum(x => x.Item2));

            return finalResults;
        }

        private async Task<int> memberSearchGetCounts(List<DatasetDTO> datasets, int concurrentThreadCount, string memberId,
                                                      SearchTypeOperator? memberIdOperator, string lastName,
                                                      SearchTypeOperator? lastNameOperator, string firstName,
                                                      SearchTypeOperator? firstNameOperator, DateTime? dateOfBirth)
        {
            List<Task<int>> taskList = new List<Task<int>>();
            List<int> finalStateTaskList = new List<int>();

            int threadCounter = 1;

            for (int i = 0; i < datasets.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<int> t = Repository.GetMemberSearchResultsCount(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path), datasets[i].Name,
                                                                         datasets[i].MemberIdType, memberId, getSearchTypeString(memberIdOperator),
                                                                         lastName, getSearchTypeString(lastNameOperator), firstName,
                                                                         getSearchTypeString(firstNameOperator), dateOfBirth,
                                                                         datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<int> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchGetCounts").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            int taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.Add(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return finalStateTaskList.Sum();
        }

        private async Task<List<MemberDetailsResultDTO>> getMemberDetails(List<DatasetDTO> datasets, int concurrentThreadCount, string organizationId,
                                                                   string groupId, string planId, string memberId, string person)
        {
            List<Task<List<MemberDetailsResultDTO>>> taskList = new List<Task<List<MemberDetailsResultDTO>>>();
            List<MemberDetailsResultDTO> finalStateTaskList = new List<MemberDetailsResultDTO>();

            int threadCounter = 1;

            for (int i = 0; i < datasets.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<List<MemberDetailsResultDTO>> t = Repository.GetMemberDetails(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path),
                                                                                      datasets[i].Name, datasets[i].MemberIdType, organizationId,
                                                                                      groupId, planId, memberId, person);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<List<MemberDetailsResultDTO>> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            List<MemberDetailsResultDTO> taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.AddRange(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return finalStateTaskList;
        }

        private async Task<List<MemberDetailsResultDTO>> getMemberDetailDiagnoses(List<DatasetDTO> datasets, int concurrentThreadCount, List<MemberDetailsResultDTO> memberDetails)
        {
            List<Task<List<MemberDetailsMemberDiagnosisDTO>>> taskList = new List<Task<List<MemberDetailsMemberDiagnosisDTO>>>();
            List<MemberDetailsMemberDiagnosisDTO> finalStateTaskList = new List<MemberDetailsMemberDiagnosisDTO>();

            int threadCounter = 1;

            for (int i = 0; i < memberDetails.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    DatasetDTO dataset = datasets.FirstOrDefault(x => x.Name.EqualsIgnoreCase(memberDetails[i].Client));

                    Task<List<MemberDetailsMemberDiagnosisDTO>> t = Repository.GetMemberDiagnoses(MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path),
                                                                                      dataset.Name, memberDetails[i].MemberDetail.MemberEnrolleeID);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<List<MemberDetailsMemberDiagnosisDTO>> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            List<MemberDetailsMemberDiagnosisDTO> taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.AddRange(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            var diagnoses = finalStateTaskList.GroupBy(x => new { x.Client, x.MemberEnrolleeID }).ToList();

            memberDetails.ForEach(x =>
            {
                int index = diagnoses.FindIndex(y => y.Key.Client == x.Client && y.Key.MemberEnrolleeID == x.MemberDetail.MemberEnrolleeID);
                if (index > -1)
                {
                    x.HealthProfile = diagnoses[index].ToList();
                }

            });

            return memberDetails;
        }

        private async Task<List<MemberPhysicianLockDetailsResultDTO>> getMemberPhysicianLockDetails_ByMember(List<DatasetEnrolleeIDDTO> datasetEnrolleeIds, int concurrentThreadCount, string planId,
                                                                             string memberId, string person)
        {
            List<Task<List<MemberPhysicianLockDetailsResultDTO>>> taskList = new List<Task<List<MemberPhysicianLockDetailsResultDTO>>>();
            List<List<MemberPhysicianLockDetailsResultDTO>> finalStateTaskList = new List<List<MemberPhysicianLockDetailsResultDTO>>();

            int threadCounter = 1;

            for (int i = 0; i < datasetEnrolleeIds.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    DatasetEnrolleeIDDTO datasetEnrolleeIdPair = datasetEnrolleeIds[i];
                    DatasetDTO dataset = datasetEnrolleeIdPair.Dataset;
                    string enrolleeId = datasetEnrolleeIdPair.EnrolleeID;
                    Task<List<MemberPhysicianLockDetailsResultDTO>> t = Repository.GetMemberPhysicianLockDetails_ByMember(MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path),
                                                                                      dataset.Name, planId, enrolleeId, person,
                                                                                      dataset.ParentIDs, dataset.OrganizationIDs, dataset.GroupIDs, dataset.PlanIDs);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasetEnrolleeIds.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<List<MemberPhysicianLockDetailsResultDTO>> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            List<MemberPhysicianLockDetailsResultDTO> taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.Add(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            List<MemberPhysicianLockDetailsResultDTO> finalResults = finalStateTaskList.SelectMany(x => x.Select(y => y)).ToList();

            return finalResults;
        }
        #endregion Member

        #region Claim

        #region Paid Claim Search
        private static void GetClaimSearchConfigurations(List<Configuration> configurations, string clientGuid, out List<DatasetDTO> datasets, out int maxRecords, out int maxSearchThreads)
        {

            var clientSettings = configurations.First(x => x.ConfigurationKey == clientGuid).ConfigurationValues;
            datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSettings["Datasets"]);

            //Get maximum number of records to return in the search results from configurations
            var claimSearchSettings = configurations.First(x => x.ConfigurationKey == ConfigSetttingKey.PBMAPI_ClaimSearchConfig).ConfigurationValues;

            maxRecords = Convert.ToInt32(claimSearchSettings[ConfigSetttingKey.ApiMaxNumberOfRecordsSetting_KeyValue]);
            maxSearchThreads = Convert.ToInt32(claimSearchSettings[ConfigSetttingKey.MaxSearchThreads]);
        }

        private async Task<Tuple<List<ClaimSearchDTO>, int>> DailyClaimSearchClientDatasets(List<DatasetDTO> datasets, int concurrentThreadCount,
                                                                           string memberId, SearchTypeOperator? memberIdOperator,
                                                                           DateTime fillDateFrom, DateTime fillDateTo)
        {

            List<Task<Tuple<List<ClaimSearchDTO>, int>>> taskList = new List<Task<Tuple<List<ClaimSearchDTO>, int>>>();
            List<Tuple<List<ClaimSearchDTO>, int>> finalStateTaskList = new List<Tuple<List<ClaimSearchDTO>, int>>();

            int threadCounter = 0;

            for (int i = 0; i < datasets.Count; i++)
            {
                string adsConnectionString = MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path);
                string searchTypeString = getSearchTypeString(memberIdOperator);

                Task<Tuple<List<ClaimSearchDTO>, int>> t1 = Repository.GetDailyClaimSearchResults(adsConnectionString, datasets[i].Name, datasets[i].MemberIdType,
                                                                                        memberId, searchTypeString, fillDateFrom, fillDateTo, datasets[i].ShowClaimsForAllMemberPlans,
                                                                         datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
                taskList.Add(t1);
                threadCounter++;
                if (threadCounter == concurrentThreadCount)
                {
                    await Task.WhenAll(taskList);
                    await FinalStateTaskList(taskList, finalStateTaskList).ConfigureAwait(false);
                    threadCounter = 0;
                    taskList.Clear();
                }

                if (i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);
                    await FinalStateTaskList(taskList, finalStateTaskList).ConfigureAwait(false);
                    threadCounter = 0;
                    taskList.Clear();
                }
            }

            return new Tuple<List<ClaimSearchDTO>, int>(
                finalStateTaskList.SelectMany(x => x.Item1).GroupBy(x => x.ClaimNumber).Select(x => x.OrderBy(y => y.CLAIM_SOURCE).First()).ToList(),
                finalStateTaskList.Sum(x => x.Item2));
        }

        private async Task<Tuple<List<ClaimSearchDTO>, int>> PaidClaimSearchClientDatasets(List<DatasetDTO> datasets, int concurrentThreadCount,
                                                                           string memberId, SearchTypeOperator? memberIdOperator,
                                                                           DateTime fillDateFrom, DateTime fillDateTo)
        {

            List<Task<Tuple<List<ClaimSearchDTO>, int>>> taskList = new List<Task<Tuple<List<ClaimSearchDTO>, int>>>();
            List<Tuple<List<ClaimSearchDTO>, int>> finalStateTaskList = new List<Tuple<List<ClaimSearchDTO>, int>>();

            int threadCounter = 0;

            for (int i = 0; i < datasets.Count; i++)
            {
                string adsConnectionString = MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path);
                string searchTypeString = getSearchTypeString(memberIdOperator);

                Task<Tuple<List<ClaimSearchDTO>, int>> t2 = Repository.GetPaidClaimSearchResults(adsConnectionString, datasets[i].Name, datasets[i].MemberIdType,
                                                                                        memberId, searchTypeString, fillDateFrom, fillDateTo, datasets[i].ShowClaimsForAllMemberPlans,
                                                                         datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
                taskList.Add(t2);
                threadCounter++;
                if (threadCounter == concurrentThreadCount)
                {
                    await Task.WhenAll(taskList);
                    await FinalStateTaskList(taskList, finalStateTaskList).ConfigureAwait(false);
                    threadCounter = 0;
                    taskList.Clear();
                }

                if (i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);
                    await FinalStateTaskList(taskList, finalStateTaskList).ConfigureAwait(false);
                    threadCounter = 0;
                    taskList.Clear();
                }
            }

            return new Tuple<List<ClaimSearchDTO>, int>(
                finalStateTaskList.SelectMany(x => x.Item1).GroupBy(x => x.ClaimNumber).Select(x => x.OrderBy(y => y.CLAIM_SOURCE).First()).ToList(),
                finalStateTaskList.Sum(x => x.Item2));
        }
        private async Task<int> DailyClaimSearchResutlsCount(List<DatasetDTO> datasets, int concurrentThreadCount,
                                                                           string memberId, SearchTypeOperator? memberIdOperator,
                                                                           DateTime fillDateFrom, DateTime fillDateTo)
        {

            List<Task<int>> taskList = new List<Task<int>>();
            List<int> finalStateTaskList = new List<int>();

            int threadCounter = 0;

            for (int i = 0; i < datasets.Count; i++)
            {
                string adsConnectionString = MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path);
                string searchTypeString = getSearchTypeString(memberIdOperator);

                Task<int> t1 = Repository.GetDailyClaimSearchResultsCount(adsConnectionString, datasets[i].Name, datasets[i].MemberIdType,
                                                                                        memberId, searchTypeString, fillDateFrom, fillDateTo, datasets[i].ShowClaimsForAllMemberPlans,
                                                                         datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
                taskList.Add(t1);
                threadCounter++;
                if (threadCounter == concurrentThreadCount)
                {
                    await Task.WhenAll(taskList);
                    await FinalStateTaskList(taskList, finalStateTaskList).ConfigureAwait(false);
                    threadCounter = 0;
                    taskList.Clear();
                }

                if (i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);
                    await FinalStateTaskList(taskList, finalStateTaskList).ConfigureAwait(false);
                    threadCounter = 0;
                    taskList.Clear();
                }
            }

            int totalNumberOfRecords = 0;

            finalStateTaskList.ForEach(x => { totalNumberOfRecords = totalNumberOfRecords + x; });

            return totalNumberOfRecords;
        }

        private async Task<int> PaidClaimSearchResutlsCount(List<DatasetDTO> datasets, int concurrentThreadCount,
                                                                           string memberId, SearchTypeOperator? memberIdOperator,
                                                                           DateTime fillDateFrom, DateTime fillDateTo)
        {

            List<Task<int>> taskList = new List<Task<int>>();
            List<int> finalStateTaskList = new List<int>();

            int threadCounter = 0;

            for (int i = 0; i < datasets.Count; i++)
            {
                string adsConnectionString = MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path);
                string searchTypeString = getSearchTypeString(memberIdOperator);

                Task<int> t2 = Repository.GetPaidClaimSearchResultsCount(adsConnectionString, datasets[i].Name, datasets[i].MemberIdType,
                                                                                        memberId, searchTypeString, fillDateFrom, fillDateTo, datasets[i].ShowClaimsForAllMemberPlans,
                                                                         datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
                taskList.Add(t2);
                threadCounter++;
                if (threadCounter == concurrentThreadCount)
                {
                    await Task.WhenAll(taskList);
                    await FinalStateTaskList(taskList, finalStateTaskList).ConfigureAwait(false);
                    threadCounter = 0;
                    taskList.Clear();
                }

                if (i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);
                    await FinalStateTaskList(taskList, finalStateTaskList).ConfigureAwait(false);
                    threadCounter = 0;
                    taskList.Clear();
                }
            }

            int totalNumberOfRecords = 0;

            finalStateTaskList.ForEach(x => { totalNumberOfRecords = totalNumberOfRecords + x; });

            return totalNumberOfRecords;
        }
        private static async Task FinalStateTaskList(List<Task<Tuple<List<ClaimSearchDTO>, int>>> taskList, List<Tuple<List<ClaimSearchDTO>, int>> finalStateTaskList)
        {
            foreach (Task<Tuple<List<ClaimSearchDTO>, int>> task in taskList)
            {
                if (task.Exception != null)
                {
                    Exception ex = task.Exception;
                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "ClaimSearchClientDatasets").ConfigureAwait(false);
                }
                else
                {
                    //Technically don't have to await since we know it's done, but doing it just to be safe
                    Tuple<List<ClaimSearchDTO>, int> taskResult = await task.ConfigureAwait(false);
                    finalStateTaskList.Add(taskResult);
                }
            }
        }

        private static async Task FinalStateTaskList(List<Task<int>> taskList, List<int> finalStateTaskList)
        {
            foreach (Task<int> task in taskList)
            {
                if (task.Exception != null)
                {
                    Exception ex = task.Exception;
                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "ClaimSearchClientDatasets").ConfigureAwait(false);
                }
                else
                {
                    //Technically don't have to await since we know it's done, but doing it just to be safe
                    int taskResult = await task.ConfigureAwait(false);
                    finalStateTaskList.Add(taskResult);
                }
            }
        }

        private ClaimSearchResult ConvertToClaimSearchResult(ClaimSearchDTO x, string requestMemberId)
        {
            ClaimSearchResult result = x.ConvertTo<ClaimSearchResult>();

            result.ActualDateOfBirth = convertToDateString(x.ActualDateOfBirth) ?? "";
            result.FillDate = convertToDateString(x.FillDate) ?? "";
            result.SubmittedDate = convertToDateString(x.SubmittedDate) ?? "";
            result.RxDate = convertToDateString(x.RxDate) ?? "";
            result.ProductIDQualifier = "01"; //Always return NDC
            result.MemberID = requestMemberId;

            List<ClaimNcpdpRejectCode> rejectCodes = new List<ClaimNcpdpRejectCode>();

            if (!(string.IsNullOrEmpty(x.REJCODE1)))
            {
                rejectCodes.Add(new ClaimNcpdpRejectCode { Code = x.REJCODE1, Description = x.REJCODE1_DESC });
            }
            if (!(string.IsNullOrEmpty(x.REJCODE2)))
            {
                rejectCodes.Add(new ClaimNcpdpRejectCode { Code = x.REJCODE2, Description = x.REJCODE2_DESC });
            }
            if (!(string.IsNullOrEmpty(x.REJCODE3)))
            {
                rejectCodes.Add(new ClaimNcpdpRejectCode { Code = x.REJCODE3, Description = x.REJCODE3_DESC });
            }
            if (!(string.IsNullOrEmpty(x.REJCODE4)))
            {
                rejectCodes.Add(new ClaimNcpdpRejectCode { Code = x.REJCODE4, Description = x.REJCODE4_DESC });
            }
            if (!(string.IsNullOrEmpty(x.REJCODE5)))
            {
                rejectCodes.Add(new ClaimNcpdpRejectCode { Code = x.REJCODE5, Description = x.REJCODE5_DESC });
            }

            result.RejectCodes = rejectCodes;

            return result;
        }

        private List<ClaimSearchResult> ClaimSearchResutlsPaging(PaidClaimSearchRequest request, PaidClaimSearchResponse response,
                                                    List<ClaimSearchDTO> searchResults, int maxRecords, int maxSearchThreads, int totalNumberOfRecords)
        {
            List<ClaimSearchResult> claimSearchResults = new List<ClaimSearchResult>();


            if (request.ItemsPerPage == null)
            {
                request.ItemsPerPage = maxRecords;
            }

            if (request.PageNumber == null)
            {
                request.PageNumber = 1;
            }

            if (request.ItemsPerPage >= totalNumberOfRecords)
            {
                searchResults.ForEach(x =>
                {
                    ClaimSearchResult result = ConvertToClaimSearchResult(x, request.MemberId);
                    claimSearchResults.Add(result);
                });
            }
            else
            {
                searchResults
                            .Skip(request.ItemsPerPage.Value * (request.PageNumber.Value - 1)) //Skip previous pages
                            .Take(request.ItemsPerPage.Value) //Get the current page of results
                            .ToList()
                            .ForEach(x =>
                            {
                                claimSearchResults.Add(ConvertToClaimSearchResult(x, request.MemberId));
                            });
            }

            response.PagingOptions = SetPagingOptions(searchResults.Count, request.ItemsPerPage, request.PageNumber, totalNumberOfRecords);

            return claimSearchResults;
        }

        private async Task<TestClaimResponse> SubmitTestClaim(ClaimSubmissionRequest request, CommonApiHelper commonApiHelper, List<Configuration> configurations)
        {
            var messageIdentifier = Guid.NewGuid();
            TestClaimResponse response = new TestClaimResponse();

            //Get claim api credentials
            var claimApiCredentials = configurations.First(x => x.ConfigurationKey == ConfigSetttingKey.PBMAPI_ClaimsAPI_ConnectionInfo).ConfigurationValues;
            string userName = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Username];
            string password = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Password];
            string url = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_BaseUrl] + ApiRoutes.ClaimSubmissionRequest;

            var output = await ApiHelper.ApiBasicAuthPost<ClaimSubmissionRequest, ClaimSubmissionResponse>(url, request, userName, password).ConfigureAwait(false);

            response.Header = output.Response.Header;
            response.Message = output.Response.Message;
            response.Insurance = output.Response.Insurance;
            response.Patient = output.Response.Patient;
            response.Claim = output.Response.Claim;
            response.COB = output.Response.COB;
            response.DUR = output.Response.DUR;
            response.Pricing = output.Response.Pricing;
            response.Status = output.Response.Status;

            if (output?.Response.NcpdpRequestString != null)
            {
                response.NcpdpRequestString = new string(output.Response.NcpdpRequestString.Where(c => !char.IsControl(c)).ToArray());
            }
            if (output?.Response.NcpdpResponseString != null)
            {
                response.NcpdpResponseString = new string(output.Response.NcpdpResponseString.Where(c => !char.IsControl(c)).ToArray());
            }

            return response;
        }
        #endregion Paid Claim Search

        #region Paid Claim Details
        private PaidClaimDetailsResponse convertToPaidClaimDetailsResult(PaidClaimDetailsDTO x, MemberIDType memberIdType)
        {
            PaidClaimDetailsResponse result = x.ConvertTo<PaidClaimDetailsResponse>();

            result.Claim = x.Claim.ConvertTo<Claim>();
            result.SubmittedMember = x.SubmittedMember.ConvertTo<ClaimSubmittedMember>();
            result.ActualMember = x.ActualMember.ConvertTo<ClaimActualMember>();
            result.Pharmacy = x.Pharmacy.ConvertTo<ClaimPharmacy>();
            result.Prescriber = x.Prescriber.ConvertTo<ClaimPrescriber>();
            result.ProductInformation = x.ProductInformation.ConvertTo<ClaimProductInformation>();
            result.ProductCost = x.ProductCost.ConvertTo<ClaimProductCost>();
            result.PlanInformation = x.PlanInformation.ConvertTo<ClaimPlanInformation>();
            //result.RejectInformation = x..ConvertTo<Claim>();
            result.DiagnosisInformation = x.DiagnosisInformation.ConvertTo<ClaimDiagnosisInformation>();
            result.SubmittedPricing = x.SubmittedPricing.ConvertTo<ClaimPricing>();
            result.CalculatedPricing = x.CalculatedPricing.ConvertTo<ClaimPricing>();
            result.IndividualAccumulationInformation = x.IndividualAccumulationInformation.ConvertTo<ClaimAccumulationInformation>();
            result.FamilyAccumulationInformation = x.FamilyAccumulationInformation.ConvertTo<ClaimAccumulationInformation>();
            result.DURPPS = x.DURPPS.ConvertTo<ClaimDURPPS>();
            result.PaymentInformation = x.PaymentInformation.ConvertTo<ClaimPaymentInformation>();
            result.AdjustmentPaymentInformation = x.AdjustmentPaymentInformation.ConvertTo<ClaimAdjustmentPaymentInformation>();

            result.Claim.FillDate = convertToDateString(x.Claim.FillDate);
            result.Claim.DateSubmitted = convertToDateString(x.Claim.DateSubmitted);
            result.Claim.SubmittedRxWrittenDate = convertToDateString(x.Claim.SubmittedRxWrittenDate);

            result.SubmittedMember.DateOfBirth = convertToDateString(x.SubmittedMember.DateOfBirth);

            if (Enum.TryParse(x.SubmittedMember.GenderCode, true, out GenderCode submittedGender))
            {
                result.SubmittedMember.GenderCode = submittedGender.ToString();
            }

            result.ActualMember.DateOfBirth = convertToDateString(x.ActualMember.DateOfBirth);

            result.PlanInformation.PlanEffectiveDate = convertToDateString(x.PlanInformation.PlanEffectiveDate);
            result.PlanInformation.PlanTerminationDate = convertToDateString(x.PlanInformation.PlanTerminationDate);
            result.PlanInformation.PAFromDate = convertToDateString(x.PlanInformation.PAFromDate);

            result.PaymentInformation.DatePosted = convertToDateString(x.PaymentInformation.DatePosted);

            switch (memberIdType)
            {
                case MemberIDType.OtherID:
                    result.ActualMember.MemberID = x.ActualMember.MemberIDOtherID;
                    break;
                case MemberIDType.CardIDCardID2:
                    result.ActualMember.MemberID = x.ActualMember.MemberIDCardIDCardID2;
                    break;
            }

            return result;
        }

        private async Task<PaidClaimDetailsDTO> getPaidClaimDailyDetails(List<DatasetDTO> datasets, int concurrentThreadCount, string claimNumber)
        {
            List<Task<PaidClaimDetailsDTO>> taskList = new List<Task<PaidClaimDetailsDTO>>();
            List<PaidClaimDetailsDTO> finalStateTaskList = new List<PaidClaimDetailsDTO>();

            int threadCounter = 1;

            for (int i = 0; i < datasets.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<PaidClaimDetailsDTO> t = Repository.GetPaidClaimDailyDetails(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path),
                                                                                      datasets[i].Name, claimNumber);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<PaidClaimDetailsDTO> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            PaidClaimDetailsDTO taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.Add(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return finalStateTaskList.FirstOrDefault(x => x.Claim != null);
        }

        private async Task<PaidClaimDetailsDTO> getPaidClaimHistoryDetails(List<DatasetDTO> datasets, int concurrentThreadCount, string claimNumber)
        {
            List<Task<PaidClaimDetailsDTO>> taskList = new List<Task<PaidClaimDetailsDTO>>();
            List<PaidClaimDetailsDTO> finalStateTaskList = new List<PaidClaimDetailsDTO>();

            int threadCounter = 1;

            for (int i = 0; i < datasets.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<PaidClaimDetailsDTO> t = Repository.GetPaidClaimHistoryDetails(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path),
                                                                                      datasets[i].Name, claimNumber);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<PaidClaimDetailsDTO> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            PaidClaimDetailsDTO taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.Add(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return finalStateTaskList.FirstOrDefault(x => x.Claim != null);
        }
        #endregion Paid Claim Details

        #region Test Claim

        #endregion

        #endregion

        #region Pharmacy

        private PharmacySearchResult convertToPharmacySearchResult(PharmacySearchDTO x)
        {
            PharmacySearchResult result = x.ConvertTo<PharmacySearchResult>();

            result.NCPDP = x.Pharmacy.PharmacyID;
            result.NPI = x.Pharmacy.PharmacyNPI;
            result.Client = x.Pharmacy.Client;
            result.Name = x.Pharmacy.PharmacyName;
            result.Address = x.Pharmacy.Address.ConvertTo<Address>();
            result.Phone = x.Pharmacy.PharmacyPhone;
            result.Distance = x.Pharmacy.Distance;
            result.Open24Hours = x.Pharmacy.Open24Hours;
            result.Flex90 = x.Pharmacy.Flex90;
            result.DispenserClass = x.Pharmacy.DispClass;
            result.DispenserType = x.Pharmacy.DispType;

            result.OpenHours = new OpenHours
            {
                MondayFridayOpen = x.Pharmacy.MondayFridayOpen,
                MondayFridayClose = x.Pharmacy.MondayFridayClose,
                SaturdayOpen = x.Pharmacy.SaturdayOpen,
                SaturdayClose = x.Pharmacy.SaturdayClose,
                SundayOpen = x.Pharmacy.SundayOpen,
                SundayClose = x.Pharmacy.SundayClose
            };

            if (x.Networks?.Count > 0)
            {
                result.Networks = new List<Messages.Response.Network>();
                x.Networks.ForEach(y =>
                {
                    if (!string.IsNullOrWhiteSpace(y.NetworkID))
                    {
                        result.Networks.Add(y.ConvertTo<Messages.Response.Network>());
                    }
                });
            }

            return result;
        }

        private async Task<List<PharmacyNetworkDTO>> pharmacySearchGetBatchWithinDistance(List<DatasetDTO> datasets, int concurrentThreadCount, string zip, int withinMiles, bool? open24Hours, bool? flex90, string planId)
        {
            List<Task<List<PharmacyNetworkDTO>>> taskList = new List<Task<List<PharmacyNetworkDTO>>>();
            List<PharmacyNetworkDTO> finalStateTaskList = new List<PharmacyNetworkDTO>();

            int threadCounter = 1;

            for (int i = 0; i < datasets.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<List<PharmacyNetworkDTO>> t = PharmacyRepository.GetPharmacySearchResultsWithinDistance(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path), datasets[i].Name, zip, withinMiles, open24Hours, flex90, planId,
                                                                         datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);

                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<List<PharmacyNetworkDTO>> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "pharmacySearchClientDatasets").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            List<PharmacyNetworkDTO> taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.AddRange(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return finalStateTaskList;
        }

        private async Task<int> pharmacySearchGetCount(List<DatasetDTO> datasets, int concurrentThreadCount, string zip, int withinMiles, bool? open24Hours, bool? flex90, string planId)
        {
            List<Task<int>> taskList = new List<Task<int>>();
            List<int> finalStateTaskList = new List<int>();

            int threadCounter = 1;

            for (int i = 0; i < datasets.Count; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<int> t = PharmacyRepository.GetPharmacySearchResultsCount(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path),
                                                                                   datasets[i].Name, zip, withinMiles, open24Hours, flex90, planId,
                                                                         datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
                {
                    await Task.WhenAll(taskList);

                    foreach (Task<int> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchGetCounts").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            int taskResult = await task.ConfigureAwait(false);
                            finalStateTaskList.Add(taskResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return finalStateTaskList.Sum();

        }
        #endregion Pharmacy

        #region Prescriber

        private PrescriberSearchResult convertToPrescriberSearchResult(PrescriberSearchDTO x)
        {
            PrescriberSearchResult result = x.ConvertTo<PrescriberSearchResult>();
            result.DeactivationDate = convertToDateString(x.DeactivationDate);
            return result;
        }

        private async Task<List<PrescriberSearchDTO>> PrescriberSearch(string prescriberId, string lastName,
                                                                             SearchTypeOperator? lastNameOperator, string firstName,
                                                                             SearchTypeOperator? firstNameOperator, bool includeDeactivatedPrescribers)
        {
            List<Task<List<PrescriberSearchDTO>>> taskList = new List<Task<List<PrescriberSearchDTO>>>();
            List<PrescriberSearchDTO> finalStateTaskList = new List<PrescriberSearchDTO>();

            Task<List<PrescriberSearchDTO>> t = PrescriberRepository.GetPrescriberSearchResults(prescriberId, lastName,
                                                                                getSearchTypeString(lastNameOperator), firstName,
                                                                                getSearchTypeString(firstNameOperator), includeDeactivatedPrescribers);
            taskList.Add(t);

            await Task.WhenAll(taskList);

            foreach (Task<List<PrescriberSearchDTO>> task in taskList)
            {
                if (task.Exception != null)
                {
                    Exception ex = task.Exception;
                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "PrescriberSearchClientDatasets").ConfigureAwait(false);
                }
                else
                {
                    // Technically don't have to await since we know it's done, but doing it just to be safe
                    List<PrescriberSearchDTO> taskResult = await task.ConfigureAwait(false);
                    finalStateTaskList.AddRange(taskResult);
                }
            }

            return finalStateTaskList;
        }

        private async Task<int> PrescriberSearchGetCount(string prescriberId, string lastName,
                                                      SearchTypeOperator? lastNameOperator, string firstName,
                                                      SearchTypeOperator? firstNameOperator, bool includeDeactivatedPrescribers)
        {
            List<Task<int>> taskList = new List<Task<int>>();
            List<int> finalStateTaskList = new List<int>();


            Task<int> t = PrescriberRepository.GetPrescriberSearchResultsCount(prescriberId, lastName, getSearchTypeString(lastNameOperator),
                                                                         firstName, getSearchTypeString(firstNameOperator), includeDeactivatedPrescribers);
            taskList.Add(t);

            await Task.WhenAll(taskList);

            foreach (Task<int> task in taskList)
            {
                if (task.Exception != null)
                {
                    Exception ex = task.Exception;
                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "PrescriberSearchGetCounts").ConfigureAwait(false);
                }
                else
                {
                    // Technically don't have to await since we know it's done, but doing it just to be safe
                    int taskResult = await task.ConfigureAwait(false);
                    finalStateTaskList.Add(taskResult);
                }
            }
            taskList.Clear();

            return finalStateTaskList.Sum();
        }

        private PrescriberDetailResponse convertToPrescriberDetailResponse(PrescriberDetailDTO x)
        {
            PrescriberDetailResponse result = x.ConvertTo<PrescriberDetailResponse>();
            result.DeactivationDate = convertToDateString(x.DeactivationDate) ?? "";
            return result;
        }

        #endregion Prescriber

        #region Member Portal

        private int getDomainIDFromDomainName(string domainName)
        {
            int result = DateWarehouseRepository.LookupDomainID(domainName).Result;

            //If a domain name was provided but we couldn't find it, error instead of continuing with default values
            if (result == 0)
            {
                throw new ArgumentException($"Domain {domainName} is not recognized.");
            }

            return result;
        }


        private int getClientIDFromDomainName(string domainName)
        {
            int result = DateWarehouseRepository.LookupClientID(domainName).Result;

            //If a domain name was provided but we couldn't find it, error instead of continuing with default values
            if (result == 0)
            {
                throw new ArgumentException($"Domain {domainName} is not recognized.");
            }

            return result;
        }

        private void verifyBINAccess(string binNumber)
        {
            if (!DateWarehouseRepository.BINAllowsOnlineAccess(binNumber).Result)
            {
                throw new ArgumentException($"BIN {binNumber} does not allow online access.");
            }
        }

        private void registerMemberPortalUser(int clientID, int domainID, string binNumber, string enrolleeID, string planID, string emailAddress,
                                              string username, string question, string answer, string firstName, string lastName, string clientName,
                                              string clientHostUrl, string smtpServer)
        {
            //Confirm member access
            verifyMemberAccess(clientID, planID, "RETL");

            //Confirm member not already registered
            verifyMemberNotAlreadyRegistered(clientID, enrolleeID, username);

            //Generate password
            string password = generateMemberPortalPassword();

            //Save user
            int userID = DateWarehouseRepository
                         .AddMember(emailAddress, getMD5Hash(password), username, binNumber, enrolleeID, question, answer, clientID, domainID).Result;

            //Send new user email
            sendMemberPortalRegistrationEmail(smtpServer, emailAddress, firstName, lastName, clientName, password, clientHostUrl);
        }

        private void sendLoginHelpInfo(int userID, int clientID, string planID, string emailAddress, string firstName, string lastName,
                                        string username, string clientName, string clientHostUrl, string smtpServer)
        {
            //TODO: JCastro - Validate the Business Rule with Hailey. 
            //Confirm member access
            verifyMemberAccess(clientID, planID, "RETL");

            //Generate new password
            string password = generateMemberPortalPassword();

            //Update Password     
            bool isUpdated = DateWarehouseRepository.UpdatePassword(userID, getMD5Hash(password)).Result;

            if (isUpdated)
            {
                //Send new user email with new password.
                sendMemberPortalLoginHelpEmail(smtpServer, emailAddress, firstName, lastName, username, clientName, password, clientHostUrl);
            }
        }

        private void sendChangePasswordInfo(int userID, string emailAddress, string firstName, string lastName,
                                        string username, string clientName, string clientHostUrl, string smtpServer)
        {
            //Generate new password
            string password = generateMemberPortalPassword();

            //Update Password     
            bool isUpdated = DateWarehouseRepository.UpdatePassword(userID, getMD5Hash(password)).Result;

            if (isUpdated)
            {
                //Send new user email with new password.
                sendMemberPortalChangePasswordEmail(smtpServer, emailAddress, firstName, lastName, username, clientName, clientHostUrl);
            }
        }

        private void verifyMemberAccess(int clientID, string planID, string planType)
        {
            PlanLookupDTO plan = verifyPlanExists(clientID, planID, planType);

            GroupLookupDTO group = getGroupForPlan(clientID, plan.PLNID, plan.GRPID);

            //If a level is in test mode, we don't verify any levels above it
            if (!group.TestMode)
            {
                OrgLookupDTO org = getOrgForGroup(clientID, group.GRPID, group.ORGID);

                if (!org.TestMode)
                {
                    ParentLookupDTO parent = getParentForOrg(clientID, org.ORGID, org.PARID);
                }
            }
        }

        public void verifyMemberNotAlreadyRegistered(int clientID, string enrolleeID, string username)
        {
            if (DateWarehouseRepository.EnrolleePreviouslyRegistered(clientID, enrolleeID).Result)
            {
                throw new ArgumentException($"The enrollee {enrolleeID} is already registered.");
            }

            if (!DateWarehouseRepository.UsernameAvailable(username).Result)
            {
                throw new ArgumentException($"The username {username} is not available.");
            }
        }

        private GroupLookupDTO getGroupForPlan(int clientID, string planID, string groupID)
        {
            string upperLevel = "group";
            string lookupLevel = "plan";

            GroupLookupDTO group = DateWarehouseRepository.LookupGroup(clientID, groupID).Result;

            verifyUpperLevelExists(upperLevel, group.GRPID, lookupLevel, planID);
            verifyUpperLevelAllowsWebAccess(upperLevel, group.GRPID, group.AllowsWebAccess);

            return group;
        }

        private OrgLookupDTO getOrgForGroup(int clientID, string groupID, string orgID)
        {
            string upperLevel = "org";
            string lookupLevel = "group";

            OrgLookupDTO org = DateWarehouseRepository.LookupOrg(clientID, orgID).Result;

            verifyUpperLevelExists(upperLevel, org.ORGID, lookupLevel, groupID);
            verifyUpperLevelAllowsWebAccess(upperLevel, org.ORGID, org.AllowsWebAccess);

            return org;
        }

        private ParentLookupDTO getParentForOrg(int clientID, string orgID, string parID)
        {
            string upperLevel = "parent";
            string lookupLevel = "org";

            ParentLookupDTO parent = DateWarehouseRepository.LookupParent(clientID, parID).Result;

            verifyUpperLevelExists(upperLevel, parent.PARID, lookupLevel, orgID);
            verifyUpperLevelAllowsWebAccess(upperLevel, parent.PARID, parent.AllowsWebAccess);

            return parent;
        }

        private PlanLookupDTO verifyPlanExists(int clientID, string planID, string planType)
        {
            PlanLookupDTO plan = DateWarehouseRepository.LookupPlan(clientID, planID, planType).Result;

            if (string.IsNullOrWhiteSpace(plan.PLNID))
            {
                throw new ArgumentException($"A plan {planID.ToUpper()} with type {planType.ToUpper()} was not found.");
            }

            if (!plan.Active)
            {
                throw new ArgumentException($"No plan {planID.ToUpper()} with type {planType.ToUpper()} is currently active.");
            }

            return plan;
        }

        private void verifyUpperLevelExists(string upperLevel, string upperLevelID, string lookupLevel, string lookupLevelID)
        {
            if (string.IsNullOrWhiteSpace(upperLevelID))
            {
                throw new ArgumentException($"No {upperLevel.ToLower()} found for {lookupLevel.ToLower()} {lookupLevelID.ToUpper()}.");
            }
        }

        private void verifyUpperLevelAllowsWebAccess(string upperLevel, string upperLevelID, bool upperLevelAllowsAccess)
        {
            if (!upperLevelAllowsAccess)
            {
                throw new ArgumentException($"{upperLevel.ToLower().ToTitleCase()} {upperLevelID.ToUpper()} does not allow online access.");
            }
        }

        private string generateMemberPortalPassword()
        {
            Random objRandom = new Random(System.DateTime.Now.Millisecond);
            char chr0 = StringsChr(objRandom.Next(StringsAsc('A'), StringsAsc('Z')));
            char chr1 = Convert.ToChar(Convert.ToString(objRandom.Next(0, 9)));
            char chr2 = StringsChr(objRandom.Next(StringsAsc('A'), StringsAsc('Z')));
            char chr3 = Convert.ToChar(Convert.ToString(objRandom.Next(0, 9)));
            char chr4 = StringsChr(objRandom.Next(StringsAsc('A'), StringsAsc('Z')));
            char chr5 = Convert.ToChar(Convert.ToString(objRandom.Next(0, 9)));
            char chr6 = StringsChr(objRandom.Next(StringsAsc('A'), StringsAsc('Z')));
            char chr7 = Convert.ToChar(Convert.ToString(objRandom.Next(0, 9)));
            string strPassword = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", chr0, chr1, chr2, chr3, chr4, chr5, chr6, chr7);
            return strPassword.Trim();
        }

        private char StringsChr(int p_intByte)
        {
            byte[] bytBuffer = BitConverter.GetBytes(p_intByte);
            char[] chrBuffer = Encoding.Unicode.GetChars(bytBuffer);
            return chrBuffer[0];
        }

        private int StringsAsc(char c)
        {
            int converted = c;

            if (converted >= 0x80)
            {

                byte[] buffer = new byte[2];

                // if the resulting conversion is 1 byte in length, just use the value

                if (System.Text.Encoding.Default.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
                {
                    converted = buffer[0];
                }
                else
                {
                    // byte swap bytes 1 and 2;
                    converted = buffer[0] << 16 | buffer[1];
                }
            }

            return converted;

        }

        private string getMD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        private void sendMemberPortalPasswordResetEmail(string smtpServer, string emailAddress, string firstName, string lastName, string username, string password, string clientName, string clientHostUrl)
        {
            MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress("admin@procarerx.com", "ProCare Rx Member Access");
            objMailMessage.To.Add(new MailAddress(emailAddress, string.Format("{0} {1}", firstName, lastName)));
            objMailMessage.Subject = clientName + " Member Portal: Login Help";
            objMailMessage.Body = "";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "You have requested a new password from " + clientName + "'s Member Portal website. ";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Your user name is: " + username.Trim();
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Your new password is:   " + password;
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "To access the Member Portal website please visit https://" + clientHostUrl;
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "To reset your password or to change your verify question, once you have ";
            objMailMessage.Body += "returned to the website and logged in, click the \"My Profile\" link at the top right of the Member Portal home page.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "If you have any questions or comments, please click ";
            objMailMessage.Body += "\"Contact Us\" at the top right of the Member Portal home page.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Thank you,";
            objMailMessage.Body += Environment.NewLine;
            objMailMessage.Body += "Member Portal Web Support Team";
            objMailMessage.Body += Environment.NewLine;

            objMailMessage.IsBodyHtml = false;
            SmtpClient objSmtpClient = new SmtpClient(smtpServer);
            objSmtpClient.Send(objMailMessage);

        }

        private void sendMemberPortalPasswordChangeEmail(string smtpServer, string emailAddress, string firstName, string lastName, string username, string clientName, string clientHostUrl)
        {

            MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress("admin@procarerx.com", "Member Portal");

            objMailMessage.To.Add(new MailAddress(emailAddress, string.Format("{0} {1}", firstName, lastName)));
            objMailMessage.Subject = clientName + " Member Portal: Password Change Confirmation";
            objMailMessage.Body = string.Empty;
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += username + ",";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "The password for the account associated with this email address at the " + clientName.Trim() + " Member Portal has been changed.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "If you made this change, nothing further needs to be done.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "If you did not make this change, please take a moment to reset your password now. To do so, visit our Login Help page: https://" + clientHostUrl.Trim() + "/Account/LoginHelp and click on the \"Click Here for Help Logging In\" link.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Thank you," + Environment.NewLine;
            objMailMessage.Body += "Member Portal Web Support Team" + Environment.NewLine;


            objMailMessage.IsBodyHtml = false;
            SmtpClient objSmtpClient = new SmtpClient(smtpServer);
            objSmtpClient.Send(objMailMessage);

        }

        private void sendMemberPortalRegistrationEmail(string smtpServer, string emailAddress, string firstName, string lastName, string clientName, string password, string clientHostUrl)
        {
            MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress("admin@procarerx.com", "ProCare Rx Member Portal");
            objMailMessage.To.Add(new MailAddress(emailAddress,
                                                  string.Format("{0} {1}", firstName, lastName)));
            objMailMessage.Subject = clientName + " Member Portal: Registration Confirmation";
            objMailMessage.Body = string.Empty;
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Welcome, and thank you for registering with " + clientName +
                                   "'s Member Portal website. ";
            objMailMessage.Body +=
                "Here you can manage your prescription needs at any time with the following benefit tools: ";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "  - Review your prescription history" + Environment.NewLine;
            objMailMessage.Body +=
                "  - Refill prescriptions and/or transfer prescriptions to mail order service, and receive up to a 90-day supply delivered directly to your home" +
                Environment.NewLine;
            objMailMessage.Body +=
                "  - Locate a pharmacy within a ZIP code, state, county, or city" + Environment.NewLine;
            objMailMessage.Body += "  - Track your year-to-date prescription expenses" + Environment.NewLine;
            objMailMessage.Body += "  - Look up a drug to identify formulary status and preferred alternatives" +
                                   Environment.NewLine;
            objMailMessage.Body += "  - Print a prescription history report for tax purposes" + Environment.NewLine;
            objMailMessage.Body += "  - Check drug pricing" + Environment.NewLine;
            objMailMessage.Body += "  - Print a temporary card" + Environment.NewLine;
            objMailMessage.Body += "  - And much, much more...." + Environment.NewLine;
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Your randomly assigned password is: " + password;
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "To access the Member Portal website please visit https://" + clientHostUrl;
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "To change your password, your security prompt, or your email address, ";
            objMailMessage.Body +=
                "click on the \"My Profile\" link at the top right of the Member Portal home page.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "If you have any questions or comments, please click \"Contact Us\" ";
            objMailMessage.Body += "at the top right of the Member Portal home page.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Once again,";
            objMailMessage.Body += Environment.NewLine;
            objMailMessage.Body += "Welcome!";
            objMailMessage.IsBodyHtml = false;
            SmtpClient objSmtpClient = new SmtpClient(smtpServer);
            objSmtpClient.Send(objMailMessage);
        }

        private void sendMemberPortalLoginHelpEmail(string smtpServer, string emailAddress, string firstName, string lastName, string username, string clientName, string password, string clientHostUrl)
        {
            MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress("admin@procarerx.com", "ProCare Rx Member Portal");
            objMailMessage.To.Add(new MailAddress(emailAddress, string.Format("{0} {1}", firstName.Trim(), lastName.Trim())));
            objMailMessage.Subject = clientName + " Member Portal: Registration Confirmation";
            objMailMessage.Body = string.Empty;

            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "You have requested a new password from " + clientName.Trim() + "'s Member Portal website. ";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Your user name is: " + username.Trim();
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Your new password is:   " + password;
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "To access the Member Portal website please visit https://" + clientHostUrl.Trim();
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "To reset your password or to change your verify question, once you have ";
            objMailMessage.Body += "returned to the website and logged in, click the \"My Profile\" link at the top right of the Member Portal home page.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "If you have any questions or comments, please click ";
            objMailMessage.Body += "\"Contact Us\" at the top right of the Member Portal home page.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Thank you,";
            objMailMessage.Body += Environment.NewLine;
            objMailMessage.Body += "Member Portal Web Support Team";


            objMailMessage.IsBodyHtml = false;
            SmtpClient objSmtpClient = new SmtpClient(smtpServer);
            objSmtpClient.Send(objMailMessage);
        }

        private void sendMemberPortalChangePasswordEmail(string smtpServer, string emailAddress, string firstName, string lastName, string username, string clientName, string clientHostUrl)
        {
            MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress("admin@procarerx.com", "ProCare Rx Member Portal");
            objMailMessage.To.Add(new MailAddress(emailAddress, string.Format("{0} {1}", firstName.Trim(), lastName.Trim())));
            objMailMessage.Subject = clientName + " Member Portal: Password Change Confirmation";
            objMailMessage.Body = string.Empty;

            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += firstName.Trim() + lastName.Trim() + ",";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "The password for the account associated with this email address at the " + username.Trim() + " Member Portal has been changed.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "If you made this change, nothing further needs to be done.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "If you did not make this change, please take a moment to reset your password now. To do so, visit our Login Help page: https://" + clientHostUrl.Trim() + "/Account/LoginHelp and click on the \"Click Here for Help Logging In\" link.";
            objMailMessage.Body += Environment.NewLine + Environment.NewLine;
            objMailMessage.Body += "Thank you," + Environment.NewLine;
            objMailMessage.Body += "Member Portal Web Support Team" + Environment.NewLine;

            objMailMessage.IsBodyHtml = false;
            SmtpClient objSmtpClient = new SmtpClient(smtpServer);
            objSmtpClient.Send(objMailMessage);
        }

        #endregion

        #region Hospice Import

        private int GetVendorId(int requestVendorID, NameValueCollection headers)
        {
            int vendorID = requestVendorID;

            if (vendorID == 0 && Request.Headers.AllKeys.Contains(Header.VendorID))
            {
                vendorID = Convert.ToInt32(Request.Headers[Header.VendorID]);
            }

            if (vendorID == 0)
            {
                throw new ArgumentException(
                    "Invalid VendorID.  A non-zero VendorID must be included in the request body or as a header on the request.");
            }

            return vendorID;
        }

        private void validateUserToken(PRXUserSSODetailsDTO tokenData)
        {
            if (tokenData == null || tokenData.ClientID == 0)
            {
                throw new ArgumentException("Member authentication token is expired or invalid");
            }

        }

        private async Task<HospiceImportInsertRecordResponse> ProcessHospiceImportRecord(int requestVendorID,
                                                                                         NameValueCollection headers,
                                                                                         string importRecord)
        {
            // Initialize variables
            var response = new HospiceImportInsertRecordResponse();

            int vendorID = GetVendorId(requestVendorID, headers);

            var externalId = int.Parse(GetHL7Value(importRecord, "PID", 3, 0));

            bool IsSothMedication = vendorID == Constants.stateOfTheHeart && GetHL7Value(importRecord, "MSH", 8, 1) == "A10";

            int queueStatus = IsSothMedication ? 0 : 1;

            long? patientId = IsSothMedication ? null : GetPatientIdByExternalId(externalId);

            // Process
            response.RecordID = await HospiceRepository.InsertImportRecord(vendorID, importRecord, queueStatus, patientId).ConfigureAwait(false);

            if (vendorID == 28)
            {
                long preImportId = await HospiceRepository.InsertPreImportRecord(response.RecordID, externalId);

                if (IsSothMedication)
                {
                    _ = await HospiceRepository.InsertPreImportMedicationQueueRecord(preImportId);
                }
            }

            return response;
        }

        // this function is necessary because the ternary expression using it
        // won't make the repository call directly unless upgraded to C# version 9 or greater
        private long? GetPatientIdByExternalId(int externalId)
        {
            return HospiceRepository.GetPatientIdByExternalId(externalId).Result;
        }

        private string GetHL7Value(string importRecord, string row, int column, int subColumn)
        {
            var response = string.Empty;

            string[] lines = importRecord.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var fields = line.Split("|");

                if (fields[0] == row)
                {
                    if (subColumn > -1)
                    {
                        var subfields = fields[column].Split("^");
                        response = subfields[subColumn];
                        break;
                    }
                    else
                    {
                        response = fields[column];
                        break;
                    }

                }
            }
            return response;
        }

        #endregion Hospice Import

        private static void mapFeeSchedule(ClaimSubmissionResponse response, FeeScheduleDTO feeResponse)
        {
            response.Pricing.FeeScheduleCharge = feeResponse.Charge;
            response.Pricing.FeeScheduleDispensingFee = feeResponse.DispensingFee;
            response.Pricing.FeeScheduleIngredientCost = feeResponse.IngredientCost;
            response.Pricing.FeeScheduleTotalPrice = feeResponse.TotalPrice;
            response.Pricing.AverageWholesalePrice = feeResponse.AverageWholesalePrice;
        }

        private TestClaimSubmissionRequest populateMissingTestClaimFieldsWithDefaults(TestClaimSubmissionRequest originalRequest, Dictionary<string, string> defaultValues)
        {
            //Map default values
            TestClaimSubmissionRequest defaultClaimRequest = new TestClaimSubmissionRequest();
            defaultClaimRequest.Header = JsonConvert.DeserializeObject<RequestHeader>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Header]);
            defaultClaimRequest.Claim = JsonConvert.DeserializeObject<RequestClaim>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Claim]);
            defaultClaimRequest.Compound = JsonConvert.DeserializeObject<RequestCompound>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Compound]);
            defaultClaimRequest.DUR = JsonConvert.DeserializeObject<RequestDrugUtilizationReview>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Dur]);
            defaultClaimRequest.Prescriber = JsonConvert.DeserializeObject<RequestPrescriber>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Prescriber]);
            defaultClaimRequest.Pricing = JsonConvert.DeserializeObject<RequestPricing>(defaultValues[ConfigSetttingKey.PBMAPI_ClaimsAPI_TestClaim_Pricing]);

            //Copy original request into new request
            TestClaimSubmissionRequest request = DataValidationHelper.CloneJson(originalRequest);

            //Populate any missing fields
            request.PopulateWithDefaultsIfEmpty(defaultClaimRequest);

            return request;
        }

        #region User

        private List<PermissionDTO> filterUserPermissions(List<PermissionDTO> permissionDTOs)
        {
            List<PermissionDTO> userRolePermissions = permissionDTOs.Where(x => x.AssignmentSource == ConfigSetttingKey.AppUserInfo_AssignmentSource_Role).Distinct().ToList();
            List<PermissionDTO> userPermissions = permissionDTOs.Where(x => x.AssignmentSource == ConfigSetttingKey.AppUserInfo_AssignmentSource_User).Distinct().ToList();
            userRolePermissions.RemoveAll(x => userPermissions.FirstOrDefault(y => y.PermissionID == x.PermissionID && y.AppUserID == x.AppUserID) != null);
            userPermissions.RemoveAll(x => x.GrantLevel == 0);
            userRolePermissions.RemoveAll(x => x.GrantLevel == 0);
            List<PermissionDTO> resultPermissions = new List<PermissionDTO>();
            resultPermissions.AddRange(userPermissions);
            resultPermissions.AddRange(userRolePermissions);

            resultPermissions = (from p in resultPermissions
                                 group p by new { p.AppUserID, p.PermissionID, p.PermissionName, p.CategoryID, p.CategoryName, p.GrantLevel } into pg
                                 select new PermissionDTO
                                 {
                                     AppUserID = pg.Key.AppUserID,
                                     PermissionID = pg.Key.PermissionID,
                                     PermissionName = pg.Key.PermissionName,
                                     CategoryID = pg.Key.CategoryID,
                                     CategoryName = pg.Key.CategoryName,
                                     GrantLevel = pg.Max(g => g.GrantLevel)
                                 }).ToList();

            return resultPermissions;
        }

        private List<UserPermissionDTO> filterUserPermissions(List<UserPermissionDTO> permissionDTOs)
        {
            List<UserPermissionDTO> userRolePermissions = permissionDTOs.Where(x => x.AssignmentSource == ConfigSetttingKey.AppUserInfo_AssignmentSource_Role).Distinct().ToList();
            List<UserPermissionDTO> userPermissions = permissionDTOs.Where(x => x.AssignmentSource == ConfigSetttingKey.AppUserInfo_AssignmentSource_User).Distinct().ToList();
            userRolePermissions.RemoveAll(x => userPermissions.FirstOrDefault(y => y.PermissionID == x.PermissionID && y.AppUserID == x.AppUserID) != null);
            userPermissions.RemoveAll(x => x.GrantLevel == 0);
            userRolePermissions.RemoveAll(x => x.GrantLevel == 0);
            List<UserPermissionDTO> resultPermissions = new List<UserPermissionDTO>();
            resultPermissions.AddRange(userPermissions);
            resultPermissions.AddRange(userRolePermissions);

            resultPermissions = (from p in resultPermissions
                                 group p by new { p.AppUserID, p.FirstName, p.LastName, p.PermissionID, p.PermissionName, p.CategoryID, p.CategoryName, p.GrantLevel } into pg
                                 select new UserPermissionDTO
                                 {
                                     AppUserID = pg.Key.AppUserID,
                                     FirstName = pg.Key.FirstName,
                                     LastName = pg.Key.LastName,
                                     PermissionID = pg.Key.PermissionID,
                                     PermissionName = pg.Key.PermissionName,
                                     CategoryID = pg.Key.CategoryID,
                                     CategoryName = pg.Key.CategoryName,
                                     GrantLevel = pg.Max(g => g.GrantLevel)
                                 }).ToList();

            return resultPermissions;
        }

        #endregion User

        #region Note  
        private string GetSubstringNote(string noteString)
        {
            string[] seperators = new string[] { @"\n", @"\r\n" };
            string[] noteStringArray = noteString.Split(seperators, StringSplitOptions.None);
            string note = "";

            foreach (string str in noteStringArray)
            {
                List<string> notes = SplitText(str, Constants.ChunkSize);
                foreach (var item in notes)
                {
                    note += item + "\\n";
                }
            }

            return note.Substring(0, note.Length - 2);
        }

        private static List<string> SplitText(string text, int chunkSize)
        {
            int textLength = text.Length;
            int partialSegment = textLength % chunkSize > 0 ? 1 : 0;
            int segments = (textLength / chunkSize) + partialSegment;

            return Enumerable.Range(0, segments)
                    .Select(i => text.Substring(i * chunkSize, Math.Min(chunkSize, textLength - (i * chunkSize)))).ToList();
        }

        #endregion 

        #endregion


    }
}
