using ProCare.Common.Data;
using System.Data;
using System;

namespace ProCare.API.PBM.Repository.DTO
{
    public class AutoLogonDTO : ILoadFromDataReader
    {
        
        public string ENRID { get; set; }
        public int ClientID { get; set; }
        public int DomainID { get; set; }
        public string BIN { get; set; }
        public string CardId { get; set; }
        public string CardID2 { get; set; }
        public DateTime LastLogin { get; set; }


        public void LoadFromDataReader(IDataReader reader)
        {            
            ENRID = reader.GetStringorDefault("ENRID");
            ClientID = reader.GetInt32orDefault("ClientID");
            DomainID = reader.GetInt32orDefault("DomainID");
            BIN = reader.GetStringorDefault("BIN");
            CardId = reader.GetStringorDefault("CardId");
            CardID2 = reader.GetStringorDefault("CardID2");
            LastLogin = reader.GetDateTimeorDefault("LastLogin", DateTime.MinValue);
        }
    }
}
