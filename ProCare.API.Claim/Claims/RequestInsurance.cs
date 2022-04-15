using ProCare.NCPDP.Telecom;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestInsurance
    {
        /// <summary>
        ///     Field Number: 312-CC
        ///     <para />
        ///     Description: Individual first name.
        ///     <para />
        ///     Format: X(12)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Service Billing:
        ///     Required if necessary for state/federal/regulatory agency programs when the cardholder has a first name.
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "CardholderFirstName", Description = FieldDescriptions.CardholderFirstName, DataType = "string", IsRequired = false)]
        public string CardholderFirstName { get; set; }

        /// <summary>
        ///     Field Number: 302-C2
        ///     <para />
        ///     Description: Insurance ID assigned to the cardholder.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     ProCare FieldName: CARDID + CARDID2
        /// </summary>
        [ApiMember(Name = "CardholderId", Description = FieldDescriptions.CardholderId, DataType = "string", IsRequired = true)]
        public string CardholderID { get; set; }

        /// <summary>
        ///     Field Number: 313-CD
        ///     <para />
        ///     Description: Individual last name. 
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Billing:
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "CardholderLastName", Description = FieldDescriptions.CardholderLastName, DataType = "string", IsRequired = false)]
        public string CardholderLastName { get; set; }

        /// <summary>
        ///     Field Number: 997-G2
        ///     <para />
        ///     Description: Indicates that the patient resides in a facility that qualifies for the CMS Part D benefit.
        ///     <para />
        ///     Format: X(01) Y=Yes, N=No
        ///     <para />
        ///     Designation:
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "CmsPartDDefinedQualifiedFacility", Description = FieldDescriptions.CmsPartDDefinedQualifiedFacility, DataType = "string", IsRequired = false)]
        public string CmsPartDDefinedQualifiedFacility { get; set; }

        /// <summary>
        ///     Field Number: 309-C9
        ///     <para />
        ///     Description: Code indicating that the pharmacy is clarifying eligibility based on receiving a denial.
        ///     <para />
        ///     Format: 9(01)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed for receiver inquiry validation and/or
        ///     determination, when eligibility is not maintained at the
        ///     <para />
        ///     dependent level.Required in special situations as defined
        ///     by the code to clarify the eligibility of an individual, which
        ///     <para />
        ///     may extend coverage.
        ///     <para />
        ///     ProCare FieldName:  OVERRIDE
        /// </summary>
        public EligibilityClarificationCode EligibilityClarificationCode { get; set; }

        /// <summary>
        ///     Field Number: 301-C1
        ///     <para />
        ///     Description: ID assigned to the cardholders or employers group.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Reversal:
        ///     Required if needed to match the reversal to the original billing transaction.
        ///     <para />
        ///     ProCare FieldName: PLNID
        /// </summary>
        [ApiMember(Name = "GroupID", Description = FieldDescriptions.GroupId, DataType = "string", IsRequired = false)]
        public string GroupID { get; set; }

        /// <summary>
        ///     Field Number: 314-CE
        ///     <para />
        ///     Description: Blue Cross/Blue Shield plan ID
        ///     <para />
        ///     Format: X(03)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed for receiver billing/encounter validation
        ///     and/or determination for Blue Cross or Blue Shield, if a
        ///     <para />
        ///     Patient has coverage under more than one plan, to
        ///     distinguish each plan.
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "HomePlan", Description = FieldDescriptions.HomePlan, DataType = "string", IsRequired = false)]
        public string HomePlan { get; set; }

        /// <summary>
        ///     Field Number: 116-N6
        ///     <para />
        ///     Description: Number assigned by processor to identify the individual Medicaid Agency or representative.
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation: Not used for Billing
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "MedicaidAgencyNumber", Description = FieldDescriptions.MedicaidAgencyNumber, DataType = "string", IsRequired = false)]
        public string MedicaidAgencyNumber { get; set; }

        /// <summary>
        ///     Field Number:115-N5
        ///     <para />
        ///     Description: A unique member identification number assigned by the Medicaid Agency.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation:
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "MedicaidIDNumber", Description = FieldDescriptions.MedicaidIdNumber, DataType = "string", IsRequired = false)]
        public string MedicaidIDNumber { get; set; }

        /// <summary>
        ///     Field Number:360-2B
        ///     <para />
        ///     Description: Two character State Postal Code indicating the state where Medicaid coverage exists.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required, if known, when patient has Medicaid coverage.
        ///     <para />
                ///     ProCare FieldName:
                /// </summary>
        [ApiMember(Name = "MedicaidIndicator", Description = FieldDescriptions.MedicaidIndicator, DataType = "string", IsRequired = false)]
        public string MedicaidIndicator { get; set; }

        /// <summary>
        ///     Field Number: 359-2A
        ///     <para />
        ///     Description: Patient’s ID assigned by the Medigap Insurer
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required, if known, when patient has Medigap coverage.
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "MedigapID", Description = FieldDescriptions.MedigapId, DataType = "string", IsRequired = false)]
        public string MedigapID { get; set; }

        /// <summary>
        ///     Field Number: 990-MG
        ///     <para />
        ///     Description:Card Issuer or Bank ID used for network routing.
        ///     <para />
        ///     Format: Not used in Billing
        ///     <para />
        ///     Designation: Qualified 
        ///     <para />
        ///     Information Reporting (Claim):
        ///     Required for Medicare Part D payer-to-payer facilitation when necessary to match the information reporting reversal
        ///     transaction to the original information reporting transaction.
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "OtherPayerBinNumber", Description = FieldDescriptions.OtherPayerBinNumber, DataType = "string", IsRequired = false)]
        public string OtherPayerBinNumber { get; set; }

        /// <summary>
        ///     Field Number: 356-NU 
        ///     <para />
        ///     Description: Cardholder ID for this member that is associated with the Payer noted.
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Information Reporting (Claim):
        ///     Required for Medicare Part D payer-to-payer facilitation when necessary to match the information reporting reversal
        ///     transaction to the original information reporting transaction.
        ///     <para />
        ///     Not used in Billing
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "OtherPayerCardholderID", Description = FieldDescriptions.OtherPayerCardholderId, DataType = "string", IsRequired = false)]
        public string OtherPayerCardholderID { get; set; }

        /// <summary>
        ///     Field Number: 992-MJ
        ///     <para />
        ///     Description: ID assigned to the cardholder group or employer group by the secondary, tertiary, etc. payer.
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Information Reporting (Claim):
        ///     Required for Medicare Part D payer-to-payer facilitation when necessary to match the information reporting reversal
        ///     transaction to the original information reporting transaction.
        ///     <para />
        ///     Not used in Billing
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "OtherPayerGroupID", Description = FieldDescriptions.OtherPayerGroupId, DataType = "string", IsRequired = false)]
        public string OtherPayerGroupID { get; set; }

        /// <summary>
        ///     Field Number: 991-MH
        ///     <para />
        ///     Description: A number that uniquely identifies the secondary, tertiary, etc. payer to the processor.
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Information Reporting (Claim):
        ///     Required for Medicare Part D payer-to-payer facilitation when necessary to match the information reporting reversal
        ///     transaction to the original information reporting transaction.
        ///     <para />
        ///     Not used in Billing
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "OtherPayerProcessorControlNumber", Description = FieldDescriptions.OtherPayerProcessorControlNumber, DataType = "string", IsRequired = false)]
        public string OtherPayerProcessorControlNumber { get; set; }

        /// <summary>
        ///     Field Number:306-C6
        ///     <para />
        ///     Description: Code identifying relationship of patient to cardholder.
        ///     <para />
        ///     Format: 9(01)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Information Reporting (Claim/Service):
        ///     Required if needed to uniquely identify the relationship of the Patient to the Cardholder ID.
        ///     <para />
        ///     ProCare FieldName: RELCD
        /// </summary>
        public PatientRelationshipCode PatientRelationshipCode { get; set; }
     
        /// <summary>
        ///     Field Number: 303-C3
        ///     <para />
        ///     Description: Code assigned to a specific person within a family.
        ///     <para />
        ///     Format: X(03)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Information Reporting (Claim/Service):
        ///     Required if needed to uniquely identify the family members within the Cardholder ID.
        ///     <para />
        ///     ProCare FieldName: PERSON
        /// </summary>
        [ApiMember(Name = "PersonCode", Description = FieldDescriptions.InsurancePersonCode, DataType = "string", IsRequired = false)]
        public string PersonCode { get; set; }

        /// <summary>
        ///     Field Number: 524-FO
        ///     <para />
        ///     Description: Assigned by the processor to identify coverage criteria used to adjudicate a claim.
        ///     <para />
        ///     Format: X(08)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Information Reporting (Claim/Service):
        ///     Required to identify the actual plan ID that was used when multiple group coverages exist.
        ///     <para />
        ///     Required if needed to contain the actual plan ID if unknown to the receiver.
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "PlanID", Description = FieldDescriptions.PlanId, DataType = "string", IsRequired = false)]
        public string PlanID { get; set; }

        /// <summary>
        ///     Field Number: 361-2D
        ///     <para />
        ///     Description: Code indicating whether the provider accepts assignment.
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation:
        ///     <para />
        ///     ProCare FieldName:
        /// </summary>
        [ApiMember(Name = "ProviderAcceptAssignmentIndicator", Description = FieldDescriptions.ProviderAcceptAssignmentIndicator, DataType = "string", IsRequired = false)]
        public string ProviderAcceptAssignmentIndicator { get; set; }


    }
}
