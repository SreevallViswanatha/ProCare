using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class RequestClinicalValidator : AbstractValidator<RequestClinical>
    {
        public RequestClinicalValidator()
        {
            RuleSet("CareMarkClinical", () =>
            {
                RuleFor(x => x.DiagnosisCodeCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Diagnosis Code Count");

                RuleFor(x => x.DiagnosisCodes)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Diagnosis Codes");
            });
            
            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.DiagnosisCodeCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Diagnosis Code Count");
           
                RuleFor(x => x.DiagnosisCodes)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Diagnosis Codes");
            });

            RuleSet("ExpressScripts", () =>
            {
                //RuleFor(x => x.DiagnosisCodeCount)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Diagnosis Code Count");

                //RuleFor(x => x.DiagnosisCodes)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Diagnosis Codes");
            });
        }
    }
}