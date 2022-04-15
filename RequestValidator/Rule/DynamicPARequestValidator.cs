using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ProCare.API.PBM.RequestValidator
{
    public class DynamicPARequestValidator : AbstractValidator<DynamicPARequest>
    {
        public DynamicPARequestValidator()
        {
            List<string> validPharmacyIncludeExcludes = new List<string>{"I", "E"};

            //required fields
            RuleFor(x => x.ClientGuid).NotEmpty();
            RuleFor(x => x.Client).NotEmpty().MaximumLength(8);
            RuleFor(x => x.MemberID).NotEmpty().MinimumLength(1).MaximumLength(18);
            RuleFor(x => x.OrganizationID).NotEmpty().Length(8);
            RuleFor(x => x.GroupID).NotEmpty().Length(8);
            RuleFor(x => x.PlanID).NotEmpty().Length(8);
            RuleFor(x => x.ProductIDQualifier).NotEmpty().MaximumLength(2).Must(x => x.Equals("03")).WithMessage("Invalid Product ID Qualifier");
            RuleFor(x => x.ProductID).NotEmpty().MaximumLength(11);
            RuleFor(x => x.RequestFromDate).NotEmpty().Must(validDOB).WithMessage("Request From Date must be a valid date in YYYYMMDD format");
            RuleFor(x => x.VendorPANumber).NotEmpty().MaximumLength(11);
            RuleFor(x => x.DynamicPACode2).NotEmpty().MaximumLength(11);

            RuleFor(x => x.RequestFromDate).Must(validDOB).WithMessage("Request To Date must be a valid date in YYYYMMDD format");
            RuleFor(x => x.MemberEnrolleeID).Length(8);
            RuleFor(x => x.Note).MaximumLength(40);
            RuleFor(x => x.DaysSupplyMaximum).Must(x => validateNullableInt(x, 0, 999)).WithMessage("Days Supply Maximum must be null or a valid integer between 0 and 999.");
            RuleFor(x => x.PatientAgeMaximum).Must(x => validateNullableInt(x, 0, 99)).WithMessage("Patient Age Maximum must be null or a valid integer between 0 and 99.");
            RuleFor(x => x.PeriodQuantityDays).Must(x => validateNullableInt(x, 0, 999)).WithMessage("Period Quantity Days must be null or a valid integer between 0 and 999.");
            RuleFor(x => x.NumberOfFills).Must(x => validateNullableInt(x, 0, 99)).WithMessage("Number Of Fills must be null or a valid integer between 0 and 99.");
            RuleFor(x => x.PeriodQuantityMaximum).Must(x => validateNullableDouble(x, null, null)).WithMessage("Period Quantity Maximum must be null or a valid decimal.");
            RuleFor(x => x.AmountDueMaximum).Must(x => validateNullableDouble(x, null, null)).WithMessage("Amount Due Maximum must be null or a valid decimal.");
            RuleFor(x => x.PharmacyIDQualifier).MaximumLength(2).Must(x => string.IsNullOrWhiteSpace(x) || x.Equals("07")).WithMessage("Invalid Pharmacy ID Qualifier");
            RuleFor(x => x.PharmacyID).MaximumLength(8);
            RuleFor(x => x.PharmacyIncludeExclude).Length(1).Must(x => string.IsNullOrWhiteSpace(x)|| validPharmacyIncludeExcludes.Contains(x.ToUpper())).WithMessage("Pharmacy Include Exclude must be null, I, or E.");
            RuleFor(x => x).Must(validPharmacyFieldCombo).WithMessage("The Pharmacy ID and Pharmacy Include/Exclude fields must both be submitted if one is submitted.");
            RuleFor(x => x.MultisourceCode).Length(1);
            RuleFor(x => x).Must(validPeriodFieldCombo).WithMessage("The Period Quantity Days and Period Quantity Maximum fields must both be submitted if one is submitted.");
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

        private bool validPharmacyFieldCombo(DynamicPARequest request)
        {
            return string.IsNullOrWhiteSpace(request.PharmacyID) == string.IsNullOrWhiteSpace(request.PharmacyIncludeExclude);
        }

        private bool validPeriodFieldCombo(DynamicPARequest request)
        {
            return string.IsNullOrWhiteSpace(request.PeriodQuantityDays) == string.IsNullOrWhiteSpace(request.PeriodQuantityMaximum);
        }

        private bool validateNullableInt(string value, int? minValid, int? maxValid)
        {
            int parsedValue = 0;
            bool valid = string.IsNullOrWhiteSpace(value) || int.TryParse(value, out parsedValue);

            if (valid && minValid.HasValue)
            {
                valid = parsedValue >= minValid.Value;
            }

            if (valid && maxValid.HasValue)
            {
                valid = parsedValue <= maxValid.Value;
            }

            return valid;
        }

        private bool validateNullableDouble(string value, double? minValid, double? maxValid)
        {
            double parsedValue = 0;
            bool valid = string.IsNullOrWhiteSpace(value) || double.TryParse(value, out parsedValue);

            if (valid && minValid.HasValue)
            {
                valid = parsedValue >= minValid.Value;
            }

            if (valid && maxValid.HasValue)
            {
                valid = parsedValue <= maxValid.Value;
            }

            return valid;
        }
    }
}
