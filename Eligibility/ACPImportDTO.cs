using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.Eligibility
{
    public class ACPImportDTO : ILoadFromDataReader
    {
        public long Import_ACPID { get; set; }

        public long TransactionTypeID { get; set; }

        public long ClientID { get; set; }

        public long ImportStatusID { get; set; }

        public string RawData { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime CompletedTime { get; set; }

        public string ReturnValue { get; set; }

        public string WarningMessage { get; set; }

        public string ErrorMessage { get; set; }

        public string RecIdentifier { get; set; }

        public string RecordAction { get; set; }

        public string NewBalance { get; set; }

        public long InsertAppUserID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Import_ACPID = reader.GetInt64orDefault("ImportID");
            TransactionTypeID = reader.GetInt64orDefault("TransactionTypeID");
            ClientID = reader.GetInt64orDefault("ClientID");
            ImportStatusID = reader.GetInt64orDefault("ImportStatusID");
            RawData = reader.GetStringorDefault("RawData");
            CreatedTime = reader.GetDateTimeorDefault("CreatedTime", DateTime.MinValue);
            CompletedTime = reader.GetDateTimeorDefault("CompletedTime", DateTime.MinValue);
            ReturnValue = reader.GetStringorDefault("ReturnValue");
            WarningMessage = reader.GetStringorDefault("WarningMessage");
            ErrorMessage = reader.GetStringorDefault("ErrorMessage");
            RecIdentifier = reader.GetStringorDefault("RecIdentifier");
            RecordAction = reader.GetStringorDefault("RecordAction");
            NewBalance = reader.GetStringorDefault("NewBalance");
            InsertAppUserID = reader.GetInt64orDefault("InsertAppUserID");
        }
    }
}
