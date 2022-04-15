using FluentValidation;
using ProCare.NCPDP.Telecom.Request;

namespace ProCare.API.Claims.Validators
{
    public class RequestReversalClaimValidator : AbstractValidator<RequestClaim>
    {
        public RequestReversalClaimValidator()
        {
            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.PrescriptionQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Qualifier");

                RuleFor(x => x.PrescriptionNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrescriptionNumber");
                RuleFor(x => x.ProductIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductIDQualifier");
                RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductID");
                RuleFor(x => x.FillNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing FillNumber");
                RuleFor(x => x.OtherCoverageCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other Coverage Code");
            });

            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.PrescriptionQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Qualifier");

                RuleFor(x => x.PrescriptionNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrescriptionNumber");
                RuleFor(x => x.ProductIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductIDQualifier");
                RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductID");
                RuleFor(x => x.FillNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing FillNumber");
            });

            RuleSet("CareMark", () =>
            {
                RuleFor(x => x.PrescriptionQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Qualifier");

                RuleFor(x => x.PrescriptionNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrescriptionNumber");

                RuleFor(x => x.ProductIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductIDQualifier");
                RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductID");
                RuleFor(x => x.FillNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing FillNumber");
            });

            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.PrescriptionQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Qualifier");

                RuleFor(x => x.PrescriptionNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrescriptionNumber");

                RuleFor(x => x.ProductIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductIDQualifier");
                RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductID");

            });

            RuleSet("MRX", () =>
            {
                RuleFor(x => x.PrescriptionQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Qualifier");

                RuleFor(x => x.PrescriptionNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrescriptionNumber");

                RuleFor(x => x.ProductIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductIDQualifier");
                RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductID");
                RuleFor(x => x.FillNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing FillNumber");
            });

            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.PrescriptionQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Qualifier");

                RuleFor(x => x.PrescriptionNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrescriptionNumber");

                RuleFor(x => x.ProductIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductIDQualifier");
            
                RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProductID");
 
                RuleFor(x => x.FillNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing FillNumber");
            });
        }
    }
}
