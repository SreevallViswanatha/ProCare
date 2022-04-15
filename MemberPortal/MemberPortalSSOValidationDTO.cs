using System;
using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberPortalSSOValidationDTO : ILoadFromDataReader
    {
        public string CardID { get; set; }
        public string CardID2 { get; set; }
        public string Person { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            CardID = reader.GetStringorDefault("CardID");
            CardID2 = reader.GetStringorDefault("CardID2");
            Person = reader.GetStringorDefault("Person");
            EffectiveDate = reader.GetDateTimeorNull("EFFDT");
            TerminationDate = reader.GetDateTimeorNull("TRMDT");
        }
    }
}
