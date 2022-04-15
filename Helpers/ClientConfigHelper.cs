using Newtonsoft.Json;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ProCare.API.PBM.Repository;
using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.DTO.Eligibility;
using ProCare.Common;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Helpers
{
    public static class ClientConfigHelper
    {
        #region Private Properties

        private static string ParentLevelName = "ParentID";
        private static string OrganizationLevelName = "OrganizationID";
        private static string GroupLevelName = "GroupID";
        private static string PlanLevelName = "PlanID";

        #endregion

        #region Public Methods

        public static async Task<Dictionary<string, string>> GetClientConfigs(string clientGuid)
        {
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            return await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + clientGuid).ConfigureAwait(false);
        }

        public static async Task<Dictionary<string, string>> GetClientConfigs(CommonApiHelper commonApiHelper, string clientGuid)
        {
            return await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + clientGuid).ConfigureAwait(false);
        }

        public static string GetDatasetConnectionString(DatasetDTO dataset)
        {
            //Error if no dataset found
            if (string.IsNullOrWhiteSpace(dataset?.Path))
            {
                throw new ArgumentException("The Client is invalid or this account does not have access to the requested Client.");
            }

            return MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path);
        }

        public static string GetUserIDFromClientGuid(string clientGuid)
        {
            int charLocation = clientGuid.IndexOf("_", StringComparison.Ordinal);
            string userName = clientGuid.Substring(0, charLocation).ToUpper();
            return userName.Substring(0, userName.Length < 15 ? userName.Length : 15);
        }

        public static DatasetDTO GetClientDataset(Dictionary<string, string> clientSetting, string client, string parid, string orgid, string grpid, string plnid)
        {
            List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);
            DatasetDTO dataset = datasets.FirstOrDefault(x => x.Name.EqualsIgnoreCase(client));
            ValidateClientAccessLevels(dataset, parid, orgid, grpid, plnid);
            return dataset;
        }

        public static List<DatasetDTO> GetClientDatasets(Dictionary<string, string> clientSetting, string client, string parid, string orgid, string grpid, string plnid)
        {
            List<DatasetDTO> datasets = JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);

            if (!string.IsNullOrWhiteSpace(client))
            {
                datasets = datasets.Where(x => x.Name.Equals(client, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ValidateClientAccessLevels(ref datasets, parid, orgid, grpid, plnid);
            return datasets.Where(x => x.ClientHasAccess).ToList();
        }

        public static bool DatasetHasAccessLimit(DatasetDTO dataset)
        {
            return dataset.ParentIDs.Any() || dataset.OrganizationIDs.Any() || dataset.GroupIDs.Any() || dataset.PlanIDs.Any();
        }

        public static bool UseLimitedAccess(List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            return parentIDs.Any() || organizationIDs.Any() || groupIDs.Any() || planIDs.Any();
        }

        public static void ValidateClientAccessLevels(ref List<DatasetDTO> datasets, string parid, string orgid, string grpid, string plnid)
        {
            datasets.Where(x => !DatasetHasAccessLimit(x)).Each(x => x.ClientHasAccess = true);
            bool validDatasetFound = datasets.Any(x => x.ClientHasAccess);

            List<AccessCheckResult> invalidDatasets = new List<AccessCheckResult>();

            foreach (DatasetDTO dataset in datasets)
            {
                if (!dataset.ClientHasAccess)
                {
                    AccessCheckResult validationResult = checkAllLevelAccess(dataset, parid, orgid, grpid, plnid);

                    if (validationResult.AllFound)
                    {
                        validDatasetFound = true;
                        dataset.ClientHasAccess = true;
                    }
                    else
                    {
                        invalidDatasets.Add(validationResult);
                    }
                }
            }

            if (!validDatasetFound)
            {
                throw new ArgumentException(getCombinedExceptionMessage(invalidDatasets, parid, orgid, grpid, plnid));
            }
        }

        public static void ValidateClientAccessLevels(DatasetDTO dataset, string parid, string orgid, string grpid, string plnid)
        {
            bool validDatasetFound = !DatasetHasAccessLimit(dataset);
            List<AccessCheckResult> invalidDatasets = new List<AccessCheckResult>();

            if (!validDatasetFound)
            {
                AccessCheckResult validationResult = checkAllLevelAccess(dataset, parid, orgid, grpid, plnid);

                if (validationResult.AllFound)
                {
                    validDatasetFound = true;
                    dataset.ClientHasAccess = true;
                }
                else
                {
                    invalidDatasets.Add(validationResult);
                }
            }

            if (!validDatasetFound)
            {
                throw new ArgumentException(getCombinedExceptionMessage(invalidDatasets, parid, orgid, grpid, plnid));
            }
        }

        public static async Task<ClientWithConfigurationsDTO> GetClientWithConfigurations(IEligibilityRepository eligibilityRepository, CommonApiHelper commonApiHelper, string settingName, string userName)
        {
            var setting = await commonApiHelper.GetSetting(settingName).ConfigureAwait(false);
            var clientApiConfigurations = JsonConfigReader.Deserialize<Dictionary<string, string>>(setting[userName]);

            long clientID = long.Parse(clientApiConfigurations["ClientID"]);
            string databaseUserName = clientApiConfigurations["UserName"];

            ClientWithConfigurationsDTO clientWithConfigs = eligibilityRepository.GetClientWithClientConfiguration(clientID);

            clientWithConfigs.UserName = databaseUserName;
            clientWithConfigs.ADSConnectionString = MultipleConnectionsHelper.GetEProcareConnectionString(clientWithConfigs.ADSDatabasePath);

            return clientWithConfigs;
        }

        #endregion

        #region Private Methods

        #region Check Access
        private static bool checkSameLevelAccess(List<string> configuredIDs, string submittedID)
        {
            return string.IsNullOrWhiteSpace(submittedID) || (!string.IsNullOrWhiteSpace(submittedID) && configuredIDs.Contains(submittedID));
        }

        private static AccessCheckResult checkAllSameLevelAccess(DatasetDTO dataset, string parid, string orgid, string grpid, string plnid)
        {
            return new AccessCheckResult(checkSameLevelAccess(dataset.ParentIDs, parid),
                                         checkSameLevelAccess(dataset.OrganizationIDs, orgid),
                                         checkSameLevelAccess(dataset.GroupIDs, grpid),
                                         checkSameLevelAccess(dataset.PlanIDs, plnid));
        }

        public static AccessCheckResult checkUpperLevelAccess(DatasetDTO dataset, bool parentAccess, string orgid, string grpid, string plnid)
        {
            return queryUpperLevelAccess(dataset, parentAccess, orgid, grpid, plnid).Result;
        }

        public static AccessCheckResult checkAllLevelAccess(DatasetDTO dataset, string parid, string orgid, string grpid, string plnid)
        {
            AccessCheckResult accessCheckResult = checkAllSameLevelAccess(dataset, parid, orgid, grpid, plnid);

            if (!accessCheckResult.AllFound)
            {
                AccessCheckResult upperLevelAccessCheckResult = checkUpperLevelAccess(dataset, accessCheckResult.ParFound, orgid, grpid, plnid);
                accessCheckResult = combineAccessCheckResults(accessCheckResult, upperLevelAccessCheckResult);
            }

            return accessCheckResult;
        }

        #endregion

        #region Generate Validation Error Message

        private static string getCombinedExceptionMessage(List<AccessCheckResult> invalidDatasets, string parid, string orgid, string grpid, string plnid)
        {
            AccessCheckResult totalAccessFailures = combineAccessCheckResults(invalidDatasets);

            string combinationMessage = getInvalidAccessLevelMessages(totalAccessFailures, parid, orgid, grpid, plnid);

            return $"The following are not configured for the ClientGuid: {combinationMessage}";
        }

        private static string getInvalidAccessLevelMessages(AccessCheckResult accessCheckResult, string parid, string orgid, string grpid, string plnid)
        {
            List<string> invalidAccessLevels = new List<string>();

            invalidAccessLevels.Add(getInvalidAccessLevelMessage(accessCheckResult.ParFound, ParentLevelName, parid));
            invalidAccessLevels.Add(getInvalidAccessLevelMessage(accessCheckResult.OrgFound, OrganizationLevelName, orgid));
            invalidAccessLevels.Add(getInvalidAccessLevelMessage(accessCheckResult.GrpFound, GroupLevelName, grpid));
            invalidAccessLevels.Add(getInvalidAccessLevelMessage(accessCheckResult.PlnFound, PlanLevelName, plnid));

            return string.Join(", ", invalidAccessLevels.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private static string getInvalidAccessLevelMessage(bool validAccess, string levelName, string levelID)
        {
            return !validAccess ? $"{levelName} '{levelID}'" : "";
        }

        #endregion

        #region Database

        public static string GetConnectionStringFromDataset(DatasetDTO dataset)
        {
            //Error if no dataset found
            if (string.IsNullOrWhiteSpace(dataset?.Path))
            {
                throw new ArgumentException("The Client is invalid or this account does not have access to the requested Client.");
            }

            return MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path);
        }

        public static async Task<AccessCheckResult> queryUpperLevelAccess(DatasetDTO dataset, bool parentAccess, string orgid, string grpid, string plnid)
        {
            AccessCheckResult result = new AccessCheckResult(parentAccess, false, false, false);

            string connectionString = MultipleConnectionsHelper.GetEProcareConnectionString(dataset.Path);

            if (potentialUpperLevelAccessExists(dataset, orgid, grpid, plnid))
            {
                AdsHelper sqlHelper = new AdsHelper(connectionString);

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@PARIDList",  getJoinedList(dataset.ParentIDs)},
                    {"@ORGIDList",  getJoinedList(dataset.OrganizationIDs)},
                    {"@GRPIDList",  getJoinedList(dataset.GroupIDs)},
                    {"@ORGID",  orgid},
                    {"@GRPID",  grpid},
                    {"@PLNID",  plnid},
                };

                Task<AccessCheckResult> t = Task.Run(() =>
                {
                    AccessCheckResult dbResult = new AccessCheckResult(parentAccess, false, false, false);

                    try
                    {
                        sqlHelper.ExecuteReader("apiPBM_ClientAccess_checkAccess", CommandType.StoredProcedure, parameters, reader =>
                        {
                            bool orgFound = reader.GetBooleanSafe("OrgFound");
                            bool grpFound = reader.GetBooleanSafe("GrpFound");
                            bool plnFound = reader.GetBooleanSafe("PlnFound");

                            dbResult = new AccessCheckResult(parentAccess, orgFound, grpFound, plnFound);
                        });
                    }
                    catch (Exception)
                    {
                        throw new TaskCanceledException($"Failed to check alternate levels of client access.");
                    }

                    return dbResult;
                });

                result = await t.ConfigureAwait(false);
            }

            return result;
        }

        #endregion

        #region Utility

        private static bool potentialUpperLevelAccessExists(DatasetDTO dataset, string orgid, string grpid, string plnid)
        {
            return (dataset.ParentIDs.Any() && (!string.IsNullOrWhiteSpace(orgid) || !string.IsNullOrWhiteSpace(grpid) || !string.IsNullOrWhiteSpace(plnid))) ||
                   (dataset.OrganizationIDs.Any() && (!string.IsNullOrWhiteSpace(grpid) || !string.IsNullOrWhiteSpace(plnid))) ||
                   (dataset.GroupIDs.Any() && (!string.IsNullOrWhiteSpace(plnid)));
        }

        private static string getJoinedList(List<string> items)
        {
            return items.Any() ? string.Join(",", items) : null;
        }

        #endregion

        #region Combine Access Check Results

        private static AccessCheckResult combineAccessCheckResults(AccessCheckResult item1, AccessCheckResult item2)
        {
            return new AccessCheckResult(item1.ParFound || item2.ParFound,
                                         item1.OrgFound || item2.OrgFound,
                                         item1.GrpFound || item2.GrpFound,
                                         item1.PlnFound || item2.PlnFound);
        }

        private static AccessCheckResult combineAccessCheckResults(List<AccessCheckResult> invalidDatasets)
        {
            int minValidationIssueCount = invalidDatasets.Select(x => x.InvalidCount).Min();

            List<AccessCheckResult> mostSuccessfulDatasets = invalidDatasets.Where(x => x.InvalidCount == minValidationIssueCount).ToList();

            AccessCheckResult totalAccessFailures = new AccessCheckResult(mostSuccessfulDatasets.All(x => x.ParFound),
                                                                          mostSuccessfulDatasets.All(x => x.OrgFound),
                                                                          mostSuccessfulDatasets.All(x => x.GrpFound),
                                                                          mostSuccessfulDatasets.All(x => x.PlnFound));

            return totalAccessFailures;
        }

        #endregion

        #endregion
    }

    public class AccessCheckResult
    {
        public bool ParFound { get; set; }
        public bool OrgFound { get; set; }
        public bool GrpFound { get; set; }
        public bool PlnFound { get; set; }
        public bool AllFound => ParFound && OrgFound && GrpFound && PlnFound;
        public int InvalidCount => (ParFound ? 0 : 1) +
                                   (OrgFound ? 0 : 1) +
                                   (GrpFound ? 0 : 1) +
                                   (PlnFound ? 0 : 1);

        public AccessCheckResult(bool parFound, bool orgFound, bool grpFound, bool plnFound)
        {
            ParFound = parFound;
            OrgFound = orgFound;
            GrpFound = grpFound;
            PlnFound = plnFound;
        }
    }
}
