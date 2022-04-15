using ProCare.API.PBM.Repository.DataAccess.Rule;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public class MemberRuleRepository : BasedbRepository, IMemberRuleRepository
    {
        public MemberRuleRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        #region Dynamic PA
        /// <summary>
        ///  Check whether a rule already exists in the database.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="planId">String representing the identifier of the plan</param>
        /// <param name="enrolleeId">String representing the ProCare identifier of the enrollee</param>
        /// <param name="vendType">String representing the vend type of the plan</param>
        /// <param name="codeType">String representing the code type of the product code (NDC, GCN, or GNN)</param>
        /// <param name="codes">String representing the submitted product code after being translated to the code type on the rule</param>
        /// <returns><see cref="string" /> representing the system identifier of the rule if found</returns>
        public async Task<string> RuleExists(string adsConnectionString, string planId, string enrolleeId, string vendType, string codeType,
                                           string codes)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            string output = await adsHelper.CheckRuleExists(planId, enrolleeId, vendType, codeType, codes).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Get the default template for the DynamicPACode2 and ProductID
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="dynamicPACode2">String representing the identifier of the rule template to be used</param>
        /// <param name="productID">String representing the submitted product code</param>
        /// <returns><see cref="string" /> representing the system identifier of the rule if found</returns>
        public async Task<RuleTemplateDTO> GetDynamicPARuleTemplate(string adsConnectionString, string clientName, string dynamicPACode2, string productID)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            RuleTemplateDTO output = await adsHelper.ReadDynamicPARuleTemplate(clientName, dynamicPACode2, productID).ConfigureAwait(false);

            return output;
        }

        public async Task<string> AddMemberRule_DynamicPA(string adsConnectionString, string clientName, string PLNID, string memberId, string enrolleeId, string CODES,
                                                string EPA_ID, string DESC, string TYPE, DateTime? EFFDT, DateTime? TRMDT, string CODETYPE, string VENDTYPE,
                                                string PADENIED, string SEX, string MAGEMETH, string FAGEMETH, int? MAGELO, int? MAGEHI,
                                                int? FAGELO, int? FAGEHI, string APPLYACC, string BAPPACC, string DSGID, string DSGID2,
                                                string CALCREFILL, int? REFILLDAYS, string REFILLMETH, int? REFILLPCT, int? MAXREFILLS,
                                                int? MAXREFMNT, string PENALTY, string DESI, string PHYLIMIT, string GI_GPI, string PPGID,
                                                string PPNID, string PPNREQRUL, string INCCOMP, string BRANDDISC, string GENONLY, string DRUGCLASS,
                                                string DRUGTYPE, string DRUGSTAT, string MAINTIND, int? COMPMAX, int? HIDOLLAR, double? QTYPERDYS,
                                                int? QTYDYLMT, string COPAYGCI, string COPLVLASSN, string OVRRJTADI, string OVRRJTAGE,
                                                string OVRRJTADD, string OVRRJTDDC, string OVRRJTDOT, string OVRRJTDUP, string OVRRJTIAT,
                                                string OVRRJTMMA, string OVRRJTLAC, string OVRRJTPRG, string ACTIVE, string Note, string pharmacyID,
                                                string vendorPANumber, string paidMsg, string REASON, string USERNAME, string CHANGEDBY, bool ignorePlan)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            string output = await adsHelper.InsertMemberRule_DynamicPA(clientName, PLNID, memberId, enrolleeId, CODES, EPA_ID, DESC, TYPE, EFFDT, TRMDT, CODETYPE, VENDTYPE,
                                                             PADENIED, SEX, MAGEMETH, FAGEMETH, MAGELO, MAGEHI, FAGELO, FAGEHI, APPLYACC, BAPPACC,
                                                             DSGID, DSGID2, CALCREFILL, REFILLDAYS, REFILLMETH, REFILLPCT, MAXREFILLS, MAXREFMNT,
                                                             PENALTY, DESI, PHYLIMIT, GI_GPI, PPGID, PPNID, PPNREQRUL, INCCOMP, BRANDDISC, GENONLY,
                                                             DRUGCLASS, DRUGTYPE, DRUGSTAT, MAINTIND, COMPMAX, HIDOLLAR, QTYPERDYS, QTYDYLMT,
                                                             COPAYGCI, COPLVLASSN, OVRRJTADI, OVRRJTAGE, OVRRJTADD, OVRRJTDDC, OVRRJTDOT, OVRRJTDUP,
                                                             OVRRJTIAT, OVRRJTMMA, OVRRJTLAC, OVRRJTPRG, ACTIVE, Note, pharmacyID, vendorPANumber, paidMsg, REASON,
                                                             USERNAME, CHANGEDBY, ignorePlan)
                                           .ConfigureAwait(false);

            return output;
        }

        public async Task<string> UpdateMemberRule_DynamicPA(string adsConnectionString, string clientName, string SYSID, string PLNID, string memberId, string enrolleeId, string CODES,
                                        string EPA_ID, string DESC, string TYPE, DateTime? EFFDT, DateTime? TRMDT, string CODETYPE, string VENDTYPE,
                                        string PADENIED, string SEX, string MAGEMETH, string FAGEMETH, int? MAGELO, int? MAGEHI,
                                        int? FAGELO, int? FAGEHI, string APPLYACC, string BAPPACC, string DSGID, string DSGID2,
                                        string CALCREFILL, int? REFILLDAYS, string REFILLMETH, int? REFILLPCT, int? MAXREFILLS,
                                        int? MAXREFMNT, string PENALTY, string DESI, string PHYLIMIT, string GI_GPI, string PPGID,
                                        string PPNID, string PPNREQRUL, string INCCOMP, string BRANDDISC, string GENONLY, string DRUGCLASS,
                                        string DRUGTYPE, string DRUGSTAT, string MAINTIND, int? COMPMAX, int? HIDOLLAR, double? QTYPERDYS,
                                        int? QTYDYLMT, string COPAYGCI, string COPLVLASSN, string OVRRJTADI, string OVRRJTAGE,
                                        string OVRRJTADD, string OVRRJTDDC, string OVRRJTDOT, string OVRRJTDUP, string OVRRJTIAT,
                                        string OVRRJTMMA, string OVRRJTLAC, string OVRRJTPRG, string ACTIVE, string Note, string pharmacyID,
                                        string vendorPANumber, string paidMsg, string REASON, string USERNAME, string CHANGEDBY, bool ignorePlan)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            string output = await adsHelper.UpdateMemberRule_DynamicPA(clientName, SYSID, PLNID, memberId, enrolleeId, CODES, EPA_ID, DESC, TYPE, EFFDT, TRMDT, CODETYPE, VENDTYPE,
                                                             PADENIED, SEX, MAGEMETH, FAGEMETH, MAGELO, MAGEHI, FAGELO, FAGEHI, APPLYACC, BAPPACC,
                                                             DSGID, DSGID2, CALCREFILL, REFILLDAYS, REFILLMETH, REFILLPCT, MAXREFILLS, MAXREFMNT,
                                                             PENALTY, DESI, PHYLIMIT, GI_GPI, PPGID, PPNID, PPNREQRUL, INCCOMP, BRANDDISC, GENONLY,
                                                             DRUGCLASS, DRUGTYPE, DRUGSTAT, MAINTIND, COMPMAX, HIDOLLAR, QTYPERDYS, QTYDYLMT,
                                                             COPAYGCI, COPLVLASSN, OVRRJTADI, OVRRJTAGE, OVRRJTADD, OVRRJTDDC, OVRRJTDOT, OVRRJTDUP,
                                                             OVRRJTIAT, OVRRJTMMA, OVRRJTLAC, OVRRJTPRG, ACTIVE, Note, pharmacyID, vendorPANumber, paidMsg, REASON,
                                                             USERNAME, CHANGEDBY, ignorePlan)
                                           .ConfigureAwait(false);

            return output;
        }

        #endregion

        #region Member Rule APIs
        /// <summary>
        ///  Check whether a rule already exists in the database.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="planId">String representing the identifier of the plan</param>
        /// <param name="enrolleeId">String representing the ProCare identifier of the enrollee</param>
        /// <param name="vendType">String representing the vend type of the plan</param>
        /// <param name="codeType">String representing the code type of the product code (NDC, GCN, or GNN)</param>
        /// <param name="codes">String representing the submitted product code after being translated to the code type on the rule</param>
        /// <returns><see cref="string" /> representing the system identifier of the rule if found</returns>
        public async Task<List<MemberRuleDTO>> GetRules(string adsConnectionString,
                                             string planId,
                                             string enrolleeId,
                                             string vendType,
                                             string codeType,
                                             string codes, 
                                             string sysId)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            return await adsHelper.GetRules(planId, enrolleeId, vendType, codeType, codes, sysId).ConfigureAwait(false);
        }

        public async Task<string> AddMemberRule_MemberRule(string adsConnectionString, string clientName, MemberRuleDTO dto)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            return await adsHelper.InsertMemberRule_MemberRule(clientName, dto).ConfigureAwait(false);
        }
        
        public async Task<string> UpdateMemberRule_MemberRule(string adsConnectionString, string clientName, MemberRuleDTO dto)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            return await adsHelper.UpdateMemberRule_MemberRule(clientName, dto).ConfigureAwait(false);
        }

        /// <summary>
        ///  Get the default template for the DynamicPACode2 and ProductID
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="templateId">String representing the identifier of the rule template to be used</param>
        /// <param name="productID">String representing the submitted product code</param>
        /// <returns><see cref="string" /> representing the system identifier of the rule if found</returns>
        public async Task<RuleTemplateDTO> GetTemplateById(string adsConnectionString, string clientName, string templateId, string productId)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            return await adsHelper.GetTemplateById(clientName, templateId, productId).ConfigureAwait(false);
        }

        /// <summary>
        ///  Looks up a DSGID by its DaysAmt and MaintDays values.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="daysSupplyMaximum">Integer representing the DaysAmt and MaintDays to use to lookup the DSGID</param>
        /// <returns><see cref="string" /> representing the DSGID that was looked up</returns>
        public async Task<string> LookupDSGID(string adsConnectionString, int daysSupplyMaximum)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));

            string output = await adsHelper.LookupDSGID(daysSupplyMaximum)
                                         .ConfigureAwait(false);

            return output;
        }

        #endregion

        #region Shared
        #endregion
    }
}
