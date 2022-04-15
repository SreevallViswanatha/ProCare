using FluentValidation;
using ProCare.NCPDP.Telecom.Request;

namespace ProCare.API.Claims.Validators
{
    public class RequestReversalInsuranceValidator : AbstractValidator<RequestInsurance>
    {
        public RequestReversalInsuranceValidator()
        {
            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.CardholderId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing CardholderId");
            });

            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.CardholderId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing CardholderId");
            });

            RuleSet("MRX", () =>
            {
                RuleFor(x => x.CardholderId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing CardholderId");
            });

            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.CardholderId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing CardholderId");
            });


        }
    }
}
