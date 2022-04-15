using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimProductInformationDTO : ILoadFromDataReader
    {
        public string ProductIDQualifier { get; set; }
        public string ProductID { get; set; }
        public string ProductNameExtension { get; set; }
        public string ProductNameAbbreviation { get; set; }
        public string MaintenanceIndicator { get; set; }
        public string AHFSClassCode { get; set; }
        public string AHFSClassDescription { get; set; }
        public string ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
        public int SubmittedDaysSupply { get; set; }
        public int SubmittedQuantityDispensed { get; set; }
        public double SubmittedMetricQuantity { get; set; }
        public string SubmittedUOM { get; set; }
        public double SubmittedProductSelectionCode { get; set; }
        public string GPI { get; set; }
        public string GCNSequenceNumber { get; set; }
        public string MedispanGenericIndicator { get; set; }
        public string OverrideGenericIndicator { get; set; }
        public string NDCListName { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ProductIDQualifier = reader.GetStringorDefault("ProductIDQualifier");
            ProductID = reader.GetStringorDefault("ProductID");
            ProductNameExtension = reader.GetStringorDefault("ProductNameExtension");
            ProductNameAbbreviation = reader.GetStringorDefault("ProductNameAbbreviation");
            MaintenanceIndicator = reader.GetStringorDefault("MaintenanceIndicator");
            AHFSClassCode = reader.GetStringorDefault("AHFSClassCode");
            AHFSClassDescription = reader.GetStringorDefault("AHFSClassDescription");
            ManufacturerID = reader.GetStringorDefault("ManufacturerID");
            ManufacturerName = reader.GetStringorDefault("ManufacturerName");
            SubmittedDaysSupply = reader.GetInt32orDefault("SubmittedDaysSupply");
            SubmittedQuantityDispensed = reader.GetInt32orDefault("SubmittedQuantityDispensed");
            SubmittedMetricQuantity = reader.GetDoubleorDefault("SubmittedMetricQuantity");
            SubmittedUOM = reader.GetStringorDefault("SubmittedUOM");
            SubmittedProductSelectionCode = reader.GetDoubleorDefault("SubmittedProductSelectionCode");
            GPI = reader.GetStringorDefault("GPI");
            GCNSequenceNumber = reader.GetStringorDefault("GCNSequenceNumber");
            MedispanGenericIndicator = reader.GetStringorDefault("MedispanGenericIndicator");
            OverrideGenericIndicator = reader.GetStringorDefault("OverrideGenericIndicator");
            NDCListName = reader.GetStringorDefault("NDCListName");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            ProductIDQualifier = reader.GetStringorDefault(prefix + "ProductIDQualifier");
            ProductID = reader.GetStringorDefault(prefix + "ProductID");
            ProductNameExtension = reader.GetStringorDefault(prefix + "ProductNameExtension");
            ProductNameAbbreviation = reader.GetStringorDefault(prefix + "ProductNameAbbreviation");
            MaintenanceIndicator = reader.GetStringorDefault(prefix + "MaintenanceIndicator");
            AHFSClassCode = reader.GetStringorDefault(prefix + "AHFSClassCode");
            AHFSClassDescription = reader.GetStringorDefault(prefix + "AHFSClassDescription");
            ManufacturerID = reader.GetStringorDefault(prefix + "ManufacturerID");
            ManufacturerName = reader.GetStringorDefault(prefix + "ManufacturerName");
            SubmittedDaysSupply = reader.GetInt32orDefault(prefix + "SubmittedDaysSupply");
            SubmittedQuantityDispensed = reader.GetInt32orDefault(prefix + "SubmittedQuantityDispensed");
            SubmittedMetricQuantity = reader.GetDoubleorDefault(prefix + "SubmittedMetricQuantity");
            SubmittedUOM = reader.GetStringorDefault(prefix + "SubmittedUOM");
            SubmittedProductSelectionCode = reader.GetDoubleorDefault(prefix + "SubmittedProductSelectionCode");
            GPI = reader.GetStringorDefault(prefix + "GPI");
            GCNSequenceNumber = reader.GetStringorDefault(prefix + "GCNSequenceNumber");
            MedispanGenericIndicator = reader.GetStringorDefault(prefix + "MedispanGenericIndicator");
            OverrideGenericIndicator = reader.GetStringorDefault(prefix + "OverrideGenericIndicator");
            NDCListName = reader.GetStringorDefault(prefix + "NDCListName");
        }
    }
}
