using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class COBOtherPayerValidator : AbstractValidator<OtherPayer>
    {
        public COBOtherPayerValidator()
        {
            RuleFor(x => x.OtherPayerCoverageType)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Other Payer Coverage Type");

            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.OtherPayerIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other PayerID Qualifier");
            
                RuleFor(x => x.OtherPayerId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other PayerID");
            
                RuleFor(x => x.OtherPayerDate)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other Payer Date");
            });

            RuleSet("Envision", () =>
            {
                RuleFor(x => x.OtherPayerIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other PayerID Qualifier");
           
                RuleFor(x => x.OtherPayerId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other PayerID");
            });

            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.OtherPayerId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other PayerID");
            
                RuleFor(x => x.OtherPayerIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other PayerID Qualifier");
           
                RuleFor(x => x.OtherPayerDate)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other Payer Date");
            });

            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.OtherPayerId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other PayerID");
           
                RuleFor(x => x.OtherPayerDate)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other Payer Date");
            });

        }

    }
}
