using ProCare.API.Core.Helpers;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;
using System;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberPortalRegistrationRequestValidator : AbstractValidator<MemberPortalRegistrationRequest>
    {
        public MemberPortalRegistrationRequestValidator()
        {
            RuleFor(x => x.DomainName).MaximumLength(50);
            RuleFor(x => x.BinNumber).NotEmpty().MaximumLength(6);
            RuleFor(x => x.CardID).NotEmpty().MaximumLength(18);
            RuleFor(x => x.DateOfBirth).NotEmpty().LessThanOrEqualTo(DateTime.Today).NotEqual(DateTime.MinValue);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
            RuleFor(x => x.EmailAddress).NotEmpty().MaximumLength(100).Must(ValidationHelper.IsValidEmailAddress)
                                        .WithMessage(ValidationHelper.ValidEmailIsRequiredMessage);
            RuleFor(x => x.Gender).NotEmpty().IsInEnum();
            RuleFor(x => x.DependentFirstName).MaximumLength(15);
            RuleFor(x => x.VerifyQuestion).NotEmpty().MaximumLength(100);
            RuleFor(x => x.VerifyAnswer).NotEmpty().MaximumLength(100);
        }
    }
}
