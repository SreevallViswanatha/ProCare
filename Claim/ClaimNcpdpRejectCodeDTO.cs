using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimNcpdpRejectCodeDTO : ILoadFromDataReader
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Code = reader.GetStringorDefault("Code");
            Description = reader.GetStringorDefault("Description");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            Code = reader.GetStringorDefault(prefix + "Code");
            Description = reader.GetStringorDefault(prefix + "Description");
        }
    }
}
