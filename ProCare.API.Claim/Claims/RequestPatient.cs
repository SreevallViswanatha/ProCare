using System;
using ServiceStack;
using ProCare.NCPDP.Telecom;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestPatient
    {
        /// <summary>
        ///     Field Number: 323-CN
        ///     <para />
        ///     Description: Free form text for city name
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Information Reporting Rebill (Claim/Service):
        ///     Required if needed to assist in identifying the patient when specific eligibility cannot be established.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "City", Description = FieldDescriptions.PersonCity, DataType = "string", IsRequired = false)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 304-C4
        ///     <para />
        ///     Description: Date of birth of patient.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare FieldName: DOB
        /// </summary>
        [ApiMember(Name = "DateOfBirth", Description = FieldDescriptions.PersonDateOfBirth, DataType = "string", Format="Date", IsRequired = true)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        ///     Field Number: 350-HN
        ///     <para />
        ///     Description: The E-Mail address of the patient (member).
        ///     <para />
        ///     Format: X(80)
        ///     <para />
        ///     Designation: Informational
        ///     <para />
        ///     Information Reporting Rebill (Claim/Service):
        ///     May be submitted for the receiver to relay patient health care communications via the Internet when provided by the
        ///     patient.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "EmailAddress", Description = FieldDescriptions.PersonEmailAddress, DataType = "string", IsRequired = false)]
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 333-CZ
        ///     <para />
        ///     Description: ID assigned to the employer.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Information Reporting Rebill (Claim/Service):
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     Required if needed for Workers’ Compensation reporting.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "EmployerID", Description = FieldDescriptions.EmployerId, DataType = "string", IsRequired = false)]
        public string EmployerID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 310-CA
        ///     <para />
        ///     Description: Patient's first name
        ///     <para />
        ///     Format: X(12)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Information Reporting Rebill (Claim/Service): Required if known.
        ///     <para />
        ///     ProCare FieldName: FNAME
        /// </summary>
        [ApiMember(Name = "FirstName", Description = FieldDescriptions.FirstName, DataType = "string", IsRequired = false)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 305-C5
        ///     <para />
        ///     Description: Code indicating the gender of the patient.
        ///     <para />
        ///     Format: 9(01)
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare FieldName: Sex (convert to M/F)
        /// </summary>
        [ApiMember(Name = "Gender", Description = FieldDescriptions.PersonGender, DataType = "string", IsRequired = true)]
        public PatientGender Gender { get; set; } = PatientGender.NotSpecified;

        /// <summary>
        ///     Field Number: 311-CB
        ///     <para />
        ///     Description: Patient's last name.
        ///     <para />
        ///     Format:X(15)
        ///     <para />
        ///     Designation: Required
        ///     <para />
        ///     ProCare FieldName: LNAME
        /// </summary>
        [ApiMember(Name = "LastName", Description = FieldDescriptions.LastName, DataType = "string", IsRequired = true)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 332-CY
        ///     <para />
        ///     Description: ID assigned to the patient.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation:Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for state/federal/regulatory agency
        ///     <para />
        ///     programs to validate dual eligibility.
        ///     <para />
        ///     ProCare FieldName: CARDID + CARDID2
        /// </summary>
        [ApiMember(Name = "PatientID", Description = FieldDescriptions.PersonPatientId, DataType = "string", IsRequired = false)]
        public string PatientID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 331-CX
        ///     <para />
        ///     Description: Code qualifying the Patient ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Patient ID(332-CY) is used.
        ///     <para />
        ///     ProCare FieldMName: n/a
        /// </summary>
        public PatientIdQualifier PatientIDQualifier { get; set; } = PatientIdQualifier.Unknown;

        /// <summary>
        ///     Field Number: 326-CQ
        ///     <para />
        ///     Description: Patient's 10-digit phone number
        ///     <para />
        ///     Format: 9(10)
        ///     <para />
        ///     Designation: Optional
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "Phone", Description = FieldDescriptions.PatientPhone, DataType = "string", IsRequired = false)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 307-C7
        ///     <para />
        ///     Description: Code identifying the location of the patient when receiving pharmacy services
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage,
        ///     pricing, or patient financial responsibility
        ///     <para />
        ///     ProCare FieldName: PATLOC ??
        /// </summary>
        public PlaceOfService PlaceOfService { get; set; } = PlaceOfService.NotSpecified;

        /// <summary>
        ///     Field Number: 335-2C
        ///     <para />
        ///     Description: Code indicating whether the patient is pregnant or not.
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Eligibility Verification:
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "PregnancyIndicator", Description = FieldDescriptions.PregnancyIndicator, DataType = "string", IsRequired = false)]
        public string PregnancyIndicator { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 384-4X
        ///     <para />
        ///     Description: Code identifying the patient’s place of residence.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Eligibility Verification:
        ///     Required if this field could result in different coverage, pricing, or patient financial responsibility.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "Residence", Description = FieldDescriptions.Residence, DataType = "string", IsRequired = false)]
        public string Residence { get; set; } = string.Empty;

        /// <summary>
        ///     334-1C
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary> 
        public SmokerIndicator SmokerCode { get; set; } = SmokerIndicator.NotSpecified;

        /// <summary>
        ///     Field Number: 324-CO
        ///     <para />
        ///     Description: Standard state/province code as defined by appropriate government agency.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Optional
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "State", Description = FieldDescriptions.PersonState, DataType = "string", IsRequired = false)]
        public string State { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 322-CM
        ///     <para />
        ///     Description: Free form text for address information.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Optional
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "Street", Description = FieldDescriptions.PatientAddress, DataType = "string", IsRequired = false)]
        public string Street { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 325-CP
        ///     <para />
        ///     Description: Code defining international postal zone excluding punctuation and blanks (zip code for US).
        ///     <para />
        ///     Format: X(150
        ///     <para />
        ///     Designation: Optional
        ///     <para />
        ///     ProCare FieldName: n/a
        /// </summary>
        [ApiMember(Name = "Zip", Description = FieldDescriptions.PatientZip, DataType = "string", IsRequired = false)]
        public string Zip { get; set; } = string.Empty;
        
    }
}
