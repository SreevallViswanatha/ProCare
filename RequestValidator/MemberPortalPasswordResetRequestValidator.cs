using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberPortalPasswordResetRequestValidator : AbstractValidator<MemberPortalPasswordResetRequest>
    {
        public MemberPortalPasswordResetRequestValidator()
        {
            //required fields
            RuleFor(x => x.DomainName).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            
        }
    }
}
