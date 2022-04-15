using System;
using System.Collections.Generic;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class ResponseInsurance
    {

        /// <summary>
        ///     Field Number: 302-C2
        ///     <para />
        ///     Description: Insurance ID assigned to the cardholder.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if the identification to be used in future transactions is different than what was submitted on the request.
        /// </summary>
        [ApiMember(Name = "CardholderID", Description = FieldDescriptions.CardholderId, DataType = "string", IsRequired = false)]
        public string CardholderID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 301-C1
        ///     <para />
        ///     Description: ID assigned to the cardholders or employers group.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if needed to identify the actual cardholder or employer group, to identify appropriate group number, when available.
        ///     <para />
        ///     Required to identify the actual group that was used when multiple group coverages exist.
        ///     <para />
        ///     Note: This field may contain the Group ID echoed from the request.May contain the actual Group ID if unknown to the receiver.
        /// </summary>
        [ApiMember(Name = "GroupID", Description = FieldDescriptions.GroupId, DataType = "string", IsRequired = false)]
        public string GroupID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 116-N6
        ///     <para />
        ///     Description: Number assigned by processor to identify the individual Medicaid Agency or representative.
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: 
        /// </summary>
        [ApiMember(Name = "MedicaidAgencyNumber", Description = FieldDescriptions.MedicaidAgencyNumber, DataType = "string", IsRequired = false)]
        public string MedicaidAgencyNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 115-N5
        ///     <para />
        ///     Description: A unique member identification number assigned by the Medicaid Agency.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: 
        /// </summary>
        [ApiMember(Name = "MedicaidIDNumber", Description = FieldDescriptions.MedicaidIdNumber, DataType = "string", IsRequired = false)]
        public string MedicaidIDNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 545-2F
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if needed to identify the network for the covered member.
        ///     <para />
        ///     Required if needed to identify the actual Network Reimbursement ID, when applicable and/or available.
        ///     <para />
        ///     Required to identify the actual Network Reimbursement ID that was used when multiple Network Reimbursement IDs exist
        /// </summary>
        [ApiMember(Name = "NetworkReimbursementID", Description = FieldDescriptions.NetworkReimbursementId, DataType = "string", IsRequired = false)]
        public string NetworkReimbursementID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 569-J8
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required to identify the ID of the payer responding.
        /// </summary>
        [ApiMember(Name = "PayerID", Description = FieldDescriptions.PayerId, DataType = "string", IsRequired = false)]
        public string PayerID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 568-J7
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if Payer ID(569-J8) is used.
        /// </summary>
        [ApiMember(Name = "PayerIDQualifier", Description = FieldDescriptions.PayerIdQualifier, DataType = "string", IsRequired = false)]
        public string PayerIDQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 524-FO
        ///     <para />
        ///     Description: Assigned by the processor to identify coverage criteria used to adjudicate a claim.
        ///     <para />
        ///     Format: X(08)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if needed to identify the actual plan parameters, benefit, or coverage criteria, when available.
        ///     <para />
        ///     Required to identify the actual plan ID that was used when multiple group coverages exist.
        ///     <para />
        ///     Required if needed to contain the actual plan ID if unknown to the receiver.
        /// </summary>
        [ApiMember(Name = "PlanID", Description = FieldDescriptions.PlanId, DataType = "string", IsRequired = false)]
        public string PlanID { get; set; } = string.Empty;

    }
}