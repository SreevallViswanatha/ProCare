using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestPrescriber
    {
        /// <summary>
        ///     Field Number: 366-2M
        ///     <para />
        ///     Description: Free form text for prescriber city name.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to assist in identifying the prescriber.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs
        ///     <para />
        /// </summary>
        [ApiMember(Name = "City", Description = FieldDescriptions.PrescriberCity, DataType = "string", IsRequired = false)]
        public string City { get; set; }


        /// <summary>
        ///     Field Number: 364-2J
        ///     <para />
        ///     Description: Individual first name
        ///     <para />
        ///     Format: X(12)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Billing:
        ///     Required if needed to assist in identifying the prescriber.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "FirstName", Description = FieldDescriptions.PrescriberFirstName, DataType = "string", IsRequired = false)]
        public string FirstName { get; set; }

        /// <summary>
        ///     Field Number: 427-DR
        ///     <para />
        ///     Description: Individual last name. 
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Billing:
        ///     Required when the Prescriber ID(411-DB) is not known.
        ///     <para />
        ///     Required if needed for Prescriber ID (411-DB) validation/clarification.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "LastName", Description = FieldDescriptions.PrescriberLastName, DataType = "string", IsRequired = false)]
        public string LastName { get; set; }

        /// <summary>
        ///     Field Number: 498-PM
        ///     <para />
        ///     Description: Prescribers 10-digit phone number
        ///     <para />
        ///     Format: 9(10)
        ///     <para />
        ///     Designation:
        ///     <para />
        ///     Encounter:
        ///     Required if needed for Prior Authorization process.
        ///     <para />
        ///     Claim Billing:
        ///     Required if needed for Workers’ Compensation.
        ///     <para />
        ///     Required if needed to assist in identifying the prescriber.
        ///     <para />
        ///     Required if needed for Prior Authorization process.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "Phone", Description = FieldDescriptions.PrescriberPhone, DataType = "string", IsRequired = false)]
        public string Phone { get; set; }

        /// <summary>
        ///     Field Number: 411-DB
        ///     <para />
        ///     Description: ID assigned to the prescriber.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage or patient financial responsibility.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PrescriberID", Description = FieldDescriptions.PrescriberId, DataType = "string", IsRequired = false)]
        public string PrescriberID { get; set; }

        /// <summary>
        ///     Field Number: 466-EZ
        ///     <para />
        ///     Description: Code qualifying the Prescriber ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Prescriber ID(411-DB) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PrescriberIDQualifier", Description = FieldDescriptions.PrescriberIdQualifier, DataType = "string", IsRequired = false)]
        public string PrescriberIDQualifier { get; set; }

        /// <summary>
        ///     Field Number: 421-DL
        ///     <para />
        ///     Description: Assigned to the primary care provider.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed for receiver claim/encounter determination, if known and available.
        ///     <para />
        ///     Required if this field could result in different coverage or patient financial responsibility.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PrimaryCareProviderID", Description = FieldDescriptions.PrimaryCareProviderId, DataType = "string", IsRequired = false)]
        public string PrimaryCareProviderID { get; set; }

        /// <summary>
        ///     Field Number:  468-2E
        ///     <para />
        ///     Description: Code qualifying the Primary Care Provider ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Primary Care Provider ID(421-DL) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PrimaryCareProviderIDQualifier", Description = FieldDescriptions.PrimaryCareProviderIdQualifier, DataType = "string", IsRequired = false)]
        public string PrimaryCareProviderIDQualifier { get; set; }

        /// <summary>
        ///     Field Number: 470-4E
        ///     <para />
        ///     Description: Providers last name.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field is used as an alternative for Primary Care Provider ID(421-DL) when ID is not known.
        ///     <para />
        ///     Required if needed for Primary Care Provider ID (421-DL) validation/clarification.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PrimaryCareProviderLastName", Description = FieldDescriptions.PrimaryCareProviderLastName, DataType = "string", IsRequired = false)]
        public string PrimaryCareProviderLastName { get; set; }

        /// <summary>
        ///     Field Number: 367-2N
        ///     <para />
        ///     Description: Standard state/province code as defined by appropriate government agency.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to assist in identifying the prescriber.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PrescriberState", Description = FieldDescriptions.PrescriberState, DataType = "string", IsRequired = false)]
        public string State { get; set; }

        /// <summary>
        ///     Field Number: 365-2K
        ///     <para />
        ///     Description: Free Form text for prescriber address information.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to assist in identifying the prescriber.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PrescriberStreet", Description = FieldDescriptions.PrescriberStreet, DataType = "string", IsRequired = false)]
        public string Street { get; set; }

        /// <summary>
        ///     Field Number: 368-2P
        ///     <para />
        ///     Description: Code defining international postal zone excluding punctuation and blanks (zip code for US).
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to assist in identifying the prescriber.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PrescriberZip", Description = FieldDescriptions.PrescriberZip, DataType = "string", IsRequired = false)]
        public string Zip { get; set; }
    }
}
