using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.ScheduledTask
{
    public class PreImportPatientDataDTO : ILoadFromDataReader
    {
        public long PreImportPatientDataID { get; set; }
        public int VendorID { get; set; }
        public int RecordTypeID { get; set; }
        public string PatientID { get; set; }
        public string RawData { get; set; }
        public long PreImportPatientPagedDataID { get; set; }
        public int ProcessStatusID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            if (reader.ColumnExists("PreImportPatientDataID"))
            {
                PreImportPatientDataID = reader.GetInt64orDefault("PreImportPatientDataID");
            }
            if (reader.ColumnExists("VendorID"))
            {
                VendorID = reader.GetInt32orDefault("VendorID");
            }
            if (reader.ColumnExists("RecordTypeID"))
            {
                RecordTypeID = reader.GetInt32orDefault("RecordTypeID");
            }
            if (reader.ColumnExists("PatientID"))
            {
                PatientID = reader.GetStringorDefault("PatientID");
            }
            if (reader.ColumnExists("RawData"))
            {
                RawData = reader.GetStringorDefault("RawData");
            }
            if (reader.ColumnExists("PreImportPatientPagedDataID"))
            {
                PreImportPatientPagedDataID = reader.GetInt64orDefault("PreImportPatientPagedDataID");
            }
        }
    }
}
