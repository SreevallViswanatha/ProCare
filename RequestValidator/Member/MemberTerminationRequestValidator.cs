using ProCare.API.Core.Helpers;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;
using System;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberTerminationRequestValidator : AbstractValidator<MemberTerminationRequest>
    {
        public MemberTerminationRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.Client).NotEmpty();
            RuleFor(x => x.PlanID).NotEmpty().Length(8);
            RuleFor(x => x.MemberID).MinimumLength(1).MaximumLength(18);
            RuleFor(x => x.MemberEnrolleeID).Length(8);
            RuleFor(x => x).Must(x => HasOnlyOneMemberIdentifier(x))
                .WithMessage("Must provide only one of the following: MemberEnrolleeID, MemberID.");
            RuleFor(x => x.TerminationDate).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(x => x.Person).Must(x => ValidationHelper.PersonValues.Contains(x))
                .WithMessage(ValidationHelper.PersonBetweenMessage);
        }

        private bool HasOnlyOneMemberIdentifier(MemberTerminationRequest request)
        {
            return string.IsNullOrWhiteSpace(request.MemberID) != string.IsNullOrWhiteSpace(request.MemberEnrolleeID);
        }
    }
}
