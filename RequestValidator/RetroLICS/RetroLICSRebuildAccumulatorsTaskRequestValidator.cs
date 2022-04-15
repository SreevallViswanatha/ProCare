using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;
using System;
using System.Linq;

namespace ProCare.API.PBM.RequestValidator
{
    public class RetroLICSRebuildAccumulatorsTaskRequestValidator : AbstractValidator<RetroLICSRebuildAccumulatorsTaskRequest>
    {
        public RetroLICSRebuildAccumulatorsTaskRequestValidator()
        {
            //required fields
            RuleFor(x => x).Must(validDates).WithMessage("If populated, StartDate must be less than or equal to EndDate.");
        }

        private bool validDates(RetroLICSRebuildAccumulatorsTaskRequest request)
        {
            bool valid = false;

            try
            {
                valid = request.Enrollees.All(x => (
                                                       !x.EndDate.HasValue &&
                                                       (
                                                           !x.StartDate.HasValue ||
                                                            x.StartDate <= DateTime.Today
                                                       )
                                                   ) ||
                                                   x.StartDate <= x.EndDate);
            }
            catch (Exception) { }

            return valid;
        }
    }
}
