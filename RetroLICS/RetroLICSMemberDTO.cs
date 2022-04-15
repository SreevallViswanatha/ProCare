using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class RetroLICSMemberDTO : ILoadFromDataReader
    {
        public string CARDID { get; set; }
        public string CARDID2 { get; set; }
        public string ENRID { get; set; }
        public string SYSID { get; set; }
        public DateTime? StartDate { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            CARDID = reader.GetStringorDefault("CARDID");
            CARDID2 = reader.GetStringorDefault("CARDID2");
            ENRID = reader.GetStringorDefault("ENRID");
            SYSID = reader.GetStringorDefault("SYSID");
            StartDate = reader.GetDateTimeorNull("EFFDT");
        }
    }
}
