namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.Common.Data;

    using System;
    using System.Data;

    public class WCClaimsDetailDTO : ILoadFromDataReader
    {
        public string EnrID { get; set; }
        public string NDCREF { get; set; }
        public string CardholderID { get; set; }
        public string CardholderName { get; set; }
        public string EmpName { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyNetwork { get; set; }
        public string AccountManager { get; set; }
        public string PlnID { get; set; }
        public string NDCPROCDATE { get; set; }
        public string NDCPROCTIME { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            EnrID = reader.GetStringorDefault("EnrID");
            NDCREF = reader.GetStringorNull("NDCREF");
            CardholderID = reader.GetStringorDefault("CARDHOLDER_ID");
            CardholderName = reader.GetStringorDefault("CARDHOLDER_NAME");
            EmpName = reader.GetStringorDefault("EMPLOYER_NAME");
            PharmacyName = reader.GetStringorDefault("PHARMACY_NAME");
            PharmacyNetwork = reader.GetStringorDefault("PHARMACY_NETWORK");
            AccountManager = reader.GetStringorDefault("ACCOUNT_MANAGER");
            PlnID = reader.GetStringorDefault("PLNID");
            NDCPROCDATE = reader.GetStringorDefault("NDCPROCDATE");
            NDCPROCTIME = reader.GetStringorDefault("NDCPROCTIME");
        }
    }
}
