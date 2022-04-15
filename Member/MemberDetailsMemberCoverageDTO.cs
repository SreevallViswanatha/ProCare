using ProCare.API.Core.Responses;
using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsMemberCoverageDTO : ILoadFromDataReader
    {
        public bool Status { get; set; }
        public string CoverageType { get; set; }
        public string CoverageCategory { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string BIN { get; set; }
        public string PCN { get; set; }
        public string HelpDeskPhone { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            CoverageType = reader.GetStringorDefault("CoverageType");
            CoverageCategory = reader.GetStringorDefault("CoverageCategory");
            EffectiveDate = reader.GetDateTimeorNull("EffectiveDate");
            TerminationDate = reader.GetDateTimeorNull("TerminationDate");
            BIN = reader.GetStringorDefault("BIN");
            PCN = reader.GetStringorDefault("PCN");
            HelpDeskPhone = reader.GetStringorDefault("HelpDeskPhone");

            Status = (EffectiveDate.GetValueOrDefault() <= DateTime.Today) &&
                     (TerminationDate == null || TerminationDate.GetValueOrDefault() > DateTime.Today);
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            CoverageType = reader.GetStringorDefault(prefix + "CoverageType");
            CoverageCategory = reader.GetStringorDefault(prefix + "CoverageCategory");
            EffectiveDate = reader.GetDateTimeorNull(prefix + "EffectiveDate");
            TerminationDate = reader.GetDateTimeorNull(prefix + "TerminationDate");
            BIN = reader.GetStringorDefault(prefix + "BIN");
            PCN = reader.GetStringorDefault(prefix + "PCN");
            HelpDeskPhone = reader.GetStringorDefault(prefix + "HelpDeskPhone");

            Status = (EffectiveDate.GetValueOrDefault() <= DateTime.Today) &&
                     (TerminationDate == null || TerminationDate.GetValueOrDefault() > DateTime.Today);
        }
    }
}