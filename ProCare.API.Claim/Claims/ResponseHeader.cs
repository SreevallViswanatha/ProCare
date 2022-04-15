using System;
using ServiceStack;
using ProCare.NCPDP.Telecom;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class ResponseHeader
    {
        private const int VersionNumberLength = 2;
        private const int TransactionCodeLength = 2;
        private const int TransactionCountLength = 1;
        private const int ServiceProviderIDQualifierLength = 2;
        private const int ServiceProviderIDLength = 15;
        private const int DateOfServiceLength = 8;
        private const int HeaderResponseStatusLength = 1;

        public ResponseHeader()
        {
            DateofService = DateTime.MinValue;
            TransactionCount = 1;
        }

        /// <summary>
        ///     Field Number: 401-D1
        ///     <para />
        ///     Description: Identifies date the prescription was filled or professional service rendered or subsequent payer began coverage following Part A expiration in a long-term care setting only.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Mandatory
        /// </summary>
        [ApiMember(Name = "DateofService", Description = FieldDescriptions.FillDt, DataType = "string",Format = "Date", IsRequired = true)]
        public DateTime DateofService { get; set; }
    
        /// <summary>
        ///     Field Number: 501-F1
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Mandatory
        /// </summary>
        [ApiMember(Name = "HeaderResponseStatus", Description = FieldDescriptions.HeaderResponseStatus, DataType = "char", IsRequired = true)]
        public char HeaderResponseStatus { get; set; } = ' ';

        /// <summary>
        ///     Field Number: 201-B1
        ///     <para />
        ///     Description: ID assigned to pharmacy or provider.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Mandatory
        /// </summary>
        [ApiMember(Name = "ServiceProviderID", Description = FieldDescriptions.ServiceProviderId, DataType = "string", IsRequired = true)]
        public string ServiceProviderID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 202-B2
        ///     <para />
        ///     Description: Code qualifying the Service Provider ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        /// </summary>
        [ApiMember(Name = "ServiceProviderIDQualifier", Description = FieldDescriptions.ServiceProviderIdQualifier, DataType = "string", IsRequired = true)]
        public string ServiceProviderIDQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 103-A3
        ///     <para />
        ///     Description: Identifies type of transaction
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     For Transaction Code of “S3”, in the Response Claim Segment, the Prescription/Service Reference Number
        ///     Qualifier(455-EM) is “2” (Service Billing).
        /// </summary>
        [ApiMember(Name = "TransactionCode", Description = FieldDescriptions.TransactionCode, DataType = "string", IsRequired = true)]
        public TransactionCode TransactionCode { get; set; } = TransactionCode.Unknown;

        /// <summary>
        ///     Field Number: 109-A9
        ///     <para />
        ///     Description: Number of transactions in the transmission.
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Mandatory
        /// </summary>
        [ApiMember(Name = "TransactionCount", Description = FieldDescriptions.TransactionCount, DataType = "int", IsRequired = true)]
        public int TransactionCount { get; set; }

        /// <summary>
        ///     Field Number: 102-A2
        ///     <para />
        ///     Description: Code identifying the release syntax and corresponding Data Dictionary.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        /// </summary>
        [ApiMember(Name = "VersionNumber", Description = FieldDescriptions.VersionNumber, DataType = "string", IsRequired = true)]
        public string VersionNumber { get; set; } = string.Empty;
    }
}