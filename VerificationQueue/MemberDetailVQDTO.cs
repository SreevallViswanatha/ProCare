namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.API.Core.Helpers;
    using ProCare.Common.Data;
    using System;
    using System.Data;
    using Enums =  ProCare.API.PBM.Messages.Shared.Enums;

    public class MemberDetailVQDTO : ILoadFromDataReader
    {
        public string SubID { get; set; }
        public string PlanID { get; set; }
        public string PlanName { get; set; }
        public string CardID { get; set; }
        public string CardID2 { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public int GenderID { get; set; }
        public string Gender { get; set; }
        public string MemberPhoneNumber { get; set; }
        public AddressExtendedDTO MemberAddress { get; set; }
        public string EffectiveDate { get; set; }
        public string TerminationDate { get; set; }
        public string PlanPhoneNumber { get; set; }
        public AddressExtendedDTO PlanAddress { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            SubID = reader.GetStringorNull("SubId");
            PlanID = reader.GetStringorNull("PlnId");
            PlanName = reader.GetStringorNull("PlnName");
            CardID = reader.GetStringorNull("CardId");
            CardID2 = reader.GetStringorNull("CardId2");
            FirstName = reader.GetStringorNull("FName");
            MiddleInitial = reader.GetStringorNull("MName");
            LastName = reader.GetStringorNull("LName");
            DOB = DateTimeHelper.ConvertDateToString(reader.GetDateTimeorNull("DOB"));
            string sex = reader.GetStringorDefault("Sex");
            GenderID = getGenderFromSexCode(sex);
            Gender = getGenderFromSex(sex);
            MemberPhoneNumber = reader.GetStringorNull("Phone");
            MemberAddress = new AddressExtendedDTO();
            MemberAddress.LoadFromDataReader(reader);
            MemberAddress.StateFull = reader.GetStringorNull("STATEFULL");
            EffectiveDate = DateTimeHelper.ConvertDateToString(reader.GetDateTimeorNull("EffDT"));
            TerminationDate = DateTimeHelper.ConvertDateToString(reader.GetDateTimeorNull("TrmDT"));
            PlanPhoneNumber = reader.GetStringorNull("Pl2Phone");
            PlanAddress = new AddressExtendedDTO();
            PlanAddress.LoadFromDataReaderWithPrefix(reader, "PL2");
            PlanAddress.StateFull = reader.GetStringorNull("PL2STATEFULL");
        }

        private int getGenderFromSexCode(string sexCode)
        {
            switch (sexCode.ToUpper())
            {
                case "M":
                    return (int)Enums.GenderID.Male;
                case "F":
                    return (int)Enums.GenderID.Female;
                default:
                    return (int)Enums.GenderID.Undefined;
            }
        }

        private string getGenderFromSex(string sexCode)
        {
            switch (sexCode.ToUpper())
            {
                case "M":
                    return Enums.GenderID.Male.ToString();
                case "F":
                    return Enums.GenderID.Female.ToString();
                default:
                    return Enums.GenderID.Undefined.ToString();
            }
        }
    }
}
