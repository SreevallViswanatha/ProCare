using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimPlanInformationDTO : ILoadFromDataReader
    {
        public string PlanID { get; set; }
        public string PlanName { get; set; }
        public DateTime? PlanEffectiveDate { get; set; }
        public DateTime? PlanTerminationDate { get; set; }
        public string SubmittedPriorAuthNumber { get; set; }
        public string PANumber { get; set; }
        public DateTime? PAFromDate { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PlanID = reader.GetStringorDefault("PlanID");
            PlanName = reader.GetStringorDefault("PlanName");
            PlanEffectiveDate = reader.GetDateTimeorNull("PlanEffectiveDate");
            PlanTerminationDate = reader.GetDateTimeorNull("PlanTerminationDate");
            SubmittedPriorAuthNumber = reader.GetStringorDefault("SubmittedPriorAuthNumber");
            PANumber = reader.GetStringorDefault("PANumber");
            PAFromDate = reader.GetDateTimeorNull("PAFromDate");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            PlanID = reader.GetStringorDefault(prefix + "PlanID");
            PlanName = reader.GetStringorDefault(prefix + "PlanName");
            PlanEffectiveDate = reader.GetDateTimeorNull(prefix + "PlanEffectiveDate");
            PlanTerminationDate = reader.GetDateTimeorNull(prefix + "PlanTerminationDate");
            SubmittedPriorAuthNumber = reader.GetStringorDefault(prefix + "SubmittedPriorAuthNumber");
            PANumber = reader.GetStringorDefault(prefix + "PANumber");
            PAFromDate = reader.GetDateTimeorNull(prefix + "PAFromDate");
        }
    }
}
