using ProCare.API.Claims.Messages.Request;
using ProCare.API.Claims.Messages.Response;
using ProCare.Common.Data;
using ProCare.NCPDP.Telecom;
using ProCare.NCPDP.Telecom.Extensions;
using System.Collections.Generic;

namespace ProCare.API.PBM.Helpers
{
    public class ClaimHelper
    {
        public IDataAccessHelper DataHelper { get; set; }
        public ClaimSubmissionRequest ClaimRequest { get; set; }
        public ClaimReversalRequest ReversalRequest { get; set; }
        public ClaimEligibilityRequest EligibilityRequest { get; set; }
        public Transmission Ncpdp { get; set; }
        public ClaimSubmissionResponse ClaimResponse { get; set; }
        public ClaimReversalResponse ReversalResponse { get; set; }
        public ClaimEligibilityResponse EligibilityResponse { get; set; }
        public TransmissionResponse NcpdpResponse { get; set; }

        private static List<string> CareMarkAnsiBinNumbers = new List<string> { "004336", "610415", "610239", "610029", "610468", "006144", "004245", "610449", "610474", "603604", "601475", "013303", "610477", "610473", "007093", "012189", "014046", "610130", "KD_WIN" };
        private static List<string> ExpressScriptsAnsiBinNumbers = new List<string> { "003858" };
        private static List<string> IlliniosMedicaidAnsiBinNumbers = new List<string> { "017804", "008259", "610241" };
        private static List<string> IlliniosMedicaidProcessorControlNumbers = new List<string> { "MHPILMCD" };

        public bool IsCareMark(string binNumber)
        {
            return CareMarkAnsiBinNumbers.Contains(binNumber);
        }

        public bool IsExpressScripts(string binNumber)
        {
            return ExpressScriptsAnsiBinNumbers.Contains(binNumber);
        }

        public bool IsIllinoisMedicaid(string binNumber, string processorControlNumber)
        {
            bool flag;
            if ((binNumber != "017804" && binNumber != "008259"))
            {
                flag = ((binNumber != "610241" ? true : processorControlNumber != "MHPILMCD") ? false : true);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        public Transmission MapClaimEligibilityRequestClaimToNcpdp(ClaimEligibilityRequest request)
        {
            EligibilityRequest = request;
            Ncpdp = new Transmission();
            MapEligibilityHeader();
            MapEligibilityInsurance();
            MapEligibilityPatient();
            if (IsIllinoisMedicaid(EligibilityRequest.Header.BinNumber, EligibilityRequest.Header.ProcessorControlNumber))
            {
                SetIllinoisMedicaidHardCodes(Ncpdp);
            }
            if (IsCareMark(EligibilityRequest.Header.BinNumber))
            {
                SetCareMarkHardCodes(Ncpdp);
            }
            if (IsExpressScripts(EligibilityRequest.Header.BinNumber))
            {
                SetExpressScriptsHardCodes(Ncpdp);
            }
            return Ncpdp;
        }

        public ClaimEligibilityResponse MapClaimEligibilityResponseFromNcpdp(TransmissionResponse response)
        {
            NcpdpResponse = response;
            EligibilityResponse = new ClaimEligibilityResponse();
            MapResponseHeader<ClaimEligibilityResponse>();
            MapResponseMessage<ClaimEligibilityResponse>();
            MapResponseStatus<ClaimEligibilityResponse>();
            MapResponseCOB<ClaimEligibilityResponse>();
            MapResponsePatient<ClaimEligibilityResponse>();
            MapResponseInsurance<ClaimEligibilityResponse>();
            return EligibilityResponse;
        }

        public void MapClaimRequestAdditionalDocumentation()
        {
            Ncpdp.CurrentTransaction.AdditionalDocumentation.AdditionalDocumentationTypeId = ClaimRequest.AdditionalDocumentation.AdditionalDocumentationTypeId;
            Ncpdp.CurrentTransaction.AdditionalDocumentation.LengthOfNeed = ClaimRequest.AdditionalDocumentation.LengthOfNeed;
            Ncpdp.CurrentTransaction.AdditionalDocumentation.LengthOfNeedQualifier = ClaimRequest.AdditionalDocumentation.LengthOfNeedQualifier;
            Ncpdp.CurrentTransaction.AdditionalDocumentation.PrescriberDateSigned = ClaimRequest.AdditionalDocumentation.PrescriberDateSigned;
            Ncpdp.CurrentTransaction.AdditionalDocumentation.RequestPeriodBegin = ClaimRequest.AdditionalDocumentation.RequestPeriodBegin;
            Ncpdp.CurrentTransaction.AdditionalDocumentation.RequestPeriodRecertDate = ClaimRequest.AdditionalDocumentation.RequestPeriodRecertDate;
            Ncpdp.CurrentTransaction.AdditionalDocumentation.RequestStatus = ClaimRequest.AdditionalDocumentation.RequestStatus;
            Ncpdp.CurrentTransaction.AdditionalDocumentation.SupportingDocumentation = ClaimRequest.AdditionalDocumentation.SupportingDocumentation;
            MapClaimRequestAdditionalDocumentationQuestions();
        }

        private void MapClaimRequestAdditionalDocumentationQuestions()
        {
            foreach (NCPDP.Telecom.Request.DocumentationQuestions question in ClaimRequest.AdditionalDocumentation.Questions)
            {
                NCPDP.Telecom.Request.DocumentationQuestions documentationQuestion = new NCPDP.Telecom.Request.DocumentationQuestions()
                {
                    AlphaResponse = question.AlphaResponse,
                    DateResponse = question.DateResponse,
                    DollarResponse = question.DollarResponse,
                    NumericResponse = question.NumericResponse,
                    PercentResponse = question.PercentResponse,
                    QuestionNumber = question.QuestionNumber
                };
                Ncpdp.CurrentTransaction.AdditionalDocumentation.Questions.Add(documentationQuestion);
            }
        }

        public void MapClaimRequestClaim()
        {
            Ncpdp.CurrentTransaction.Claim.AlternateId = ClaimRequest.Claim.AlternateId;
            Ncpdp.CurrentTransaction.Claim.AssociatedPrescriptionDate = ClaimRequest.Claim.AssociatedPrescriptionDate;
            Ncpdp.CurrentTransaction.Claim.AssociatedPrescriptionNumber = ClaimRequest.Claim.AssociatedPrescriptionNumber;
            Ncpdp.CurrentTransaction.Claim.CompoundCode = ClaimRequest.Claim.CompoundCode;
            Ncpdp.CurrentTransaction.Claim.CompoundType = ClaimRequest.Claim.CompoundType;
            Ncpdp.CurrentTransaction.Claim.DatePrescriptionWritten = ClaimRequest.Claim.DatePrescriptionWritten;
            Ncpdp.CurrentTransaction.Claim.DaysSupply = ClaimRequest.Claim.DaysSupply;
            Ncpdp.CurrentTransaction.Claim.DaysSupplyIntendedToBeDispensed = ClaimRequest.Claim.DaysSupplyIntendedToBeDispensed;
            Ncpdp.CurrentTransaction.Claim.DelayReasonCode = ClaimRequest.Claim.DelayReasonCode;
            Ncpdp.CurrentTransaction.Claim.DispenseAsWritten = ClaimRequest.Claim.DispenseAsWritten;
            Ncpdp.CurrentTransaction.Claim.DispensingStatus = ClaimRequest.Claim.DispensingStatus;
            Ncpdp.CurrentTransaction.Claim.FillNumber = ClaimRequest.Claim.FillNumber;
            Ncpdp.CurrentTransaction.Claim.IntermediaryAuthoriationId = ClaimRequest.Claim.IntermediaryAuthoriationId;
            Ncpdp.CurrentTransaction.Claim.IntermediaryAuthorizationTypeId = ClaimRequest.Claim.IntermediaryAuthorizationTypeId;
            Ncpdp.CurrentTransaction.Claim.LevelOfService = ClaimRequest.Claim.LevelOfService;
            Ncpdp.CurrentTransaction.Claim.MedicaidICN = ClaimRequest.Claim.MedicaidICN;
            Ncpdp.CurrentTransaction.Claim.OriginallyPrescribedProductId = ClaimRequest.Claim.OriginallyPrescribedProductId;
            Ncpdp.CurrentTransaction.Claim.OriginallyPrescribedProductIdQualifier = ClaimRequest.Claim.OriginallyPrescribedProductIdQualifier;
            Ncpdp.CurrentTransaction.Claim.OriginallyPrescribedQuantity = ClaimRequest.Claim.OriginallyPrescribedQuantity;
            Ncpdp.CurrentTransaction.Claim.OtherCoverageCode = ClaimRequest.Claim.OtherCoverageCode;
            Ncpdp.CurrentTransaction.Claim.PatientAssignmentIndicator = ClaimRequest.Claim.PatientAssignmentIndicator;
            Ncpdp.CurrentTransaction.Claim.PharmacyServiceType = ClaimRequest.Claim.PharmacyServiceType;
            Ncpdp.CurrentTransaction.Claim.PrescriptionNumber = ClaimRequest.Claim.PrescriptionNumber;
            Ncpdp.CurrentTransaction.Claim.PrescriptionOriginCode = ClaimRequest.Claim.PrescriptionOriginCode;
            Ncpdp.CurrentTransaction.Claim.PrescriptionQualifier = ClaimRequest.Claim.PrescriptionQualifier;
            Ncpdp.CurrentTransaction.Claim.PriorAuthorizationNumberSubmitted = ClaimRequest.Claim.PriorAuthorizationNumberSubmitted;
            Ncpdp.CurrentTransaction.Claim.PriorAuthorizationTypeCode = ClaimRequest.Claim.PriorAuthorizationTypeCode;
            Ncpdp.CurrentTransaction.Claim.ProductId = ClaimRequest.Claim.ProductId;
            Ncpdp.CurrentTransaction.Claim.ProductIdQualifier = ClaimRequest.Claim.ProductIdQualifier;
            Ncpdp.CurrentTransaction.Claim.QuantityDispensed = ClaimRequest.Claim.QuantityDispensed;
            Ncpdp.CurrentTransaction.Claim.QuantityIntendedToBeDispensed = ClaimRequest.Claim.QuantityIntendedToBeDispensed;
            Ncpdp.CurrentTransaction.Claim.QuantityPrescribed = ClaimRequest.Claim.QuantityPrescribed;
            Ncpdp.CurrentTransaction.Claim.RefillsAuthorized = ClaimRequest.Claim.RefillsAuthorized;
            Ncpdp.CurrentTransaction.Claim.RouteOfAdministration = ClaimRequest.Claim.RouteOfAdministration;
            Ncpdp.CurrentTransaction.Claim.ScheduledPrescriptionIdNumber = ClaimRequest.Claim.ScheduledPrescriptionIdNumber;
            Ncpdp.CurrentTransaction.Claim.SpecialPackagingIndicator = ClaimRequest.Claim.SpecialPackagingIndicator;
            Ncpdp.CurrentTransaction.Claim.SubmissionClarificationCodes = ClaimRequest.Claim.SubmissionClarificationCodes;
            Ncpdp.CurrentTransaction.Claim.TransactionReferenceNumber = ClaimRequest.Claim.TransactionReferenceNumber;
            Ncpdp.CurrentTransaction.Claim.UnitOfMeasure = ClaimRequest.Claim.UnitOfMeasure;
        }

        public void MapClaimRequestClinical()
        {
            MapClinicalDiagnosisCodes();
            MapClinicalMeasurements();
        }

        public void MapClaimRequestCompound()
        {
            Ncpdp.CurrentTransaction.Compound.DispensingUnitFormIndicator = ClaimRequest.Compound.DispensingUnitFormIndicator;
            Ncpdp.CurrentTransaction.Compound.DosageFormDescriptionCode = ClaimRequest.Compound.DosageFormDescriptionCode;
            MapCompoundIngredients();
            MapCompoundModifierCodes();
            Ncpdp.CurrentTransaction.Compound.RouteOfAdministration = ClaimRequest.Compound.RouteOfAdministration;
        }

        private void MapClaimRequestCoordinationOfBenefits()
        {
            MapCOBBenefitStages();
            MapCOBOtherPayerAmountsPaid();
            MapCOBOtherPayerResponsibilty();
            MapCOBOtherPayerRejectCodes();
            MapCOBOtherPayers();
        }

        public void MapClaimRequestCoupon()
        {
            Ncpdp.CurrentTransaction.Coupon.CouponNumber = ClaimRequest.Coupon.CouponNumber;
            Ncpdp.CurrentTransaction.Coupon.CouponType = ClaimRequest.Coupon.CouponType;
            Ncpdp.CurrentTransaction.Coupon.CouponValueAmount = ClaimRequest.Coupon.CouponValueAmount;
        }

        public void MapClaimRequestDrugUtilizationReview()
        {
            MapDURServices();
        }

        public void MapClaimRequestFacility()
        {
            Ncpdp.CurrentTransaction.Facility.State = ClaimRequest.Facility.State;
            Ncpdp.CurrentTransaction.Facility.City = ClaimRequest.Facility.City;
            Ncpdp.CurrentTransaction.Facility.FacilityId = ClaimRequest.Facility.FacilityId;
            Ncpdp.CurrentTransaction.Facility.FacilityName = ClaimRequest.Facility.FacilityName;
            Ncpdp.CurrentTransaction.Facility.Street = ClaimRequest.Facility.Street;
            Ncpdp.CurrentTransaction.Facility.Zip = ClaimRequest.Facility.Zip;
        }

        public void MapClaimRequestHeader()
        {
            Ncpdp.Header.BinNumber = ClaimRequest.Header.BinNumber;
            Ncpdp.Header.VersionNumber = ClaimRequest.Header.VersionNumber;
            Ncpdp.Header.DateOfService = ClaimRequest.Header.DateOfService;
            Ncpdp.Header.ProcessorControlNumber = ClaimRequest.Header.ProcessorControlNumber;
            Ncpdp.Header.ServiceProviderId = ClaimRequest.Header.ServiceProviderId;
            Ncpdp.Header.ServiceProviderIdQualifier = ClaimRequest.Header.ServiceProviderIdQualifier;
            Ncpdp.Header.TransactionCode = ClaimRequest.Header.TransactionCode;
            Ncpdp.Header.TransactionCount = ClaimRequest.Header.TransactionCount;
            Ncpdp.Header.SoftwareId = ClaimRequest.Header.SoftwareId; //GetSoftwareIdByAnsiBin(ClaimRequest.Header.BinNumber, ClaimRequest.Header.ProcessorControlNumber);
        }

        public void MapClaimRequestInsurance()
        {
            Ncpdp.Insurance.CardholderFirstName = ClaimRequest.Insurance.CardholderFirstName;
            Ncpdp.Insurance.CardholderId = ClaimRequest.Insurance.CardholderId;
            Ncpdp.Insurance.CardholderLastName = ClaimRequest.Insurance.CardholderLastName;
            Ncpdp.Insurance.CmsPartDDefinedQualifiedFacility = ClaimRequest.Insurance.CmsPartDDefinedQualifiedFacility;
            Ncpdp.Insurance.EligibilityClarificationCode = ClaimRequest.Insurance.EligibilityClarificationCode;
            Ncpdp.Insurance.GroupId = ClaimRequest.Insurance.GroupId;
            Ncpdp.Insurance.HomePlan = ClaimRequest.Insurance.HomePlan;
            Ncpdp.Insurance.MedicaidAgencyNumber = ClaimRequest.Insurance.MedicaidAgencyNumber;
            Ncpdp.Insurance.MedicaidIdNumber = ClaimRequest.Insurance.MedicaidIdNumber;
            Ncpdp.Insurance.MedicaidIndicator = ClaimRequest.Insurance.MedicaidIndicator;
            Ncpdp.Insurance.MedigapId = ClaimRequest.Insurance.MedigapId;
            Ncpdp.Insurance.OtherPayerBinNumber = ClaimRequest.Insurance.OtherPayerBinNumber;
            Ncpdp.Insurance.OtherPayerCardholderId = ClaimRequest.Insurance.OtherPayerCardholderId;
            Ncpdp.Insurance.OtherPayerGroupId = ClaimRequest.Insurance.OtherPayerGroupId;
            Ncpdp.Insurance.OtherPayerProcessorControlNumber = ClaimRequest.Insurance.OtherPayerProcessorControlNumber;
            Ncpdp.Insurance.PatientRelationshipCode = ClaimRequest.Insurance.PatientRelationshipCode;
            Ncpdp.Insurance.PersonCode = ClaimRequest.Insurance.PersonCode;
            Ncpdp.Insurance.PlanId = ClaimRequest.Insurance.PlanId;
            Ncpdp.Insurance.ProviderAcceptAssignmentIndicator = ClaimRequest.Insurance.ProviderAcceptAssignmentIndicator;
        }

        public void MapClaimRequestPatient()
        {
            Ncpdp.Patient.City = ClaimRequest.Patient.City;
            Ncpdp.Patient.DateOfBirth = ClaimRequest.Patient.DateOfBirth;
            Ncpdp.Patient.EmailAddress = ClaimRequest.Patient.EmailAddress;
            Ncpdp.Patient.EmployerId = ClaimRequest.Patient.EmployerId;
            Ncpdp.Patient.FirstName = ClaimRequest.Patient.FirstName;
            Ncpdp.Patient.Gender = ClaimRequest.Patient.Gender;
            Ncpdp.Patient.LastName = ClaimRequest.Patient.LastName;
            Ncpdp.Patient.PatientId = ClaimRequest.Patient.PatientId;
            Ncpdp.Patient.Phone = ClaimRequest.Patient.Phone;
            Ncpdp.Patient.PlaceOfService = ClaimRequest.Patient.PlaceOfService;
            Ncpdp.Patient.PregnancyIndicator = Ncpdp.Patient.PregnancyIndicator.Parse<PregnancyIndicator>(ClaimRequest.Patient.PregnancyIndicator.ToOutputString(), PregnancyIndicator.NotSpecified);
            Ncpdp.Patient.Residence = Ncpdp.Patient.Residence.Parse<PatientResidence>(ClaimRequest.Patient.Residence.ToOutputString(), PatientResidence.NotSpecified);
            Ncpdp.Patient.SmokerCode = ClaimRequest.Patient.SmokerCode;
            Ncpdp.Patient.State = ClaimRequest.Patient.State;
            Ncpdp.Patient.Street = ClaimRequest.Patient.Street;
            Ncpdp.Patient.Zip = ClaimRequest.Patient.Zip;
            Ncpdp.Patient.PatientIdQualifier = ClaimRequest.Patient.PatientIdQualifier;
        }

        public void MapClaimRequestPharmacy()
        {
            Ncpdp.CurrentTransaction.Pharmacy.ProviderId = ClaimRequest.Pharmacy.ProviderId;
            Ncpdp.CurrentTransaction.Pharmacy.ProviderIdQualifier = Ncpdp.CurrentTransaction.Pharmacy.ProviderIdQualifier.Parse<ProviderIdQualifier>(ClaimRequest.Pharmacy.ProviderIdQualifier.ToOutputString(), ProviderIdQualifier.NotSpecified);
        }

        public void MapClaimRequestPrescriber()
        {
            Ncpdp.CurrentTransaction.Prescriber.State = ClaimRequest.Prescriber.State;
            Ncpdp.CurrentTransaction.Prescriber.City = ClaimRequest.Prescriber.City;
            Ncpdp.CurrentTransaction.Prescriber.FirstName = ClaimRequest.Prescriber.FirstName;
            Ncpdp.CurrentTransaction.Prescriber.LastName = ClaimRequest.Prescriber.LastName;
            Ncpdp.CurrentTransaction.Prescriber.Phone = ClaimRequest.Prescriber.Phone;
            Ncpdp.CurrentTransaction.Prescriber.PrescriberId = ClaimRequest.Prescriber.PrescriberId;
            Ncpdp.CurrentTransaction.Prescriber.PrescriberIdQualifier = Ncpdp.CurrentTransaction.Prescriber.PrescriberIdQualifier.Parse<PrescriberIdQualifier>(ClaimRequest.Prescriber.PrescriberIdQualifier.ToOutputString(), PrescriberIdQualifier.NotSpecified);
            Ncpdp.CurrentTransaction.Prescriber.PrimaryCareProviderId = ClaimRequest.Prescriber.PrimaryCareProviderId;
            Ncpdp.CurrentTransaction.Prescriber.PrimaryCareProviderIdQualifier = Ncpdp.CurrentTransaction.Prescriber.PrimaryCareProviderIdQualifier.Parse<PrimaryCareProviderIdQualifier>(ClaimRequest.Prescriber.PrimaryCareProviderIdQualifier.ToOutputString(), PrimaryCareProviderIdQualifier.NotSpecified);
            Ncpdp.CurrentTransaction.Prescriber.PrimaryCareProviderLastName = ClaimRequest.Prescriber.PrimaryCareProviderLastName;
            Ncpdp.CurrentTransaction.Prescriber.Street = ClaimRequest.Prescriber.Street;
            Ncpdp.CurrentTransaction.Prescriber.Zip = ClaimRequest.Prescriber.Zip;
        }

        public void MapClaimRequestPricing()
        {
            Ncpdp.CurrentTransaction.Pricing.BasisOfCostDetermination = ClaimRequest.Pricing.BasisOfCostDetermination;
            Ncpdp.CurrentTransaction.Pricing.DispensingFeeSubmitted = ClaimRequest.Pricing.DispensingFeeSubmitted;
            Ncpdp.CurrentTransaction.Pricing.FlatSalesTaxAmountSubmitted = ClaimRequest.Pricing.FlatSalesTaxAmountSubmitted;
            Ncpdp.CurrentTransaction.Pricing.GrossAmountDue = ClaimRequest.Pricing.GrossAmountDue;
            Ncpdp.CurrentTransaction.Pricing.IncentiveAmountSubmitted = ClaimRequest.Pricing.IncentiveAmountSubmitted;
            Ncpdp.CurrentTransaction.Pricing.IngredientCostSubmitted = ClaimRequest.Pricing.IngredientCostSubmitted;
            Ncpdp.CurrentTransaction.Pricing.PatientPaidAmountSubmitted = ClaimRequest.Pricing.PatientPaidAmountSubmitted;
            Ncpdp.CurrentTransaction.Pricing.PercentageSalesTaxAmountSubmitted = ClaimRequest.Pricing.PercentageSalesTaxAmountSubmitted;
            Ncpdp.CurrentTransaction.Pricing.PercentageSalesTaxBasisSubmitted = ClaimRequest.Pricing.PercentageSalesTaxBasisSubmitted;
            Ncpdp.CurrentTransaction.Pricing.PercentageSalesTaxRateSubmitted = ClaimRequest.Pricing.PercentageSalesTaxRateSubmitted;
            Ncpdp.CurrentTransaction.Pricing.ProfessionalServiceFeeSubmitted = ClaimRequest.Pricing.ProfessionalServiceFeeSubmitted;
            Ncpdp.CurrentTransaction.Pricing.UsualAndCustomaryCharge = ClaimRequest.Pricing.UsualAndCustomaryCharge;
            MapPricingOtherAmountClaimed();
        }

        public void MapClaimRequestPriorAuth()
        {
            Ncpdp.CurrentTransaction.PriorAuth.AuthorizationNumber = ClaimRequest.PriorAuth.AuthorizationNumber;
            Ncpdp.CurrentTransaction.PriorAuth.BasisOfRequest = ClaimRequest.PriorAuth.BasisOfRequest;
            Ncpdp.CurrentTransaction.PriorAuth.City = ClaimRequest.PriorAuth.City;
            Ncpdp.CurrentTransaction.PriorAuth.FirstName = ClaimRequest.PriorAuth.FirstName;
            Ncpdp.CurrentTransaction.PriorAuth.LastName = ClaimRequest.PriorAuth.LastName;
            Ncpdp.CurrentTransaction.PriorAuth.PeriodBegin = ClaimRequest.PriorAuth.PeriodBegin;
            Ncpdp.CurrentTransaction.PriorAuth.PeriodEnd = ClaimRequest.PriorAuth.PeriodEnd;
            Ncpdp.CurrentTransaction.PriorAuth.PriorAuthorizationNumberAssigned = ClaimRequest.PriorAuth.PriorAuthorizationNumberAssigned;
            Ncpdp.CurrentTransaction.PriorAuth.RequestType = ClaimRequest.PriorAuth.RequestType;
            Ncpdp.CurrentTransaction.PriorAuth.State = ClaimRequest.PriorAuth.State;
            Ncpdp.CurrentTransaction.PriorAuth.Street = ClaimRequest.PriorAuth.Street;
            Ncpdp.CurrentTransaction.PriorAuth.SupportingDocumentation = ClaimRequest.PriorAuth.SupportingDocumentation;
        }

        public void MapClaimRequestTransactions()
        {
            MapClaimRequestClaim();
            MapClaimRequestAdditionalDocumentation();
            MapClaimRequestClinical();
            MapClaimRequestCompound();
            MapClaimRequestCoupon();
            MapClaimRequestCoordinationOfBenefits();
            MapClaimRequestDrugUtilizationReview();
            MapClaimRequestFacility();
            MapClaimRequestPharmacy();
            MapClaimRequestPrescriber();
            MapClaimRequestPricing();
            MapClaimRequestPriorAuth();
            MapClaimRequestWorkersComp();
        }

        public void MapClaimRequestWorkersComp()
        {
            Ncpdp.CurrentTransaction.WorkersComp.BillingEntityTypeIndicator = ClaimRequest.WorkersComp.BillingEntityTypeIndicator;
            Ncpdp.CurrentTransaction.WorkersComp.CarrierId = ClaimRequest.WorkersComp.CarrierId;
            Ncpdp.CurrentTransaction.WorkersComp.ClaimId = ClaimRequest.WorkersComp.ClaimId;
            Ncpdp.CurrentTransaction.WorkersComp.DateofInjury = ClaimRequest.WorkersComp.DateofInjury;
            Ncpdp.CurrentTransaction.WorkersComp.EmployerCity = ClaimRequest.WorkersComp.EmployerCity;
            Ncpdp.CurrentTransaction.WorkersComp.EmployerContact = ClaimRequest.WorkersComp.EmployerContact;
            Ncpdp.CurrentTransaction.WorkersComp.EmployerName = ClaimRequest.WorkersComp.EmployerName;
            Ncpdp.CurrentTransaction.WorkersComp.EmployerPhone = ClaimRequest.WorkersComp.EmployerPhone;
            Ncpdp.CurrentTransaction.WorkersComp.EmployerState = ClaimRequest.WorkersComp.EmployerState;
            Ncpdp.CurrentTransaction.WorkersComp.EmployerStreet = ClaimRequest.WorkersComp.EmployerStreet;
            Ncpdp.CurrentTransaction.WorkersComp.EmployerZip = ClaimRequest.WorkersComp.EmployerZip;
            Ncpdp.CurrentTransaction.WorkersComp.GenericEquivalentProductId = ClaimRequest.WorkersComp.GenericEquivalentProductId;
            Ncpdp.CurrentTransaction.WorkersComp.GenericEquivalentProductIdQualifier = ClaimRequest.WorkersComp.GenericEquivalentProductIdQualifier;
            Ncpdp.CurrentTransaction.WorkersComp.PayToCity = ClaimRequest.WorkersComp.PayToCity;
            Ncpdp.CurrentTransaction.WorkersComp.PayToId = ClaimRequest.WorkersComp.PayToId;
            Ncpdp.CurrentTransaction.WorkersComp.PayToName = ClaimRequest.WorkersComp.PayToName;
            Ncpdp.CurrentTransaction.WorkersComp.PayToQualifier = ClaimRequest.WorkersComp.PayToQualifier;
            Ncpdp.CurrentTransaction.WorkersComp.PayToState = ClaimRequest.WorkersComp.PayToState;
            Ncpdp.CurrentTransaction.WorkersComp.PayToStreet = ClaimRequest.WorkersComp.PayToStreet;
            Ncpdp.CurrentTransaction.WorkersComp.PayToZip = ClaimRequest.WorkersComp.PayToZip;
        }

        public Transmission MapClaimReversalRequestClaimToNcpdp(ClaimReversalRequest request)
        {
            ReversalRequest = request;
            Ncpdp = new Transmission();
            MapReversalRequestHeader();
            MapReversalRequestInsurance();
            MapReversalRequestClaim();
            MapReversalRequestPricing();
            MapReversalRequestCoordinationOfBenefits();
            if (IsIllinoisMedicaid(ReversalRequest.Header.BinNumber, ReversalRequest.Header.ProcessorControlNumber))
            {
                SetIllinoisMedicaidHardCodes(Ncpdp);
            }
            if (IsCareMark(ReversalRequest.Header.BinNumber))
            {
                SetCareMarkHardCodes(Ncpdp);
            }
            if (IsExpressScripts(ReversalRequest.Header.BinNumber))
            {
                SetExpressScriptsHardCodes(Ncpdp);
            }
            return Ncpdp;
        }

        public ClaimReversalResponse MapClaimReversalResponseFromNcpdp(TransmissionResponse response)
        {
            NcpdpResponse = response;
            ReversalResponse = new ClaimReversalResponse();
            MapResponseHeader<ClaimReversalResponse>();
            MapResponseMessage<ClaimReversalResponse>();
            MapResponseStatus<ClaimReversalResponse>();
            MapResponseClaim<ClaimReversalResponse>();
            MapResponseInsurance<ClaimReversalResponse>();
            return ReversalResponse;
        }

        public ClaimSubmissionResponse MapClaimSubmissionResponseFromNcpdp(TransmissionResponse response)
        {
            NcpdpResponse = response;
            ClaimResponse = new ClaimSubmissionResponse();

            MapResponseHeader<ClaimSubmissionResponse>();
            MapResponseMessage<ClaimSubmissionResponse>();
            MapResponseInsurance<ClaimSubmissionResponse>();
            MapResponsePatient<ClaimSubmissionResponse>();
            MapResponseClaim<ClaimSubmissionResponse>();
            MapResponseCOB<ClaimSubmissionResponse>();
            MapResponseDUR<ClaimSubmissionResponse>();
            MapResponsePricing<ClaimSubmissionResponse>();
            MapResponseStatus<ClaimSubmissionResponse>();

            return ClaimResponse;
        }

        private void MapClinicalDiagnosisCodes()
        {
            foreach (NCPDP.Telecom.Request.DiagnosisCodes diagnosisCode in ClaimRequest.Clinical.DiagnosisCodes)
            {
                NCPDP.Telecom.Request.DiagnosisCodes diagnosisCode1 = new NCPDP.Telecom.Request.DiagnosisCodes()
                {
                    DiagnosisCode = diagnosisCode.DiagnosisCode,
                    DiagnosisCodeQualifier = diagnosisCode.DiagnosisCodeQualifier
                };
                Ncpdp.CurrentTransaction.Clinical.DiagnosisCodes.Add(diagnosisCode1);
            }
        }

        private void MapClinicalMeasurements()
        {
            foreach (NCPDP.Telecom.Request.Measurements measurement in ClaimRequest.Clinical.Measurements)
            {
                NCPDP.Telecom.Request.Measurements measurement1 = new NCPDP.Telecom.Request.Measurements()
                {
                    MeasurementDate = measurement.MeasurementDate,
                    MeasurementDimension = measurement.MeasurementDimension,
                    MeasurementTime = measurement.MeasurementTime,
                    MeasurementUnit = measurement.MeasurementUnit,
                    MeasurementValue = measurement.MeasurementValue
                };
                Ncpdp.CurrentTransaction.Clinical.Measurements.Add(measurement1);
            }
        }

        private void MapCOBBenefitStages()
        {
            foreach (NCPDP.Telecom.Request.BenefitStage benefitStage in ClaimRequest.COB.BenefitStages)
            {
                NCPDP.Telecom.Request.BenefitStage benefitStage1 = new NCPDP.Telecom.Request.BenefitStage()
                {
                    Amount = benefitStage.Amount,
                    Qualifier = benefitStage.Qualifier
                };
                Ncpdp.CurrentTransaction.CoordinationOfBenefits.BenefitStages.Add(benefitStage1);
            }
        }

        private void MapCOBOtherPayerAmountsPaid()
        {
            foreach (NCPDP.Telecom.Request.OtherPayerAmountsPaid otherPayerAmount in ClaimRequest.COB.OtherPayerAmounts)
            {
                NCPDP.Telecom.Request.OtherPayerAmountsPaid otherPayerAmountsPaid = new NCPDP.Telecom.Request.OtherPayerAmountsPaid()
                {
                    OtherPayerAmountPaid = otherPayerAmount.OtherPayerAmountPaid,
                    OtherPayerAmountPaidQualifier = otherPayerAmount.OtherPayerAmountPaidQualifier
                };
                Ncpdp.CurrentTransaction.CoordinationOfBenefits.OtherPayerAmounts.Add(otherPayerAmountsPaid);
            }
        }

        private void MapCOBOtherPayerRejectCodes()
        {
            foreach (string otherPayerRejectCode in ClaimRequest.COB.OtherPayerRejectCodes)
            {
                Ncpdp.CurrentTransaction.CoordinationOfBenefits.OtherPayerRejectCodes.Add(otherPayerRejectCode);
            }
        }

        private void MapCOBOtherPayerResponsibilty()
        {
            foreach (NCPDP.Telecom.Request.OtherPayerPatientResponsibility otherPayerPatientResponsibility in ClaimRequest.COB.OtherPayerPatientResponsibilities)
            {
                NCPDP.Telecom.Request.OtherPayerPatientResponsibility otherPayerPatientResponsibility1 = new NCPDP.Telecom.Request.OtherPayerPatientResponsibility()
                {
                    OtherPayerPatientAmount = otherPayerPatientResponsibility.OtherPayerPatientAmount,
                    OtherPayerPatientAmountQualifier = otherPayerPatientResponsibility.OtherPayerPatientAmountQualifier
                };
                Ncpdp.CurrentTransaction.CoordinationOfBenefits.OtherPayerPatientResponsibilities.Add(otherPayerPatientResponsibility1);
            }
        }

        private void MapCOBOtherPayers()
        {
            foreach (NCPDP.Telecom.Request.OtherPayer otherPayer in ClaimRequest.COB.OtherPayers)
            {
                NCPDP.Telecom.Request.OtherPayer otherPayer1 = new NCPDP.Telecom.Request.OtherPayer()
                {
                    InternalControlNumber = otherPayer.InternalControlNumber,
                    OtherPayerCoverageType = otherPayer.OtherPayerCoverageType,
                    OtherPayerDate = otherPayer.OtherPayerDate,
                    OtherPayerId = otherPayer.OtherPayerId,
                    OtherPayerIdQualifier = otherPayer.OtherPayerIdQualifier
                };
                Ncpdp.CurrentTransaction.CoordinationOfBenefits.OtherPayers.Add(otherPayer1);
            }
        }

        private void MapCompoundIngredients()
        {
            foreach (NCPDP.Telecom.Request.CompoundProduct ingredient in ClaimRequest.Compound.Ingredients)
            {
                NCPDP.Telecom.Request.CompoundProduct compoundProduct = new NCPDP.Telecom.Request.CompoundProduct()
                {
                    BasisOfCostDetermination = ingredient.BasisOfCostDetermination,
                    Cost = ingredient.Cost,
                    ProductId = ingredient.ProductId,
                    ProductIdQualifier = ingredient.ProductIdQualifier,
                    Quantity = ingredient.Quantity
                };
                Ncpdp.CurrentTransaction.Compound.Ingredients.Add(compoundProduct);
            }
        }

        private void MapCompoundModifierCodes()
        {
            foreach (string modifierCode in ClaimRequest.Compound.ModifierCodes)
            {
                Ncpdp.CurrentTransaction.Compound.ModifierCodes.Add(modifierCode);
            }
        }

        private void MapDURServices()
        {
            foreach (NCPDP.Telecom.Request.DrugUtilizationReviewService service in ClaimRequest.DUR.Services)
            {
                NCPDP.Telecom.Request.DrugUtilizationReviewService drugUtilizationReviewService = new NCPDP.Telecom.Request.DrugUtilizationReviewService()
                {
                    CoAgentId = service.CoAgentId,
                    CoAgentIdQualifier = service.CoAgentIdQualifier,
                    LevelOfEffort = service.LevelOfEffort,
                    ProfessionalServiceCode = service.ProfessionalServiceCode,
                    ReasonForServiceCode = service.ReasonForServiceCode,
                    ResultOfServiceCode = service.ResultOfServiceCode
                };
                Ncpdp.CurrentTransaction.DrugUtilizationReview.Services.Add(drugUtilizationReviewService);
            }
        }

        public void MapEligibilityHeader()
        {
            Ncpdp.Header.BinNumber = EligibilityRequest.Header.BinNumber;
            Ncpdp.Header.VersionNumber = EligibilityRequest.Header.VersionNumber;
            Ncpdp.Header.DateOfService = EligibilityRequest.Header.DateOfService;
            Ncpdp.Header.ProcessorControlNumber = EligibilityRequest.Header.ProcessorControlNumber;
            Ncpdp.Header.ServiceProviderId = EligibilityRequest.Header.ServiceProviderId;
            Ncpdp.Header.ServiceProviderIdQualifier = EligibilityRequest.Header.ServiceProviderIdQualifier;
            Ncpdp.Header.TransactionCode = EligibilityRequest.Header.TransactionCode;
            Ncpdp.Header.TransactionCount = EligibilityRequest.Header.TransactionCount;
            Ncpdp.Header.SoftwareId = EligibilityRequest.Header.SoftwareId; //GetSoftwareIdByAnsiBin(EligibilityRequest.Header.BinNumber, EligibilityRequest.Header.ProcessorControlNumber);
        }

        public void MapEligibilityInsurance()
        {
            Ncpdp.Insurance.CardholderId = EligibilityRequest.Insurance.CardholderId;
        }

        public void MapEligibilityPatient()
        {
            if (EligibilityRequest.Patient != null)
            {
                Ncpdp.Patient.DateOfBirth = EligibilityRequest.Patient.DateOfBirth;
                Ncpdp.Patient.Gender = EligibilityRequest.Patient.Gender;
                Ncpdp.Patient.FirstName = EligibilityRequest.Patient.FirstName;
                Ncpdp.Patient.LastName = EligibilityRequest.Patient.LastName;
                Ncpdp.Patient.Zip = EligibilityRequest.Patient.Zip;
            }
        }

        private void MapPreferredProducts<T>()
        {
            List<NCPDP.Telecom.Response.PreferredProduct> preferredProducts = new List<NCPDP.Telecom.Response.PreferredProduct>();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    //Eligibility does not have Claim
                    break;
                case "ClaimSubmissionResponse":
                    preferredProducts = ClaimResponse.Claim.PreferredProducts;
                    break;
                case "ClaimReversalResponse":
                    preferredProducts = ReversalResponse.Claim.PreferredProducts;
                    break;
            }

            foreach (NCPDP.Telecom.Response.PreferredProduct preferredProduct in NcpdpResponse.CurrentTransaction.Claim.PreferredProducts)
            {
                preferredProducts.Add(new NCPDP.Telecom.Response.PreferredProduct()
                {
                    CostShareIncentive = preferredProduct.CostShareIncentive,
                    Incentive = preferredProduct.Incentive,
                    ProductDescription = preferredProduct.ProductDescription,
                    ProductId = preferredProduct.ProductId,
                    ProductIdQualifier = preferredProduct.ProductIdQualifier
                });
            }
        }

        private void MapPricingOtherAmountClaimed()
        {
            foreach (NCPDP.Telecom.Request.OtherAmountClaimed otherAmountClaimed in ClaimRequest.Pricing.OtherAmountClaimeds)
            {
                NCPDP.Telecom.Request.OtherAmountClaimed otherAmountClaimed1 = new NCPDP.Telecom.Request.OtherAmountClaimed()
                {
                    OtherAmountClaimedSubmitted = otherAmountClaimed.OtherAmountClaimedSubmitted,
                    OtherAmountClaimedSubmittedQualifier = otherAmountClaimed.OtherAmountClaimedSubmittedQualifier
                };
                Ncpdp.CurrentTransaction.Pricing.OtherAmountClaimeds.Add(otherAmountClaimed1);
            }
        }

        public void MapResponseClaim<T>()
        {
            NCPDP.Telecom.Response.ResponseClaim responseClaim = new NCPDP.Telecom.Response.ResponseClaim();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    //Eligibility does not have Claim
                    break;
                case "ClaimSubmissionResponse":
                    responseClaim = ClaimResponse.Claim;
                    break;
                case "ClaimReversalResponse":
                    responseClaim = ReversalResponse.Claim;
                    break;
            }

            responseClaim.MedicaidInternalControlNumber = NcpdpResponse.CurrentTransaction.Claim.MedicaidInternalControlNumber;
            responseClaim.ReferenceNumber = NcpdpResponse.CurrentTransaction.Claim.ReferenceNumber;
            responseClaim.ReferenceNumberQualifier = NcpdpResponse.CurrentTransaction.Claim.ReferenceNumberQualifier;
            MapPreferredProducts<T>();
        }

        public void MapResponseCOB<T>()
        {
            List<NCPDP.Telecom.Response.OtherPayers> otherPayers = new List<NCPDP.Telecom.Response.OtherPayers>();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    otherPayers = EligibilityResponse.COB.OtherPayers;
                    break;
                case "ClaimSubmissionResponse":
                    otherPayers = ClaimResponse.COB.OtherPayers;
                    break;
                case "ClaimReversalResponse":
                    //Reversal does not have COB
                    break;
            }

            foreach (NCPDP.Telecom.Response.OtherPayers otherPayer in NcpdpResponse.CurrentTransaction.CoordinationOfBenefits.OtherPayers)
            {
                otherPayers.Add(new NCPDP.Telecom.Response.OtherPayers()
                {
                    BenefitEffectiveDate = otherPayer.BenefitEffectiveDate,
                    BenefitTerminationDate = otherPayer.BenefitTerminationDate,
                    CardholderId = otherPayer.CardholderId,
                    CoverageType = otherPayer.CoverageType,
                    GroupId = otherPayer.GroupId,
                    HelpDeskNumber = otherPayer.HelpDeskNumber,
                    IdQualifier = otherPayer.IdQualifier,
                    OtherPayerId = otherPayer.OtherPayerId,
                    PatientRelationshipCode = otherPayer.PatientRelationshipCode,
                    PersonCode = otherPayer.PersonCode,
                    ProcessorControlNumber = otherPayer.ProcessorControlNumber
                });
            }
        }

        public void MapResponseDUR<T>()
        {
            List<NCPDP.Telecom.Response.DURFields> dURFields = new List<NCPDP.Telecom.Response.DURFields>();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    //Eligibility does not have DUR
                    break;
                case "ClaimSubmissionResponse":
                    dURFields = ClaimResponse.DUR.DURFields;
                    break;
                case "ClaimReversalResponse":
                    //Reversal does not have DUR
                    break;
            }

            foreach (NCPDP.Telecom.Response.DURFields dURField in NcpdpResponse.CurrentTransaction.DUR.DURFields)
            {
                dURFields.Add(new NCPDP.Telecom.Response.DURFields()
                {
                    AdditionalText = dURField.AdditionalText,
                    ClinicalSignificanceCode = dURField.ClinicalSignificanceCode,
                    DatabaseIndicator = dURField.DatabaseIndicator,
                    FreeTextMessage = dURField.FreeTextMessage,
                    OtherPharmacyIndicator = dURField.OtherPharmacyIndicator,
                    OtherPrescriberIndicator = dURField.OtherPrescriberIndicator,
                    PreviousDateOfFill = dURField.PreviousDateOfFill,
                    QuantityOfPreviousFill = dURField.QuantityOfPreviousFill,
                    ReasonForServiceCode = dURField.ReasonForServiceCode
                });
            }
        }

        public void MapResponseHeader<T>()
        {
            NCPDP.Telecom.Response.ResponseHeader responseHeader = new NCPDP.Telecom.Response.ResponseHeader();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    responseHeader = EligibilityResponse.Header;
                    break;
                case "ClaimSubmissionResponse":
                    responseHeader = ClaimResponse.Header;
                    break;
                case "ClaimReversalResponse":
                    responseHeader = ReversalResponse.Header;
                    break;
            }

            responseHeader.DateofService = NcpdpResponse.Header.DateofService;
            responseHeader.HeaderResponseStatus = NcpdpResponse.Header.HeaderResponseStatus;
            responseHeader.ServiceProviderId = NcpdpResponse.Header.ServiceProviderId;
            responseHeader.ServiceProviderIdQualifier = NcpdpResponse.Header.ServiceProviderIdQualifier;
            responseHeader.TransactionCode = NcpdpResponse.Header.TransactionCode;
            responseHeader.VersionNumber = NcpdpResponse.Header.VersionNumber;
        }

        public void MapResponseInsurance<T>()
        {
            NCPDP.Telecom.Response.ResponseInsurance responseInsurance = new NCPDP.Telecom.Response.ResponseInsurance();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    responseInsurance = EligibilityResponse.Insurance;
                    break;
                case "ClaimSubmissionResponse":
                    responseInsurance = ClaimResponse.Insurance;
                    break;
                case "ClaimReversalResponse":
                    responseInsurance = ReversalResponse.Insurance;
                    break;
            }

            responseInsurance.CardholderId = NcpdpResponse.Insurance.CardholderId;
            responseInsurance.GroupId = NcpdpResponse.Insurance.GroupId;
            responseInsurance.MedicaidAgencyNumber = NcpdpResponse.Insurance.MedicaidAgencyNumber;
            responseInsurance.MedicaidIdNumber = NcpdpResponse.Insurance.MedicaidIdNumber;
            responseInsurance.NetworkReimbursementId = NcpdpResponse.Insurance.NetworkReimbursementId;
            responseInsurance.PayerId = NcpdpResponse.Insurance.PayerId;
            responseInsurance.PayerIdQualifier = NcpdpResponse.Insurance.PayerIdQualifier;
            responseInsurance.PlanId = NcpdpResponse.Insurance.PlanId;
        }

        public void MapResponseMessage<T>()
        {
            if (NcpdpResponse.Message != null)
            {
                NCPDP.Telecom.Response.ResponseMessage responseMessage = new NCPDP.Telecom.Response.ResponseMessage();

                switch (typeof(T).Name)
                {
                    case "ClaimEligibilityResponse":
                        responseMessage = EligibilityResponse.Message;
                        break;
                    case "ClaimSubmissionResponse":
                        responseMessage = ClaimResponse.Message;
                        break;
                    case "ClaimReversalResponse":
                        responseMessage = ReversalResponse.Message;
                        break;
                }

                responseMessage.Message = NcpdpResponse.Message.Message;
            }
        }

        public void MapResponsePatient<T>()
        {
            NCPDP.Telecom.Response.ResponsePatient responsePatient = new NCPDP.Telecom.Response.ResponsePatient();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    responsePatient = EligibilityResponse.Patient;
                    break;
                case "ClaimSubmissionResponse":
                    responsePatient = ClaimResponse.Patient;
                    break;
                case "ClaimReversalResponse":
                    //Reversal does not have Patient
                    break;
            }

            responsePatient.LastName = NcpdpResponse.Patient.LastName;
            responsePatient.DateOfBirth = NcpdpResponse.Patient.DateOfBirth;
            responsePatient.FirstName = NcpdpResponse.Patient.FirstName;
        }

        public void MapResponsePricing<T>()
        {
            NCPDP.Telecom.Response.ResponsePricing responsePricing = new NCPDP.Telecom.Response.ResponsePricing();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    //Eligibility does not have Pricing
                    break;
                case "ClaimSubmissionResponse":
                    responsePricing = ClaimResponse.Pricing;
                    break;
                case "ClaimReversalResponse":
                    //Reversal does not have Pricing
                    break;
            }

            responsePricing.AccumulatedDeductibleAmount = NcpdpResponse.CurrentTransaction.Pricing.AccumulatedDeductibleAmount;
            responsePricing.AmountAppliedToPeriodicDeductible = NcpdpResponse.CurrentTransaction.Pricing.AmountAppliedToPeriodicDeductible;
            responsePricing.AmountAttributedToBrandNonPreferredFormularySelection = NcpdpResponse.CurrentTransaction.Pricing.AmountAttributedToBrandNonPreferredFormularySelection;
            responsePricing.AmountAttributedToCoverageGap = NcpdpResponse.CurrentTransaction.Pricing.AmountAttributedToCoverageGap;
            responsePricing.AmountAttributedToNonPreferredFormularySelection = NcpdpResponse.CurrentTransaction.Pricing.AmountAttributedToNonPreferredFormularySelection;
            responsePricing.AmountAttributedToProcessorFee = NcpdpResponse.CurrentTransaction.Pricing.AmountAttributedToProcessorFee;
            responsePricing.AmountAttributedToProductSelectionBrandDrug = NcpdpResponse.CurrentTransaction.Pricing.AmountAttributedToProductSelectionBrandDrug;
            responsePricing.AmountAttributedToProviderNetworkSelection = NcpdpResponse.CurrentTransaction.Pricing.AmountAttributedToProviderNetworkSelection;
            responsePricing.AmountAttributedToSalesTax = NcpdpResponse.CurrentTransaction.Pricing.AmountAttributedToSalesTax;
            responsePricing.AmountExceedingPeriodicBenefitMaximum = NcpdpResponse.CurrentTransaction.Pricing.AmountExceedingPeriodicBenefitMaximum;
            responsePricing.BasisOfCalculationCoInsurance = NcpdpResponse.CurrentTransaction.Pricing.BasisOfCalculationCoInsurance;
            responsePricing.BasisOfCalculationCopay = NcpdpResponse.CurrentTransaction.Pricing.BasisOfCalculationCopay;
            responsePricing.BasisOfCalculationDispensingFee = NcpdpResponse.CurrentTransaction.Pricing.BasisOfCalculationDispensingFee;
            responsePricing.BasisOfCalculationFlatSalesTax = NcpdpResponse.CurrentTransaction.Pricing.BasisOfCalculationFlatSalesTax;
            responsePricing.BasisOfCalculationPercentageSalesTax = NcpdpResponse.CurrentTransaction.Pricing.BasisOfCalculationPercentageSalesTax;
            responsePricing.BasisOfReimbursementDetermination = NcpdpResponse.CurrentTransaction.Pricing.BasisOfReimbursementDetermination;
            MapResponsePricingBenefitStages<T>();
            responsePricing.CoInsuranceAmount = NcpdpResponse.CurrentTransaction.Pricing.CoInsuranceAmount;
            responsePricing.CopayAmount = NcpdpResponse.CurrentTransaction.Pricing.CopayAmount;
            responsePricing.DispensingFeeContractedAmount = NcpdpResponse.CurrentTransaction.Pricing.DispensingFeeContractedAmount;
            responsePricing.DispensingFeePaid = NcpdpResponse.CurrentTransaction.Pricing.DispensingFeePaid;
            responsePricing.EstimatedGenericSavings = NcpdpResponse.CurrentTransaction.Pricing.EstimatedGenericSavings;
            responsePricing.FlatSalesTaxAmountPaid = NcpdpResponse.CurrentTransaction.Pricing.FlatSalesTaxAmountPaid;
            responsePricing.HealthPlanFundedAssistanceAmount = NcpdpResponse.CurrentTransaction.Pricing.HealthPlanFundedAssistanceAmount;
            responsePricing.IncentiveAmountPaid = NcpdpResponse.CurrentTransaction.Pricing.IncentiveAmountPaid;
            responsePricing.IngredientCostContractedAmount = NcpdpResponse.CurrentTransaction.Pricing.IngredientCostContractedAmount;
            responsePricing.IngredientCostPaid = NcpdpResponse.CurrentTransaction.Pricing.IngredientCostPaid;
            MapResponsePricingOtherAmount<T>();
            responsePricing.PatientPayAmount = NcpdpResponse.CurrentTransaction.Pricing.PatientPayAmount;
            responsePricing.PatientSalesTaxAmount = NcpdpResponse.CurrentTransaction.Pricing.PatientSalesTaxAmount;
            responsePricing.PercentageSalesTaxAmountPaid = NcpdpResponse.CurrentTransaction.Pricing.PercentageSalesTaxAmountPaid;
            responsePricing.PercentageSalesTaxBasisPaid = NcpdpResponse.CurrentTransaction.Pricing.PercentageSalesTaxBasisPaid;
            responsePricing.PlanSalesTaxAmount = NcpdpResponse.CurrentTransaction.Pricing.PlanSalesTaxAmount;
            responsePricing.PercentageSalesTaxRatePaid = NcpdpResponse.CurrentTransaction.Pricing.PercentageSalesTaxRatePaid;
            responsePricing.ProfessionalServiceFeePaid = NcpdpResponse.CurrentTransaction.Pricing.ProfessionalServiceFeePaid;
            responsePricing.RemainingBenefitAmount = NcpdpResponse.CurrentTransaction.Pricing.RemainingBenefitAmount;
            responsePricing.RemainingDeductibleAmount = NcpdpResponse.CurrentTransaction.Pricing.RemainingDeductibleAmount;
            responsePricing.SpendingAccountAmountRemaining = NcpdpResponse.CurrentTransaction.Pricing.SpendingAccountAmountRemaining;
            responsePricing.TaxExemptIndicator = NcpdpResponse.CurrentTransaction.Pricing.TaxExemptIndicator;
            responsePricing.TotalAmountPaid = NcpdpResponse.CurrentTransaction.Pricing.TotalAmountPaid;
        }

        private void MapResponsePricingBenefitStages<T>()
        {
            List<NCPDP.Telecom.Response.BenefitStage> benefitStages = new List<NCPDP.Telecom.Response.BenefitStage>();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    //Eligibility does not have Pricing
                    break;
                case "ClaimSubmissionResponse":
                    benefitStages = ClaimResponse.Pricing.BenefitStages;
                    break;
                case "ClaimReversalResponse":
                    //Reversal does not have Pricing
                    break;
            }

            foreach (NCPDP.Telecom.Response.BenefitStage benefitStage in NcpdpResponse.CurrentTransaction.Pricing.BenefitStages)
            {
                benefitStages.Add(new NCPDP.Telecom.Response.BenefitStage()
                {
                    Amount = benefitStage.Amount,
                    BenefitStageQualifier = benefitStage.BenefitStageQualifier
                });
            }
        }

        private void MapResponsePricingOtherAmount<T>()
        {
            List<NCPDP.Telecom.Response.OtherAmountPaid> otherAmountPaids = new List<NCPDP.Telecom.Response.OtherAmountPaid>();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    //Eligibility does not have Pricing
                    break;
                case "ClaimSubmissionResponse":
                    otherAmountPaids = ClaimResponse.Pricing.OtherAmounts;
                    break;
                case "ClaimReversalResponse":
                    //Reversal does not have Pricing
                    break;
            }

            foreach (NCPDP.Telecom.Response.OtherAmountPaid otherAmount in NcpdpResponse.CurrentTransaction.Pricing.OtherAmounts)
            {
                otherAmountPaids.Add(new NCPDP.Telecom.Response.OtherAmountPaid()
                {
                    AmountPaid = otherAmount.AmountPaid,
                    AmountPaidQualifier = otherAmount.AmountPaidQualifier,
                    OtherPayerAmountRecognized = otherAmount.OtherPayerAmountRecognized
                });
            }
        }

        public void MapResponseStatus<T>()
        {
            NCPDP.Telecom.Response.ResponseStatus responseStatus = new NCPDP.Telecom.Response.ResponseStatus();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    responseStatus = EligibilityResponse.Status;
                    break;
                case "ClaimSubmissionResponse":
                    responseStatus = ClaimResponse.Status;
                    break;
                case "ClaimReversalResponse":
                    responseStatus = ReversalResponse.Status;
                    break;
            }

            responseStatus.AuthorizationNumber = NcpdpResponse.CurrentTransaction.Status.AuthorizationNumber;
            responseStatus.HelpDeskNumberQualifier = NcpdpResponse.CurrentTransaction.Status.HelpDeskNumberQualifier;
            responseStatus.HelpDeskPhoneNumber = NcpdpResponse.CurrentTransaction.Status.HelpDeskPhoneNumber;
            responseStatus.InternalControlNumber = NcpdpResponse.CurrentTransaction.Status.InternalControlNumber;
            responseStatus.TransactionReferenceNumber = NcpdpResponse.CurrentTransaction.Status.TransactionReferenceNumber;
            responseStatus.TransactionResponseStatus = NcpdpResponse.CurrentTransaction.Status.TransactionResponseStatus;
            responseStatus.URL = NcpdpResponse.CurrentTransaction.Status.URL;

            MapStatusAdditionalMessages<T>();
            MapStatusApprovedMessageCodes<T>();
            MapStatusRejects<T>();
        }

        public void MapReversalRequestClaim()
        {
            Ncpdp.CurrentTransaction.Claim.PrescriptionNumber = ReversalRequest.Claim.PrescriptionNumber;
            Ncpdp.CurrentTransaction.Claim.PrescriptionQualifier = ReversalRequest.Claim.PrescriptionQualifier;
            Ncpdp.CurrentTransaction.Claim.ProductId = ReversalRequest.Claim.ProductId;
            Ncpdp.CurrentTransaction.Claim.ProductIdQualifier = ReversalRequest.Claim.ProductIdQualifier;
            Ncpdp.CurrentTransaction.Claim.FillNumber = ReversalRequest.Claim.FillNumber;
            Ncpdp.CurrentTransaction.Claim.OtherCoverageCode = ReversalRequest.Claim.OtherCoverageCode;
            Ncpdp.CurrentTransaction.Claim.PharmacyServiceType = ReversalRequest.Claim.PharmacyServiceType;
        }

        public void MapReversalRequestCoordinationOfBenefits()
        {
            if (ReversalRequest.COB != null)
            {
                foreach (NCPDP.Telecom.Request.OtherPayer otherPayer in ReversalRequest.COB.OtherPayers)
                {
                    NCPDP.Telecom.Request.OtherPayer otherPayer1 = new NCPDP.Telecom.Request.OtherPayer()
                    {
                        OtherPayerCoverageType = otherPayer.OtherPayerCoverageType
                    };
                    Ncpdp.CurrentTransaction.CoordinationOfBenefits.OtherPayers.Add(otherPayer1);
                }
            }
        }

        public void MapReversalRequestHeader()
        {
            Ncpdp.Header.BinNumber = ReversalRequest.Header.BinNumber;
            Ncpdp.Header.VersionNumber = ReversalRequest.Header.VersionNumber;
            Ncpdp.Header.DateOfService = ReversalRequest.Header.DateOfService;
            Ncpdp.Header.ProcessorControlNumber = ReversalRequest.Header.ProcessorControlNumber;
            Ncpdp.Header.ServiceProviderId = ReversalRequest.Header.ServiceProviderId;
            Ncpdp.Header.ServiceProviderIdQualifier = ReversalRequest.Header.ServiceProviderIdQualifier;
            Ncpdp.Header.TransactionCode = ReversalRequest.Header.TransactionCode;
            Ncpdp.Header.TransactionCount = ReversalRequest.Header.TransactionCount;
            Ncpdp.Header.SoftwareId = ReversalRequest.Header.SoftwareId; //GetSoftwareIdByAnsiBin(ReversalRequest.Header.BinNumber, ReversalRequest.Header.ProcessorControlNumber);
        }

        public void MapReversalRequestInsurance()
        {
            if (ReversalRequest.Insurance != null)
            {
                Ncpdp.Insurance.CardholderId = ReversalRequest.Insurance.CardholderId;
                Ncpdp.Insurance.GroupId = ReversalRequest.Insurance.GroupId;
            }
        }

        public void MapReversalRequestPricing()
        {
            if (ReversalRequest.Pricing != null)
            {
                Ncpdp.CurrentTransaction.Pricing.IngredientCostSubmitted = ReversalRequest.Pricing.IngredientCostSubmitted;
            }
        }

        private void MapStatusAdditionalMessages<T>()
        {
            List<NCPDP.Telecom.Response.AdditionalMessage> additionalMessages = new List<NCPDP.Telecom.Response.AdditionalMessage>();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    additionalMessages = EligibilityResponse.Status.AdditionalMessages;
                    break;
                case "ClaimSubmissionResponse":
                    additionalMessages = ClaimResponse.Status.AdditionalMessages;
                    break;
                case "ClaimReversalResponse":
                    additionalMessages = ReversalResponse.Status.AdditionalMessages;
                    break;
            }

            foreach (NCPDP.Telecom.Response.AdditionalMessage additionalMessage in NcpdpResponse.CurrentTransaction.Status.AdditionalMessages)
            {
                additionalMessages.Add(new NCPDP.Telecom.Response.AdditionalMessage()
                {
                    Information = additionalMessage.Information,
                    InformationContinuity = additionalMessage.InformationContinuity,
                    InformationQualifier = additionalMessage.InformationQualifier
                });
            }
        }

        private void MapStatusApprovedMessageCodes<T>()
        {
            List<string> approvedMessageCodes = new List<string>();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    approvedMessageCodes = EligibilityResponse.Status.ApprovedMessageCodes;
                    break;
                case "ClaimSubmissionResponse":
                    approvedMessageCodes = ClaimResponse.Status.ApprovedMessageCodes;
                    break;
                case "ClaimReversalResponse":
                    approvedMessageCodes = ReversalResponse.Status.ApprovedMessageCodes;
                    break;
            }

            foreach (string approvedMessageCode in NcpdpResponse.CurrentTransaction.Status.ApprovedMessageCodes)
            {
                approvedMessageCodes.Add(approvedMessageCode);
            }
        }

        private void MapStatusRejects<T>()
        {
            List<NCPDP.Telecom.Response.Reject> rejects = new List<NCPDP.Telecom.Response.Reject>();

            switch (typeof(T).Name)
            {
                case "ClaimEligibilityResponse":
                    rejects = EligibilityResponse.Status.Rejects;
                    break;
                case "ClaimSubmissionResponse":
                    rejects = ClaimResponse.Status.Rejects;
                    break;
                case "ClaimReversalResponse":
                    rejects = ReversalResponse.Status.Rejects;
                    break;
            }

            foreach (NCPDP.Telecom.Response.Reject reject in NcpdpResponse.CurrentTransaction.Status.Rejects)
            {
                rejects.Add(new NCPDP.Telecom.Response.Reject()
                {
                    FieldOccurrenceIndicator = reject.FieldOccurrenceIndicator,
                    RejectCode = reject.RejectCode
                });
            }
        }

        public Transmission MapSubmissionRequestClaimToNcpdp(ClaimSubmissionRequest request)
        {
            ClaimRequest = request;
            Ncpdp = new Transmission();
            MapClaimRequestHeader();
            MapClaimRequestInsurance();
            MapClaimRequestPatient();
            MapClaimRequestTransactions();
            if (IsIllinoisMedicaid(ClaimRequest.Header.BinNumber, ClaimRequest.Header.ProcessorControlNumber))
            {
                SetIllinoisMedicaidHardCodes(Ncpdp);
            }
            if (IsCareMark(ClaimRequest.Header.BinNumber))
            {
                SetCareMarkHardCodes(Ncpdp);
            }
            if (IsExpressScripts(ClaimRequest.Header.BinNumber))
            {
                SetExpressScriptsHardCodes(Ncpdp);
            }
            return Ncpdp;
        }

        public void SetCareMarkHardCodes(Transmission Ncpdp)
        {
            if (Ncpdp.Header.TransactionCode == TransactionCode.Reversal)
            {
                Ncpdp.Insurance.CardholderFirstName = string.Empty;
                Ncpdp.Insurance.CardholderLastName = string.Empty;
                Ncpdp.Insurance.PersonCode = string.Empty;
                Ncpdp.Insurance.PatientRelationshipCode = PatientRelationshipCode.NotSpecified;
                Ncpdp.Insurance.EligibilityClarificationCode = EligibilityClarificationCode.NotSpecified;
                Ncpdp.CurrentTransaction.Claim.QuantityDispensed = null;
                Ncpdp.CurrentTransaction.Claim.DaysSupply = null;
                Ncpdp.CurrentTransaction.Claim.CompoundCode = CompoundCode.NotSpecified;
                Ncpdp.CurrentTransaction.Claim.DispenseAsWritten = '\0';
                Ncpdp.CurrentTransaction.Claim.DatePrescriptionWritten = null;
                Ncpdp.CurrentTransaction.Claim.PrescriptionOriginCode = '\0';
                Ncpdp.CurrentTransaction.Claim.UnitOfMeasure = string.Empty;
                Ncpdp.CurrentTransaction.Claim.SpecialPackagingIndicator = string.Empty;
                if (ReversalRequest.Pricing != null)
                {
                    Ncpdp.CurrentTransaction.Pricing.IngredientCostSubmitted = null;
                }
            }
            Ncpdp.Patient.Residence = PatientResidence.Home;
        }

        public void SetExpressScriptsHardCodes(Transmission Ncpdp)
        {
            Ncpdp.Patient.Residence = PatientResidence.Home;
        }

        public void SetIllinoisMedicaidHardCodes(Transmission Ncpdp)
        {
            Ncpdp.CurrentTransaction.Claim.QuantityPrescribed = null;
            Ncpdp.CurrentTransaction.Claim.RefillsAuthorized = null;
            Ncpdp.CurrentTransaction.Prescriber.LastName = null;
            Ncpdp.Patient.Residence = PatientResidence.Home;
        }
    }
}
