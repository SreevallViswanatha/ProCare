using ProCare.Common.Data;

using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class EpisodeDTO : ILoadFromDataReader
    {
        public long EpisodeID { get; set; }
        public string EnrID { get; set; }
        public string CardholderID { get; set; }
        public string CardholderName { get; set; }
        public string EmpName { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyNetwork { get; set; }
        public string AccountManager { get; set; }
        public string PlanID { get; set; }
        public string OriginalPlanID { get; set; }
        public int EpisodeStatusID { get; set; }
        public string EpisodeStatus { get; set; }
        public string AssignedTo { get; set; }
        public int? AssignedToAppUserID { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string NDCREF { get; set; }
        public DateTime? NDCProcDateTime { get; set; }
        public string CarrierNote { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            EpisodeID = reader.GetInt64orDefault("EpisodeID");
            EnrID = reader.GetStringorDefault("EnrID")?.ToUpper();
            CardholderID = reader.GetStringorDefault("CardholderID")?.ToUpper();
            CardholderName = reader.GetStringorDefault("CardholderName")?.ToUpper();
            EmpName = reader.GetStringorDefault("EmpName")?.ToUpper();
            PharmacyName = reader.GetStringorDefault("PharmacyName")?.ToUpper();
            PharmacyNetwork = reader.GetStringorDefault("PharmacyNetwork")?.ToUpper();
            AccountManager = reader.GetStringorDefault("AccountManager")?.ToUpper();
            PlanID = reader.GetStringorDefault("PlnID")?.ToUpper();
            OriginalPlanID = reader.GetStringorDefault("OriginalPlnID")?.ToUpper();
            EpisodeStatusID = reader.GetInt32orDefault("EpisodeStatusID");
            EpisodeStatus = reader.GetStringorNull("EpisodeStatusDesciption")?.ToUpper();
            AssignedTo = reader.GetStringorNull("AssignedTo")?.ToUpper();
            AssignedToAppUserID = reader.GetInt32orNull("AssignedToAppUserID");
            CreatedDateTime = reader.GetDateTimeorNull("CreatedDateTime");
            //NDCREF = reader.GetStringorNull("NDCREF");
            NDCProcDateTime = reader.GetDateTimeorNull("NDCProcDateTime");
            CarrierNote = reader.GetStringorNull("CarrierNote");
        }
    }
}
