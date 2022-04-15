using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class GroupEligibilityDTO : ILoadFromDataReader
    {
        public bool Active { get; set; }
        public DateTime? GroupEffectiveDate { get; set; }
        public DateTime? GroupTerminationDate { get; set; }
        public string PlanId { get; set; }
        public DateTime? PlanEffectiveDate { get; set; }
        public string Flex1 { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            GroupEffectiveDate = reader.GetDateTimeorNull("GroupEffectiveDate");
            GroupTerminationDate = reader.GetDateTimeorNull("GroupTerminationDate");
            Active = (GroupEffectiveDate.GetValueOrDefault() <= DateTime.Today) &&
                     (GroupTerminationDate == null || GroupTerminationDate.GetValueOrDefault() > DateTime.Today);
            PlanId = reader.GetStringorDefault("PLNID");
            PlanEffectiveDate = reader.GetDateTimeorNull("PlanEffectiveDate");
            Flex1 = reader.GetStringorDefault("FLEX1");
        }
    }
}
