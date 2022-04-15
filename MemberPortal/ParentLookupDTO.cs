using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ParentLookupDTO : MemberPortalLookupDTO
    {
        public string PARID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PARID = reader.GetStringorDefault("PARID");
            base.LoadFromDataReader(reader);
        }
    }
}
