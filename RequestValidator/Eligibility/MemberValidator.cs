using ProCare.API.PBM.Messages.Request.Eligibility;
using ServiceStack.FluentValidation;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProCare.API.PBM.RequestValidator
{
    public class EligibilityMemberValidator : AbstractValidator<EligibilityMember>
    {
        private List<string> cobValues = new List<string> { "C", "R", "D", "E" };
        private List<string> personValues = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
        private List<int> relationshipValues = new List<int> { 1, 2, 3, 4 };
        private List<string> genderValues = new List<string> { "M", "F", "U" };
        private List<string> coverageTypeValues = new List<string> { "01", "03", "06", "14", "15", "16", "17" };
        private List<string> stateValues = new List<string> { "AA", "AE", "AP", "AL", "AK", "AZ", "AR", "AS", "CA", "CO", "CT", "DE", "DC", "FM", "FL", "GA", "GU", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MH", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "MP", "OH", "OK", "OR", "PW", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "VI", "WA", "WV", "WI", "WY", "AB", "BC", "MB", "NB", "NF", "NS", "NT", "NU", "ON", "PE", "QC", "SK", "YT" };
        private List<string> cutOffMethodValues = new List<string> { "C", "S", "R" };
        private List<string> allowGovernmentValues = new List<string>{ "TRUE", "FALSE", "0", "1"};

        public EligibilityMemberValidator()
        {
            //required fields
            RuleFor(x => x.PlanID).NotEmpty();
            RuleFor(x => x.CardID).NotEmpty();
            RuleFor(x => x.Person).NotEmpty().Must(x => personValues.Contains(x)).WithMessage(string.Format("Person must be a valid person code ({0})", string.Join(", ", personValues)));
            RuleFor(x => x.Relationship).NotEmpty().Must(x => relationshipValues.Contains(x)).WithMessage(string.Format("Relationship must be a valid relationship code ({0})", string.Join(", ", relationshipValues)));
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(12);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(16);  
            RuleFor(x => x.MiddleName).MaximumLength(1);
            RuleFor(x => x.AddressLine1).NotEmpty();
            RuleFor(x => x.City).NotEmpty().MaximumLength(17);
            RuleFor(x => x.State).Must(x => stateValues.Contains(x)).WithMessage(string.Format("State must be a valid state code ({0})", string.Join(", ", stateValues)));
            RuleFor(x => x.Zip).Length(5);
            RuleFor(x => x.Phone).Length(13).Must(ValidPhoneNumberFormat).WithMessage("Phone must be in (XXX)XXX-XXXX format");
            RuleFor(x => x.Gender).Length(1).Must(x => genderValues.Contains(x)).WithMessage(string.Format("Gender must be a valid gender code ({0})", string.Join(", ", genderValues)));
            RuleFor(x => x.COB).MaximumLength(1);
            RuleFor(x => x.CoverageType).NotEmpty().Must(x => coverageTypeValues.Contains(x)).WithMessage(string.Format("Coverage Type must be a valid coverage type code ({0})", string.Join(", ", coverageTypeValues)));
            RuleFor(x => x.EffectiveDate).NotEmpty();
            RuleFor(x => x.TerminationDate).NotEmpty();

            RuleFor(x => x.Zip4).Must(ValidZip4).WithMessage("Zip4 must be blank or 4 characters.");
            RuleFor(x => x.COB).Must(ValidCOB).WithMessage(string.Format("COB must be blank or a valid COB code ({0})", string.Join(", ", cobValues)));
            RuleFor(x => x.CutOffMethod).Must(ValidCutOffMethod).WithMessage(string.Format("Cut-Off Method must be blank or a valid cut-off method code ({0})", string.Join(", ", cutOffMethodValues)));
            RuleFor(x => x.AllowGovernment).Must(ValidAllowGovernment).WithMessage(string.Format("Allow Government must be blank or a valid value ({0})", string.Join(", ", allowGovernmentValues)));
        }

        private bool ValidAllowGovernment(string allowGovernment)
        {
            bool valid = true;

            if (!string.IsNullOrWhiteSpace(allowGovernment) && !allowGovernmentValues.Contains(allowGovernment.ToUpper()))
            {
                valid = false;
            }

            return valid;
        }

        private bool ValidZip4(string zip4)
        {
            bool valid = true;

            if ((!string.IsNullOrWhiteSpace(zip4)) && (zip4.Length != 0 && zip4.Length != 4))
            {
                valid = false;
            }

            return valid;
        }

        private bool ValidCOB(string cob)
        {
            bool valid = true;

            if (!string.IsNullOrWhiteSpace(cob) && !cobValues.Contains(cob))
            {
                valid = false;
            }

            return valid;
        }

        private bool ValidCutOffMethod(string cutOffMethod)
        {
            bool valid = true;

            if (!string.IsNullOrWhiteSpace(cutOffMethod) && !cutOffMethodValues.Contains(cutOffMethod))
            {
                valid = false;
            }

            return valid;
        }


        private bool ValidPhoneNumberFormat(string value)
        {
            return Regex.IsMatch(value, @"\((\d{3})\)(\d{3})-(\d{4})");
        }
    }
}
