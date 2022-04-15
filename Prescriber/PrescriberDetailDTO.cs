using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PrescriberDetailDTO : ILoadFromDataReader
    {
        public string PrescriberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public AddressDTO Address { get; set; }
        public PrescriberContactDTO Contact { get; set; }
        public string TaxonomyCode { get; set; }
        public string TaxonomyName { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            if (reader.ColumnExists("PHYNPI"))
            {
                PrescriberID = reader.GetStringorDefault("PHYNPI");
            }
            if (reader.ColumnExists("DEA"))
            {
                PrescriberID = reader.GetStringorDefault("DEA");
            }

            FirstName = reader.GetStringorDefault("FNAME");
            LastName = reader.GetStringorDefault("LNAME");

            MiddleInitial = reader.GetStringorDefault("MNAME");
            if (MiddleInitial?.Length > 0)
            { 
                MiddleInitial = MiddleInitial.Substring(0, 1);
            }

            DeactivationDate = reader.GetDateTimeorNull("DEAC_DATE");

            Address = new AddressDTO();
            Address.LoadFromDataReader(reader);

            Contact = new PrescriberContactDTO();
            Contact.LoadFromDataReader(reader);

            TaxonomyCode = reader.GetStringorDefault("TAXONOMY");
            TaxonomyName = reader.GetStringorDefault("TAX_CLASS");
        }
    }
}
