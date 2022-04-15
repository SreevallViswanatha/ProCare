using System;
using FluentValidation;
using ProCare.NCPDP.Telecom;
using ProCare.API.Claims.Validators;
using ProCare.API.Claims.Helpers;

namespace ProCare.API.Claims.PayerSheets
{
    public class DefaultPayerSheet : AbstractValidator<Transmission>
    {
        ValidatorHelper helper = new ValidatorHelper();

        #region Claim Validators
        public void ValidateClaimSubmissionRequest(Transmission request)
        {
            ValidateSubmissionHeaderSegment();
            ValidateSubmissionInsuranceSegment();
            ValidateSubmissionPatientSegment();
            ValidateSubmissionClaimSegment();
            ValidateSubmissionPricingSegment();

            RuleSet("IllinoisMedicaid", () =>
            {
                RuleFor(transmission => transmission).SetValidator(new IllinoisMedicaidPayerSheet());
            });
        }
        public void ValidateClaimReversalRequest(Transmission request)
        {

        }
        public void ValidateClaimEligibilityRequest(Transmission request)
        {
            ValidateEligibilityHeaderSegment();
            RuleFor(x => x.Insurance).SetValidator(new RequestEligibilityInsuranceValidator());
        }
        #endregion

        #region Submission Segment Validators
        public void ValidateSubmissionHeaderSegment()
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

            //RuleFor(x => x.Header.ProcessorControlNumber)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing Processor Control Number");

            RuleFor(x => x.Header.ServiceProviderId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Service Provider ID");

            RuleFor(x => x.Header.ServiceProviderIdQualifier)
                .Must(helper.IsValidServiceProviderIdQualifier)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Service ProviderID Qualifier");

            RuleFor(x => x.Header.TransactionCode)
                .Must(helper.EqualBillingClaim)
                .WithSeverity(Severity.Error)
                .WithState(x => "Not Billing Transaction Code");

            RuleFor(x => x.Header.TransactionCount)
                .InclusiveBetween(0, 4)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Transaction Count");

            RuleFor(x => x.Header.SoftwareId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing SoftwareID");
        }
        public void ValidateSubmissionInsuranceSegment()
        {
            RuleFor(x => x.Insurance.CardholderId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing CardholderID");
            
        }
        public void ValidateSubmissionPatientSegment()
        {
            RuleFor(x => x.Patient.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Today)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Date of Birth");

            RuleFor(x => x.Patient.Gender)
                .Must(helper.IsValidGender)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Gender");
       
            RuleFor(x => x.Patient.LastName)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Last Name");
        }
        public void ValidateSubmissionClaimSegment()
        {
            RuleFor(x => x.CurrentTransaction.Claim.PrescriptionQualifier)
                .Must(helper.EqualNpiPrescriptionQualifier)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Prescription Qualifier");

            RuleFor(x => x.CurrentTransaction.Claim.PrescriptionNumber)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing PrescriptionNumber");

            RuleFor(x => x.CurrentTransaction.Claim.ProductIdQualifier)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing ProductIDQualifier");

            RuleFor(x => x.CurrentTransaction.Claim.ProductId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing ProductID");

            RuleFor(x => x.CurrentTransaction.Claim.QuantityDispensed)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Quantity Dispensed");

            RuleFor(x => x.CurrentTransaction.Claim.FillNumber)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing FillNumber");

            RuleFor(x => x.CurrentTransaction.Claim.DaysSupply)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Days Supply");

            RuleFor(x => x.CurrentTransaction.Claim.CompoundCode)
                .IsInEnum()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Compound Code");

            //RuleFor(x => x.CurrentTransaction.Claim.DispenseAsWritten)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing Dispense As Written");

            RuleFor(x => x.CurrentTransaction.Claim.DatePrescriptionWritten)
                .LessThanOrEqualTo(DateTime.Today)
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing/Invalid Date Prescription Written");
        }
        public void ValidateSubmissionPricingSegment()
        {
            //RuleFor(x => x.CurrentTransaction.Pricing.GrossAmountDue)
            //    .NotEmpty()
            //    .WithSeverity(Severity.Error)
            //    .WithState(x => "Missing Gross Amount Due");
        }
  
        #endregion Submission Segment Validators

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
