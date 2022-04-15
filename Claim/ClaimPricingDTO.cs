using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimPricingDTO : ILoadFromDataReader
    {
        public double IngredientCost { get; set; }
        public double DispensingFee { get; set; }
        public double IncentiveFee { get; set; }
        public double Other { get; set; }
        public double ProfessionalServiceFee { get; set; }
        public double PatientPayAmount { get; set; }
        public double COB { get; set; }
        public double TotalAmountDue { get; set; }
        public double OOPCopay { get; set; }
        public double Deductible { get; set; }
        public double Overcap { get; set; }
        public double ProductSelect { get; set; }
        public double Tax { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            IngredientCost = reader.GetDoubleorDefault("IngredientCost");
            DispensingFee = reader.GetDoubleorDefault("DispensingFee");
            IncentiveFee = reader.GetDoubleorDefault("IncentiveFee");
            Other = reader.GetDoubleorDefault("Other");
            ProfessionalServiceFee = reader.GetDoubleorDefault("ProfessionalServiceFee");
            PatientPayAmount = reader.GetDoubleorDefault("PatientPayAmount");
            COB = reader.GetDoubleorDefault("COB");
            TotalAmountDue = reader.GetDoubleorDefault("TotalAmountDue");
            OOPCopay = reader.GetDoubleorDefault("OOPCopay");
            Deductible = reader.GetDoubleorDefault("Deductible");
            Overcap = reader.GetDoubleorDefault("Overcap");
            ProductSelect = reader.GetDoubleorDefault("ProductSelect");
            Tax = reader.GetDoubleorDefault("Tax");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            IngredientCost = reader.GetDoubleorDefault(prefix + "IngredientCost");
            DispensingFee = reader.GetDoubleorDefault(prefix + "DispensingFee");
            IncentiveFee = reader.GetDoubleorDefault(prefix + "IncentiveFee");
            Other = reader.GetDoubleorDefault(prefix + "Other");
            ProfessionalServiceFee = reader.GetDoubleorDefault(prefix + "ProfessionalServiceFee");
            PatientPayAmount = reader.GetDoubleorDefault(prefix + "PatientPayAmount");
            COB = reader.GetDoubleorDefault(prefix + "COB");
            TotalAmountDue = reader.GetDoubleorDefault(prefix + "TotalAmountDue");
            OOPCopay = reader.GetDoubleorDefault(prefix + "OOPCopay");
            Deductible = reader.GetDoubleorDefault(prefix + "Deductible");
            Overcap = reader.GetDoubleorDefault(prefix + "Overcap");
            ProductSelect = reader.GetDoubleorDefault(prefix + "ProductSelect");
            Tax = reader.GetDoubleorDefault(prefix + "Tax");
        }
    }
}
