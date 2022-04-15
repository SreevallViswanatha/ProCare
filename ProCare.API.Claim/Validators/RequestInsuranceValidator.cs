using FluentValidation;
using ProCare.API.Claims.Helpers;
using ProCare.NCPDP.Telecom.Request;

namespace ProCare.API.Claims.Validators
{
    public class RequestInsuranceValidator : AbstractValidator<RequestInsurance>
    {
        public RequestInsuranceValidator()
        {
            ValidatorHelper helper = new ValidatorHelper();

            RuleFor(x => x.CardholderId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing CardholderID");

            RuleSet("OptumRx", () =>
            {
                RuleFor(x => x.CardholderFirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Cardholder First Name");
            
                RuleFor(x => x.CardholderLastName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Cardholder Last Name");
           
                RuleFor(x => x.GroupId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing GroupId");
            });
            RuleSet("MRX", () =>
            {
                RuleFor(x => x.CardholderFirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Cardholder First Name");

                RuleFor(x => x.CardholderLastName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Cardholder Last Name");

                RuleFor(x => x.GroupId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing GroupId");
            });
            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.GroupId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing GroupId");

                RuleFor(x => x.PersonCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Person Code");

                RuleFor(x => x.PatientRelationshipCode)
                    .NotEmpty()
                    .Must(helper.IsValidPatientRelationshipCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Patient Relationship Code");
            });
            RuleSet("CareMark", () =>
            {
                RuleFor(x => x.GroupId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing GroupId");

                RuleFor(x => x.PersonCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Person Code");

                RuleFor(x => x.PatientRelationshipCode)
                    .NotEmpty()
                    .Must(helper.IsValidPatientRelationshipCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Patient Relationship Code");
            });
            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.CardholderFirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Cardholder First Name");

                RuleFor(x => x.CardholderLastName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Cardholder Last Name");

                //RuleFor(x => x.EligibilityClarificationCode)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Eligibility Clarification Code");

                RuleFor(x => x.GroupId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing GroupId");

                RuleFor(x => x.PersonCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Person Code");

                RuleFor(x => x.PatientRelationshipCode)
                    .NotEmpty()
                    .Must(helper.IsValidPatientRelationshipCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Patient Relationship Code");
            });
            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.PatientRelationshipCode)
                    .NotEmpty()
                    .Must(helper.IsValidPatientRelationshipCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Patient Relationship Code");
            });
            RuleSet("Envision", () =>
            {
                RuleFor(x => x.CardholderFirstName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Cardholder First Name");

                RuleFor(x => x.CardholderLastName)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Cardholder Last Name");

                RuleFor(x => x.GroupId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing GroupId");

                RuleFor(x => x.PersonCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Person Code");

                RuleFor(x => x.PatientRelationshipCode)
                    .NotEmpty()
                    .Must(helper.IsValidPatientRelationshipCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Patient Relationship Code");

                RuleFor(x => x.ProviderAcceptAssignmentIndicator)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Provider Accept Assignment Indicator");
            
                RuleFor(x => x.MedicaidIdNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing MedicaidID Number");
      
                RuleFor(x => x.MedicaidAgencyNumber)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Medicaid Agency Number");
            });
        }
      
    }
}
