using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberFavoriteContactDTO : ILoadFromDataReader
    {
        public int UserID { get; set; }
        public long MemberContactID { get; set; }
        public int MemberContactTypeID { get; set; }
        public long EntityIdentifier { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPhone { get; set; }
        public void LoadFromDataReader(IDataReader reader)
        {
            if (reader.ColumnExists("UserID"))
            {
                UserID = reader.GetInt32orDefault("UserID");
            }
            if (reader.ColumnExists("MemberContactID"))
            {
                MemberContactID = reader.GetInt64orDefault("MemberContactID");
            }
            MemberContactTypeID = reader.GetInt32orDefault("MemberContactTypeID");
            EntityIdentifier = reader.GetInt64orDefault("EntityIdentifier");
            ContactName = reader.GetStringorDefault("ContactName").Trim();
            ContactAddress = reader.GetStringorDefault("ContactAddress").Trim();
            ContactPhone = reader.GetStringorDefault("ContactPhone").Trim();
        }
    }
}