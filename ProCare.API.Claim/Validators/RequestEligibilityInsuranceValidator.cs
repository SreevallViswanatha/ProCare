using FluentValidation;
using ProCare.NCPDP.Telecom.Request;

namespace ProCare.API.Claims.Validators
{
    public class RequestEligibilityInsuranceValidator : AbstractValidator<RequestInsurance>
    {
        public RequestEligibilityInsuranceValidator()
        {
            //RuleFor(x => x.CardholderId)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing CardholderID");
        }
    }
}
