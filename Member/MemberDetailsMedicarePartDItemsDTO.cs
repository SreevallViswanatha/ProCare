using ProCare.API.Core.Responses;
using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsMedicarePartDItemsDTO : ILoadFromDataReader
    {
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ContractNumber { get; set; }
        public string PBPNumber { get; set; }
        public string SegmentID { get; set; }
        public string EnrollmentSource { get; set; }
        public string SubsidyLevel { get; set; }
        public string CopayCategory { get; set; }
        public DateTime? CopayCategoryEffectiveDate { get; set; }
        public string MedicarePartDProfileIndicator { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Status = "";
            FromDate = null;
            ToDate = null;
            ContractNumber = "";
            PBPNumber = "";
            SegmentID = "";
            EnrollmentSource = "";
            SubsidyLevel = "";
            CopayCategory = "";
            CopayCategoryEffectiveDate = null;
            MedicarePartDProfileIndicator = "";
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            Status = "";
            FromDate = null;
            ToDate = null;
            ContractNumber = "";
            PBPNumber = "";
            SegmentID = "";
            EnrollmentSource = "";
            SubsidyLevel = "";
            CopayCategory = "";
            CopayCategoryEffectiveDate = null;
            MedicarePartDProfileIndicator = "";
        }
    }
}