using ProCare.API.Core;
using ProCare.API.PBM.Helpers;
using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Response;
using ProCare.API.PBM.Repository;
using ProCare.API.PBM.Repository.DTO;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.API
{
    public class MemberRuleServices : ServiceBase
    {
        #region Public Properties
        public IPbmRepository PBMRepository { get; set; }
        public IMemberRepository MemberRepository { get; set; }
        public IMemberRuleRepository MemberRuleRepository { get; set; }
        public IPharmacyRepository PharmacyRepository { get; set; }
        #endregion

        #region Public Methods
        //[Authenticate]
        //[RequiresAnyPermission(ApiRoutes.DynamicPARequest + "|" + ApiRoutes.DynamicPARequest)]
        //public async Task<DynamicPAResponse> Post(DynamicPARequest request)
        //{
        //    DynamicPAResponse response = new DynamicPAResponse();
        //    CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
        //    response = await processRequest(request, ApiRoutes.DynamicPARequest, async () =>
        //    {
        //        // Retrieve client connection string and maximum records settings from configuration
        //        Dictionary<string, string> clientSetting = await ClientConfigHelper.GetClientConfigs(commonApiHelper, request.ClientGuid).ConfigureAwait(false);
        //        DatasetDTO dataset = ClientConfigHelper.GetClientDataset(clientSetting, request.Client, null, null, request.GroupID, request.PlanID);
        //        string clientConnectionString = ClientConfigHelper.GetDatasetConnectionString(dataset);
        //        string userID = ClientConfigHelper.GetUserIDFromClientGuid(request.ClientGuid);

        //        RuleTemplateDTO ruleTemplate = await getRuleTemplate_DynamicPA(clientConnectionString, dataset?.Name, request).ConfigureAwait(false);

        //        DataValidationHelper.ValidatePharmacy(PharmacyRepository, clientConnectionString, request.PharmacyID, ruleTemplate.PPNID);

        //        List<string> enrolleeIds = await DataValidationHelper.GetEnrolleeIds(PBMRepository, clientConnectionString, dataset.MemberIdType, request.PlanID, request.MemberID, request.MemberEnrolleeID).ConfigureAwait(false);
        //        string enrolleeId = enrolleeIds.First();

        //        if (!DataValidationHelper.IsValidMember(PBMRepository, clientConnectionString, request.PlanID,
        //                          !string.IsNullOrWhiteSpace(request.MemberEnrolleeID) ? request.MemberEnrolleeID : enrolleeId,
        //                          out string errorMessage))
        //        {
        //            throw new ArgumentException(errorMessage);
        //        }

        //        response.MemberID = request.MemberID;
        //        response.MemberEnrolleeID = enrolleeId;
        //        response.OrganizationID = request.OrganizationID;
        //        response.GroupID = request.GroupID;
        //        response.PlanID = request.PlanID;
        //        response.VendorPANumber = request.VendorPANumber;
        //        //response.ProductIDQualifier = getProductIDQualifier_DynamicPA(ruleTemplate.CODETYPE);
        //        //response.ProductIDQualifierDescription = ruleTemplate.CODETYPE;
        //        //response.ProductID = ruleTemplate.CODES;
        //        //Per Sandi, reply with what was sent instead of what was written on the rule
        //        response.ProductIDQualifier = request.ProductIDQualifier;
        //        response.ProductIDQualifierDescription = getProductIDQualifierDescription_DynamicPA(request.ProductIDQualifier);
        //        response.ProductID = request.ProductID;
        //        response.ProductDescription = ruleTemplate.LN60;
        //        response.ProcarePANumber = new List<string>();

        //        string retailSysid = "", mailSysid = "";
        //        bool ruleAlreadyExists = false;

        //        if (dataset.ShowClaimsForAllMemberPlans)
        //        {
        //            bool exists = false;
        //            foreach (var enrid in enrolleeIds)
        //            {
        //                if (memberHasRule_DynamicPA(clientConnectionString, request.PlanID, enrid, ruleTemplate.VENDTYPE,
        //                                    ruleTemplate.CODETYPE, ruleTemplate.CODES, dataset.ShowClaimsForAllMemberPlans,
        //                                    out retailSysid, out mailSysid))
        //                {
        //                    exists = true;
        //                    break;
        //                }
        //            }

        //            ruleAlreadyExists = exists;
        //        }
        //        else
        //        {
        //            ruleAlreadyExists = memberHasRule_DynamicPA(clientConnectionString, request.PlanID, enrolleeId, ruleTemplate.VENDTYPE,
        //                                                ruleTemplate.CODETYPE, ruleTemplate.CODES, dataset.ShowClaimsForAllMemberPlans,
        //                                                out retailSysid, out mailSysid);
        //        }

        //        //Check if member already has this rule
        //        if (ruleAlreadyExists)
        //        {
        //            //Update rule
        //            if (!string.IsNullOrWhiteSpace(mailSysid))
        //            {
        //                response.ProcarePANumber.Add(request.Client.ToUpper() +
        //                                                await saveMemberRule_DynamicPA(clientConnectionString, dataset.Name, mailSysid, enrolleeId, request,
        //                                                                    ruleTemplate, nameof(VendorType.MAIL), userID, request.OrganizationID,
        //                                                                    dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
        //            }

        //            if (!string.IsNullOrWhiteSpace(retailSysid) && !dataset.ShowClaimsForAllMemberPlans)
        //            {
        //                response.ProcarePANumber.Add(request.Client.ToUpper() +
        //                                                await saveMemberRule_DynamicPA(clientConnectionString, dataset.Name, retailSysid, enrolleeId, request,
        //                                                                    ruleTemplate, nameof(VendorType.RETL), userID, request.OrganizationID,
        //                                                                    dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
        //            }
        //        }
        //        else
        //        {
        //            //Add rule
        //            if (!dataset.ShowClaimsForAllMemberPlans && ruleTemplate.VENDTYPE.EqualsIgnoreCase(nameof(VendorType.BOTH)))
        //            {
        //                response.ProcarePANumber.Add(request.Client.ToUpper() +
        //                                                await saveMemberRule_DynamicPA(clientConnectionString, dataset.Name, null, enrolleeId, request,
        //                                                                    ruleTemplate, nameof(VendorType.MAIL), userID, request.OrganizationID,
        //                                                                    dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
        //                response.ProcarePANumber.Add(request.Client.ToUpper() +
        //                                                await saveMemberRule_DynamicPA(clientConnectionString, dataset.Name, null, enrolleeId, request,
        //                                                                    ruleTemplate, nameof(VendorType.RETL), userID, request.OrganizationID,
        //                                                                    dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
        //            }
        //            else
        //            {
        //                response.ProcarePANumber.Add(request.Client.ToUpper() +
        //                                                await saveMemberRule_DynamicPA(clientConnectionString, dataset.Name, null, enrolleeId, request,
        //                                                                    ruleTemplate, ruleTemplate.VENDTYPE, userID, request.OrganizationID,
        //                                                                    dataset.ShowClaimsForAllMemberPlans).ConfigureAwait(false));
        //            }
        //        }

        //        return response;
        //    }).ConfigureAwait(false);

        //    return response;
        //}

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.MemberRuleRequest + "|" + ApiRoutes.MemberRuleRequest)]
        public async Task<MemberRuleResponse> Post(MemberRuleRequest request)
        {
            var response = new MemberRuleResponse();
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            response = await processRequest(request, ApiRoutes.MemberRuleRequest, async () =>
            {
                // Retrieve client dataset from configuration
                Dictionary<string, string> clientSetting = await ClientConfigHelper.GetClientConfigs(commonApiHelper, request.ClientGuid).ConfigureAwait(false);
                DatasetDTO dataset = ClientConfigHelper.GetClientDataset(clientSetting, request.Client, null, null, null, request.PlanID);

                var clientConnectionString = ClientConfigHelper.GetConnectionStringFromDataset(dataset);

                // Get the rule template to use, if applicable
                var ruleTemplate = new RuleTemplateDTO();
                if (!string.IsNullOrWhiteSpace(request.MemberRuleTemplateID))
                {
                    ruleTemplate = await getRuleTemplate_MemberRule(clientConnectionString,
                                                                     dataset?.Name,
                                                                     request.MemberRuleTemplateID,
                                                                     request.NDC,
                                                                     request.Client).ConfigureAwait(false);
                }


                // Get the Enrollee Ids needed for processing
                string enrolleeId = await DataValidationHelper.GetEnrolleeId(PBMRepository, clientConnectionString,
                                                          dataset.MemberIdType,
                                                          request.PlanID,
                                                          request.MemberID,
                                                          request.Person,
                                                          request.MemberEnrolleeID).ConfigureAwait(false);

                // Check for existing member rules matching the requested rule
                List<MemberRuleDTO> rules = await getExistingRules_MemberRule(clientConnectionString, request.PlanID, enrolleeId,
                                                                                   ruleTemplate.VENDTYPE, ruleTemplate, request.MemberRuleID).ConfigureAwait(false);

                // Get Action
                MemberRuleAction action = getAction_MemberRule(request, rules);

                // If the request is for a new rule, determine the rule types to add
                if (action == MemberRuleAction.Insert)
                {
                    response.MemberRuleIds = await addRules_MemberRule(request, dataset, clientConnectionString, ruleTemplate, enrolleeId).ConfigureAwait(false);
                }
                else if (action == MemberRuleAction.Update)
                {
                    response.MemberRuleIds = await modifyRules_MemberRule(request, rules, dataset, clientConnectionString).ConfigureAwait(false);
                }
                else
                {
                    response.MemberRuleIds = rules.Select(x => x.SYSID).ToList();
                }

                response.Message = getMemberRuleMessage_MemberRule(action);

                return response;

            }).ConfigureAwait(false);

            return response;
        }
        #endregion

        #region Private Methods

        //#region Dynamic PA

        //private bool memberHasRule_DynamicPA(string adsConnectionString, string planId, string enrolleeId, string vendType, string codeType, string codes, bool ignorePlan, out string retailSysid, out string mailSysid)
        //{
        //    retailSysid = "";
        //    mailSysid = "";
        //    bool exists = false;

        //    if (ignorePlan)
        //    {
        //        retailSysid = MemberRuleRepository.RuleExists(adsConnectionString, null, enrolleeId, null, codeType, codes).Result;
        //        mailSysid = retailSysid;
        //    }
        //    else
        //    {
        //        switch (vendType)
        //        {
        //            case nameof(VendorType.RETL):
        //                retailSysid = MemberRuleRepository.RuleExists(adsConnectionString, planId, enrolleeId, nameof(VendorType.RETL), codeType, codes).Result;
        //                break;
        //            case nameof(VendorType.MAIL):
        //                mailSysid = MemberRuleRepository.RuleExists(adsConnectionString, planId, enrolleeId, nameof(VendorType.MAIL), codeType, codes).Result;
        //                break;
        //            case nameof(VendorType.BOTH):
        //                mailSysid = MemberRuleRepository.RuleExists(adsConnectionString, planId, enrolleeId, nameof(VendorType.MAIL), codeType, codes).Result;
        //                retailSysid = MemberRuleRepository.RuleExists(adsConnectionString, planId, enrolleeId, nameof(VendorType.RETL), codeType, codes).Result;
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    if (!string.IsNullOrWhiteSpace(retailSysid) || !string.IsNullOrWhiteSpace(mailSysid))
        //    {
        //        exists = true;
        //    }

        //    return exists;
        //}

        //private async Task<string> saveMemberRule_DynamicPA(string clientConnectionString, string clientName, string sysid, string enrolleeId, DynamicPARequest request, RuleTemplateDTO ruleTemplate, string vendType, string userName, string changedBy, bool ignorePlan)
        //{
        //    string newSysid = string.Empty;
        //    int? daysSupplyMaximum = request.DaysSupplyMaximum.ParseNullableInt();
        //    int? patientAgeMaximum = request.PatientAgeMaximum.ParseNullableInt();
        //    int? periodQuantityDays = request.PeriodQuantityDays.ParseNullableInt();
        //    int? numberOfFills = request.NumberOfFills.ParseNullableInt();
        //    double? periodQuantityMaximum = request.PeriodQuantityMaximum.ParseNullableInt();
        //    double? amountDueMaximum = request.AmountDueMaximum.ParseNullableInt();

        //    DateTime? fromDate = null;
        //    DateTime? toDate = null;

        //    if (!string.IsNullOrWhiteSpace(request.RequestFromDate))
        //    {
        //        fromDate = DateTime.ParseExact(request.RequestFromDate, "yyyyMMdd", null);
        //    }

        //    if (!string.IsNullOrWhiteSpace(request.RequestToDate))
        //    {
        //        toDate = DateTime.ParseExact(request.RequestToDate, "yyyyMMdd", null);
        //    }
        //    string includeExcludeValue = "";

        //    if (!string.IsNullOrWhiteSpace(request.PharmacyIncludeExclude))
        //    {
        //        switch (request.PharmacyIncludeExclude.ToUpper())
        //        {
        //            case "I":
        //                includeExcludeValue = "C";
        //                break;
        //            case "E":
        //                includeExcludeValue = "E";
        //                break;
        //        }
        //    }

        //    DateTime? effdt = fromDate ?? ruleTemplate.EFFDT;
        //    DateTime? trmdt = toDate ?? ruleTemplate.TRMDT;
        //    int? roundedAmountDueMaximum = null, magelo, fagelo;
        //    string sex, magemeth, fagemeth;

        //    if (amountDueMaximum.HasValue)
        //    {
        //        roundedAmountDueMaximum = (int)Math.Ceiling(amountDueMaximum.Value);
        //    }

        //    if (patientAgeMaximum.HasValue)
        //    {
        //        sex = "B";
        //        magemeth = "<";
        //        fagemeth = "<";
        //        magelo = patientAgeMaximum;
        //        fagelo = patientAgeMaximum;
        //    }
        //    else
        //    {
        //        sex = ruleTemplate.SEX;
        //        magemeth = ruleTemplate.MAGEMETH;
        //        fagemeth = ruleTemplate.FAGEMETH;
        //        magelo = ruleTemplate.MAGELO;
        //        fagelo = ruleTemplate.FAGELO;
        //    }

        //    string dsgid = ruleTemplate.DSGID;
        //    if (daysSupplyMaximum.HasValue && daysSupplyMaximum.Value > 0)
        //    {
        //        string lookupDSGID = await MemberRuleRepository.LookupDSGID(clientConnectionString, daysSupplyMaximum.Value).ConfigureAwait(false);

        //        if (!string.IsNullOrWhiteSpace(lookupDSGID))
        //        {
        //            dsgid = lookupDSGID;
        //        }
        //        else
        //        {
        //            throw new ArgumentException("The submitted DaysSupplyMaximum value was not found.");
        //        }
        //    }

        //    int? maxrefills = ruleTemplate.MAXREFILLS ?? 99;
        //    int? maxrefmnt = numberOfFills.HasValue ? numberOfFills - 1 : ruleTemplate.MAXREFMNT;
        //    string pharmacyId = string.IsNullOrWhiteSpace(request.PharmacyID) ? ruleTemplate.PPNID : request.PharmacyID;
        //    string includeExclude = string.IsNullOrWhiteSpace(includeExcludeValue) ? ruleTemplate.PPNREQRUL : includeExcludeValue;
        //    int? hidollar = roundedAmountDueMaximum ?? ruleTemplate.HIDOLLAR;
        //    double? qtyperdys = periodQuantityMaximum ?? ruleTemplate.QTYPERDYS;
        //    int? qtydylmt = periodQuantityDays ?? ruleTemplate.QTYDYLMT;

        //    //Story 20337 - Always write blank CopayGCI/MultisourceCode
        //    //List<string> validMultisourceCodes = new List<string> { "M", "O", "N", "Y" };
        //    //string validatedMultisourceCode = validMultisourceCodes.Contains(request.MultisourceCode.ToUpper()) ? request.MultisourceCode : "";
        //    string multisourceCode = ""; //string.IsNullOrWhiteSpace(validatedMultisourceCode) ? ruleTemplate.COPAYGCI : validatedMultisourceCode;

        //    string trmDtPaidMsgText = trmdt != null ? $" thru {trmdt.Value:MM/dd/yyyy}" : "";
        //    string paidMsg = $"Approved{trmDtPaidMsgText}, {request.VendorPANumber}";

        //    if (string.IsNullOrWhiteSpace(sysid))
        //    {
        //        newSysid = await addMemberRule_DynamicPA(clientConnectionString, clientName, enrolleeId, request, ruleTemplate, vendType, effdt, trmdt, sex,
        //                                   magemeth, fagemeth, magelo, fagelo, dsgid, maxrefills, maxrefmnt, pharmacyId, includeExclude, hidollar,
        //                                   qtyperdys, qtydylmt, multisourceCode, paidMsg, userName, changedBy, ignorePlan);
        //    }
        //    else
        //    {
        //        newSysid = await updateMemberRule_DynamicPA(clientConnectionString, clientName, sysid, enrolleeId, request, ruleTemplate, vendType, effdt, trmdt, sex,
        //                                   magemeth, fagemeth, magelo, fagelo, dsgid, maxrefills, maxrefmnt, pharmacyId, includeExclude, hidollar,
        //                                   qtyperdys, qtydylmt, multisourceCode, paidMsg, userName, changedBy, ignorePlan);
        //    }

        //    return newSysid;
        //}

        //private async Task<string> addMemberRule_DynamicPA(string clientConnectionString, string clientName, string enrolleeId, DynamicPARequest request,
        //                                         RuleTemplateDTO ruleTemplate, string vendType, DateTime? effdt, DateTime? trmdt, string sex,
        //                                         string magemeth, string fagemeth, int? magelo, int? fagelo, string dsgid, int? maxrefills,
        //                                         int? maxrefmnt, string pharmacyId, string includeExclude, int? hidollar, double? qtyperdys,
        //                                         int? qtydylmt, string multisourceCode, string paidMsg, string userName, string changedBy, bool ignorePlan)
        //{
        //    return await MemberRuleRepository.AddMemberRule(
        //        clientConnectionString
        //        , clientName
        //        , request.PlanID
        //        , request.MemberID
        //        , enrolleeId
        //        , ruleTemplate.CODES
        //        , ruleTemplate.EPA_ID
        //        , ruleTemplate.DESC
        //        , ruleTemplate.TYPE
        //        , effdt
        //        , trmdt
        //        , ruleTemplate.CODETYPE
        //        , vendType
        //        , ruleTemplate.PADENIED
        //        , sex
        //        , magemeth
        //        , fagemeth
        //        , magelo
        //        , ruleTemplate.MAGEHI
        //        , fagelo
        //        , ruleTemplate.FAGEHI
        //        , ruleTemplate.APPLYACC
        //        , ruleTemplate.BAPPACC
        //        , dsgid
        //        , ruleTemplate.DSGID2
        //        , ruleTemplate.CALCREFILL
        //        , ruleTemplate.REFILLDAYS
        //        , ruleTemplate.REFILLMETH
        //        , ruleTemplate.REFILLPCT
        //        , maxrefills
        //        , maxrefmnt
        //        , ruleTemplate.PENALTY
        //        , ruleTemplate.DESI
        //        , ruleTemplate.PHYLIMIT
        //        , ruleTemplate.GI_GPI
        //        , ruleTemplate.PPGID
        //        , pharmacyId
        //        , includeExclude
        //        , ruleTemplate.INCCOMP
        //        , ruleTemplate.BRANDDISC
        //        , ruleTemplate.GENONLY
        //        , ruleTemplate.DRUGCLASS
        //        , ruleTemplate.DRUGTYPE
        //        , ruleTemplate.DRUGSTAT
        //        , ruleTemplate.MAINTIND
        //        , ruleTemplate.COMPMAX
        //        , hidollar
        //        , qtyperdys
        //        , qtydylmt
        //        , multisourceCode
        //        , ruleTemplate.COPLVLASSN
        //        , ruleTemplate.OVRRJTADI
        //        , ruleTemplate.OVRRJTAGE
        //        , ruleTemplate.OVRRJTADD
        //        , ruleTemplate.OVRRJTDDC
        //        , ruleTemplate.OVRRJTDOT
        //        , ruleTemplate.OVRRJTDUP
        //        , ruleTemplate.OVRRJTIAT
        //        , ruleTemplate.OVRRJTMMA
        //        , ruleTemplate.OVRRJTLAC
        //        , ruleTemplate.OVRRJTPRG
        //        , ruleTemplate.ACTIVE
        //        , request.Note
        //        , null
        //        , request.VendorPANumber
        //        , paidMsg
        //        , ruleTemplate.REASON
        //        , userName
        //        , changedBy
        //        , ignorePlan).ConfigureAwait(false);
        //}

        //private async Task<string> updateMemberRule_DynamicPA(string clientConnectionString, string clientName, string sysid, string enrolleeId,
        //                                            DynamicPARequest request, RuleTemplateDTO ruleTemplate, string vendType, DateTime? effdt,
        //                                            DateTime? trmdt, string sex, string magemeth, string fagemeth, int? magelo, int? fagelo,
        //                                            string dsgid, int? maxrefills, int? maxrefmnt, string pharmacyId, string includeExclude,
        //                                            int? hidollar, double? qtyperdys, int? qtydylmt, string multisourceCode, string paidMsg,
        //                                            string userName, string changedBy, bool ignorePlan)
        //{
        //    return await MemberRuleRepository.UpdateMemberRule(
        //        clientConnectionString
        //        , clientName
        //        , sysid
        //        , request.PlanID
        //        , request.MemberID
        //        , enrolleeId
        //        , ruleTemplate.CODES
        //        , ruleTemplate.EPA_ID
        //        , ruleTemplate.DESC
        //        , ruleTemplate.TYPE
        //        , effdt
        //        , trmdt
        //        , ruleTemplate.CODETYPE
        //        , vendType
        //        , ruleTemplate.PADENIED
        //        , sex
        //        , magemeth
        //        , fagemeth
        //        , magelo
        //        , ruleTemplate.MAGEHI
        //        , fagelo
        //        , ruleTemplate.FAGEHI
        //        , ruleTemplate.APPLYACC
        //        , ruleTemplate.BAPPACC
        //        , dsgid
        //        , ruleTemplate.DSGID2
        //        , ruleTemplate.CALCREFILL
        //        , ruleTemplate.REFILLDAYS
        //        , ruleTemplate.REFILLMETH
        //        , ruleTemplate.REFILLPCT
        //        , maxrefills
        //        , maxrefmnt
        //        , ruleTemplate.PENALTY
        //        , ruleTemplate.DESI
        //        , ruleTemplate.PHYLIMIT
        //        , ruleTemplate.GI_GPI
        //        , ruleTemplate.PPGID
        //        , pharmacyId
        //        , includeExclude
        //        , ruleTemplate.INCCOMP
        //        , ruleTemplate.BRANDDISC
        //        , ruleTemplate.GENONLY
        //        , ruleTemplate.DRUGCLASS
        //        , ruleTemplate.DRUGTYPE
        //        , ruleTemplate.DRUGSTAT
        //        , ruleTemplate.MAINTIND
        //        , ruleTemplate.COMPMAX
        //        , hidollar
        //        , qtyperdys
        //        , qtydylmt
        //        , multisourceCode
        //        , ruleTemplate.COPLVLASSN
        //        , ruleTemplate.OVRRJTADI
        //        , ruleTemplate.OVRRJTAGE
        //        , ruleTemplate.OVRRJTADD
        //        , ruleTemplate.OVRRJTDDC
        //        , ruleTemplate.OVRRJTDOT
        //        , ruleTemplate.OVRRJTDUP
        //        , ruleTemplate.OVRRJTIAT
        //        , ruleTemplate.OVRRJTMMA
        //        , ruleTemplate.OVRRJTLAC
        //        , ruleTemplate.OVRRJTPRG
        //        , ruleTemplate.ACTIVE
        //        , request.Note
        //        , null
        //        , request.VendorPANumber
        //        , paidMsg
        //        , ruleTemplate.REASON
        //        , userName
        //        , changedBy
        //        , ignorePlan).ConfigureAwait(false);
        //}

        //private static string getProductIDQualifier_DynamicPA(string codeType)
        //{
        //    Enum.TryParse(codeType, out ProductType parsedCodeType);

        //    if (parsedCodeType == ProductType.Unknown)
        //    {
        //        parsedCodeType = ProductType.Other;
        //    }

        //    return ((int)parsedCodeType).ToString().PadLeft(2, '0');
        //}

        //private string getProductIDQualifierDescription_DynamicPA(string qualifier)
        //{
        //    ProductType parsedCodeType = (ProductType)int.Parse(qualifier);

        //    if (parsedCodeType == ProductType.Unknown)
        //    {
        //        parsedCodeType = ProductType.Other;
        //    }

        //    return parsedCodeType.ToString();
        //}

        //private async Task<RuleTemplateDTO> getRuleTemplate_DynamicPA(string clientConnectionString, string datasetName, DynamicPARequest request)
        //{

        //    RuleTemplateDTO ruleTemplate = await MemberRuleRepository
        //                                         .GetDynamicPARuleTemplate(clientConnectionString, datasetName, request.DynamicPACode2, request.ProductID)
        //                                         .ConfigureAwait(false);

        //    //Error if a rule template was not found
        //    if (string.IsNullOrWhiteSpace(ruleTemplate.EPA_ID))
        //    {
        //        throw new ArgumentException($"Rule template not found for {request.Client} submitted Dynamic PA Code 2 ({request.DynamicPACode2}).");
        //    }

        //    if (string.IsNullOrWhiteSpace(ruleTemplate.CODES))
        //    {
        //        throw new ArgumentException($"NDC {request.ProductID} not found.");
        //    }

        //    return ruleTemplate;
        //}
        //#endregion

        #region Member Rules
        private string getMemberRuleMessage_MemberRule(MemberRuleAction action)
        {
            var message = "The requested rule was";
            switch (action)
            {
                case MemberRuleAction.Insert:
                    message = $"{message} created.";
                    break;
                case MemberRuleAction.Update:
                    message = $"{message} updated.";
                    break;
                default:
                    message = "No changes were made to the requested rule.";
                    break;
            }

            return message;
        }

        private async Task<List<string>> insertRules_MemberRule(string clientConnectionString, string datasetName,
                                                             List<MemberRuleDTO> rules)
        {
            List<string> sysids = new List<string>();

            foreach (var rule in rules)
            {
                var sysid = await MemberRuleRepository.AddMemberRule_MemberRule(clientConnectionString, datasetName, rule);

                sysids.Add(sysid);
            }

            return sysids;
        }

        private async Task<List<string>> updateRules_MemberRule(string clientConnectionString, string datasetName,
                                                             List<MemberRuleDTO> rules)
        {
            List<string> sysids = new List<string>();

            foreach (var rule in rules)
            {
                var sysid = await MemberRuleRepository.UpdateMemberRule_MemberRule(clientConnectionString, datasetName, rule);

                sysids.Add(sysid);
            }

            return sysids;
        }

        private async Task<List<string>> addRules_MemberRule(MemberRuleRequest request,
                                                  DatasetDTO dataset,
                                                  string clientConnectionString,
                                                  RuleTemplateDTO ruleTemplate,
                                                  string enrolleeId)
        {
            List<MemberRuleDTO> rules = buildNewMemberRules_MemberRule(request, ruleTemplate, enrolleeId);

            //Add the member's rule(s)
            List<string> sysids = await insertRules_MemberRule(clientConnectionString, dataset.Name, rules);

            return sysids;
        }

        private async Task<List<string>> modifyRules_MemberRule(MemberRuleRequest request, List<MemberRuleDTO> rules,
                                                  DatasetDTO dataset,
                                                  string clientConnectionString)
        {
            List<MemberRuleDTO> ruleUpdates = buildUpdatedMemberRules_MemberRule(request, rules);

            //Update the member's rule(s)
            List<string> sysids = await updateRules_MemberRule(clientConnectionString, dataset.Name, ruleUpdates).ConfigureAwait(false);

            return sysids;
        }

        private List<MemberRuleDTO> buildNewMemberRules_MemberRule(MemberRuleRequest request,
                                                  RuleTemplateDTO ruleTemplate,
                                                  string enrolleeId)
        {
            List<MemberRuleDTO> rules = getRuleVendorTypeBases_MemberRule(ruleTemplate.VENDTYPE);

            rules.ForEach(dto =>
            {
                setDtoValues_FromTemplate_MemberRule(ref dto, enrolleeId, ruleTemplate);
                setDtoRequestValues_MemberRule(ref dto, request);
                setDtoLoggingFields_MemberRule(ref dto, ClientConfigHelper.GetUserIDFromClientGuid(request.ClientGuid));
            });
            
            return rules;
        }

        private List<MemberRuleDTO> buildUpdatedMemberRules_MemberRule(MemberRuleRequest request, List<MemberRuleDTO> rules)
        {
            List<MemberRuleDTO> ruleUpdates = new List<MemberRuleDTO>();

            rules.ForEach(dto =>
            {
                MemberRuleDTO updateDto = new MemberRuleDTO
                {
                    SYSID = dto.SYSID,
                    ENRID = dto.ENRID,
                    PLNID = dto.PLNID
                };

                setDtoDateValues_MemberRule(ref updateDto, request);
                setDtoLoggingFields_MemberRule(ref updateDto, ClientConfigHelper.GetUserIDFromClientGuid(request.ClientGuid));

                ruleUpdates.Add(updateDto);
            });

            return ruleUpdates;
        }

        private List<MemberRuleDTO> getRuleVendorTypeBases_MemberRule(string vendType)
        {
            List<MemberRuleDTO> ruleBases = new List<MemberRuleDTO>();

            getRulesTypesToSave_MemberRule(vendType).ForEach(type =>
            {
                ruleBases.Add(new MemberRuleDTO { VENDTYPE = type });
            });

            return ruleBases;
        }

        private MemberRuleAction getAction_MemberRule(MemberRuleRequest request, List<MemberRuleDTO> rules)
        {
            MemberRuleAction actionBeingPerformed;

            // New
            if (!rules.Any())
            {
                if (!string.IsNullOrWhiteSpace(request.MemberRuleTemplateID))
                {
                    actionBeingPerformed = MemberRuleAction.Insert;
                }
                else
                {
                    throw new ArgumentException($"The rule submitted ({request.MemberRuleID}) could not be found.");
                }
            }
            else
            {
                MemberRuleDTO rule = rules.FirstOrDefault();

                // No change
                if (rule.EFFDT == request.EffectiveDate
                && rule.TRMDT == request.TerminationDate)
                {
                    actionBeingPerformed = MemberRuleAction.NoChange;
                }
                else
                {
                    actionBeingPerformed = MemberRuleAction.Update;
                }
            }

            return actionBeingPerformed;
        }

        private async Task<RuleTemplateDTO> getRuleTemplate_MemberRule(string clientConnectionString,
                                                            string datasetName,
                                                            string templateId,
                                                            string productId,
                                                            string client)
        {

            RuleTemplateDTO ruleTemplate = await MemberRuleRepository
                                                 .GetTemplateById(clientConnectionString, datasetName, templateId, productId)
                                                 .ConfigureAwait(false);

            //Error if a rule template was not found
            if (string.IsNullOrWhiteSpace(ruleTemplate.EPA_ID))
            {
                throw new ArgumentException($"Rule template not found for {client} and template ({templateId}).");
            }

            if (string.IsNullOrWhiteSpace(ruleTemplate.CODES))
            {
                throw new ArgumentException($"NDC {productId} not found.");
            }

            return ruleTemplate;
        }

        private async Task<List<MemberRuleDTO>> getExistingRules_MemberRule(string adsConnectionString,
                                                                string planId,
                                                                string enrolleeId,
                                                                string vendType,
                                                                RuleTemplateDTO template,
                                                                string sysId)
        {
            bool useBothVendTypes = vendType == nameof(VendorType.BOTH);

            List<MemberRuleDTO> existingRules = await MemberRuleRepository.GetRules(adsConnectionString, planId, enrolleeId, useBothVendTypes ? "" : vendType, template.CODETYPE, template.CODES, sysId).ConfigureAwait(false);

            return existingRules;
        }

        private static List<string> getRulesTypesToSave_MemberRule(string vendType)
        {
            List<string> rules = new List<string>();

            if (vendType == nameof(VendorType.BOTH))
            {
                rules.Add(nameof(VendorType.MAIL));
                rules.Add(nameof(VendorType.RETL));
            }
            else
            {
                rules.Add(vendType);
            }

            return rules;
        }

        private MemberRuleDTO setDtoRequestValues_MemberRule(ref MemberRuleDTO dto, MemberRuleRequest request)
        {
            setDtoDateValues_MemberRule(ref dto, request);

            dto.CLIENTNAME = request.Client;
            dto.PLNID = request.PlanID;

            string trmDtPaidMsgText = dto.TRMDT != null ? $" thru {dto.TRMDT.Value:MM/dd/yyyy}" : "";
            dto.PAIDMSG = $"Approved{trmDtPaidMsgText}";
            
            dto.VENDORPANUMBER = string.Empty;

            return dto;
        }

        private MemberRuleDTO setDtoDateValues_MemberRule(ref MemberRuleDTO dto, MemberRuleRequest request)
        {
            dto.EFFDT = request.EffectiveDate;
            dto.TRMDT = request.TerminationDate;

            return dto;
        }

        private void setDtoLoggingFields_MemberRule(ref MemberRuleDTO dto, string userID)
        {
            dto.USERNAME = userID;
            dto.CHANGEDBY = userID;
        }

        private void setDtoValues_FromTemplate_MemberRule(ref MemberRuleDTO dto, string enrolleeId,
                                                          RuleTemplateDTO ruleTemplate)
        {
            dto.ENRID = enrolleeId;
            dto.VENDTYPE = dto.VENDTYPE;
            dto.EFFDT = ruleTemplate.EFFDT;
            dto.TRMDT = ruleTemplate.TRMDT;
            dto.SEX = ruleTemplate.SEX;
            dto.MAGEMETH = ruleTemplate.MAGEMETH;
            dto.FAGEMETH = ruleTemplate.FAGEMETH;
            dto.MAGELO = ruleTemplate.MAGELO;
            dto.FAGELO = ruleTemplate.FAGELO;
            dto.DSGID = ruleTemplate.DSGID;
            dto.MAXREFILLS = ruleTemplate.MAXREFILLS ?? 99;
            dto.MAXREFMNT = ruleTemplate.MAXREFMNT;
            dto.PHARMACYID = ruleTemplate.PPNID;
            dto.INCLUDEEXCLUDE = ruleTemplate.PPNREQRUL;
            dto.HIDOLLAR = ruleTemplate.HIDOLLAR;
            dto.QTYPERDYS = ruleTemplate.QTYPERDYS;
            dto.QTYDYLMT = ruleTemplate.QTYDYLMT;
            dto.MULTISOURCECODE = string.Empty;
            dto.CODES = ruleTemplate.CODES;
            dto.TYPE = ruleTemplate.TYPE;
            dto.CODETYPE = ruleTemplate.CODETYPE;
            dto.MAGEHI = ruleTemplate.MAGEHI;
            dto.FAGEHI = ruleTemplate.FAGEHI;
            dto.APPLYACC = ruleTemplate.APPLYACC;
            dto.BAPPACC = ruleTemplate.BAPPACC;
            dto.DSGID2 = ruleTemplate.DSGID2;
            dto.CALCREFILL = ruleTemplate.CALCREFILL;
            dto.REFILLDAYS = ruleTemplate.REFILLDAYS;
            dto.REFILLMETH = ruleTemplate.REFILLMETH;
            dto.REFILLPCT = ruleTemplate.REFILLPCT;
            dto.PENALTY = ruleTemplate.PENALTY;
            dto.DESI = ruleTemplate.DESI;
            dto.PHYLIMIT = ruleTemplate.PHYLIMIT;
            dto.GI_GPI = ruleTemplate.GI_GPI;
            dto.PPGID = ruleTemplate.PPGID;
            dto.INCCOMP = ruleTemplate.INCCOMP;
            dto.BRANDDISC = ruleTemplate.BRANDDISC;
            dto.GENONLY = ruleTemplate.GENONLY;
            dto.DRUGCLASS = ruleTemplate.DRUGCLASS;
            dto.DRUGTYPE = ruleTemplate.DRUGTYPE;
            dto.DRUGSTAT = ruleTemplate.DRUGSTAT;
            dto.MAINTIND = ruleTemplate.MAINTIND;
            dto.COMPMAX = ruleTemplate.COMPMAX;
            dto.COPLVLASSN = ruleTemplate.COPLVLASSN;
            dto.OVRRJTADI = ruleTemplate.OVRRJTADI;
            dto.OVRRJTAGE = ruleTemplate.OVRRJTAGE;
            dto.OVRRJTADD = ruleTemplate.OVRRJTADD;
            dto.OVRRJTDDC = ruleTemplate.OVRRJTDDC;
            dto.OVRRJTDOT = ruleTemplate.OVRRJTDOT;
            dto.OVRRJTDUP = ruleTemplate.OVRRJTDUP;
            dto.OVRRJTIAT = ruleTemplate.OVRRJTIAT;
            dto.OVRRJTMMA = ruleTemplate.OVRRJTMMA;
            dto.OVRRJTLAC = ruleTemplate.OVRRJTLAC;
            dto.OVRRJTPRG = ruleTemplate.OVRRJTPRG;
            dto.NOTE = null;
            dto.REASON = ruleTemplate.REASON;
        }
        #endregion

        #endregion
    }
}
