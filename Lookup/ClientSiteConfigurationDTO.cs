using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClientSiteConfigurationDTO: ILoadFromDataReader
    {
        public int ClientSiteConfigurationID { get; set; }
        public string DatasetDisplayName { get; set; }
        public int? LogoID { get; set; }
        public byte[] LogoBinary { get; set; }
        public int ClientSideTimeOut { get; set; }
        public bool AddUsesExistingENRID { get; set; }
        public string URL { get; set; }


        public void LoadFromDataReader(IDataReader reader)
        {
            ClientSiteConfigurationID = reader.GetInt32orDefault("SiteID");
            DatasetDisplayName = reader.GetStringorDefault("DatasetDisplayName");
            LogoID = reader.GetInt32orNull("LogoID");
            LogoBinary = reader.GetBytesSafe("LogoBinary");
            ClientSideTimeOut = reader.GetInt32orDefault("ClientSideTimeOut");
            AddUsesExistingENRID = reader.GetBooleanSafe("AddUsesExistingENRID");
            URL = reader.GetStringorDefault("URL");
        }
    }


}
