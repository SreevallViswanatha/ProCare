using System;
using System.Collections.Generic;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class ResponsePricing
    {
        /// <summary>
        /// </summary>
        public ResponsePricing()
        {
            OtherAmounts = new List<OtherAmountPaid>();
            BenefitStages = new List<BenefitStagePricing>();
        }

        /// <summary>
        ///     Field Number: 512-FC
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service): Provided for informational purposes only.
        /// </summary>
        [ApiMember(Name = "AccumulatedDeductibleAmount", Description = FieldDescriptions.AccumulatedDeductibleAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? AccumulatedDeductibleAmount { get; set; }

        /// <summary>
        ///     Field Number: 517-FH
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if Patient Pay Amount(5Ø5-F5) includes deductible.
        /// </summary>
        [ApiMember(Name = "AmountAppliedToPeriodicDeductible", Description = FieldDescriptions.AmountAppliedToPeriodicDeductible, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountAppliedToPeriodicDeductible { get; set; }

        /// <summary>
        ///     Field Number: 136-UN
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim):
        ///     Required if Patient Pay Amount(5Ø5-F5) includes an amount that is attributable to a patient’s selection of a
        ///     Brand non-preferred formulary product.
        ///     <para />
        ///     Service: Not used.
        /// </summary>
        [ApiMember(Name = "AmountAttributedToBrandNonPreferredFormularySelection", Description = FieldDescriptions.AmountAttributedToBrandNonPreferredFormularySelection, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountAttributedToBrandNonPreferredFormularySelection { get; set; }

        /// <summary>
        ///     Field Number: 137-UP
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required when the patient’s financial responsibility is due to the coverage gap.
        /// </summary>
        [ApiMember(Name = "AmountAttributedToCoverageGap", Description = FieldDescriptions.AmountAttributedToCoverageGap, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountAttributedToCoverageGap { get; set; }

        /// <summary>
        ///     Field Number: 135-UM
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim):
        ///     Required if Patient Pay Amount(5Ø5-F5) includes an amount that is attributable to a patient’s selection of a nonpreferred formulary product
        /// </summary>
        [ApiMember(Name = "AmountAttributedToNonPreferredFormularySelection", Description = FieldDescriptions.AmountAttributedToNonPreferredFormularySelection, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountAttributedToNonPreferredFormularySelection { get; set; }

        /// <summary>
        ///     Field Number: 571-NZ
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Inquiry (Claim):
        ///     Required if the customer is responsible for 1ØØ% of the prescription payment and when the provider net sale is less
        ///     than the amount the customer is expected to pay.
        ///     <para />
        ///     Service:
        ///     Required if the customer is responsible for 1ØØ% of the service payment and when the provider net sale is less
        ///     than the amount the customer is expected to pay.
        /// </summary>
        [ApiMember(Name = "AmountAttributedToProcessorFee", Description = FieldDescriptions.AmountAttributedToProcessorFee, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountAttributedToProcessorFee { get; set; }

        /// <summary>
        ///     Field Number: 134-UK
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Inquiry (Claim):
        ///     Required if Patient Pay Amount(5Ø5-F5) includes an amount that is attributable to a patient’s selection of a Brand drug.
        /// </summary>
        [ApiMember(Name = "AmountAttributedToProductSelectionBrandDrug", Description = FieldDescriptions.AmountAttributedToProductSelectionBrandDrug, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountAttributedToProductSelectionBrandDrug { get; set; }

        /// <summary>
        ///     Field Number: 133-UJ
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Inquiry (Claim/Service):
        ///     Required if Patient Pay Amount(5Ø5-F5) includes an amount that is attributable to a cost share differential due to
        ///     the selection of one pharmacy over another.
        /// </summary>
        [ApiMember(Name = "AmountAttributedToProviderNetworkSelection", Description = FieldDescriptions.AmountAttributedToProviderNetworkSelection, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountAttributedToProviderNetworkSelection { get; set; }

        /// <summary>
        ///     Field Number: 523-FN
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Patient Pay Amount(5Ø5-F5) includes sales tax that is the financial responsibility of the member but is not
        ///     also included in any of the other fields that add up to Patient Pay Amount
        /// </summary>
        [ApiMember(Name = "AmountAttributedToSalesTax", Description = FieldDescriptions.AmountAttributedToSalesTax, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountAttributedToSalesTax { get; set; }

        /// <summary>
        ///     Field Number: 520-FK
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Patient Pay Amount(5Ø5-F5) includes amount exceeding periodic benefit maximum.
        /// </summary>
        [ApiMember(Name = "AmountExceedingPeriodicBenefitMaximum", Description = FieldDescriptions.AmountExceedingPeriodicBenefitMaximum, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountExceedingPeriodicBenefitMaximum { get; set; }

        /// <summary>
        ///     Field Number: 573-4V
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Dispensing Status(343-HD) on submission is “P” (Partial Fill) or “C” (Completion of Partial Fill).
        /// </summary>
        [ApiMember(Name = "BasisOfCalculationCoInsurance", Description = FieldDescriptions.BasisOfCalculationCoInsurance, DataType = "string", IsRequired = false)]
        public string BasisOfCalculationCoInsurance { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 347-HJ
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Dispensing Status(343-HD) on submission is “P” (Partial Fill) or “C” (Completion of Partial Fill).
        /// </summary>
        [ApiMember(Name = "BasisOfCalculationCopay", Description = FieldDescriptions.BasisOfCalculationCopay, DataType = "string", IsRequired = false)]
        public string BasisOfCalculationCopay { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 346-HH
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Dispensing Status(343-HD) on submission is “P” (Partial Fill) or “C” (Completion of Partial Fill).
        /// </summary>
        [ApiMember(Name = "BasisOfCalculationDispensingFee", Description = FieldDescriptions.BasisOfCalculationDispensingFee, DataType = "string", IsRequired = false)]
        public string BasisOfCalculationDispensingFee { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 348-HK
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Dispensing Status(343-HD) on submission is “P” (Partial Fill) or “C” (Completion of Partial Fill) and Flat 
        ///     Sales Tax Amount Paid(558-AW) is greater than zero(Ø).
        /// </summary>
        [ApiMember(Name = "BasisOfCalculationFlatSalesTax", Description = FieldDescriptions.BasisOfCalculationFlatSalesTax, DataType = "string", IsRequired = false)]
        public string BasisOfCalculationFlatSalesTax { get; set; } = string.Empty;


        /// <summary>
        ///     Field Number: 349-HM
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Dispensing Status(343-HD) on submission is “P” (Partial Fill) or “C” (Completion of Partial Fill) and 
        ///     Percentage Sales Tax Amount Paid(559-AX) is greater than zero(Ø).
        /// </summary>
        [ApiMember(Name = "BasisOfCalculationPercentageSalesTax", Description = FieldDescriptions.BasisOfCalculationPercentageSalesTax, DataType = "string", IsRequired = false)]
        public string BasisOfCalculationPercentageSalesTax { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 522-FM
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Ingredient Cost Paid(5Ø6-F6) is greater than zero(Ø).
        ///     <para />
        ///     Required if Basis of Cost Determination(432-DN) is submitted on billing.
        /// </summary>
        [ApiMember(Name = "BasisOfReimbursementDetermination", Description = FieldDescriptions.BasisOfReimbursementDetermination, DataType = "string", IsRequired = false)]
        public string BasisOfReimbursementDetermination { get; set; } = string.Empty;

        public int BenefitStageCount => BenefitStages.Count;
        public List<BenefitStagePricing> BenefitStages { get; set; }

        /// <summary>
        ///     Field Number: 572-4U
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Patient Pay Amount(5Ø5-F5) includes coinsurance as patient financial responsibility.
        /// </summary>
        [ApiMember(Name = "CoInsuranceAmount", Description = FieldDescriptions.CoInsuranceAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? CoInsuranceAmount { get; set; }

        /// <summary>
        ///     Field Number: 518-FI
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Patient Pay Amount(5Ø5-F5) includes copay as patient financial responsibility.
        /// </summary>
        [ApiMember(Name = "CopayAmount", Description = FieldDescriptions.CopayAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? CopayAmount { get; set; }

        /// <summary>
        ///     Field Number: 149-U9
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Claim Rebill:
        ///     Required when Basis of Reimbursement Determination (522-FM) is “14” (Patient Responsibility Amount) or “15”
        ///     (Patient Pay Amount) unless prohibited by state/federal/regulatory agency.
        ///     <para />
        ///     This field is informational only.
       /// </summary>
        [ApiMember(Name = "DispensingFeeContractedAmount", Description = FieldDescriptions.DispensingFeeContractedAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? DispensingFeeContractedAmount { get; set; }

        /// <summary>
        ///     Field Number: 507-F7
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Rebill:
        ///     Required if this value is used to arrive at the estimated reimbursement.If reimbursement is not estimated, this field
        ///     contains the submitted value.
        /// </summary>
        [ApiMember(Name = "DispensingFeePaid", Description = FieldDescriptions.DispensingFeePaid, DataType = "Decimal", IsRequired = false)]
        public decimal? DispensingFeePaid { get; set; }

        /// <summary>
        ///     Field Number: 577-G3
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Claim Rebill:
        ///     This information should be provided when a patient selected the brand drug and a generic form of the drug was
        ///     available.It will contain an estimate of the difference between the cost of the brand drug and the generic drug,
        ///     when the brand drug is more expensive than the generic.It is information that the provider should provide to the patient.
        /// </summary>
        [ApiMember(Name = "EstimatedGenericSavings", Description = FieldDescriptions.EstimatedGenericSavings, DataType = "Decimal", IsRequired = false)]
        public decimal? EstimatedGenericSavings { get; set; }

        /// <summary>
        ///     Field Number: 558-AW
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if Flat Sales Tax Amount Submitted(481-HA) is greater than zero(Ø) or if Flat Sales Tax Amount Paid 
        ///     (558-AW) is used to arrive at the final reimbursement.Zero (Ø) value is valid.
        /// </summary>
        [ApiMember(Name = "FlatSalesTaxAmountPaid", Description = FieldDescriptions.FlatSalesTaxAmountPaid, DataType = "Decimal", IsRequired = false)]
        public decimal? FlatSalesTaxAmountPaid { get; set; }

        /// <summary>
        ///     Field Number: 129-UD
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required when the patient meets the plan-funded assistance criteria, to reduce Patient Pay Amount(5Ø5-F5). 
        ///     The resulting Patient Pay Amount(5Ø5-F5) must be greater than or equal to zero.
        ///     <para />
        ///     This field is always a negative amount or zero.
        /// </summary>
        [ApiMember(Name = "HealthPlanFundedAssistanceAmount", Description = FieldDescriptions.HealthPlanFundedAssistanceAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? HealthPlanFundedAssistanceAmount { get; set; }

        /// <summary>
        ///     Field Number: 521-FL
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim):
        ///     Required if this value is used to arrive at the final reimbursement.
        ///     <para />
        ///     Required if Incentive Amount Submitted(438-E3) is greater than zero(Ø). Zero(Ø) value is valid.
        ///     <para />
        ///     Service:
        ///     Not used. Not supported in Service Billing formula.
        /// </summary>
        [ApiMember(Name = "IncentiveAmountPaid", Description = FieldDescriptions.IncentiveAmountPaid, DataType = "Decimal", IsRequired = false)]
        public decimal? IncentiveAmountPaid { get; set; }

        /// <summary>
        ///     Field Number: 148-U8
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim):
        ///     Required when Basis of Reimbursement Determination (522-FM) is “14” (Patient Responsibility Amount) or “15”
        ///     (Patient Pay Amount) unless prohibited by state/federal/regulatory agency.
        ///     <para />
        ///     This field is informational only.
        ///     Service: Not used.
        /// </summary>
        [ApiMember(Name = "IngredientCostContractedAmount", Description = FieldDescriptions.IngredientCostContractedAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? IngredientCostContractedAmount { get; set; }

        /// <summary>
        ///     Field Number: 506-F6
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Inquiry (Claim):
        ///     Required if this value is used to arrive at the final reimbursement.
        ///     <para />
        ///     Service: Not used.
        /// </summary>
        [ApiMember(Name = "IngredientCostPaid", Description = FieldDescriptions.IngredientCostPaid, DataType = "Decimal", IsRequired = false)]
        public decimal? IngredientCostPaid { get; set; }

        public int OtherAmountPaidCount => OtherAmounts.Count;
        public List<OtherAmountPaid> OtherAmounts { get; set; }

        /// <summary>
        ///     Field Number: 505-F5
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     Prior Authorization Inquiry (Claim/Service): Required.
        /// </summary>
        [ApiMember(Name = "PatientPayAmount", Description = FieldDescriptions.PatientPayAmount, DataType = "Decimal", IsRequired = true)]
        public decimal? PatientPayAmount { get; set; }

        /// <summary>
        ///     Field Number: 575-EQ
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Prior Authorization Inquiry (Claim/Service):
        ///     Used when necessary to identify the Patient’s portion of the Sales Tax.
        ///     <para />
        ///     Provided for informational purposes only
        /// </summary>
        [ApiMember(Name = "PatientSalesTaxAmount", Description = FieldDescriptions.PatientSalesTaxAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? PatientSalesTaxAmount { get; set; }

        /// <summary>
        ///     Field Number: 559-AX
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this value is used to arrive at the final reimbursement.
        ///     <para />
        ///     Required if Percentage Sales Tax Amount Submitted(482-GE) is greater than zero(Ø).
        ///     <para />
        ///     Zero(Ø) is a valid value.
        ///     <para />
        ///     Required if Percentage Sales Tax Rate Paid(56Ø-AY) and Percentage Sales Tax Basis Paid(561-AZ) are used.
        /// </summary>
        [ApiMember(Name = "PercentageSalesTaxAmountPaid", Description = FieldDescriptions.PercentageSalesTaxAmountPaid, DataType = "Decimal", IsRequired = false)]
        public decimal? PercentageSalesTaxAmountPaid { get; set; }

        /// <summary>
        ///     Field Number: 561-AZ
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Percentage Sales Tax Amount Paid (559-AX) is greater than zero(Ø).
        /// </summary>
        [ApiMember(Name = "PercentageSalesTaxBasisPaid", Description = FieldDescriptions.PercentageSalesTaxBasisPaid, DataType = "string", IsRequired = false)]
        public string PercentageSalesTaxBasisPaid { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 560-AY
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Percentage Sales Tax Amount Paid(559-AX) is greater than zero(Ø).
        /// </summary>
        [ApiMember(Name = "PercentageSalesTaxRatePaid", Description = FieldDescriptions.PercentageSalesTaxRatePaid, DataType = "Decimal", IsRequired = false)]
        public decimal? PercentageSalesTaxRatePaid { get; set; }

        /// <summary>
        ///     Field Number: 574-2Y
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Used when necessary to identify the Plan’s portion of the Sales Tax.
        ///     <para />
        ///     Provided for informational purposes only
        /// </summary>
        [ApiMember(Name = "PlanSalesTaxAmount", Description = FieldDescriptions.PlanSalesTaxAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? PlanSalesTaxAmount { get; set; }

        /// <summary>
        ///     Field Number: 562-J1
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Required
        /// </summary>
        [ApiMember(Name = "ProfessionalServiceFeePaid", Description = FieldDescriptions.ProfessionalServiceFeePaid, DataType = "Decimal", IsRequired = true)]
        public decimal? ProfessionalServiceFeePaid { get; set; }

        /// <summary>
        ///     Field Number: 514-FE
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Service Billing:
        ///     Provided for informational purposes only.
        ///     <para />
        ///     The Remaining Benefit Amount must not be returned with zeroes unless there are no benefit dollars remaining.
        ///     The default value of 999999999 from previous versions must not be used as a default in this field.
        /// </summary>
        [ApiMember(Name = "RemainingBenefitAmount", Description = FieldDescriptions.RemainingBenefitAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? RemainingBenefitAmount { get; set; }

        /// <summary>
        ///     Field Number: 513-FD
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Service Billing:
        ///     Provided for informational purposes only.
        /// </summary>
        [ApiMember(Name = "RemainingDeductibleAmount", Description = FieldDescriptions.RemainingDeductibleAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? RemainingDeductibleAmount { get; set; }

        /// <summary>
        ///     Field Number: 128-UC
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Service Billing:
        ///     This dollar amount will be provided, if known, to the receiver when the transaction had spending account dollars
        ///     reported as part of the patient pay amount.
        ///     <para />
        ///     This field is informational only. It is reported back to the provider and the patient the amount remaining on the spending 
        ///     account after the current claim updated the spending account.
        /// </summary>
        [ApiMember(Name = "SpendingAccountAmountRemaining", Description = FieldDescriptions.SpendingAccountAmountRemaining, DataType = "Decimal", IsRequired = false)]
        public decimal? SpendingAccountAmountRemaining { get; set; }

        /// <summary>
        ///     Field Number: 557-AV
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Billing:
        ///     Required if the sender(health plan) and/or patient is tax exempt and exemption applies to this billing.
        /// </summary>
        [ApiMember(Name = "TaxExemptIndicator", Description = FieldDescriptions.TaxExemptIndicator, DataType = "string", IsRequired = false)]
        public string TaxExemptIndicator { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 509-F9
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Required
        /// </summary>
        [ApiMember(Name = "TotalAmountPaid", Description = FieldDescriptions.TotalAmountPaid, DataType = "Decimal", IsRequired = true)]
        public decimal? TotalAmountPaid { get; set; }
        
        private void AddOtherAmountPaidIfNeeded(int index)
        {
            while (OtherAmounts.Count < index)
            {
                OtherAmounts.Add(new OtherAmountPaid());
            }
        }

        private void AddBenefitStageIfNeeded(int index)
        {
            while (BenefitStages.Count < index)
            {
                BenefitStages.Add(new BenefitStagePricing());
            }
        }
 
    }

    public class OtherAmountPaid
    {

        /// <summary>
        ///     Field Number: 565-J4
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if this value is used to arrive at the estimated reimbursement.If reimbursement is not estimated, this field
        ///     contains the submitted value.
        ///     <para />
        ///     Required if Other Amount Claimed Submitted(48Ø-H9) is greater than zero(Ø).
        ///     <para />
        ///     Zero(Ø) is a valid value.
        ///     <para />   
        ///     Must respond to each occurrence submitted.
        /// </summary>
        [ApiMember(Name = "AmountPaid", Description = FieldDescriptions.AmountPaid, DataType = "Decimal", IsRequired = false)]
        public decimal? AmountPaid { get; set; }

        /// <summary>
        ///     Field Number: 564-J3
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if Other Amount Paid(565-J4) is used.
        /// </summary>
        [ApiMember(Name = "AmountPaidQualifier", Description = FieldDescriptions.AmountPaidQualifier, DataType = "string", IsRequired = false)]
        public string AmountPaidQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 566-J5
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Billing:
        ///     Required if Other Payer Amount Paid(431-DV) is greater than zero(Ø) or if this field is used to arrive at the estimated reimbursement.
        ///     <para />
        ///     Zero(Ø) value is valid.
        ///     <para />
        ///     If reimbursement is not estimated, this field contains the submitted value.
        /// </summary>
        [ApiMember(Name = "OtherPayerAmountRecognized", Description = FieldDescriptions.OtherPayerAmountRecognized, DataType = "Decimal", IsRequired = false)]
        public decimal? OtherPayerAmountRecognized { get; set; }
    }

    public class BenefitStagePricing
    {

        /// <summary>
        ///     Field Number: 394-MW
        ///     <para />
        ///     Description: The amount of claim allocated to the Medicare stage identified by the ‘Benefit Stage Qualifier’ (393-MV).
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified Repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if the previous payer has financial amounts that apply to Medicare Part D beneficiary benefit stages.
        ///     This field is required when the plan is a participant in a Medicare Part D program that requires reporting of benefit stage
        ///     specific financial amounts.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        /// </summary>
        [ApiMember(Name = "Amount", Description = FieldDescriptions.BenefitStageAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? Amount { get; set; }

        /// <summary>
        ///     Field Number: 393-MV
        ///     <para />
        ///     Description: Code qualifying the ’Benefit Stage Amount’ (394-MW).
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified Repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Benefit Stage Amount(394-MW) is used.
        ///     <para />
        ///     Must only have one value per iteration - value must not be repeated.
        /// </summary>
        [ApiMember(Name = "BenefitStageQualifier", Description = FieldDescriptions.BenefitStageQualifier, DataType = "string", IsRequired = false)]
        public string BenefitStageQualifier { get; set; } = string.Empty;
    }

}