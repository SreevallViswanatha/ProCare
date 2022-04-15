using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class ResponseMessage
    {

        /// <summary>
        ///     Field Number: 504-F4
        ///     <para />
        ///     Description: Free form message.
        ///     <para />
        ///     Format: X(35)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Service Rebill:
        ///     Required if text is needed for clarification or detail.
        ///     <para />
        ///     When Transaction Count(1Ø9-A9) is = 1 (single transaction per transmission),
        ///         • The Additional Message Information(526-FQ) may contain an extension of the Message(5Ø4-F4), or
        ///         • The Message(5Ø4-F4) will contain transmission-level text and Additional Message Information(526-FQ) will contain transaction level text.
        ///     <para />
        ///     When Transaction Count (1Ø9-A9) is > 1 (multiple transactions per transmission),
        ///         • The Message(5Ø4-F4) will only contain transmission-level text, and Additional Message Information(526-FQ) will only contain transaction-level text.
        /// </summary>
        [ApiMember(Name = "Message", Description = FieldDescriptions.Message, DataType = "string", IsRequired = false)]
        public string Message { get; set; }

  
    }
}