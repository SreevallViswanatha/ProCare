using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class FeeScheduleDTO : ILoadFromDataReader
    {
        public decimal Charge { get; set; }
        public decimal DispensingFee { get; set; }
        public decimal IngredientCost { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal AverageWholesalePrice { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Charge = (decimal)reader["FSCHARGE"];
            DispensingFee = (decimal)reader["FSDISPFEE"];
            IngredientCost = (decimal)reader["FSINGRCOST"];
            TotalPrice = (decimal)reader["FSTOTPRC"];
            AverageWholesalePrice = (decimal)reader["UNITAWP"];
        }
    }
}
