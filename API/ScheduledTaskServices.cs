using Newtonsoft.Json;
using ProCare.API.Claims.Messages.Request;
using ProCare.API.Claims.Messages.Response;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ProCare.API.Core.DTO;
using ProCare.API.Core.Requests;
using ProCare.API.Core.Responses;
using ProCare.API.PBM.Exceptions;
using ProCare.API.PBM.Helpers;
using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Request.ScheduledTask;
using ProCare.API.PBM.Messages.Response;
using ProCare.API.PBM.Messages.Response.ScheduledTask;
using ProCare.API.PBM.Repository;
using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.DTO.ScheduledTask;
using ProCare.API.PBM.Repository.Helpers;
using ProCare.Common;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using ProCare.Common.Data.SQL;

using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ProCare.API.Core.Requests.Enums;
using Enums = ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.API
{
    public class ScheduledTaskServices : Service
    {
        #region Public Properties
        /// <summary>
        /// Dependency Injected Automatically by ServiceStack
        /// </summary>
        public IDataAccessHelper DataHelper { get; set; }

        public IPreImportRepository PreImportRepository { get; set; }
        public IRetroLICSRepository RetroLICSRepository { get; set; }
        public IRTMRepository RtmRepository { get; set; }
        public IHospiceRepository HospiceRepository { get; set; }

        public IVerificationQueueRepository VerificationQueueRepository { get; set; }

        #endregion

        #region Private Variables

        private ICommonApiHelper _commonApiHelper;

        //private ICommonApiHelper _commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

        #endregion Private Variables

        public ScheduledTaskServices(ICommonApiHelper commonApiHelper)
        {
            _commonApiHelper = commonApiHelper;
        }

        #region Public Methods

        #region Brightree PreImport

        #region Enqueue

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ProcessBrightreeImportEnqueueActivePatientsTask + "|" + ApiRoutes.ProcessBrightreeImportEnqueueActivePatientsTask)]
        public async Task<ProcessBrightreeImportEnqueueActivePatientsTaskResponse> Post(ProcessBrightreeImportEnqueueActivePatientsTaskRequest request)
        {
            ProcessBrightreeImportEnqueueActivePatientsTaskResponse response = null;
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.ProcessBrightreeImportEnqueueActivePatientsTask, async () =>
            {
                var brightreeConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_BrightreeImport_APIConfig).ConfigureAwait(false);
                var activePatientConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_BrightreeImport_ActivePatientConfig).ConfigureAwait(false);

                brightreeConfig.Add("SecurityUsername", getBrightreeUsername(request));
                brightreeConfig.Add("APISecurityPassword", getBrightreePassword(request));

                int threadCount = int.Parse(activePatientConfig[Brightree.MaxConcurrentEnqueueThreadCount]);
                int patientPagedDataPageSize = int.Parse(activePatientConfig[Brightree.DatabaseAuditPageSize]);
                List<KeyValuePair<string, string>> parameters = getActivePatientsParameters(activePatientConfig);

                string routeName = "";
                if (request.LastTriggerTime != null)
                {
                    parameters.Remove(parameters.First(x => x.Key.Equals("PageSize")));
                    parameters.Add(new KeyValuePair<string, string>("?searchDate=", request.LastTriggerTime.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm")));
                    routeName = ConfigSetttingKey.BrightreeAPIRoute_ActivePatientsWithDateFilter;
                }
                else
                {
                    routeName = ConfigSetttingKey.BrightreeAPIRoute_ActivePatients;
                }

                List<PreImportResponse> processedPages = await runPatientEnqueueProcess(Enums.PreImportRecordType.ActivePatient, request.VendorID, brightreeConfig, routeName, parameters, threadCount, patientPagedDataPageSize).ConfigureAwait(false);
                response = new ProcessBrightreeImportEnqueueActivePatientsTaskResponse();

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ProcessBrightreeImportEnqueueInactivePatientsTask + "|" + ApiRoutes.ProcessBrightreeImportEnqueueInactivePatientsTask)]
        public async Task<ProcessBrightreeImportEnqueueInactivePatientsTaskResponse> Post(ProcessBrightreeImportEnqueueInactivePatientsTaskRequest request)
        {
            ProcessBrightreeImportEnqueueInactivePatientsTaskResponse response = null;
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            response = await processRequest(request, ApiRoutes.ProcessBrightreeImportEnqueueInactivePatientsTask, async () =>
            {
                var brightreeConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_BrightreeImport_APIConfig).ConfigureAwait(false);
                var inactivePatientConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_BrightreeImport_InactivePatientConfig).ConfigureAwait(false);

                brightreeConfig.Add("SecurityUsername", getBrightreeUsername(request));
                brightreeConfig.Add("APISecurityPassword", getBrightreePassword(request));

                int threadCount = int.Parse(inactivePatientConfig[Brightree.MaxConcurrentEnqueueThreadCount]);
                int patientPagedDataPageSize = int.Parse(inactivePatientConfig[Brightree.DatabaseAuditPageSize]);
                List<KeyValuePair<string, string>> parameters = getInactivePatientsParameters(inactivePatientConfig, request.LastTriggerTime, request.LastTriggerTime != null ? DateTime.Now : request.LastTriggerTime);

                string routeName = "";
                if (request.LastTriggerTime != null)
                {
                    routeName = ConfigSetttingKey.BrightreeAPIRoute_InactivePatientsWithDateFilter;
                }
                else
                {
                    routeName = ConfigSetttingKey.BrightreeAPIRoute_InactivePatients;
                }

                List<PreImportResponse> processedPages = await runPatientEnqueueProcess(Enums.PreImportRecordType.InactivePatient, request.VendorID, brightreeConfig, routeName, parameters, threadCount, patientPagedDataPageSize).ConfigureAwait(false);

                response = new ProcessBrightreeImportEnqueueInactivePatientsTaskResponse();
                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ProcessBrightreeImportEnqueueActivePatientsForMedicationTask + "|" + ApiRoutes.ProcessBrightreeImportEnqueueActivePatientsForMedicationTask)]
        public async Task<ProcessBrightreeImportEnqueueActivePatientsForMedicationTaskResponse> Post(ProcessBrightreeImportEnqueueActivePatientsForMedicationTaskRequest request)
        {
            ProcessBrightreeImportEnqueueActivePatientsForMedicationTaskResponse response = new ProcessBrightreeImportEnqueueActivePatientsForMedicationTaskResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            await processRequest(request, ApiRoutes.ProcessBrightreeImportEnqueueActivePatientsForMedicationTask, async () =>
            {
                await runPatientEnqueueProcessForMedication(request.LastTriggerTime, request.VendorID).ConfigureAwait(false);

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ProcessBrightreeImportEnqueueMedicationsTask + "|" + ApiRoutes.ProcessBrightreeImportEnqueueMedicationsTask)]
        public async Task<ProcessBrightreeImportEnqueueMedicationsTaskResponse> Post(ProcessBrightreeImportEnqueueMedicationsTaskRequest request)
        {
            ProcessBrightreeImportEnqueueMedicationsTaskResponse response = new ProcessBrightreeImportEnqueueMedicationsTaskResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            await processRequest(request, ApiRoutes.ProcessBrightreeImportEnqueueMedicationsTask, async () =>
            {
                await runMedicationsEnqueueProcess(request, commonApiHelper).ConfigureAwait(false);

                return response;

            }).ConfigureAwait(false);

            return response;
        }

        #region Netsmart PreImport

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ImportNetsmartMedicationsBatchTask + "|" + ApiRoutes.ImportNetsmartMedicationsBatchTask)]
        public async Task<ProcessNetsmartImportProcessMedicationsBatchTaskResponse> Post(ProcessNetsmartImportProcessMedicationsBatchTaskRequest request)
        {
            ProcessNetsmartImportProcessMedicationsBatchTaskResponse response = null;

            // Get the configuration settings            

            response = await processRequest(request, ApiRoutes.ImportNetsmartMedicationsBatchTask, async () =>
            {
                var config = await _commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_ScheduledTask_Netsmart_Medications_Config, 100000).ConfigureAwait(false);

                DecryptSetting(config, "OAuthToken.QueryParams.client_secret");

                var counts = await ProcessNetsmartMedicationsAsync(config);

                response = new ProcessNetsmartImportProcessMedicationsBatchTaskResponse(counts);

                return response;

            }).ConfigureAwait(false);

            return response;
        }
        #endregion

        #endregion

        #region Process

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ProcessBrightreeImportProcessActivePatientBatchTask + "|" + ApiRoutes.ProcessBrightreeImportProcessActivePatientBatchTask)]
        public async Task<ProcessBrightreeImportProcessActivePatientBatchTaskResponse> Post(ProcessBrightreeImportProcessActivePatientBatchTaskRequest request)
        {
            ProcessBrightreeImportProcessActivePatientBatchTaskResponse response = null;

            response = await processRequest(request, ApiRoutes.ProcessBrightreeImportProcessActivePatientBatchTask, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                var activePatientConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_BrightreeImport_ActivePatientConfig).ConfigureAwait(false);

                runProcessPatientProcess((int)Enums.PreImportRecordType.ActivePatient, request.VendorID, int.Parse(activePatientConfig[Brightree.ProcessingBatchSize]));

                response = new ProcessBrightreeImportProcessActivePatientBatchTaskResponse();

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ProcessBrightreeImportProcessInactivePatientBatchTask + "|" + ApiRoutes.ProcessBrightreeImportProcessInactivePatientBatchTask)]
        public async Task<ProcessBrightreeImportProcessInactivePatientBatchTaskResponse> Post(ProcessBrightreeImportProcessInactivePatientBatchTaskRequest request)
        {
            ProcessBrightreeImportProcessInactivePatientBatchTaskResponse response = null;

            response = await processRequest(request, ApiRoutes.ProcessBrightreeImportProcessInactivePatientBatchTask, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                var inactivePatientConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_BrightreeImport_InactivePatientConfig).ConfigureAwait(false);

                runProcessPatientProcess((int)Enums.PreImportRecordType.InactivePatient, request.VendorID, int.Parse(inactivePatientConfig[Brightree.ProcessingBatchSize]));

                response = new ProcessBrightreeImportProcessInactivePatientBatchTaskResponse();

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ProcessBrightreeImportProcessMedicationsBatchTask + "|" + ApiRoutes.ProcessBrightreeImportProcessMedicationsBatchTask)]
        public async Task<ProcessBrightreeImportProcessMedicationBatchTaskResponse> Post(ProcessBrightreeImportProcessMedicationBatchTaskRequest request)
        {
            ProcessBrightreeImportProcessMedicationBatchTaskResponse response = null;

            response = await processRequest(request, ApiRoutes.ProcessBrightreeImportProcessActivePatientBatchTask, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                var medicationsConfig = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_BrightreeImport_MedicationsConfig).ConfigureAwait(false);

                runMedicationsProcess((int)Enums.PreImportRecordType.Medication, request.VendorID, int.Parse(medicationsConfig[Brightree.ProcessingBatchSize]));

                response = new ProcessBrightreeImportProcessMedicationBatchTaskResponse();

                return response;
            }).ConfigureAwait(false);

            return response;
        }
        #endregion

        #endregion

        #region Retro LICS Reprocess
        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.RetroLICSReprocessBatchTaskRequest + "|" + ApiRoutes.RetroLICSReprocessBatchTaskRequest)]
        public async Task<RetroLICSReprocessBatchTaskResponse> Post(RetroLICSReprocessBatchTaskRequest request)
        {
            RetroLICSReprocessBatchTaskResponse response = null;
            DateTime startTime = DateTime.Now;

            response = await processRequest(request, ApiRoutes.RetroLICSReprocessBatchTaskRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                Dictionary<string, string> retroLICSReprocessConfigs = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_RetroLICS_ReprocessConfigs).ConfigureAwait(false);

                int maxBatchSize = int.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.BatchSize]);
                string adsConnectionString = "";

                try
                {
                    adsConnectionString = getEprocareConnectionString(commonApiHelper, request.HostUrl);
                }
                catch (Exception ex)
                {
                    //Need notification that no records were found
                    var rlpex = new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.InvalidHostURL,
                                                                 "BATCH PROCESS",
                                                                 "Confirm that that PBM API setting exists, eProCare is sending the host correctly, and that both are in the correct format.  The format for host URLs is XXX.procarerx.com, and they should not include http/https or an ending slash.",
                                                                 "The eProCare host URL provided is not valid.");

                    RetroLICSSummaryEmailHelper.SendBatchErrorEmail(request.RequestingUserEmail, startTime, rlpex, retroLICSReprocessConfigs);

                    throw ex;
                }

                if (RetroLICSRepository.TryLockRetroLICSBatch(adsConnectionString))
                {
                    List<RetroLICSMemberDTO> retroLicsRecords = RetroLICSRepository.GetRetroLICSBatch(adsConnectionString);

                    if (retroLicsRecords.Any())
                    {
                        //Group into batches of MaxBatchSize
                        //Trying out sending the whole list instead of pre-batching them
                        List<List<RetroLICSMemberDTO>> processingBatches = getProcessingBatches(retroLicsRecords, retroLicsRecords.Count);
                        //List<List<RetroLICSMemberDTO>> processingBatches = getProcessingBatches(retroLicsRecords, maxBatchSize);

                        int batchCount = processingBatches.Count();
                        int currentBatch = 1;

                        processingBatches.ForEach(batch =>
                        {
                            try
                            {
                                //Call processing API
                                processRetroLicsReprocessBatch(request.HostUrl, request.StartDate, request.RequestingUserEmail,
                                                               batch.Select(x => x.ENRID).ToList(), currentBatch, batchCount);

                                currentBatch++;
                            }
                            catch (Exception ex)
                            {
                                //Now sending emails for individual batch failures, this should avoid the mystery batch stops
                                var rlpex = new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.APSRLMNotFound,
                                                                     "BATCH PROCESS",
                                                                     $"Message: {ex.Message}" + "<br/><br/>" +
                                                                     "ENRIDs in batch: " + "<br/><br/>" +
                                                                      string.Join("<br/>", batch.Select(x => x.ENRID).ToList()) + "<br/><br/>",
                                                                     "A Retro LICS batch failed.");

                                RetroLICSSummaryEmailHelper.SendBatchErrorEmail(request.RequestingUserEmail, startTime, rlpex, retroLICSReprocessConfigs);

                                //Log the error but continue processing the batches
                                Dictionary<string, string> exceptionIdentifiers =
                                    new Dictionary<string, string> { { "ApiRequestID", Request.Items["ApiMessageID"].ToString() } };
                                commonApiHelper.LogException(false, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace,
                                                             exceptionIdentifiers, ApiRoutes.RetroLICSReprocessBatchTaskRequest)
                                               .ConfigureAwait(false);
                            }
                        });
                    }
                    else
                    {
                        //Need notification that no records were found
                        var rlpex = new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.APSRLMNotFound,
                                                                     "BATCH PROCESS",
                                                                     "Verify that APSRLM records exist and are in PROCESSED = 'N' state.  All records may have completed processing or may currently be in-progress.",
                                                                     "No unprocessed APSRLM records could be locked for the batch.");

                        RetroLICSSummaryEmailHelper.SendBatchErrorEmail(request.RequestingUserEmail, startTime, rlpex, retroLICSReprocessConfigs);
                    }
                }
                else
                {
                    //Need to notify user that batch couldn't be run because one is already in progress
                    var rlpex = new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.APSRLMNotLocked,
                                                                 "BATCH PROCESS",
                                                                 "Batch could not be locked for processing.  Check APSRLM, records may need to be queued or a batch run may already be in progress.",
                                                                 "No APSRLM records could be locked for the batch.");

                    RetroLICSSummaryEmailHelper.SendBatchErrorEmail(request.RequestingUserEmail, startTime, rlpex, retroLICSReprocessConfigs);
                }


                response = new RetroLICSReprocessBatchTaskResponse();
                return response;
            }).ConfigureAwait(false);
            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.RetroLICSReprocessClaimsTaskRequest + "|" + ApiRoutes.RetroLICSReprocessClaimsTaskRequest)]
        public async Task<RetroLICSReprocessClaimsTaskResponse> Post(RetroLICSReprocessClaimsTaskRequest request)
        {
            RetroLICSReprocessClaimsTaskResponse response = null;
            DateTime startTime = DateTime.Now;
            DateTime? reprocessingStartDate = request.StartDate;

            if (!reprocessingStartDate.HasValue)
            {
                reprocessingStartDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            response = await processRequest(request, ApiRoutes.RetroLICSReprocessClaimsTaskRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                List<string> configKeys = new List<string> { ConfigSetttingKey.PBMAPI_RetroLICS_ClaimsAPI_ConnectionInfo, ConfigSetttingKey.PBMAPI_RetroLICS_ReprocessConfigs };
                Dictionary<string, string> retroLICSReprocessConfigs = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_RetroLICS_ReprocessConfigs).ConfigureAwait(false);

                var configurations = commonApiHelper.GetConfigurations(configKeys).Result;
                string adsConnectionString = getEprocareConnectionString(commonApiHelper, request.HostUrl);

                RetroLICSProcessingConfigDTO processingConfig = new RetroLICSProcessingConfigDTO
                {
                    WaitSeconds = int.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_WaitSeconds]),
                    ClaimFilePath = retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_ClaimFilePath],
                    SyncAccumulators = bool.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_SyncAccumulators]),
                    ErrorOnNonZeroAccums = bool.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_ErrorOnNonZeroAccums]),
                    DeleteClaimFilesOnCompletion = bool.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_DeleteClaimFilesOnCompletion]),
                    WriteLastCompletionTime = bool.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_WriteLastCompletionTime]),
                    BeginAfterLastCompletionTime = bool.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_BeginAfterLastCompletionTime]),
                    ClaimApiCredentials = GetConfiguration(configurations, ConfigSetttingKey.PBMAPI_RetroLICS_ClaimsAPI_ConnectionInfo),
                    ConcurrentThreadCount = int.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_MaxThreadCount]),
                    StartDate = reprocessingStartDate.Value
                };

                List<RetroLICSProcessOutputDTO> processingResults = await processRetroLICSReprocessClaims(adsConnectionString,
                                                      request.EnrolleeIDList,
                                                      processingConfig
                                                     ).ConfigureAwait(false);

                if (request.EnrolleeIDList.Count == 1)
                {
                    RetroLICSSummaryEmailHelper.SendRetroLICSSummaryEmail(
                                              retroLICSReprocessConfigs[ConfigSetttingKey.SMTPServer],
                                              retroLICSReprocessConfigs[ConfigSetttingKey.SMTPPort],
                                              retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailFromName],
                                              retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailCopyToName],
                                              request.RequestingUserEmail,
                                              retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailSubjectTemplate],
                                              processingResults[0],
                                              startTime,
                                              false
                        );
                }
                else if (request.EnrolleeIDList.Count > 1)
                {
                    RetroLICSSummaryEmailHelper.SendRetroLICSBatchSummaryEmail(
                                              retroLICSReprocessConfigs[ConfigSetttingKey.SMTPServer],
                                              retroLICSReprocessConfigs[ConfigSetttingKey.SMTPPort],
                                              retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailFromName],
                                              retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailCopyToName],
                                              request.RequestingUserEmail,
                                              retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailSubjectTemplate],
                                              processingResults,
                                              startTime,
                                              request.CurrentBatch,
                                              request.TotalBatches);
                }

                response = new RetroLICSReprocessClaimsTaskResponse();
                return response;
            }).ConfigureAwait(false);
            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.RetroLICSGenerateClaimFilesTaskRequest + "|" + ApiRoutes.RetroLICSGenerateClaimFilesTaskRequest)]
        public async Task<RetroLICSGenerateClaimFilesTaskResponse> Post(RetroLICSGenerateClaimFilesTaskRequest request)
        {
            RetroLICSGenerateClaimFilesTaskResponse response = null;
            DateTime? processingStartDate = request.StartDate;

            if (!processingStartDate.HasValue)
            {
                processingStartDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            response = await processRequest(request, ApiRoutes.RetroLICSGenerateClaimFilesTaskRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                List<string> configKeys = new List<string> { ConfigSetttingKey.PBMAPI_ClaimsAPI_ConnectionInfo, ConfigSetttingKey.PBMAPI_RetroLICS_ReprocessConfigs };

                var configurations = commonApiHelper.GetConfigurations(configKeys).Result;
                var retroLICSReprocessConfigs = GetConfiguration(configurations, ConfigSetttingKey.PBMAPI_RetroLICS_ReprocessConfigs);

                string adsConnectionString = getEprocareConnectionString(commonApiHelper, request.HostUrl);

                List<Tuple<string, RetroLICSProcessingException>> processingResults = await ProcessRetroLICSGenerateClaimFiles(adsConnectionString,
                                                      request.EnrolleeIDList, retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_ClaimFilePath],
                                                      int.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_MaxThreadCount]),
                                                      processingStartDate.Value
                                                     ).ConfigureAwait(false);

                processingResults.ForEach(result =>
                {
                    RetroLICSProcessingResult processingResult = new RetroLICSProcessingResult
                    {
                        EnrolleeID = result.Item1
                    };

                    if (result.Item2 != null)
                    {
                        processingResult.ProcessingError = new RetroLICSProcessingError
                        {
                            Error = result.Item2?.ErrorReason,
                            StackTrace = result.Item2?.GetBaseException().StackTrace
                        };
                    }

                    response = new RetroLICSGenerateClaimFilesTaskResponse();
                    response.ProcessingResults.Add(processingResult);
                });

                return response;
            }).ConfigureAwait(false);
            return response;
        }


        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.RetroLICSRebuildAccumulatorsTaskRequest + "|" + ApiRoutes.RetroLICSRebuildAccumulatorsTaskRequest)]
        public async Task<RetroLICSRebuildAccumulatorsTaskResponse> Post(RetroLICSRebuildAccumulatorsTaskRequest request)
        {
            RetroLICSRebuildAccumulatorsTaskResponse response = null;
            DateTime defaultStartDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime defaultEndDate = new DateTime(DateTime.Now.Year, 12, 31);

            response = await processRequest(request, ApiRoutes.RetroLICSRebuildAccumulatorsTaskRequest, async () =>
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                List<string> configKeys = new List<string> { ConfigSetttingKey.PBMAPI_ClaimsAPI_ConnectionInfo, ConfigSetttingKey.PBMAPI_RetroLICS_ReprocessConfigs };

                var configurations = commonApiHelper.GetConfigurations(configKeys).Result;
                var retroLICSReprocessConfigs = GetConfiguration(configurations, ConfigSetttingKey.PBMAPI_RetroLICS_ReprocessConfigs);

                string adsConnectionString = getEprocareConnectionString(commonApiHelper, request.HostUrl);

                List<Tuple<string, List<string>, RetroLICSProcessingException>> processingResults = await ProcessRetroLICSRebuildAccumulators(adsConnectionString,
                                                      request.Enrollees, int.Parse(retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_MaxThreadCount]),
                                                      defaultStartDate, defaultEndDate, request.ErrorOnNonZero
                                                     ).ConfigureAwait(false);

                processingResults.ForEach(result =>
                {
                    RetroLICSAccumulatorRebuildResult rebuildResult = new RetroLICSAccumulatorRebuildResult
                    {
                        EnrolleeID = result.Item1,
                        NonZeroAccumulators = result.Item2
                    };

                    if (result.Item3 != null)
                    {
                        rebuildResult.ProcessingError = new RetroLICSProcessingError
                        {
                            Error = result.Item3.ErrorReason,
                            StackTrace = result.Item3.GetBaseException().StackTrace,
                            ErrorType = (int)result.Item3.ExceptionType
                        };
                    }

                    response = new RetroLICSRebuildAccumulatorsTaskResponse();
                    response.RebuildResults.Add(rebuildResult);
                });

                return response;
            }).ConfigureAwait(false);
            return response;
        }
        #endregion

        #region RTM

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ProcessSendRTMDataTask + "|" + ApiRoutes.ProcessSendRTMDataTask)]
        public async Task<ProcessSendRTMDataTaskResponse> Post(ProcessSendRTMDataTaskResquest request)
        {
            ProcessSendRTMDataTaskResponse response = new ProcessSendRTMDataTaskResponse();

            response = await processRequest(request, ApiRoutes.ProcessSendRTMDataTask, async () =>
            {
                string clientCode = request.ClientCode;
                string fieldsToPostConfigKey = $"{ConfigSetttingKey.PBMAPI_SendRTMData_FieldsToPost}.{clientCode}";
                string apiConfigKey = $"{ConfigSetttingKey.PBMAPI_SendRTMData_API}.{clientCode}";


                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                var configKeys = new List<string> { ConfigSetttingKey.PBMAPI_SendRTMData,
                                                    ConfigSetttingKey.PBMAPI_SendRTMData_Connections,
                                                    ConfigSetttingKey.PBMAPI_SendRTMData_FieldsToPost_Master,
                                                    fieldsToPostConfigKey,
                                                    apiConfigKey };

                List<Configuration> configurations = await commonApiHelper.GetConfigurations(configKeys).ConfigureAwait(false);
                var adsConnectionString = GetRtmConnectionString(configurations, clientCode);

                var masterRTMFields = GetConfiguration(configurations, ConfigSetttingKey.PBMAPI_SendRTMData_FieldsToPost_Master)["APSRTM-Fields"].Split(",").ToList();
                var masterMiscFields = GetConfiguration(configurations, ConfigSetttingKey.PBMAPI_SendRTMData_FieldsToPost_Master)["Misc-Fields"].Split(",").ToList(); ;

                var clientRTMFields = GetConfiguration(configurations, fieldsToPostConfigKey)["APSRTM-Fields"].Split(",").ToList();
                var clientMiscFields = GetConfiguration(configurations, fieldsToPostConfigKey)["Misc-Fields"].Split(",").ToList();

                var clientApiSettings = GetConfiguration(configurations, apiConfigKey);

                var enableRecordLogging = GetConfiguration(configurations, ConfigSetttingKey.PBMAPI_SendRTMData)["EnableRecordLogging"];

                IDataAccessHelper adsRtmDataHelper = new Common.Data.ADS.AdsHelper(adsConnectionString);
                RtmRepository = new RTMRepository(adsRtmDataHelper);

                await RtmRepository.ProcessSendRTMData(adsConnectionString, masterRTMFields, masterMiscFields, clientRTMFields, clientMiscFields, clientCode, enableRecordLogging, clientApiSettings).ConfigureAwait(false);

                return response;

            }).ConfigureAwait(false);

            return response;
        }



        #endregion

        #region Verification Queue
        [Authenticate]
        [RequiredPermission(ApiRoutes.ProcessDetectAndCreateNewEpisodeTask + "|" + ApiRoutes.ProcessDetectAndCreateNewEpisodeTask)]
        public async Task<ProcessDetectAndCreateNewEpisodeTaskResponse> Post(ProcessDetectAndCreateNewEpisodeTaskRequest request)
        {
            var response = new ProcessDetectAndCreateNewEpisodeTaskResponse();
            response = await processRequest(request, ApiRoutes.ProcessDetectAndCreateNewEpisodeTask, async () =>
            {
                var commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                
                try
                {
                    var clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.GlobalPBM_Settings_Key_Prefix + request.ClientGUID).ConfigureAwait(false);
                    var adsConnectionString = MultipleConnectionsHelper.GetADSConnectionString(clientSetting, request.ClientID);

                    if (!adsConnectionString.IsNullOrEmpty())
                    {
                        var config = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_VerificationQueueSettings + request.ClientID).ConfigureAwait(false);
                        var batchSize = Convert.ToInt32(config[ConfigSetttingKey.BatchSize]);
                        var staleQueueItemDurationInSeconds = Convert.ToInt32(config[ConfigSetttingKey.StaleQueueItemDurationInSeconds]);
                        var planIDs = config[ConfigSetttingKey.PlanIDs];

                        VerificationQueueRepository = new VerificationQueueRepository(new AdsHelper(adsConnectionString));
                        VerificationQueueRepository.DetectAndCreateEpisodes(adsConnectionString, batchSize, staleQueueItemDurationInSeconds, planIDs);
                    }
                    else
                    {
                        throw new Exception(Constants.ConnectionInfoNotFound);
                    }
                }
                catch(Exception ex)
                {
                    await commonApiHelper.LogException(false, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, null, ApiRoutes.ProcessDetectAndCreateNewEpisodeTask).ConfigureAwait(false);
                }

                return response;

            }).ConfigureAwait(false);

            return response;
        }
        #endregion

        #endregion

        #region Private Methods

        #region Shared
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
                await commonApiHelper.LogException(false, Core.Requests.Enums.ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, exceptionIdentifiers, apiRoute).ConfigureAwait(false);
                throw new Exception(FieldDescriptions.UnhandledExceptionError);
            }

            return response;
        }
        #endregion

        #region Brightree PreImport

        #region Enqueue

        private async Task<List<PreImportResponse>> runPatientEnqueueProcess(Enums.PreImportRecordType recordType, Enums.PreImportVendors vendorID, Dictionary<string, string> brightreeConfig, string routeName, List<KeyValuePair<string, string>> parameters, int threadCount, int patientPagedDataPageSize)
        {
            List<string> businessUnitGuids = await getBusinessUnitGuidsToCheck(brightreeConfig).ConfigureAwait(false);
            Guid batchGuid = Guid.NewGuid();

            List<PreImportResponse> brightreeResponses = await processBrightreeAPICalls(threadCount, brightreeConfig[Brightree.BaseURL] + brightreeConfig[routeName], parameters,
                                                                                        brightreeConfig, businessUnitGuids).ConfigureAwait(false);

            processBrightreeResponsesForPatientPagedData(vendorID, recordType, brightreeResponses, batchGuid, patientPagedDataPageSize);

            return brightreeResponses;
        }

        private async Task<List<long>> runPatientEnqueueProcessForMedication(DateTime? medLastPullTime, Enums.PreImportVendors vendorID)
        {
            List<long> medPatientQueueIDs = await Task.Run(() => PreImportRepository.BatchInsertMedPatientQueueRecords(medLastPullTime, (long)vendorID)).ConfigureAwait(false);

            return medPatientQueueIDs;
        }

        private async Task<List<BrightreeApiResponse>> runMedicationsEnqueueProcess(ProcessBrightreeImportEnqueueMedicationsTaskRequest request, CommonApiHelper commonApiHelper)
        {
            List<BrightreeApiResponse> brightreeApiResponses = new List<BrightreeApiResponse>();

            List<string> configKeys = new List<string> { ConfigSetttingKey.PBMAPI_BrightreeImport_APIConfig, ConfigSetttingKey.PBMAPI_BrightreeImport_MedicationsConfig };
            List<Configuration> configurations = await commonApiHelper.GetConfigurations(configKeys).ConfigureAwait(false);

            var apiSettings = configurations.First(x => x.ConfigurationKey == ConfigSetttingKey.PBMAPI_BrightreeImport_APIConfig).ConfigurationValues;
            var medSettings = configurations.First(x => x.ConfigurationKey == ConfigSetttingKey.PBMAPI_BrightreeImport_MedicationsConfig).ConfigurationValues;

            apiSettings.Add("SecurityUsername", getBrightreeUsername(request));
            apiSettings.Add("APISecurityPassword", getBrightreePassword(request));

            //--Get patients for medications
            var patients = PreImportRepository.GetMedPatientQueueBatch(medSettings[Brightree.ActivePatientsBatchSize].ToInt(), (long)request.VendorID);

            if (patients.Count > 0)
            {
                //--Update MedPatientQueue Status to “In Process” 
                patients.ForEach(patient => { patient.ProcessStatusID = (int)Enums.PreImportProcessStatus.InProcess; });
                PreImportRepository.BatchUpdateMedPatientQueueStatus(patients);

                //--Call Brightree API
                brightreeApiResponses = await ProcessMedicationsBrightreeApiCalls(patients, apiSettings, medSettings).ConfigureAwait(false);

                //--Add responses to MedPagedData
                List<long> medPagedDataRecordIDs = processBrightreeResponsesForMedPagedData(request.VendorID,
                                                         brightreeApiResponses,
                                                         medPagedDataPageSize: medSettings[Brightree.DatabaseAuditPageSize].ToInt());
                //--Queue Medications
                PreImportRepository.BatchInsertMedQueueRecords(medPagedDataRecordIDs);

                //--Update MedPatientQueue status to “Complete” or "Failed"
                processMedPatientQueueStatus(patients, brightreeApiResponses);
            }

            return brightreeApiResponses;
        }

        #region Prepare API Parameters

        private List<KeyValuePair<string, string>> getActivePatientsParameters(Dictionary<string, string> activePatientConfig)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("PageSize", activePatientConfig["APIPageSize"]));

            return parameters;
        }

        private List<KeyValuePair<string, string>> getInactivePatientsParameters(Dictionary<string, string> inactivePatientConfig, DateTime? startDate = null, DateTime? endDate = null)
        {
            int daysBackToPull = int.Parse(inactivePatientConfig["DaysToPull"]);
            if (startDate == null)
            {
                startDate = DateTime.Today.AddDays(daysBackToPull);
            }

            if (endDate == null)
            {
                endDate = DateTime.Today;
            }

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("StartDate", startDate.Value.ToUniversalTime().ToString("MM-dd-yyyy")));
            parameters.Add(new KeyValuePair<string, string>("EndDate", endDate.Value.ToUniversalTime().ToString("MM-dd-yyyy")));
            parameters.Add(new KeyValuePair<string, string>("PageSize", inactivePatientConfig["APIPageSize"]));

            return parameters;
        }

        private Dictionary<string, string> getQueryParameters(BrightreeApiRequest apiRequest)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("GUID", apiRequest.Guid);
            parameters.Add("PageNumber", apiRequest.PageNumber);

            if (apiRequest.PageSize != null)
            {
                parameters.Add("PageSize", apiRequest.PageSize);
            }
            if (apiRequest.SearchDate != null)
            {
                parameters.Add("?searchDate=", apiRequest.SearchDate.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm"));
            }

            return parameters;
        }

        private List<KeyValuePair<string, string>> getAccessTokenParameters(Dictionary<string, string> config)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("grant_type", "password"));
            parameters.Add(new KeyValuePair<string, string>("Username", config["SecurityUsername"]));
            parameters.Add(new KeyValuePair<string, string>("Password", config["APISecurityPassword"]));
            return parameters;
        }

        private string getBrightreePassword(PreImportBrightreeCredentialRequest request)
        {
            return decryptIfEncrypted(request.CredentialsEncrypted, request.APISecurityPassword);
        }

        private string getBrightreeUsername(PreImportBrightreeCredentialRequest request)
        {
            return decryptIfEncrypted(request.CredentialsEncrypted, request.SecurityUsername);
        }

        private string decryptIfEncrypted(bool isEncrypted, string value)
        {
            string decryptedValue = value;

            if (isEncrypted)
            {
                decryptedValue = ApplicationSettings.Decrypt(decryptedValue);
            }

            return decryptedValue;
        }
        #endregion

        #region Main Process
        private async Task<List<PreImportResponse>> processBrightreeAPICalls(int concurrentThreadCount, string url, List<KeyValuePair<string, string>> parameters, Dictionary<string, string> brightreeConfig, List<string> businessUnitGuids = null)
        {
            string accessToken = await getAccessToken(brightreeConfig).ConfigureAwait(false);

            List<PreImportResponse> finalStateTaskList = new List<PreImportResponse>();

            var tasks = businessUnitGuids.Select(i =>
            {
                var businessUnitParameters = parameters.CreateCopy();
                businessUnitParameters.Insert(0, new KeyValuePair<string, string>("BusUnitGuid", i));
                return processBusinessUnit(accessToken, concurrentThreadCount, url, businessUnitParameters,
                                                                                brightreeConfig);
            });

            foreach (Task<List<PreImportResponse>> resultGroup in tasks.Where(t => t.Exception == null))
            {
                // Technically don't have to await since we know it's done, but doing it just to be safe
                finalStateTaskList.AddRange(await resultGroup.ConfigureAwait(false));
            }

            return finalStateTaskList;
        }

        private async Task<List<BrightreeApiResponse>> ProcessMedicationsBrightreeApiCalls(List<PreImportMedPatientQueueDTO> patients, Dictionary<string, string> brightreeConfig, Dictionary<string, string> medicationsConfig)
        {
            string pageSize = null;
            int threadCounter = 1;
            string pageNumber = "1";
            int patientsCount = patients.Count;
            string endPoint = string.Empty;
            DateTime? medLastPullTime = null;
            string url = string.Empty;

            List<Task<BrightreeApiResponse>> taskList = new List<Task<BrightreeApiResponse>>();
            List<BrightreeApiResponse> apiResponses = new List<BrightreeApiResponse>();

            int concurrentThreadCount = int.Parse(medicationsConfig[Brightree.MaxConcurrentEnqueueThreadCount]);

            string accessToken = await getAccessToken(brightreeConfig).ConfigureAwait(false);

            for (int i = 0; i <= (patientsCount - 1); i++)
            {

                endPoint = ConfigSetttingKey.BrightreeAPIRoute_PatientMedicationsWithDateFilter;

                medLastPullTime = patients[i].MedLastPullTime;
                if (medLastPullTime == null)
                {
                    //Pull all patient medications
                    endPoint = ConfigSetttingKey.BrightreeAPIRoute_PatientMedications;
                    pageSize = medicationsConfig[Brightree.APIPageSize];
                }

                url = brightreeConfig[Brightree.BaseURL] + brightreeConfig[endPoint];

                if (threadCounter <= concurrentThreadCount)
                {
                    BrightreeApiRequest apiRequest = new BrightreeApiRequest
                    {
                        Guid = patients[i].PatientID,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        SearchDate = medLastPullTime,
                    };

                    Task<BrightreeApiResponse> t = processSingleBrightreeAPICall(patients[i].PreImportMedPatientQueueID, url, apiRequest, accessToken, brightreeConfig);
                    taskList.Add(t);
                    threadCounter++;
                }
                if (threadCounter == concurrentThreadCount || i == (patientsCount - 1))
                {
                    try
                    {
                        await Task.WhenAll(taskList);
                    }
                    catch { }

                    foreach (Task<BrightreeApiResponse> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, url?.Substring(33) + ex.Message, ex.StackTrace, methodSource: "ProcessMedicationsBrightreeApiCalls").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            BrightreeApiResponse queueItemResult = await task;
                            apiResponses.Add(queueItemResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return apiResponses;
        }

        private async Task<List<PreImportResponse>> processBusinessUnit(string providedAccessToken, int concurrentThreadCount, string url, List<KeyValuePair<string, string>> parameters, Dictionary<string, string> brightreeConfig)
        {
            string accessToken = providedAccessToken;
            List<Task<PreImportResponse>> taskList = new List<Task<PreImportResponse>>();
            List<PreImportResponse> finalStateTaskList = new List<PreImportResponse>();

            PreImportResponse response = await processSingleBrightreeAPICall(url, parameters.CreateCopy(), accessToken, 1, brightreeConfig).ConfigureAwait(false);

            int totalPages = getNumberOfBrightreeResponsePages(response);
            finalStateTaskList.Add(response);

            int threadCounter = 1;

            for (int i = 2; i <= totalPages; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<PreImportResponse> t = processSingleBrightreeAPICall(url, parameters.CreateCopy(), accessToken, i, brightreeConfig);
                    taskList.Add(t);
                    threadCounter++;
                }

                if (threadCounter == concurrentThreadCount || i == totalPages)
                {
                    try
                    {
                        await Task.WhenAll(taskList);
                    }
                    catch { }

                    foreach (Task<PreImportResponse> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, url?.Substring(33) + ex.Message, ex.StackTrace, methodSource: "processBusinessUnit").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            PreImportResponse brightreeResult = await task.ConfigureAwait(false);
                            finalStateTaskList.Add(brightreeResult);
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return finalStateTaskList;
        }

        private int getNumberOfBrightreeResponsePages(PreImportResponse response)
        {
            int totalPages = -1;

            if (response != null)
            {
                if (!string.IsNullOrEmpty(response?.Error))
                {
                    throw new Exception(response.Error);
                }
                else
                {
                    int totalNumberOfRecords = int.Parse(response.Total);
                    totalPages = totalNumberOfRecords / int.Parse(response.PageSize);
                    if (totalNumberOfRecords % int.Parse(response.PageSize) != 0)
                    {
                        totalPages++;
                    }
                }
            }

            return totalPages;
        }

        private void processMedPatientQueueStatus(List<PreImportMedPatientQueueDTO> patients, List<BrightreeApiResponse> brightreeApiResponses)
        {
            patients.ForEach(patient =>
            {
                if (brightreeApiResponses.Any(x => x.PreImportMedPatientQueueID == patient.PreImportMedPatientQueueID && x.Response != null && x.Response.Error == null))
                {
                    patient.ProcessStatusID = (int)Enums.PreImportProcessStatus.Completed;
                }
                else
                {
                    patient.ProcessStatusID = (int)Enums.PreImportProcessStatus.Failed;
                }
            });

            PreImportRepository.BatchUpdateMedPatientQueueStatus(patients);
        }

        #endregion

        #region Call Brightree API
        private async Task<PreImportResponse> processSingleBrightreeAPICall(string url, List<KeyValuePair<string, string>> parameters, string accessToken, int pageNumber, Dictionary<string, string> brightreeConfig)
        {
            PreImportResponse response = null;

            try
            {
                response = await callBrightreeAPI(url, parameters, accessToken, pageNumber).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex is AuthenticationException)
                {
                    //reauthenticate and reattempt if the token has expired
                    accessToken = await getAccessToken(brightreeConfig).ConfigureAwait(false);
                    response = await callBrightreeAPI(url, parameters, accessToken, pageNumber).ConfigureAwait(false);
                }
                else
                {
                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, url + ex.Message, ex.StackTrace, methodSource: "processSingleBrightreeAPICall").ConfigureAwait(false);
                    throw ex;
                }
            }

            return response;
        }

        private async Task<BrightreeApiResponse> processSingleBrightreeAPICall(long preImportMedPatientQueueID, string url, BrightreeApiRequest apiRequest, string accessToken, Dictionary<string, string> brightreeConfig)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            BrightreeApiResponse brightreeApi = new BrightreeApiResponse();

            try
            {
                brightreeApi.PreImportMedPatientQueueID = preImportMedPatientQueueID;
                brightreeApi.Request = apiRequest;
                brightreeApi.Response = await BrightreeHelper.ApiGet(url, getQueryParameters(apiRequest), accessToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex is AuthenticationException)
                {
                    //reauthenticate and reattempt if the token has expired
                    accessToken = await getAccessToken(brightreeConfig).ConfigureAwait(false);
                    brightreeApi.Request = apiRequest;
                    brightreeApi.Response = await BrightreeHelper.ApiGet(url, getQueryParameters(apiRequest), accessToken).ConfigureAwait(false);
                }
                else
                {
                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, url + ex.Message, ex.StackTrace, methodSource: "processSingleBrightreeAPICall").ConfigureAwait(false);
                    throw ex;
                }
            }

            return brightreeApi;
        }

        private Task<PreImportResponse> callBrightreeAPI(string url, List<KeyValuePair<string, string>> parameters, string accessToken, int pageNumber)
        {
            //The API call is particular about the order we send parameters in
            parameters.Insert(parameters.Count - 1, new KeyValuePair<string, string>("PageNumber", pageNumber.ToString()));
            return BrightreeHelper.ApiGet(url, parameters.ToDictionary(x => x.Key, x => x.Value), accessToken);
        }

        private async Task<string> getAccessToken(Dictionary<string, string> config)
        {
            string accessToken = string.Empty;

            string url = config[Brightree.BaseURL] + config[Brightree.AccessTokenRoute];
            string securityClientID = config[Brightree.SecurityClientID];
            string securitySecret = config[Brightree.SecuritySecret];

            List<KeyValuePair<string, string>> parameters = getAccessTokenParameters(config);

            AccessTokenResponse response = await BrightreeHelper.GetAccessToken(url, parameters, securityClientID, securitySecret).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response?.Error))
            {
                throw new Exception(response.Error);
            }
            else
            {
                accessToken = response?.Access_Token;
            }

            return accessToken;
        }

        private async Task<List<string>> getAllAccessibleBusinessUnitGuids(Dictionary<string, string> brightreeConfig)
        {
            string url = brightreeConfig["BaseURL"] + brightreeConfig["BusinessUnitsApiUserHasAccessRoute"];
            List<string> businessUnitGuids = new List<string>();

            string accessToken = await getAccessToken(brightreeConfig).ConfigureAwait(false); ;

            List<BusinessUnitResponse> response = await BrightreeHelper.ApiGetBusinessUnitGuids(url, new Dictionary<string, string>(), accessToken).ConfigureAwait(false);

            if (response.Count < 1)
            {
                throw new Exception("No business unit guids received from Brightree");
            }
            else
            {
                businessUnitGuids.AddRange(response.Select(x => x.Id.ToUpper()));
            }

            return businessUnitGuids;
        }

        private async Task<List<string>> getBusinessUnitGuidsToCheck(Dictionary<string, string> brightreeConfig)
        {
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            List<string> businessUnitGuidsWeCanAccess = await getAllAccessibleBusinessUnitGuids(brightreeConfig).ConfigureAwait(false);
            Dictionary<string, string> approvedGuids = await commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_BrightreeImport_APIBusinessUnitGUIDsToCheck).ConfigureAwait(false);
            List<string> businessUnitsToAttemptToProcess = JsonConvert.DeserializeObject<List<string>>(approvedGuids["Guids"]);
            List<string> businessUnitGuidsToCheck = businessUnitGuidsWeCanAccess.Intersect(businessUnitsToAttemptToProcess).ToList();

            return businessUnitGuidsToCheck;
        }
        #endregion

        #region Database
        private void processBrightreeResponsesForPatientPagedData(Enums.PreImportVendors vendorID, Enums.PreImportRecordType recordType, List<PreImportResponse> brightreeResponses, Guid batchGuid, int patientPagedDataPageSize)
        {
            if (brightreeResponses != null)
            {
                var apiPageDt = new DataTable();
                apiPageDt.Columns.Add("VendorID");
                apiPageDt.Columns.Add("BatchGuid");
                apiPageDt.Columns.Add("RecordTypeID");
                apiPageDt.Columns.Add("RawData");

                apiPageDt.Columns["BatchGuid"].DataType = System.Type.GetType("System.Guid");

                List<List<string>> groups = BrightreeHelper.GroupResponseItemsForDatabasePagedData(brightreeResponses.SelectMany(x => x.Items).ToArray(), patientPagedDataPageSize);

                groups.ForEach(responseGroup =>
                {
                    //Join records for PreImportDataPatientPagedData API page response logging
                    string rawData = string.Join(",", responseGroup);
                    apiPageDt.Rows.Add
                    (
                        (long)vendorID,
                        batchGuid,
                        (int)recordType,
                        rawData
                    );
                });

                List<long> PatientPagedDataRecordIDs = PreImportRepository.BatchInsertPatientPagedDataRecords(apiPageDt);
                PreImportRepository.BatchInsertPatientQueueRecords((int)recordType, PatientPagedDataRecordIDs);
            }
        }
        private List<long> processBrightreeResponsesForMedPagedData(Enums.PreImportVendors vendorID, List<BrightreeApiResponse> apiResponseList, int medPagedDataPageSize)
        {
            List<long> medPagedDataRecordIDs = new List<long>();
            if (apiResponseList != null)
            {
                var apiPageDt = new DataTable();
                apiPageDt.Columns.Add("PreImportMedPatientQueueID");
                apiPageDt.Columns.Add("VendorID");
                apiPageDt.Columns.Add("RawData");

                apiResponseList.ForEach(x =>
                {
                    if (x.Response?.Items.Length > 0)
                    {
                        string rawData = string.Join(",", x.Response.Items);
                        apiPageDt.Rows.Add
                        (
                            x.PreImportMedPatientQueueID,
                            (long)vendorID,
                            rawData
                        );
                    };
                });

                if (apiPageDt.Rows?.Count > 0)
                {
                    medPagedDataRecordIDs = PreImportRepository.BatchInsertMedPagedData(apiPageDt);
                }
            }
            return medPagedDataRecordIDs;
        }
        #endregion

        #endregion

        #region Process

        private void runProcessPatientProcess(int recordTypeID, Enums.PreImportVendors vendorID, int batchSize)
        {
            List<PreImportPatientQueueDTO> PatientPagedDataBatch = PreImportRepository.GetPatientQueueBatch(recordTypeID, (long)vendorID, batchSize);

            if (PatientPagedDataBatch.Count > 0)
            {
                PatientPagedDataBatch.ForEach(batchItem => { batchItem.ProcessStatusID = (int)Enums.PreImportProcessStatus.InProcess; });

                PreImportRepository.BatchUpdatePatientQueueStatus(PatientPagedDataBatch);

                DataTable referenceDt = parsePatientPagedDataBatchToReferenceBatch(ref PatientPagedDataBatch);

                PreImportRepository.BatchProcessChanges(referenceDt);

                PreImportRepository.BatchUpdatePatientQueueStatus(PatientPagedDataBatch);
            }
        }

        private void runMedicationsProcess(int recordTypeID, Enums.PreImportVendors vendorID, int batchSize)
        {
            List<PreImportMedQueueDTO> MedPagedDataBatch = PreImportRepository.GetMedQueueBatch(batchSize, (long)vendorID);

            if (MedPagedDataBatch.Count > 0)
            {
                MedPagedDataBatch.ForEach(batchItem => { batchItem.ProcessStatusID = (int)Enums.PreImportProcessStatus.InProcess; });

                PreImportRepository.BatchUpdateMedQueueStatus(MedPagedDataBatch);

                DataTable medData = parseMedPagedDateToMedData(ref MedPagedDataBatch);

                if (medData?.Rows.Count > 0)
                {
                    PreImportRepository.BatchProcessMedData(medData);
                }
                PreImportRepository.BatchUpdateMedQueueStatus(MedPagedDataBatch);
            }
        }

        private async Task<Dictionary<string, int>> ProcessNetsmartMedicationsAsync(Dictionary<string, string> config)
        {
            var response = new Dictionary<string, int>();

            // Get the Medication Queue batch list to process
            List<PreImportNetsmartMedQueueDTO> batch = PreImportRepository.GetNetsmartMedQueueBatch(int.Parse(config["BatchSize"]));

            response.Add("Total", batch.Count);

            var successes = 0;
            var failures = 0;

            if (batch.Count > 0)
            {
                // Update the Medication Queue status to 1 (In Process) for the whole batch
                batch.ForEach(batchItem => { batchItem.ProcessStatusID = (int)Enums.PreImportProcessStatus.InProcess; });

                PreImportRepository.BatchUpdateNetsmartMedQueueStatus(batch);


                // Process each item individually
                foreach (var item in batch)
                {
                    long importId = 0;
                    try
                    {
                        // Retrieve Medications from Netsmart API for ExternalPatientId
                        var json = await GetPatientMedications(item.ExternalPatientID, config);

                        // Insert response into the Import table
                        importId = await HospiceRepository.InsertImportRecord(Constants.stateOfTheHeart, json).ConfigureAwait(false);

                        successes += 1;
                    }
                    catch (Exception ex)
                    {
                        // Log the issue
                        failures += 1;
                        CommonApiHelper helper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                        await helper.LogException(true, ApplicationSource.PBMAPI, ex.Message, ex.StackTrace, null, nameof(ProcessNetsmartImportProcessMedicationsBatchTaskRequest));
                    }
                    finally
                    {
                        // Update Medication Queue ImportId and Status
                        PreImportRepository.UpdateNetsmartMedQueue(item.PreImportNetsmartSOTHMedQueueID, (int)GetStatus(importId), importId);
                    }
                }
            }

            response.Add("Success", successes);
            response.Add("Fail", failures);

            return response;
        }

        private async Task<string> GetPatientMedications(int externalPatientID, Dictionary<string, string> config)
        {
            //get token
            var token = await GetAccessToken(config);

            //get Medications
            var parameters = new Dictionary<string, string>
            {
                { "patient", externalPatientID.ToString() }
            };

            var url = $"{config["BaseUrl.Application"]}{config["Routes.MedicationRequest"]}";

            return await NetsmartHelper.ApiGet(url, parameters, token);

        }

        private async Task<string> GetAccessToken(Dictionary<string, string> config)
        {
            string token = string.Empty;

            AccessTokenResponse response = await NetsmartHelper.GetAccessToken(config.ToList()).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response?.Error))
            {
                throw new Exception(response.Error);
            }
            else
            {
                token = response?.Access_Token;
            }

            return token;
        }

        private void DecryptSetting(Dictionary<string, string> config, string key)
        {
            if (config.ContainsKey(key))
            {
                config[key] = ApplicationSettings.Decrypt(config[key]);
            }
        }

        private static Enums.PreImportProcessStatus GetStatus(long Id)
        {
            if (Id > 0)
            {
                return Enums.PreImportProcessStatus.Completed;
            }
            else
            {
                return Enums.PreImportProcessStatus.Failed;
            }
        }

        private DataTable parsePatientPagedDataBatchToReferenceBatch(ref List<PreImportPatientQueueDTO> PatientPagedDataBatch)
        {
            var referenceDt = new DataTable();
            referenceDt.Columns.Add("VendorID");
            referenceDt.Columns.Add("RecordTypeID");
            referenceDt.Columns.Add("PatientID");
            referenceDt.Columns.Add("RawData");
            referenceDt.Columns.Add("PreImportPatientPagedDataID");

            PatientPagedDataBatch.ForEach(batchItem =>
            {
                try
                {
                    batchItem.RawData = "[" + batchItem.RawData + "]";
                    dynamic rawDataItems = JsonConvert.DeserializeObject(batchItem.RawData);

                    foreach (var rawDataItem in rawDataItems)
                    {
                        string recordIdentifier = string.Empty;
                        dynamic recordData = rawDataItem;

                        foreach (var item in recordData["identifier"])
                        {
                            if (((string)item["system"]).ToLower() == "admission guid")
                            {
                                recordIdentifier = item["value"];
                            }
                        }

                        referenceDt.Rows.Add
                        (
                            batchItem.VendorID,
                            batchItem.RecordTypeID,
                            recordIdentifier,
                            rawDataItem,
                            batchItem.PreImportPatientPagedDataID
                        );
                    }

                    batchItem.ProcessStatusID = (int)Enums.PreImportProcessStatus.Completed;
                }
                catch (Exception)
                {
                    //Mark as failed in PreImportPatientPatientQueue
                    batchItem.ProcessStatusID = (int)Enums.PreImportProcessStatus.Failed;
                }
            });

            return referenceDt;
        }

        private DataTable parseMedPagedDateToMedData(ref List<PreImportMedQueueDTO> medPagedDataBatch)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PreImportMedQueueID");
            dataTable.Columns.Add("VendorID");
            dataTable.Columns.Add("PatientID");
            dataTable.Columns.Add("OrderID");
            dataTable.Columns.Add("RawData");


            medPagedDataBatch.ForEach(batchItem =>
            {
                try
                {
                    batchItem.RawData = "[" + batchItem.RawData + "]";
                    dynamic rawDataItems = JsonConvert.DeserializeObject(batchItem.RawData);

                    foreach (var rawDataItem in rawDataItems)
                    {

                        //--Get Order ID
                        string order_Uid = string.Empty;
                        dynamic recordData = rawDataItem;
                        foreach (var item in recordData["identifier"])
                        {
                            if (((string)item["system"]).ToLower() == "order_uid")
                            {
                                order_Uid = item["value"];
                            }
                        }

                        dataTable.Rows.Add
                            (
                                 batchItem.PreImportMedQueueID,
                                 batchItem.VendorID,
                                 batchItem.PatientID,
                                 order_Uid,
                                 rawDataItem
                       );
                    }

                    batchItem.ProcessStatusID = (int)Enums.PreImportProcessStatus.Completed;
                }
                catch (Exception)
                {
                    batchItem.ProcessStatusID = (int)Enums.PreImportProcessStatus.Failed;
                }
            });

            return dataTable;
        }

        #endregion

        #endregion

        #region Retro LICS Reprocess

        private async Task<List<RetroLICSProcessOutputDTO>> processRetroLICSReprocessClaims(string adsConnectionString, List<string> enrolleeIds, RetroLICSProcessingConfigDTO processingConfig)
        {
            int threadCounter = 0;
            int enrolleeCount = enrolleeIds.Count;
            List<RetroLICSProcessOutputDTO> enrolleeClaims = new List<RetroLICSProcessOutputDTO>();

            List<Task<RetroLICSProcessOutputDTO>> taskList = new List<Task<RetroLICSProcessOutputDTO>>();
            List<Task<RetroLICSProcessOutputDTO>> completedTaskList = new List<Task<RetroLICSProcessOutputDTO>>();

            for (int i = 0; i < enrolleeCount; i++)
            {
                if (threadCounter <= processingConfig.ConcurrentThreadCount)
                {
                    threadCounter++;
                    string enrid = enrolleeIds[i];
                    Task <RetroLICSProcessOutputDTO> t = Task.Run(() => runRetroLICSReprocessClaimsProcess(adsConnectionString, enrid, processingConfig.ClaimFilePath + enrid + @"\", processingConfig));
                    taskList.Add(t); 
                }
                if (threadCounter == processingConfig.ConcurrentThreadCount || i == enrolleeCount - 1)
                {
                    try
                    {
                        if (i == enrolleeCount - 1)
                        {
                            //If all reprocs have been started, wait for all to finish
                            await Task.WhenAll(taskList);

                            completedTaskList.AddRange(taskList);
                            taskList.Clear();
                        }
                        else
                        {
                            //If we still have more reprocs to run, go start the next enrollee
                            await Task.WhenAny(taskList);

                            List<Task<RetroLICSProcessOutputDTO>> justCompleted = taskList.Where(x => x.IsCompleted).ToList();
                            completedTaskList.AddRange(justCompleted);
                            taskList.RemoveAll(x => justCompleted.Contains(x));

                            threadCounter -= justCompleted.Count;
                        }
                    }
                    catch (Exception){ }

                    //threadCounter = 0;
                    //taskList.Clear();
                }
            }

            if(taskList.Any())
            {
                completedTaskList.AddRange(taskList);
                taskList.Clear();
            }

            foreach (Task<RetroLICSProcessOutputDTO> task in completedTaskList)
            {
                if (task.Exception != null)
                {
                    Exception ex = task.Exception;

                    if (ex is RetroLICSProcessingException)
                    {
                        RetroLICSProcessingException rlpex = (RetroLICSProcessingException)ex;
                        enrolleeClaims.Add(
                            new RetroLICSProcessOutputDTO
                            {
                                EnrolleeID = rlpex.EnrolleeID,
                                ProcessingException = rlpex,
                                NonZeroAccums = new List<string>(),
                                MemberLocked = rlpex.MemberLocked
                            });
                    }
                    else if (ex is AggregateException)
                    {
                        AggregateException aex = (AggregateException)ex;
                        aex.Handle(ex1 =>
                        {
                            bool handled = false;

                            if (ex1 is RetroLICSProcessingException)
                            {
                                RetroLICSProcessingException rlpex = (RetroLICSProcessingException)ex1;
                                enrolleeClaims.Add(
                                    new RetroLICSProcessOutputDTO
                                    {
                                        EnrolleeID = rlpex.EnrolleeID,
                                        ProcessingException = rlpex,
                                        NonZeroAccums =
                                        rlpex.ExceptionType == RetroLICSProcessingException.RetroLICSProcessingExceptionType.NonZeroAccumulatorAfterReversals ?
                                        ((List<string>)rlpex.Data) :
                                        new List<string>(),
                                        MemberLocked = rlpex.MemberLocked
                                    });

                                handled = true;
                            }

                            return handled;
                        });
                    }

                    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                    await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ApiRoutes.RetroLICSReprocessClaimsTaskRequest, ex.StackTrace, methodSource: "ProcessRetroLICSReprocessClaims").ConfigureAwait(false);
                }
                else
                {
                    // Technically don't have to await since we know it's done, but doing it just to be safe
                    RetroLICSProcessOutputDTO reprocessingResult = await task.ConfigureAwait(false);
                    enrolleeClaims.Add(
                                            new RetroLICSProcessOutputDTO
                                            {
                                                EnrolleeID = reprocessingResult.EnrolleeID,
                                                NonZeroAccums = reprocessingResult.NonZeroAccums,
                                                MemberLocked = reprocessingResult.MemberLocked,
                                                ReverseClaimsExist = reprocessingResult.ReverseClaimsExist,
                                                MemberProcessingAlreadyLocked = reprocessingResult.MemberProcessingAlreadyLocked
                                            }
                                        );

                }
            }

            return enrolleeClaims;
        }

        private async Task<List<Tuple<string, RetroLICSProcessingException>>> ProcessRetroLICSGenerateClaimFiles(string adsConnectionString, List<string> enrolleeIds, string claimFilePath, int concurrentThreadCount, DateTime startDate)
        {
            int threadCounter = 1;
            int enrolleeCount = enrolleeIds.Count;
            List<Tuple<string, RetroLICSProcessingException>> enrolleeClaims = new List<Tuple<string, RetroLICSProcessingException>>();

            List<Task<string>> taskList = new List<Task<string>>();

            for (int i = 0; i < enrolleeCount; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<string> t = Task.Run(() => generateClaimFiles(adsConnectionString, enrolleeIds[i], claimFilePath + enrolleeIds[i] + @"\", startDate));
                    taskList.Add(t);
                    threadCounter++;
                }
                if (threadCounter == concurrentThreadCount || i == enrolleeCount - 1)
                {
                    try
                    {
                        await Task.WhenAll(taskList);
                    }
                    catch (Exception ex1)
                    {
                        if (ex1 is RetroLICSProcessingException)
                        {
                            RetroLICSProcessingException rlpex = (RetroLICSProcessingException)ex1;
                            enrolleeClaims.Add(
                                new Tuple<string, RetroLICSProcessingException>(
                                    rlpex.EnrolleeID, rlpex));
                        }
                        else if (ex1 is AggregateException)
                        {
                            AggregateException aex = (AggregateException)ex1;
                            aex.Handle(ex =>
                            {
                                bool handled = false;

                                if (ex is RetroLICSProcessingException)
                                {
                                    RetroLICSProcessingException rlpex = (RetroLICSProcessingException)ex;
                                    enrolleeClaims.Add(
                                        new Tuple<string, RetroLICSProcessingException>(
                                            rlpex.EnrolleeID, rlpex));
                                    handled = true;
                                }

                                return handled;
                            });
                        }
                    }

                    foreach (Task<string> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ApiRoutes.RetroLICSReprocessClaimsTaskRequest, ex.StackTrace, methodSource: "ProcessRetroLICSReprocessClaims").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            string reprocessingResult = await task.ConfigureAwait(false);
                            enrolleeClaims.Add(
                                new Tuple<string, RetroLICSProcessingException>(
                                    reprocessingResult, null));
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return enrolleeClaims;
        }

        private async Task<List<Tuple<string, List<string>, RetroLICSProcessingException>>> ProcessRetroLICSRebuildAccumulators(string adsConnectionString, List<RetroLICSEnrollee> enrollees, int concurrentThreadCount, DateTime defaultStartDate, DateTime defaultEndDate, bool errorOnNonZero)
        {
            int threadCounter = 1;
            int enrolleeCount = enrollees.Count;
            List<Tuple<string, List<string>, RetroLICSProcessingException>> enrolleeClaims = new List<Tuple<string, List<string>, RetroLICSProcessingException>>();

            List<Task<Tuple<string, List<string>>>> taskList = new List<Task<Tuple<string, List<string>>>>();

            for (int i = 0; i < enrolleeCount; i++)
            {
                if (threadCounter <= concurrentThreadCount)
                {
                    Task<Tuple<string, List<string>>> t = Task.Run(() => rebuildAccumulators(adsConnectionString, enrollees[i].EnrolleeID, enrollees[i].StartDate ?? defaultStartDate, enrollees[i].EndDate ?? defaultEndDate, errorOnNonZero));
                    taskList.Add(t);
                    threadCounter++;
                }
                if (threadCounter == concurrentThreadCount || i == enrolleeCount - 1)
                {
                    try
                    {
                        await Task.WhenAll(taskList);
                    }
                    catch (Exception ex1)
                    {
                        if (ex1 is RetroLICSProcessingException)
                        {
                            RetroLICSProcessingException rlpex = (RetroLICSProcessingException)ex1;
                            enrolleeClaims.Add(
                                new Tuple<string, List<string>, RetroLICSProcessingException>(
                                    rlpex.EnrolleeID, ((List<string>) rlpex.Data) ?? new List<string>(), rlpex));
                        }
                        else if (ex1 is AggregateException)
                        {
                            AggregateException aex = (AggregateException)ex1;
                            aex.Handle(ex =>
                            {
                                bool handled = false;

                                if (ex is RetroLICSProcessingException)
                                {
                                    RetroLICSProcessingException rlpex = (RetroLICSProcessingException)ex;
                                    enrolleeClaims.Add(
                                        new Tuple<string, List<string>, RetroLICSProcessingException>(
                                            rlpex.EnrolleeID, new List<string>(), rlpex));
                                    handled = true;
                                }

                                return handled;
                            });
                        }
                    }

                    foreach (Task<Tuple<string, List<string>>> task in taskList)
                    {
                        if (task.Exception != null)
                        {
                            Exception ex = task.Exception;
                            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                            await commonApiHelper.LogException(true, Core.Requests.Enums.ApplicationSource.PBMAPI, ApiRoutes.RetroLICSReprocessClaimsTaskRequest, ex.StackTrace, methodSource: "ProcessRetroLICSReprocessClaims").ConfigureAwait(false);
                        }
                        else
                        {
                            // Technically don't have to await since we know it's done, but doing it just to be safe
                            Tuple<string, List<string>> reprocessingResult = await task.ConfigureAwait(false);
                            enrolleeClaims.Add(
                                new Tuple<string, List<string>, RetroLICSProcessingException>(
                                    reprocessingResult.Item1, reprocessingResult.Item2, null));
                        }
                    }

                    threadCounter = 1;
                    taskList.Clear();
                }
            }

            return enrolleeClaims;
        }

        private Tuple<string, List<string>> rebuildAccumulators(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate, bool errorOnNonZero)
        {
            try
            {
                var forcedZeroResult = forceAccumsToZero(adsConnectionString, enrolleeId, startDate, endDate, errorOnNonZero);

                DateTime syncDate = startDate;

                while (syncDate <= endDate)
                {
                    RetroLICSRepository.SyncAccumulatorAdjustments(adsConnectionString, enrolleeId, syncDate);
                    syncDate = syncDate.AddMonths(1);
                }

                return new Tuple<string, List<string>>(enrolleeId, forcedZeroResult.Item2);
            }
            catch (Exception ex)
            {
                if (ex is RetroLICSProcessingException)
                {
                    throw ex;
                }

                throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.UnexpectedError, enrolleeId,
                                                       ex.Message,
                                                       $"An unexpected error occurred when generating claim files for enrollee {enrolleeId}.");
            }
        }

        private Tuple<string, List<string>> forceAccumsToZero(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate, bool errorOnNonZero)
        {
            try
            {
                List<string> nonZeroAccums = getNonZeroAccums(adsConnectionString, enrolleeId, startDate, endDate);
                if (nonZeroAccums.Any())
                {
                    if (errorOnNonZero)
                    {
                        throw new RetroLICSProcessingException(
                            RetroLICSProcessingException.RetroLICSProcessingExceptionType.NonZeroAccumulatorAfterReversals,
                            enrolleeId, "Accumulators not 0 after completion of reversals",
                            $"Accumulators were not correctly zero'd out by reversals for enrollee {enrolleeId}.  The process is currently set to error if any accumulators for the benefit year are non-zero after reversals have completed.",
                            nonZeroAccums);

                    }

                    RetroLICSRepository.ForceAccumulatorsToZero(adsConnectionString, enrolleeId, startDate, endDate);

                    if (!RetroLICSRepository.VerifyAccumulatorsAreZero(adsConnectionString, enrolleeId, startDate, endDate))
                    {
                        throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.AccumulatorRebuildFailed,
                                                               enrolleeId, $"Check accumulators for enrollee for benefit year {startDate.Year}", $"Failed to force accumulators to zero for enrollee {enrolleeId}.", nonZeroAccums);
                    }
                }

                return new Tuple<string, List<string>>(enrolleeId, nonZeroAccums);
            }
            catch (Exception ex)
            {
                if (ex is RetroLICSProcessingException)
                {
                    throw ex;
                }

                throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.UnexpectedError, enrolleeId,
                                                       ex.Message,
                                                       $"An unexpected error occurred when generating claim files for enrollee {enrolleeId}.");
            }
        }

        private List<string> getNonZeroAccums(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate)
        {
            List<RetroLICSAccumulatorDTO> accums = RetroLICSRepository.GetAccumulatorValues(adsConnectionString, enrolleeId, startDate, endDate);

            return getNonZeroFieldNames(accums);
        }

        private List<string> getNonZeroFieldNames<T>(List<T> items)
        {
            List<string> nonZeroProperties = new List<string>();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (T item in items)
            {
                foreach (PropertyInfo property in properties)
                {
                    bool valueIsZero = true;

                    var propValue = property.GetValue(item, null);
                    if (propValue is int
                        || propValue is long
                        || propValue is float
                        || propValue is double
                        || propValue is decimal)
                    {
                        valueIsZero =
                        (
                            (propValue is int && ((int)propValue) == 0)
                            || (propValue is long && ((long)propValue) == 0)
                            || (propValue is float && ((float)propValue) == 0)
                            || (propValue is double && ((double)propValue) == 0)
                            || (propValue is decimal && ((decimal)propValue) == 0)
                        );
                    }

                    if (!valueIsZero)
                    {
                        nonZeroProperties.Add(property.Name);
                    }
                }
            }

            return nonZeroProperties.Distinct().ToList();
        }

        private string generateClaimFiles(string adsConnectionString, string enrolleeId, string claimFilePath, DateTime? startDate)
        {
            try
            {
                var enrolleeClaims = getEnrolleeClaims(adsConnectionString, enrolleeId, startDate.Value, new DateTime(startDate.Value.Year, 12, 31), false, null);
                loadCompoundInfo(adsConnectionString, ref enrolleeClaims);

                writeClaimFiles(ref enrolleeClaims, claimFilePath);

                return enrolleeId;
            }
            catch (Exception ex)
            {
                throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.UnexpectedError, enrolleeId,
                                                       ex.Message,
                                                       $"An unexpected error occurred when generating claim files for enrollee {enrolleeId}.");
            }
        }

        private RetroLICSProcessOutputDTO runRetroLICSReprocessClaimsProcess(string adsConnectionString, string enrolleeId, string claimFilePath, RetroLICSProcessingConfigDTO processingConfig)
        {
            RetroLICSProcessOutputDTO reprocessedClaims = new RetroLICSProcessOutputDTO();

            string step = "None";
            string apsrtoSysid = "";
            string newPlnId = "";
            string apsrlmSysid = "";
            bool memberProcessingAlreadyPaused = false;
            List<string> nonZeroAccums = new List<string>();
            bool memberLocked = false;
            bool unlockLockedMember = false;
            DateTime? processingStartDate = null,
                      processingEndDate = null;

            try
            {
                //Get APSRLM info from ADS
                var retroLicsMemberInfo = RetroLICSRepository.GetRetroLICSRecord(adsConnectionString, enrolleeId);
                apsrlmSysid = retroLicsMemberInfo.SYSID;
                step = "Get Member Info - Load APSRLM Record";

                processingStartDate = retroLicsMemberInfo.StartDate ?? processingConfig.StartDate;
                processingEndDate = processingStartDate.Value.Year == DateTime.Today.Year ?
                                        DateTime.Today.AddDays(1) :
                                        new DateTime(processingStartDate.Value.Year, 12, 31);

                if (!string.IsNullOrEmpty(retroLicsMemberInfo.ENRID))
                {
                    if (RetroLICSRepository.TryLockRetroLICSRecord(adsConnectionString, apsrlmSysid, enrolleeId))
                    {
                        //Check if we need to complete up a partial run that previously failed
                        apsrtoSysid =
                            RetroLICSRepository.CheckForPausedMemberClaimsProcessing(adsConnectionString, retroLicsMemberInfo.CARDID, retroLicsMemberInfo.CARDID2,
                                                                                     retroLicsMemberInfo.ENRID);

                        memberProcessingAlreadyPaused = !string.IsNullOrWhiteSpace(apsrtoSysid);
                        step = "Check Claims Processing Status - Determine If User Is Locked From Previous Failed Partial Run";

                        if (!memberProcessingAlreadyPaused)
                        {
                            //Add APSRTO record to stop claim processing for member
                            apsrtoSysid = RetroLICSRepository.PauseClaimsProcessing(adsConnectionString, retroLicsMemberInfo.CARDID, retroLicsMemberInfo.CARDID2,
                                                                                    retroLicsMemberInfo.ENRID);
                            step = "Pause Claims Processing - Write APSRTO Record";
                        }

                        memberLocked = true;

                        //Wait 10 seconds to ensure any claims currently processing finish
                        System.Threading.Thread.Sleep(1000 * processingConfig.WaitSeconds);
                        step = $"Wait {processingConfig.WaitSeconds} Seconds - Allow Current Claims To Safely Finish Processing";

                        DateTime? lastCompletionTime = null;

                        if (processingConfig.BeginAfterLastCompletionTime)
                        {
                            lastCompletionTime = RetroLICSRepository.GetEnrolleeLastFullReprocCompletionTime(adsConnectionString, enrolleeId, processingStartDate.Value.Year);
                        }

                        IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claimsToReprocess = getClaimsToReprocess(adsConnectionString, enrolleeId, processingStartDate.Value, processingEndDate.Value, ref step, ref unlockLockedMember, lastCompletionTime);

                        if (claimsToReprocess.Any())
                        {

                            List<DateTime> claimMonths = claimsToReprocess
                                                         .Select(x => new DateTime(x.First().FILLDT.Year, x.First().FILLDT.Month, 1)).Distinct()
                                                         .OrderBy(x => x).ToList();

                            //Check eligibility for each claim
                            checkAllEligibility(ref claimsToReprocess, processingConfig.ClaimApiCredentials);
                            step = "Check Eligibility - Ensure There Are No Gaps In Eligibility On Dates Of Claims To Be Reprocessed";

                            //Load additional Compound info after confirmation we are good to begin reprocessing
                            loadCompoundInfo(adsConnectionString, ref claimsToReprocess);
                            step = "Get Compound Info - Load Additional Ingredient Info If Product Is A Compound";

                            //Last group is all submissions for the most recent claim
                            //First is the original claim submission from that group
                            var mostRecentOriginalClaim = claimsToReprocess.Last().First();
                            newPlnId = mostRecentOriginalClaim.ReprocessingPLNID;

                            //Write claims to file
                            writeClaimFiles(ref claimsToReprocess, claimFilePath);
                            step = $"Write Claim Files - Export Claims To {claimFilePath} In Case Of Error";

                            //Reverse original claims
                            reverseClaims(ref claimsToReprocess, processingConfig.ClaimApiCredentials, ref unlockLockedMember);
                            step = "Reverse Claims - Reverse All Original Claims In Enrollee's Reprocess Batch";

                            DateTime month = processingStartDate.Value;
                            DateTime nextMonth = processingStartDate.Value.Year == DateTime.Today.Year ?
                                new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1) :
                                new DateTime(processingStartDate.Value.Year, 1, 1).AddYears(1);

                            //Make sure accumulators for the benefit year
                            var forcedAccumZeroResult = forceAccumsToZero(adsConnectionString, enrolleeId, processingStartDate.Value, nextMonth, processingConfig.ErrorOnNonZeroAccums);
                            nonZeroAccums = forcedAccumZeroResult.Item2;
                            step = "Sync Accumulators - Verify Accumulators Are Zero And Add Aggregated Adjustments";

                            RetroLICSClaimDTO lastClaim = null;
                            int accumsSinceZeroOut = 0;

                            //Iterate through each month of claims, syncing accumulators and then reprocessing the month
                            while (month.Date < nextMonth.Date)
                            {
                                var claimsForMonth = claimsToReprocess.Where(x => x.First().FILLDT >= month && x.First().FILLDT < month.AddMonths(1));

                                //Sync accumulators for the month
                                //Now that BYAADJ adjustments are being added into the main APSBYA record, we need to determine
                                //if the recalc already started for the month before adding them
                                if (processingConfig.SyncAccumulators && !monthRecalcAlreadyStarted(adsConnectionString, claimsForMonth, lastClaim, accumsSinceZeroOut))
                                {
                                    RetroLICSRepository.SyncAccumulatorAdjustments(adsConnectionString, enrolleeId, month);
                                    step = $"Sync Month Of Accumulators - Sync Accumulators For Month {month.Month} Of {month.Year}";
                                }

                                //Need to keep the count even if we aren't syncing the current month
                                if (RetroLICSRepository.MonthHasAccumUpdates(adsConnectionString, enrolleeId, month))
                                {
                                    accumsSinceZeroOut++;
                                }

                                if (claimMonths.Contains(month))
                                {
                                    //Submit claims under new plan with Retro LICS segment
                                    resubmitClaims(ref claimsForMonth, month, processingConfig.ClaimApiCredentials);
                                    step = $"Resubmit Month Of Claims - Resubmit Claims For Month {month.Month} Of {month.Year}";
                                }

                                if (claimsForMonth?.Count() > 0)
                                {
                                    lastClaim = claimsForMonth.Last().First();
                                }

                                month = new DateTime(month.Year, month.Month, 1).AddMonths(1);
                            }

                            verifySuccessfulReproc(adsConnectionString, enrolleeId, processingStartDate.Value, processingEndDate.Value, ref step, lastCompletionTime);

                            if(processingConfig.WriteLastCompletionTime)
                            {
                                RetroLICSRepository.UpdateEnrolleeLastFullReprocCompletionTime(adsConnectionString, enrolleeId, processingStartDate.Value.Year, DateTime.Now);
                            }
                        }

                        //Tilde out APSRTO record so claims can process again for member
                        RetroLICSRepository.ResumeClaimsProcessing(adsConnectionString, apsrtoSysid);
                        step = "Resume Claims Processing - Tilde Out APSRTO Record";
                        memberLocked = false;

                        //Set Processed = 'Y' in the APSRLM record
                        RetroLICSRepository.MarkRetroLICSMemberAsProcessed(adsConnectionString, apsrlmSysid, newPlnId, processingStartDate, processingEndDate, "", "");
                        step = "Resume Claims Processing - Tilde Out APSRTO Record";

                        if (processingConfig.DeleteClaimFilesOnCompletion)
                        {
                            removeClaimFiles(claimFilePath);
                            step = "Remove Claim Files - Remove Claim Files After Full Reprocess Has Completed";
                        }

                        //Return NDCREFs of claims
                        reprocessedClaims = new RetroLICSProcessOutputDTO
                        {
                            EnrolleeID = enrolleeId,
                            NonZeroAccums = nonZeroAccums,
                            MemberLocked = memberLocked,
                            ReverseClaimsExist = claimsToReprocess.Any(x => x.Any(y => y.ClaimStatus == ClaimStatus.Reversal)),
                            MemberProcessingAlreadyLocked = memberProcessingAlreadyPaused
                        };

                    }
                    else
                    {
                        throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.APSRLMNotLocked,
                                                               enrolleeId, "A run for this enrollee may currently be in-progress.  Check APSRLM.",
                                                               $"The APSRLM record for enrollee {enrolleeId} could not be locked.");
                    }
                }
                else
                {
                    throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.APSRLMNotFound,
                                                           enrolleeId, "Verify that the APSRLM record exists and is in PROCESSED = 'N' state.  All records for this enrollee may have completed processing or a run for this enrollee may currently be in-progress.",
                                                           $"An unprocessed APSRLM record for enrollee {enrolleeId} could not be found.");
                }
            }
            catch (Exception ex)
            {
                if (memberLocked && unlockLockedMember)
                {
                    RetroLICSRepository.ResumeClaimsProcessing(adsConnectionString, apsrtoSysid);
                    memberLocked = false;
                }

                if (ex is RetroLICSProcessingException)
                {
                    string errorType = "E";
                    RetroLICSProcessingException rlpex = (RetroLICSProcessingException) ex;

                    if (!memberProcessingAlreadyPaused
                        && rlpex.ExceptionType == RetroLICSProcessingException.RetroLICSProcessingExceptionType.GapInEligibility)
                    {
                        RetroLICSRepository.ResumeClaimsProcessing(adsConnectionString, apsrtoSysid);
                        memberLocked = false;
                    }

                    rlpex.MemberLocked = memberLocked;

                    if ((string.IsNullOrWhiteSpace(rlpex.ErrorReason) || !rlpex.ErrorReason.Contains("A task was canceled.")) &&
                        !string.IsNullOrWhiteSpace(rlpex.GetBaseException().Message) &&
                        (rlpex.ExceptionType == RetroLICSProcessingException.RetroLICSProcessingExceptionType.GapInEligibility ||
                        rlpex.ExceptionType == RetroLICSProcessingException.RetroLICSProcessingExceptionType.ClaimReversalFailed ||
                        rlpex.ExceptionType == RetroLICSProcessingException.RetroLICSProcessingExceptionType.ClaimSubmissionFailed))
                    {
                        errorType = "R";
                    }

                    string exceptionText = rlpex.GetBaseException().Message ?? "Unexpected error occurred";
                    exceptionText = exceptionText.Substring(0, exceptionText.Length > 120 ? 120 : exceptionText.Length);

                    RetroLICSRepository.MarkRetroLICSMemberAsProcessed(adsConnectionString, apsrlmSysid, newPlnId, processingStartDate, processingEndDate, exceptionText, errorType);

                    throw rlpex;
                }

                RetroLICSRepository.MarkRetroLICSMemberAsProcessed(adsConnectionString, apsrlmSysid, newPlnId, processingStartDate, processingEndDate, "Unexpected error occurred", "E");

                throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.UnexpectedError, enrolleeId,
                                                        ex.Message,
                                                        $"An unexpected error occurred when processing enrollee {enrolleeId}.  The last step completed successfully was {step}.",
                                                        null, memberLocked);
            }

            return reprocessedClaims;
        }

        private List<string> rebuildAccumulatorsForReprocess(string enrolleeId, DateTime startMonth, DateTime endMonth, bool errorOnNonZeroAccums)
        {
            RetroLICSRebuildAccumulatorsTaskRequest rebuildAccumRequest = new RetroLICSRebuildAccumulatorsTaskRequest
            {
                Enrollees = new List<RetroLICSEnrollee>
                {
                    new RetroLICSEnrollee
                    {
                        EnrolleeID = enrolleeId,
                        StartDate = startMonth,
                        EndDate = endMonth
                    }
                },
                ErrorOnNonZero = errorOnNonZeroAccums
            };

            var rebuildAccumResults = Post(rebuildAccumRequest).Result;
            var result = rebuildAccumResults.RebuildResults.Count > 0 ? rebuildAccumResults.RebuildResults[0] : new RetroLICSAccumulatorRebuildResult();

            if (result.ProcessingError != null || rebuildAccumResults.IsErrorResponse())
            {
                if (((RetroLICSProcessingException.RetroLICSProcessingExceptionType) result.ProcessingError.ErrorType)
                    == RetroLICSProcessingException.RetroLICSProcessingExceptionType.NonZeroAccumulatorAfterReversals)
                {
                    throw new RetroLICSProcessingException(
                        RetroLICSProcessingException.RetroLICSProcessingExceptionType.NonZeroAccumulatorAfterReversals,
                        enrolleeId,
                        $"Accumulators were not correctly zero'd out by reversals for enrollee {enrolleeId}.  The process is currently set to error if any accumulators for the benefit year are non-zero after reversals have completed.",
                        result.ProcessingError.Error,
                        result.NonZeroAccumulators);
                }
                else if (((RetroLICSProcessingException.RetroLICSProcessingExceptionType) result.ProcessingError.ErrorType)
                         == RetroLICSProcessingException.RetroLICSProcessingExceptionType.AccumulatorRebuildFailed)
                {
                    throw new RetroLICSProcessingException(
                        RetroLICSProcessingException.RetroLICSProcessingExceptionType.AccumulatorRebuildFailed,
                        enrolleeId,
                        $"Failed to force accumulators for enrollee {enrolleeId} to 0 when reversals did not zero them.",
                        result.ProcessingError.Error,
                        result.NonZeroAccumulators);
                }
                else
                {
                    throw new RetroLICSProcessingException(
                        RetroLICSProcessingException.RetroLICSProcessingExceptionType.UnexpectedError,
                        enrolleeId, rebuildAccumResults.ResponseStatus.Message,
                        $"Failed to rebuild accumulators for enrollee {enrolleeId}.");
                }
            }

            return result.NonZeroAccumulators;
        }

        private IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> getEnrolleeClaims(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate, bool forStatusVerification, DateTime? lastCompletionTime)
        {
            //Build list of claims to reprocess
            List<RetroLICSClaimDTO> fullClaimList = new List<RetroLICSClaimDTO>();

            if (forStatusVerification)
            {
                if(lastCompletionTime.HasValue)
                {
                    fullClaimList = RetroLICSRepository.GetClaimsForStatusVerification_FromLastCompletionTime(adsConnectionString, enrolleeId, startDate, endDate, lastCompletionTime.Value).OrderBy(x => x.FILLDT).ToList();
                }
                else
                {
                    fullClaimList = RetroLICSRepository.GetClaimsForStatusVerification(adsConnectionString, enrolleeId, startDate, endDate).OrderBy(x => x.FILLDT).ToList();
                }
            }
            else
            {
                if (lastCompletionTime.HasValue)
                {
                    fullClaimList = RetroLICSRepository.GetClaimsToReprocess_FromLastCompletionTime(adsConnectionString, enrolleeId, startDate, endDate, lastCompletionTime.Value).OrderBy(x => x.FILLDT).ToList();
                }
                else
                {
                    fullClaimList = RetroLICSRepository.GetClaimsToReprocess(adsConnectionString, enrolleeId, startDate, endDate).OrderBy(x => x.FILLDT).ToList();
                }
            }


            var enrolleeClaims = groupAndOrderClaims(fullClaimList);

            return enrolleeClaims;
        }

        private IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> groupAndOrderClaims(List<RetroLICSClaimDTO> claims)
        {
            return claims.GroupBy(x => new { x.RXNO, x.FILLDT, x.PHAID })
                         .Select(x => x.OrderBy(y => y.ProcessingTime)) //Order claims in group by processing date and time
                         .OrderBy(x => x.First().FILLDT);  //Order groups by fill date
        }

        private IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> orderClaims(List<List<RetroLICSClaimDTO>> claims)
        {
            return claims.Select(x => x.OrderBy(y => y.ProcessingTime)) //Order claims in group by processing date and time
                         .OrderBy(x => x.First().FILLDT);  //Order groups by fill date
        }

        private IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> getClaimsToReprocess(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate, ref string step, ref bool unlockLockedMember, DateTime? lastCompletionTime)
        {
            //Build list of claims to reprocess
            var claimsToReprocess = getEnrolleeClaims(adsConnectionString, enrolleeId, startDate, endDate, false, lastCompletionTime);
            step = $"Get Claims To Reprocess - Get All Paid Claims Starting From {startDate.ToShortDateString()}{(lastCompletionTime.HasValue ? $", Looking At Claims Processed Since {lastCompletionTime.Value.ToShortDateString()}" : "")}";

            //Don't include claim groups that end with a manual reversal, those need to stay reversed
            //If not removed, the reprocessor will treat them like partially-processed groups from a previous run
            removeManualReversals(ref claimsToReprocess);
            step = "Remove Manual Reversals - Ignore Claim Groups Where The Last Claim Is A Non-Retro LICS Reversal";

            //We only reprocess the latest claims from each group
            unlockLockedMember = removeFullReprocs(ref claimsToReprocess);
            step = "Remove Full Reprocs - Ignore Previous Successfully Completed Retro LICS Runs And Begin From Most Recent Submitted Claims";

            return claimsToReprocess;
        }

        private IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> getClaimsToReprocess(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate, DateTime? lastCompletionTime)
        {
            //Build list of claims to reprocess
            var claimsToReprocess = getEnrolleeClaims(adsConnectionString, enrolleeId, startDate, endDate, false, lastCompletionTime);

            //Don't include claim groups that end with a manual reversal, those need to stay reversed
            //If not removed, the reprocessor will treat them like partially-processed groups from a previous run
            removeManualReversals(ref claimsToReprocess);

            //We only reprocess the latest claims from each group
            removeFullReprocs(ref claimsToReprocess);

            return claimsToReprocess;
        }

        private void verifySuccessfulReproc(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate, ref string step, DateTime? lastCompletionTime)
        {
            if (!verifyByReprocessorGroupingLogic(adsConnectionString, enrolleeId, startDate, endDate, ref step, out string groupingError, lastCompletionTime))
            {
                throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.VerificationFailureByReprocGroupingCheck,
                                             enrolleeId, groupingError,
                                             $"The completed run verification by reprocessor grouping failed for enrollee {enrolleeId}");
            }

            if (!verifyByStatusLogic(adsConnectionString, enrolleeId, startDate, endDate, ref step, out string statusError, lastCompletionTime))
            {
                throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.VerificationFailureByStatusCheck,
                                                       enrolleeId, statusError,
                                                       $"The completed run verification by status failed for enrollee {enrolleeId}");
            }
        }

        private bool verifyByReprocessorGroupingLogic(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate, ref string step, out string error, DateTime? lastCompletionTime)
        {
            error = "";

            //Reload claims
            var claims = getClaimsToReprocess(adsConnectionString, enrolleeId, startDate, endDate, lastCompletionTime: lastCompletionTime);
            step = "Reload Claims - Reload Claims For Successful Run Verification By Reprocessor Grouping Logic";

            //Reproc is successful if all claim groups end in a paid claim
            //And no claim groups are showing a reversal in the current run
            bool successfulReproc = claims.All(x => x.Last().ClaimStatus == ClaimStatus.Paid);
            if (!successfulReproc)
            {
                var badClaims = claims.Where(x => x.Last().ClaimStatus != ClaimStatus.Paid)
                                      .Select(x => $"({x.Last().RXNO}, {x.Last().FILLDT}, {x.Last().PHAID})");

                error = $"Reloaded claims showing not in a paid state: {badClaims.Join(", ")}";
            }

            if (successfulReproc)
            {
                successfulReproc = claims.All(x => !x.Any(y => y.ClaimStatus == ClaimStatus.Reversal));
                if (!successfulReproc)
                {
                    var badClaims = claims.Where(x => x.Any(y => y.ClaimStatus == ClaimStatus.Reversal))
                                          .Select(x => $"({x.Last().RXNO}, {x.Last().FILLDT}, {x.Last().PHAID})");

                    error = $"Reloaded claims showing partial run: {badClaims.Join(", ")}";
                }
            }

            step = "Verify Successful Reproc Step 1 - Verify That The Run Was Successful By Reprocessor Grouping Logic";

            return successfulReproc;
        }

        private bool verifyByStatusLogic(string adsConnectionString, string enrolleeId, DateTime startDate, DateTime endDate, ref string step, out string error, DateTime? lastCompletionTime)
        {
            StringBuilder errorBuilder = new StringBuilder();
            error = "";

            //Load claims by status as backup verification
            var claims = getEnrolleeClaims(adsConnectionString, enrolleeId, startDate, endDate, true, lastCompletionTime);
            step = "Reload Claims - Reload Claims For Successful Run Verification By Status Logic";

            //Ignore claim groups that end with reversals that are not status 5
            claims = orderClaims(claims.Where(x => !(x.Last().ClaimStatus == ClaimStatus.Reversal &&
                                                   (x.Last().STATUS ?? "").Trim() != "5"))
                                       .Select(x => x.Select(y => y).ToList()).ToList());

            //Reproc is successful if the processing date and time of the last paid claim in the group
            //Is after the processing date and time of the last reversed claim in the group
            bool successfulReproc = claims.All(x => x.Any(y => y.ClaimStatus == ClaimStatus.Reversal) && x.Any(y => y.ClaimStatus == ClaimStatus.Paid) &&
                                                    x.Where(y => y.ClaimStatus == ClaimStatus.Reversal).Max(y => y.ProcessingTime) <
                                                    x.Where(y => y.ClaimStatus == ClaimStatus.Paid).Max(y => y.ProcessingTime));
            if (!successfulReproc)
            {
                var missingReversalClaims = claims.Where(x => !x.Any(y => y.ClaimStatus == ClaimStatus.Reversal))
                                          .Select(x => $"({x.Last().RXNO}, {x.Last().FILLDT}, {x.Last().PHAID})");

                var missingPaidClaims = claims.Where(x => !x.Any(y => y.ClaimStatus == ClaimStatus.Paid))
                                                  .Select(x => $"({x.Last().RXNO}, {x.Last().FILLDT}, {x.Last().PHAID})");

                var badClaimTimes = claims.Where(x => x.Any(y => y.ClaimStatus == ClaimStatus.Reversal) && x.Any(y => y.ClaimStatus == ClaimStatus.Paid) &&
                                                  x.Where(y => y.ClaimStatus == ClaimStatus.Reversal).Max(y => y.ProcessingTime) >=
                                                  x.Where(y => y.ClaimStatus == ClaimStatus.Paid).Max(y => y.ProcessingTime))
                                      .Select(x => $"({x.Last().RXNO}, {x.Last().FILLDT}, {x.Last().PHAID})");

                if (missingReversalClaims.Any())
                {
                    errorBuilder.AppendLine($"Status 5 claims missing reversals: {missingReversalClaims.Join(", ")}");
                }

                if (missingPaidClaims.Any())
                {
                    errorBuilder.AppendLine($"Status 5 claims missing paid claims: {missingPaidClaims.Join(", ")}");
                }

                if (badClaimTimes.Any())
                {
                    errorBuilder.AppendLine($"Status 5 claims showing last reversal processed after last paid claim: {badClaimTimes.Join(", ")}");
                }

                error = errorBuilder.ToString();
            }

            step = "Verify Successful Reproc Step 2 - Verify That The Run Was Successful By Status Logic";

            return successfulReproc;
        }

        private void removeManualReversals(ref IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claims)
        {
            claims = claims.Where(x => x.Last().SubmittedByRetroLICSReprocessor || x.Last().ClaimStatus == ClaimStatus.Paid)
                           .OrderBy(x => 1);
        }

        private bool removeFullReprocs(ref IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claims)
        {
            List<List<RetroLICSClaimDTO>> unorderedClaims = claims.Select(x => x.Select(y => y).ToList()).ToList();
            bool unlockLockedMember = false;

            if (unorderedClaims.Any())
            {
                //New run: All claims in paid state, get latest Paid claims and start fresh run
                if (unorderedClaims.All(x => x.Last().ClaimStatus == ClaimStatus.Paid))
                {
                    unlockLockedMember = true;
                    unorderedClaims = claims.Select(x => new List<RetroLICSClaimDTO> { x.Last() }).ToList();
                }
                else
                {
                    List<List<RetroLICSClaimDTO>>
                        filteredClaims = new List<List<RetroLICSClaimDTO>>();

                    RunType runType = claims.First().Last().ClaimStatus == ClaimStatus.Reversal
                        ? RunType.PartialFromReversalStep
                        : RunType.PartialFromResubmissionStep;

                    foreach (List<RetroLICSClaimDTO> claimGroup in unorderedClaims)
                    {
                        ClaimStatus currentStatus = claimGroup.Last().ClaimStatus;
                        filteredClaims.Add(getFilteredClaims(runType, currentStatus, claimGroup));
                    }

                    unorderedClaims = filteredClaims.CreateCopy();
                }
            }

            claims = orderClaims(unorderedClaims);

            return unlockLockedMember;
        }

        private List<RetroLICSClaimDTO> getFilteredClaims(RunType runType, ClaimStatus currentStatus, List<RetroLICSClaimDTO> claimList)
        {
            List<RetroLICSClaimDTO> filteredClaims = new List<RetroLICSClaimDTO>();
            int index = -1;

            switch (runType)
            {
                //Partial run: claims haven't begun resubmission step, get last Paid or Paid-Reversed
                case RunType.PartialFromReversalStep:
                {
                    if (currentStatus == ClaimStatus.Reversal)
                    {
                        index = getStartIndexOfLastReproc(claimList, true);
                        
                    }
                    else if (currentStatus == ClaimStatus.Paid)
                    {
                        index = claimList.Count - 1;
                    }

                        break;
                    }
                //Partial run: claims have begun resubmission step, get last Paid-Reversed or Paid-Reversed-Paid
                case RunType.PartialFromResubmissionStep:
                    {
                        if (currentStatus == ClaimStatus.Reversal)
                        {
                            index = getStartIndexOfLastReproc(claimList, true);
                        }
                        else if (currentStatus == ClaimStatus.Paid)
                        {
                            index = getStartIndexOfLastReproc(claimList);
                        }

                        break;
                    }
            }

            filteredClaims = claimList.GetRange(index, claimList.Count - index);

            return filteredClaims;
        }

        private void loadCompoundInfo(string adsConnectionString, ref IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claims)
        {
            foreach (var claim in claims)
            {
                var originalClaim = claim.First();
                List<CompoundInfoDTO> compoundInfo = new List<CompoundInfoDTO>();

                if (originalClaim.IsCompound)
                {
                    compoundInfo = RetroLICSRepository.GetClaimCompoundInfo(adsConnectionString, originalClaim.ClaimTable.ToString(), originalClaim.NDCREF);
                }

                claim.Each(x => { x.CompoundInfo = compoundInfo; });
            }
        }

        //Gets the index of the completion of the first reproc in the claim group
        private int getStartIndexOfFirstReproc(List<RetroLICSClaimDTO> claims)
        {
            int index = -1;

            if (claims.Count() >= 3)
            {
                index = getFirstIndexOfClaimStatus(claims, ClaimStatus.Paid);
                int checkIndex = index;
                checkIndex = getFirstIndexOfClaimStatus(claims, ClaimStatus.Reversal, checkIndex);
                checkIndex = getFirstIndexOfClaimStatus(claims, ClaimStatus.Paid, checkIndex);

                index = checkIndex == -1 ? checkIndex : index;
            }

            return index;
        }

        //Gets the index of the completion of the first reproc in the claim group
        private int getEndIndexOfFirstReproc(List<RetroLICSClaimDTO> claims)
        {
            int index = -1;

            if (claims.Count() >= 3)
            {
                index = getFirstIndexOfClaimStatus(claims, ClaimStatus.Paid);
                index = getFirstIndexOfClaimStatus(claims, ClaimStatus.Reversal, index);
                index = getFirstIndexOfClaimStatus(claims, ClaimStatus.Paid, index);
            }

            return index;
        }

        //Gets the index of the completion of the last reproc in the claim group
        private int getStartIndexOfLastReproc(List<RetroLICSClaimDTO> claims, bool partialReproc = false)
        {
            int index = -1;

            if (claims.Count() >= 3 || (partialReproc && claims.Count() >= 2))
            {
                index = claims.Count - 1;
                if (!partialReproc)
                {
                    index = getLastIndexOfClaimStatus(claims, ClaimStatus.Paid, index);
                }
                index = getLastIndexOfClaimStatus(claims, ClaimStatus.Reversal, index);
                index = getLastIndexOfClaimStatus(claims, ClaimStatus.Paid, index);
            }

            return index;
        }

        //Gets the index of the completion of the last reproc in the claim group
        private int getEndIndexOfLastReproc(IOrderedEnumerable<RetroLICSClaimDTO> claims)
        {
            int index = -1;

            if (claims.Count() >= 3)
            {
                var claimSearchList = claims.ToList();
                index = getLastIndexOfClaimStatus(claimSearchList, ClaimStatus.Paid, claimSearchList.Count - 1);

                int checkIndex = index;
                checkIndex = getLastIndexOfClaimStatus(claimSearchList, ClaimStatus.Reversal, checkIndex);
                checkIndex = getLastIndexOfClaimStatus(claimSearchList, ClaimStatus.Paid, checkIndex);

                index = checkIndex == -1 ? checkIndex : index;
            }

            return index;
        }

        //Last claims were reproc if the end of the last reproc matches the length of the enumerable
        private bool lastClaimsWereCompletedReproc(IOrderedEnumerable<RetroLICSClaimDTO> claims)
        {
            bool lastClaimsWereCompletedReproc = claims.Count() >= 3;

            if (lastClaimsWereCompletedReproc)
            {
                int lastReprocEndIndex = getEndIndexOfLastReproc(claims);
                lastClaimsWereCompletedReproc = lastReprocEndIndex == claims.Count() - 1;
            }

            return lastClaimsWereCompletedReproc;
        }

        //Recalc has begun if last status for a group of claims for the month wasn't Paid
        //Or if the last claims for any groups for the month were a completed reproc
        //Or if the updates in the accum log mismatch the expected values for the start of the monthS
        private bool monthRecalcAlreadyStarted(string adsConnectionString, IEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claims, RetroLICSClaimDTO lastClaim, int accumsSinceZeroOut)
        {
            return claims.Any(x => x.Last().ClaimStatus == ClaimStatus.Paid || lastClaimsWereCompletedReproc(x))
                   || lastActionWasAccumUpdate(adsConnectionString, lastClaim, accumsSinceZeroOut);
        }

        private bool lastActionWasAccumUpdate(string adsConnectionString, RetroLICSClaimDTO claim, int accumsSinceZeroOut)
        {
            return RetroLICSRepository.LastActionWasAccumAdjustment(adsConnectionString, claim?.ENRID, claim?.RXNO, claim?.FILLDT, accumsSinceZeroOut);
        }

        private int getFirstIndexOfClaimStatus(List<RetroLICSClaimDTO> claims, ClaimStatus claimStatus, int startIndex = 0)
        {
            return startIndex >= 0 ? claims.FindIndex(startIndex, x => x.ClaimStatus == claimStatus) : -1;
        }

        private int getLastIndexOfClaimStatus(List<RetroLICSClaimDTO> claims, ClaimStatus claimStatus, int startIndex)
        {
            return startIndex > 0 ? claims.FindLastIndex(startIndex, x => x.ClaimStatus == claimStatus) : -1;
        }

        private void writeClaimFiles(ref IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claims, string claimFilePath)
        {
            ClaimHelper helper = new ClaimHelper();

            if (!Directory.Exists(claimFilePath))
            {
                Directory.CreateDirectory(claimFilePath);
            }

            foreach (var claim in claims)
            {
                var originalClaim = claim.First();

                NCPDP.Telecom.Transmission Ncpdp =
                    helper.MapSubmissionRequestClaimToNcpdp(originalClaim.MapClaimSubmissionRequest(ClaimSubmissionType.OriginalClaimForLogging));
                string claimString = Ncpdp.ToString();

                string dateFolder = claimFilePath + originalClaim.FILLDT.ToString("yyyy-MM-dd") + @"\";

                if (!Directory.Exists(dateFolder))
                {
                    Directory.CreateDirectory(dateFolder);
                }

                string fileNameStart = $"{dateFolder}{originalClaim.NDCREF}";
                string fileNameEnd = ".txt";

                int fileCount = 0;

                //Keep a copy from each run in case of errors or file write issues/changes
                while (File.Exists(getClaimLogFileName(fileNameStart, fileCount, fileNameEnd)))
                {
                    fileCount++;
                }

                using (StreamWriter fileOut = new StreamWriter(getClaimLogFileName(fileNameStart, fileCount, fileNameEnd)))
                {
                    string strSwitcherRequest = buildSwitcherRequest(claimString, 1);
                    fileOut.Write(strSwitcherRequest);
                    fileOut.WriteLine();
                }
            }
        }

        private static string getClaimLogFileName(string fileNameStart, int fileCount, string fileNameEnd)
        {
            return fileNameStart + $"-{fileCount:000}" + fileNameEnd;
        }

        private static string buildSwitcherRequest(string strNCPDPRequest, int intTransactionId)
        {
            // ---------------------------------------------------------------------
            // Build the ProCare Rx Switcher Request from the NCPDP Request.
            // ---------------------------------------------------------------------
            string CstrHeader = "RUNWAY/ICMW0021{0}";
            char STX = (char)(0x02);
            char ETX = (char)(0x03);

            string strHeader = String.Format(CstrHeader, intTransactionId.ToString("0000"));
            string strSwitcherRequest = STX + strHeader + strNCPDPRequest + ETX;
            return strSwitcherRequest;
        }

        private void removeClaimFiles(string claimFilePath)
        {
            if (Directory.Exists(claimFilePath))
            {
                Directory.Delete(claimFilePath, true);
            }
        }

        private void checkAllEligibility(ref IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claims, Dictionary<string, string> claimApiCredentials)
        {
            foreach (var claim in claims)
            {
                var originalClaim = claim.First();

                try
                {
                    var response = CheckEligibility(originalClaim.MapClaimEligibilityRequest(), claimApiCredentials);

                    if (!response.IsErrorResponse() && string.IsNullOrWhiteSpace(response.ResponseStatus?.ErrorCode) && response.Status.RejectCount == 0)
                    {
                        claim.Each(x =>
                        {
                            x.ReprocessingPLNID = response.Insurance.PlanId;
                            x.ValidEligibility = true;
                        });
                    }
                    else
                    {
                        throw new Exception(response.Status.AdditionalMessages?.Select(x => x.Information).Join("; ") ?? "");
                    }
                }
                catch (Exception ex)
                {
                    throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.GapInEligibility,
                                                           originalClaim.ENRID, ex.Message,
                                                           $"A gap in eligibility was found for enrollee {originalClaim.ENRID} for date {originalClaim.FILLDT.ToShortDateString()}");
                }
            }
        }

        private void reverseClaims(ref IOrderedEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claims, Dictionary<string, string> claimApiCredentials, ref bool unlockLockedMember)
        {
            foreach (var claim in claims)
            {
                var originalClaim = claim.First();

                if (!claim.Any(x => x.ClaimStatus == ClaimStatus.Reversal))
                {
                    try
                    {
                        var response = ReverseClaim(originalClaim.MapClaimReversalRequest(), claimApiCredentials);

                        if (!response.IsErrorResponse() && string.IsNullOrWhiteSpace(response.ResponseStatus?.ErrorCode) &&
                            response.Status.RejectCount == 0 && !string.IsNullOrWhiteSpace(response.NcpdpResponseString))
                        {
                            unlockLockedMember = false;
                            claim.Last().ClaimStatus = ClaimStatus.Reversal;

                            claim.Each(x =>
                            {
                                x.ReprocessedNDCREF = response.Status.AuthorizationNumber;
                                x.ReversalSuccessful = true;
                            });
                        }
                        else
                        {
                            //AdditionalMessages if we have them
                            //If not, no NCPDP Response message if none was returned
                            //Or No Exception message if none was returned
                            string exceptionMessage = !string.IsNullOrWhiteSpace(response.NcpdpResponseString) ?
                                                        "No Claims API exception returned, potential switcher/CPE timeout issue" :
                                                        "No NCPDP Response string returned.";

                            if (response.ResponseStatus?.Message != null)
                            {
                                exceptionMessage = response.ResponseStatus.Message;
                            }

                            throw new Exception(response.Status.AdditionalMessages != null && response.Status.AdditionalMessages.Count > 0 ?
                                                response.Status.AdditionalMessages.Select(x => x.Information).Join("; ") :
                                                exceptionMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.ClaimReversalFailed,
                                                               originalClaim.ENRID, ex.Message,
                                                               $"Failed to reverse claim with NDCREF {originalClaim.NDCREF} for enrollee {originalClaim.ENRID}.");
                    }
                }
                else
                {
                    claim.Each(x =>
                    {
                        x.ReprocessedNDCREF = claim.Last(y => y.ClaimStatus == ClaimStatus.Reversal).NDCREF;
                        x.ReversalSuccessful = true;
                    });
                }
            }
        }

        private void resubmitClaims(ref IEnumerable<IOrderedEnumerable<RetroLICSClaimDTO>> claims, DateTime month, Dictionary<string, string> claimApiCredentials)
        {
            foreach (var claim in claims)
            {
                var originalClaim = claim.First();

                if (claim.Last().ClaimStatus == ClaimStatus.Reversal)
                {
                    try
                    {
                        var response = SubmitClaim(originalClaim.MapClaimSubmissionRequest(ClaimSubmissionType.ResubmitUnderNewPlan),
                                                   claimApiCredentials);
                        if (!response.IsErrorResponse() && string.IsNullOrWhiteSpace(response.ResponseStatus?.ErrorCode) &&
                            response.Status.RejectCount == 0 && !string.IsNullOrWhiteSpace(response.NcpdpResponseString))
                        {
                            claim.Last().ClaimStatus = ClaimStatus.Paid;

                            claim.Each(x =>
                            {
                                x.ReprocessedNDCREF = response.Status.AuthorizationNumber;
                                x.ResubmissionSuccessful = true;
                            });
                        }
                        else
                        {
                            //AdditionalMessages if we have them
                            //If not, no NCPDP Response message if none was returned
                            //Or No Exception message if none was returned
                            string exceptionMessage = !string.IsNullOrWhiteSpace(response.NcpdpResponseString) ?
                                                        "No Claims API exception returned, potential switcher/CPE timeout issue" :
                                                        "No NCPDP Response string returned.";

                            if (response.ResponseStatus?.Message != null)
                            {
                                exceptionMessage = response.ResponseStatus.Message;
                            }

                            throw new Exception(response.Status.AdditionalMessages != null && response.Status.AdditionalMessages.Count > 0 ?
                                                response.Status.AdditionalMessages.Select(x => x.Information).Join("; ") :
                                                exceptionMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new RetroLICSProcessingException(RetroLICSProcessingException.RetroLICSProcessingExceptionType.ClaimSubmissionFailed,
                                                               originalClaim.ENRID, ex.Message,
                                                               $"Failed to submit claim with NDCREF {originalClaim.NDCREF} for enrollee {originalClaim.ENRID} against plan {originalClaim.PLNID}.");
                    }
                }
                else
                {
                    claim.Each(x =>
                    {
                        x.ReprocessedNDCREF = claim.Last().NDCREF;
                        x.ResubmissionSuccessful = true;
                    });
                }
            }
        }

        private ClaimEligibilityResponse CheckEligibility(ClaimEligibilityRequest request, Dictionary<string, string> claimApiCredentials)
        {
            ClaimEligibilityResponse response = new ClaimEligibilityResponse();

            //Get claim api credentials
            string userName = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Username];
            string password = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Password];
            string url = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_BaseUrl] + ApiRoutes.ClaimEligibilityRequest;

            var output = ApiHelper.ApiBasicAuthPost<ClaimEligibilityRequest, ClaimEligibilityResponse>(url, request, userName, password).Result;

            response = output.Response;

            return response;
        }

        private ClaimReversalResponse ReverseClaim(ClaimReversalRequest request, Dictionary<string, string> claimApiCredentials)
        {
            ClaimReversalResponse response = new ClaimReversalResponse();

            //Get claim api credentials
            string userName = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Username];
            string password = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Password];
            string url = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_BaseUrl] + ApiRoutes.ClaimReversalRequest;

            var output = ApiHelper.ApiBasicAuthPost<ClaimReversalRequest, ClaimReversalResponse>(url, request, userName, password).Result;

            response = output.Response;

            return response;
        }

        private ClaimSubmissionResponse SubmitClaim(ClaimSubmissionRequest request, Dictionary<string, string> claimApiCredentials)
        {
            ClaimSubmissionResponse response = new ClaimSubmissionResponse();

            //Get claim api credentials
            string userName = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Username];
            string password = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_Password];
            string url = claimApiCredentials[ConfigSetttingKey.PBMAPI_ClaimsAPI_BaseUrl] + ApiRoutes.ClaimSubmissionRequest;

            var output = ApiHelper.ApiBasicAuthPost<ClaimSubmissionRequest, ClaimSubmissionResponse>(url, request, userName, password).Result;

            response = output.Response;

            return response;
        }

        private Dictionary<string, string> GetConfiguration(List<Configuration> configurations, string settingName)
        {
            return configurations.First(x => x.ConfigurationKey ==
                                             settingName)
                                 .ConfigurationValues;
        }

        private string formatEprocareConnectionString(string datasetPath)
        {
            //Error if no dataset found
            if (string.IsNullOrWhiteSpace(datasetPath))
            {
                throw new ArgumentException("Host URL is not valid.");
            }

            return MultipleConnectionsHelper.GetEProcareConnectionString(datasetPath);
        }

        private string getEprocareConnectionString(CommonApiHelper commonApiHelper, string hostUrl)
        {
            string adsConnectionString = "";

            if (!string.IsNullOrWhiteSpace(hostUrl))
            {
                Dictionary<string, string> eProcareDatasetList = commonApiHelper.GetSetting(ConfigSetttingKey.PBMAPI_eProcareDatasets).Result;
                List<eProcareDatasetDTO> datasets = JsonConvert.DeserializeObject<List<eProcareDatasetDTO>>(eProcareDatasetList[ConfigSetttingKey.APIPBM_Datasets]);
                string adsDatasetPath = datasets.FirstOrDefault(x => x.HostUrl.EqualsIgnoreCase(hostUrl)).DatasetPath;
                adsConnectionString = formatEprocareConnectionString(adsDatasetPath);
            }

            return adsConnectionString;
        }

        public static List<List<RetroLICSMemberDTO>> getProcessingBatches(List<RetroLICSMemberDTO> items, int batchSize)
        {
            return items
                   .Select((record, index) => new { record, index })
                   .GroupBy(x => x.index / batchSize)
                   .Select(g => g.Select(x => x.record).ToList())
                   .ToList();
        }

        private RetroLICSReprocessClaimsTaskResponse processRetroLicsReprocessBatch(string hostUrl, DateTime? startDate, string requestingUserEmail,
                                                                                    List<string> enrolleeIds, int currentBatch, int totalBatches)
        {
            RetroLICSReprocessClaimsTaskRequest request = new RetroLICSReprocessClaimsTaskRequest
            {
                HostUrl = hostUrl,
                StartDate = startDate,
                RequestingUserEmail = requestingUserEmail,
                EnrolleeIDList = enrolleeIds,
                CurrentBatch = currentBatch,
                TotalBatches = totalBatches
            };

            RetroLICSReprocessClaimsTaskResponse response = Post(request).Result;

            return response;
        }

        private string formatList(List<string> items)
        {
            string result = "";

            if (items.Count > 0)
            {
                StringBuilder listBuilder = new StringBuilder("");
                foreach (string item in items)
                {

                }

                result = listBuilder.ToString();
            }

            return result;
        }

        #endregion

        #region RTM
        private string GetRtmConnectionString(List<Configuration> configurations, string clientId)
        {
            string adsConnectionString = "";

            if (!string.IsNullOrWhiteSpace(clientId))
            {
                Dictionary<string, string> datasetList = GetConfiguration(configurations, ConfigSetttingKey.PBMAPI_SendRTMData_Connections);
                adsConnectionString = datasetList.FirstOrDefault(x => x.Key.EqualsIgnoreCase(clientId)).Value;
            }

            return adsConnectionString;
        }
        #endregion

        #endregion
    }
}
