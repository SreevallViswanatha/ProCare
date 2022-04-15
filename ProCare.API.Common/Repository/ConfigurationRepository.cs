using ProCare.API.Common.Repository.DataAccess;
using ProCare.API.Common.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;

namespace ProCare.API.Common.Repository
{
    public class ConfigurationRepository : BasedbRepository, IConfigurationRepository
    {
        #region Constructors

        public ConfigurationRepository(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets a configuratoin for the given key
        /// </summary>
        /// <param name="settingKey">String representing the key for the configuration to retrieve</param>
        /// <returns><see cref="ConfigurationDTO"/>representing the configuration</returns>
        public ConfigurationDTO GetConfiguration(string key)
        {
            var sqlHelper = new ConfigurationDataAccess(DataHelper);

            return sqlHelper.ReadConfiguration(key);
        }

        /// <summary>
        /// Gets  configurations for the given list of keys
        /// </summary>
        /// <param name="key"></param>
        /// <param name="settingKey">String representing the key for the configuration to retrieve</param>
        /// <returns><see cref="ConfigurationDTO"/>representing the configuration</returns>
        public List<ConfigurationDTO> GetConfigurations(List<string> key)
        {
            var sqlHelper = new ConfigurationDataAccess(DataHelper);

            return sqlHelper.ReadConfigurationsList(key);
        }

        #endregion Public Methods
    }
}
