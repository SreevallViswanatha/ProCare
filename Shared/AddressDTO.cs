using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class AddressDTO : ILoadFromDataReader
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip1 { get; set; }
        public string Zip2 { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            if (reader.ColumnExists("ADDR1"))
            {
                Address1 = reader.GetStringorDefault("ADDR1");
            }
            else if (reader.ColumnExists("ADDR"))
            {
                Address1 = reader.GetStringorDefault("ADDR");
            }
            else if (reader.ColumnExists("ADDRESS_1"))
            {
                Address1 = reader.GetStringorDefault("ADDRESS_1");
            }

            if (reader.ColumnExists("ADDR2"))
            {
                Address2 = reader.GetStringorDefault("ADDR2");
            }
            else if (reader.ColumnExists("ADDRESS_2"))
            {
                Address2 = reader.GetStringorDefault("ADDRESS_2");
            }

            City = reader.GetStringorDefault("CITY");
            State = reader.GetStringorDefault("STATE");

            if (reader.ColumnExists("ZIP4"))
            {
                Zip1 = reader.GetStringorDefault("ZIP");
                Zip2 = reader.GetStringorDefault("ZIP4");
            }
            else
            {
                string zipValue = reader.GetStringorDefault("ZIP");

                if (zipValue.Length >= 5)
                {
                    Zip1 = zipValue.Substring(0, 5);
                }

                if (zipValue.Length == 9)
                {
                    Zip2 = zipValue.Substring(5, 4);
                }
            }
        }
        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            if (reader.ColumnExists(prefix + "ADDR1"))
            {
                Address1 = reader.GetStringorDefault(prefix + "ADDR1");
            }
            else if (reader.ColumnExists(prefix + "ADDR"))
            {
                Address1 = reader.GetStringorDefault(prefix + "ADDR");
            }
            else if (reader.ColumnExists(prefix + "ADDRESS_1"))
            {
                Address1 = reader.GetStringorDefault(prefix + "ADDRESS_1");
            }

            if (reader.ColumnExists(prefix + "ADDR2"))
            {
                Address2 = reader.GetStringorDefault(prefix + "ADDR2");
            }
            else if (reader.ColumnExists(prefix + "ADDRESS_2"))
            {
                Address2 = reader.GetStringorDefault(prefix + "ADDRESS_2");
            }

            City = reader.GetStringorDefault(prefix + "CITY");
            State = reader.GetStringorDefault(prefix + "STATE");

            if (reader.ColumnExists(prefix + "ZIP4"))
            {
                Zip1 = reader.GetStringorDefault(prefix + "ZIP");
                Zip2 = reader.GetStringorDefault(prefix + "ZIP4");
            }
            else
            {
                string zipValue = reader.GetStringorDefault(prefix + "ZIP");

                if (zipValue.Length >= 5)
                {
                    Zip1 = zipValue.Substring(0, 5);
                }

                if (zipValue.Length == 9)
                {
                    Zip2 = zipValue.Substring(5, 4);
                }
            }

        }
        public bool IsEqual(AddressDTO other)
        {
            bool equal = false;

            if (Address1 == other.Address1 &&
                Address2 == other.Address2 &&
                City == other.City &&
                State == other.State &&
                Zip1 == other.Zip1 &&
                Zip2 == other.Zip2)
            {
                equal = true;
            }

            return equal;
        }
    }
}
