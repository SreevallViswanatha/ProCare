using ProCare.API.PBM.Messages.Request.Eligibility;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class AccumulatorImportRequestValidator : AbstractValidator<AccumulatorImportRequest>
    {
        public AccumulatorImportRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientID).NotEmpty();
            RuleFor(x => x.Member).SetValidator(new AccumulatorMemberValidator());
        }
    }
}
