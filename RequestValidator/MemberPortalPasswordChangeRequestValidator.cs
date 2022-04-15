using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberPortalPasswordChangeRequestValidator : AbstractValidator<MemberPortalPasswordChangeRequest>
    {
        public MemberPortalPasswordChangeRequestValidator()
        {
            //required fields
            RuleFor(x => x.DomainName).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
            
        }
    }
}
