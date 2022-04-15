using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberDetailsRequestValidator : AbstractValidator<MemberDetailsRequest>
    {
        public MemberDetailsRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.OrganizationID).NotEmpty().Length(8);
            RuleFor(x => x.GroupID).NotEmpty().Length(8);
            RuleFor(x => x.PlanID).NotEmpty().Length(8);
            RuleFor(x => x.MemberID).MinimumLength(3).MaximumLength(18);
            RuleFor(x => x.Person).Length(2);
        }
    }
}
