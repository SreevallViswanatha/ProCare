using ProCare.API.PBM.Messages;
using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PharmacyDTO : ILoadFromDataReader
    {
        public string Client { get; set; }
        public double Distance { get; set; }
        public string PharmacyID { get; set; }
        public string PharmacyNPI { get; set; }
        public string PharmacyName { get; set; }
        public AddressDTO Address { get; set; }
        public string PharmacyPhone { get; set; }
        public string MondayFridayOpen { get; set; }
        public string MondayFridayClose { get; set; }
        public string SaturdayOpen { get; set; }
        public string SaturdayClose { get; set; }
        public string SundayOpen { get; set; }
        public string SundayClose { get; set; }
        public bool Open24Hours { get; set; }
        public bool Flex90 { get; set; }
        public string DispClass { get; set; }
        public string DispType { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PharmacyID = reader.GetStringorDefault("PHAID");
            PharmacyNPI = reader.GetStringorDefault("PHANPI");
            PharmacyName = reader.GetStringorDefault("NAME");
            PharmacyPhone = reader.GetStringorDefault("PHONE");
            MondayFridayOpen = reader.GetStringorDefault("MondayFridayOpen");
            MondayFridayClose = reader.GetStringorDefault("MondayFridayClose");
            SaturdayOpen = reader.GetStringorDefault("SaturdayOpen");
            SaturdayClose = reader.GetStringorDefault("SaturdayClose");
            SundayOpen = reader.GetStringorDefault("SundayOpen");
            SundayClose = reader.GetStringorDefault("SundayClose");
            DispClass = reader.GetStringorDefault("DISPCLASS");
            DispType = reader.GetStringorDefault("DISPTYPE");
            Distance = reader.GetDoubleorDefault("Distance");

            Open24Hours = false;
            string hours24 = reader.GetStringorDefault("HOURS24");
            if (!string.IsNullOrWhiteSpace(hours24) && hours24.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Open24Hours = true;
            }
            Flex90 = false;
            string flex90 = reader.GetStringorDefault("FLEX90");
            if (!string.IsNullOrWhiteSpace(flex90) && flex90.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Flex90 = true;
            }

            Address = new AddressDTO();
            Address.LoadFromDataReader(reader);
        }
    }
}
