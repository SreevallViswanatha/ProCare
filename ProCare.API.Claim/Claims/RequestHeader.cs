using System;
using ProCare.NCPDP.Telecom;
using ServiceStack;
using ProCare.API.Core;
using System.Collections.Generic;

namespace ProCare.API.Claims.Claims
{
    public class RequestHeader
    {
        /// <summary>
        ///     Field Number: 101-A1
        ///     <para />
        ///     Description: Card Issuer or Bank ID used for network routing
        ///     <para />
        ///     Format: 9(06)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     ProCare Field: ANSI_BIN
        /// </summary>
        [ApiMember(Name = "BinNumber", Description = FieldDescriptions.AnsiBinNumber, DataType = "string", IsRequired = true)]
        public string BinNumber { get; set; }

        /// <summary>
        ///     Field Number: 102-A2
        ///     <para />
        ///     Description: Code identifying the release syntax and corresponding Data Dictionary
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     ProCare Field: VERSIONNO
        /// </summary>
        [ApiMember(Name = "ClaimVersion", Description = FieldDescriptions.ClaimVersion , DataType = "string", IsRequired = true)]
        public VersionNumber ClaimVersion { get; set; }

        /// <summary>
        ///     Field Number: 401-D1
        ///     <para />
        ///     Description: Identifies date the prescription was filled or professional service rendered or subsequent payer
        ///     began coverage following Part A expiration in a long-term care setting only.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     PRX Field: FillDT
        /// </summary>
        [ApiMember(Name = "DateOfService", Description = FieldDescriptions.FillDt, DataType = "string", Format = "date", IsRequired = true)]
        public DateTime DateOfService { get; set; }

        /// <summary>
        ///     Field Number: 104-A4
        ///     <para />
        ///     Description: Number assigned by processor.
        ///     <para />
        ///     Format: X(10)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     ProCare Field:  PROCESSNO
        /// </summary>
        [ApiMember(Name = "ProcessorControlNumber", Description = FieldDescriptions.ProcessNo, DataType = "string", IsRequired = true)]
        public string ProcessorControlNumber { get; set; }

        /// <summary>
        ///     Field Number: 201-B1
        ///     <para />
        ///     Description: ID assigned to pharmacy or provider
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     ProCare Field:  PHANPI/PHAID
        /// </summary>
        [ApiMember(Name = "ServiceProviderId", Description = FieldDescriptions.ServiceProviderId, DataType = "string", IsRequired = true)]
        public string ServiceProviderId { get; set; }

        /// <summary>
        ///     Field Number: 202-B2
        ///     <para />
        ///     Description: Code qualifying the Service Provider ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     ProCare Field: PHAQUAL
        /// </summary>
        [ApiMember(Name = "ServiceProviderIdQualifier", Description = FieldDescriptions.ServiceProviderIdQualifier, DataType = "string",
            IsRequired = true)]
        public ServiceProviderIdQualifier ServiceProviderIdQualifier { get; set; }

        /// <summary>
        ///     Field Number: 110-AK
        ///     <para />
        ///     Description: ID assigned by the switch or processor to identify the software source.
        ///     <para />
        ///     Format: X(10)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     ProCare Field: n/a
        /// </summary>
        [ApiMember(Name = "SoftwareId", Description = FieldDescriptions.SoftwareId, DataType = "string", IsRequired = true)]
        public string SoftwareId {get;set;}

        /// <summary>
        ///     Field Number: 103-A3
        ///     <para />
        ///     Description: Identifies type of transaction
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     ProCare Field: TRANSCODE
        /// </summary>
        [ApiMember(Name = "TransactionCode", Description = FieldDescriptions.TransactionCode, DataType = "string", IsRequired = true)]
        public TransactionCode TransactionCode { get; set; }

        /// <summary>
        ///     Field Number: 109-A9
        ///     <para />
        ///     Description: Number of transactions in the transmission
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     Max: 4
        ///     <para />
        ///     ProCare Field: TRANSCOUNT
        /// </summary>
        [ApiMember(Name = "TransactionCount", Description = FieldDescriptions.TransactionCount, DataType = "Int", IsRequired = true)]
        public int TransactionCount { get; set; }
    }
}