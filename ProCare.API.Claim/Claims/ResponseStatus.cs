using System.Collections.Generic;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class ResponseStatus
    {
        public ResponseStatus()
        {
            Rejects = new List<Reject>();
            ApprovedMessageCodes = new List<string>();
            AdditionalMessages = new List<AdditionalMessage>();
        }

        /// <summary>
        ///     Field Number: 130-UF
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 25.
        ///     <para />
        ///     Required if Additional Message Information(526-FQ) is used.
        ///     <para />
        ///     Used to qualify the number of occurrences of the Additional Message Information(526-FQ) that is included in the Response Status Segment.
        /// </summary>
        [ApiMember(Name = "AdditionalMessageInformationCount", Description = FieldDescriptions.AdditionalMessageInformationCount, DataType = "int", IsRequired = false)]
        public int AdditionalMessageInformationCount => AdditionalMessages.Count;

        public List<AdditionalMessage> AdditionalMessages { get; set; }

        /// <summary>
        ///     Field Number: 547-5F
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 5.
        ///     <para />
        ///     Required if Approved Message Code(548-6F) is used.
        /// </summary>
        [ApiMember(Name = "ApprovedMessageCodeCount", Description = FieldDescriptions.ApprovedMessageCodeCount, DataType = "int", IsRequired = false)]
        public int ApprovedMessageCodeCount => ApprovedMessageCodes.Count;

        /// <summary>
        ///     Field Number: 548-6F
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Approved Message Code Count(547-5F) is used and the sender needs to communicate additional 
        ///     follow up for a potential opportunity.
        /// </summary>
        [ApiMember(Name = "ApprovedMessageCodes", Description = FieldDescriptions.ApprovedMessageCodes, DataType = "string", IsRequired = false)]
        public List<string> ApprovedMessageCodes { get; set; }

        /// <summary>
        ///     Field Number: 503-F3
        ///     <para />
        ///     Description: Number assigned by the processor to identify an authorized transaction.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to identify the transaction.
        /// </summary>
        [ApiMember(Name = "AuthorizationNumber", Description = FieldDescriptions.AuthorizationNumber, DataType = "string", IsRequired = false)]
        public string AuthorizationNumber { get; set; }

        /// <summary>
        ///     Field Number: 550-7F
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Help Desk Phone Number(55Ø-8F) is used.
        /// </summary>
        [ApiMember(Name = "HelpDeskNumberQualifier", Description = FieldDescriptions.HelpDeskNumberQualifier, DataType = "string", IsRequired = false)]
        public string HelpDeskNumberQualifier { get; set; }

        /// <summary>
        ///     Field Number: 550-8F
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if needed to provide a support telephone number to the receiver
        /// </summary>
        [ApiMember(Name = "HelpDeskPhoneNumber", Description = FieldDescriptions.HelpDeskPhoneNumber, DataType = "string", IsRequired = false)]
        public string HelpDeskPhoneNumber { get; set; }

        /// <summary>
        ///     Field Number: 993-A7
        ///     <para />
        ///     Description: Number assigned by the processor to identify an adjudicated claim when supplied in payer-to-payer coordination of benefits only.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required when used for payer-to-payer coordination of benefits to track the claim without regard to the “Service
        ///     Provider ID, Prescription Number, & Date of Service”.
        /// </summary>
        [ApiMember(Name = "InternalControlNumber", Description = FieldDescriptions.InternalControlNumber, DataType = "string", IsRequired = false)]
        public string InternalControlNumber { get; set; }

        /// <summary>
        ///     Field Number: 510-FA
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: 
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Not used.
        /// </summary>
        [ApiMember(Name = "RejectCount", Description = FieldDescriptions.RejectCount, DataType = "int", IsRequired = false)]
        public int RejectCount => Rejects.Count;

        public List<Reject> Rejects { get; set; }

        /// <summary>
        ///     Field Number: 880-K5
        ///     <para />
        ///     Description: A reference number assigned by the provider to each of the data records in the batch or real-time transactions. 
        ///     <para />
        ///     The purpose of this number is to facilitate the process of matching the transaction response to the transaction. 
        ///     The transaction reference number assigned should be returned in the response.
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: 
        ///     <para />
        ///     Claim Billing/Encounter: Not used.
        /// </summary>
        [ApiMember(Name = "TransactionReferenceNumber", Description = FieldDescriptions.TransactionReferenceNumber, DataType = "string", IsRequired = false)]
        public string TransactionReferenceNumber { get; set; }

        /// <summary>
        ///     Field Number: 112-AN
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Mandatory
        /// </summary>
        [ApiMember(Name = "TransactionResponseStatus", Description = FieldDescriptions.TransactionResponseStatus, DataType = "string", IsRequired = true)]
        public char TransactionResponseStatus { get; set; } = ' ';

        /// <summary>
        ///     Field Number: 987-MA
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: 
        ///     <para />
        ///     Eligibility Verification:
        ///     Not used.
        /// </summary>
        [ApiMember(Name = "URL", Description = FieldDescriptions.URL, DataType = "string", IsRequired = false)]
        public string URL { get; set; }
        
        private void AddAdditionalMessageIfNeeded(int index)
        {
            while (AdditionalMessages.Count < index)
            {
                AdditionalMessages.Add(new AdditionalMessage());
            }
        }

        private void AddRejectIfNeeded(int index)
        {
            while (Rejects.Count < index)
            {
                Rejects.Add(new Reject());
            }
        }
    }

    public class Reject
    {

        /// <summary>
        ///     Field Number: 546-4F
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Eligibility Verification:
        ///     Required if a repeating field is in error, to identify repeating field occurrence.
        ///     <para />
        ///     This field must be sent when relaying error information about a repeating field or set.Note, if the Reject Code is not
        ///     denoting a repeating field or set, the Reject Field Occurrence Indicator must not be sent.
        /// </summary>
        [ApiMember(Name = "FieldOccurrenceIndicator", Description = FieldDescriptions.FieldOccurrenceIndicator, DataType = "string", IsRequired = false)]
        public string FieldOccurrenceIndicator { get; set; }

        /// <summary>
        ///     Field Number: 511-FB
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Required
        /// </summary>
        [ApiMember(Name = "RejectCode", Description = FieldDescriptions.RejectCode, DataType = "string", IsRequired = true)]
        public string RejectCode { get; set; }
    }

    public class AdditionalMessage
    {
        /// <summary>
        ///     Field Number: 526-FQ
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Eligibility Verification:
        ///     Required if additional text is needed for clarification or detail.
        ///     <para />
        ///     When Transaction Count(1Ø9-A9) is = 1 (single transaction per transmission),
        ///         • The Additional Message Information(526-FQ) may contain an extension of the Message(5Ø4-F4), or
        ///         • The Message(5Ø4-F4) will contain transmission-level text and Additional Message
        ///             Information(526-FQ) will contain transaction-level text.
        ///     <para />
        ///     When Transaction Count (1Ø9-A9) is > 1 (multiple transactions per transmission),
        ///         • The Message(5Ø4-F4) will only contain transmission-level text, and Additional Message 
        ///           Information(526-FQ) will only contain transaction-level text.
        /// </summary>
        [ApiMember(Name = "Information", Description = FieldDescriptions.Information, DataType = "string", IsRequired = false)]
        public string Information { get; set; }

        /// <summary>
        ///     Field Number: 131-UG
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Eligibility Verification:
        ///     Required if and only if current repetition of Additional Message Information(526-FQ) is used, another populated
        ///     repetition of Additional Message Information(526-FQ) follows it, and the text of the following message is a
        ///     continuation of the current.
        /// </summary>
        [ApiMember(Name = "InformationContinuity", Description = FieldDescriptions.InformationContinuity, DataType = "string", IsRequired = false)]
        public string InformationContinuity { get; set; }

        /// <summary>
        ///     Field Number: 132-UH
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Eligibility Verification:
        ///     Required if Additional Message Information(526-FQ) is used.
        /// </summary>
        [ApiMember(Name = "InformationQualifier", Description = FieldDescriptions.InformationQualifier, DataType = "string", IsRequired = false)]
        public string InformationQualifier { get; set; }
    }
}