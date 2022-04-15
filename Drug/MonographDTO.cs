using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MonographDTO : ILoadFromDataReader
    {
        public string GPI14 { get; set; }
        public string NDC { get; set; }
        public string Monograph { get; set; }
        public string Copyright { get; set; }
        public void LoadFromDataReader(IDataReader reader)
        {
            Monograph = reader.GetStringorDefault("Monograph",string.Empty);
            Copyright = reader.GetStringorDefault("Copyright", string.Empty);
        }

    }
}
