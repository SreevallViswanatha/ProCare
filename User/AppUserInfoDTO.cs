using ProCare.Common.Data;

using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class AppUserInfoDTO : ILoadFromDataReader
    {
        public int AppUserID { get; set; }
        public string LogonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            AppUserID = reader.GetInt32orDefault("AppUserId");
            LogonID = reader.GetStringorDefault("LogonId");
            FirstName = reader.GetStringorDefault("FirstName");
            LastName = reader.GetStringorDefault("LastName");
        }
    }
}
