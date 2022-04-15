using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Shared;
using ServiceStack.FluentValidation;
using System;
using System.Globalization;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberSearchRequestValidator : AbstractValidator<MemberSearchRequest>
    {
        public MemberSearchRequestValidator()
        {
            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0).LessThan(101);
            RuleFor(x => x.MemberIdOperator).Length(1);
            RuleFor(x => x.LastNameOperator).Length(1);
            RuleFor(x => x.FirstNameOperator).Length(1);
            RuleFor(x => x.MemberId).MinimumLength(3).MaximumLength(18);
            RuleFor(x => x.LastName).MinimumLength(3).MaximumLength(25);
            RuleFor(x => x.FirstName).MinimumLength(3).MaximumLength(15);
            RuleFor(x => x.DateOfBirth).Must(validDOB).WithMessage("Date Of Birth may only be used alongside a Name search and must be in YYYYMMDD format.");
            RuleFor(x => x).Must(validParamCombo).WithMessage("Request must contain a valid MemberId or LastName.  If submitting both MemberId and LastName, FirstName and DateOfBirth must be submitted as well.  If passing an Operator, valid values are S (Starts With) and E (Ends With).");
        }

        private bool validDOB(string dob)
        {
            bool valid = false;

            if (!string.IsNullOrWhiteSpace(dob))
            {
                //must be YYYYMMDD format if passed
                try
                {
                    DateTime.TryParseExact(dob, "yyyyMMdd", null, DateTimeStyles.None, out DateTime value);
                    valid = value != DateTime.MinValue;
                }
                catch (Exception) { }
            }
            else
            {
                //null is valid
                valid = true;
            }

            return valid;
        }

        private bool validParamCombo(MemberSearchRequest request)
        {
            bool valid = false;

            try
            {

                bool memberIdSearch = !string.IsNullOrWhiteSpace(request.MemberId) &&
                                      (
                                          request.MemberIdOperator == null ||
                                          Enum.IsDefined(typeof(Enums.SearchTypeOperator), (int) char.ToUpper(request.MemberIdOperator[0]))
                                      );
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

                //XOR, must be one of these but can't be multiple
                valid = (memberIdSearch && lastNameSearch && !string.IsNullOrWhiteSpace(request.FirstName) && !string.IsNullOrWhiteSpace(request.DateOfBirth))
                        ^ (memberIdSearch && string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.LastNameOperator) && string.IsNullOrWhiteSpace(request.FirstName) && string.IsNullOrWhiteSpace(request.FirstNameOperator) && string.IsNullOrWhiteSpace(request.DateOfBirth))
                        ^ (lastNameSearch && string.IsNullOrWhiteSpace(request.MemberId) && string.IsNullOrWhiteSpace(request.MemberIdOperator));
            }
            catch (Exception) { }

            return valid;
        }
    }
}
