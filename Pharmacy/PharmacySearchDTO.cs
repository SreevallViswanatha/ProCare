using System.Collections.Generic;
using System.Data;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PharmacySearchDTO : ILoadFromDataReader
    {
        public PharmacyDTO Pharmacy { get; set; }
        public List<NetworkDTO> Networks { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Pharmacy = new PharmacyDTO();
            Pharmacy.LoadFromDataReader(reader);
        }
    }
}
