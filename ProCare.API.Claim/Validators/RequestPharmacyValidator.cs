using System;
using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class RequestPharmacyValidator : AbstractValidator<RequestPharmacy>
    {
        public RequestPharmacyValidator()
        {
            RuleSet("CareMarkPharmacy", () =>
            {

                RuleFor(x => x.ProviderIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProviderId Qualifier");
            
                RuleFor(x => x.ProviderId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProviderId");
            });

            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.ProviderIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProviderId Qualifier");
            
                RuleFor(x => x.ProviderId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProviderId");
            });

            RuleSet("Envision", () =>
            {
                RuleFor(x => x.ProviderIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProviderId Qualifier");
           
                RuleFor(x => x.ProviderId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProviderId");
            });

            RuleSet("Prime", () =>
            {
                RuleFor(x => x.ProviderIdQualifier)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProviderId Qualifier");
            
                RuleFor(x => x.ProviderId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing ProviderId");
            });

        }
    }
}
