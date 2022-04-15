using Newtonsoft.Json;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ProCare.API.Core.DTO;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using ProCare.Common.Data.SQL;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public static class MultipleConnectionsHelper
    {
        public const string eProcareAdsConnectionStringTemplate =
            "Data Source={0};User ID=Runway_User;Password=Runway;Advantage Server Type=ADS_REMOTE_SERVER;Connect Timeout=0;TrimTrailingSpaces=True; LockMode=proprietary";

        public static IDataAccessHelper GetAdsHelper(string adsConnectionString, IDataAccessHelper DataHelper)
        {
            IDataAccessHelper helper = DataHelper;
            if (!String.IsNullOrWhiteSpace(adsConnectionString))
            {
                helper = new AdsHelper(adsConnectionString);
            }

            return helper;
        }

        public static IDataAccessHelper GetSqlHelper(string sqlConnectionString, IDataAccessHelper DataHelper)
        {
            IDataAccessHelper helper = DataHelper;
            if (!String.IsNullOrWhiteSpace(sqlConnectionString))
            {
                helper = new SQLHelper(sqlConnectionString);
            }

            return helper;
        }

        public static string GetEProcareConnectionString(string datasetPath)
        {
            return string.Format(eProcareAdsConnectionStringTemplate, datasetPath);
        }

        public static string GetADSConnectionString(ADSServerDatabaseDTO aDSServerDatabaseDTO)
        {
            string connectionString = string.Empty;

            if(aDSServerDatabaseDTO != null)
            {
                connectionString = string.Format("Data Source={0};{1}", aDSServerDatabaseDTO.Path, aDSServerDatabaseDTO.Properties);
            }

            return connectionString;
        }

        public static string GetADSConnectionString(Dictionary<string, string> clientSetting, string clientID)
        {
           var adsServerDatabases = JsonConvert.DeserializeObject<List<ADSServerDatabaseDTO>>(clientSetting[ConfigSetttingKey.GlobalPBM_Settings_ADSServerDatabases]);
           var  adsServerDatabaseDTO = adsServerDatabases.FirstOrDefault(x => x.Name == clientID.ToUpper());

           var connectionString = string.Empty;

            if (adsServerDatabaseDTO != null)
            {
                connectionString = string.Format("Data Source={0};{1}", adsServerDatabaseDTO.Path, adsServerDatabaseDTO.Properties);
            }

            return connectionString;
        }
        public static async Task<DatasetDTO> GetClientDataset(string clientGuid, string datasetName)
        {
            List<DatasetDTO> datasets = await GetAllClientGuidDatasets(clientGuid).ConfigureAwait(false);

            bool useSpecificDataset = !string.IsNullOrWhiteSpace(datasetName);
            bool singleDatasetClient = datasets.Count() == 1;

            DatasetDTO dataset =  datasets.First(x =>
                                                    singleDatasetClient ||
                                                    (useSpecificDataset && x.Name.EqualsIgnoreCase(datasetName)));

            if(dataset == null || string.IsNullOrWhiteSpace(dataset.Name))
            {
                string exceptionMesssage = useSpecificDataset ?
                                                "The requested Client is not configured for this ClientGuid." :
                                                singleDatasetClient ?
                                                    "No configured datasets were found for this ClientGuid." :
                                                    "Multiple configured datasets were found for this ClientGuid.  Specify Client on request to select dataset.";

                throw new ArgumentException(exceptionMesssage);
            }

            return dataset;
        }

        public static async Task<List<DatasetDTO>> GetClientDatasetList(string clientGuid, string datasetName)
        {
            List<DatasetDTO> datasets = await GetAllClientGuidDatasets(clientGuid).ConfigureAwait(false);

            bool useSpecificDataset = !string.IsNullOrWhiteSpace(datasetName);

            if (useSpecificDataset)
            {
                datasets = datasets.Where(x => x.Name.EqualsIgnoreCase(datasetName)).ToList();
            }

            if(datasets.Count == 0)
            {
                string exceptionMesssage = useSpecificDataset ?
                                                "The requested Client is not configured for this ClientGuid." :
                                                    "No configured datasets were found for this ClientGuid.";

                throw new ArgumentException(exceptionMesssage);
            }

            return datasets;
        }

        public static async Task<List<DatasetDTO>> GetAllClientGuidDatasets(string clientGuid)
        {
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            Dictionary<string, string> clientSetting = await commonApiHelper.GetSetting(ConfigSetttingKey.BasicAuth_PBMApiUsernamePrefix + clientGuid).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<List<DatasetDTO>>(clientSetting[ConfigSetttingKey.APIPBM_Datasets]);

        }
    }
}
