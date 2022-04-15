using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class UserInfoDTO : ILoadFromDataReader
    {
        public int UserID { get; set; }
        public string ENRID { get; set; }
        public string LoginName { get; set; }
        public string EmailAddress { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool? Validated { get; set; }
        public bool? AllowOthers { get; set; }
        public string BinNumber { get; set; }
        public string Password { get; set; }
        public int LoginCount { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            UserID = reader.GetInt32orDefault("UserID");
            ENRID = reader.GetStringorDefault("ENRID");
            LoginName = reader.GetStringorDefault("LoginName");
            EmailAddress = reader.GetStringorDefault("EmailAddress");
            Question = reader.GetStringorDefault("Question");
            Answer = reader.GetStringorDefault("Answer");
            Validated = reader.GetBooleanorNull("Validated");
            AllowOthers = reader.GetBooleanorNull("AllowOthers");
            BinNumber = reader.GetStringorDefault("BinNumber");
            Password = reader.GetStringorDefault("Password");
            LoginCount = reader.GetInt32orDefault("LoginCount");
        }
    }
}
