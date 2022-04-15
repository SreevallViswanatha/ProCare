using ServiceStack;
using System;
using System.Collections.Generic;
using ProCare.NCPDP.Telecom;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestClaim
    {
        /// <summary>
        ///     Field Number: 330-CW
        ///     <para />
        ///     Description:  Person identifier to be used for controlled product reporting. ID may be that of person picking up the prescription.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Not Used in Billing Trans.
        ///     <para />
        ///     ProCare Field:
        /// </summary>
        [ApiMember(Name = "AlternateID", Description = FieldDescriptions.AlternateId, DataType = "string", IsRequired = false)]
        public string AlternateID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 457-EP
        ///     <para />
        ///     Description: Date of the Associated Prescription/Service Reference Number.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     <para />
        ///     Required if the “completion” transaction in a partial fill
        ///     (Dispensing Status (343-HD) = “C” (Completed)).
        ///     <para />
        ///     Required if Associated Prescription/Service Reference
        ///     Number(456-EN) is used.
        ///     <para />
        ///     See section “Specific Segment Discussion”, “Request
        ///     Segments”, Claim Segment” for more information.
        ///     <para />
        ///     Required if the Dispensing Status (343-HD) = “P” (Partial
        ///     Fill) and there are multiple occurrences of partial fills for this
        ///     prescription.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "AssociatedPrescriptionDate", Description = FieldDescriptions.AssociatedPrescriptionDate, DataType = "String", Format = "Date", IsRequired = false)]
        public DateTime? AssociatedPrescriptionDate { get; set; }

        /// <summary>
        ///     Field Number: 456-EN
        ///     <para />
        ///     Description: Related Prescription/Service Reference Number to which the service is associated.
        ///     <para />
        ///     Format: 9(12)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     <para />
        ///     Required if the “completion” transaction in a partial fill
        ///     (Dispensing Status (343-HD) = “C” (Completed)).
        ///     <para />
        ///     Required if Associated Prescription/Service Reference
        ///     Number(456-EN) is used.
        ///     <para />
        ///     See section “Specific Segment Discussion”, “Request
        ///     Segments”, Claim Segment” for more information.
        ///     <para />
        ///     Required if the Dispensing Status (343-HD) = “P” (Partial
        ///     Fill) and there are multiple occurrences of partial fills for this
        ///     prescription.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "AssociatedPrescriptionNumber", Description = FieldDescriptions.AssociatedPrescriptionNumber, DataType = "9(12)", IsRequired = false)]
        public string AssociatedPrescriptionNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 406-D6
        ///     <para />
        ///     Description: Code indicating whether or not the prescription is a compound.
        ///     <para />
        ///     Format: 9(01)
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare Field: COMPCODE
        /// </summary>
        [ApiMember(Name = "CompoundCode", Description = FieldDescriptions.CompoundCode, DataType = "9(01)", IsRequired = true)]
        public CompoundCode CompoundCode { get; set; } = CompoundCode.NotSpecified;

        /// <summary>
        ///     Field Number: 996-G1
        ///     <para />
        ///     Description: Clarifies the type of compound
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: Required if specified in trading partner agreement
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "CompoundType", Description = FieldDescriptions.CompoundType, DataType = "String", IsRequired = false)]
        public string CompoundType { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 414-DE
        ///     <para />
        ///     Description: Date prescription was written.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare Field: RXDATE
        /// </summary>
        [ApiMember(Name = "DatePrescriptionWritten", Description = FieldDescriptions.DatePrescriptionWritten, DataType = "String", Format = "Date", IsRequired = true)]
        public DateTime? DatePrescriptionWritten { get; set; }

        /// <summary>
        ///     Field Number: 405-D5
        ///     <para />
        ///     Description: Estimated number of days that the prescription will last.
        ///     <para />
        ///     Format: 9(03)
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare Field: DAYSUP
        /// </summary>
        [ApiMember(Name = "DaysSupply", Description = FieldDescriptions.DaysSupply, DataType = "9(03)", IsRequired = true)]
        public int? DaysSupply { get; set; }

        /// <summary>
        ///     Field Number: 345-HG
        ///     <para />
        ///     Description: Days supply for metric decimal quantity that would be dispensed on original fill if inventory were
        ///     available.
        ///     <para />
        ///     Format:9(03)
        ///     <para />
        ///     Designation: Qualified:
        ///     Claim Billing/Encounter:
        ///     <para />
        ///     Required for the partial fill or the completion fill of a prescription.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "DaysSupplyIntendedToBeDispensed", Description = FieldDescriptions.DaysSupplyIntendedToBeDispensed, DataType = "9(03)", IsRequired = false)]
        public int? DaysSupplyIntendedToBeDispensed { get; set; }

        /// <summary>
        ///     Field Number: 357-NV
        ///     <para />
        ///     Description: Code to specify the reason that submission of the transactions has been delayed.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter: Required when needed to specify the reason that
        ///     submission of the transaction has been delayed.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "DelayReasonCode", Description = FieldDescriptions.DelayReasonCode, DataType = "9(02)", IsRequired = false)]
        public string DelayReasonCode { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 408-D8
        ///     <para />
        ///     Description: Code indicating whether or not the prescriber's instructions regarding generic substitution were
        ///     followed
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare Field: DAW
        /// </summary>
        [ApiMember(Name = "DispenseAsWritten", Description = FieldDescriptions.DispenseAsWritten, DataType = "String", IsRequired = true)]
        public string DispenseAsWritten { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 343-HD
        ///     <para />
        ///     Description: Code indicating the quantity is a partial fill or the completion of a partial fill.
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required for the partial fill or the completion fill of a prescription.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "DispensingStatus", Description = FieldDescriptions.DispensingStatus, DataType = "String", IsRequired = false)]
        public string DispensingStatus { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 403-D3
        ///     <para />
        ///     Description: The code indicating whether the prescription is an original or a refill.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare Field: NWREF
        /// </summary>
        [ApiMember(Name = "FillNumber", Description = FieldDescriptions.FillNumber, DataType = "9(02)", IsRequired = true)]
        public string FillNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 464-EX
        ///     <para />
        ///     Description: Value indicating intermediary authorization occurred.
        ///     <para />
        ///     Format: X(11)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required for overriding an authorized intermediary system
        ///     edit when the pharmacy participates with an intermediary.
        ///     <para />
        ///     Not used for payer-to-payer transactions
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "IntermediaryAuthoriationID", Description = FieldDescriptions.IntermediaryAuthoriationId, DataType = "String", IsRequired = false)]
        public string IntermediaryAuthoriationID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 463-EW
        ///     <para />
        ///     Description: Value indicating that authorization occurred for intermediary processing
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required for overriding an authorized intermediary system
        ///     edit when the pharmacy participates with an intermediary.
        ///     Required if Intermediary Authorization ID(464-EX) is used.
        ///     <para />
        ///     Not used for payer-to-payer transactions.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "IntermediaryAuthorizationTypeID", Description = FieldDescriptions.IntermediaryAuthorizationTypeId, DataType = "9(02)", IsRequired = false)]
        public string IntermediaryAuthorizationTypeID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 418-DI
        ///     <para />
        ///     Description: Coding indicating the type of service the provider rendered.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage,
        ///     pricing, or patient financial responsibility.
        ///     <para />
        ///     ProCare Field: SLEVEL
        /// </summary>
        [ApiMember(Name = "LevelOfService", Description = FieldDescriptions.LevelOfService, DataType = "9(02)", IsRequired = false)]
        public string LevelOfService { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number:114-N4
        ///     <para />
        ///     Description: Claim number assigned by the Medicaid Agency.
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        ///     Claim/Billing Encounter: Not used.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "MedicaidICN", Description = FieldDescriptions.MedicaidIcn, DataType = "String", IsRequired = false)]
        public string MedicaidICN { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 445-EA
        ///     <para />
        ///     Description: Code of the initially prescribed product or service.
        ///     <para />
        ///     Format: X(19)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     <para />
        ///     Required if the receiver requests association to a
        ///     therapeutic, or a preferred product substitution, or when a
        ///     <para />
        ///     DUR alert has been resolved by changing medications, or
        ///     an alternative service than what was originally prescribed.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "OriginallyPrescribedProductID", Description = FieldDescriptions.OriginallyPrescribedProductId, DataType = "String", IsRequired = false)]
        public string OriginallyPrescribedProductID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 435-EJ
        ///     <para />
        ///     Description: Code qualifying the value in Originally Prescribed Product/Service Code.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Originally Prescribed Product/Service Code (455-EA) is used.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "OriginallyPrescribedProductIDQualifier", Description = FieldDescriptions.OriginallyPrescribedProductIdQualifier, DataType = "String", IsRequired = false)]
        public string OriginallyPrescribedProductIDQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 446-EB
        ///     <para />
        ///     Description: Product initially prescribed amount expressed in metric decimal units
        ///     <para />
        ///     Format: 9(7)v999
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if the receiver requests reporting for quantity
        ///     changes due to a therapeutic substitution that has occurred
        ///     <para />
        ///     or a preferred product/service substitution that has
        ///     occurred, or when a DUR alert has been resolved by
        ///     changing quantities.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "OriginallyPrescribedQuantity", Description = FieldDescriptions.OriginallyPrescribedQuantity, DataType = "Decimal", IsRequired = false)]
        public decimal? OriginallyPrescribedQuantity { get; set; }

        /// <summary>
        ///     Field Number: 308-C8
        ///     <para />
        ///     Description: Code indicating whether or not the patient has other insurance coverage.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed by receiver, to communicate a
        ///     summation of other coverage information that has been
        ///     collected from other payers.
        ///     <para />
        ///     Required for Coordination of Benefits.
        ///     <para />
        ///     See section “Specific Segment Discussion”, “Request
        ///     Segments”, “Claim Segment”, “Other Coverage Code(3Ø8-C8).
        ///     <para />
        ///     ProCare Field: OTHERCODE
        /// </summary>
        [ApiMember(Name = "OtherCoverageCode", Description = FieldDescriptions.OtherCoverageCode, DataType = "9(02)", IsRequired = false)]
        public string OtherCoverageCode { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 391-MT
        ///     <para />
        ///     Description: Code to indicate a patient’s choice on assignment of benefits.
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required when the claims adjudicator does not assume the
        ///     <para />
        ///     patient assigned his/her benefits to the provider or when
        ///     the claims adjudicator supports a patient determination of
        ///     <para />
        ///     whether he/she wants to assign or retain his/her benefits
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "PatientAssignmentIndicator", Description = FieldDescriptions.PatientAssignmentIndicator, DataType = "String", IsRequired = false)]
        public string PatientAssignmentIndicator { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 147-U7
        ///     <para />
        ///     Description: The type of service being performed by a pharmacy
        ///     when different contractual terms exist between a payer and the pharmacy,
        ///     or when benefits are based upon the type of service performed.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required when the submitter must clarify the type of
        ///     services being performed as a condition for proper
        ///     reimbursement by the payer.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "PharmacyServiceType", Description = FieldDescriptions.PharmacyServiceType, DataType = "9(02)", IsRequired = false)]
        public string PharmacyServiceType { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 402-D2
        ///     <para />
        ///     Description: Reference number assigned by the provider for the dispensed drug/product and/or service provided.
        ///     <para />
        ///     Format: 9(12)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     See section “Standard Conventions”, “Character Set
        ///     Designation Truncation”, “Numeric”, “Numeric Truncation
        ///     <para />
        ///     ProCare Field: RXNO
        /// </summary>
        [ApiMember(Name = "PrescriptionNumber", Description = FieldDescriptions.PrescriptionNumber, DataType = "9(12)", IsRequired = true)]
        public string PrescriptionNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 419-DJ
        ///     <para />
        ///     Description: Code indicating the origin of the prescription.
        ///     <para />
        ///     Format: 9(01)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for plan benefit administration.
        ///     <para />
        ///     ProCare Field: RXORIGIN
        /// </summary>
        [ApiMember(Name = "PrescriptionOriginCode", Description = FieldDescriptions.PrescriptionOriginCode, DataType = "9(01)", IsRequired = false)]
        public string PrescriptionOriginCode { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 455-EM
        ///     <para />
        ///     Description: Indicates the type of billing submitted.
        ///     <para />
        ///     Format: 9(01)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     For Transaction Code of “B1”, in the Claim Segment, the
        ///     Prescription/Service Reference Number Qualifier(455-EM)
        ///     is “1” (Rx Billing).
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        /// <remarks>
        ///     If 2, reject with ProCare Error 99, NCPDP Error EM, Message "SERVICE BILLING NOT ALLOWED"
        /// </remarks>
        [ApiMember(Name = "PrescriptionQualifier", Description = FieldDescriptions.PrescriptionQualifier, DataType = "9(01)", IsRequired = true)]
        public string PrescriptionQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 462-EV
        ///     <para />
        ///     Description: Number submitted by the provider to identify the prior authorization
        ///     <para />
        ///     Format: 9(11)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage,
        ///     pricing, or patient financial responsibility.
        ///     <para />
        ///     ProCare Field: PAUTHNO
        /// </summary>
        [ApiMember(Name = "PriorAuthorizationNumberSubmitted", Description = FieldDescriptions.PriorAuthorizationNumberSubmitted, DataType = "9(11)", IsRequired = false)]
        public string PriorAuthorizationNumberSubmitted { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 461-EU
        ///     <para />
        ///     Description: Code clarifying the Prior Authorization Number
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage,
        ///     pricing, or patient financial responsibility.
        ///     <para />
        ///     ProCare Field: PRIORAUTH
        /// </summary>
        [ApiMember(Name = "PriorAuthorizationTypeCode", Description = FieldDescriptions.PriorAuthorizationTypeCode, DataType = "9(02)", IsRequired = false)]
        public string PriorAuthorizationTypeCode { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 459-ER
        ///     <para />
        ///     Description: Identifies special circumstances related to the performance of the service.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required to define a further level of specificity if the
        ///     Product/Service ID(4Ø7-D7) indicated a Procedure Code was submitted
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "ProcedureModifierCodes", Description = FieldDescriptions.ProcedureModifierCodes, DataType = "9(02)", IsRequired = false)]
        public List<string> ProcedureModifierCodes { get; set; }

        /// <summary>
        ///     Field Number: 458-SE
        ///     <para />
        ///     Description: Count of the Procedure Modifier Code
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 10.
        ///     Required if Procedure Modifier Code(459-ER) is used.
        ///     <para />
        ///     Max: 10
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "ProcedureModifierCount", Description = FieldDescriptions.ProcedureModifierCount, DataType = "9(02)", IsRequired = false)]
        public int ProcedureModifierCount => ProcedureModifierCodes.Count;

        /// <summary>
        ///     Field Number: 407-D7
        ///     <para />
        ///     Description: ID of the product dispensed or service provided.
        ///     <para />
        ///     Format: X(19)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     If billing for a multi-ingredient prescription, Product/Service
        ///     ID(4Ø7-D7) is zero. (Zero means “Ø”.)
        ///     <para />
        ///     ProCare Field: NDC
        /// </summary>
        [ApiMember(Name = "ProductID", Description = FieldDescriptions.ProductId, DataType = "String", IsRequired = true)]
        public string ProductID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 436-E1
        ///     <para />
        ///     Description: Code qualifying the Product/Service ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     If billing for a multi-ingredient prescription, Product/Service
        ///     ID Qualifier(436-E1) is zero(“ØØ”).
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        /// <remarks>
        ///     *Currently, only value 03 (NDC) is accepted
        ///     <para></para>
        ///     <code>
        /// IF 436-E1 is empty THEN
        /// IF CPE Mode is Online or Adjudicator THEN
        ///    436-E1 = "03"
        /// ELSE
        ///    Error message "MISSING PRODUCT ID QUALIFIER"
        /// </code>
        /// </remarks>
        [ApiMember(Name = "ProductIDQualifier", Description = FieldDescriptions.ProductIdQualifier, DataType = "String", IsRequired = true)]
        public string ProductIDQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 442-E7
        ///     <para />
        ///     Description: Quantity dispensed expressed in metric decimal units.
        ///     <para />
        ///     Format: 9(7)v999
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare Field: SDECIQTY (Whole value in QTY)
        /// </summary>
        [ApiMember(Name = "QuantityDispensed", Description = FieldDescriptions.QuantityDispensed, DataType = "Decimal", IsRequired = true)]
        public decimal? QuantityDispensed { get; set; }

        /// <summary>
        ///     Field Number: 344-HF
        ///     <para />
        ///     Description: Metric decimal quantity of medication that would be dispensed on original filling if inventory were
        ///     available.
        ///     <para />
        ///     Format: 9(7)v999
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required for the partial fill or the completion fill of a
        ///     prescription.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "QuantityIntendedToBeDispensed", Description = FieldDescriptions.QuantityIntendedToBeDispensed, DataType = "Decimal", IsRequired = false)]
        public decimal? QuantityIntendedToBeDispensed { get; set; }

        /// <summary>
        ///     Field Number: 460-ET
        ///     <para />
        ///     Description: Amount expressed in metric decimal units.
        ///     <para />
        ///     Format: 9(7)v999
        ///     <para />
        ///     Designation:
        ///     <para />
        ///     Claim Billing/Encounter: Not used
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "QuantityPrescribed", Description = FieldDescriptions.QuantityPrescribed, DataType = "Decimal", IsRequired = false)]
        public decimal? QuantityPrescribed { get; set; }

        /// <summary>
        ///     Field Number: 415-DF
        ///     <para />
        ///     Description: Number of refills authorized by the prescriber.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation:
        ///     Claim Billing/Encounter: Required if necessary for plan benefit administration.
        ///     <para />
        ///     ProCare Field: AUTHREF
        /// </summary>
        [ApiMember(Name = "RefillsAuthorized", Description = FieldDescriptions.RefillsAuthorized, DataType = "9(02)", IsRequired = false)]
        public int? RefillsAuthorized { get; set; }

        /// <summary>
        ///     Field Number: 995-E2
        ///     <para />
        ///     Description: This is an override to the “default” route referenced for the product. For a multi-ingredient
        ///     compound,
        ///     it is the route of the complete compound mixture.
        ///     <para />
        ///     Format: X(11)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if specified in trading partner agreement.
        ///     <para />
        ///     ProCare Field: ROUTE
        /// </summary>
        [ApiMember(Name = "RouteOfAdministration", Description = FieldDescriptions.RouteOfAdministration, DataType = "String", IsRequired = false)]
        public string RouteOfAdministration { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 454-EK
        ///     <para />
        ///     Description: The serial number of the prescription blank/form.
        ///     <para />
        ///     Format: X(12)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for state/federal/regulatory agency
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "ScheduledPrescriptionIDNumber", Description = FieldDescriptions.ScheduledPrescriptionIdNumber, DataType = "String", IsRequired = false)]
        public string ScheduledPrescriptionIDNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 429-DT
        ///     <para />
        ///     Description: Code indicating the type of unit dose dispensing.
        ///     <para />
        ///     Format: 9(01)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage,
        ///     pricing, or patient financial responsibility.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "SpecialPackagingIndicator", Description = FieldDescriptions.SpecialPackagingIndicator, DataType = "9(01)", IsRequired = false)]
        public string SpecialPackagingIndicator { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 354-NX
        ///     <para />
        ///     Description: Count of the ‘Submission Clarification Code’ (420-DK) occurrences.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 3.
        ///     <para />
        ///     Required if Submission Clarification Code(42Ø-DK) is used.
        ///     <para />
        ///     Max: 3
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "SubmissionClarificationCodeCount", Description = FieldDescriptions.SubmissionClarificationCodeCount, DataType = "9(02)", IsRequired = false)]
        public int SubmissionClarificationCodeCount => SubmissionClarificationCodes.Count;

        /// <summary>
        ///     Field Number: 420-DK
        ///     <para />
        ///     Description: Code indicating that the pharmacist is clarifying the submission.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified - Repeating:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if clarification is needed and value submitted is
        ///     greater than zero(Ø).
        ///     <para />
        ///     Occurs the number of times identified in Submission
        ///     Clarification Code Count(354-NX).
        ///     <para />
        ///     If the Date of Service(4Ø1-D1) contains the subsequent
        ///     payer coverage date, the Submission Clarification Code
        ///     <para />
        ///     (42Ø-DK) is required with value of “19” (Split Billing –
        ///     indicates the quantity dispensed is the remainder billed to a
        ///     <para />
        ///     subsequent payer when Medicare Part A expires.Used
        ///     only in long-term care settings) for individual unit of use medications.
        ///     <para />
        ///     ProCare Field: SUBOVERRIDE
        /// </summary>
        [ApiMember(Name = "SubmissionClarificationCodes", Description = FieldDescriptions.SubmissionClarificationCodes, DataType = "9(02)", IsRequired = false)]
        public List<string> SubmissionClarificationCodes { get; set; }

        /// <summary>
        ///     Field Number: 880-K5
        ///     <para />
        ///     Description: A reference number assigned by the provider to each of the data records
        ///     in the batch or real-time transactions.
        ///     <para />
        ///     The purpose of this number is to facilitate the process of matching the transaction response
        ///     to the transaction. The transaction reference number assigned should be returned in the response
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        ///     Claim Billing/Encounter: Not used.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "TransactionReferenceNumber", Description = FieldDescriptions.TransactionReferenceNumber, DataType = "", IsRequired = false)]
        public string TransactionReferenceNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 600-28
        ///     <para />
        ///     Description: NCPDP standard product billing codes
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for state/federal/regulatory agency
        ///     programs.
        ///     <para />
        ///     Required if this field could result in different coverage,
        ///     pricing, or patient financial responsibility.
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "UnitOfMeasure", Description = FieldDescriptions.UnitOfMeasure, DataType = "String", IsRequired = false)]
        public string UnitOfMeasure { get; set; } = string.Empty;

    }
}
