using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO.ScheduledTask
{
    public class PreImportNetsmartMedQueueDTO : ILoadFromDataReader
    {
        public long PreImportNetsmartSOTHMedQueueID { get; set; }
        public int ExternalPatientID { get; set; }
        public int ProcessStatusID { get; set; }
        public long ImportID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PreImportNetsmartSOTHMedQueueID = reader.GetInt64orDefault("PreImportNetsmartSOTHMedQueueID");
            ExternalPatientID = reader.GetInt32orDefault("ExternalPatientID");
            ProcessStatusID = reader.GetInt32orDefault("ProcessStatusID");
            ImportID = reader.GetInt64orDefault("ImportID");
        }
    }
}
