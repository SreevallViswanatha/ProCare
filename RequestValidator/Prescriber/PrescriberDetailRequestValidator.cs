using ProCare.API.Core.Helpers;
using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Shared;
using ServiceStack.FluentValidation;
using System;

namespace ProCare.API.PBM.RequestValidator
{
    public class PrescriberDetailRequestValidator : AbstractValidator<PrescriberDetailRequest>
    {
        public PrescriberDetailRequestValidator()
        {
            //required fields
            RuleFor(x => x.PrescriberID).NotEmpty();
            RuleFor(x => x.Qualifier).NotEmpty();
            RuleFor(x => x.PrescriberID).MaximumLength(10);
            RuleFor(x => x.Qualifier).IsInEnum();
        }
    }
}
