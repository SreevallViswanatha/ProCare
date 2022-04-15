using System;
using System.Collections.Generic;
using ProCare.API.Core;
using ServiceStack;

namespace ProCare.API.Claims.Claims
{
    public class ResponseCOB
    {
        public ResponseCOB()
        {
            OtherPayers = new List<OtherPayers>();
        }

        /// <summary>
        ///     Field Number: 355-NT
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 3.
        /// </summary>
        [ApiMember(Name = "OtherPayerIDCount", Description = FieldDescriptions.OtherPayerIdCount, DataType = "Int", IsRequired = true)]
        public int OtherPayerIDCount => OtherPayers.Count;

        public List<OtherPayers> OtherPayers { get; set; }
        
        private void AddOtherPayerIfNeeded(int index)
        {
            while (OtherPayers.Count < index)
            {
                OtherPayers.Add(new OtherPayers());
            }
        }
 
    }

    public class OtherPayers
    {
        /// <summary>
        ///     Field Number: 144-UX
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required when other coverage is known which is after the Date of Service submitted.
        /// </summary>
        [ApiMember(Name = "BenefitEffectiveDate", Description = FieldDescriptions.BenefitEffectiveDate, DataType = "string",Format="Date", IsRequired = false)]
        public DateTime? BenefitEffectiveDate { get; set; }

        /// <summary>
        ///     Field Number: 145-UY
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required when other coverage is known which is after the Date of Service submitted.
        /// </summary>
        [ApiMember(Name = "BenefitTerminationDate", Description = FieldDescriptions.BenefitTerminationDate, DataType = "string",Format="Date", IsRequired = false)]
        public DateTime? BenefitTerminationDate { get; set; }

        /// <summary>
        ///     Field Number: 356-NU
        ///     <para />
        ///     Description: Cardholder ID for this member that is associated with the Payer noted.
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required if other insurance information is available for coordination of benefits.
        /// </summary>
        [ApiMember(Name = "CardholderID", Description = FieldDescriptions.OtherCardholderId, DataType = "string", IsRequired = false)]
        public string CardholderID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 338-5C
        ///     <para />
        ///     Description: Code identifying the type of Other Payer ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "CoverageType", Description = FieldDescriptions.CoverageType, DataType = "string", IsRequired = true)]
        public string CoverageType { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 992-MJ
        ///     <para />
        ///     Description: ID assigned to the cardholder group or employer group by the secondary, tertiary, etc. payer.
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required if other insurance information is available for coordination of benefits.
        /// </summary>
        [ApiMember(Name = "GroupID", Description = FieldDescriptions.OtherGroupId, DataType = "string", IsRequired = false)]
        public string GroupID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 127-UB
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required if needed to provide a support telephone number of the other payer to the receiver
        /// </summary>
        [ApiMember(Name = "HelpDeskNumber", Description = FieldDescriptions.HelpDeskNumber, DataType = "string", IsRequired = false)]
        public string HelpDeskNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 339-6C
        ///     <para />
        ///     Description: Code qualifying the Other Payer ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required if Other Payer ID(34Ø-7C) is used.
        /// </summary>
        [ApiMember(Name = "IDQualifier", Description = FieldDescriptions.IdQualifier, DataType = "string", IsRequired = false)]
        public string IDQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 340-7C
        ///     <para />
        ///     Description: ID assigned to the payer.
        ///     <para />
        ///     Format: X(10)
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required if other insurance information is available for coordination of benefits.
        /// </summary>
        [ApiMember(Name = "OtherPayerID", Description = FieldDescriptions.OtherPayerID, DataType = "string", IsRequired = false)]
        public string OtherPayerID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 143-UW
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required if needed to uniquely identify the relationship of
        ///     the patient to the cardholder ID, as assigned by the other payer.
        /// </summary>
        [ApiMember(Name = "PatientRelationshipCode", Description = FieldDescriptions.PatientRelationshipCode, DataType = "string", IsRequired = false)]
        public string PatientRelationshipCode { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 142-UV
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required if needed to uniquely identify the family members within the Cardholder ID, as assigned by the other payer.
        /// </summary>
        [ApiMember(Name = "PersonCode", Description = FieldDescriptions.COBPersonCode, DataType = "string", IsRequired = false)]
        public string PersonCode { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 991-MH
        ///     <para />
        ///     Description: 
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Service Billing:
        ///     Required if other insurance information is available for coordination of benefits.
        /// </summary>
        [ApiMember(Name = "ProcessorControlNumber", Description = FieldDescriptions.ProcessorControlNumber, DataType = "string", IsRequired = false)]
        public string ProcessorControlNumber { get; set; } = string.Empty;
    }
}