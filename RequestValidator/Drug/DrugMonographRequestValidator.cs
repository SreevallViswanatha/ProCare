using ProCare.API.Core.Helpers;
using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Shared;
using ServiceStack.FluentValidation;
using System;

namespace ProCare.API.PBM.RequestValidator
{
    public class DrugMonographRequestValidator : AbstractValidator<DrugMonographRequest>
    {
        public DrugMonographRequestValidator()
        {
            RuleFor(x => x.ProductQualifier).NotEmpty();
            RuleFor(x => x.Productid).NotEmpty();
            RuleFor(x => x.LanguageCode).NotEmpty();
            RuleFor(x => x.ProductQualifier).IsInEnum();
            RuleFor(x => x.LanguageCode).IsInEnum();
        }
    }
}
