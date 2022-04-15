using Newtonsoft.Json;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ProCare.API.PBM.Helpers;
using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Response;
using ProCare.API.PBM.Repository;
using ProCare.API.PBM.Repository.DTO;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.API
{
    public class MemberServices : ServiceBase
    {
        #region Public Properties
        public IPbmRepository PBMRepository { get; set; }
        public IMemberRepository MemberRepository { get; set; }
        #endregion

        #region Public Methods
        //[Authenticate]
        //[RequiresAnyPermission(ApiRoutes.MemberSearchRequest + "|" + ApiRoutes.MemberSearchRequest)]
        //public async Task<MemberSearchResponse> Get(MemberSearchRequest request)
        //{
        //    SearchTypeOperator? lastNameOperator = DataValidationHelper.GetSearchType(request.LastName, request.LastNameOperator);
        //    SearchTypeOperator? firstNameOperator = DataValidationHelper.GetSearchType(request.FirstName, request.FirstNameOperator);
        //    SearchTypeOperator? memberIdOperator = DataValidationHelper.GetSearchType(request.MemberId, request.MemberIdOperator);

        //    MemberSearchResponse response = new MemberSearchResponse();
        //    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
        //    response = await processRequest(request, ApiRoutes.MemberSearchRequest, async () =>
        //    {
        //        DateTime? dob = null;

        //        if (!string.IsNullOrWhiteSpace(request.DateOfBirth))
        //        {
        //            dob = DateTime.ParseExact(request.DateOfBirth, "yyyyMMdd", null);
        //        }

        //        // Retrieve client connection string and maximum records settings from configuration
        //        Dictionary<string, string> clientSetting = await ClientConfigHelper.GetClientConfigs(request.ClientGuid).ConfigureAwait(false);
        //        Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MemberSearchConfig).ConfigureAwait(false);

        //        List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);

        //        if (!string.IsNullOrWhiteSpace(request.Client))
        //        {
        //            datasets = datasets.Where(x => x.Name.EqualsIgnoreCase(request.Client)).ToList();
        //        }

        //        int maxRecords = Convert.ToInt32(config[ConfigSetttingKey.ApiMaxNumberOfRecordsSetting_KeyValue]);
        //        int maxCountThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxCountThreads]);
        //        int maxSearchThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxSearchThreads]);

        //        if (request.ItemsPerPage == null)
        //        {
        //            request.ItemsPerPage = maxRecords;
        //        }

        //        if (request.PageNumber == null)
        //        {
        //            request.PageNumber = 1;
        //        }

        //        // Perform Member search, retrieve results and populate response
        //        int totalNumberOfRecords = await memberSearchGetCounts(datasets, maxCountThreads, request.MemberId, memberIdOperator,
        //                                                               request.LastName, lastNameOperator, request.FirstName,
        //                                                               firstNameOperator, dob).ConfigureAwait(false);

        //        //Get search results and count of historical duplicates that were removed
        //        Tuple<List<MemberSearchDTO>, int> searchResultInfo = await memberSearchClientDatasets(datasets, maxSearchThreads, request.MemberId, memberIdOperator,
        //                                                                               request.LastName, lastNameOperator, request.FirstName,
        //                                                                               firstNameOperator, dob)
        //                                                                               .ConfigureAwait(false);

        //        //Only return top max records, FamilyId is CardID + CardID2
        //        List<MemberSearchDTO> searchResults = searchResultInfo.Item1.OrderBy(x => x.FamilyId).Take(maxRecords).ToList();

        //        //Subtract any duplicates from the count
        //        totalNumberOfRecords -= searchResultInfo.Item2;

        //        List<MemberSearchResult> MemberSearchResults = new List<MemberSearchResult>();
        //        PagingOptionsResponse pagingOption = new PagingOptionsResponse();
                
        //        response.MemberSearchResults = MemberSearchResults;

        //        MemberSearchDTO firstItem = searchResults.FirstOrDefault();

        //        if (request.ItemsPerPage >= totalNumberOfRecords)
        //        {
        //            //Add all
        //            searchResults.ForEach(x =>
        //            {
        //                MemberSearchResult result = convertToMemberSearchResult(x);
        //                MemberSearchResults.Add(result);
        //            });
        //        }
        //        else
        //        {
        //            //Add the current page
        //            searchResults
        //                .Skip(request.ItemsPerPage.Value * (request.PageNumber.Value - 1)) //Skip previous pages
        //                .Take(request.ItemsPerPage.Value) //Get the current page of results
        //                .ToList()
        //                .ForEach(x => { MemberSearchResults.Add(convertToMemberSearchResult(x)); });
        //        }


        //        if (firstItem != null)
        //        {
        //            pagingOption.PageNumber = request.PageNumber;
        //            pagingOption.ItemsPerPage = request.ItemsPerPage;
        //            pagingOption.TotalNumberOfPages = totalNumberOfRecords / request.ItemsPerPage;
        //            if (totalNumberOfRecords % request.ItemsPerPage > 0)
        //            {
        //                pagingOption.TotalNumberOfPages++;
        //            }
        //            pagingOption.TotalNumberOfRecords = totalNumberOfRecords;
        //        }
        //        else
        //        {
        //            pagingOption.PageNumber = 1;
        //            pagingOption.ItemsPerPage = 0;
        //            pagingOption.TotalNumberOfPages = 1;
        //            pagingOption.TotalNumberOfRecords = 0;
        //        }

        //        response.PagingOptions = pagingOption;

        //        return response;
        //    }).ConfigureAwait(false);

        //    return response;
        //}

    
        //[Authenticate]
        //[RequiresAnyPermission(ApiRoutes.MemberDetailsRequest + "|" + ApiRoutes.MemberDetailsRequest)]
        //public async Task<MemberDetailsResponse> Get(MemberDetailsRequest request)
        //{
        //    MemberDetailsResponse response = new MemberDetailsResponse();
        //    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
        //    response = await processRequest(request, ApiRoutes.MemberDetailsRequest, async () =>
        //    {
        //        // Retrieve client connection string and maximum records settings from configuration
        //        Dictionary<string, string> clientSetting = await ClientConfigHelper.GetClientConfigs(request.ClientGuid).ConfigureAwait(false);
        //        Dictionary<string, string> config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MemberDetailsConfig).ConfigureAwait(false);

        //        int maxSearchThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxSearchThreads]);
        //        List<DatasetDTO> datasets = ClientConfigHelper.GetClientDatasets(clientSetting, request.Client, null, request.OrganizationID, request.GroupID, request.PlanID);

        //        // Perform Member search, retrieve results and populate response
        //        List<MemberDetailsResultDTO> results =
        //            await getMemberDetails(datasets, maxSearchThreads, request.OrganizationID, request.GroupID, request.PlanID, request.MemberID,
        //                                   request.Person).ConfigureAwait(false);

        //        //Now that we know which results are from ENR, we want to get the distinct result list regardless of table
        //        results = results.GroupBy(x => new { x.MemberDetail.MemberEnrolleeID, x.MemberDetail.PlanID, PlanEffectiveDate = x.PlanInfo.EffectiveDate, PlanTerminationDate = x.PlanInfo.TerminationDate, MemberEffectiveDate = x.MemberCoverage.EffectiveDate, MemberTerminationDate = x.MemberCoverage.TerminationDate})
        //                    .Select(x => x.OrderByDescending(y => y.TableName)) //Prefer APSENR
        //                    .OrderByDescending(x => x.First().PlanInfo.EffectiveDate).ThenByDescending(x => x.First().MemberCoverage.EffectiveDate) //Order groups by plan effective date desc, member effective date desc
        //                    .Select(x => x.First()).ToList(); //Take first from each group, this will prefer APSENR in the case of dupes

        //        List<MemberDetailsResultDTO> enrResults = results.Where(x => x.TableName.EqualsIgnoreCase("APSENR")).ToList();

        //        //Filter to first result if not a multi-segent response
        //        if (!request.MultiSegmentResponseIndicator && results.Count > 1)
        //        {
        //            if (enrResults.Count > 0)
        //            {
        //                results = new List<MemberDetailsResultDTO> { enrResults.FirstOrDefault() };
        //            }
        //            else
        //            {
        //                results = new List<MemberDetailsResultDTO> {results.FirstOrDefault()};
        //            }
        //        }

        //        results = await getMemberDetailDiagnoses(datasets, maxSearchThreads, results).ConfigureAwait(false);

        //        List<MemberDetailsResult> MemberDetails = new List<MemberDetailsResult>();

        //        response.MemberDetailsResults = MemberDetails;

        //        results.ForEach(x =>
        //        {
        //            MemberDetailsResult result = convertToMemberDetailsResult(x);
        //            MemberDetails.Add(result);
        //        });

        //        return response;
        //    }).ConfigureAwait(false);

        //    return response;
        //}

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberTerminationRequest + "|" + ApiRoutes.MemberTerminationRequest)]
        public async Task<MemberTerminationResponse> Post(MemberTerminationRequest request)
        {
            MemberTerminationResponse response = new MemberTerminationResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.MemberDetailsRequest, async () =>
            {
                // Retrieve client dataset from configuration
                Dictionary<string, string> clientSetting = await ClientConfigHelper.GetClientConfigs(request.ClientGuid).ConfigureAwait(false);
                DatasetDTO dataset = ClientConfigHelper.GetClientDataset(clientSetting, request.Client, null, null, null, request.PlanID);

                // Terminate 
                string clientConnectionString = ClientConfigHelper.GetConnectionStringFromDataset(dataset);
                string userID = ClientConfigHelper.GetUserIDFromClientGuid(request.ClientGuid);

                string enrolleeId = await DataValidationHelper.GetEnrolleeId(PBMRepository, clientConnectionString, dataset.MemberIdType, request.PlanID, request.MemberID, request.Person, request.MemberEnrolleeID).ConfigureAwait(false);

                if (!DataValidationHelper.IsValidMember(PBMRepository, clientConnectionString, request.PlanID,
                                  !string.IsNullOrWhiteSpace(request.MemberEnrolleeID) ? request.MemberEnrolleeID : enrolleeId,
                                  out string errorMessage))
                {
                    throw new ArgumentException(errorMessage);
                }

                MemberTerminateDTO member = await MemberRepository.TerminateMember(clientConnectionString, request.PlanID, enrolleeId, request.TerminationDate, userID);

                // Build response and return
                response.Client = request.Client;
                response.PlanID = member.PlanId;
                response.MemberID = request.MemberID;
                response.MemberEnrolleeID = member.MemberEnrolleeId;
                response.PersonCode = member.Person;
                response.TerminationDate = member.MemberTerminationDate;

                return response;
            }).ConfigureAwait(false);

            return response;
        }
        #endregion

        #region Private Methods

        //private async Task<int> memberSearchGetCounts(List<DatasetDTO> datasets, int concurrentThreadCount, string memberId,
        //                                              SearchTypeOperator? memberIdOperator, string lastName,
        //                                              SearchTypeOperator? lastNameOperator, string firstName,
        //                                              SearchTypeOperator? firstNameOperator, DateTime? dateOfBirth)
        //{
        //    List<Task<int>> taskList = new List<Task<int>>();
        //    List<int> finalStateTaskList = new List<int>();

        //    int threadCounter = 1;

        //    for (int i = 0; i < datasets.Count; i++)
        //    {
        //        if (threadCounter <= concurrentThreadCount)
        //        {
        //            Task<int> t = MemberRepository.GetMemberSearchResultsCount(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path), datasets[i].Name,
        //                                                                 datasets[i].MemberIdType, memberId, DataValidationHelper.GetSearchTypeString(memberIdOperator),
        //                                                                 lastName, DataValidationHelper.GetSearchTypeString(lastNameOperator), firstName,
        //                                                                 DataValidationHelper.GetSearchTypeString(firstNameOperator), dateOfBirth,
        //                                                                 datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
        //            taskList.Add(t);
        //            threadCounter++;
        //        }

        //        if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
        //        {
        //            await Task.WhenAll(taskList);

        //            foreach (Task<int> task in taskList)
        //            {
        //                if (task.Exception != null)
        //                {
        //                    Exception ex = task.Exception;
        //                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
        //                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchGetCounts").ConfigureAwait(false);
        //                }
        //                else
        //                {
        //                    // Technically don't have to await since we know it's done, but doing it just to be safe
        //                    int taskResult = await task.ConfigureAwait(false);
        //                    finalStateTaskList.Add(taskResult);
        //                }
        //            }

        //            threadCounter = 1;
        //            taskList.Clear();
        //        }
        //    }

        //    return finalStateTaskList.Sum();
        //}

        //private async Task<List<MemberDetailsResultDTO>> getMemberDetails(List<DatasetDTO> datasets, int concurrentThreadCount, string organizationId,
        //                                                           string groupId, string planId, string memberId, string person)
        //{
        //    List<Task<List<MemberDetailsResultDTO>>> taskList = new List<Task<List<MemberDetailsResultDTO>>>();
        //    List<MemberDetailsResultDTO> finalStateTaskList = new List<MemberDetailsResultDTO>();

        //    int threadCounter = 1;

        //    for (int i = 0; i < datasets.Count; i++)
        //    {
        //        if (threadCounter <= concurrentThreadCount)
        //        {
        //            Task<List<MemberDetailsResultDTO>> t = MemberRepository.GetMemberDetails(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path),
        //                                                                              datasets[i].Name, datasets[i].MemberIdType, organizationId,
        //                                                                              groupId, planId, memberId, person);
        //            taskList.Add(t);
        //            threadCounter++;
        //        }

        //        if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
        //        {
        //            await Task.WhenAll(taskList);

        //            foreach (Task<List<MemberDetailsResultDTO>> task in taskList)
        //            {
        //                if (task.Exception != null)
        //                {
        //                    Exception ex = task.Exception;
        //                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
        //                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
        //                }
        //                else
        //                {
        //                    // Technically don't have to await since we know it's done, but doing it just to be safe
        //                    List<MemberDetailsResultDTO> taskResult = await task.ConfigureAwait(false);
        //                    finalStateTaskList.AddRange(taskResult);
        //                }
        //            }

        //            threadCounter = 1;
        //            taskList.Clear();
        //        }
        //    }

        //    return finalStateTaskList;
        //}

        //private async Task<List<MemberDetailsResultDTO>> getMemberDetailDiagnoses(List<DatasetDTO> datasets, int concurrentThreadCount, List<MemberDetailsResultDTO> memberDetails)
        //{
        //    List<Task<List<MemberDetailsMemberDiagnosisDTO>>> taskList = new List<Task<List<MemberDetailsMemberDiagnosisDTO>>>();
        //    List<MemberDetailsMemberDiagnosisDTO> finalStateTaskList = new List<MemberDetailsMemberDiagnosisDTO>();

        //    int threadCounter = 1;

        //    for (int i = 0; i < memberDetails.Count; i++)
        //    {
        //        if (threadCounter <= concurrentThreadCount)
        //        {
        //            DatasetDTO dataset = datasets.FirstOrDefault(x => x.Name.EqualsIgnoreCase(memberDetails[i].Client));

        //            Task<List<MemberDetailsMemberDiagnosisDTO>> t = MemberRepository.GetMemberDiagnoses(MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path),
        //                                                                              dataset.Name, memberDetails[i].MemberDetail.MemberEnrolleeID);
        //            taskList.Add(t);
        //            threadCounter++;
        //        }

        //        if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
        //        {
        //            await Task.WhenAll(taskList);

        //            foreach (Task<List<MemberDetailsMemberDiagnosisDTO>> task in taskList)
        //            {
        //                if (task.Exception != null)
        //                {
        //                    Exception ex = task.Exception;
        //                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
        //                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
        //                }
        //                else
        //                {
        //                    // Technically don't have to await since we know it's done, but doing it just to be safe
        //                    List<MemberDetailsMemberDiagnosisDTO> taskResult = await task.ConfigureAwait(false);
        //                    finalStateTaskList.AddRange(taskResult);
        //                }
        //            }

        //            threadCounter = 1;
        //            taskList.Clear();
        //        }
        //    }

        //    var diagnoses = finalStateTaskList.GroupBy(x => new { x.Client, x.MemberEnrolleeID }).ToList();

        //    memberDetails.ForEach(x =>
        //    {
        //        int index = diagnoses.FindIndex(y => y.Key.Client == x.Client && y.Key.MemberEnrolleeID == x.MemberDetail.MemberEnrolleeID);
        //        if (index > -1)
        //        {
        //            x.HealthProfile = diagnoses[index].ToList();
        //        }

        //    });

        //    return memberDetails;
        //}

        //private async Task<Tuple<List<MemberSearchDTO>, int>> memberSearchClientDatasets(List<DatasetDTO> datasets, int concurrentThreadCount, string memberId,
        //                                                                     SearchTypeOperator? memberIdOperator, string lastName,
        //                                                                     SearchTypeOperator? lastNameOperator, string firstName,
        //                                                                     SearchTypeOperator? firstNameOperator, DateTime? dateOfBirth)
        //{
        //    List<Task<Tuple<List<MemberSearchDTO>, int>>> taskList = new List<Task<Tuple<List<MemberSearchDTO>, int>>>();
        //    List<Tuple<List<MemberSearchDTO>, int>> finalStateTaskList = new List<Tuple<List<MemberSearchDTO>, int>>();

        //    int threadCounter = 1;

        //    for (int i = 0; i < datasets.Count; i++)
        //    {
        //        if (threadCounter <= concurrentThreadCount)
        //        {
        //            Task<Tuple<List<MemberSearchDTO>, int>> t = MemberRepository.GetMemberSearchResults(MultipleConnectionsHelper.GetEProcareConnectionString(datasets[i].Path),
        //                                                                              datasets[i].Name, datasets[i].MemberIdType, memberId,
        //                                                                              DataValidationHelper.GetSearchTypeString(memberIdOperator), lastName,
        //                                                                              DataValidationHelper.GetSearchTypeString(lastNameOperator), firstName,
        //                                                                              DataValidationHelper.GetSearchTypeString(firstNameOperator), dateOfBirth,
        //                                                                 datasets[i].ParentIDs, datasets[i].OrganizationIDs, datasets[i].GroupIDs, datasets[i].PlanIDs);
        //            taskList.Add(t);
        //            threadCounter++;
        //        }

        //        if (threadCounter == concurrentThreadCount || i == datasets.Count - 1)
        //        {
        //            await Task.WhenAll(taskList);

        //            foreach (Task<Tuple<List<MemberSearchDTO>, int>> task in taskList)
        //            {
        //                if (task.Exception != null)
        //                {
        //                    Exception ex = task.Exception;
        //                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
        //                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, methodSource: "memberSearchClientDatasets").ConfigureAwait(false);
        //                }
        //                else
        //                {
        //                    // Technically don't have to await since we know it's done, but doing it just to be safe
        //                    Tuple<List<MemberSearchDTO>, int> taskResult = await task.ConfigureAwait(false);
        //                    finalStateTaskList.Add(taskResult);
        //                }
        //            }

        //            threadCounter = 1;
        //            taskList.Clear();
        //        }
        //    }

        //    Tuple<List<MemberSearchDTO>, int> finalResults = new Tuple<List<MemberSearchDTO>, int>(finalStateTaskList.SelectMany(x => x.Item1).ToList(), finalStateTaskList.Sum(x => x.Item2));

        //    return finalResults;
        //}

        //private static MemberSearchResult convertToMemberSearchResult(MemberSearchDTO x)
        //{
        //    MemberSearchResult result = x.ConvertTo<MemberSearchResult>();
        //    result.PlanEffectiveDate = x.PlanEffectiveDate.ConvertToDateString();
        //    result.DateOfBirth = x.DateOfBirth.ConvertToDateString();
        //    result.MemberEffectiveDate = x.MemberEffectiveDate.ConvertToDateString();
        //    result.MemberTerminationDate = x.MemberTerminationDate.ConvertToDateString();

        //    result.GroupEligibility = new GroupEligibility();
        //    result.GroupEligibility.GroupTerminationDate = x.GroupEligibility.GroupTerminationDate.ConvertToDateString();
        //    result.GroupEligibility.GroupEffectiveDate = x.GroupEligibility.GroupEffectiveDate.ConvertToDateString();
        //    result.GroupEligibility.PlanEffectiveDate = x.GroupEligibility.PlanEffectiveDate.ConvertToDateString();
        //    result.GroupEligibility.Active = x.GroupEligibility.Active;
        //    result.GroupEligibility.PlanId = x.GroupEligibility.PlanId;
        //    result.GroupEligibility.Flex1 = x.GroupEligibility.Flex1;

        //    //only include if populated
        //    if (!string.IsNullOrWhiteSpace(x.AdditionalEligibility.Flex2) || !string.IsNullOrWhiteSpace(x.AdditionalEligibility.Flex3))
        //    {
        //        result.AdditionalEligibility = new AdditionalEligibility();
        //        result.AdditionalEligibility.Flex2 = !string.IsNullOrWhiteSpace(x.AdditionalEligibility.Flex2) ? x.AdditionalEligibility.Flex2 : null;
        //        result.AdditionalEligibility.Flex3 = !string.IsNullOrWhiteSpace(x.AdditionalEligibility.Flex3) ? x.AdditionalEligibility.Flex3 : null;
        //    }
        //    else
        //    {
        //        result.AdditionalEligibility = null;
        //    }

        //    return result;
        //}

        //private static MemberDetailsResult convertToMemberDetailsResult(MemberDetailsResultDTO x)
        //{
        //    MemberDetailsResult result = x.ConvertTo<MemberDetailsResult>();

        //    x.HealthProfile.ForEach(y =>
        //    {
        //        MemberDetailsMemberDiagnosis convertedDiagnosis = y.ConvertTo<MemberDetailsMemberDiagnosis>();
        //        result.HealthProfile.Add(convertedDiagnosis);
        //    });

        //    result.MemberDetail.DateOfBirth = x.MemberDetail.DateOfBirth.ConvertToDateString();
        //    result.MemberDetail.OriginalFromDate = x.MemberDetail.OriginalFromDate.ConvertToDateString();

        //    result.AlternateInsurance.MedicareFromDate = x.AlternateInsurance.MedicareFromDate.ConvertToDateString();

        //    result.MemberCoverage.Status = x.MemberCoverage.Status ? "A" : "I";
        //    result.MemberCoverage.EffectiveDate = x.MemberCoverage.EffectiveDate.ConvertToDateString();
        //    result.MemberCoverage.TerminationDate = x.MemberCoverage.TerminationDate.ConvertToDateString();

        //    result.IDCard.CardDate = x.IDCard.CardDate.ConvertToDateString();

        //    result.PlanInfo.EffectiveDate = x.PlanInfo.EffectiveDate.ConvertToDateString();
        //    result.PlanInfo.TerminationDate = x.PlanInfo.TerminationDate.ConvertToDateString();

        //    result.MedicarePartDItems.FromDate = x.MedicarePartDItems.FromDate.ConvertToDateString();
        //    result.MedicarePartDItems.ToDate = x.MedicarePartDItems.ToDate.ConvertToDateString();
        //    result.MedicarePartDItems.CopayCategoryEffectiveDate = x.MedicarePartDItems.CopayCategoryEffectiveDate.ConvertToDateString();

        //    return result;
        //}
        #endregion
    }
}
