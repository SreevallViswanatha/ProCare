using ProCare.API.Claims.Messages.Request;
using ProCare.Common.Data;
using ProCare.NCPDP.Telecom;
using ProCare.NCPDP.Telecom.Request;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProCare.API.PBM.Repository.DTO
{
    public enum ClaimSubmissionType
    {
        OriginalClaimForLogging = 1,
        ResubmitUnderNewPlan = 2
    }

    public enum ClaimTable
    {
        APSDLY = 1,
        CLAIMHIST = 2
    }

    public enum ClaimStatus
    {
        Paid = 1,
        Reversal = 2
    }

    public enum RunType
    {
        NewRun = 1,
        PartialFromReversalStep = 2,
        PartialFromResubmissionStep = 3
    }

    public class RetroLICSClaimDTO : ILoadFromDataReader
    {
        public bool ValidEligibility { get; set; }
        public bool ReversalSuccessful { get; set; }
        public bool ResubmissionSuccessful { get; set; }
        public string ReprocessedNDCREF { get; set; }
        public string ReprocessingPLNID { get; set; }

        public ClaimTable ClaimTable { get; set; }
        public ClaimStatus ClaimStatus { get; set; }
        public string ENRID { get; set; }
        public string PLNID { get; set; }
        public string RXNO { get; set; }
        public string PHAID { get; set; }
        public DateTime FILLDT { get; set; }
        public string NDCREF { get; set; }
        public decimal? OTHERAMT { get; set; }
        public string ANSI_BIN { get; set; }
        public string PHAQUAL { get; set; }
        public string SoftwareID { get; set; }
        public string PROCESSNO { get; set; }
        public string CARDID { get; set; }
        public string CARDID2 { get; set; }
        public string RELCD { get; set; }
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string PAYEE { get; set; }
        public string ENPID { get; set; }
        public string SUBGRPID { get; set; }
        public string PERSON { get; set; }
        public string PAT_SSN { get; set; }
        public DateTime? DOB { get; set; }
        public string SEX { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string PATLOC { get; set; }
        public string DIAGCODE { get; set; }
        public string DIAGQUAL { get; set; }
        public bool IsCompound { get; set; }
        public string DISPUNIT { get; set; }
        public string DOSEFORM { get; set; }
        public string ROUTE { get; set; }
        public string NDC { get; set; }
        public decimal? DECIQTY { get; set; }
        public decimal? INGRCOST { get; set; }
        public string BASISCOST { get; set; }
        public string RxQualifier { get; set; }
        public string ProductIDQualifier { get; set; }
        public decimal? SDECIQTY { get; set; }
        public string NWREF { get; set; }
        public int? DAYSUP { get; set; }
        public string COMPCODE { get; set; }
        public string DAW { get; set; }
        public string OTHERCODE { get; set; }
        public string SUBOVERIDE { get; set; }
        public int PAUTHNO { get; set; }
        public string PROFSVCCD { get; set; }
        public string SUBLOE { get; set; }
        public string PHANPI { get; set; }
        public string PHYQUAL { get; set; }
        public string PHYNPI { get; set; }
        public string PHYID { get; set; }
        public decimal? SINGRCOST { get; set; }
        public decimal? SDISPFEE { get; set; }
        public decimal? STAX { get; set; }
        public decimal? SCHARGE { get; set; }
        public decimal? STOTPRC { get; set; }
        public decimal? SENRAMT { get; set; }
        public decimal? SINCENTIV { get; set; }
        public decimal? OTHRCLAIMD { get; set; }
        public string EMPNAME { get; set; }
        public string EMPSTATE { get; set; }
        public string EMPZIP { get; set; }
        public string EMPPHONE { get; set; }
        public DateTime? INJURDT { get; set; }
        public string WCCLAIMID { get; set; }
        public string CARRIER_ID { get; set; }
        public string BBPFrom { get; set; }
        public string BBRFrom { get; set; }
        public string PhaNeutral { get; set; }
        public string PlnNeutral { get; set; }
        public decimal? RUnitAwp { get; set; }
        public decimal? DISPFEE { get; set; }
        public decimal? CHARGE { get; set; }
        public decimal? TOTPRC { get; set; }
        public decimal? ADMINFEE { get; set; }
        public decimal? MARGIN { get; set; }
        public decimal? SVCFEE { get; set; }
        public decimal? TROOP { get; set; }
        public decimal? GAP_TROOP { get; set; }
        public decimal? ENRAMT { get; set; }
        public decimal? TAX { get; set; }
        public decimal? GDCB { get; set; }
        public decimal? GDCA { get; set; }
        public decimal? CPP { get; set; }
        public decimal? NPP { get; set; }
        public decimal? VaccineFee { get; set; }
        public decimal? Pref30 { get; set; }
        public decimal? Pref90 { get; set; }
        public DateTime? RXDATE { get; set; }
        public int AUTHREF { get; set; }
        public DateTime? EFFDT { get; set; }
        public DateTime? TRMDT { get; set; }
        public DateTime NDCPROCDT { get; set; }
        public string BATID { get; set; }
        public string RXORIGIN { get; set; }
        public string PHASVCTYPE { get; set; }
        public string PLCOFSVC { get; set; }
        public string SECONDS { get; set; }
        public string STATUS { get; set; }
        public decimal? QTYPRESC { get; set; }
        public List<CompoundInfoDTO> CompoundInfo { get; set; }
        public DateTime ProcessingTime { get; set; }
        public bool SubmittedByRetroLICSReprocessor { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ValidEligibility = false;
            ReversalSuccessful = false;
            ResubmissionSuccessful = false;
            SoftwareID = "RETRO";

            ClaimTable = Enum.Parse<ClaimTable>(reader.GetStringorDefault("ClaimTable"));
            ClaimStatus = getClaimStatus(reader.GetStringorDefault("JVID"));
            ENRID = reader.GetStringorDefault("ENRID");
            PLNID = reader.GetStringorDefault("PLNID");
            RXNO = reader.GetStringorDefault("RXNO");
            PHAID = reader.GetStringorDefault("PHAID");
            FILLDT = reader.GetDateTimeorDefault("FILLDT", DateTime.MinValue);
            NDCREF = reader.GetStringorDefault("NDCREF");
            OTHERAMT = reader.GetDecimalorNull("OTHERAMT");
            ANSI_BIN = reader.GetStringorDefault("ANSI_BIN");
            PHAQUAL = reader.GetStringorDefault("PHAQUAL");
            //SoftwareID = reader.GetStringorDefault("SoftwareID");
            PROCESSNO = reader.GetStringorDefault("PROCESSNO");
            CARDID = reader.GetStringorDefault("CARDID");
            CARDID2 = reader.GetStringorDefault("CARDID2");
            RELCD = reader.GetStringorDefault("RELCD");
            FNAME = reader.GetStringorDefault("FNAME");
            LNAME = reader.GetStringorDefault("LNAME");
            PAYEE = reader.GetStringorDefault("PAYEE");
            ENPID = reader.GetStringorDefault("ENPID");
            SUBGRPID = reader.GetStringorDefault("SUBGRPID");
            PERSON = reader.GetStringorDefault("PERSON");
            PAT_SSN = reader.GetStringorDefault("PAT_SSN");
            //EligClarificationCode = reader.GetStringorDefault("EligClarificationCode");
            DOB = reader.GetDateTimeorNull("DOB");
            SEX = reader.GetStringorDefault("SEX");
            Street1 = reader.GetStringorDefault("ADDR");
            City = reader.GetStringorDefault("City");
            State = reader.GetStringorDefault("State");
            Zip = reader.GetStringorDefault("Zip");
            Phone = reader.GetStringorDefault("Phone");
            PATLOC = reader.GetStringorDefault("PATLOC");
            DIAGCODE = reader.GetStringorDefault("DIAGCODE");
            DIAGQUAL = reader.GetStringorDefault("DIAGQUAL");
            //IsCompound = reader.GetBooleanSafe("IsCompound");
            DISPUNIT = reader.GetStringorDefault("DISPUNIT");
            DOSEFORM = reader.GetStringorDefault("DOSEFORM");
            ROUTE = reader.GetStringorDefault("ROUTE");
            NDC = reader.GetStringorDefault("NDC");
            DECIQTY = reader.GetDecimalorNull("DECIQTY");
            INGRCOST = reader.GetDecimalorNull("INGRCOST");
            BASISCOST = reader.GetStringorDefault("BASISCOST");
            //RxQualifier = reader.GetStringorDefault("RxQualifier");
            //ProductIDQualifier = reader.GetStringorDefault("ProductIDQualifier");
            SDECIQTY = reader.GetDecimalorNull("SDECIQTY");
            NWREF = reader.GetStringorDefault("NWREF");
            DAYSUP = reader.GetInt32orNull("DAYSUP");
            COMPCODE = reader.GetStringorDefault("COMPCODE");
            IsCompound = !string.IsNullOrWhiteSpace(COMPCODE) && COMPCODE == "2";
            DAW = reader.GetStringorDefault("DAW");
            OTHERCODE = reader.GetStringorDefault("OTHERCODE");
            SUBOVERIDE = reader.GetStringorDefault("SUBOVERIDE");
            PAUTHNO = reader.GetInt32orDefault("PAUTHNO");
            PROFSVCCD = reader.GetStringorDefault("PROFSVCCD");
            SUBLOE = reader.GetStringorDefault("SUBLOE");
            PHANPI = reader.GetStringorDefault("PHANPI");
            PHYQUAL = reader.GetStringorDefault("PHYQUAL");
            PHYNPI = reader.GetStringorDefault("PHYNPI");
            PHYID = reader.GetStringorDefault("PHYID");
            SINGRCOST = reader.GetDecimalorNull("SINGRCOST");
            SDISPFEE = reader.GetDecimalorNull("SDISPFEE");
            STAX = reader.GetDecimalorNull("STAX");
            SCHARGE = reader.GetDecimalorNull("SCHARGE");
            STOTPRC = reader.GetDecimalorNull("STOTPRC");
            SENRAMT = reader.GetDecimalorNull("SENRAMT");
            SINCENTIV = reader.GetDecimalorNull("SINCENTIV");
            OTHRCLAIMD = reader.GetDecimalorNull("OTHRCLAIMD");
            EMPNAME = reader.GetStringorDefault("EMPNAME");
            EMPSTATE = reader.GetStringorDefault("EMPSTATE");
            EMPZIP = reader.GetStringorDefault("EMPZIP");
            EMPPHONE = reader.GetStringorDefault("EMPPHONE");
            INJURDT = reader.GetDateTimeorNull("INJURDT");
            WCCLAIMID = reader.GetStringorDefault("WCCLAIMID");
            CARRIER_ID = reader.GetStringorDefault("CARRIER_ID");
            BBPFrom = reader.GetStringorDefault("BBPFrom");
            BBRFrom = reader.GetStringorDefault("BBRFrom");
            PhaNeutral = reader.GetStringorDefault("PhaNeutral");
            PlnNeutral = reader.GetStringorDefault("PlnNeutral");
            RUnitAwp = reader.GetDecimalorNull("RUnitAwp");
            DISPFEE = reader.GetDecimalorNull("DISPFEE");
            CHARGE = reader.GetDecimalorNull("CHARGE");
            TOTPRC = reader.GetDecimalorNull("TOTPRC");
            ADMINFEE = reader.GetDecimalorNull("ADMINFEE");
            MARGIN = reader.GetDecimalorNull("MARGIN");
            SVCFEE = reader.GetDecimalorNull("SVCFEE");
            TROOP = reader.GetDecimalorNull("TROOP");
            GAP_TROOP = reader.GetDecimalorNull("GAP_TROOP");
            ENRAMT = reader.GetDecimalorNull("ENRAMT");
            TAX = reader.GetDecimalorNull("TAX");
            GDCB = reader.GetDecimalorNull("GDCB");
            GDCA = reader.GetDecimalorNull("GDCA");
            CPP = reader.GetDecimalorNull("CPP");
            NPP = reader.GetDecimalorNull("NPP");
            VaccineFee = reader.GetDecimalorNull("VaccineFee");
            Pref30 = reader.GetDecimalorNull("Pref30");
            Pref90 = reader.GetDecimalorNull("Pref90");
            RXDATE = reader.GetDateTimeorNull("RXDATE");
            AUTHREF = reader.GetInt32orDefault("AUTHREF");
            EFFDT = reader.GetDateTimeorNull("EFFDT");
            TRMDT = reader.GetDateTimeorNull("TRMDT");
            NDCPROCDT = reader.GetDateTimeorDefault("NDCPROCDT", DateTime.MinValue);
            BATID = reader.GetStringorDefault("BATID");
            RXORIGIN = reader.GetStringorDefault("RXORIGIN");
            PHASVCTYPE = reader.GetStringorDefault("PHASVCTYPE");
            PLCOFSVC = reader.GetStringorDefault("PLCOFSVC");
            SECONDS = reader.GetStringorDefault("SECONDS");
            QTYPRESC = reader.GetDecimalorNull("QTYPRESC");

            SetCalculatedFields();
        }

        public void LoadFromDataReaderForStatusVerification(IDataReader reader)
        {
            ClaimTable = Enum.Parse<ClaimTable>(reader.GetStringorDefault("ClaimTable"));
            ClaimStatus = getClaimStatus(reader.GetStringorDefault("JVID"));
            RXNO = reader.GetStringorDefault("RXNO");
            PHAID = reader.GetStringorDefault("PHAID");
            FILLDT = reader.GetDateTimeorDefault("FILLDT", DateTime.MinValue);
            NDCPROCDT = reader.GetDateTimeorDefault("NDCPROCDT", DateTime.MinValue);
            BATID = reader.GetStringorDefault("BATID");
            SECONDS = reader.GetStringorDefault("SECONDS");
            STATUS = reader.GetStringorDefault("STATUS");

            SetCalculatedFields();
        }

        private void SetCalculatedFields()
        {
            string paddedSeconds = SECONDS.PadRight(4, '0');

            double hours = BATID.Substring(BATID.Length - 4, 2).ToDouble();
            double minutes = BATID.Substring(BATID.Length - 2, 2).ToDouble();
            double seconds = paddedSeconds.Substring(0, 2).ToDouble();
            double milliseconds = paddedSeconds.Substring(2, 2).ToDouble();

            ProcessingTime = NDCPROCDT.AddHours(hours)
                                      .AddMinutes(minutes)
                                      .AddSeconds(seconds)
                                      .AddMilliseconds(milliseconds);

            SubmittedByRetroLICSReprocessor = BATID.ToUpper().Contains("LICS");
        }

        public ClaimEligibilityRequest MapClaimEligibilityRequest()
        {
            ClaimEligibilityRequest request = new ClaimEligibilityRequest();
            request.Header = GetHeaderSection();
            request.Insurance = GetInsuranceSection(ClaimSubmissionType.OriginalClaimForLogging);
            request.Patient = GetPatientSection();

            request.Header.TransactionCode = TransactionCode.EligibilityVerification;
            request.IsInternalClaim = true;
            return request;
        }

        public ClaimSubmissionRequest MapClaimSubmissionRequest(ClaimSubmissionType claimSubmissionType)
        {
            ClaimSubmissionRequest request = new ClaimSubmissionRequest();
            request.Claim = GetClaimSection();
            request.Clinical = GetClinicalSection();
            request.COB = GetCOBSection();
            request.Compound = GetCompoundSection();
            request.Coupon = GetCouponSection();
            request.DUR = GetDURSection();
            request.Facility = GetFacilitySection();
            request.Header = GetHeaderSection();
            request.Insurance = GetInsuranceSection(claimSubmissionType);
            request.Patient = GetPatientSection();
            request.Pharmacy = GetPharmacySection();
            request.Prescriber = GetPrescriberSection();
            request.Pricing = GetPricingSection();
            request.PriorAuth = GetPriorAuthSection();
            request.RetroLics = GetRetroLICSSection();
            request.WorkersComp = GetWorkersCompSection();

            request.Header.TransactionCode = TransactionCode.Billing;
            request.IsInternalClaim = true;
            return request;
        }

        //We only really need to submit pharmacyid, rxno, and filldate for reversals
        public ClaimReversalRequest MapClaimReversalRequest()
        {
            ClaimReversalRequest request = new ClaimReversalRequest();

            request.Claim = GetClaimSection();
            request.Header = GetHeaderSection();
            request.Insurance = GetInsuranceSection(ClaimSubmissionType.OriginalClaimForLogging);

            request.Claim.PrescriptionNumber = RXNO;
            request.Header.DateOfService = FILLDT;
            
            if (!string.IsNullOrWhiteSpace(PLNID))
            {
                request.Insurance.GroupId = PLNID;
            }

            request.Header.TransactionCode = TransactionCode.Reversal;
            request.IsInternalClaim = true;

            return request;
        }

        private RequestHeader GetHeaderSection()
        {
            RequestHeader section = new RequestHeader();
            section.DateOfService = FILLDT;
            section.TransactionCount = 1;
            section.BinNumber = ANSI_BIN;
            section.VersionNumber = VersionNumber.vD0;
            section.TransactionCount = 1;
            section.ServiceProviderIdQualifier = !string.IsNullOrWhiteSpace(PHAQUAL)
                ? (ServiceProviderIdQualifier) Enum.Parse(typeof(ServiceProviderIdQualifier), PHAQUAL)
                : ServiceProviderIdQualifier.Unknown;
            section.ServiceProviderId = section.ServiceProviderIdQualifier == ServiceProviderIdQualifier.NCPDPProviderID ? PHAID : PHANPI;
            section.SoftwareId = SoftwareID;
            section.ProcessorControlNumber = PROCESSNO;
            return section;
        }

        private RequestInsurance GetInsuranceSection(ClaimSubmissionType claimSubmissionType)
        {
            RequestInsurance section = new RequestInsurance();

            section.CardholderId = CARDID.Trim() + CARDID2.Trim();

            if (!string.IsNullOrWhiteSpace(FNAME))
            {
                section.CardholderFirstName = FNAME;
            }

            if (!string.IsNullOrWhiteSpace(LNAME))
            {
                section.CardholderLastName = LNAME;
            }

            if (claimSubmissionType == ClaimSubmissionType.OriginalClaimForLogging)
            {
                if (!string.IsNullOrWhiteSpace(PLNID))
                {
                    section.PlanId = PLNID;
                    section.GroupId = PLNID;
                }
            }
            else if (claimSubmissionType == ClaimSubmissionType.ResubmitUnderNewPlan)
            {
                if (!string.IsNullOrWhiteSpace(ReprocessingPLNID))
                {
                    section.PlanId = ReprocessingPLNID;
                    section.GroupId = ReprocessingPLNID;
                }
            }

            if (!string.IsNullOrWhiteSpace(PAYEE) || !string.IsNullOrWhiteSpace(ENPID))
            {
                section.FacilityId = PAYEE + ENPID;
            }

            if (!string.IsNullOrWhiteSpace(SUBGRPID))
            {
                section.GroupId = SUBGRPID;
            }

            section.PersonCode = PERSON;
            section.PatientRelationshipCode = !string.IsNullOrWhiteSpace(RELCD)
                ? (PatientRelationshipCode)Enum.Parse(typeof(PatientRelationshipCode), RELCD)
                : PatientRelationshipCode.NotSpecified;
            section.EligibilityClarificationCode = EligibilityClarificationCode.NotSpecified;

            return section;
        }

        private RequestPatient GetPatientSection()
        {
            RequestPatient section = new RequestPatient();
            if (!string.IsNullOrWhiteSpace(PAT_SSN))
            {
                section.PatientId = PAT_SSN;
            }

            section.PatientIdQualifier = PatientIdQualifier.NotSpecified;

            if (DOB.HasValue)
            {
                section.DateOfBirth = DOB.Value;
            }

            if (!string.IsNullOrWhiteSpace(SEX))
            {
                section.Gender = !string.IsNullOrWhiteSpace(SEX)
                    ? (PatientGender)Enum.Parse(typeof(PatientGender), SEX)
                    : PatientGender.NotSpecified;
            }

            if (!string.IsNullOrWhiteSpace(FNAME))
            {
                section.FirstName = FNAME;
            }

            if (!string.IsNullOrWhiteSpace(LNAME))
            {
                section.LastName = LNAME;
            }

            if (!string.IsNullOrWhiteSpace(Street1))
            {
                section.Street = Street1;
            }

            if (!string.IsNullOrWhiteSpace(City))
            {
                section.City = City;
            }

            if (!string.IsNullOrWhiteSpace(State))
            {
                section.State = State;
            }

            if (!string.IsNullOrWhiteSpace(Zip))
            {
                section.Zip = Zip;
            }

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                section.Phone = Phone;
            }

            if (!string.IsNullOrWhiteSpace(PLCOFSVC))
            {
                section.PlaceOfService = !string.IsNullOrWhiteSpace(PLCOFSVC)
                    ? (PlaceOfService)Enum.Parse(typeof(PlaceOfService), PLCOFSVC)
                    : PlaceOfService.NotSpecified;
            }

            if (!string.IsNullOrWhiteSpace(PATLOC))
            {
                section.Residence = !string.IsNullOrWhiteSpace(PATLOC)
                    ? (PatientResidence)Enum.Parse(typeof(PatientResidence), PATLOC)
                    : PatientResidence.NotSpecified;
            }
            else
            {
                section.Residence = PatientResidence.NotSpecified;
            }

            return section;
        }

        private RequestClinical GetClinicalSection()
        {
            RequestClinical section = new RequestClinical();

            if (!string.IsNullOrWhiteSpace(DIAGCODE))
            {
                section.DiagnosisCodes.Add(new DiagnosisCodes
                {
                    DiagnosisCodeQualifier = DIAGQUAL,
                    DiagnosisCode = DIAGCODE
                });
            }

            return section;
        }

        private RequestCompound GetCompoundSection()
        {
            RequestCompound section = new RequestCompound();

            if (IsCompound)
            {
                //Begin with original info from claim
                section.DispensingUnitFormIndicator = parseDispensingUnitFormIndicator(DISPUNIT);
                section.DosageFormDescriptionCode = parseDosageFormDescriptionCode(DOSEFORM);
                
                section.Ingredients.Add(new CompoundProduct
                {
                    ProductIdQualifier = "03",
                    ProductId = NDC,
                    Quantity = DECIQTY,
                    Cost = INGRCOST,
                    BasisOfCostDetermination = BASISCOST
                });

                //Attempt to use DLYCPD/HSTCPD/HSTPCDD info if available
                if (CompoundInfo.Count > 0)
                {
                    if (ClaimTable == ClaimTable.CLAIMHIST)
                    {
                        section.DispensingUnitFormIndicator = CompoundInfo.First().DispensingUnitFormIndicator;
                        section.DosageFormDescriptionCode = CompoundInfo.First().DosageFormDescriptionCode;
                        section.ModifierCodes = CompoundInfo.First().ModifierCodes;
                    }

                    section.Ingredients = new List<CompoundProduct>();
                    CompoundInfo.Each(x =>
                    {
                        if (!section.Ingredients.Any(y => y.ProductId == x.IngredientProductId))
                        {
                            section.Ingredients.Add(new CompoundProduct
                            {
                                BasisOfCostDetermination = x.IngredientBasisOfCostDetermination,
                                Cost = x.IngredientCost,
                                ProductId = x.IngredientProductId,
                                ProductIdQualifier = x.IngredientProductIdQualifier,
                                Quantity = x.IngredientQuantity
                            });
                        }
                    });
                }
            }

            return section;
        }

        private RequestCoupon GetCouponSection()
        {
            RequestCoupon section = new RequestCoupon();

            return section;
        }

        private RequestClaim GetClaimSection()
        {
            RequestClaim section = new RequestClaim();
            section.PrescriptionQualifier = "1";
            section.PrescriptionNumber = RXNO;
            section.ProductIdQualifier = "03";
            section.ProductId = NDC;

            if (RXDATE.HasValue)
            {
                section.DatePrescriptionWritten = RXDATE;
            }

            if (AUTHREF != 0)
            {
                section.RefillsAuthorized = AUTHREF;
            }

            if (SDECIQTY != 0)
            {
                section.QuantityDispensed = SDECIQTY;
            }

            if (!string.IsNullOrWhiteSpace(NWREF))
            {
                section.FillNumber = NWREF;
            }

            if (DAYSUP != 0)
            {
                section.DaysSupply = DAYSUP;
            }

            if (!string.IsNullOrWhiteSpace(COMPCODE))
            {
                section.CompoundCode = !string.IsNullOrWhiteSpace(COMPCODE)
                    ? (CompoundCode)Enum.Parse(typeof(CompoundCode), COMPCODE)
                    : CompoundCode.NotSpecified;
            }

            if (!string.IsNullOrWhiteSpace(DAW))
            {
                section.DispenseAsWritten = DAW.ToCharArray()[0];
            }

            if (!string.IsNullOrWhiteSpace(OTHERCODE))
            {
                section.OtherCoverageCode = OTHERCODE;
            }

            if (!string.IsNullOrWhiteSpace(SUBOVERIDE))
            {
                section.SubmissionClarificationCodes.Add(SUBOVERIDE);
            }

            if (PAUTHNO != 0)
            {
                section.PriorAuthorizationTypeCode = "1";
                section.PriorAuthorizationNumberSubmitted = PAUTHNO;
            }

            if (!string.IsNullOrWhiteSpace(RXORIGIN))
            {
                section.PrescriptionOriginCode = RXORIGIN[0];
            }
            else
            {
                section.PrescriptionOriginCode = '0';
            }

            if (!string.IsNullOrWhiteSpace(PHASVCTYPE) && PHASVCTYPE != "0")
            {
                section.PharmacyServiceType = PHASVCTYPE;
            }

            section.SubmissionClarificationCodes = new List<string>();

            if (!string.IsNullOrWhiteSpace(SUBOVERIDE))
            {
                section.SubmissionClarificationCodes.Add(SUBOVERIDE);
            }
            else
            {
                section.SubmissionClarificationCodes.Add(" ");
            }

            if(QTYPRESC.HasValue && QTYPRESC.Value > 0)
            {
                section.QuantityPrescribed = QTYPRESC;
            }

            if(!string.IsNullOrWhiteSpace(ROUTE))
            {
                section.RouteOfAdministration = ROUTE;
            }

            return section;
        }

        private RequestDrugUtilizationReview GetDURSection()
        {
            RequestDrugUtilizationReview section = new RequestDrugUtilizationReview();

            if (!string.IsNullOrWhiteSpace(PROFSVCCD))
            {
                section.Services.Add(new DrugUtilizationReviewService
                {
                    ProfessionalServiceCode = !string.IsNullOrWhiteSpace(PROFSVCCD) ? PROFSVCCD : string.Empty,
                    LevelOfEffort = IsCompound ? SUBLOE : string.Empty
                });
            }

            return section;
        }

        private RequestFacility GetFacilitySection()
        {
            RequestFacility section = new RequestFacility();

            return section;
        }

        private RequestPharmacy GetPharmacySection()
        {
            RequestPharmacy section = new RequestPharmacy();

            ProviderIDQualifier qualifier = !string.IsNullOrWhiteSpace(PHAQUAL)
                ? (ProviderIDQualifier)Enum.Parse(typeof(ProviderIDQualifier), PHAQUAL)
                : ProviderIDQualifier.NotSpecified;

            if (!string.IsNullOrWhiteSpace(PHAQUAL))
            {
                if (qualifier == ProviderIDQualifier.StateIssued)
                {
                    section.ProviderIdQualifier = ProviderIdQualifier.StateIssued;
                    section.ProviderId = PHAID;
                }
                else
                {
                    section.ProviderIdQualifier = ProviderIdQualifier.NationalProviderIdentifier;
                    section.ProviderId = PHANPI;
                }
            }

            return section;
        }

        private RequestPrescriber GetPrescriberSection()
        {
            RequestPrescriber section = new RequestPrescriber();

            PrescriberIdQualifier qualifier = !string.IsNullOrWhiteSpace(PHYQUAL)
                ? (PrescriberIdQualifier)Enum.Parse(typeof(PrescriberIdQualifier), PHYQUAL)
                : PrescriberIdQualifier.NotSpecified;

            if (qualifier == PrescriberIdQualifier.NationalProviderIdentifier)
            {
                section.PrescriberIdQualifier = PrescriberIdQualifier.NationalProviderIdentifier;
                section.PrescriberId = PHYNPI;
            }
            else
            {
                section.PrescriberIdQualifier = PrescriberIdQualifier.DrugEnforcementAdministration;
                section.PrescriberId = PHYID;
            }

            return section;
        }

        private RequestPricing GetPricingSection()
        {
            RequestPricing section = new RequestPricing();

            if (SINGRCOST != 0)
            {
                section.IngredientCostSubmitted = SINGRCOST;
            }

            if (SDISPFEE != 0)
            {
                section.DispensingFeeSubmitted = SDISPFEE;
            }

            if (STAX != 0)
            {
                section.FlatSalesTaxAmountSubmitted = STAX;
            }

            if (SCHARGE != 0)
            {
                section.UsualAndCustomaryCharge = SCHARGE;
            }

            if (STOTPRC != 0)
            {
                section.GrossAmountDue = STOTPRC;
            }

            if (SENRAMT != 0)
            {
                section.PatientPaidAmountSubmitted = SENRAMT;
            }

            if (SINCENTIV != 0)
            {
                section.IncentiveAmountSubmitted = SINCENTIV;
            }

            if (OTHRCLAIMD > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "OC",
                    OtherAmountClaimedSubmitted = OTHRCLAIMD
                });
            }

            if (OTHERAMT > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "OA",
                    OtherAmountClaimedSubmitted = OTHERAMT
                });
            }

            if (INGRCOST.HasValue && INGRCOST.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "IN",
                    OtherAmountClaimedSubmitted = INGRCOST
                });
            }

            if (DISPFEE.HasValue && DISPFEE.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "DI",
                    OtherAmountClaimedSubmitted = DISPFEE
                });
            }

            if (CHARGE.HasValue && CHARGE.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "CH",
                    OtherAmountClaimedSubmitted = CHARGE
                });
            }

            if (TOTPRC.HasValue && TOTPRC.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "TO",
                    OtherAmountClaimedSubmitted = TOTPRC
                });
            }

            if (ADMINFEE.HasValue && ADMINFEE.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "AD",
                    OtherAmountClaimedSubmitted = ADMINFEE
                });
            }

            if (MARGIN.HasValue && MARGIN.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "MA",
                    OtherAmountClaimedSubmitted = MARGIN
                });
            }

            if (SVCFEE.HasValue && SVCFEE.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "SV",
                    OtherAmountClaimedSubmitted = SVCFEE
                });
            }

            if (TROOP.HasValue && TROOP.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "TR",
                    OtherAmountClaimedSubmitted = TROOP
                });
            }

            if (GAP_TROOP.HasValue && GAP_TROOP.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "GA",
                    OtherAmountClaimedSubmitted = GAP_TROOP
                });
            }

            if (ENRAMT.HasValue && ENRAMT.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "EN",
                    OtherAmountClaimedSubmitted = ENRAMT
                });
            }

            if (TAX.HasValue && TAX.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "TA",
                    OtherAmountClaimedSubmitted = TAX
                });
            }

            if (GDCB.HasValue && GDCB.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "CB",
                    OtherAmountClaimedSubmitted = GDCB
                });
            }

            if (GDCA.HasValue && GDCA.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "CA",
                    OtherAmountClaimedSubmitted = GDCA
                });
            }

            if (CPP.HasValue && CPP.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "CP",
                    OtherAmountClaimedSubmitted = CPP
                });
            }

            if (NPP.HasValue && NPP.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "NP",
                    OtherAmountClaimedSubmitted = NPP
                });
            }

            if (VaccineFee.HasValue && VaccineFee.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "VF",
                    OtherAmountClaimedSubmitted = VaccineFee
                });
            }

            if (Pref30.HasValue && Pref30.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "P3",
                    OtherAmountClaimedSubmitted = Pref30
                });
            }

            if (Pref90.HasValue && Pref90.Value > 0)
            {
                section.OtherAmountClaimeds.Add(new OtherAmountClaimed
                {
                    OtherAmountClaimedSubmittedQualifier = "P9",
                    OtherAmountClaimedSubmitted = Pref90
                });
            }

            section.BasisOfCostDetermination = BASISCOST;

            return section;
        }

        private RequestPriorAuthorization GetPriorAuthSection()
        {
            RequestPriorAuthorization section = new RequestPriorAuthorization();
 
            return section;
        }

        private RequestRetroLics GetRetroLICSSection()
        {
            RequestRetroLics section = new RequestRetroLics();
            
            section.BBPFrom = BBPFrom;
            section.BBRFrom = BBRFrom;
            section.PhaNeutral = PhaNeutral;
            section.PlnNeutral = PlnNeutral;
            section.RUnitAwp = RUnitAwp;
            //section.FDBAWP = FDBAWP;

            return section;
        }

        private RequestWorkersComp GetWorkersCompSection()
        {
            RequestWorkersComp section = new RequestWorkersComp();
            if (!string.IsNullOrWhiteSpace(EMPNAME)
                || !string.IsNullOrWhiteSpace(EMPSTATE)
                || !string.IsNullOrWhiteSpace(EMPZIP)
                || !string.IsNullOrWhiteSpace(EMPPHONE)
                || !string.IsNullOrWhiteSpace(CARRIER_ID)
                || INJURDT != DateTime.MinValue
                || !string.IsNullOrWhiteSpace(WCCLAIMID))
            {
                if(!string.IsNullOrWhiteSpace(EMPNAME))
                { 
                    section.EmployerName = EMPNAME;
                }

                if (!string.IsNullOrWhiteSpace(EMPSTATE))
                {
                    section.EmployerState = EMPSTATE;
                }

                if (!string.IsNullOrWhiteSpace(EMPZIP))
                {
                    section.EmployerZip = EMPZIP;
                }

                if (!string.IsNullOrWhiteSpace(EMPPHONE))
                {
                    section.EmployerPhone = EMPPHONE;
                }

                if (!string.IsNullOrWhiteSpace(CARRIER_ID))
                {
                    section.CarrierId = CARRIER_ID;
                }

                if(INJURDT != System.DateTime.MinValue)
                {
                    section.DateofInjury = INJURDT;
                }

                if (!string.IsNullOrWhiteSpace(WCCLAIMID))
                {
                    section.ClaimId = WCCLAIMID;
                }
            }

            return section;
        }

        private RequestCoordinationOfBenefits GetCOBSection()
        {
            RequestCoordinationOfBenefits section = new RequestCoordinationOfBenefits();
            if (OTHERAMT > 0)
            {
                section.OtherPayers.Add(new OtherPayer
                {
                    OtherPayerCoverageType = "01"
                });

                section.OtherPayerAmounts.Add(new OtherPayerAmountsPaid
                {
                    OtherPayerAmountPaid = OTHERAMT
                });
            }

            return section;
        }

        private ClaimStatus getClaimStatus(string jvid)
        {
            ClaimStatus status = ClaimStatus.Paid;

            if (jvid.ContainsAny("B", "BD"))
            {
                status = ClaimStatus.Reversal;
            }

            return status;
        }

        private DispensingUnitFormIndicator parseDispensingUnitFormIndicator(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                ? (DispensingUnitFormIndicator)Enum.Parse(typeof(DispensingUnitFormIndicator), value)
                : DispensingUnitFormIndicator.Unknown;
        }

        private DosageFormDescriptionCode parseDosageFormDescriptionCode(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                ? (DosageFormDescriptionCode)Enum.Parse(typeof(DosageFormDescriptionCode), value)
                : DosageFormDescriptionCode.NotSpecified;
        }
    }
}
