using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.ScheduledTask
{
    public class PreImportPatientQueueDTO : ILoadFromDataReader
    {
        public long PreImportPatientQueueID { get; set; }
        public int ProcessStatusID { get; set; }
        public long PreImportPatientPagedDataID { get; set; }
        public int VendorID { get; set; }
        public Guid BatchGuid { get; set; }
        public int RecordTypeID { get; set; }
        public string RawData { get; set; }
        public DateTime InsertDateTime { get; set; }


        public void LoadFromDataReader(IDataReader reader)
        {
            PreImportPatientQueueID = reader.GetInt64orDefault("PreImportPatientQueueID");
            ProcessStatusID = reader.GetInt32orDefault("ProcessStatusID");
            PreImportPatientPagedDataID = reader.GetInt64orDefault("PreImportPatientPagedDataID");
            VendorID = reader.GetInt32orDefault("VendorID");
            BatchGuid = reader.GetGuidorDefault("BatchGuid", Guid.Empty);
            RecordTypeID = reader.GetInt32orDefault("RecordTypeID");
            RawData = reader.GetStringorDefault("RawData");
            InsertDateTime = reader.GetDateTimeorDefault("InsertDateTime", DateTime.MinValue);
        }
    }
}
