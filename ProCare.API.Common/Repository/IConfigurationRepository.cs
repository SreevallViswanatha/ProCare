using ProCare.API.Common.Repository.DTO;
using System.Collections.Generic;

namespace ProCare.API.Common.Repository
{
    public interface IConfigurationRepository
    {
        ConfigurationDTO GetConfiguration(string settingKey);
        List<ConfigurationDTO> GetConfigurations(List<string> key);
    }
}
