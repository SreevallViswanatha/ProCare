using System;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestPriorAuthorization
    {

        /// <summary>
        ///     Field Number: 503-F3
        ///     <para />
        ///     Description: Number assigned by the processor to identify an authorized transaction.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if needed for receiver claim/encounter determination.
        /// </summary>
        [ApiMember(Name = "AuthorizationNumber", Description = FieldDescriptions.AuthorizationNumber, DataType = "string", IsRequired = false)]
        public string AuthorizationNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PD
        ///     <para />
        ///     Description: Code describing the reason for prior authorization request.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "BasisOfRequest", Description = FieldDescriptions.BasisOfRequest, DataType = "string", IsRequired = true)]
        public string BasisOfRequest { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PH
        ///     <para />
        ///     Description: Free-form text for city name.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if needed for receiver claim/encounter or prior authorization determination.
        /// </summary>
        [ApiMember(Name = "City", Description = FieldDescriptions.PriorAuthCity, DataType = "string", IsRequired = false)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PE
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: X(12)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if needed for receiver claim/encounter or prior authorization determination.
        /// </summary>
        [ApiMember(Name = "FirstName", Description = FieldDescriptions.PriorAuthFirstName, DataType = "string", IsRequired = false)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PF
        ///     <para />
        ///     Description: Last name of the patient’s authorized representative.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if needed for receiver claim/encounter or prior authorization determination.
        /// </summary>
        [ApiMember(Name = "LastName", Description = FieldDescriptions.PriorAuthLastName, DataType = "string", IsRequired = false)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PB
        ///     <para />
        ///     Description: The beginning date of need.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PeriodBegin", Description = FieldDescriptions.PeriodBegin, DataType = "string", Format="Date", IsRequired = true)]
        public DateTime? PeriodBegin { get; set; }

        /// <summary>
        ///     Field Number: 498-PC
        ///     <para />
        ///     Description: Ending date for a prior authorization request.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PeriodEnd", Description = FieldDescriptions.PeriodEnd, DataType = "string", Format="Date", IsRequired = true)]
        public DateTime? PeriodEnd { get; set; }

        /// <summary>
        ///     Field Number: 498-PY
        ///     <para />
        ///     Description: Unique number identifying the prior authorization assigned by the processor.
        ///     <para />
        ///     Format: 9(11)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if the Request Type(498-PA) = 2 (Reauthorization)
        /// </summary>
        [ApiMember(Name = "PriorAuthorizationNumberAssigned", Description = FieldDescriptions.PriorAuthorizationNumberAssigned, DataType = "string", IsRequired = false)]
        public string PriorAuthorizationNumberAssigned { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PA
        ///     <para />
        ///     Description: Code identifying type of prior authorization request.
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "RequestType", Description = FieldDescriptions.RequestType, DataType = "string", IsRequired = true)]
        public string RequestType { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PJ
        ///     <para />
        ///     Description: Standard state/province code as defined by appropriate government agency.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if needed for receiver claim/encounter or prior authorization determination
        /// </summary>
        [ApiMember(Name = "State", Description = FieldDescriptions.PriorAuthState, DataType = "string", IsRequired = false)]
        public string State { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number:  498-PG
        ///     <para />
        ///     Description: Free-form text for address information.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if needed for receiver claim/encounter or prior authorization determination
        /// </summary>
        [ApiMember(Name = "Street", Description = FieldDescriptions.PriorAuthStreet, DataType = "string", IsRequired = false)]
        public string Street { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PP
        ///     <para />
        ///     Description: This space is being used to store CMN information, Narrative information, Facility information, and 
        ///     Compound Ingredient Modifiers that are not available elsewhere in the NCPDP format. Details on the fields are listed below.
        ///     <para />
        ///     Format: X(500)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if needed for receiver claim/encounter or prior authorization determination.
        /// </summary>
        [ApiMember(Name = "SupportingDocumentation", Description = FieldDescriptions.PriorAuthSupportingDocumentation, DataType = "string", IsRequired = false)]
        public string SupportingDocumentation { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 498-PK
        ///     <para />
        ///     Description: Code defining international postal zone excluding punctuation and blanks (zip code for US).
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Prior Authorization Request And Billing (Claim/Service):
        ///     Required if needed for receiver claim/encounter or prior authorization determination.
        /// </summary>
        [ApiMember(Name = "Zip", Description = FieldDescriptions.PriorAuthZip, DataType = "string", IsRequired = false)]
        public string Zip { get; set; } = string.Empty;

    }
}
