using ProCare.API.Core.Helpers;
using ProCare.API.PBM.Messages.Request;

using ServiceStack.FluentValidation;
using ProCare.API.PBM.Messages.Shared;

namespace ProCare.API.PBM.RequestValidator
{
    public class CodedEntityRequestValidator : AbstractValidator<CodedEntityRequest>
    {
        public CodedEntityRequestValidator()
        {
            RuleFor(x => x.ClientGUID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClientID).NotEmpty().MaximumLength(100);

            //required fields
            RuleFor(x => x.CodedEntityTypeID).NotEmpty();

            //content rules
            RuleFor(x => x.CodedEntityTypeID).Must(x => ValidationHelper.IsEnumValueDefined<Enums.CodedEntityType>(x)).WithMessage("Coded Entity Type ID is not valid");
        }
    }
}
