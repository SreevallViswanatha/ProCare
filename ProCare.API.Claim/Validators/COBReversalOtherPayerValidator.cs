using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class COBReversalOtherPayerValidator : AbstractValidator<OtherPayer>
    {
        public COBReversalOtherPayerValidator()
        {
            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.OtherPayerCoverageType)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other Payer Coverage Type");
            });

            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.OtherPayerCoverageType)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other Payer Coverage Type");
            });

            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.OtherPayerCoverageType)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other Payer Coverage Type");
            });

        }

    }

    
}
