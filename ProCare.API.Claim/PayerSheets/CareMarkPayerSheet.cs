using System;
using FluentValidation;
using ProCare.NCPDP.Telecom;
using ProCare.API.Claims.Validators;
using ProCare.API.Claims.Helpers;

namespace ProCare.API.Claims.PayerSheets
{
    public class CareMarkPayerSheet : AbstractValidator<Transmission>
    {
        ValidatorHelper helper = new ValidatorHelper();

        #region Claims Validators
        public void ValidateClaimSubmissionRequest(Transmission request)
        {
            ValidateSubmissionHeaderSegment();
            ValidateSubmissionInsuranceSegment();
            ValidateSubmissionPatientSegment();
            ValidateSubmissionClaimSegment();
            ValidateSubmissionPricingSegment();
        }

        public void ValidateClaimReversalRequest(Transmission request)
        {
            ValidateReversalHeaderSegment();
            ValidateReversalClaimSegment();
            ValidateReversalInsuranceSegment();
            ValidateReversalPatientSegment();
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

            RuleFor(x => x.Insurance.GroupId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing GroupID");

            RuleFor(x => x.Insurance.PersonCode)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Person Code");

            RuleFor(x => x.Insurance.PatientRelationshipCode)
                .NotEmpty()
                .Must(helper.IsValidPatientRelationshipCode)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Patient Relationship Code");
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

            RuleFor(x => x.Patient.FirstName)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing First Name");

            RuleFor(x => x.Patient.LastName)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Last Name");

            RuleFor(x => x.Patient.Residence)
                .Must(helper.EqualPatientResidenceHome)
                .WithSeverity(Severity.Error)
                .WithState(x => "Patient Residence Not equal Home");

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

            RuleFor(x => x.CurrentTransaction.Claim.DispenseAsWritten)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Dispense As Written");

            RuleFor(x => x.CurrentTransaction.Claim.DatePrescriptionWritten)
                .LessThanOrEqualTo(DateTime.Today)
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing/Invalid Date Prescription Written");

            RuleFor(x => x.CurrentTransaction.Claim.RefillsAuthorized)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Refills Authorized");
            }
        public void ValidateSubmissionPricingSegment()
        {
            RuleFor(x => x.CurrentTransaction.Pricing.IngredientCostSubmitted)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Ingredient Cost Submitted");

            RuleFor(x => x.CurrentTransaction.Pricing.DispensingFeeSubmitted)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Dispensing Fee Submitted");

            RuleFor(x => x.CurrentTransaction.Pricing.GrossAmountDue)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Gross Amount Due");

            RuleFor(x => x.CurrentTransaction.Pricing.BasisOfCostDetermination)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Basis of Cost Determination");

            RuleFor(x => x.CurrentTransaction.Pricing.UsualAndCustomaryCharge)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Usual and Customary Charge");
        }
        public void ValidateSubmissionPrescriberSegment()
        {
            RuleFor(x => x.CurrentTransaction.Prescriber.PrescriberIdQualifier)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Prescriber ID Qualifier");

            RuleFor(x => x.CurrentTransaction.Prescriber.PrescriberId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing PrescriberID");

            RuleFor(x => x.CurrentTransaction.Prescriber.State)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing Prescriber State");
        }

        #endregion Submission Segment Validators

        #region Reversal Segment Validators

        public void ValidateReversalHeaderSegment()
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
                .Must(helper.EqualReversalClaim)
                .WithSeverity(Severity.Error)
                .WithState(x => "Not Reversal Transaction Code");

            RuleFor(x => x.Header.TransactionCount)
                .InclusiveBetween(0, 4)
                .WithSeverity(Severity.Error)
                .WithState(x => "Invalid Transaction Count");

            RuleFor(x => x.Header.SoftwareId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing SoftwareID");
        }
        public void ValidateReversalClaimSegment()
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

            RuleFor(x => x.CurrentTransaction.Claim.FillNumber)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Missing FillNumber");

            RuleFor(x => x.CurrentTransaction.Claim.QuantityDispensed)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Quantity Dispensed not equal Zero");

            RuleFor(x => x.CurrentTransaction.Claim.DaysSupply)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Days Supply not equal Zero");

            RuleFor(x => x.CurrentTransaction.Claim.CompoundCode)
                .Must(helper.EqualNotSpecifiedCompoundCode)
                .WithSeverity(Severity.Error)
                .WithState(x => "Compound Code not set to Not Specified");

            RuleFor(x => x.CurrentTransaction.Claim.DispenseAsWritten)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Dispense As Written is not blank");

            RuleFor(x => x.CurrentTransaction.Claim.DatePrescriptionWritten)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Date Prescription Written is not blank");

            RuleFor(x => x.CurrentTransaction.Claim.PrescriptionOriginCode)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Prescription Origin Code is not blank");

            RuleFor(x => x.CurrentTransaction.Claim.UnitOfMeasure)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Unit Of Measure is not blank");

            RuleFor(x => x.CurrentTransaction.Claim.SpecialPackagingIndicator)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Special Packaging Indicator is not blank");
        }
        public void ValidateReversalInsuranceSegment()
        {
            RuleFor(x => x.Insurance.CardholderFirstName)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Cardholder First Name not empty");

            RuleFor(x => x.Insurance.CardholderLastName)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Cardholder Last Name not empty");

            RuleFor(x => x.Insurance.PersonCode)
                .Empty()
                .WithSeverity(Severity.Error)
                .WithState(x => "Person Code not empty");

            RuleFor(x => x.Insurance.PatientRelationshipCode)
                .Must(helper.EqualNotSpecifiedPatientRelationship)
                .WithSeverity(Severity.Error)
                .WithState(x => "Patient Relationship Code not set to Not Specified");

            RuleFor(x => x.Insurance.EligibilityClarificationCode)
                .Must(helper.EqualNotSpecifiedEligibilityClarificationCode)
                .WithSeverity(Severity.Error)
                .WithState(x => "Eligibility Clarification Code not set to Not Specified");
        }
        public void ValidateReversalPatientSegment()
        {
            RuleFor(x => x.Patient.Residence)
                .Must(helper.EqualPatientResidenceHome)
                .WithSeverity(Severity.Error)
                .WithState(x => "Patient Residence Not equal Home");
        }

        #endregion Reversal Segment Validators

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
