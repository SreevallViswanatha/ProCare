using System.Collections.Generic;
using System.Data;
using ProCare.API.Core.Responses;
using ProCare.API.PBM.Messages;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsResultDTO : ILoadFromDataReader
    {
        public string TableName { get; set; }
        public string Client { get; set; }
        public MemberDetailsMemberDTO MemberDetail { get; set; }
        public AddressDTO Address { get; set; }
        public MemberDetailsAlternateInsuranceDTO AlternateInsurance { get; set; }
        public MemberDetailsMemberCoverageDTO MemberCoverage { get; set; }
        public List<MemberDetailsMemberDiagnosisDTO> HealthProfile { get; set; }
        public MemberDetailsIDCardInfoDTO IDCard { get; set; }
        public MemberDetailsCopayOverrideDTO CopayOverride { get; set; }
        public MemberDetailsPlanInfoDTO PlanInfo { get; set; }
        public MemberDetailsMedicarePartDItemsDTO MedicarePartDItems { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            MemberDetail = new MemberDetailsMemberDTO();
            Address = new AddressDTO();
            AlternateInsurance = new MemberDetailsAlternateInsuranceDTO();
            MemberCoverage = new MemberDetailsMemberCoverageDTO();
            HealthProfile = new List<MemberDetailsMemberDiagnosisDTO>();
            IDCard = new MemberDetailsIDCardInfoDTO();
            CopayOverride = new MemberDetailsCopayOverrideDTO();
            PlanInfo = new MemberDetailsPlanInfoDTO();
            MedicarePartDItems = new MemberDetailsMedicarePartDItemsDTO();

            MemberDetail.LoadFromDataReaderWithPrefix(reader, "MemberDetail");
            Address.LoadFromDataReaderWithPrefix(reader, "Address");
            AlternateInsurance.LoadFromDataReaderWithPrefix(reader, "AlternateInsurance");
            MemberCoverage.LoadFromDataReaderWithPrefix(reader, "MemberCoverage");
            IDCard.LoadFromDataReaderWithPrefix(reader, "IDCard");
            CopayOverride.LoadFromDataReaderWithPrefix(reader, "CopayOverride");
            PlanInfo.LoadFromDataReaderWithPrefix(reader, "PlanInfo");
            MedicarePartDItems.LoadFromDataReaderWithPrefix(reader, "MedicarePartD");
        }
    }
}