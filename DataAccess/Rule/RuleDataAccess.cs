using System;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ProCare.API.PBM.Messages.Request;

namespace ProCare.API.PBM.Repository.DataAccess.Rule
{
    public class RuleDataAccess : DataAccessBase
    {
        #region Constructors

        public RuleDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods

        /// <summary>
        ///  Check whether a rule already exists in the database.
        /// </summary>
        /// <param name="planId">String representing the identifier of the plan</param>
        /// <param name="enrolleeId">String representing the ProCare identifier of the enrollee</param>
        /// <param name="vendType">String representing the vend type of the plan</param>
        /// <param name="codeType">String representing the code type of the product code (NDC, GCN, or GNN)</param>
        /// <param name="codes">String representing the submitted product code after being translated to the code type on the rule</param>
        /// <returns><see cref="string" /> representing the system identifier of the rule if found</returns>
        public async Task<string> CheckRuleExists(string planId, string enrolleeId, string vendType, string codeType,
                                           string codes)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@PLNID", planId?.ToUpper() },
                { "@ENRID", enrolleeId?.ToUpper() },
                { "@VENDTYPE", vendType?.ToUpper() },
                { "@CODETYPE", codeType?.ToUpper() },
                { "@CODES", codes?.ToUpper() }
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Rule_exists", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("RuleExists");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to validate whether rule already exists.");
                }

                return dbResult;
            });

            string result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get the default template for the DynamicPACode2 and ProductID
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="dynamicPACode2">String representing the identifier of the rule template to be used</param>
        /// <param name="productID">String representing the submitted product code</param>
        /// <returns><see cref="RuleTemplateDTO" /> representing the templated rule that was generated</returns>
        public async Task<RuleTemplateDTO> ReadDynamicPARuleTemplate(string clientName, string dynamicPACode2, string productID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@EPA_ID", dynamicPACode2?.ToUpper() },
                { "@NDC", productID?.ToUpper() }
            };

            Task<RuleTemplateDTO> t = Task.Run(() =>
            {
                RuleTemplateDTO dbResult = new RuleTemplateDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Rule_Template_byEPA_IDNDC", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult.LoadFromDataReader(reader);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to lookup template in {clientName} dataset.");
                }

                return dbResult;
            });

            RuleTemplateDTO result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<string> InsertMemberRule_DynamicPA(string clientName, string PLNID, string memberId, string enrolleeId, string CODES, string EPA_ID, string DESC, string TYPE,
                                                   DateTime? EFFDT, DateTime? TRMDT, string CODETYPE, string VENDTYPE, string PADENIED, string SEX,
                                                   string MAGEMETH, string FAGEMETH, int? MAGELO, int? MAGEHI, int? FAGELO, int? FAGEHI,
                                                   string APPLYACC, string BAPPACC, string DSGID, string DSGID2, string CALCREFILL, int? REFILLDAYS,
                                                   string REFILLMETH, int? REFILLPCT, int? MAXREFILLS, int? MAXREFMNT, string PENALTY,
                                                   string DESI, string PHYLIMIT, string GI_GPI, string PPGID, string PPNID, string PPNREQRUL,
                                                   string INCCOMP, string BRANDDISC, string GENONLY, string DRUGCLASS, string DRUGTYPE,
                                                   string DRUGSTAT, string MAINTIND, int? COMPMAX, int? HIDOLLAR, double? QTYPERDYS,
                                                   int? QTYDYLMT, string COPAYGCI, string COPLVLASSN, string OVRRJTADI, string OVRRJTAGE,
                                                   string OVRRJTADD, string OVRRJTDDC, string OVRRJTDOT, string OVRRJTDUP, string OVRRJTIAT,
                                                   string OVRRJTMMA, string OVRRJTLAC, string OVRRJTPRG, string ACTIVE, string Note, string pharmacyID,
                                                   string vendorPANumber, string paidMsg, string REASON, string USERNAME, string CHANGEDBY, bool ignorePlan)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@USERNAME", USERNAME},
                { "@CHANGEDBY", CHANGEDBY},
                { "@DATE", DateTime.Now.Date},
                { "@TIME", DateTime.Now.ToString("HH:mm:ss")},
                { "@PLNID", ignorePlan ? null : PLNID?.ToUpper()},
                { "@ENRID", enrolleeId?.ToUpper()},
                { "@TYPE", TYPE?.ToUpper()},
                { "@EFFDT", EFFDT},
                { "@TRMDT", TRMDT},
                { "@CODETYPE", CODETYPE?.ToUpper()},
                { "@CODES", CODES?.ToUpper()},
                { "@VENDTYPE", ignorePlan ? null : VENDTYPE?.ToUpper()},
                { "@DSGID", DSGID?.ToUpper()},
                { "@SEX", SEX?.ToUpper()},
                { "@PENALTY", PENALTY?.ToUpper()},
                { "@APPLYACC", APPLYACC?.ToUpper()},
                { "@BRANDDISC", BRANDDISC?.ToUpper()},
                { "@GENONLY", GENONLY?.ToUpper()},
                { "@MAXREFILLS", MAXREFILLS},
                { "@MAINTIND", MAINTIND?.ToUpper()},
                { "@HIDOLLAR", HIDOLLAR},
                { "@PANUMBER", vendorPANumber?.ToUpper()},
                { "@QTYPERDYS", QTYPERDYS},
                { "@QTYDYLMT", QTYDYLMT},
                { "@REFILLDAYS", REFILLDAYS},
                { "@REFILLMETH", REFILLMETH?.ToUpper()},
                { "@REFILLPCT", REFILLPCT},
                { "@DESI", DESI?.ToUpper()},
                { "@DSGID2", DSGID2?.ToUpper()},
                { "@PHYLIMIT", PHYLIMIT?.ToUpper()},
                { "@GI_GPI", GI_GPI?.ToUpper()},
                { "@COPAYGCI", COPAYGCI?.ToUpper()},
                { "@MAGEMETH", MAGEMETH?.ToUpper()},
                { "@FAGEMETH", FAGEMETH?.ToUpper()},
                { "@MAGELO", MAGELO},
                { "@MAGEHI", MAGEHI},
                { "@FAGELO", FAGELO},
                { "@FAGEHI", FAGEHI},
                { "@TIER", null},
                { "@PPGID", PPGID?.ToUpper()},
                { "@COMMENT", Note?.ToUpper()},
                { "@INCCOMP", INCCOMP?.ToUpper()},
                { "@PPNID", PPNID?.ToUpper()},
                { "@PPNREQRUL", PPNREQRUL?.ToUpper()},
                { "@CALCREFILL", CALCREFILL?.ToUpper()},
                { "@MAXREFMNT", MAXREFMNT},
                { "@COMPMAX", COMPMAX},
                { "@OVRRJTADI", OVRRJTADI?.ToUpper()},
                { "@OVRRJTAGE", OVRRJTAGE?.ToUpper()},
                { "@OVRRJTADD", OVRRJTADD?.ToUpper()},
                { "@OVRRJTDDC", OVRRJTDDC?.ToUpper()},
                { "@OVRRJTDOT", OVRRJTDOT?.ToUpper()},
                { "@OVRRJTDUP", OVRRJTDUP?.ToUpper()},
                { "@OVRRJTIAT", OVRRJTIAT?.ToUpper()},
                { "@OVRRJTLAC", OVRRJTLAC?.ToUpper()},
                { "@OVRRJTMMA", OVRRJTMMA?.ToUpper()},
                { "@OVRRJTPRG", OVRRJTPRG?.ToUpper()},
                { "@PHAID", pharmacyID?.ToUpper()},
                { "@DRUGSTAT", DRUGSTAT?.ToUpper()},
                { "@COPLVLASSN", COPLVLASSN?.ToUpper()},
                { "@DRUGCLASS", DRUGCLASS?.ToUpper()},
                { "@DRUGTYPE", DRUGTYPE?.ToUpper()},
                { "@BAPPACC", BAPPACC?.ToUpper()},
                { "@PAIDMSG", paidMsg?.ToUpper()},
                { "@REASON", REASON?.ToUpper()}
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = "";

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Rule_insert", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("SYSID");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to add rule to {clientName} dataset.");
                }

                return dbResult;
            });

            string result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<string> UpdateMemberRule_DynamicPA(string clientName, string SYSID, string PLNID, string memberId, string enrolleeId, string CODES, string EPA_ID, string DESC, string TYPE,
                                           DateTime? EFFDT, DateTime? TRMDT, string CODETYPE, string VENDTYPE, string PADENIED, string SEX,
                                           string MAGEMETH, string FAGEMETH, int? MAGELO, int? MAGEHI, int? FAGELO, int? FAGEHI,
                                           string APPLYACC, string BAPPACC, string DSGID, string DSGID2, string CALCREFILL, int? REFILLDAYS,
                                           string REFILLMETH, int? REFILLPCT, int? MAXREFILLS, int? MAXREFMNT, string PENALTY,
                                           string DESI, string PHYLIMIT, string GI_GPI, string PPGID, string PPNID, string PPNREQRUL,
                                           string INCCOMP, string BRANDDISC, string GENONLY, string DRUGCLASS, string DRUGTYPE,
                                           string DRUGSTAT, string MAINTIND, int? COMPMAX, int? HIDOLLAR, double? QTYPERDYS,
                                           int? QTYDYLMT, string COPAYGCI, string COPLVLASSN, string OVRRJTADI, string OVRRJTAGE,
                                           string OVRRJTADD, string OVRRJTDDC, string OVRRJTDOT, string OVRRJTDUP, string OVRRJTIAT,
                                           string OVRRJTMMA, string OVRRJTLAC, string OVRRJTPRG, string ACTIVE, string Note, string pharmacyID,
                                           string vendorPANumber, string paidMsg, string REASON, string USERNAME, string CHANGEDBY, bool ignorePlan)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                
                { "@SYSID", SYSID},
                { "@USERNAME", USERNAME},
                { "@CHANGEDBY", CHANGEDBY},
                { "@DATE", DateTime.Now.Date},
                { "@TIME", DateTime.Now.ToString("HH:mm:ss")},
                { "@PLNID", ignorePlan ? null : PLNID?.ToUpper()},
                { "@ENRID", enrolleeId?.ToUpper()},
                { "@TYPE", TYPE?.ToUpper()},
                { "@EFFDT", EFFDT},
                { "@TRMDT", TRMDT},
                { "@CODETYPE", CODETYPE?.ToUpper()},
                { "@CODES", CODES?.ToUpper()},
                { "@VENDTYPE", ignorePlan ? null : VENDTYPE?.ToUpper()},
                { "@DSGID", DSGID?.ToUpper()},
                { "@SEX", SEX?.ToUpper()},
                { "@PENALTY", PENALTY?.ToUpper()},
                { "@APPLYACC", APPLYACC?.ToUpper()},
                { "@BRANDDISC", BRANDDISC?.ToUpper()},
                { "@GENONLY", GENONLY?.ToUpper()},
                { "@MAXREFILLS", MAXREFILLS},
                { "@MAINTIND", MAINTIND?.ToUpper()},
                { "@HIDOLLAR", HIDOLLAR},
                { "@PANUMBER", vendorPANumber?.ToUpper()},
                { "@QTYPERDYS", QTYPERDYS},
                { "@QTYDYLMT", QTYDYLMT},
                { "@REFILLDAYS", REFILLDAYS},
                { "@REFILLMETH", REFILLMETH?.ToUpper()},
                { "@REFILLPCT", REFILLPCT},
                { "@DESI", DESI?.ToUpper()},
                { "@DSGID2", DSGID2?.ToUpper()},
                { "@PHYLIMIT", PHYLIMIT?.ToUpper()},
                { "@GI_GPI", GI_GPI?.ToUpper()},
                { "@COPAYGCI", COPAYGCI?.ToUpper()},
                { "@MAGEMETH", MAGEMETH?.ToUpper()},
                { "@FAGEMETH", FAGEMETH?.ToUpper()},
                { "@MAGELO", MAGELO},
                { "@MAGEHI", MAGEHI},
                { "@FAGELO", FAGELO},
                { "@FAGEHI", FAGEHI},
                { "@TIER", null},
                { "@PPGID", PPGID?.ToUpper()},
                { "@COMMENT", Note?.ToUpper()},
                { "@INCCOMP", INCCOMP?.ToUpper()},
                { "@PPNID", PPNID?.ToUpper()},
                { "@PPNREQRUL", PPNREQRUL?.ToUpper()},
                { "@CALCREFILL", CALCREFILL?.ToUpper()},
                { "@MAXREFMNT", MAXREFMNT},
                { "@COMPMAX", COMPMAX},
                { "@OVRRJTADI", OVRRJTADI?.ToUpper()},
                { "@OVRRJTAGE", OVRRJTAGE?.ToUpper()},
                { "@OVRRJTADD", OVRRJTADD?.ToUpper()},
                { "@OVRRJTDDC", OVRRJTDDC?.ToUpper()},
                { "@OVRRJTDOT", OVRRJTDOT?.ToUpper()},
                { "@OVRRJTDUP", OVRRJTDUP?.ToUpper()},
                { "@OVRRJTIAT", OVRRJTIAT?.ToUpper()},
                { "@OVRRJTLAC", OVRRJTLAC?.ToUpper()},
                { "@OVRRJTMMA", OVRRJTMMA?.ToUpper()},
                { "@OVRRJTPRG", OVRRJTPRG?.ToUpper()},
                { "@PHAID", pharmacyID?.ToUpper()},
                { "@DRUGSTAT", DRUGSTAT?.ToUpper()},
                { "@COPLVLASSN", COPLVLASSN?.ToUpper()},
                { "@DRUGCLASS", DRUGCLASS?.ToUpper()},
                { "@DRUGTYPE", DRUGTYPE?.ToUpper()},
                { "@BAPPACC", BAPPACC?.ToUpper()},
                { "@PAIDMSG", paidMsg?.ToUpper()},
                { "@REASON", REASON?.ToUpper()}
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = "";

                try
                {
                    DataHelper.ExecuteReaderWithTransaction("apiPBM_Rule_update", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("SYSID");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to update rule in {clientName} dataset.");
                }

                return dbResult;
            });

            string result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Check whether a rule already exists in the database.
        /// </summary>
        /// <param name="planId">String representing the identifier of the plan</param>
        /// <param name="enrolleeId">String representing the ProCare identifier of the enrollee</param>
        /// <param name="vendType">String representing the vend type of the plan</param>
        /// <param name="codeType">String representing the code type of the product code (NDC, GCN, or GNN)</param>
        /// <param name="codes">String representing the submitted product code after being translated to the code type on the rule</param>
        /// <returns><see cref="string" /> representing the system identifier of the rule if found</returns>
        public async Task<List<MemberRuleDTO>> GetRules(string planId,
                                                  string enrolleeId,
                                                  string vendType,
                                                  string codeType,
                                                  string codes,
                                                  string sysId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@PLNID", planId?.ToUpper() },
                { "@ENRID", enrolleeId?.ToUpper() },
                { "@SYSID", sysId },
                { "@VENDTYPE", vendType?.ToUpper() },
                { "@CODETYPE", codeType?.ToUpper() },
                { "@CODES", codes?.ToUpper() }
            };

            Task<List<MemberRuleDTO>> t = Task.Run(() =>
            {
                var dbResults = new List<MemberRuleDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Rule_GetByEnrolleeIdAndNdcOrSysId", CommandType.StoredProcedure, parameters, reader =>
                    {
                        var dto = new MemberRuleDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    throw new TaskCanceledException("Failed to retrieve member rules.");
                }

                return dbResults;
            });

            var result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get the default template for the DynamicPACode2 and ProductID
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="templateId">String representing the identifier of the rule template to be used</param>
        /// <param name="productID">String representing the submitted product code</param>
        /// <returns><see cref="RuleTemplateDTO" /> representing the templated rule that was generated</returns>
        public async Task<RuleTemplateDTO> GetTemplateById(string clientName,
                                                                     string templateId,
                                                                     string productId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@EPA_ID", templateId?.ToUpper() },
                { "@NDC", productId?.ToUpper() }
            };

            Task<RuleTemplateDTO> t = Task.Run(() =>
            {
                RuleTemplateDTO dbResult = new RuleTemplateDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Rule_Template_byEPA_IDNDC", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult.LoadFromDataReader(reader);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to lookup template in {clientName} dataset.");
                }

                return dbResult;
            });

            RuleTemplateDTO result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<string> InsertMemberRule_MemberRule(string clientName, MemberRuleDTO dto)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@USERNAME", dto.USERNAME},
                { "@CHANGEDBY", dto.CHANGEDBY},
                { "@DATE", DateTime.Now.Date},
                { "@TIME", DateTime.Now.ToString("HH:mm:ss")},
                { "@PLNID", dto.PLNID},
                { "@ENRID", dto.ENRID},
                { "@TYPE", dto.TYPE},
                { "@EFFDT", dto.EFFDT},
                { "@TRMDT", dto.TRMDT},
                { "@CODETYPE", dto.CODETYPE},
                { "@CODES", dto.CODES},
                { "@VENDTYPE", dto.VENDTYPE},
                { "@DSGID", dto.DSGID},
                { "@SEX", dto.SEX},
                { "@PENALTY", dto.PENALTY},
                { "@APPLYACC", dto.APPLYACC},
                { "@BRANDDISC", dto.BRANDDISC},
                { "@GENONLY", dto.GENONLY},
                { "@MAXREFILLS", dto.MAXREFILLS},
                { "@MAINTIND", dto.MAINTIND},
                { "@HIDOLLAR", dto.HIDOLLAR},
                { "@PANUMBER", dto.VENDORPANUMBER},
                { "@QTYPERDYS", dto.QTYPERDYS},
                { "@QTYDYLMT", dto.QTYDYLMT},
                { "@REFILLDAYS", dto.REFILLDAYS},
                { "@REFILLMETH", dto.REFILLMETH},
                { "@REFILLPCT", dto.REFILLPCT},
                { "@DESI", dto.DESI},
                { "@DSGID2", dto.DSGID2},
                { "@PHYLIMIT", dto.PHYLIMIT},
                { "@GI_GPI", dto.GI_GPI},
                { "@COPAYGCI", dto.MULTISOURCECODE},
                { "@MAGEMETH", dto.MAGEMETH},
                { "@FAGEMETH", dto.FAGEMETH},
                { "@MAGELO", dto.MAGELO},
                { "@MAGEHI", dto.MAGEHI},
                { "@FAGELO", dto.FAGELO},
                { "@FAGEHI", dto.FAGEHI},
                { "@TIER", null},
                { "@PPGID", dto.PPGID},
                { "@COMMENT", dto.NOTE},
                { "@INCCOMP", dto.INCCOMP},
                { "@PPNID",  dto.PHARMACYID},
                { "@PPNREQRUL", dto.INCLUDEEXCLUDE},
                { "@CALCREFILL", dto.CALCREFILL},
                { "@MAXREFMNT", dto.MAXREFMNT},
                { "@COMPMAX", dto.COMPMAX},
                { "@OVRRJTADI", dto.OVRRJTADI},
                { "@OVRRJTAGE", dto.OVRRJTAGE},
                { "@OVRRJTADD", dto.OVRRJTADD},
                { "@OVRRJTDDC", dto.OVRRJTDDC},
                { "@OVRRJTDOT", dto.OVRRJTDOT},
                { "@OVRRJTDUP", dto.OVRRJTDUP},
                { "@OVRRJTIAT", dto.OVRRJTIAT},
                { "@OVRRJTLAC", dto.OVRRJTLAC},
                { "@OVRRJTMMA", dto.OVRRJTMMA},
                { "@OVRRJTPRG", dto.OVRRJTPRG},
                { "@PHAID", dto.PHARMACYID},
                { "@DRUGSTAT", dto.DRUGSTAT},
                { "@COPLVLASSN", dto.COPLVLASSN},
                { "@DRUGCLASS", dto.DRUGCLASS},
                { "@DRUGTYPE", dto.DRUGTYPE},
                { "@BAPPACC", dto.BAPPACC},
                { "@PAIDMSG", dto.PAIDMSG},
                { "@REASON", dto.REASON}
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = "";

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Rule_insert", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("SYSID");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to add rule to {clientName} dataset.");
                }

                return dbResult;
            });

            string result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<string> UpdateMemberRule_MemberRule(string clientName, MemberRuleDTO dto)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@SYSID", dto.SYSID },
                { "@USERNAME", dto.USERNAME},
                { "@CHANGEDBY", dto.CHANGEDBY},
                { "@DATE", DateTime.Now.Date},
                { "@TIME", DateTime.Now.ToString("HH:mm:ss")},
                { "@ENRID", dto.ENRID?.ToUpper()},
                { "@EFFDT", dto.EFFDT},
                { "@TRMDT", dto.TRMDT}
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = "";

                try
                {
                    DataHelper.ExecuteReaderWithTransaction("apiPBM_Rule_updateDates", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("SYSID");
                    });
                }
                catch (Exception ex)
                {
                    throw new TaskCanceledException($"Failed to update rule in {clientName} dataset.");
                }

                return dbResult;
            });

            string result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Looks up a DSGID by its DaysAmt and MaintDays values.
        /// </summary>
        /// <param name="daysSupplyMaximum">Integer representing the DaysAmt and MaintDays to use to lookup the DSGID</param>
        /// <returns><see cref="string" /> representing the DSGID that was looked up</returns>
        public async Task<string> LookupDSGID(int daysSupplyMaximum)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@DaysSupplyMaximum", daysSupplyMaximum}
            };

            Task<string> t = Task.Run(() =>
            {
                string output = "";

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Rule_getDSGID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        output = reader.GetStringorDefault("DSGID");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to lookup DSGID for submitted DaysSupplyMaximum.");
                }

                return output;
                ;
            });

            string dsgid = await t.ConfigureAwait(false);

            return dsgid;
        }

        #endregion
    }
}
