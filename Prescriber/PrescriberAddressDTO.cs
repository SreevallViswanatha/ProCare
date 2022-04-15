using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PrescriberAddressDTO : ILoadFromDataReader
    {
        public string PaqQualifier { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip1 { get; set; }
        public string Zip2 { get; set; }
        public PrescriberContactDTO Contact { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PaqQualifier = reader.GetStringorDefault("RECCODE");

            Address1 = reader.GetStringorDefault("ADDRESS_1");
            Address2 = reader.GetStringorDefault("ADDRESS_2");
            City = reader.GetStringorDefault("CITY");
            State = reader.GetStringorDefault("STATE");

            string zipValue = reader.GetStringorDefault("ZIP");
            if (zipValue.Length >= 5)
            {
                Zip1 = zipValue.Substring(0, 5);
            }

            if (zipValue.Length == 9)
            {
                Zip2 = zipValue.Substring(5, 4);
            }

            Contact = new PrescriberContactDTO();
            Contact.LoadFromDataReader(reader);
        }

    }
}
