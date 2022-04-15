using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PharmacyNetworkDTO : ILoadFromDataReader
    {
        public PharmacyDTO Pharmacy { get; set; }
        public NetworkDTO Network { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Pharmacy = new PharmacyDTO();
            Pharmacy.LoadFromDataReader(reader);

            Network = new NetworkDTO();
            Network.LoadFromDataReader(reader);
        }
    }
}
