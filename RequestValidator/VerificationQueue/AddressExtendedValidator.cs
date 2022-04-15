namespace ProCare.API.PBM.RequestValidator
{

    using ProCare.API.PBM.Messages.Request;

    using ServiceStack.FluentValidation;

    using System;

    using static ProCare.API.PBM.Messages.Shared.Enums;

    public class AddressExtendedValidator : AbstractValidator<AddressExtended>
    {
        public AddressExtendedValidator()
        {
            RuleFor(x => x.Address1).NotEmpty().MaximumLength(40);
            RuleFor(x => x.Address2).MaximumLength(40);
            RuleFor(x => x.City).NotEmpty().MaximumLength(20);
            RuleFor(x => x.State).NotEmpty().MaximumLength(2);
            RuleFor(x => x.Zip1).NotEmpty().MaximumLength(5);

            RuleFor(x => x.State).Must(IsValidState);
        }

        private bool IsValidState(string state)
        {
           return Enum.IsDefined(typeof(State), state);
        }
    }
}
