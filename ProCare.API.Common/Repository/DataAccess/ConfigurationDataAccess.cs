using ProCare.API.Common.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.Common.Repository.DataAccess
{
    public class ConfigurationDataAccess : DataAccessBase
    {
        #region Constructors

        public ConfigurationDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets a configuration from the database for the given key
        /// </summary>
        /// <param name="key">String representing the key for the configuration to retreive</param>
        /// <returns><see cref="ConfigurationDTO"/> representing the configuration</returns>
        public ConfigurationDTO ReadConfiguration(string key)
        {
            ConfigurationDTO output = new ConfigurationDTO();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@SettingKey",  key}
            };

            DataHelper.ExecuteReader("usp_GetSettings", CommandType.StoredProcedure, parameters, reader =>
            {

                output.LoadFromDataReader(reader);
            });

            return output;
        }


        public List<ConfigurationDTO> ReadConfigurationsList(List<string> ConfigurationKeys)
        {
            List<ConfigurationDTO> output = new List<ConfigurationDTO>();

            DataTable input = CreateDataTable(ConfigurationKeys);
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ConfigurationKeys",  input}
            };

            DataHelper.ExecuteReader("usp_GetConfigurations", CommandType.StoredProcedure, parameters, reader =>
            {
                ConfigurationDTO configurationDTO = new ConfigurationDTO();
                configurationDTO.LoadFromDataReader(reader);
                output.Add(configurationDTO);
            });

            return output;
        }
        #endregion Public Methods

        #region Private Methods
        private static DataTable CreateDataTable(List<string> ConfigurationKeys)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ConfigurationKey", typeof(string));

            foreach (string key in ConfigurationKeys)
            {
                DataRow dataRow = table.NewRow();
                dataRow["ConfigurationKey"] = key;

                table.Rows.Add(dataRow);
            }
            return table;
        }
        #endregion
    }
}
