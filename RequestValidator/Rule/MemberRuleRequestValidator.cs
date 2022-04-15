using ProCare.API.Core.Helpers;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;
using System;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberRuleRequestValidator : AbstractValidator<MemberRuleRequest>
    {
        public MemberRuleRequestValidator()
        {
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.Client).NotEmpty().MaximumLength(8);
            RuleFor(x => x.PlanID).NotEmpty().Length(8);
            RuleFor(x => x.MemberID).MinimumLength(1).MaximumLength(18);
            RuleFor(x => x.MemberEnrolleeID).Length(8);
            RuleFor(x => x).Must(x => HasOnlyOneMemberIdentifier(x))
                .WithMessage("Must provide only one of the following: MemberEnrolleeID, MemberID.");
            RuleFor(x => x.Person).Must(x => ValidationHelper.PersonValues.Contains(x))
                .WithMessage(ValidationHelper.PersonBetweenMessage);
            RuleFor(x => x.NDC).NotEmpty();
            RuleFor(x => x.EffectiveDate).NotEmpty().GreaterThan(DateTime.MinValue);
            RuleFor(x => x.TerminationDate).GreaterThanOrEqualTo(x => x.EffectiveDate);
            RuleFor(x => x).Must(x => HasOnlyRuleIdOrTemplateId(x))
                .WithMessage("Must not provide both a Member Rule ID and Member Rule Template ID.");
        }

        private bool HasOnlyRuleIdOrTemplateId(MemberRuleRequest request)
        {
            return string.IsNullOrWhiteSpace(request.MemberRuleTemplateID) != string.IsNullOrWhiteSpace(request.MemberRuleID);
        }

        private bool HasOnlyOneMemberIdentifier(MemberRuleRequest request)
        {
            return string.IsNullOrWhiteSpace(request.MemberID) != string.IsNullOrWhiteSpace(request.MemberEnrolleeID);
        }
    }
}
