using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class RequestPricingValidator : AbstractValidator<RequestPricing>
    {
        public RequestPricingValidator()
        {
            //RuleFor(x => x.IngredientCostSubmitted)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing Ingredient Cost Submitted");

            //RuleFor(x => x.UsualAndCustomaryCharge)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing Usual and Customary Charge");

            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.DispensingFeeSubmitted)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Fee Submitted");
                RuleFor(x => x.GrossAmountDue)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Gross Amount Due");
                RuleFor(x => x.BasisOfCostDetermination)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Basis of Cost Determination");
            });

            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.DispensingFeeSubmitted)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Fee Submitted");
                RuleFor(x => x.GrossAmountDue)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Gross Amount Due");
                RuleFor(x => x.BasisOfCostDetermination)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Basis of Cost Determination");
            });

            RuleSet("CareMark", () =>
            {
                RuleFor(x => x.DispensingFeeSubmitted)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Fee Submitted");
                RuleFor(x => x.GrossAmountDue)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Gross Amount Due");
                RuleFor(x => x.BasisOfCostDetermination)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Basis of Cost Determination");
            });

            RuleSet("Envision", () =>
            {
                RuleFor(x => x.DispensingFeeSubmitted)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Fee Submitted");
            });

            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.DispensingFeeSubmitted)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Fee Submitted");
                RuleFor(x => x.GrossAmountDue)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Gross Amount Due");
                RuleFor(x => x.BasisOfCostDetermination)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Basis of Cost Determination");
            });

            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.GrossAmountDue)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Gross Amount Due");
            });

            RuleSet("MRX", () =>
            {
                RuleFor(x => x.GrossAmountDue)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Gross Amount Due");
                RuleFor(x => x.BasisOfCostDetermination)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Basis of Cost Determination");
            });
            
            RuleSet("Prime", () =>
            {
                RuleFor(x => x.GrossAmountDue)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Gross Amount Due");
            });

        }
    }
}
