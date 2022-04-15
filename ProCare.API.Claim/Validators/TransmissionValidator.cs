using FluentValidation;
using ProCare.API.Claims.Messages.Request;


namespace ProCare.API.Claims.Validators
{
    public class TransmissionValidator : AbstractValidator<ClaimSubmissionRequest>
    {
        public TransmissionValidator()
        {
            RuleFor(transmission => transmission.Header).SetValidator(new RequestHeaderValidator());
            RuleFor(transmission => transmission.Insurance).SetValidator(new RequestInsuranceValidator());
            RuleFor(transmission => transmission.Patient).SetValidator(new RequestPatientValidator());
            RuleFor(transmission => transmission.Claim).SetValidator(new RequestClaimValidator());
            RuleFor(transmission => transmission.Pricing).SetValidator(new RequestPricingValidator());
            RuleFor(transmission => transmission.Pharmacy).SetValidator(new RequestPharmacyValidator());
            RuleFor(transmission => transmission.Prescriber).SetValidator(new RequestPrescriberValidator());
            RuleFor(transmission => transmission.COB).SetValidator(new RequestCOBValidator());
            RuleFor(transmission => transmission.DUR).SetValidator(new RequestDURValidator());
            RuleFor(transmission => transmission.Compound).SetValidator(new RequestCompoundValidator());
            RuleFor(transmission => transmission.Clinical).SetValidator(new RequestClinicalValidator());
            RuleFor(transmission => transmission.AdditionalDocumentation).SetValidator(new RequestAdditionalDocsValidator());
            RuleFor(transmission => transmission.Facility).SetValidator(new RequestFacilityValidator());
            RuleFor(transmission => transmission.WorkersComp).SetValidator(new RequestWorkersCompValidator());
            RuleFor(transmission => transmission.Coupon).SetValidator(new RequestCouponValidator());
        }
    }
}
