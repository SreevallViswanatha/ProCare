using ProCare.API.Core.Helpers;
using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Shared;
using ServiceStack.FluentValidation;
using System;

namespace ProCare.API.PBM.RequestValidator
{
    public class PrescriberSearchRequestValidator : AbstractValidator<PrescriberSearchRequest>
    {
        public PrescriberSearchRequestValidator()
        {
            //required fields
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0).LessThan(101);
            RuleFor(x => x.PrescriberIdQualifier).Length(2).Must(x => ValidationHelper.IsNumericOnly(x)).WithMessage(ValidationHelper.GetErrorMessage(ValidationHelper.MustBeNumericMessage, "PrescriberIdQualifier"));
            RuleFor(x => x.LastNameOperator).Length(1);
            RuleFor(x => x.FirstNameOperator).Length(1);
            RuleFor(x => x.PrescriberId).MinimumLength(3).MaximumLength(18);
            RuleFor(x => x.LastName).MinimumLength(3).MaximumLength(35);
            RuleFor(x => x.FirstName).MinimumLength(3).MaximumLength(20);
            RuleFor(x => x).Must(validParamCombo).WithMessage("Request must contain either a valid PrescriberId/PrescriberIdQualifier or Name.  It may not contain both.  If passing an Operator, valid values are S (Starts With) and E (Ends With).");
        }

        private bool validParamCombo(PrescriberSearchRequest request)
        {
            bool valid = false;

            try
            {

                bool prescriberIdSearch = !string.IsNullOrWhiteSpace(request.PrescriberId) && !string.IsNullOrWhiteSpace(request.PrescriberIdQualifier);

                bool lastNameSearch = !string.IsNullOrWhiteSpace(request.LastName) &&
                                      (
                                          request.LastNameOperator == null ||
                                          Enum.IsDefined(typeof(Enums.SearchTypeOperator), (int) char.ToUpper(request.LastNameOperator[0]))
                                      ) &&
                                      (
                                        (string.IsNullOrWhiteSpace(request.FirstName) && string.IsNullOrWhiteSpace(request.FirstNameOperator)) ||
                                        (!string.IsNullOrWhiteSpace(request.FirstName) &&
                                         (
                                             request.FirstNameOperator == null ||
                                             Enum.IsDefined(typeof(Enums.SearchTypeOperator), (int)char.ToUpper(request.FirstNameOperator[0]))
                                         )
                                        )
                                      );

                //XOR, must be one of these but can't be both
                valid = (prescriberIdSearch && string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.LastNameOperator) && string.IsNullOrWhiteSpace(request.FirstName) && string.IsNullOrWhiteSpace(request.FirstNameOperator))
                        ^ (lastNameSearch && string.IsNullOrWhiteSpace(request.PrescriberId) && string.IsNullOrWhiteSpace(request.PrescriberIdQualifier));
            }
            catch (Exception) { }

            return valid;
        }
    }
}
