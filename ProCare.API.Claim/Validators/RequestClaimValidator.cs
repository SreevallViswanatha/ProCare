using System;
using FluentValidation;
using ProCare.NCPDP.Telecom.Request;
using ProCare.API.Claims.Helpers;

namespace ProCare.API.Claims.Validators
{
    public class RequestClaimValidator : AbstractValidator<RequestClaim>
    {
        public RequestClaimValidator()
        {
            ValidatorHelper helper = new ValidatorHelper();

            RuleFor(x => x.PrescriptionQualifier)
                .NotEmpty()
                .Must(helper.IsValidPrescriptionQualifier)
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Prescription Qualifier");

            //RuleFor(x => x.PrescriptionNumber)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing Prescription Number");

            RuleFor(x => x.QuantityDispensed)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Quantity Dispensed");

            RuleFor(x => x.FillNumber)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Fill Number");

            RuleFor(x => x.DaysSupply)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Days Supply");

            RuleFor(x => x.CompoundCode)
                .IsInEnum()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Compound Code");

            //RuleFor(x => x.DispenseAsWritten)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing Dispense As Written");

            RuleFor(x => x.DatePrescriptionWritten)
                .LessThanOrEqualTo(DateTime.Today)
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing/Invalid Date Prescription Written");

            RuleSet("CareMark", () =>
            {
                RuleFor(x => x.RefillsAuthorized)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Refills Authorized");
            });

            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.RefillsAuthorized)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Refills Authorized");
            });

            RuleSet("MRX", () =>
            {
                RuleFor(x => x.RefillsAuthorized)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Refills Authorized");

                RuleFor(x => x.PrescriptionOriginCode)
                    .Must(helper.IsValidOriginCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Origin Code");

                RuleFor(x => x.UnitOfMeasure)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Unit Of Measure");
            });

            RuleSet("Envision", () =>
            {
                RuleFor(x => x.PrescriptionOriginCode)
                    .Must(helper.IsValidOriginCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Origin Code");

                RuleFor(x => x.OtherCoverageCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other overage Code");

                RuleFor(x => x.DispensingStatus)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Status");

                RuleFor(x => x.PatientAssignmentIndicator)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Patient Assignment Indicator");

                RuleFor(x => x.PharmacyServiceType)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Pharmacy Service Type");
            });

            RuleSet("ExpressScripts", () =>
            {
                RuleFor(x => x.PrescriptionOriginCode)
                    .Must(helper.IsValidOriginCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Origin Code");

                RuleFor(x => x.OtherCoverageCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Other overage Code");

                RuleFor(x => x.RefillsAuthorized)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Refills Authorized");

                //RuleFor(x => x.UnitOfMeasure)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Unit Of Measure");

                //RuleFor(x => x.PharmacyServiceType)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Pharmacy Service Type");
            });

            RuleSet("Prime", () =>
            {
                RuleFor(x => x.PrescriptionOriginCode)
                    .Must(helper.IsValidOriginCode)
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Prescription Origin Code");
            });
            
        }

        
    }
}
