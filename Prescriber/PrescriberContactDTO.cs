using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PrescriberContactDTO : ILoadFromDataReader
    {
        public string Phone { get; set; }
        public string Fax { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Phone = reader.GetStringorDefault("PHONE");
            Fax = reader.GetStringorDefault("FAX");
        }
    }
}
