using FluentValidation;
using ProCare.API.Claims.Messages.Request;


namespace ProCare.API.Claims.Validators
{
    public class ClaimEligibilityValidator : AbstractValidator<ClaimEligibilityRequest>
    {
        public ClaimEligibilityValidator()
        {
            RuleFor(transmission => transmission.Header).SetValidator(new RequestHeaderValidator());
            RuleFor(transmission => transmission.Insurance).SetValidator(new RequestEligibilityInsuranceValidator());
        }
    }
}
