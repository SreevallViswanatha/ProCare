using System;
using FluentValidation;
using ProCare.NCPDP.Telecom;
using ProCare.NCPDP.Telecom.Request;
using ProCare.API.Claims.Helpers;


namespace ProCare.API.Claims.Validators
{
    public class RequestPatientValidator : AbstractValidator<RequestPatient>
    {
        public RequestPatientValidator()
        {
            ValidatorHelper helper = new ValidatorHelper();

            RuleFor(x => x.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Today)
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing/Invalid Date of Birth");

            RuleFor(x => x.Gender)
                .Must(helper.IsValidGender)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Gender");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Last Name");

            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing First Name");
            });

            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing First Name");
            });

            RuleSet("CareMark", () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing First Name");
            });

            RuleSet("MRX", () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing First Name");
            });
            
            RuleSet("Envision", () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing First Name");

                RuleFor(x => x.Street)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Street Address");
            
                RuleFor(x => x.City)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing City");
            
                RuleFor(x => x.State)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing State");
            
                RuleFor(x => x.Zip)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Zip Code");

                RuleFor(x => x.Residence)
                    .Must(helper.IsValidPatientResidence)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Invalid Patient Residence");
            });

            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing First Name");

                //RuleFor(x => x.Zip)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Zip Code");
            
                RuleFor(x => x.Residence)
                    .Must(helper.IsValidPatientResidence)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Invalid Patient Residence");
            });
        }
       
    }
}
