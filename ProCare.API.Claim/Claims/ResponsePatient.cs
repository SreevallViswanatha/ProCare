using System;
using ProCare.API.Core;
using ServiceStack;

namespace ProCare.API.Claims.Claims
{
    public class ResponsePatient
    {

        /// <summary>
        ///     Field Number: 304-C4
        ///     <para />
        ///     Description: Date of birth of patient.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if known.
        /// </summary>
        [ApiMember(Name = "DateOfBirth", Description = FieldDescriptions.PersonDateOfBirth, DataType = "string",Format="Date", IsRequired = false)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        ///     Field Number: 310-CA
        ///     <para />
        ///     Description: Patient's first name.
        ///     <para />
        ///     Format: X(12)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if known.
        /// </summary>
        [ApiMember(Name = "FirstName", Description = FieldDescriptions.FirstName, DataType = "string", IsRequired = false)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 311-CB
        ///     <para />
        ///     Description: Patient's last name.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if known.
        /// </summary>
        [ApiMember(Name = "LastName", Description = FieldDescriptions.LastName, DataType = "string", IsRequired = false)]
        public string LastName { get; set; } = string.Empty;

    }
}