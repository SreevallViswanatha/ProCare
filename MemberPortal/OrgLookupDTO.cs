using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class OrgLookupDTO : MemberPortalLookupDTO
    {
        public string ORGID { get; set; }
        public string PARID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ORGID = reader.GetStringorDefault("ORGID");
            PARID = reader.GetStringorDefault("PARID");
            base.LoadFromDataReader(reader);
        }
    }
}
