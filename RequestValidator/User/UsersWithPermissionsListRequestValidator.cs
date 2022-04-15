using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class UsersWithPermissionsListRequestValidator : AbstractValidator<UsersWithPermissionsListRequest>
    {
        public UsersWithPermissionsListRequestValidator()
        {
            RuleFor(x => x.ClientGUID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClientID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Permissions).Must(x => x != null);

            RuleForEach(x => x.Permissions).SetValidator(new PermissionRequestValidator());
        }
    }
}
