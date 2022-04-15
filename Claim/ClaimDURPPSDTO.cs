using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimDURPPSDTO : ILoadFromDataReader
    {
        public string ReasonforServiceCode { get; set; }
        public string ProfessionalServiceCode { get; set; }
        public string ResultofServiceCode { get; set; }
        public string LevelofEffort { get; set; }
        public string CoAgentIDQualifier { get; set; }
        public string CoAgentID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ReasonforServiceCode = reader.GetStringorDefault("ReasonforServiceCode");
            ProfessionalServiceCode = reader.GetStringorDefault("ProfessionalServiceCode");
            ResultofServiceCode = reader.GetStringorDefault("ResultofServiceCode");
            LevelofEffort = reader.GetStringorDefault("LevelofEffort");
            CoAgentIDQualifier = reader.GetStringorDefault("CoAgentIDQualifier");
            CoAgentID = reader.GetStringorDefault("CoAgentID");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            ReasonforServiceCode = reader.GetStringorDefault(prefix + "ReasonforServiceCode");
            ProfessionalServiceCode = reader.GetStringorDefault(prefix + "ProfessionalServiceCode");
            ResultofServiceCode = reader.GetStringorDefault(prefix + "ResultofServiceCode");
            LevelofEffort = reader.GetStringorDefault(prefix + "LevelofEffort");
            CoAgentIDQualifier = reader.GetStringorDefault(prefix + "CoAgentIDQualifier");
            CoAgentID = reader.GetStringorDefault(prefix + "CoAgentID");
        }
    }
}
