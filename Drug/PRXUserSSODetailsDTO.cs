using ProCare.Common.Data;
using System.Data;


namespace ProCare.API.PBM.Repository.DTO.Drug
{
    public class PRXUserSSODetailsDTO : ILoadFromDataReader
    {
        public int ClientID { get; set; }
        public string FRMID { get; set; }
        public string FORMULARY { get; set; }
        public int UserID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ClientID = reader.GetInt32orDefault("ClientID");
            FRMID = reader.GetStringorDefault("FRMID");
            FORMULARY = reader.GetStringorDefault("FORMULARY");
            UserID = reader.GetInt32orDefault("UserID");
        }
    }
}

