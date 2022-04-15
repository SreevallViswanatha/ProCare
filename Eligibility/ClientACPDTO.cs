using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.Eligibility
{
    public class ClientACPDTO : ILoadFromDataReader
    {
        public long Client_ACPID { get; set; }
        public long PRXClientID { get; set; }
        public Boolean Active { get; set; }
        public long DataTypeID { get; set; }
        public long TransactionTypeID { get; set; }
        public Boolean FullReplacement { get; set; }
        public String URL { get; set; }
        public String ADSDatabasePath { get; set; }
        public long InsertAppUserID { get; set; }
        public string UserName { get; set; }


        public void LoadFromDataReader(IDataReader reader)
        {
            Client_ACPID = reader.GetInt64orDefault("Client_ACPID");
            PRXClientID = reader.GetInt64orDefault("PRXClientID");
            Active = reader.GetBooleanSafe("Active");
            DataTypeID = reader.GetInt64orDefault("DataTypeID");
            TransactionTypeID = reader.GetInt64orDefault("TransactionTypeID");
            FullReplacement = reader.GetBooleanSafe("FullReplacement");
            URL = reader.GetStringorDefault("URL");
            ADSDatabasePath = reader.GetStringorDefault("ADSDatabasePath");
            InsertAppUserID = reader.GetInt64orDefault("InsertAppUserID");
        }
    }
}
