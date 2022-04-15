using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberTerminateDTO : ILoadFromDataReader
    {
        public string SystemId { get; set; }
        public string MemberEnrolleeId { get; set; }
        public string PlanId { get; set; }
        public string Person { get; set; }
        public DateTime? MemberEffectiveDate { get; set; }
        public DateTime? MemberTerminationDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }
        public AddressDTO Address { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            SystemId = reader.GetStringorDefault("SYSID");
            MemberEnrolleeId = reader.GetStringorDefault("ENRID");
            PlanId = reader.GetStringorDefault("PLNID");
            Person = reader.GetStringorDefault("PERSON");
            MemberEffectiveDate = reader.GetDateTimeorNull("EFFDT");
            MemberTerminationDate = reader.GetDateTimeorNull("TRMDT");
            FirstName = reader.GetStringorDefault("FNAME");
            MiddleInitial = reader.GetStringorDefault("MNAME");
            LastName = reader.GetStringorDefault("LNAME");
            Gender = reader.GetStringorDefault("SEX");
            DateOfBirth = reader.GetDateTimeorDefault("DOB", DateTime.MinValue);
            Phone = reader.GetStringorDefault("PHONE");
            Active = (MemberEffectiveDate.GetValueOrDefault() <= DateTime.Today) &&
                     (MemberTerminationDate == null || MemberTerminationDate.GetValueOrDefault() > DateTime.Today);

            Address = new AddressDTO();
            Address.LoadFromDataReader(reader);
        }
    }
}
