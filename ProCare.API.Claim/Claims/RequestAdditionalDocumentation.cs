using System;
using System.Collections.Generic;
using ProCare.API.Core;
using ServiceStack;

namespace ProCare.API.Claims.Claims
{
    public class RequestAdditionalDocumentation
    {
        /// <summary>
        ///     Field Number: 369-2Q
        ///     <para />
        ///     Description: Unique identifier for the data being submitted
        ///     <para />
        ///     Format: X(03)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "AdditionalDocumentationTypeID", Description = FieldDescriptions.AdditionalDocumentationTypeId, DataType = "String", IsRequired = true)]
        public string AdditionalDocumentationTypeID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 370-2R
        ///     <para />
        ///     Description: Length of time the physician expects the patient to require use of the ordered item.
        ///     <para />
        ///     Format: 9(03)
        ///     <para />
        ///     Designation: Qualified: Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "LengthOfNeed", Description = FieldDescriptions.LengthOfNeed, DataType = "String", IsRequired = false)]
        public string LengthOfNeed { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 371-2S
        ///     <para />
        ///     Description: Code qualifying the length of need.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified: Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "LengthOfNeedQualifier", Description = FieldDescriptions.LengthOfNeedQualifier, DataType = "String", IsRequired = false)]
        public string LengthOfNeedQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 372-2T
        ///     <para />
        ///     Description: The date the form was completed and signed by the ordering physician.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Qualified: Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "PrescriberDateSigned", Description = FieldDescriptions.PrescriberDateSigned, DataType = "String", Format = "Date", IsRequired = false)]
        public DateTime? PrescriberDateSigned { get; set; }

        /// <summary>
        ///     Field Number: 377-2Z
        ///     <para />
        ///     Description: Count of Question Number/Letter occurrences.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified: Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     Max: 50
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "QuestionCount", Description = FieldDescriptions.QuestionCount, DataType = "Int", IsRequired = false)]
        public int QuestionCount => Questions.Count;

        public List<DocumentationQuestions> Questions { get; set; }

        /// <summary>
        ///     Field Number: 374-2V
        ///     <para />
        ///     Description: The beginning date of need.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Qualified: Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "RequestPeriodBegin", Description = FieldDescriptions.RequestPeriodBegin, DataType = "String", Format = "Date", IsRequired = false)]
        public DateTime? RequestPeriodBegin { get; set; }

        /// <summary>
        ///     Field Number: 375-2W
        ///     <para />
        ///     Description: The effective date of the revision or re-certification provided by the certifying physician.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Qualified: Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "RequestPeriodRecertDate", Description = FieldDescriptions.RequestPeriodRecertDate, DataType = "String", Format = "Date", IsRequired = false)]
        public DateTime? RequestPeriodRecertDate { get; set; }

        /// <summary>
        ///     Field Number: 373-2U
        ///     <para />
        ///     Description: Code identifying type of request.
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Qualified: Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "RequestStatus", Description = FieldDescriptions.RequestStatus, DataType = "String", IsRequired = false)]
        public string RequestStatus { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 376-2X
        ///     <para />
        ///     Description: Free text message
        ///     <para />
        ///     Format: X(65)
        ///     <para />
        ///     Designation: Qualified: Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "SupportingDocumentation", Description = FieldDescriptions.SupportingDocumentation, DataType = "String", IsRequired = false)]
        public string SupportingDocumentation { get; set; } = string.Empty;

    }

    public class DocumentationQuestions
    {
        /// <summary>
        ///     Field Number: 383-4K
        ///     <para />
        ///     Description: Alphanumeric response to a question (part of the question information).
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Eligibility Verification:
        ///     Required if necessary for state/federal/regulatory agency programs to respond to questions included on a Medicare
        ///     form that requires an alphanumeric as the response. (At least one response is required per question.)
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "AlphaResponse", Description = FieldDescriptions.AlphaResponse, DataType = "String", IsRequired = false)]
        public string AlphaResponse { get; set; }

        /// <summary>
        ///     Field Number: 380-4G
        ///     <para />
        ///     Description: Date response to a question (part of the question information)
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Qualified Repeating:
        ///     <para />
        ///     Eligibility Verification:
        ///     Required if necessary for state/federal/regulatory agency programs to respond to questions included on a Medicare
        ///     form that requires a date as the response. (At least one response is required per question.)
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "DateResponse", Description = FieldDescriptions.DateResponse, DataType = "String", Format = "Date", IsRequired = false)]
        public DateTime? DateResponse { get; set; }

        /// <summary>
        ///     Field Number: 381-4H
        ///     <para />
        ///     Description: Dollar Amount response to a question (part of the question information)
        ///     <para />
        ///     Format: s9(9)v99
        ///     <para />
        ///     Designation: Qualified Repeating:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for state/federal/regulatory agency programs to respond to questions included on a Medicare
        ///     form that requires a dollar amount as the response. (At least one response is required per question.)
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "DollarResponse", Description = FieldDescriptions.DollarResponse, DataType = "Decimal", IsRequired = false)]
        public decimal? DollarResponse { get; set; }

        /// <summary>
        ///     Field Number: 382-4J
        ///     <para />
        ///     Description: Numeric response to a question (part of the question information)
        ///     <para />
        ///     Format: 9(11)
        ///     <para />
        ///     Designation: Qualified Repeating:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for State/federal/regulatory agency programs to respond to questions included on a Medicare form that requires a
        ///     numeric as the response. (At least one response is required per question.)
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "NumericResponse", Description = FieldDescriptions.NumericResponse, DataType = "Decimal", IsRequired = false)]
        public decimal? NumericResponse { get; set; }

        /// <summary>
        ///     Field Number: 379-4D
        ///     <para />
        ///     Description: Percent response to a question (part of the question information)
        ///     <para />
        ///     Format: 9(3)v99
        ///     <para />
        ///     Designation: Qualified Repeating:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for state/federal/regulatory agency programs to respond to questions included on a Medicare
        ///     form that requires a percent as the response. (At least one response is required per question.)
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "PercentResponse", Description = FieldDescriptions.PercentResponse, DataType = "String", IsRequired = false)]
        public string PercentResponse { get; set; }

        /// <summary>
        ///     Field Number: 378-4B
        ///     <para />
        ///     Description: Identifies the question number/letter that the question response applies to (part of the question
        ///     information).
        ///     <para />
        ///     Format: X(03)
        ///     <para />
        ///     Designation: Qualified Repeating:
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary for state/federal/regulatory agency programs to respond to questions included on a Medicare
        ///     form.
        ///     <para />
        ///     Required if Question Number/Letter Count (377-2Z) isgreater than Ø.
        ///     <para />
        ///     PRX: n/a
        /// </summary>
        [ApiMember(Name = "QuestionNumber", Description = FieldDescriptions.QuestionNumber, DataType = "String", IsRequired = false)]
        public string QuestionNumber { get; set; }
    }
}
