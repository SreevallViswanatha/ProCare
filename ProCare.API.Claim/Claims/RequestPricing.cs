using System.Collections.Generic;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestPricing
    {
        /// <summary>
        ///     Field Number: 423-DN
        ///     <para />
        ///     Description: Code indicating the method by which Ingredient Cost Submitted was calculated.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed for receiver claim/encounter adjudication
        ///     <para />
        ///     ProCare FieldName: NDCBASISCOST
        /// </summary>
        [ApiMember(Name = "BasisOfCostDetermination", Description = FieldDescriptions.BasisOfCostDetermination, DataType = "string", IsRequired = false)]
        public string BasisOfCostDetermination { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 412-DC
        ///     <para />
        ///     Description: Dispensing fee submitted by pharmacy. Included in Gross Amount Due.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if its value has an effect on the Gross Amount
        ///     Due(43Ø-DU) calculation.
        ///     <para />
        ///     Zero(Ø) is a valid value.
        ///     <para />
        ///     ProCare FieldName: SDISPFEE
        /// </summary>
        [ApiMember(Name = "DispensingFeeSubmitted", Description = FieldDescriptions.DispensingFeeSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? DispensingFeeSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 481-HA
        ///     <para />
        ///     Description: Flat sales tax amount submitted for prescription. Included in Gross Amount Due
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if its value has an effect on the Gross Amount
        ///     Due(43Ø-DU) calculation.
        ///     <para />
        ///     Zero(Ø) is a valid value
        ///     <para />
        ///     ProCare FieldName: STAX
        /// </summary>
        [ApiMember(Name = "FlatSalesTaxAmountSubmitted", Description = FieldDescriptions.FlatSalesTaxAmountSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? FlatSalesTaxAmountSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 430-DU
        ///     <para />
        ///     Description: Total price claimed from all sources.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Required:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required.  See Pricing Formula for fields used in calculation.
        ///     <para />
        ///     ProCare FieldName: STOTPRC
        /// </summary>
        [ApiMember(Name = "GrossAmountDue", Description = FieldDescriptions.GrossAmountDue, DataType = "Decimal", IsRequired = true)]
        public decimal? GrossAmountDue { get; set; }

        /// <summary>
        ///     Field Number: 438-E3
        ///     <para />
        ///     Description: Amount represents the contractually agreed upon incentive fee paid for specific services rendered.
        ///     Included in Gross Amount Due.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if its value has an effect on the Gross Amount
        ///     Due(43Ø-DU) calculation.
        ///     <para />
        ///     Zero(Ø) is a valid value.
        ///     <para />
        ///     ProCare FieldName: SINCENTIV
        /// </summary>
        [ApiMember(Name = "IncentiveAmountSubmitted", Description = FieldDescriptions.IncentiveAmountSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? IncentiveAmountSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 409-D9
        ///     <para />
        ///     Description: Submitted product component cost of the dispensed prescription.
        ///     Included in the Gross Amount Due.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare FieldName: SINGRCOST
        /// </summary>
        [ApiMember(Name = "IngredientCostSubmitted", Description = FieldDescriptions.IngredientCostSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? IngredientCostSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 478-H7
        ///     <para />
        ///     Description: Count of Other Amount Claimed Submitted occurrences.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 3.
        ///     Required if Other Amount Claimed Submitted Qualifier (479-H8) is used.
        ///     <para />
        ///     Max: 3
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "OtherAmountClaimedCount", Description = FieldDescriptions.OtherAmountClaimedCount, DataType = "Int", IsRequired = false)]
        public int OtherAmountClaimedCount => OtherAmountClaimeds.Count;

        public List<OtherAmountClaimed> OtherAmountClaimeds { get; set; }

        /// <summary>
        ///     Field Number: 433-DX
        ///     <para />
        ///     Description: Amount the pharmacy received from the patient for the prescription dispensed.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage,
        ///     pricing, or patient financial responsibility.
        ///     <para />
        ///     Not used in coordination of benefit claim to pass patient
        ///     liability information to a downstream payer.See section
        ///     <para />
        ///     “Standard Conventions”, “Repetition and Multiple
        ///     Occurrences”, Repeating Data Elements”, “Request
        ///     Segments”, “Coordination of Benefits/Other Payments
        ///     Segment”.
        ///     <para />
        ///     ProCare FieldName: SENRAMT
        /// </summary>
        [ApiMember(Name = "PatientPaidAmountSubmitted", Description = FieldDescriptions.PatientPaidAmountSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? PatientPaidAmountSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 482-GE
        ///     <para />
        ///     Description: Percentage sales tax submitted. Included in Gross Amount Due.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if its value has an effect on the Gross Amount
        ///     Due(43Ø-DU) calculation.
        ///     <para />
        ///     Zero(Ø) is a valid value
        ///     <para />
        ///     ProCare FieldName: STAX
        /// </summary>
        [ApiMember(Name = "PercentageSalesTaxAmountSubmitted", Description = FieldDescriptions.PercentageSalesTaxAmountSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? PercentageSalesTaxAmountSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 484-JE
        ///     <para />
        ///     Description: Code indicating the percentage sales tax paid basis.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Percentage Sales Tax Amount Submitted(482-
        ///     GE) and Percentage Sales Tax Rate Submitted(483-HE) are used
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "PercentageSalesTaxBasisSubmitted", Description = FieldDescriptions.PercentageSalesTaxBasisSubmitted, DataType = "string", IsRequired = false)]
        public string PercentageSalesTaxBasisSubmitted { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 483-HE
        ///     <para />
        ///     Description: Percentage sales tax rate used to calculate Percentage Sales Tax Amount Submitted.
        ///     <para />
        ///     Format: s9(3)v9(4)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Percentage Sales Tax Amount Submitted(482-
        ///     GE) and Percentage Sales Tax Basis Submitted(484-JE) are used.
        ///     <para />
        ///     Required if this field could result in different pricing.
        ///     <para />
        ///     Required if needed to calculate Percentage Sales Tax Amount Paid (559-AX).
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "PercentageSalesTaxRateSubmitted", Description = FieldDescriptions.PercentageSalesTaxRateSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? PercentageSalesTaxRateSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 477-BE
        ///     <para />
        ///     Description: Amount submitted by the provider for professional services rendered. Included in Gross Amount Due.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation:
        ///     <para />
        ///     Claim Billing/Encounter: Not used.
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "ProfessionalServiceFeeSubmitted", Description = FieldDescriptions.ProfessionalServiceFeeSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? ProfessionalServiceFeeSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 426-DQ
        ///     <para />
        ///     Description: Amount charged cash customers for the prescription exclusive of sales tax or other amounts claimed.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter: Required if needed per trading partner agreement.
        ///     <para />
        ///     ProCare FieldName: SCHARGE
        /// </summary>
        [ApiMember(Name = "UsualAndCustomaryCharge", Description = FieldDescriptions.UsualAndCustomaryCharge, DataType = "Decimal", IsRequired = false)]
        public decimal? UsualAndCustomaryCharge { get; set; }

    }
    public class OtherAmountClaimed
    {
        /// <summary>
        ///     Field Number: 480-H9
        ///     <para />
        ///     Description: Amount representing the additional incurred costs for a dispensed prescription or service. Included in
        ///     Gross Amount Due.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified - Repeating:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if its value has an effect on the Gross Amount
        ///     Due(43Ø-DU) calculation.
        ///     <para />
        ///     Zero(Ø) is a valid value.
        ///     <para />
        ///     ProCare FieldName: OTHRCLAIMD ??
        /// </summary>
        [ApiMember(Name = "OtherAmountClaimedSubmitted", Description = FieldDescriptions.OtherAmountClaimedSubmitted, DataType = "Decimal", IsRequired = false)]
        public decimal? OtherAmountClaimedSubmitted { get; set; }

        /// <summary>
        ///     Field Number: 479-H8
        ///     <para />
        ///     Description: Code identifying the additional incurred cost claimed in Other Amount Claimed Submitted.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified - Repeating:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Other Amount Claimed Submitted(48Ø-H9) is used
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "OtherAmountClaimedSubmittedQualifier", Description = FieldDescriptions.OtherAmountClaimedSubmittedQualifier, DataType = "string", IsRequired = false)]
        public string OtherAmountClaimedSubmittedQualifier { get; set; } = string.Empty;
    }
}
