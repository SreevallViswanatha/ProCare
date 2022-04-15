using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimAdjustmentPaymentInformationDTO : ILoadFromDataReader
    {
        public double GenericGapCoverage { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            GenericGapCoverage = reader.GetDoubleorDefault("GenericGapCoverage");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            GenericGapCoverage = reader.GetDoubleorDefault(prefix + "GenericGapCoverage");
        }
    }
}
