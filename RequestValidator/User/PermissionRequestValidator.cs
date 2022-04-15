using ProCare.API.PBM.Messages.Request;
using ProCare.API.Core.Helpers;
using ServiceStack.FluentValidation;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.PBM.RequestValidator
{
    public class PermissionRequestValidator : AbstractValidator<PermissionRequest>
    {
        public PermissionRequestValidator()
        {
            RuleFor(x => x.PermissionID).NotEmpty();
            RuleFor(x => x.MinimumGrantLevel).Must(x => ValidationHelper.IsEnumValueDefined<PermissionGrantLevel>(x));
        }
    }
}
