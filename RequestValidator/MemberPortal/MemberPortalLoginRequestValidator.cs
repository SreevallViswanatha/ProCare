using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberPortalLoginRequestValidator : AbstractValidator<MemberPortalLoginRequest>
    {
        public MemberPortalLoginRequestValidator()
        {
            //required fields
            RuleFor(x => x.DomainName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
        }
    }
}
