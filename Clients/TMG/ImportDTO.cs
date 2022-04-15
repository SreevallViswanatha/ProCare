using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO.Clients.TMG
{
    public class ImportDTO : ILoadFromDataReader
    {
        public long ImportID { get; set; }

        public long TransactionTypeID { get; set; }

        public long ClientID { get; set; }

        public long ImportStatusID { get; set; }

        public long PreImportID { get; set; }

        public string PlanID { get; set; }

        public string PatName { get; set; }

        public string RawData { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime CompletedTime { get; set; }

        public string ReturnValue { get; set; }

        public string WarningMessage { get; set; }

        public string ErrorMessage { get; set; }

        public string RecIdentifier { get; set; }

        public string RecordAction { get; set; }

        public long InsertAppUserID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ImportID = reader.GetInt64orDefault("ImportID");
            TransactionTypeID = reader.GetInt64orDefault("TransactionTypeID");
            ClientID = reader.GetInt64orDefault("ClientID");
            ImportStatusID = reader.GetInt64orDefault("ImportStatusID");
            PreImportID = reader.GetInt64orDefault("PreImportID");
            PlanID = reader.GetStringorDefault("PlanID");
            PatName = reader.GetStringorDefault("PatName");
            RawData = reader.GetStringorDefault("RawData");
            CreatedTime = reader.GetDateTimeorDefault("CreatedTime", DateTime.MinValue);
            CompletedTime = reader.GetDateTimeorDefault("CompletedTime", DateTime.MinValue);
            ReturnValue = reader.GetStringorDefault("ReturnValue");
            WarningMessage = reader.GetStringorDefault("WarningMessage");
            ErrorMessage = reader.GetStringorDefault("ErrorMessage");
            RecIdentifier = reader.GetStringorDefault("RecIdentifier");
            RecordAction = reader.GetStringorDefault("RecordAction");
            InsertAppUserID = reader.GetInt64orDefault("InsertAppUserID");
        }
    }
}
