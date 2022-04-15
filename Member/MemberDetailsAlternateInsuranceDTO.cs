using System.Data;
using ProCare.API.Core.Responses;
using ProCare.Common.Data;
using System;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsAlternateInsuranceDTO : ILoadFromDataReader
    {
        public string MedicareCoverageType { get; set; }
        public DateTime? MedicareFromDate { get; set; }
        public string MedicareID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            MedicareCoverageType = reader.GetStringorDefault("MedicareCoverageType");
            MedicareFromDate = reader.GetDateTimeorNull("MedicareFromDate");
            MedicareID = reader.GetStringorDefault("MedicareID");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            MedicareCoverageType = reader.GetStringorDefault(prefix + "MedicareCoverageType");
            MedicareFromDate = reader.GetDateTimeorNull(prefix + "MedicareFromDate");
            MedicareID = reader.GetStringorDefault(prefix + "MedicareID");
        }
    }
}