using ProCare.Common.Data;

using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class WCClaimQueueItemDTO : ILoadFromDataReader
    {
        public string EnrID { get; set; }
        public string NDCREF { get; set; }
        public void LoadFromDataReader(IDataReader reader)
        {
            NDCREF = reader.GetStringorDefault("NDCREF");
            EnrID = reader.GetStringorDefault("EnrID");
        }
    }
}
