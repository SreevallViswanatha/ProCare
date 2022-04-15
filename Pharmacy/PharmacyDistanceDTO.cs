using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PharmacyDistanceDTO : ILoadFromDataReader
    {
        public string PharmacyID { get; set; }
        public double Distance { get; set; }
		
        public void LoadFromDataReader(IDataReader reader)
        {
            PharmacyID = reader.GetStringorDefault("PHAID");
            Distance = reader.GetDoubleorDefault("Distance");
        }
    }
}
