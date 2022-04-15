using ProCare.API.Core.Helpers;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class PharmacySearchRequestValidator : AbstractValidator<PharmacySearchRequest>
    {
        public PharmacySearchRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0).LessThan(101);
            RuleFor(x => x.WithinMiles).GreaterThan(-1).LessThan(51);
            RuleFor(x => x.Zip).NotEmpty().Length(5).Must(x => ValidationHelper.IsNumericOnly(x))
                               .WithMessage(ValidationHelper.GetErrorMessage(ValidationHelper.MustBeNumericMessage, "Zip"));
        }
    }
}
