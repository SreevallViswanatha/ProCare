using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimSubmittedMemberDTO : ILoadFromDataReader
    {
        public string CardholderID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonCode { get; set; }
        public string RelationshipCode { get; set; }
        public string GenderCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PatientLocation { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            CardholderID = reader.GetStringorDefault("CardholderID");
            FirstName = reader.GetStringorDefault("FirstName");
            LastName = reader.GetStringorDefault("LastName");
            PersonCode = reader.GetStringorDefault("PersonCode");
            RelationshipCode = reader.GetStringorDefault("RelationshipCode");
            GenderCode = reader.GetStringorDefault("GenderCode");
            DateOfBirth = reader.GetDateTimeorNull("DateOfBirth");
            PatientLocation = reader.GetStringorDefault("PatientLocation");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            CardholderID = reader.GetStringorDefault(prefix + "CardholderID");
            FirstName = reader.GetStringorDefault(prefix + "FirstName");
            LastName = reader.GetStringorDefault(prefix + "LastName");
            PersonCode = reader.GetStringorDefault(prefix + "PersonCode");
            RelationshipCode = reader.GetStringorDefault(prefix + "RelationshipCode");
            GenderCode = reader.GetStringorDefault(prefix + "GenderCode");
            DateOfBirth = reader.GetDateTimeorNull(prefix + "DateOfBirth");
            PatientLocation = reader.GetStringorDefault(prefix + "PatientLocation");
        }
    }
}
