using ProCare.API.PBM.Messages;
using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimPharmacyDTO : ILoadFromDataReader
    {
        public string NCPDPPharmacyID { get; set; }
        public string NPI { get; set; }
        public string SubmittedPharmacyIDQualifier { get; set; }
        public string SubmittedPharmacyID { get; set; }
        public string FullName { get; set; }
        public AddressDTO Address { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            NCPDPPharmacyID = reader.GetStringorDefault("NCPDPPharmacyID");
            NPI = reader.GetStringorDefault("NPI");
            SubmittedPharmacyIDQualifier = reader.GetStringorDefault("SubmittedPharmacyIDQualifier");
            SubmittedPharmacyID = reader.GetStringorDefault("SubmittedPharmacyID");
            FullName = reader.GetStringorDefault("FullName");

            Address = new AddressDTO();
            Address.LoadFromDataReader(reader);
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            NCPDPPharmacyID = reader.GetStringorDefault(prefix + "NCPDPPharmacyID");
            NPI = reader.GetStringorDefault(prefix + "NPI");
            SubmittedPharmacyIDQualifier = reader.GetStringorDefault(prefix + "SubmittedPharmacyIDQualifier");
            SubmittedPharmacyID = reader.GetStringorDefault(prefix + "SubmittedPharmacyID");
            FullName = reader.GetStringorDefault(prefix + "FullName");

            Address = new AddressDTO();
            Address.LoadFromDataReaderWithPrefix(reader, prefix);
        }
    }
}
