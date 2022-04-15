namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.Common.Data;
    using System;
    using System.Data;
    public class ChangeLogDetailDTO : ILoadFromDataReader
    {
        public int LogFileID { get; set; }
        public string LogFile { get; set; }

        public string ChangedField { get; set; }

        public string OldValue { get; set; }

        public string ReadableOldValue { get; set; }

        public string NewValue { get; set; }

        public string ReadableNewValue { get; set; }

        public string ChangedBy { get; set; }

        public string ChangedDateTime { get; set; }

        public string LogSysID { get; set; }


        public void LoadFromDataReader(IDataReader reader)
        {
            LogFile = reader.GetStringorNull("logfile");
            ChangedField = reader.GetStringorNull("fldchanged");
            OldValue = reader.GetStringorNull("oldval");
            ReadableOldValue = reader.GetStringorNull("ReadableOldValue");
            NewValue = reader.GetStringorNull("newval");
            ReadableNewValue = reader.GetStringorNull("ReadableNewValue");
            ChangedBy = reader.GetStringorNull("ChangedBy");
            ChangedDateTime = reader.GetStringorNull("ChangedDateTime");
            LogSysID = reader.GetStringorNull("LogSYSID");
             
        }
    }
}
