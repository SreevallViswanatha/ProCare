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

namespace ProCare.API.PBM.API
{
    public class MemberPhysicianLockServices : ServiceBase
    {
        #region Public Properties
        public IPbmRepository PBMRepository { get; set; }

        public IPrescriberRepository PrescriberRepository { get; set; }
        #endregion

        #region Public Methods
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberPhysicianLockUpdateRequest + "|" + ApiRoutes.MemberPhysicianLockUpdateRequest)]
        public async Task<MemberPhysicianLockUpdateResponse> Post(MemberPhysicianLockUpdateRequest request)
        {
            MemberPhysicianLockUpdateResponse response = new MemberPhysicianLockUpdateResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.MemberPhysicianLockUpdateRequest, async () =>
            {
                // Retrieve client connection string settings from configuration
                Dictionary<string, string> clientSetting = await ClientConfigHelper.GetClientConfigs(request.ClientGuid).ConfigureAwait(false);
                Dictionary<string, string> physreqDescriptions = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_PHYSREQ_Descriptions).ConfigureAwait(false);

                DatasetDTO dataset = ClientConfigHelper.GetClientDataset(clientSetting, request.Client, null, null, null, request.PlanID);

                string clientConnectionString = ClientConfigHelper.GetConnectionStringFromDataset(dataset);
                string userID = ClientConfigHelper.GetUserIDFromClientGuid(request.ClientGuid);
                string enrolleeId = await DataValidationHelper.GetEnrolleeId(PBMRepository, clientConnectionString, dataset.MemberIdType, request.PlanID, request.MemberID, request.Person, request.MemberEnrolleeID).ConfigureAwait(false);

                // Validate member is in plan
                if (!DataValidationHelper.IsValidMember(PBMRepository, clientConnectionString, request.PlanID, enrolleeId, out string errorMessage))
                {
                    throw new ArgumentException(errorMessage);
                }

                //Confirm APSENR.PHYSREQ matches MemberLockInStatus on request
                if (!await PBMRepository.TrySetMemberLockInStatus(clientConnectionString, request.PlanID, enrolleeId, request.MemberLockInStatus))
                {
                    throw new ArgumentException("Unable to set member lock-in status: Invalid status based on current setting.");
                }

                PhysicianDTO physician = await PrescriberRepository.GetPhysician(clientConnectionString, request.PhysicianNPI, request.PhysicianDEA).ConfigureAwait(false);


                string memberPhysicianLockSysid = await PBMRepository.MemberPhysicianLockExists(clientConnectionString,
                                                                            request.PlanID,
                                                                            enrolleeId,
                                                                            request.PhysicianNPI).ConfigureAwait(false);


                //Check if member already has a lock for this physician
                if (string.IsNullOrWhiteSpace(memberPhysicianLockSysid))
                {
                    // lock not found, so add it
                    memberPhysicianLockSysid = await PBMRepository.AddMemberPhysicianLock(clientConnectionString,
                                                                                        request.PlanID,
                                                                                        enrolleeId,
                                                                                        physician.NPI,
                                                                                        physician.DEA,
                                                                                        physician.FirstName,
                                                                                        physician.LastName,
                                                                                        request.EffectiveDate,
                                                                                        request.TerminationDate,
                                                                                        userID).ConfigureAwait(false);
                }
                else
                {
                    // Get the existing lock
                    MemberPhysicianLockDetailsResultDTO existingLock = await PBMRepository.GetMemberPhysicianLockDetails_BySysid(clientConnectionString,
                                                                                            request.Client,
                                                                                            memberPhysicianLockSysid).ConfigureAwait(false);

                    if (existingLock.TerminationDate.HasValue && request.TerminationDate.HasValue && request.TerminationDate != existingLock.TerminationDate)
                    {
                        throw new ArgumentException($"The submitted effective date ({request.EffectiveDate.Date}) and termination date ({request.TerminationDate?.Date}) does not match the existing effective date ({existingLock.EffectiveDate?.Date}) and termination date ({existingLock.TerminationDate?.Date}).");
                    }
                    else if (request.EffectiveDate != existingLock.EffectiveDate)
                    {
                        if (existingLock.TerminationDate == null)
                        {
                            throw new ArgumentException($"The submitted effective date ({request.EffectiveDate.Date}) does not match the existing effective date ({existingLock.EffectiveDate?.Date}).");
                        }
                        else if (existingLock.TerminationDate.HasValue && request.TerminationDate.HasValue && existingLock.TerminationDate == request.TerminationDate)
                        {
                            throw new ArgumentException($"The submitted effective date ({request.EffectiveDate.Date}) does not match the existing effective date ({existingLock.EffectiveDate?.Date}).");
                        }
                    }
                    else if (existingLock.TerminationDate.HasValue && request.TerminationDate == null)
                    {
                        // Reinstate the lock
                        PBMRepository.ReinstateMemberPhysicianLock(clientConnectionString,
                                                                    memberPhysicianLockSysid,
                                                                    request.EffectiveDate,
                                                                    userID);
                    }
                    else if (request.EffectiveDate == existingLock.EffectiveDate
                        && request.TerminationDate.HasValue
                        && existingLock.TerminationDate == null)
                    {
                        // Terminate the lock
                        PBMRepository.TerminateMemberPhysicianLock(clientConnectionString,
                                                                    memberPhysicianLockSysid,
                                                                    request.TerminationDate,
                                                                    userID);
                    }
                }

                // Retrieve results and populate response
                MemberPhysicianLockDetailsResultDTO memberInfo = await PBMRepository.GetMemberPhysicianLockDetails_BySysid(clientConnectionString, dataset.Name, memberPhysicianLockSysid).ConfigureAwait(false);

                // Check that info was loaded
                if (!string.IsNullOrWhiteSpace(memberInfo.ENRID))
                {
                    string physreq = string.IsNullOrWhiteSpace(memberInfo.PHYSREQ) ? "" : memberInfo.PHYSREQ.ToUpper();

                    response.Client = memberInfo.Client;
                    response.OrganizationID = memberInfo.ORGID;
                    response.GroupID = memberInfo.GRPID;
                    response.PlanID = request.PlanID.ToUpper().Trim();
                    response.MemberID = request.MemberID.ToUpper().Trim();
                    response.MemberEnrolleeID = memberInfo.ENRID;
                    response.PersonCode = request.Person.Trim();
                    response.MemberLockInStatus = $"{physreq} - {physreqDescriptions[physreq]}";
                    response.PhysicianLock = convertToMemberPhysicianLockPhysician(memberInfo);
                }
                else
                {
                    throw new ArgumentException("Failed to load member physician lock details after save; data was not loaded in a valid format.");
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }


        //[Authenticate]
        //[RequiresAnyPermission(ApiRoutes.MemberPhysicianLockDetailsRequest + "|" + ApiRoutes.MemberPhysicianLockDetailsRequest)]
        //public async Task<MemberPhysicianLockDetailsResponse> Get(MemberPhysicianLockDetailsRequest request)
        //{
        //    var response = new MemberPhysicianLockDetailsResponse();
        //    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

        //    response = await processRequest(request, ApiRoutes.MemberPhysicianLockDetailsRequest, async () =>
        //    {
        //        // Retrieve client connection string and maximum records settings from configuration
        //        Dictionary<string, string> clientSetting = await ClientConfigHelper.GetClientConfigs(request.ClientGuid).ConfigureAwait(false);

        //        List<DatasetDTO> datasets = ClientConfigHelper.GetClientDatasets(clientSetting, request.Client, null, null, null, request.PlanID);
        //        var datasetEnrolleeIds = await DataValidationHelper.GetDatasetEnrolleeIds(PBMRepository, datasets, request.PlanID, request.MemberID, request.Person).ConfigureAwait(false);

        //        if (datasetEnrolleeIds.Count() == 0)
        //        {
        //            throw new ArgumentException($"A Member {request.MemberID} with Person Code {request.Person} was not found in Plan {request.PlanID}");
        //        }

        //        // Retrieve results and populate response
        //        var config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_MultithreadDefaults);
        //        var maxSearchThreads = Convert.ToInt32(config[ConfigSetttingKey.MaxSearchThreads]);
        //        var results = await getMemberPhysicianLockDetails_ByMember(datasetEnrolleeIds, maxSearchThreads, request.PlanID, request.Person).ConfigureAwait(false);

        //        if (results.Count() > 0)
        //        {
        //            var memberInfo = results.First();
        //            var physreq = string.IsNullOrWhiteSpace(memberInfo.PHYSREQ) ? "" : memberInfo.PHYSREQ.ToUpper();
        //            var physreqDescriptions = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_PHYSREQ_Descriptions).ConfigureAwait(false);

        //            response.Client = memberInfo.Client;
        //            response.OrganizationID = memberInfo.ORGID;
        //            response.GroupID = memberInfo.GRPID;
        //            response.PlanID = request.PlanID.ToUpper().Trim();
        //            response.MemberID = request.MemberID.ToUpper().Trim();
        //            response.MemberEnrolleeID = memberInfo.ENRID;
        //            response.PersonCode = request.Person.Trim();
        //            response.MemberLockInStatus = $"{physreq} - {physreqDescriptions[physreq]}";

        //            var PhysicianLocks = new List<MemberPhysicianLockPhysician>();
        //            response.PhysicianLocks = PhysicianLocks;

        //            if (memberInfo.EffectiveDate.HasValue)
        //            {
        //                results.ForEach(x =>
        //                {
        //                    var result = convertToMemberPhysicianLockPhysician(x);
        //                    PhysicianLocks.Add(result);
        //                });
        //            }
        //            else if (string.IsNullOrWhiteSpace(physreq))
        //            {
        //                throw new ArgumentException($"No physician lock records found for Member {request.MemberID} with Person Code {request.Person} in Plan {request.PlanID}");
        //            }
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Member information was not loaded in a valid format.");
        //        }

        //        return response;

        //    }).ConfigureAwait(false);

        //    return response;
        //}
        #endregion


        #region Private Methods
        private MemberPhysicianLockPhysician convertToMemberPhysicianLockPhysician(MemberPhysicianLockDetailsResultDTO x)
        {
            MemberPhysicianLockPhysician result = x.ConvertTo<MemberPhysicianLockPhysician>();
            result.PhysicianDEA = x.PHYDEA;
            result.PhysicianNPI = x.PHYNPI;

            return result;
        }

        //private async Task<List<MemberPhysicianLockDetailsResultDTO>> getMemberPhysicianLockDetails_ByMember(List<DatasetEnrolleeIDDTO> datasetEnrolleeIds, int concurrentThreadCount, string planId, string person)
        //{
        //    List<Task<List<MemberPhysicianLockDetailsResultDTO>>> taskList = new List<Task<List<MemberPhysicianLockDetailsResultDTO>>>();
        //    List<List<MemberPhysicianLockDetailsResultDTO>> finalStateTaskList = new List<List<MemberPhysicianLockDetailsResultDTO>>();

        //    int threadCounter = 1;

        //    for (int i = 0; i < datasetEnrolleeIds.Count; i++)
        //    {
        //        if (threadCounter <= concurrentThreadCount)
        //        {
        //            DatasetEnrolleeIDDTO datasetEnrolleeIdPair = datasetEnrolleeIds[i];
        //            DatasetDTO dataset = datasetEnrolleeIdPair.Dataset;
        //            string enrolleeId = datasetEnrolleeIdPair.EnrolleeID;
        //            Task<List<MemberPhysicianLockDetailsResultDTO>> t = PBMRepository.GetMemberPhysicianLockDetails_ByMember(MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path),
        //                                                                              dataset.Name, planId, enrolleeId, person,
        //                                                                              dataset.ParentIDs, dataset.OrganizationIDs, dataset.GroupIDs, dataset.PlanIDs);
        //            taskList.Add(t);
        //            threadCounter++;
        //        }

        //        if (threadCounter == concurrentThreadCount || i == datasetEnrolleeIds.Count - 1)
        //        {
        //            await Task.WhenAll(taskList);

        //            foreach (Task<List<MemberPhysicianLockDetailsResultDTO>> task in taskList)
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
        //                    List<MemberPhysicianLockDetailsResultDTO> taskResult = await task.ConfigureAwait(false);
        //                    finalStateTaskList.Add(taskResult);
        //                }
        //            }

        //            threadCounter = 1;
        //            taskList.Clear();
        //        }
        //    }

        //    List<MemberPhysicianLockDetailsResultDTO> finalResults = finalStateTaskList.SelectMany(x => x.Select(y => y)).ToList();

        //    return finalResults;
        //}

        #endregion
    }
}
