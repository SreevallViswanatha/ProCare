namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.Common.Data;
    using System;
    using System.Data;
    public class UserPlanDTO : ILoadFromDataReader
    {
        public string GroupID { get; set; }

        public string GroupName { get; set; }

        public string PlanID { get; set; }

        public string PlanName { get; set; }

        public AddressExtendedDTO Address { get; set; }

        public string Phone { get; set; }

        public string PlanEffectiveDate { get; set; }

        public string PlanTerminationDate { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            GroupID = reader.GetStringorNull("GrpID");
            GroupName = reader.GetStringorNull("GroupName");
            PlanID = reader.GetStringorNull("PlnID");
            PlanName = reader.GetStringorNull("PlanName");
            Phone = reader.GetStringorNull("Phone");
            PlanEffectiveDate = reader.GetDateTimeorNull("EffDt")?.ToShortDateString();
            PlanTerminationDate = reader.GetDateTimeorNull("TrmDt")?.ToShortDateString();
            Address = new AddressExtendedDTO();
            Address.LoadFromDataReader(reader);
            Address.StateFull = reader.GetStringorNull("EmpStateFull");
        }
    }
}
