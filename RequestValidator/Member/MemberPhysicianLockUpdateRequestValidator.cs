using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;
using System;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberPhysicianLockUpdateRequestValidator : AbstractValidator<MemberPhysicianLockUpdateRequest>
    {
        public MemberPhysicianLockUpdateRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.Client).NotEmpty();
            RuleFor(x => x.PlanID).NotEmpty().Length(8);
            RuleFor(x => x.PhysicianNPI).NotEmpty().Length(10);
            RuleFor(x => x.PhysicianDEA).Length(9);
            RuleFor(x => x.EffectiveDate).NotEmpty().GreaterThan(DateTime.MinValue);
            RuleFor(x => x).Must(x => HasOnlyOneMemberIdentifier(x))
                .WithMessage("Must provide only one of the following: MemberEnrolleeID, MemberID.");
            RuleFor(x => x.TerminationDate).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(x => x.MemberLockInStatus).Must(x => IsMemberLockInStatus(x))
                .WithMessage($"The MemberLockInStatus provided is not permitted.");
        }

        private bool IsMemberLockInStatus(string status)
        {
            return status == MemberLockinStatus.L.ToString()
                || status == MemberLockinStatus.Q.ToString()
                || status == string.Empty;
        }

        private bool HasOnlyOneMemberIdentifier(MemberPhysicianLockUpdateRequest request)
        {
            return string.IsNullOrWhiteSpace(request.MemberID) != string.IsNullOrWhiteSpace(request.MemberEnrolleeID);
        }
    }
}
