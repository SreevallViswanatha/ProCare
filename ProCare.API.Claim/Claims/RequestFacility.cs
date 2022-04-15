using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestFacility
    {
        /// <summary>
        ///     Field Number: 388-5J
        ///     <para />
        ///     Description: Free form text for facility city Name.
        ///     <para />
        ///     Format: X(20)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage, pricing, patient financial responsibility, and/or drug
        ///     utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "City", Description = FieldDescriptions.FacilityCity, DataType = "String", IsRequired = false)]
        public string City { get; set; }

        /// <summary>
        ///     Field Number: 336-8C
        ///     <para />
        ///     Description: ID assigned to the patient’s clinic/host party.
        ///     <para />
        ///     Format: X(10)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage, pricing, patient financial responsibility, and/or drug
        ///     utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "FacilityID", Description = FieldDescriptions.FacilityId, DataType = "String", IsRequired = false)]
        public string FacilityID { get; set; }

        /// <summary>
        ///     Field Number: 385-3Q
        ///     <para />
        ///     Description: Name identifying the location of the service rendered.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage, pricing, patient financial responsibility, and/or drug
        ///     utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "FacilityName", Description = FieldDescriptions.FacilityName, DataType = "Int", IsRequired = false)]
        public string FacilityName { get; set; }

        /// <summary>
        ///     Field Number: 387-3V
        ///     <para />
        ///     Description: Standard state/province code as defined by appropriate government agency.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage, pricing, patient financial responsibility, and/or drug
        ///     utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "State", Description = FieldDescriptions.FacilityState, DataType = "Int", IsRequired = false)]
        public string State { get; set; }

        /// <summary>
        ///     Field Number: 386-3U
        ///     <para />
        ///     Description: Free form text for Facility address information.
        ///     <para />
        ///     Format: X(30)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage, pricing, patient financial responsibility, and/or drug
        ///     utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "Street", Description = FieldDescriptions.FacilityStreet, DataType = "Int", IsRequired = false)]
        public string Street { get; set; }

        /// <summary>
        ///     Field Number: 389-6D
        ///     <para />
        ///     Description: Code defining international postal zone excluding punctuation and blanks (zip code for US).
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if this field could result in different coverage,pricing, patient financial responsibility, and/or drug
        ///     <para />
        /// </summary>
        [ApiMember(Name = "Zip", Description = FieldDescriptions.FacilityZip, DataType = "Int", IsRequired = false)]
        public string Zip { get; set; }

    }
}
