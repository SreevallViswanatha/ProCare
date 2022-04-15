using ServiceStack;
using System.Collections.Generic;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestDrugUtilizationReview
    {
        /// <summary>
        ///     Field Number:  473-7E
        ///     <para />
        ///     Description: Counter number for each DUR/PPS set/logical grouping.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Rebill:
        ///     Maximum of 9 occurrences.
        ///     <para />
        ///     Required if DUR/PPS Segment is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "CodeCount", Description = FieldDescriptions.CodeCount, DataType = "Int", IsRequired = false)]
        public int CodeCount => Services.Count;

        public List<DrugUtilizationReviewService> Services { get; set; }


    }
    public class DrugUtilizationReviewService
    {
        /// <summary>
        ///     Field Number: 476-H6
        ///     <para />
        ///     Description: Identifies the co-existing agent contributing to the DUR event.
        ///     <para />
        ///     Format: X(19)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Rebill:
        ///     Required if this field could result in different drug utilization review outcome.
        ///     <para />
        ///     Required if this field affects payment for or documentation of professional pharmacy service.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "CoAgentID", Description = FieldDescriptions.CoAgentId, DataType = "String", IsRequired = false)]
        public string CoAgentID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 475-J9
        ///     <para />
        ///     Description: Code qualifying the value in DUR Co-agent ID.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Rebill:
        ///     Required if DUR Co-Agent ID(476-H6) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "CoAgentIDQualifier", Description = FieldDescriptions.CoAgentIdQualifier, DataType = "String", IsRequired = false)]
        public string CoAgentIDQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 474-8E
        ///     <para />
        ///     Description: Code indicating the level of effort as determined by the complexity of decision making or resources
        ///     utilized by a pharmacist to perform a professional service.
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Rebill:
        ///     Required if this field could result in different coverage, pricing, patient financial responsibility, and/or drug
        ///     utilization review outcome.
        ///     <para />
        ///     Required if this field affects payment for or documentation of professional pharmacy service.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "LevelOfEffort", Description = FieldDescriptions.LevelOfEffort, DataType = "String", IsRequired = false)]
        public string LevelOfEffort { get; set; } = string.Empty;

        /// <summary>
        ///     440-E5
        /// </summary>
        /// <summary>
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary>
        [ApiMember(Name = "ProfessionalServiceCode", Description = FieldDescriptions.ProfessionalServiceCode, DataType = "String", IsRequired = true)]
        public string ProfessionalServiceCode { get; set; } = string.Empty;

        /// <summary>
        ///     439-E4
        /// </summary>
        /// <summary>
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary>
        [ApiMember(Name = "ReasonForServiceCode", Description = FieldDescriptions.ReasonForServiceCode, DataType = "String", IsRequired = true)]
        public string ReasonForServiceCode { get; set; } = string.Empty;

        /// <summary>
        ///     441-E6
        /// </summary>
        /// <summary>
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary>
        [ApiMember(Name = "ResultOfServiceCode", Description = FieldDescriptions.ResultOfServiceCode, DataType = "String", IsRequired = true)]
        public string ResultOfServiceCode { get; set; } = string.Empty;
    }
}
