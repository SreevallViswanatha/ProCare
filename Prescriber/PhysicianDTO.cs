using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PhysicianDTO : ILoadFromDataReader
    {
        public string SourceTable { get; set; }
        public string NPI { get; set; }
        public string DEA { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PHYID { get; set; }
        public string SYSID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            SourceTable = reader.GetStringorDefault("SourceTable");
            NPI = reader.GetStringorDefault("PHYNPI");
            DEA = reader.GetStringorDefault("PHYDEA");
            FirstName = reader.GetStringorDefault("FNAME");
            LastName = reader.GetStringorDefault("LNAME");
			PHYID = reader.GetStringorDefault("PHYID");
            SYSID = reader.GetStringorDefault("SYSID");
        }
    }
}
