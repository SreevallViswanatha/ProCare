using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class RequestDURValidator : AbstractValidator<RequestDrugUtilizationReview>
    {
        public RequestDURValidator()
        {
            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.CodeCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Code Count");
            });

            RuleSet("CareMarkDUR", () =>
            {
                RuleFor(x => x.CodeCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Code Count");
            });
            RuleSet("Envision", () =>
            {
                RuleFor(x => x.CodeCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Code Count");
            });
            RuleSet("ExpressScripts", () =>
            {
                //RuleFor(x => x.CodeCount)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Code Count");
            });
        }
    }
}
