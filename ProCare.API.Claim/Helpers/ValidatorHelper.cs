using ProCare.API.Claim.Claims;
using ProCare.NCPDP.Telecom;
using System;

namespace ProCare.API.Claims.Helpers
{
    public class ValidatorHelper
    {
        public bool CurrentVersionNumber(VersionNumber version)
        {
            return version == VersionNumber.vD0;
        }

        public bool IsValidServiceProviderIdQualifier(ServiceProviderIdQualifier idQualifier)
        {
            return idQualifier == ServiceProviderIdQualifier.NationalProviderIdentifier || idQualifier == ServiceProviderIdQualifier.NCPDPProviderID;
        }

        public bool EqualReversalClaim(TransactionCode code)
        {
            return code == TransactionCode.Reversal;
        }

        public bool EqualBillingClaim(TransactionCode code)
        {
            return code == TransactionCode.Billing;
        }

        public bool EqualEligibilityClaim(TransactionCode code)
        {
            return code == TransactionCode.EligibilityVerification;
        }

        public bool IsValidPatientRelationshipCode(PatientRelationshipCode relationship)
        {
            var result = false;

            switch (relationship)
            {
                case PatientRelationshipCode.NotSpecified:
                case PatientRelationshipCode.Cardholder:
                case PatientRelationshipCode.Child:
                case PatientRelationshipCode.Spouse:
                case PatientRelationshipCode.Other:
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool IsValidGender(PatientGender gender)
        {
            var result = false;
            switch (gender)
            {
                case PatientGender.Female:
                case PatientGender.Male:
                case PatientGender.NotSpecified:
                {
                    result = true;
                    break;
                }
            }

            return result;

        }

        public bool IsValidPrescriptionQualifier(string qualifier)
        {
            var result = false;
            switch (qualifier)
            {
                case "1":
                case "3":
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool IsValidOriginCode(char code)
        {
            var result = false;
            switch (code)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                {
                    result = true;
                    break;
                }

            }

            return result;
        }

        public bool IsValidVersionNumber(VersionNumber version)
        {
            var result = false;
            switch (version)
            {
                case VersionNumber.v51:
                case VersionNumber.v55:
                case VersionNumber.vD0:
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool IsValidPlaceOfService(PlaceOfService place)
        {
            var result = false;
            switch (place)
            {
                case PlaceOfService.AcuteCareFascility:
                case PlaceOfService.BoardingHome:
                case PlaceOfService.Home:
                case PlaceOfService.Hospice:
                case PlaceOfService.InterCare:
                case PlaceOfService.LongTerm:
                case PlaceOfService.NotSpecified:
                case PlaceOfService.NursingHome:
                case PlaceOfService.Outpatient:
                case PlaceOfService.RestHome:
                case PlaceOfService.SkilledCareFacility:
                case PlaceOfService.SubAcuteCareFacility:

                {
                    result = true;
                    break;
                }
            }

            return result;

        }

        public bool IsValidPatientResidence(PatientResidence residence)
        {
            var result = false;
            switch (residence)
            {
                case PatientResidence.AssistedLivingFacility:
                case PatientResidence.CorrectionalInstitution:
                case PatientResidence.CustodialCareFacility:
                case PatientResidence.GroupHome_PartBOnly:
                case PatientResidence.Home:
                case PatientResidence.Hospice:
                case PatientResidence.IntermediateCareFacility_MentallyRetarded:
                case PatientResidence.NotSpecified:
                case PatientResidence.NursingFacility:
                case PatientResidence.SkilledNursingFacility_PartBOnly:
                {
                    result = true;
                    break;
                }
            }

            return result;

        }

        public bool IsValidEligibilityClarificationCode(EligibilityClarificationCode code)
        {
            var result = false;
            switch (code)
            {
                case EligibilityClarificationCode.DependentParent:
                case EligibilityClarificationCode.DisabledDependent:
                case EligibilityClarificationCode.FullTimeStudent:
                case EligibilityClarificationCode.NoOverride:
                case EligibilityClarificationCode.NotSpecified:
                case EligibilityClarificationCode.Override:
                case EligibilityClarificationCode.SignificantOther:
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool EqualNpiPrescriptionQualifier(string Id)
        {
            var result = false;
            switch (Id)
            {
                case "1":
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool EqualNotSpecifiedPatientRelationship(PatientRelationshipCode code)
        {
            return code == PatientRelationshipCode.NotSpecified;
        }

        public bool EqualNotSpecifiedEligibilityClarificationCode(EligibilityClarificationCode code)
        {
            return code == EligibilityClarificationCode.NotSpecified;
        }

        public bool EqualNotSpecifiedCompoundCode(CompoundCode code)
        {
            return code == CompoundCode.NotSpecified;
        }

        public bool EqualPatientResidenceHome(PatientResidence code)
        {
            return code == PatientResidence.Home;
        }

        public bool MustEqualZero(int? value)
        {
            return value == 0;
        }
        public bool MustEqualZero(decimal? value)
        {
            return value == 0;
        }

        public static string AlphanumericCharactersOnly(string value)
        {
            char[] arr = value.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c))));
            return new string(arr);
        }

        public static string TrimAtFirstSpace(string value)
        {
            int index = value.Trim().IndexOf(" ");
            return index > 0 ? value.Substring(0, index) : value;
        }

        public static string FormatReferenceStringSet(string value)
        {
            return AlphanumericCharactersOnly(TrimAtFirstSpace(value)).ToUpper();
        }

        public static string FormatSwitcherResponseForReferenceStringSetCompare(string value, ReferenceStringSet referenceStringSet)
        {
            string formattedString = value.ToUpper();
            int index = value.Trim().IndexOf(referenceStringSet.VersionNumber);
            index = index - 4; //Transaction ID length
            index = index < 0 ? 0 : index;

            return AlphanumericCharactersOnly(referenceStringSet.BinNumber + formattedString.Substring(index));
        }
    }

}
