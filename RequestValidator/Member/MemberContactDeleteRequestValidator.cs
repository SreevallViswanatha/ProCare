using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberContactDeleteRequestValidator : MemberPortalTokenValidator<MemberContactDeleteRequest>
    {
        public MemberContactDeleteRequestValidator()
        {
            RuleFor(x => x.MemberContactID).GreaterThan(0);
        }

    }
}
