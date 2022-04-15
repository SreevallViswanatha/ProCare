namespace ProCare.API.PBM.RequestValidator
{
    using ProCare.API.PBM.Messages.Request;

    using ServiceStack.FluentValidation;


    public class UsersWithPlansListRequestValidator : AbstractValidator<UsersWithPlansListRequest>
    {
        public UsersWithPlansListRequestValidator()
        {
            RuleFor(x => x.ClientGUID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClientID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.PlanIDs).Must(x => x != null);
        }
    }
}

