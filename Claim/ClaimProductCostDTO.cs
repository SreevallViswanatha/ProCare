using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimProductCostDTO : ILoadFromDataReader
    {
        public double UnitCost { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            UnitCost = reader.GetDoubleorDefault("UnitCost");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            UnitCost = reader.GetDoubleorDefault(prefix + "UnitCost");
        }
    }
}
