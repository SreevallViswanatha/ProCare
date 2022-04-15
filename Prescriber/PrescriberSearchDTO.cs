using ProCare.API.PBM.Messages;
using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PrescriberSearchDTO : ILoadFromDataReader
    {
        public string PrescriberId { get; set; }
        public string PrescriberIdQualifier { get; set; }
        public string PrescriberState { get; set; }
        public string State { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public string Phone { get; set; }
        public AddressDTO Address { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PrescriberId = reader.GetStringorDefault("PHYNPI");
            PrescriberIdQualifier = "01";
            PrescriberState = reader.GetStringorDefault("STATE");
            FirstName = reader.GetStringorDefault("FNAME");
            MiddleInitial = reader.GetStringorDefault("MNAME");
            LastName = reader.GetStringorDefault("LNAME");
            DeactivationDate = reader.GetDateTimeorNull("DEAC_DATE");
            Phone = reader.GetStringorDefault("PHONE");

            Address = new AddressDTO();
            Address.LoadFromDataReader(reader);
        }
    }
}
