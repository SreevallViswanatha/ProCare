using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class HospiceSecurityCodeLookupRequestValidator : AbstractValidator<HospiceSecurityCodeLookupRequest>
    {

        public HospiceSecurityCodeLookupRequestValidator()
        {
            //required fields
            RuleFor(x => x.HospiceSecurityCode).NotEmpty().MaximumLength(12);
        }
    }
}
