using ServiceStack;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestCoupon
    {
        /// <summary>
        ///     Field Number: 486-ME
        ///     <para />
        ///     Description: Unique serial number assigned to the prescription coupons.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "CouponNumber", Description = FieldDescriptions.CouponNumber, DataType = "String", IsRequired = true)]
        public string CouponNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number:  485-KE
        ///     <para />
        ///     Description: Code indicating the type of coupon being used.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "CouponType", Description = FieldDescriptions.CouponType, DataType = "String", IsRequired = true)]
        public string CouponType { get; set; } = string.Empty;

        /// <summary>
        ///     Field Number: 487-NE
        ///     <para />
        ///     Description: Value of the coupon.
        ///     <para />
        ///     Format: s9(6)v99
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Rebill:
        ///     Required if needed for receiver claim/encounter determination when a coupon value is known.
        ///     <para />
        ///     Required if this field could result in different pricing and/or patient financial responsibility.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "CouponValueAmount", Description = FieldDescriptions.CouponValueAmount, DataType = "Decimal", IsRequired = false)]
        public decimal? CouponValueAmount { get; set; }

    }
}
