using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO.Clients.Shared
{
    public class ClientWithConfigurationsDTO : ILoadFromDataReader
    {
        public long ClientID { get; set; }

        public long PRXClientID { get; set; }

        public long ClientConfigurationID { get; set; }

        public long DataTypeID { get; set; }

        public long TransactionTypeID { get; set; }

        public string PlanID { get; set; }

        public string URL { get; set; }

        public string ADSDatabasePath { get; set; }

        public string PoetsDatabasePath { get; set; }

        public long ClientInsertAppUserID { get; set; }

        public DateTime ClientInsertTime { get; set; }

        public long ClientUpdateAppUserID { get; set; }

        public DateTime ClientUpdateTime { get; set; }

        public long ClientDeleteAppUserID { get; set; }

        public DateTime ClientDeleteTime { get; set; }

        public long ClientClientConfigurationID { get; set; }

        public string XSD { get; set; }

        public bool TermDateOffset { get; set; }

        public string Encryption { get; set; }

        public bool AutoTerminate { get; set; }

        public bool DOBPresent { get; set; }

        public bool EligOverDefault { get; set; }

        public bool FlexValidations { get; set; }

        public bool UseOldPerson { get; set; }

        public bool TerminateDependents { get; set; }

        public int MinSpouseAge { get; set; }

        public bool BatchPrintCards { get; set; }

        public long ClientConfigurationInsertAppUserID { get; set; }

        public DateTime ClientConfigurationInsertTime { get; set; }

        public long ClientConfigurationUpdateAppUserID { get; set; }

        public DateTime ClientConfigurationUpdateTime { get; set; }

        public long ClientConfigurationDeleteAppUserID { get; set; }

        public DateTime ClientConfigurationDeleteTime { get; set; }


        public void LoadFromDataReader(IDataReader reader)
        {
            ClientID = reader.GetInt64orDefault("ClientID");
            PRXClientID = reader.GetInt64orDefault("PRXClientID");
            ClientConfigurationID = reader.GetInt64orDefault("ClientConfigurationID");
            DataTypeID = reader.GetInt64orDefault("DataTypeID");
            TransactionTypeID = reader.GetInt64orDefault("TransactionTypeID");
            PlanID = reader.GetStringorDefault("PlanID");
            URL = reader.GetStringorDefault("URL");
            ADSDatabasePath = reader.GetStringorDefault("ADSDatabasePath");
            PoetsDatabasePath = reader.GetStringorDefault("PoetsDatabasePath");
            ClientInsertAppUserID = reader.GetInt64orDefault("ClientInsertAppUserID");
            ClientInsertTime = reader.GetDateTimeorDefault("ClientInsertTime", DateTime.MinValue);
            ClientUpdateAppUserID = reader.GetInt64orDefault("ClientUpdateAppUserID");
            ClientUpdateTime = reader.GetDateTimeorDefault("ClientUpdateTime", DateTime.MinValue);
            ClientDeleteAppUserID = reader.GetInt64orDefault("ClientDeleteAppUserID");
            ClientDeleteTime = reader.GetDateTimeorDefault("ClientDeleteTime", DateTime.MinValue);
            ClientConfigurationID = reader.GetInt64orDefault("ClientConfigurationID");
            XSD = reader.GetStringorDefault("XSD");
            TermDateOffset = reader.GetBooleanSafe("TermDateOffset");
            Encryption = reader.GetStringorDefault("Encryption");
            AutoTerminate = reader.GetBooleanSafe("AutoTerminate");
            DOBPresent = reader.GetBooleanSafe("DOBPresent");
            EligOverDefault = reader.GetBooleanSafe("EligOverDefault");
            FlexValidations = reader.GetBooleanSafe("FlexValidations");
            UseOldPerson = reader.GetBooleanSafe("UseOldPerson");
            TerminateDependents = reader.GetBooleanSafe("TerminateDependents");
            MinSpouseAge = reader.GetInt32orDefault("MinSpouseAge");
            BatchPrintCards = reader.GetBooleanSafe("BatchPrintCards");
            ClientConfigurationInsertAppUserID = reader.GetInt64orDefault("ClientConfigurationInsertAppUserID");
            ClientConfigurationInsertTime = reader.GetDateTimeorDefault("ClientConfigurationInsertTime", DateTime.MinValue);
            ClientConfigurationUpdateAppUserID = reader.GetInt64orDefault("ClientConfigurationUpdateAppUserID");
            ClientConfigurationUpdateTime = reader.GetDateTimeorDefault("ClientConfigurationUpdateTime", DateTime.MinValue);
            ClientConfigurationDeleteAppUserID = reader.GetInt64orDefault("ClientConfigurationDeleteAppUserID");
            ClientConfigurationDeleteTime = reader.GetDateTimeorDefault("ClientConfigurationDeleteTime", DateTime.MinValue);
        }
    }
}
