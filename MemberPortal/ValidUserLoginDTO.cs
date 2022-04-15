using System;
using ProCare.Common.Data;
using System.Data;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ValidUserLoginDTO : ILoadFromDataReader
    {
        public string Token { get; set; }
        public string CardID1And2 { get; set; }
        public string PLNID { get; set; }
        public string GRPID { get; set; }
        public string ORGID { get; set; }
        public string PARID { get; set; }
        public string PCN { get; set; }
        public string SUBGRPID { get; set; }
        public DateTime PlanEffDate { get; set; }
        public DateTime? PlanTermDate { get; set; }
        public DateTime GroupEffDate { get; set; }
        public DateTime? GroupTermDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderCode Gender { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Token = reader.GetStringorDefault("Token");
            CardID1And2 = reader.GetStringorDefault("CardID1And2");
            PLNID = reader.GetStringorDefault("PLNID");
            GRPID = reader.GetStringorDefault("GRPID");
            ORGID = reader.GetStringorDefault("ORGID");
            PARID = reader.GetStringorDefault("PARID");
            PCN = reader.GetStringorDefault("PCN");
            SUBGRPID = reader.GetStringorDefault("SUBGRPID");
            PlanEffDate = reader.GetDateTimeorDefault("PlanEffDate", DateTime.MinValue);
            PlanTermDate = reader.GetDateTimeorNull("PlanTermDate");
            GroupEffDate = reader.GetDateTimeorDefault("GroupEffDate", DateTime.MinValue);
            GroupTermDate = reader.GetDateTimeorNull("GroupTermDate");
            FirstName = reader.GetStringorDefault("FNAME").Trim();
            MiddleName = reader.GetStringorDefault("MNAME")?.Trim();
            LastName = reader.GetStringorDefault("LNAME").Trim();
            DateOfBirth = reader.GetDateTimeorDefault("DateOfBirth", DateTime.MinValue);
            Gender= (GenderCode)Enum.Parse(typeof(GenderCode), reader.GetStringorDefault("Gender").Trim()); 
        }
    }
}
