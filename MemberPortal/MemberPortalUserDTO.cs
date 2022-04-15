using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberPortalUserDTO : ILoadFromDataReader
    {
        public int UserID { get; set; }
        public int DomainID { get; set; }
        public string BinNumber { get; set; }
        public string EnrolleeID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            UserID = reader.GetInt32orDefault("UserID");
            DomainID = reader.GetInt32orDefault("DomainID");
            BinNumber = reader.GetStringorDefault("BinNumber");
            EnrolleeID = reader.GetStringorDefault("ENRID");
        }
    }
}
