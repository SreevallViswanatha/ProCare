using ProCare.API.PBM.Messages.Request.Eligibility;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;

namespace ProCare.API.PBM.RequestValidator
{
    public class ACPImportRequestValidator : AbstractValidator<ACPImportRequest>
    {
        private List<string> personValues = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
        public ACPImportRequestValidator()
        {
            //required fields
            RuleFor(x => x.Person).NotEmpty().WithMessage("Person is missing or not 01 - 30")
                                  .Must(x => personValues.Contains(x)).WithMessage("Person is missing or not 01 - 30");
            RuleFor(x => x.RecIdentifier).NotEmpty().WithMessage("RecIdentifer Blank")
                                         .MaximumLength(50).WithMessage("RecIdentifer is missing or longer than 50 characters");
            RuleFor(x => x.AdjustmentDate).NotEmpty().WithMessage("Adjustment Date Blank")
                                          .Must(x => DateTime.TryParse(x, out DateTime parsed)).WithMessage("AdjustmentDate is missing or invalid")
                                          .Must(x => DateTime.Parse(x) < DateTime.Now.AddDays(1)).WithMessage(@"Adjustment Greater Than Today's Date")
                                          .Must(x => DateTime.Parse(x) > DateTime.Now.AddYears(-3)).WithMessage("AdjustmentDate is missing or invalid");
            RuleFor(x => x.PlanID).NotEmpty().WithMessage("PlanID not supplied");
            RuleFor(x => x.CardholderID).NotEmpty().WithMessage("CardID is missing or longer than 18 characters")
                                        .MaximumLength(18).WithMessage("CardID is missing or longer than 18 characters");
        }
    }
}
