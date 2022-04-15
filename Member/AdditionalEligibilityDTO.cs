using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class AdditionalEligibilityDTO : ILoadFromDataReader
    {
        public string Flex2 { get; set; }
        public string Flex3 { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Flex2 = reader.GetStringorDefault("FLEX2");
            Flex3 = reader.GetStringorDefault("FLEX3");
        }
    }
}
