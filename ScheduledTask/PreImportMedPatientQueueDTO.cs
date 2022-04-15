using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.ScheduledTask
{
    public class PreImportMedPatientQueueDTO : ILoadFromDataReader
    {
        public long PreImportMedPatientQueueID { get; set; }
        public int VendorID { get; set; }
        public string PatientID { get; set; }
        public Guid BatchGuid { get; set; }
        public int ProcessStatusID { get; set; }
        public DateTime? MedLastPullTime { get; set; }
        public DateTime InsertDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PreImportMedPatientQueueID = reader.GetInt64orDefault("PreImportMedPatientQueueID");
            PatientID = reader.GetStringorDefault("PatientID");
            MedLastPullTime = reader.GetDateTimeorDefault("MedLastPullTime", DateTime.MinValue);
            if (MedLastPullTime == DateTime.MinValue)
            {
                MedLastPullTime = null;
            }


        }
    }
}
