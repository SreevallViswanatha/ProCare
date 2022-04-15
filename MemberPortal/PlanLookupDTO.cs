using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PlanLookupDTO : ILoadFromDataReader
    {
        public string PLNID { get; set; }
        public string GRPID { get; set; }
        public DateTime EFFDT { get; set; }
        public DateTime? TRMDT { get; set; }
        public bool Active { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PLNID = reader.GetStringorDefault("PLNID");
            GRPID = reader.GetStringorDefault("GRPID");
            EFFDT = reader.GetDateTimeorDefault("EFFDT", DateTime.MinValue);
            TRMDT = reader.GetDateTimeorNull("TRMDT");
            Active = !TRMDT.HasValue || TRMDT.Value.Date > DateTime.Today;
        }
    }
}
