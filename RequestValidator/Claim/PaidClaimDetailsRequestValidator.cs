using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class PaidClaimDetailsRequestValidator : AbstractValidator<PaidClaimDetailsRequest>
    {
        public PaidClaimDetailsRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.ClaimNumber).NotEmpty().Length(14);
        }
    }
}
