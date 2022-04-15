using System;
using FluentValidation;
using ProCare.NCPDP.Telecom.Request;
using ProCare.API.Claims.Helpers;

namespace ProCare.API.Claims.Validators
{
    public class RequestHeaderValidator : AbstractValidator<RequestHeader>
    {
        public RequestHeaderValidator()
        {
            ValidatorHelper helper = new ValidatorHelper();

            RuleFor(x => x.BinNumber)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Bin Number");

            RuleFor(x => x.VersionNumber)
                .Must(helper.IsValidVersionNumber)
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing/Invalid Version Number");

            //RuleFor(x => x.DateOfService)
            //    .LessThanOrEqualTo(DateTime.Today)
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing/Invalid Date Of Service");

            //RuleFor(x => x.ProcessorControlNumber)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing Processor Control Number");

            RuleFor(x => x.ServiceProviderId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing/Invalid Service Provider ID");

            RuleFor(x => x.ServiceProviderIdQualifier)
                .Must(helper.IsValidServiceProviderIdQualifier)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Service ProviderID Qualifier");

            //RuleFor(x => x.TransactionCode)
            //    .Must(helper.EqualBillingClaim)
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Invalid Transaction Code");

            RuleFor(x => x.TransactionCount)
                .InclusiveBetween(0, 4)
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing/Invalid Transaction Count");
            

            RuleSet("CareMark", () =>
            {
                RuleFor(x => x.SoftwareId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing SoftwareId");
            });

            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.SoftwareId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing SoftwareId");
            });

            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.SoftwareId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing SoftwareId");
            });

            RuleSet("MRX", () =>
            {
                RuleFor(x => x.SoftwareId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing SoftwareId");
            });

            RuleSet("Prime", () =>
            {
                RuleFor(x => x.SoftwareId)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing SoftwareId");
            });

        }
        
    }
}
