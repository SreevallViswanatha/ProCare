using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberPhysicianLockDetailsResultDTO : ILoadFromDataReader
    {
        public string Client { get; set; }
        public string ORGID { get; set; }
        public string GRPID { get; set; }
        public string PLNID { get; set; }
        public string PHYSREQ { get; set; }
        public string ENRID { get; set; }
        public string Person { get; set; }
        public string PHYNPI { get; set; }
        public string PHYDEA { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ORGID = reader.GetStringorDefault("ORGID");
            GRPID = reader.GetStringorDefault("GRPID");
            PLNID = reader.GetStringorDefault("PLNID");
            PHYSREQ = reader.GetStringorDefault("PHYSREQ");
            ENRID = reader.GetStringorDefault("ENRID");
            Person = reader.GetStringorDefault("Person");
            PHYNPI = reader.GetStringorDefault("PHYNPI");
            PHYDEA = reader.GetStringorDefault("PHYDEA");
            PhysicianFirstName = reader.GetStringorDefault("FNAME");
            PhysicianLastName = reader.GetStringorDefault("LNAME");
            EffectiveDate = reader.GetDateTimeorNull("EFFDT");
            TerminationDate = reader.GetDateTimeorNull("TRMDT");
        }
    }
}
