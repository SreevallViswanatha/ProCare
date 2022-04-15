using  ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestPharmacy
    {
        /// <summary>
        ///     Field Number: 444-E9
        ///     <para />
        ///     Description: ID assigned to the person responsible for the dispensing of the prescription.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     Required if necessary to identify the individual responsible for dispensing of the prescription.
        ///     <para />
        ///     Required if needed for reconciliation of encounter-reported data or encounter reporting.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "ProviderID", Description = FieldDescriptions.ProviderId, DataType = "string", IsRequired = false)]
        public string ProviderID { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 465-EY
        ///     <para />
        ///     Description: Code qualifying the Provider ID
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter: 
        ///     Required if Provider ID(444-E9) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "ProviderIDQualifier", Description = FieldDescriptions.ProviderIdQualifier, DataType = "string", IsRequired = false)]
        public string ProviderIDQualifier { get; set; } = string.Empty;

    }
}
