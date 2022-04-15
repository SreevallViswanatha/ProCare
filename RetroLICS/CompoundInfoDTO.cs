using ProCare.Common.Data;
using ProCare.NCPDP.Telecom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProCare.API.PBM.Repository.DTO
{
    public class CompoundInfoDTO : ILoadFromDataReader
    {
        public DispensingUnitFormIndicator DispensingUnitFormIndicator { get; set; }
        public DosageFormDescriptionCode DosageFormDescriptionCode { get; set; }
        public List<string> ModifierCodes { get; set; }
        public string RouteOfAdministration { get; set; }
        public string IngredientBasisOfCostDetermination { get; set; }
        public decimal? IngredientCost { get; set; }
        public string IngredientProductId { get; set; }
        public string IngredientProductIdQualifier { get; set; }
        public decimal? IngredientQuantity { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            try
            {
                DispensingUnitFormIndicator = parseDispensingUnitFormIndicator(reader.GetStringorDefault("DISPUNIT"));
                DosageFormDescriptionCode = parseDosageFormDescriptionCode(reader.GetStringorDefault("DOSEFORM"));
                ModifierCodes = new List<string>();
                RouteOfAdministration = reader.GetStringorDefault("ROUTE");
            }
            catch (Exception){ }

            IngredientBasisOfCostDetermination = reader.GetStringorDefault("BASISCOST");
            IngredientCost = reader.GetDecimalorDefault("INGRCOST");
            IngredientProductId = reader.GetStringorDefault("NDC");
            //IngredientProductIdQualifier = MapProductIdQualifierFromCodeType(reader.GetStringorDefault("CODETYPE"));
            IngredientProductIdQualifier = "03";
            IngredientQuantity = reader.GetDecimalorDefault("DECIQTY");
        }

        public string MapProductIdQualifierFromCodeType(string codeType)
        {
            List<Tuple<string, string>> qualifierCodes = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("NDC", "03"),
                new Tuple<string, string>("GCN", "15"),
                new Tuple<string, string>("GNN", "99")
            };

            string qualifier = qualifierCodes.FirstOrDefault(x => x.Item1.ToUpper() == codeType.ToUpper())?.Item2 ?? "03";

            return qualifier;
        }

        private DispensingUnitFormIndicator parseDispensingUnitFormIndicator(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                ? (DispensingUnitFormIndicator)Enum.Parse(typeof(DispensingUnitFormIndicator), value)
                : DispensingUnitFormIndicator.Unknown;
        }

        private DosageFormDescriptionCode parseDosageFormDescriptionCode(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                ? (DosageFormDescriptionCode)Enum.Parse(typeof(DosageFormDescriptionCode), value)
                : DosageFormDescriptionCode.NotSpecified;
        }
    }
}
