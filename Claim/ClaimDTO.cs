using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimDTO : ILoadFromDataReader
    {
        public string ClaimNumber { get; set; }
        public string RxNumber { get; set; }
        public string FillNumber { get; set; }
        public DateTime? FillDate { get; set; }
        public string ClaimStatus { get; set; }
        public string ClaimType { get; set; }
        //public bool COBIndicator { get; set; }
        public string SubmittedOtherCoverageCode { get; set; }
        public string SubmittedEligibilityClarificationCode { get; set; }
        public string SubmittedCompoundCode { get; set; }
        public string SubmittedClarificationCode { get; set; }
        public string SubmittedLevelofServiceCode { get; set; }
        public string SubmittedBasisofCostDeterminationCode { get; set; }
        public string SubmittedUnitDoseIndicator { get; set; }
        public string SubmittedRxOriginCode { get; set; }
        public DateTime? SubmittedRxWrittenDate { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string BIN { get; set; }
        public string PCN { get; set; }
        public string SubmittedGroup { get; set; }
        public string TransactionCode { get; set; }
        public string Version { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ClaimNumber = reader.GetStringorDefault("ClaimNumber");
            RxNumber = reader.GetStringorDefault("RxNumber");
            FillNumber = reader.GetStringorDefault("FillNumber");
            FillDate = reader.GetDateTimeorNull("FillDate");
            ClaimStatus = reader.GetStringorDefault("ClaimStatus");
            ClaimType = reader.GetStringorDefault("ClaimType");
            //COBIndicator = reader.GetBooleanSafe("COBIndicator");
            SubmittedOtherCoverageCode = reader.GetStringorDefault("SubmittedOtherCoverageCode");
            SubmittedEligibilityClarificationCode = reader.GetStringorDefault("SubmittedEligibilityClarificationCode");
            SubmittedCompoundCode = reader.GetStringorDefault("SubmittedCompoundCode");
            SubmittedClarificationCode = reader.GetStringorDefault("SubmittedClarificationCode");
            SubmittedLevelofServiceCode = reader.GetStringorDefault("SubmittedLevelofServiceCode");
            SubmittedBasisofCostDeterminationCode = reader.GetStringorDefault("SubmittedBasisofCostDeterminationCode");
            SubmittedUnitDoseIndicator = reader.GetStringorDefault("SubmittedUnitDoseIndicator");
            SubmittedRxOriginCode = reader.GetStringorDefault("SubmittedRxOriginCode");
            SubmittedRxWrittenDate = reader.GetDateTimeorNull("SubmittedRxWrittenDate");
            DateSubmitted = reader.GetDateTimeorNull("DateSubmitted");
            BIN = reader.GetStringorDefault("BIN");
            PCN = reader.GetStringorDefault("PCN");
            SubmittedGroup = reader.GetStringorDefault("SubmittedGroup");
            TransactionCode = reader.GetStringorDefault("TransactionCode");
            Version = reader.GetStringorDefault("Version");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            ClaimNumber = reader.GetStringorDefault(prefix + "ClaimNumber");
            RxNumber = reader.GetStringorDefault(prefix + "RxNumber");
            FillNumber = reader.GetStringorDefault(prefix + "FillNumber");
            FillDate = reader.GetDateTimeorNull(prefix + "FillDate");
            ClaimStatus = reader.GetStringorDefault(prefix + "ClaimStatus");
            ClaimType = reader.GetStringorDefault(prefix + "ClaimType");
            //COBIndicator = reader.GetBooleanSafe(prefix + "COBIndicator");
            SubmittedOtherCoverageCode = reader.GetStringorDefault(prefix + "SubmittedOtherCoverageCode");
            SubmittedEligibilityClarificationCode = reader.GetStringorDefault(prefix + "SubmittedEligibilityClarificationCode");
            SubmittedCompoundCode = reader.GetStringorDefault(prefix + "SubmittedCompoundCode");
            SubmittedClarificationCode = reader.GetStringorDefault(prefix + "SubmittedClarificationCode");
            SubmittedLevelofServiceCode = reader.GetStringorDefault(prefix + "SubmittedLevelofServiceCode");
            SubmittedBasisofCostDeterminationCode = reader.GetStringorDefault(prefix + "SubmittedBasisofCostDeterminationCode");
            SubmittedUnitDoseIndicator = reader.GetStringorDefault(prefix + "SubmittedUnitDoseIndicator");
            SubmittedRxOriginCode = reader.GetStringorDefault(prefix + "SubmittedRxOriginCode");
            SubmittedRxWrittenDate = reader.GetDateTimeorNull(prefix + "SubmittedRxWrittenDate");
            DateSubmitted = reader.GetDateTimeorNull(prefix + "DateSubmitted");
            BIN = reader.GetStringorDefault(prefix + "BIN");
            PCN = reader.GetStringorDefault(prefix + "PCN");
            SubmittedGroup = reader.GetStringorDefault(prefix + "SubmittedGroup");
            TransactionCode = reader.GetStringorDefault(prefix + "TransactionCode");
            Version = reader.GetStringorDefault(prefix + "Version");
        }
    }
}
