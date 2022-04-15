using System.Collections.Generic;
using FluentValidation;
using ProCare.NCPDP.Telecom.Request;

namespace ProCare.API.Claims.Validators
{
    public class RequestReversalCOBValidator : AbstractValidator<RequestCoordinationOfBenefits>
    {
        public RequestReversalCOBValidator()
        {
            RuleSet("OptumRx", () =>
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
            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.OtherPaymentsCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing OtherPaymentsCount");
            });

            RuleFor(x => x.OtherPayers).SetValidator(new OtherPayerValidator());
        }

        public class OtherPayerValidator : AbstractValidator<List<OtherPayer>>
        {
            public OtherPayerValidator()
            {
                RuleFor(x => x).SetCollectionValidator(new COBReversalOtherPayerValidator());
            }
        }
    }
}
