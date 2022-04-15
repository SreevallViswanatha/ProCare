using System;
using System.Collections.Generic;
using System.Globalization;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class MemberMedicationSaveRequestValidator : MemberPortalTokenValidator<MemberMedicationSaveRequest>
    {
        public MemberMedicationSaveRequestValidator()
        {
            //required fields

            RuleFor(x => x).Must(ValidateRequestForFavouriteMedicationWhenTypeSavedMedication).WithMessage("EntityIdentifier is required when MemberMedicationTypeID = 1 to store NDC identifier of a medication");
            RuleFor(x => x).Must(ValidateRequestForFavouriteMedicationWhenTypeAddedByMember).WithMessage("EntityIdentifier is required when MemberMedicationTypeID = 2 t store custom favorite medication that was created by the user");

        }

        private bool ValidateRequestForFavouriteMedicationWhenTypeSavedMedication(MemberMedicationSaveRequest request)
        {
            bool valid = true;
            if (!Enum.IsDefined(typeof(Messages.Shared.Enums.MemberMedicationType), request.MemberMedicationTypeID))
            {
                valid = false;
            }

            if (request.MemberMedicationTypeID == 1 && valid)
            {
                valid = request.EntityIdentifier > 0;
            }
            return valid;
        }

        private bool ValidateRequestForFavouriteMedicationWhenTypeAddedByMember(MemberMedicationSaveRequest request)
        {
            bool valid = true;
            if (!Enum.IsDefined(typeof(Messages.Shared.Enums.MemberMedicationType), request.MemberMedicationTypeID))
            {
                valid = false;
            }

            if (request.MemberMedicationTypeID == 2 && valid)
            {
                valid = !string.IsNullOrEmpty(request.MedicationName);
            }

            return valid;
        }
    }
}
