using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberPortalTokenValidator<T> : AbstractValidator<T> where T : MemberPortalMobileAuthenticatedUserRequest
    {
        public MemberPortalTokenValidator()
        {
            RuleFor(x => x.Token).NotEmpty().Length(36);
        }
    }
}