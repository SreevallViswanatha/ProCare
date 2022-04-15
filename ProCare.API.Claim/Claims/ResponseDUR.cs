using System;
using System.Collections.Generic;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class ResponseDUR
    {
        public ResponseDUR()
        {
            DURFields = new List<DURFields>();
        }
        /// <summary>
        ///     Field Number: 567-J6
        ///     <para />
        ///     Description: Counter number for each DUR/PPS set/logical grouping.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill:
        ///     Maximum 9 occurrences supported.
        ///     Required if Reason For Service Code (439-E4) is used.
        /// </summary>
        [ApiMember(Name = "DURCodeCounter", Description = FieldDescriptions.DURCodeCounter, DataType = "Int",IsRequired = false)]
        public int DURCodeCounter => DURFields.Count;

        public List<DURFields> DURFields { get; set; }
        
        private void AddDURFieldsIfNeeded(int index)
        {
            while (DURFields.Count < index)
            {
                DURFields.Add(new DURFields());
            }
        }
 
    }

    public class DURFields
    {

        /// <summary>
        ///     Field Number: 570-NS
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill: 
        ///     Required if needed to supply additional information for the utilization conflict.
        /// </summary>
        [ApiMember(Name = "AdditionalText", Description = FieldDescriptions.AdditionalText, DataType = "string", IsRequired = false)]
        public string AdditionalText { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 528-FS
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill:
        ///     Required if needed to supply additional information for the utilization conflict.
        /// </summary>
        [ApiMember(Name = "ClinicalSignificanceCode", Description = FieldDescriptions.ClinicalSignificanceCode, DataType = "string", IsRequired = false)]
        public string ClinicalSignificanceCode { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 532-FW
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill:
        ///     Required if needed to supply additional information for the utilization conflict.
        /// </summary>
        [ApiMember(Name = "DatabaseIndicator", Description = FieldDescriptions.DatabaseIndicator, DataType = "string", IsRequired = false)]
        public string DatabaseIndicator { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 544-FY
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill:
        ///     Required if needed to supply additional information for the utilization conflict.
        /// </summary>
        [ApiMember(Name = "FreeTextMessage", Description = FieldDescriptions.FreeTextMessage, DataType = "string", IsRequired = false)]
        public string FreeTextMessage { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 529-FT
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill: 
        ///     Required if needed to supply additional information for the utilization conflict.
        /// </summary>
        [ApiMember(Name = "OtherPharmacyIndicator", Description = FieldDescriptions.OtherPharmacyIndicator, DataType = "string", IsRequired = false)]
        public string OtherPharmacyIndicator { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 533-FX
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualfied Required
        ///     <para />
        ///     Claim Rebill:
        ///     Required if needed to supply additional information for the utilization conflict.
        /// </summary>
        [ApiMember(Name = "OtherPrescriberIndicator", Description = FieldDescriptions.OtherPrescriberIndicator, DataType = "string", IsRequired = false)]
        public string OtherPrescriberIndicator { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 530-FU
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill:
        ///     Required if needed to supply additional information for the utilization conflict.
        ///     <para />
        ///     Required if Quantity of Previous Fill(531-FV) is used.
        /// </summary>
        [ApiMember(Name = "PreviousDateOfFill", Description = FieldDescriptions.PreviousDateOfFill, DataType = "string", Format = "Date", IsRequired = false)]
        public DateTime? PreviousDateOfFill { get; set; }

        /// <summary>
        ///     Field Number: 531-FV
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format: 
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill:
        ///     Required if needed to supply additional information for the utilization conflict.
        ///     <para />
        ///     Required if Previous Date Of Fill(53Ø-FU) is used
        /// </summary>
        [ApiMember(Name = "QuantityOfPreviousFill", Description = FieldDescriptions.QuantityOfPreviousFill, DataType = "Decimal", IsRequired = false)]
        public decimal? QuantityOfPreviousFill { get; set; }

        /// <summary>
        ///     Field Number: 439-E4
        ///     <para />
        ///     Description: Code identifying the type of utilization conflict detected or the reason for the pharmacist's professional service.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Rebill:
        ///     Required if utilization conflict is detected.
        /// </summary>
        [ApiMember(Name = "ReasonForServiceCode", Description = FieldDescriptions.DURReasonForServiceCode, DataType = "string", IsRequired = false)]
        public string ReasonForServiceCode { get; set; } = string.Empty;
    }
}