using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class GroupLookupDTO : MemberPortalLookupDTO
    {
        public string GRPID { get; set; }
        public string ORGID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            GRPID = reader.GetStringorDefault("GRPID");
            ORGID = reader.GetStringorDefault("ORGID");
            base.LoadFromDataReader(reader);
        }
    }
}
