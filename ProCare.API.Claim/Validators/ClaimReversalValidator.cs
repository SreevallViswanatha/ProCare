using FluentValidation;
using ProCare.API.Claims.Messages.Request;


namespace ProCare.API.Claims.Validators
{
    public class ClaimReversalValidator : AbstractValidator<ClaimReversalRequest>
    {
        public ClaimReversalValidator()
        {
            RuleFor(transmission => transmission.Header).SetValidator(new RequestHeaderValidator());
            RuleFor(transmission => transmission.Insurance).SetValidator(new RequestReversalInsuranceValidator());
            RuleFor(transmission => transmission.Claim).SetValidator(new RequestReversalClaimValidator());
            //RuleFor(transmission => transmission.COB).SetValidator(new RequestReversalCOBValidator());
        }
    }
}
