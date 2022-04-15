using ProCare.API.PBM.Messages.Request.Eligibility;
using ServiceStack.FluentValidation;
using System.Collections.Generic;

namespace ProCare.API.PBM.RequestValidator
{
    public class AccumulatorMemberValidator : AbstractValidator<AccumulatorMember>
    {
        private List<string> personValues = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
        private List<int> relationshipValues = new List<int> { 1, 2, 3, 4 };

        public AccumulatorMemberValidator()
        {
            //required fields
            RuleFor(x => x.PlanID).NotEmpty();
            RuleFor(x => x.CardID).NotEmpty();
            RuleFor(x => x.Person).NotEmpty().Must(x => personValues.Contains(x)).WithMessage(string.Format("Person must be a valid person code ({0})", string.Join(", ", personValues)));
            RuleFor(x => x.Relationship).NotEmpty().Must(x => relationshipValues.Contains(x)).WithMessage(string.Format("Relationship must be a valid relationship code ({0})", string.Join(", ", relationshipValues)));
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(12);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(16);
            RuleFor(x => x.EffectiveDate).NotEmpty();
            RuleFor(x => x.TerminationDate).NotEmpty();
        }
    }
}
