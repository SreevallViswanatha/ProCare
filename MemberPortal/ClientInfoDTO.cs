using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClientInfoDTO : ILoadFromDataReader
    {
        public int ClientID { get; set; }
        public string Name { get; set; }
        public bool AllowsOnlineAccess { get; set; }
        public int DefaultDomainID { get; set; }
        public string DomainName { get; set; }
        public string ParentID { get; set; }
        public string OrgID { get; set; }
        public string GroupID { get; set; }
        public string Class { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ClientID = reader.GetInt32orDefault("ClientID");
            Name = reader.GetStringorDefault("Name");
            AllowsOnlineAccess = reader.GetBooleanSafe("AllowAccess");
            DefaultDomainID = reader.GetInt32orDefault("DefaultDomainID");
            DomainName = reader.GetStringorDefault("DomainName");
            ParentID = reader.GetStringorDefault("PARID");
            OrgID = reader.GetStringorDefault("ORGID");
            GroupID = reader.GetStringorDefault("GRPID");
            Class = reader.GetStringorDefault("CLASS");
        }
    }
}
