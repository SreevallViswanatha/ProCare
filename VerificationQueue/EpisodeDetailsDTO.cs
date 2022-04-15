namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.Common.Data;

    using System;
    using System.Data;

    public class EpisodeDetailsDTO : ILoadFromDataReader
    {
        public long EpisodeID { get; set; }
        public string EnrID { get; set; }
        public string NDCREF { get; set; }
        public string OriginalPlanID { get; set; }
        public string PlanID { get; set; }
        public int EpisodeStatusID { get; set; }
        public string EpisodeStatus { get; set; }
        public string AssignedTo { get; set; }
        public int? AssignedToAppUserID { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string CarrierNote { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            EpisodeID = reader.GetInt64orDefault("EpisodeID");
            EnrID = reader.GetStringorDefault("EnrID");
            NDCREF = reader.GetStringorNull("NDCREF");
            OriginalPlanID = reader.GetStringorNull("OriginalPlnID");
            PlanID = reader.GetStringorNull("PlnID");
            EpisodeStatusID = reader.GetInt32orDefault("EpisodeStatusID");
            EpisodeStatus = reader.GetStringorNull("Description");
            AssignedToAppUserID = reader.GetInt32orNull("AssignedToAppUserID");
            CreatedDateTime = reader.GetDateTimeorNull("CreatedDateTime");
            AssignedTo = reader.GetStringorNull("AssignedTo");
            CarrierNote = reader.GetStringorNull("CarrierNote");
        }
    }
}
