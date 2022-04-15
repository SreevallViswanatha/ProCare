using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimDiagnosisInformationDTO : ILoadFromDataReader
    {
        public string SubmittedDiagnosisCodeQualifier { get; set; }
        public string SubmittedDiagnosisCode { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            SubmittedDiagnosisCodeQualifier = reader.GetStringorDefault("SubmittedDiagnosisCodeQualifier");
            SubmittedDiagnosisCode = reader.GetStringorDefault("SubmittedDiagnosisCode");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            SubmittedDiagnosisCodeQualifier = reader.GetStringorDefault(prefix + "SubmittedDiagnosisCodeQualifier");
            SubmittedDiagnosisCode = reader.GetStringorDefault(prefix + "SubmittedDiagnosisCode");
        }
    }
}
