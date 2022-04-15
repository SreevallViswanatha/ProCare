using System;
using System.Collections.Generic;
using System.Data;
using ProCare.Common.Data;

namespace ProCare.API.Common.Repository.DTO
{
    public class MasterPatientIndexDTO : ILoadFromDataReader
    {
        public int PrxID { get; set; }
        public string Client { get; set; }
        public string PatientID { get; set; }
        public string CardID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        

        public void LoadFromDataReader(IDataReader reader)
        {
            PrxID = reader.GetInt32orDefault("PrxID");
            Client = reader.GetStringorDefault("ClientName");
            PatientID = reader.GetStringorDefault("PatientID");
            CardID = reader.GetStringorDefault("CardID");
            FirstName = reader.GetStringorDefault("FirstName");
            LastName = reader.GetStringorDefault("LastName");
            DateOfBirth = reader.GetDateTimeorDefault("DateOfBirth", DateTime.MinValue);
            Address = reader.GetStringorDefault("Address");
            City = reader.GetStringorDefault("City");
            State = reader.GetStringorDefault("State");
            ZipCode = reader.GetStringorDefault("ZipCode");
            PhoneNumber = reader.GetStringorDefault("PhoneNumber");
            
        }
    }
}
