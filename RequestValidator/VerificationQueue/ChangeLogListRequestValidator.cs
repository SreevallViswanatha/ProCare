namespace ProCare.API.PBM.RequestValidator
{
    using ProCare.API.PBM.Messages.Request;

    using ServiceStack.FluentValidation;

    public class ChangeLogListRequestValidator : AbstractValidator<ChangeLogListRequest>
    {
        public ChangeLogListRequestValidator()
        {
            RuleFor(x => x.ClientGUID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClientID).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ENRID).NotEmpty().MaximumLength(8);
        }
    }
}
