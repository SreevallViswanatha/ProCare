using log4net;
using Newtonsoft.Json;
using ProCare.API.Common.Repository;
using ProCare.API.Common.Repository.DTO;
using ProCare.API.Core;
using ProCare.API.Core.Requests;
using ProCare.API.Core.Responses;
using ProCare.Common;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.Common
{
    /// <summary>
    /// Common Services are services that are not specific to a certain LOB (PBM/Pharmacy).
    /// Settings are a good example of services that are provided here.
    /// </summary>
    public class CommonServices : Service
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Properties

        public IMPIRepository MPIRepository { get; set; }

        public IConfigurationRepository ConfigRepository { get; set; }

        public ILogRepository LogRepository { get; set; }

        #endregion Properties

        #region APIs

        #region Configuration

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.Configurations + "|" + ApiRoutes.Configurations)]
        public ConfigurationsResponse Get(ConfigurationsRequest request)
        {
            Stopwatch timer = new Stopwatch();
            ConfigurationsResponse response = new ConfigurationsResponse
            {

                ApiRequestID = Guid.NewGuid()
            };

            try
            {
                List<ConfigurationDTO> dto = ConfigRepository.GetConfigurations(request.ConfigurationKeys);
                List<Configuration> configurations = new List<Configuration>();
                foreach (ConfigurationDTO configurationDTO in dto)
                {
                    string settingValueJSON = null;
                    if (configurationDTO.IsSettingEncrypted)
                    {
                        settingValueJSON = EncryptionHelper.RijndaelDecryptString(configurationDTO.SettingValue, ApplicationSettings.ConfigurationsEncryptionKey);
                    }
                    else
                    {
                        settingValueJSON = configurationDTO.SettingValue;
                    }
                    Configuration configuration = new Configuration()
                    {
                        ConfigurationKey = configurationDTO.SettingKey,
                        ConfigurationValues = JsonConfigReader.Deserialize<Dictionary<string, string>>(settingValueJSON)
                    };
                    configurations.Add(configuration);
                }
                response.Configurations = configurations;
            }
            catch (Exception ex)
            {
                try
                {
                    LogRepository.LogExceptionMessage(false, (int)ApplicationSource.CommonAPI, ex.Message, ex.StackTrace,
                        JsonConvert.SerializeObject(new Dictionary<string, string> { { "SettingKeys", JsonConvert.SerializeObject(request.ConfigurationKeys) } }), "Get.ConfigurationsRequest");
                }
                catch { }
                throw new Exception(FieldDescriptions.UnhandledExceptionError);
            }
            finally
            {
                timer.Stop();
                response.TimeToProcess = timer.ElapsedMilliseconds;
            }

            return response;

        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.SettingsRequest + "|" + ApiRoutes.SettingsRequest)]
        public ServiceSettingResponse Get(ServiceSettingRequest request)
        {
            Stopwatch timer = new Stopwatch();
            ServiceSettingResponse response = new ServiceSettingResponse
            {
                SettingKey = request.SettingKey,
                ApiRequestID = Guid.NewGuid()
            };

            try
            {
                ConfigurationDTO dto = ConfigRepository.GetConfiguration(request.SettingKey);

                string settingValueJSON = null;
                if (dto.IsSettingEncrypted)
                {
                    settingValueJSON = EncryptionHelper.RijndaelDecryptString(dto.SettingValue, ApplicationSettings.ConfigurationsEncryptionKey);
                }
                else
                {
                    settingValueJSON = dto.SettingValue;
                }

                response.SettingValues = JsonConfigReader.Deserialize<Dictionary<string, string>>(settingValueJSON);
            }
            catch (Exception ex)
            {
                try
                {
                    LogRepository.LogExceptionMessage(false, (int)ApplicationSource.CommonAPI, ex.Message, ex.StackTrace,
                        JsonConvert.SerializeObject(new Dictionary<string, string> { { "Setting", request.SettingKey } }), "Get.ServiceSettingRequest");
                }
                catch { }
                throw new Exception(FieldDescriptions.UnhandledExceptionError);
            }
            finally
            {
                timer.Stop();
                response.TimeToProcess = timer.ElapsedMilliseconds;
            }

            return response;

        }

        #endregion Configuration

        #region Log

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ApiMessageLogging + "|" + ApiRoutes.ApiMessageLogging)]
        public LogApiMessageResponse Post(LogApiMessageRequest request)
        {
            Stopwatch timer = new Stopwatch();
            LogApiMessageResponse response = new LogApiMessageResponse { ApiRequestID = Guid.NewGuid() };

            try
            {
                LogRepository.LogApiMessage(request.ApiTypeID, new Guid(request.ApiMessageID), request.MethodName, request.MessageTypeID,
                    request.XmlMessage, request.JsonMessage, request.ClientIPAddress, request.Identifier1, request.Identifier2,
                    request.Identifier3, request.Identifier4, request.MessageTimeStamp);

                response.ApiMessageID = request.ApiMessageID;
            }
            catch
            {
                throw new Exception(FieldDescriptions.UnhandledExceptionError);
            }
            finally
            {
                timer.Stop();
                response.TimeToProcess = timer.ElapsedMilliseconds;
            }

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.AppWarningLogging + "|" + ApiRoutes.AppWarningLogging)]
        public LogWarningMessageResponse Post(LogWarningMessageRequest request)
        {
            Stopwatch timer = new Stopwatch();
            LogWarningMessageResponse response = new LogWarningMessageResponse { ApiRequestID = Guid.NewGuid() };

            try
            {
                LogRepository.LogWarningMessage(request.ApplicationSourceID, request.Message, request.Properties);
            }
            catch
            {
                throw new Exception(FieldDescriptions.UnhandledExceptionError);
            }
            finally
            {
                timer.Stop();
                response.TimeToProcess = timer.ElapsedMilliseconds;
            }

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.AppInfoLogging + "|" + ApiRoutes.AppInfoLogging)]
        public LogInfoMessageResponse Post(LogInfoMessageRequest request)
        {
            Stopwatch timer = new Stopwatch();
            LogInfoMessageResponse response = new LogInfoMessageResponse { ApiRequestID = Guid.NewGuid() };

            try
            {
                LogRepository.LogInfoMessage(request.ApplicationSourceID, request.Message, request.Properties);
            }
            catch
            {
                throw new Exception(FieldDescriptions.UnhandledExceptionError);
            }
            finally
            {
                timer.Stop();
                response.TimeToProcess = timer.ElapsedMilliseconds;
            }

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.AppExceptionLogging + "|" + ApiRoutes.AppExceptionLogging)]
        public LogExceptionMessageResponse Post(LogExceptionMessageRequest request)
        {
            Stopwatch timer = new Stopwatch();
            LogExceptionMessageResponse response = new LogExceptionMessageResponse { ApiRequestID = Guid.NewGuid() };

            try
            {
                LogRepository.LogExceptionMessage(request.IsCritical, request.ApplicationSourceID, request.Message, request.StackTrace, request.Properties, request.MethodSource);
            }
            catch
            {
                throw new Exception(FieldDescriptions.UnhandledExceptionError);
            }
            finally
            {
                timer.Stop();
                response.TimeToProcess = timer.ElapsedMilliseconds;
            }

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.AppEventResponseTimeLogging + "|" + ApiRoutes.AppEventResponseTimeLogging)]
        public AppEventResponseTimeLoggingResponse Post(AppEventResponseTimeLoggingRequest request)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            AppEventResponseTimeLoggingResponse response = new AppEventResponseTimeLoggingResponse { ApiRequestID = Guid.NewGuid() };

            try
            {

                LogRepository.LogAppEventResponseTime(request.ApplicationSourceID, request.ApplicationEventTypeID, request.ApplicationEventName,
                    request.ApplicationFeatureName, request.ResponseTime, request.MiscField1, request.MiscField2, request.MiscField3, request.Properties);
            }
            catch (Exception)
            {
                throw new Exception(FieldDescriptions.UnhandledExceptionError);
            }
            finally
            {
                timer.Stop();
                response.TimeToProcess = timer.ElapsedMilliseconds;
            }

            return response;
        }

        #endregion Log

        #region MPI

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MasterPatientIndex + "|" + ApiRoutes.MasterPatientIndex)]
        public List<MasterPatientIndexResponse> Get(MasterPatientIndexRequest request)
        {
            List<MasterPatientIndexResponse> results = new List<MasterPatientIndexResponse>();

            Stopwatch timer = new Stopwatch();
            timer.Start();
            try
            {
                var dto = request.ConvertTo<MasterPatientIndexDTO>();
                var dtos = MPIRepository.SelectByFNameLNameAddressDOBZip(dto);
                dtos.ForEach(x => { results.Add(x.ConvertTo<MasterPatientIndexResponse>()); });

                if (results.Count == 0)
                {
                    throw HttpError.NotFound("No patient found for {0}".Fmt(request.FirstName));
                }
            }
            finally
            {
                timer.Stop();
            }

            return results;
        }

        #endregion MPI

        /// <summary>
        /// Responds to a Ping Request to inform the caller of the status of the API services
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PingResponse Get(PingRequest request)
        {
            return new PingResponse { Alive = true };
        }

        [Authenticate]
        public object Get(SecureTestRequest request)
        {
            var session = this.SessionAs<AuthUserSession>();

            return "This is secure content";
        }

        #endregion APIs

    }
}
