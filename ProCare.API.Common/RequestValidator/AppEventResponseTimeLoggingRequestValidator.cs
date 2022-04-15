using ProCare.API.Core.Helpers;
using ProCare.API.Core.Requests;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.Common.RequestValidator
{
    public class AppEventResponseTimeLoggingRequestValidator: AbstractValidator<AppEventResponseTimeLoggingRequest>
    {
        public AppEventResponseTimeLoggingRequestValidator()
        {
            // required fields
            RuleFor(x => x.ApplicationSourceID).NotEmpty().WithMessage(ValidationHelper.GetFieldRequiredMessage("ApplicationSourceID"));
            RuleFor(x => x.ApplicationEventTypeID).NotEmpty().WithMessage(ValidationHelper.GetFieldRequiredMessage("ApplicationEventTypeID"));
            RuleFor(x => x.ApplicationEventName).NotEmpty().WithMessage(ValidationHelper.GetFieldRequiredMessage("ApplicationEventName"));
            RuleFor(x => x.ApplicationFeatureName).NotEmpty().WithMessage(ValidationHelper.GetFieldRequiredMessage("ApplicationFeatureName"));
            RuleFor(x => x.ResponseTime).NotEmpty().WithMessage(ValidationHelper.GetFieldRequiredMessage("ResponseTime"));

            // content rules
            RuleFor(x => x.ApplicationSourceID).Must(x => ValidationHelper.IsEnumValueDefined<Enums.ApplicationSource>(x)).WithMessage("Invalid ApplicationSourceID");
            RuleFor(x => x.ApplicationEventTypeID).Must(x => ValidationHelper.IsEnumValueDefined<Enums.ApplicationEventType>(x)).WithMessage("Invalid ApplicationEventTypeID");

            // length rules
            RuleFor(x => x.ApplicationEventName).MaximumLength(100);
            RuleFor(x => x.ApplicationFeatureName).MaximumLength(100);
            RuleFor(x => x.MiscField1).MaximumLength(100);
            RuleFor(x => x.MiscField2).MaximumLength(100);
            RuleFor(x => x.MiscField3).MaximumLength(100);
        }
    }
}
