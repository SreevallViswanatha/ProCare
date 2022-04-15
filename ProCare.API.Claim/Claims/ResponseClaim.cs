using System;
using System.Collections.Generic;
using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class ResponseClaim
    {
        public ResponseClaim()
        {
            PreferredProducts = new List<PreferredProduct>();
        }

        /// <summary>
        ///     Field Number: 114-N4
        ///     <para />
        ///     Description: Claim number assigned by the Medicaid Agency.
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     Medicaid Subrogation - Claim Billing/Encounter:
        ///     Required to report back on the response the claim number assigned by the Medicaid Agency.
        /// </summary>
        [ApiMember(Name = "MedicaidInternalControlNumber", Description = FieldDescriptions.MedicaidInternalControlNumber, DataType = "string", IsRequired = true)]
        public string MedicaidInternalControlNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 551-9F
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Maximum count of 6.
        ///     <para />
        ///     Required if Preferred Product ID(553-AR) is used.
        /// </summary>
        [ApiMember(Name = "PreferredProductCount", Description = FieldDescriptions.PreferredProductCount, DataType = "string", IsRequired = false)]
        public int PreferredProductCount => PreferredProducts.Count;

        public List<PreferredProduct> PreferredProducts { get; set; }

        /// <summary>
        ///     Field Number: 402-D2
        ///     <para />
        ///     Description: Reference number assigned by the provider for the dispensed drug/product and/or service provided.
        ///     <para />
        ///     Format: 9(12)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     Significant digits on submission must be returned on response.
        ///     See section “Standard Conventions”, “Character Set Designation Truncation”, 
        ///     “Numeric”, “Numeric Truncation”.
        /// </summary>
        [ApiMember(Name = "ReferenceNumber", Description = FieldDescriptions.ReferenceNumber, DataType = "string", IsRequired = true)]
        public string ReferenceNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 455-EM
        ///     <para />
        ///     Description: Indicates the type of billing submitted.
        ///     <para />
        ///     Format: X(01)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     For Transaction Code of “B1”, in the Response Claim
        ///     Segment, the Prescription/Service Reference Number Qualifier(455-EM) is “1” (Rx Billing).
        /// </summary>
        [ApiMember(Name = "ReferenceNumberQualifier", Description = FieldDescriptions.ReferenceNumberQualifier, DataType = "string", IsRequired = true)]
        public string ReferenceNumberQualifier { get; set; } = string.Empty;
        

        private void AddPreferredProductIfNeeded(int index)
        {
            while (PreferredProducts.Count < index)
            {
                PreferredProducts.Add(new PreferredProduct());
            }
        }
        
    }

    public class PreferredProduct
    {
        /// <summary>
        ///     Field Number: 555-AT
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if there is a known patient financial responsibility
        ///     incentive amount associated with the Preferred Product ID (553-AR) and/or 
        ///     Preferred Product Description(556-AU). Not used in payer-to-payer transactions.
        /// </summary>
        [ApiMember(Name = "CostShareIncentive", Description = FieldDescriptions.CostShareIncentive, DataType = "string", IsRequired = false)]
        public decimal? CostShareIncentive { get; set; }

        /// <summary>
        ///     Field Number: 554-AS
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if there is a known incentive amount associated with the Preferred Product ID(553-AR) and/or Preferred
        ///     Product Description(556-AU).
        ///     <para />
        ///     Not used in payer-to-payer transactions.
        /// </summary>
        [ApiMember(Name = "Incentive", Description = FieldDescriptions.Incentive, DataType = "string", IsRequired = false)]
        public decimal? Incentive { get; set; }

        /// <summary>
        ///     Field Number: 556-AU
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if a product preference exists that either cannot be communicated by the Preferred Product ID(553-AR) or
        ///     to clarify the Preferred Product ID(553-AR).
        ///     <para />
        ///     Not used in payer-to-payer transactions.
        /// </summary>
        [ApiMember(Name = "ProductDescription", Description = FieldDescriptions.ProductDescription, DataType = "string", IsRequired = true)]
        public string ProductDescription { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 553-AR
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if a product preference exists that needs to be communicated to the receiver via an ID.
        ///     <para />
        ///     Not used in payer-to-payer transactions.
        /// </summary>
        [ApiMember(Name = "ProductID", Description = FieldDescriptions.PreferredProductId, DataType = "string", IsRequired = false)]
        public string ProductID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 552-AP
        ///     <para />
        ///     Description: ??
        ///     <para />
        ///     Format: ??
        ///     <para />
        ///     Designation: Qualified Required
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Preferred Product ID(553-AR) is used.
        ///     <para />
        ///     Not used in payer-to-payer transactions.
        /// </summary>
        [ApiMember(Name = "ProductIDQualifier", Description = FieldDescriptions.PreferredProductIdQualifier, DataType = "string", IsRequired = true)]
        public string ProductIDQualifier { get; set; } = string.Empty;
    }
}