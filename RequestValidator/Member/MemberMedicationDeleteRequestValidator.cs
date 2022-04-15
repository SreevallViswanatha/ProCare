
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberMedicationDeleteRequestValidator : MemberPortalTokenValidator<MemberMedicationDeleteRequest>
    {
        public MemberMedicationDeleteRequestValidator()
        {
            RuleFor(x => x.MemberMedicationID).GreaterThan(0);
        }
    }

}