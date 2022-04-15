using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimAccumulationInformationDTO : ILoadFromDataReader
    {
        public double Deductible { get; set; }
        public double OOPMax { get; set; }
        public double Benefit { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Deductible = reader.GetDoubleorDefault("Deductible");
            OOPMax = reader.GetDoubleorDefault("OOPMax");
            Benefit = reader.GetDoubleorDefault("Benefit");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            Deductible = reader.GetDoubleorDefault(prefix + "Deductible");
            OOPMax = reader.GetDoubleorDefault(prefix + "OOPMax");
            Benefit = reader.GetDoubleorDefault(prefix + "Benefit");
        }
    }
}
