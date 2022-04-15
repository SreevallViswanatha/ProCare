using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class NetworkDTO : ILoadFromDataReader
    {
        public string NetworkID { get; set; }
        public string NetworkType { get; set; }
        public string NetworkName { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            NetworkID = reader.GetStringorDefault("NetworkID");
            NetworkType = reader.GetStringorDefault("NetworkType");
            NetworkName = reader.GetStringorDefault("NetworkName");
        }
    }
}
