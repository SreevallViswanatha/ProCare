using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class TestClaimRequestValidator : AbstractValidator<TestClaimRequest>
    {
        public TestClaimRequestValidator()
        {
            RuleFor(x => x.Claim.DaysSupply).NotEmpty();
            RuleFor(x => x.Claim.ProductIdQualifier).NotEmpty();
            RuleFor(x => x.Claim.QuantityDispensed).NotEmpty();
            RuleFor(x => x.Claim.ProductId).NotEmpty();

            RuleFor(x => x.Header.BinNumber).NotEmpty();
            RuleFor(x => x.Header.ServiceProviderId).NotEmpty();
            RuleFor(x => x.Header.ServiceProviderIdQualifier).NotEmpty();

            RuleFor(x => x.Insurance.CardholderId).NotEmpty();
            RuleFor(x => x.Insurance.GroupId).NotEmpty();
            RuleFor(x => x.Insurance.PersonCode).NotEmpty();
            RuleFor(x => x.Insurance.PatientRelationshipCode).NotEmpty();

            RuleFor(x => x.Patient.DateOfBirth).NotEmpty();
            RuleFor(x => x.Patient.FirstName).NotEmpty();
            RuleFor(x => x.Patient.Gender).NotEmpty();
            RuleFor(x => x.Patient.LastName).NotEmpty();

            RuleFor(x => x.Header.ServiceProviderIdQualifier).IsInEnum();
            RuleFor(x => x.Insurance.PatientRelationshipCode).IsInEnum();
            RuleFor(x => x.Patient.Gender).IsInEnum();

        }
    }
}
