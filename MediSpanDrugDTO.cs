using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MediSpanDrugDTO : ILoadFromDataReader
    {
        public string NDC { get; set; }
        public string DrugName { get; set; }
        public char RxOTC { get; set; }
        public char MSCode { get; set; }
        public string GPI { get; set; }
        public decimal PkgSize { get; set; }
        public string NADAC_CD { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            if (reader.ColumnExists("NDC"))
            {
                NDC = reader.GetStringorDefault("NDC");
            }

            if (reader.ColumnExists("DrugName"))
            {
                DrugName = reader.GetStringorDefault("DrugName");
            }

            if (reader.ColumnExists("RxOTC"))
            {
                RxOTC = reader.GetCharSafe("RxOTC");
            }

            if (reader.ColumnExists("MSCode"))
            {
                MSCode = reader.GetCharSafe("MSCode");
            }

            if (reader.ColumnExists("GPI"))
            {
                GPI = reader.GetStringorDefault("GPI");
            }

            if (reader.ColumnExists("PkgSize"))
            {
                PkgSize = reader.GetDecimalorDefault("PkgSize");
            }

            if (reader.ColumnExists("NADAC_CD"))
            {
                NADAC_CD = reader.GetStringorDefault("NADAC_CD");
            }
        }
    }
}
