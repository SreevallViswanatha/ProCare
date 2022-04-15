using System.Collections.Generic;
using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class RequestCOBValidator : AbstractValidator<RequestCoordinationOfBenefits>
    {
        public RequestCOBValidator()
        {
            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.OtherPaymentsCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing OtherPaymentsCount");
            });
            RuleSet("Envision", () =>
            {
                RuleFor(x => x.OtherPaymentsCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing OtherPaymentsCount");
            });
            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.OtherPaymentsCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing OtherPaymentsCount");
            });
            RuleSet("MRX", () =>
            {
                RuleFor(x => x.OtherPaymentsCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing OtherPaymentsCount");
            });
            RuleSet("ExpressScripts", () =>
            {
                //RuleFor(x => x.OtherPaymentsCount)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing OtherPaymentsCount");
            });
            RuleSet("Prime", () =>
            {
                RuleFor(x => x.OtherPaymentsCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing OtherPaymentsCount");
            });

            RuleFor(x => x.OtherPayers).SetValidator(new OtherPayerValidator());
            
        }
    }
    public class OtherPayerValidator : AbstractValidator<List<OtherPayer>>
    {
        public OtherPayerValidator()
        {
            RuleFor(x => x).SetCollectionValidator(new COBOtherPayerValidator());
        }
    }
}
