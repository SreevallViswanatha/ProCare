using System;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestWorkersComp
    {
        /// <summary>
        ///     Field Number: 117-TR
        ///     <para />
        ///     Description: A code that identifies the entity submitting the billing transaction.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Required
        ///     <para />
        /// </summary>
        [ApiMember(Name = "BillingEntityTypeIndicator", Description = FieldDescriptions.BillingEntityTypeIndicator, DataType = "string", IsRequired = true)]
        public string BillingEntityTypeIndicator { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 327-CR
        ///     <para />
        ///     Description: Carrier code assigned in Worker's Compensation program
        ///     <para />
        ///     Format: X(10)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition.
        /// </summary>
        [ApiMember(Name = "CarrierID", Description = FieldDescriptions.CarrierId, DataType = "string", IsRequired = false)]
        public string CarrierID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 435-DZ
        ///     <para />
        ///     Description: Identifies the claim number assigned by the Worker's Compensation program.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition.
        /// </summary>
        [ApiMember(Name = "ClaimID", Description = FieldDescriptions.ClaimId, DataType = "string", IsRequired = false)]
        public string ClaimID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 434-DY
        ///     <para />
        ///     Description: Date on which the injury occurred.
        ///     <para />
        ///     Format: 9(08)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "DateofInjury", Description = FieldDescriptions.DateofInjury, DataType = "string", Format= "Date", IsRequired = true)]
        public DateTime? DateofInjury { get; set; }

        /// <summary>
        ///     Field Number: 317-CH
        ///     <para />
        ///     Description: Free-form text for city name.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition
        /// </summary>
        [ApiMember(Name = "EmployerCity", Description = FieldDescriptions.EmployerCity, DataType = "string", IsRequired = false)]
        public string EmployerCity { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 321-CL
        ///     <para />
        ///     Description: Employer primary contact.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition.
        /// </summary>
        [ApiMember(Name = "EmployerContact", Description = FieldDescriptions.EmployerContact, DataType = "string", IsRequired = false)]
        public string EmployerContact { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 315-CF
        ///     <para />
        ///     Description: Complete name of employer.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition.
        /// </summary>
        [ApiMember(Name = "EmployerName", Description = FieldDescriptions.EmployerName, DataType = "string", IsRequired = false)]
        public string EmployerName { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 320-CK
        ///     <para />
        ///     Description: Ten-digit phone number of employer.
        ///     <para />
        ///     Format: 9(10)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition.
        /// </summary>
        [ApiMember(Name = "EmployerPhone", Description = FieldDescriptions.EmployerPhone, DataType = "string", IsRequired = false)]
        public string EmployerPhone { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 318-CI
        ///     <para />
        ///     Description: Standard state/province code as defined by appropriate government agency.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition.
        /// </summary>
        [ApiMember(Name = "EmployerState", Description = FieldDescriptions.EmployerState, DataType = "string", IsRequired = false)]
        public string EmployerState { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 316-CG
        ///     <para />
        ///     Description: Free-form text for address information.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition
        /// </summary>
        [ApiMember(Name = "EmployerStreet", Description = FieldDescriptions.EmployerStreet, DataType = "string", IsRequired = false)]
        public string EmployerStreet { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 319-CJ
        ///     <para />
        ///     Description: Code defining international postal zone excluding punctuation and blanks (zip code for US).
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to process a claim/encounter for a work related injury or condition.
        /// </summary>
        [ApiMember(Name = "EmployerZip", Description = FieldDescriptions.EmployerZip, DataType = "string", IsRequired = false)]
        public string EmployerZip { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 126-UA
        ///     <para />
        ///     Description: Identifies the generic equivalent of the brand product dispensed.
        ///     <para />
        ///     Format: X(19)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for state/federal/regulatory agency programs.
        /// </summary>
        [ApiMember(Name = "GenericEquivalentProductID", Description = FieldDescriptions.GenericEquivalentProductId, DataType = "string", IsRequired = false)]
        public string GenericEquivalentProductID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 125-TZ
        ///     <para />
        ///     Description: Code qualifying the ‘Generic Equivalent Product ID’ (126-UA).
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Generic Equivalent Product ID(126-UA) is used.
        /// </summary>
        [ApiMember(Name = "GenericEquivalentProductIDQualifier", Description = FieldDescriptions.GenericEquivalentProductIdQualifier, DataType = "string", IsRequired = false)]
        public string GenericEquivalentProductIDQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 122-TW
        ///     <para />
        ///     Description: City of the entity to receive payment for claim.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualfied
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if transaction is submitted by a provider or agent, but paid to another party
        /// </summary>
        [ApiMember(Name = "PayToCity", Description = FieldDescriptions.PayToCity, DataType = "string", IsRequired = false)]
        public string PayToCity { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 119-TT
        ///     <para />
        ///     Description: Identifying number of the entity to receive payment for claim.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if transaction is submitted by a provider or agent, but paid to another party
        /// </summary>
        [ApiMember(Name = "PayToID", Description = FieldDescriptions.PayToId, DataType = "string", IsRequired = false)]
        public string PayToID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 120-TU
        ///     <para />
        ///     Description: Name of the entity to receive payment for claim.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if transaction is submitted by a provider or agent, but paid to another party.
        /// </summary>
        [ApiMember(Name = "PayToName", Description = FieldDescriptions.PayToName, DataType = "string", IsRequired = false)]
        public string PayToName { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 118-TS
        ///     <para />
        ///     Description: Code qualifying the ‘Pay To ID’ (119-TT).
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Pay To ID(119-TT) is used.
        /// </summary>
        [ApiMember(Name = "PayToQualifier", Description = FieldDescriptions.PayToQualifier, DataType = "string", IsRequired = false)]
        public string PayToQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 123-TX
        ///     <para />
        ///     Description: Standard state/province code as defined by appropriate government agency.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if transaction is submitted by a provider or agent, but paid to another party
        /// </summary>
        [ApiMember(Name = "PayToState", Description = FieldDescriptions.PayToState, DataType = "string", IsRequired = false)]
        public string PayToState { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 121-TV
        ///     <para />
        ///     Description: Street address of the entity to receive payment for claim.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if transaction is submitted by a provider or agent, but paid to another party
        /// </summary>
        [ApiMember(Name = "PayToStreet", Description = FieldDescriptions.PayToStreet, DataType = "string", IsRequired = false)]
        public string PayToStreet { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 124-TY
        ///     <para />
        ///     Description: Code defining international postal zone excluding punctuation and blanks (zip code for US).
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if transaction is submitted by a provider or agent, but paid to another party.
        /// </summary>
        [ApiMember(Name = "PayToZip", Description = FieldDescriptions.PayToZip, DataType = "string", IsRequired = false)]
        public string PayToZip { get; set; } = string.Empty;


    }
}
