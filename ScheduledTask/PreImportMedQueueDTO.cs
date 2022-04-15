using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.ScheduledTask
{
    public class PreImportMedQueueDTO : ILoadFromDataReader
    {
        public long PreImportMedQueueID { get; set; }
        public long PreImportMedPagedDataID { get; set; }

        public int ProcessStatusID { get; set; }
        public int VendorID { get; set; }
        public string PatientID { get; set; }
        public Guid OrderID { get; set; }
        public string RawData { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PreImportMedQueueID = reader.GetInt32orDefault("PreImportMedQueueID");
            PreImportMedPagedDataID = reader.GetInt32orDefault("PreImportMedPagedDataID");
            VendorID = reader.GetInt32orDefault("VendorID");
            PatientID = reader.GetStringorDefault("PatientID");
            RawData = reader.GetStringorDefault("RawData");
        }
    }
}
