using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberPhysicianLockDetailsRequestValidator : AbstractValidator<MemberPhysicianLockDetailsRequest>
    {
        public MemberPhysicianLockDetailsRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.Client).NotEmpty();
            RuleFor(x => x.MemberID).NotEmpty().MinimumLength(1).MaximumLength(18);
        }
    }
}
