

using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class AppUserInfoListRequestValidator : AbstractValidator<AppUserInfoListRequest>
    {
        public AppUserInfoListRequestValidator()
        {
            RuleFor(x => x.ClientGUID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClientID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AppUserID).NotEmpty();
            RuleFor(x => x.IncludePermissions).NotNull();
        }
    }
}
