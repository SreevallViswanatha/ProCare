using System;
using System.Collections.Generic;
using System.Data;
using ProCare.Common;
using ProCare.Common.Data;

namespace ProCare.API.Common.Repository.DTO
{
    public class ConfigurationDTO : ILoadFromDataReader
    {
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }

        public bool IsSettingEncrypted { get; set; }
        

        public void LoadFromDataReader(IDataReader reader)
        {
            SettingValue = reader["SettingValue"].ToString();
            IsSettingEncrypted = Convert.ToBoolean(reader["IsEncrypted"]);
            SettingKey = reader["SettingKey"].ToString();
        }
    }
}
