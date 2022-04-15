using ServiceStack;
using System;
using System.Collections.Generic;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestCoordinationOfBenefits
    {

        /// <summary>
        ///     Field Number: 392-MU
        ///     <para />
        ///     Description: Count of ‘Benefit Stage Amount’ (394-MW) occurrences.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 4.
        ///     <para />
        ///     Required if Benefit Stage Amount(394-MW) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "BenefitStageCount", Description = FieldDescriptions.BenefitStageCount, DataType = "Int", IsRequired = false)]
        public int BenefitStageCount => BenefitStages.Count;

        public List<BenefitStage> BenefitStages { get; set; }

        /// <summary>
        ///     Field Number: 341-HB
        ///     <para />
        ///     Description: Count of the Other Payer Amount Paid occurrences.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Billing:
        ///     Maximum count of 9.
        ///     <para />
        ///     Required if Other Payer Amount Paid Qualifier(342-HC) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerAmountPaidCount", Description = FieldDescriptions.OtherPayerAmountPaidCount, DataType = "Int", IsRequired = false)]
        public int OtherPayerAmountPaidCount => OtherPayerAmounts.Count;

        public List<OtherPayerAmountsPaid> OtherPayerAmounts { get; set; }

        /// <summary>
        ///     Field Number: 353-NR
        ///     <para />
        ///     Description: The patient’s cost share from a previous payer.
        ///     <para />
        ///     Format: s9(8)v99 - ??
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 25.
        ///     <para />
        ///     Required if Other Payer-Patient Responsibility Amount Qualifier(351-NP) is used.
        ///     <para />
        ///     Note the occurrences are dependent upon the number of component parts returned from a previous payer.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerPatientAmountCount", Description = FieldDescriptions.OtherPayerPatientAmountCount, DataType = "Int", IsRequired = false)]
        public int OtherPayerPatientAmountCount => OtherPayerPatientResponsibilities.Count;

        public List<OtherPayerPatientResponsibility> OtherPayerPatientResponsibilities { get; set; }

        /// <summary>
        ///     Field Number: 472-6E
        ///     <para />
        ///     Description: The error encountered by the previous Other Payer.
        ///     <para />
        ///     Format: X(03)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required when the other payer has denied the payment for the billing, designated with Other Coverage Code(3Ø8-C8)
        ///     = 3 (Other Coverage Billed – claim not covered).
        ///     <para />
        ///     Note: This field must only contain the NCPDP Reject Code (511-FB) values.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerRejectCodes", Description = FieldDescriptions.OtherPayerRejectCodes, DataType = "List<String>", IsRequired = false)]
        public List<string> OtherPayerRejectCodes { get; set; }

        /// <summary>
        ///     Field Number: 471-5E
        ///     <para />
        ///     Description: Count of the Other Payer Reject Code occurrences.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 5.
        ///     <para />
        ///     Required if Other Payer Reject Code(472-6E) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerRejectCount", Description = FieldDescriptions.OtherPayerRejectCount, DataType = "Int", IsRequired = false)]
        public int OtherPayerRejectCount => OtherPayerRejectCodes.Count;

        public List<OtherPayer> OtherPayers { get; set; }

        /// <summary>
        ///     Field Number: 337-4C
        ///     <para />
        ///     Description: Count of other payment occurrences.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     Maximum count of 9.
        /// </summary>
        [ApiMember(Name = "OtherPaymentsCount", Description = FieldDescriptions.OtherPaymentsCount, DataType = "Int", IsRequired = false)]
        public int OtherPaymentsCount => OtherPayers.Count;

    }
    public class OtherPayer
    {
        /// <summary>
        ///     Field Number: 993-A7
        ///     <para />
        ///     Description: Number assigned by the processor to identify an adjudicated claim when supplied in payer-to-payer
        ///     coordination of benefits only.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required when used for payer-to-payer coordination of benefits to track the claim without regard to the “Service
        ///     Provider ID, Prescription Number, & Date of Service”.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "InternalControlNumber", Description = FieldDescriptions.InternalControlNumber, DataType = "String", IsRequired = false)]
        public string InternalControlNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 338-5C
        ///     <para />
        ///     Description: Code identifying the type of Other Payer ID.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory: repeating
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerCoverageType", Description = FieldDescriptions.OtherPayerCoverageType, DataType = "String", IsRequired = true)]
        public string OtherPayerCoverageType { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 443-E8
        ///     <para />
        ///     Description: Payment or denial date of the claim submitted to the other payer.
        ///     <para />
        ///     Format: 9(08)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if identification of the Other Payer Date is necessary for service billing adjudication.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerDate", Description = FieldDescriptions.OtherPayerDate, DataType = "String", Format = "Date", IsRequired = false)]
        public DateTime? OtherPayerDate { get; set; }

        /// <summary>
        ///     Field Number: 340-7C
        ///     <para />
        ///     Description: ID assigned to the payer.
        ///     <para />
        ///     Format: X(10)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if identification of the Other Payer is necessary for service billing adjudication.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerID", Description = FieldDescriptions.OtherPayerId, DataType = "String", IsRequired = false)]
        public string OtherPayerID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 339-6C
        ///     <para />
        ///     Description: Code qualifying the Other Payer ID.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if Other Payer ID(34Ø-7C) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerIDQualifier", Description = FieldDescriptions.OtherPayerIdQualifier, DataType = "String", IsRequired = false)]
        public string OtherPayerIDQualifier { get; set; } = string.Empty;
    }

    public class OtherPayerAmountsPaid
    {
        /// <summary>
        ///     Field Number: 431-DV
        ///     <para />
        ///     Description: Amount of any payment known by the pharmacy from other sources (including coupons).
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if other payer has approved payment for some/all of the billing.
        ///     <para />
        ///     Zero(Ø) is a valid value.
        ///     <para />
        ///     Not used for patient financial responsibility only billing.
        ///     <para />
        ///     Not used for non-governmental agency programs if Other Payer-Patient Responsibility Amount(352-NQ) is submitted.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerAmountPaid", Description = FieldDescriptions.OtherPayerAmountPaid, DataType = "Decimal", IsRequired = false)]
        public decimal? OtherPayerAmountPaid { get; set; }

        /// <summary>
        ///     Field Number: 342-HC
        ///     <para />
        ///     Description: Code qualifying the Other Payer Amount Paid.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if Other Payer Amount Paid(431-DV) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerAmountPaidQualifier", Description = FieldDescriptions.OtherPayerAmountPaidQualifier, DataType = "String", IsRequired = false)]
        public string OtherPayerAmountPaidQualifier { get; set; } = string.Empty;
    }

    public class OtherPayerPatientResponsibility
    {
        /// <summary>
        ///     Field Number: 352-NQ
        ///     <para />
        ///     Description: The patient’s cost share from a previous payer
        ///     <para />
        ///     Format: s9(8)v99
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if necessary for patient financial responsibility only billing.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     Not used for non-governmental agency programs if Other Payer Amount Paid(431-DV) is submitted.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerPatientAmount", Description = FieldDescriptions.OtherPayerPatientAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? OtherPayerPatientAmount { get; set; }

        /// <summary>
        ///     Field Number: 351-NP
        ///     <para />
        ///     Description: Code qualifying the “Other Payer-Patient Responsibility Amount (352-NQ)”.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Required if Other Payer-Patient Responsibility Amount(352-NQ) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "OtherPayerPatientAmountQualifier", Description = FieldDescriptions.OtherPayerPatientAmountQualifier, DataType = "String", IsRequired = false)]
        public string OtherPayerPatientAmountQualifier { get; set; } = string.Empty;
    }

    public class BenefitStage
    {
        /// <summary>
        ///     Field Number: 394-MW
        ///     <para />
        ///     Description: The amount of claim allocated to the Medicare stage identified by the ‘Benefit Stage Qualifier’ (393-MV).
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Not Used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "Amount", Description = FieldDescriptions.Amount, DataType = "Decimal", IsRequired = false)]
        public decimal? Amount { get; set; }

        /// <summary>
        ///     Field Number: 393-MV
        ///     <para />
        ///     Description: Code qualifying the ’Benefit Stage Amount’ (394-MW).
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Service Billing:
        ///     Not Used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "Qualifier", Description = FieldDescriptions.Qualifier, DataType = "String", IsRequired = false)]
        public string Qualifier { get; set; } = string.Empty;
    }
}
