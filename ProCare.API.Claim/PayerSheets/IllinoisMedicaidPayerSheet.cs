using System;
using FluentValidation;
using ProCare.NCPDP.Telecom;
using ProCare.API.Claims.Validators;
using ProCare.API.Claims.Helpers;


namespace ProCare.API.Claims.PayerSheets
{ 
    public class IllinoisMedicaidPayerSheet : AbstractValidator<Transmission>
    {
        ValidatorHelper helper = new ValidatorHelper();

        #region Claims Validators
        public void ValidateClaimSubmissionRequest(Transmission request)
        {
            RuleFor(x => x.CurrentTransaction.Claim.QuantityPrescribed)
                .Must(helper.MustEqualZero)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Quantity Prescribed");

            RuleFor(x => x.CurrentTransaction.Claim.RefillsAuthorized)
                .Must(helper.MustEqualZero)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Refills Authorized");

            RuleFor(x => x.CurrentTransaction.Prescriber.LastName)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Prescriber Last Name");

            RuleFor(x => x.Patient.Residence)
                .Must(helper.EqualPatientResidenceHome)
                .WithSeverity(Severity.Error)
                .WithState(x => "Patient Residence Not equal Home");
        }

        public void ValidateClaimReversalRequest(Transmission request)
        {
            RuleFor(x => x.CurrentTransaction.Claim.QuantityPrescribed)
                .Must(helper.MustEqualZero)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Quantity Prescribed");

            RuleFor(x => x.CurrentTransaction.Claim.RefillsAuthorized)
                .Must(helper.MustEqualZero)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Refills Authorized");

            RuleFor(x => x.CurrentTransaction.Prescriber.LastName)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Prescriber Last Name");

            RuleFor(x => x.Patient.Residence)
                .Must(helper.EqualPatientResidenceHome)
                .WithSeverity(Severity.Error)
                .WithState(x => "Patient Residence Not equal Home");
        }

        public void ValidateClaimEligibilityRequest(Transmission request)
        {
            ValidateEligibilityHeaderSegment();
            RuleFor(x => x.Insurance).SetValidator(new RequestEligibilityInsuranceValidator());
        }
        #endregion

        #region Eligibility Segment Validators

        public void ValidateEligibilityHeaderSegment()
        {
            RuleFor(x => x.Header.BinNumber)
                .Length(6)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Bin Number");

            RuleFor(x => x.Header.VersionNumber)
                .Must(helper.CurrentVersionNumber)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Version Number");

            RuleFor(x => x.Header.DateOfService)
                .LessThanOrEqualTo(DateTime.Today)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Date Of Service");

            RuleFor(x => x.Header.ProcessorControlNumber)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Processor Control Number");

            RuleFor(x => x.Header.ServiceProviderId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Service Provider ID");

            RuleFor(x => x.Header.ServiceProviderIdQualifier)
                .Must(helper.IsValidServiceProviderIdQualifier)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Service ProviderID Qualifier");

            RuleFor(x => x.Header.TransactionCode)
                .Must(helper.EqualEligibilityClaim)
                .WithSeverity(Severity.Error)
                .WithState(x => "Not Eligibility Transaction Code");

            RuleFor(x => x.Header.TransactionCount)
                .InclusiveBetween(0, 4)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Transaction Count");

            RuleFor(x => x.Header.SoftwareId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing SoftwareID");
        }

        #endregion Eligibility Segment Validators
    }
}
