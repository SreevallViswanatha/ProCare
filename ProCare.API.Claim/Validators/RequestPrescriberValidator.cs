using System;
using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class RequestPrescriberValidator : AbstractValidator<RequestPrescriber>
    {
        public RequestPrescriberValidator()
        {
            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.PrescriberIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescriber ID Qualifier");
            
                RuleFor(x => x.PrescriberId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrecsriberID");
            });

            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.PrescriberIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescriber ID Qualifier");
            
                RuleFor(x => x.PrescriberId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrecsriberID");
            
                RuleFor(x => x.State)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Precsriber State");
            });

            RuleSet("CareMark", () =>
            {
                RuleFor(x => x.PrescriberIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescriber ID Qualifier");
           
                RuleFor(x => x.PrescriberId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrecsriberID");
            });

            RuleSet("Envision", () =>
            {
                RuleFor(x => x.PrescriberIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescriber ID Qualifier");
            
                RuleFor(x => x.PrescriberId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrecsriberID");
            });

            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.PrescriberIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescriber ID Qualifier");
            
                RuleFor(x => x.PrescriberId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing PrecsriberID");
            });

        }
    }
}
