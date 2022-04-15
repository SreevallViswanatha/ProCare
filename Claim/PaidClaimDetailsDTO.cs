using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PaidClaimDetailsDTO : ILoadFromDataReader
    {
        public string Client { get; set; }
        public ClaimDTO Claim { get; set; }
        public ClaimSubmittedMemberDTO SubmittedMember { get; set; }
        public ClaimActualMemberDTO ActualMember { get; set; }
        public ClaimPharmacyDTO Pharmacy { get; set; }
        public ClaimPrescriberDTO Prescriber { get; set; }
        public ClaimProductInformationDTO ProductInformation { get; set; }
        public ClaimProductCostDTO ProductCost { get; set; }
        public ClaimPlanInformationDTO PlanInformation { get; set; }
        //public ClaimRejectInformationDTO RejectInformation { get; set; }
        public ClaimDiagnosisInformationDTO DiagnosisInformation { get; set; }
        public ClaimPricingDTO SubmittedPricing { get; set; }
        public ClaimPricingDTO CalculatedPricing { get; set; }
        public ClaimAccumulationInformationDTO IndividualAccumulationInformation { get; set; }
        public ClaimAccumulationInformationDTO FamilyAccumulationInformation { get; set; }
        public ClaimDURPPSDTO DURPPS { get; set; }
        public ClaimPaymentInformationDTO PaymentInformation { get; set; }
        public ClaimAdjustmentPaymentInformationDTO AdjustmentPaymentInformation { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Claim = new ClaimDTO();
            SubmittedMember = new ClaimSubmittedMemberDTO();
            ActualMember = new ClaimActualMemberDTO();
            Pharmacy = new ClaimPharmacyDTO();
            Prescriber = new ClaimPrescriberDTO();
            ProductInformation = new ClaimProductInformationDTO();
            ProductCost = new ClaimProductCostDTO();
            PlanInformation = new ClaimPlanInformationDTO();
            //RejectInformation = new ClaimRejectInformationDTO();
            DiagnosisInformation = new ClaimDiagnosisInformationDTO();
            SubmittedPricing = new ClaimPricingDTO();
            CalculatedPricing = new ClaimPricingDTO();
            IndividualAccumulationInformation = new ClaimAccumulationInformationDTO();
            FamilyAccumulationInformation = new ClaimAccumulationInformationDTO();
            DURPPS = new ClaimDURPPSDTO();
            PaymentInformation = new ClaimPaymentInformationDTO();
            AdjustmentPaymentInformation = new ClaimAdjustmentPaymentInformationDTO();

            Claim.LoadFromDataReaderWithPrefix(reader, "Claim");
            SubmittedMember.LoadFromDataReaderWithPrefix(reader, "SubmittedMember");
            ActualMember.LoadFromDataReaderWithPrefix(reader, "ActualMember");
            Pharmacy.LoadFromDataReaderWithPrefix(reader, "Pharmacy");
            Prescriber.LoadFromDataReaderWithPrefix(reader, "Prescriber");
            ProductInformation.LoadFromDataReaderWithPrefix(reader, "ProductInformation");
            ProductCost.LoadFromDataReaderWithPrefix(reader, "ProductCost");
            PlanInformation.LoadFromDataReaderWithPrefix(reader, "PlanInformation");
            //RejectInformation.LoadFromDataReaderWithPrefix(reader, "RejectInformation");
            DiagnosisInformation.LoadFromDataReaderWithPrefix(reader, "DiagnosisInformation");
            SubmittedPricing.LoadFromDataReaderWithPrefix(reader, "SubmittedPricing");
            CalculatedPricing.LoadFromDataReaderWithPrefix(reader, "CalculatedPricing");
            IndividualAccumulationInformation.LoadFromDataReaderWithPrefix(reader, "IndividualAccumulationInformation");
            FamilyAccumulationInformation.LoadFromDataReaderWithPrefix(reader, "FamilyAccumulationInformation");
            DURPPS.LoadFromDataReaderWithPrefix(reader, "DURPPS");
            PaymentInformation.LoadFromDataReaderWithPrefix(reader, "PaymentInformation");
            AdjustmentPaymentInformation.LoadFromDataReaderWithPrefix(reader, "AdjustmentPaymentInformation");
        }
    }
}
