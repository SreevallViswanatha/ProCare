using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.RequestValidator.Member
{
    public class MemberContactSaveRequestValidator : MemberPortalTokenValidator<MemberContactSaveRequest>
    {
        public MemberContactSaveRequestValidator()
        {
            //required fields
            RuleFor(x => (MemberContactType)x.MemberContactTypeID).IsInEnum().WithMessage("Invalid MemberContactTypeID.");
            RuleFor(x => x).Must(ValidateRequestForFavoriteContactAddWhenTypeSavedPharmacy).WithMessage("EntityIdentifier is required when MemberContactTypeID is 1 or 2.");
            RuleFor(x => x).Must(ValidateRequestForFavoriteContactAddWhenTypeCustomPhysicianOrPharmacy).WithMessage("ContactName along with either ContactAddress or ContactPhone number is required when MemberContactTypeID is 3.");
        }

        private bool ValidateRequestForFavoriteContactAddWhenTypeCustomPhysicianOrPharmacy(MemberContactSaveRequest arg)
        {
            bool valid = true;
            if (arg.MemberContactTypeID == (int)MemberContactType.AddedByMember && (string.IsNullOrEmpty(arg.ContactName) || (string.IsNullOrEmpty(arg.ContactPhone) && string.IsNullOrEmpty(arg.ContactAddress))))
            {                
                valid = false;
            }
            return valid;
        }
        private bool ValidateRequestForFavoriteContactAddWhenTypeSavedPharmacy(MemberContactSaveRequest arg)
        {
            bool valid = true;
            if ((arg.MemberContactTypeID == (int)MemberContactType.SavedPharmacy || arg.MemberContactTypeID == (int)MemberContactType.SavedPhysician) && (arg.EntityIdentifier ?? 0) == 0)
            {              
                valid = false;
            }
            return valid;

        }

    }
}
