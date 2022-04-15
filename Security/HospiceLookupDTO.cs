using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class HospiceLookupDTO : ILoadFromDataReader
    {
        public long? HospiceID { get; set; }
        public string HospiceName { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            HospiceID = reader.GetInt64orNull("HospiceID");
            HospiceName = reader.GetStringorDefault("HospiceName");

            HospiceName = HospiceName?.Trim();
        }
    }
}
