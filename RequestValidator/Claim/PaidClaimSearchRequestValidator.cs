using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class PaidClaimSearchRequestValidator : AbstractValidator<PaidClaimSearchRequest>
    {
        public PaidClaimSearchRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0).LessThan(101);
            RuleFor(x => x.MemberId).NotEmpty();
            RuleFor(x => x.MemberIdOperator).Length(1);
            RuleFor(x => x.MemberId).MinimumLength(3).MaximumLength(18);
            RuleFor(x => x.FillDateFrom).NotEmpty();
            RuleFor(x => x.FillDateTo).NotEmpty();
            RuleFor(x => x.FillDateFrom).LessThanOrEqualTo(x => x.FillDateTo);
        }
    }
}
