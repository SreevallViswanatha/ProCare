using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.Eligibility
{
    public class SuspensionDTO : ILoadFromDataReader
    {
        public string TABLENAME { get; set; }
        public string LINKKEY { get; set; }
        public DateTime? CUTOFFDATE { get; set; }
        public string CUTOFFMETH { get; set; }
        public string JOURNAL { get; set; }
        public string SYSID { get; set; }
        public string CHANGEDBY { get; set; }
        public DateTime DATE { get; set; }
        public string TIME { get; set; }
        public string USERNAME { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            TABLENAME = reader.GetStringorDefault("TABLENAME");
            LINKKEY = reader.GetStringorDefault("LINKKEY");
            CUTOFFDATE = reader.GetDateTimeorNull("CUTOFFDATE");
            CUTOFFMETH = reader.GetStringorDefault("CUTOFFMETH");
            JOURNAL = reader.GetStringorDefault("JOURNAL");
            SYSID = reader.GetStringorDefault("SYSID");

            try
            {
                CHANGEDBY = reader.GetStringorDefault("CHANGEDBY");
                DATE = reader.GetDateTimeorDefault("DATE", DateTime.MinValue);
                TIME = reader.GetStringorDefault("TIME");
                USERNAME = reader.GetStringorDefault("USERNAME");
            }
            catch (Exception)
            {

            }
        }
    }
}
