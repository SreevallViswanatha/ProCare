using System.Data;
using ProCare.API.Core.Responses;
using ProCare.Common.Data;
using System;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsMemberDiagnosisDTO : ILoadFromDataReader
    {
        public string Client { get; set; }
        public string MemberEnrolleeID { get; set; }
        public string DiagnosisCode { get; set; }
        public string DiagnosisQualifier { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            DiagnosisCode = reader.GetStringorDefault("DiagnosisCode");
            DiagnosisQualifier = reader.GetStringorDefault("DiagnosisQualifier");
        }
    }
}