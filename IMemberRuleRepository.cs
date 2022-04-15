using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public interface IMemberRuleRepository
    {
        Task<string> RuleExists(string adsConnectionString, string planId, string enrolleeId, string vendType, string codeType,
                                           string codes);
        Task<RuleTemplateDTO> GetDynamicPARuleTemplate(string adsConnectionString, string clientName, string dynamicPACode2, string productID);
        Task<string> AddMemberRule_DynamicPA(string adsConnectionString, string clientName, string PLNID, string memberId, string enrolleeId, string CODES,
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
                                                string vendorPANumber, string paidMsg, string REASON, string USERNAME, string CHANGEDBY, bool ignorePlan);
        Task<string> UpdateMemberRule_DynamicPA(string adsConnectionString, string clientName, string SYSID, string PLNID, string memberId, string enrolleeId, string CODES,
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
                                        string vendorPANumber, string paidMsg, string REASON, string USERNAME, string CHANGEDBY, bool ignorePlan);
        
        Task<List<MemberRuleDTO>> GetRules(string adsConnectionString, string planId, string enrolleeId, string vendType, string codeType, string codes, string sysId);
        Task<RuleTemplateDTO> GetTemplateById(string adsConnectionString, string clientName, string templateId, string productId);
        Task<string> LookupDSGID(string adsConnectionString, int daysSupplyMaximum);
        Task<string> AddMemberRule_MemberRule(string adsConnectionString, string clientName, MemberRuleDTO dto);
        Task<string> UpdateMemberRule_MemberRule(string adsConnectionString, string clientName, MemberRuleDTO dto);
    }
}