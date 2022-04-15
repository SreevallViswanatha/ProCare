using ProCare.API.PBM.Repository.DTO.Eligibility;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.PBM.Repository.DataAccess.Eligibility
{
    public class EligibilityDataAccess : DataAccessBase
    {
        #region Constructors

        public EligibilityDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods

        /// <summary>
        ///  Read data for a Client and its ClientConfiguration from the database.
        /// </summary>
        /// <param name="clientID">string representing note entity identifier</param>
        /// <returns><see cref="ClientWithConfigurationsDTO" /> representing the client and its configuration details</returns>
        public ClientWithConfigurationsDTO ReadClientWithClientConfiguration(long clientID)
        {
            ClientWithConfigurationsDTO dto = new ClientWithConfigurationsDTO();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  clientID}
            };

            DataHelper.ExecuteReader("apiPBM_Client_selectClientWithClientConfigurationByClientID", CommandType.StoredProcedure, parameters, reader =>
            {
                
                dto.LoadFromDataReader(reader);
            });

            return dto;
        }

        /// <summary>
        ///  Read data for a Client and its ClientConfiguration from the database.
        /// </summary>
        /// <param name="clientID">string representing note entity identifier</param>
        /// <returns><see cref="ClientACPDTO" /> representing the client and its configuration details</returns>
        public ClientACPDTO ReadClientACPConfiguration(long clientID)
        {
            ClientACPDTO dto = new ClientACPDTO();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@Client_ACPID",  clientID}
            };

            DataHelper.ExecuteReader("apiPBM_Client_ACP_Select_ByClient_ACPID", CommandType.StoredProcedure, parameters, reader =>
            {
                dto.LoadFromDataReader(reader);
            });

            return dto;
        }

        /// <summary>
        /// Inserts or updates an ACP import record.
        /// </summary>
        /// <param name="importID">Long representing the identifier of the import</param>
        /// <param name="transactionTypeID">Long representing the identifier of the transaction type</param>
        /// <param name="clientID">Long representing the identifier of the client</param>
        /// <param name="importStatusID">Long representing the identifier of the import status</param>
        /// <param name="rawData">String representing the full eligibility import request in XML format</param>
        /// <param name="createdTime">DateTime representing the date and time the import record was created</param>
        /// <param name="completedTime">DateTime representing the date and time the import was completed</param>
        /// <param name="returnValue">String representing the return value of the import</param>
        /// <param name="warningMessage">String representing warnings generated while processing the import</param>
        /// <param name="errorMessage">String representing errors generated while processing the import</param>
        /// <param name="recIdentifier">String representing the external record identifier of the import</param>
        /// <param name="recordAction">String representing the action being performed on the import record</param>
        /// <param name="newBalance">String representing the updated balance after processing the import record</param>
        /// <param name="insertAppUserID">Long representing the identifier of the inserting app user</param>
        /// <param name="updateAppUserID">Long representing the identifier of the updating app user</param>
        /// <returns><see cref="long" /> representing the identifier of the ACP import that was saved.</returns>
        public long SaveACPImport(long importACPID, long? transactionTypeID, long clientID, long importStatusID, string rawData, DateTime createdTime,
                                  DateTime completedTime, string returnValue, string warningMessage, string errorMessage, string recIdentifier,
                                  string recordAction, string newBalance, long insertAppUserID, long updateAppUserID)
        {
            long result = 0;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@Import_ACPID",  importACPID},
                {"@TransactionTypeID",  transactionTypeID},
                {"@ClientID",  clientID},
                {"@ImportStatusID",  importStatusID},
                {"@RawData",  rawData},
                {"@CreatedTime", createdTime },
                {"@CompletedTime", completedTime },
                {"@ReturnValue",  returnValue},
                {"@WarningMessage",  warningMessage},
                {"@ErrorMessage",  errorMessage},
                {"@RecIdentifier",  recIdentifier},
                {"@RecordAction",  recordAction},
                {"@NewBalance",  newBalance},
                {"@InsertAppUserID",  insertAppUserID},
                {"@UpdateAppUserID",  updateAppUserID}
            };

            DataHelper.ExecuteReader("apiPBM_Import_ACP_save", CommandType.StoredProcedure, parameters, reader =>
            {
                result = reader.GetInt64(0);
            });

            return result;
        }

        /// <summary>
        /// Gets plan fields needed to perform an ACP update.
        /// </summary>
        /// <param name="planID">String representing the identifier of the plan</param>
        /// <returns><see cref="ACPPlanFieldsDTO" /> representing plan fields needed to perform the ACP update</returns>
        public ACPPlanFieldsDTO GetACPPlanFields(string planID)
        {
            ACPPlanFieldsDTO dto = new ACPPlanFieldsDTO();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID",  planID.ToUpper()}
            };

            DataHelper.ExecuteReader("apiPBM_APSPLN_readACPPlanFieldsByPlnID", CommandType.StoredProcedure, parameters, reader =>
            {
                dto.LoadFromDataReader(reader);
            });

            return dto;
        }

        /// <summary>
        /// Performs a full replacement on a medical BYA record
        /// </summary>
        /// <param name="adjustmentDate">DateTime representing the date the adjustment is being made</param>
        /// <param name="effectiveDate">DateTime representing the date the adjustment is effective</param>
        /// <param name="termDate">DateTime representing the date the adjustment is termed</param>
        /// <param name="planId">String representing the identifier of the plan to perform the adjustment on</param>
        /// <param name="enrId">String representing the identifier of the enrollee to perform the adjustment on</param>
        /// <param name="medicalDeductible">Double representing the date the deductible of the BYA record</param>
        /// <param name="medicalOutOfPocketAmount">Double representing the date the out of pocket amount of the BYA record</param>
        /// <param name="medicalMaximumBenefitAmount">Double representing the date the maximum benefit amount of the BYA record</param>
        /// <param name="userName">String representing the username of the user making the adjustment</param>
        public void ReplaceAccumulator(DateTime AdjDt, DateTime ACCUM_EFF, DateTime ACCUM_TRM, string PLNID, string ENRID,
                                       double MEDDEDACC, double MEDOOPACC, double MEDMAXACC, string USERNAME)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AdjDt", AdjDt },
                {"@ACCUM_EFF", ACCUM_EFF },
                {"@ACCUM_TRM", ACCUM_TRM },
                {"@PLNID", PLNID?.ToUpper() },
                {"@ENRID", ENRID?.ToUpper() },
                {"@MEDDEDACC", MEDDEDACC },
                {"@MEDOOPACC", MEDOOPACC },
                {"@MEDMAXACC", MEDMAXACC },
                {"@USERNAME", USERNAME }
            };

            DataHelper.ExecuteNonQuery("Neat_FullReplace_Medical", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        /// Performs an update on a medical BYA record
        /// </summary>
        /// <param name="adjustmentDate">DateTime representing the date the adjustment is being made</param>
        /// <param name="effectiveDate">DateTime representing the date the adjustment is effective</param>
        /// <param name="termDate">DateTime representing the date the adjustment is termed</param>
        /// <param name="planId">String representing the identifier of the plan to perform the adjustment on</param>
        /// <param name="enrId">String representing the identifier of the enrollee to perform the adjustment on</param>
        /// <param name="medicalDeductible">Double representing the date the deductible of the BYA record</param>
        /// <param name="medicalOutOfPocketAmount">Double representing the date the out of pocket amount of the BYA record</param>
        /// <param name="medicalMaximumBenefitAmount">Double representing the date the maximum benefit amount of the BYA record</param>
        /// <param name="userName">String representing the username of the user making the adjustment</param>
        public void UpdateAccumulator(DateTime AdjDt, DateTime ACCUM_EFF, DateTime ACCUM_TRM, string PLNID, string ENRID,
                                       double MEDDEDACC, double MEDOOPACC, double MEDMAXACC, string USERNAME)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AdjDt", AdjDt },
                {"@ACCUM_EFF", ACCUM_EFF },
                {"@ACCUM_TRM", ACCUM_TRM },
                {"@PLNID", PLNID?.ToUpper() },
                {"@ENRID", ENRID?.ToUpper() },
                {"@MEDDEDACC", MEDDEDACC },
                {"@MEDOOPACC", MEDOOPACC },
                {"@MEDMAXACC", MEDMAXACC },
                {"@USERNAME", USERNAME }
            };

            DataHelper.ExecuteNonQuery("Neat_Update_Medical", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        /// Gets new balance after performing an ACP update.
        /// </summary>
        /// <param name="adjustmentDate">DateTime representing the date of the adjustment</param>
        /// <param name="planId">String representing the identifier of the plan the adjustment was made on</param>
        /// <param name="enrId">String representing the identifier of the enrollee the adjustment was made on</param>
        /// <returns><see cref="string" /> representing the new balance after performing the ACP update</returns>
        public string ReadNewBalance(DateTime adjustmentDate, string planID, string enrID)
        {
            string result = string.Empty;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ADJ_DATE",  adjustmentDate},
                {"@PLNID",  planID},
                {"@ENRID",  enrID}
            };

            DataHelper.ExecuteReader("Neat_GetNewBalance", CommandType.StoredProcedure, parameters, reader =>
            {
                result = reader.GetDouble(0).ToString("F2");
            });

            return result;
        }

        /// <summary>
        /// Read a list of members with matching Plan ID, CardID, CardID2, and Person from the database.
        /// </summary>
        /// <param name="planID">String representing the history member plan identifier</param>
        /// <param name="cardID">String representing first 9 digits of the history member card identifier</param>
        /// <param name="cardID2">String representing additional digits of the history member card identifier</param>
        /// <param name="person">String representing history member person identifier</param>
        /// <returns><see cref="List{MemberDTO}" /> representing the history members with matching PlanID, CardID, CardID2, and Person</returns>
        public List<MemberDTO> ReadMemberByPlanIDCardIDCardID2Person(string planID, string cardID, string cardID2, string person)
        {
            List<MemberDTO> dto = new List<MemberDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID",  planID.ToUpper()},
                {"@CARDID",  cardID.ToUpper()},
                {"@CARDID2",  cardID2.ToUpper()},
                {"@PERSON",  person}
            };

            DataHelper.ExecuteReader("apiPBM_APSENR_readByPlnIDCardIDCardID2Person", CommandType.StoredProcedure, parameters, reader =>
            {
                MemberDTO member = new MemberDTO();
                member.LoadFromDataReader(reader);
                dto.Add(member);
            });

            return dto;
        }

        /// <summary>
        /// Read a list of benefit year accumulators with matching Plan ID and Enrollee ID from the database.
        /// </summary>
        /// <param name="planID">String representing the history member plan identifier</param>
        /// <param name="enrolleeID">String representing the history member plan identifier</param>
        /// <returns><see cref="List{BenefitYearAccumulatorDTO}" /> representing the benefit year accumulators with matching PlanID and EnrolleeID</returns>
        public List<BenefitYearAccumulatorDTO> ReadBenefitYearAccumulatorByPlanIDEnrolleeID(string planID, string enrolleeID)
        {
            List<BenefitYearAccumulatorDTO> dto = new List<BenefitYearAccumulatorDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID",  planID.ToUpper()},
                {"@ENRID",  enrolleeID.ToUpper()}
            };

            DataHelper.ExecuteReader("apiPBM_APSBYA_readByPlnIDEnrID", CommandType.StoredProcedure, parameters, reader =>
            {
                BenefitYearAccumulatorDTO benefit = new BenefitYearAccumulatorDTO();
                benefit.LoadFromDataReader(reader);
                dto.Add(benefit);
            });

            return dto;
        }

        /// <summary>
        /// Read a list of suspension records with a matching link key for the given table from the database.
        /// </summary>
        /// <param name="tableName">String representing the name of the table the suspended record is from</param>
        /// <param name="linkKey">String representing the key value used to look up data in the table</param>
        /// <returns><see cref="List{SuspensionDTO}" /> representing the suspension records for the given table name and link key</returns>
        public List<SuspensionDTO> ReadSuspensionByTableNameLinkKey(string tableName, string linkKey)
        {
            List<SuspensionDTO> dto = new List<SuspensionDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@TABLENAME",  tableName.ToUpper()},
                {"@LINKKEY",  linkKey.ToUpper()}
            };

            DataHelper.ExecuteReader("apiPBM_APSSUS_readByTableNameLinkKey", CommandType.StoredProcedure, parameters, reader =>
            {
                SuspensionDTO suspension = new SuspensionDTO();
                suspension.LoadFromDataReader(reader);
                dto.Add(suspension);
            });

            return dto;
        }

        /// <summary>
        /// Read a list of history members with matching Plan ID, CardID, CardID2, and Person from the database.
        /// </summary>
        /// <param name="planID">String representing the history member plan identifier</param>
        /// <param name="cardID">String representing first 9 digits of the history member card identifier</param>
        /// <param name="cardID2">String representing additional digits of the history member card identifier</param>
        /// <param name="person">String representing history member person identifier</param>
        /// <returns><see cref="List{MemberDTO}" /> representing the history members with matching PlanID, CardID, CardID2, and Person</returns>
        public List<MemberDTO> ReadHistoryMemberByPlanIDCardIDCardID2Person(string planID, string cardID, string cardID2, string person)
        {
            List<MemberDTO> dto = new List<MemberDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID",  planID},
                {"@CARDID",  cardID},
                {"@CARDID2",  cardID2},
                {"@PERSON",  person}
            };

            DataHelper.ExecuteReader("apiPBM_APSENH_readByPlnIDCardIDCardID2Person", CommandType.StoredProcedure, parameters, reader =>
            {
                MemberDTO member = new MemberDTO();
                member.LoadFromDataReader(reader);
                dto.Add(member);
            });

            return dto;
        }

        /// <summary>
        /// Logs a member record to the database.  To be used prior to updating the member record.
        /// </summary>
        /// <param name="ENRID">String representing the ENRID of the member</param>
        /// <param name="PLNID">String representing the PLNID of the member</param>
        /// <param name="SUBID">String representing the SUBID of the member</param>
        /// <param name="CARDID">String representing the CARDID of the member</param>
        /// <param name="PERSON">String representing the PERSON of the member</param>
        /// <param name="EFFDT">DateTime representing the EFFDT of the member</param>
        /// <param name="TRMDT">DateTime representing the TRMDT of the member</param>
        /// <param name="FLEX1">String representing the FLEX1 of the member</param>
        /// <param name="FLEX2">String representing the FLEX2 of the member</param>
        /// <param name="RELCD">String representing the RELCD of the member</param>
        /// <param name="ELGOVER">String representing the ELGOVER of the member</param>
        /// <param name="FNAME">String representing the FNAME of the member</param>
        /// <param name="MNAME">String representing the MNAME of the member</param>
        /// <param name="LNAME">String representing the LNAME of the member</param>
        /// <param name="ADDR">String representing the ADDR of the member</param>
        /// <param name="ADDR2">String representing the ADDR2 of the member</param>
        /// <param name="CITY">String representing the CITY of the member</param>
        /// <param name="STATE">String representing the STATE of the member</param>
        /// <param name="ZIP">String representing the ZIP of the member</param>
        /// <param name="ZIP4">String representing the ZIP4 of the member</param>
        /// <param name="DOB">DateTime representing the DOB of the member</param>
        /// <param name="SEX">String representing the SEX of the member</param>
        /// <param name="ELGCD">String representing the ELGCD of the member</param>
        /// <param name="EMPCD">String representing the EMPCD of the member</param>
        /// <param name="CRDDT">String representing the CRDDT of the member</param>
        /// <param name="SYSID">String representing the SYSID of the member</param>
        /// <param name="LSTDTCARD">DateTime representing the LSTDTCARD of the member</param>
        /// <param name="NOUPDATE">String representing the NOUPDATE of the member</param>
        /// <param name="NDCUPDATE">String representing the NDCUPDATE of the member</param>
        /// <param name="LASTUPDT">DateTime representing the LASTUPDT of the member</param>
        /// <param name="MBRSINCE">DateTime representing the MBRSINCE of the member</param>
        /// <param name="PHYID">String representing the PHYID of the member</param>
        /// <param name="OLDPERSON">String representing the OLDPERSON of the member</param>
        /// <param name="FLEX3">String representing the FLEX3 of the member</param>
        /// <param name="OTHERID">String representing the OTHERID of the member</param>
        /// <param name="DEPCODE">String representing the DEPCODE of the member</param>
        /// <param name="MAINT">String representing the MAINT of the member</param>
        /// <param name="ACCUM">Double representing the ACCUM of the member</param>
        /// <param name="PATSTAT">String representing the PATSTAT of the member</param>
        /// <param name="ENRCOPAYM">String representing the ENRCOPAYM of the member</param>
        /// <param name="ENRCOPAYR">String representing the ENRCOPAYR of the member</param>
        /// <param name="PHYSREQ">String representing the PHYSREQ of the member</param>
        /// <param name="USEELM">String representing the USEELM of the member</param>
        /// <param name="ACCMETH">String representing the ACCMETH of the member</param>
        /// <param name="CARDID2">String representing the CARDID2 of the member</param>
        /// <param name="COB">String representing the COB of the member</param>
        /// <param name="JOURNAL">String representing the JOURNAL of the member</param>
        /// <param name="ADDEDBY">String representing the ADDEDBY of the member</param>
        /// <param name="PMGID">String representing the PMGID of the member</param>
        /// <param name="PHONE">String representing the PHONE of the member</param>
        /// <param name="MEDICARE">String representing the MEDICARE of the member</param>
        /// <param name="PPNREQENR">String representing the PPNREQENR of the member</param>
        /// <param name="PPNID">String representing the PPNID of the member</param>
        /// <param name="HICN">String representing the HICN of the member</param>
        /// <param name="RXBIN">String representing the RXBIN of the member</param>
        /// <param name="RXPCN">String representing the RXPCN of the member</param>
        /// <param name="RXGROUP">String representing the RXGROUP of the member</param>
        /// <param name="RXID">String representing the RXID of the member</param>
        /// <param name="TRELIG">String representing the TRELIG of the member</param>
        /// <param name="PHYQUAL">String representing the PHYQUAL of the member</param>
        /// <param name="MMEDAYMAX">Double representing the MMEDAYMAX of the member</param>
        /// <param name="ALLOWGOVT">Double representing the ALLOWGOVT of the member</param>
        /// <param name="USERNAME">Double representing the USERNAME of the account updating the member</param>
        /// <returns><see cref="string" /> representing the system identifier of the log record that was written</returns>
        public string LogMember(string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                               string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                               string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                               string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                               string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                               string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                               string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                               string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                               int? MMEDAYMAX, string ALLOWGOVT, string USERNAME)
        {
            string logSysid = "";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID", ENRID?.ToUpper() },
                {"@PLNID", PLNID?.ToUpper() },
                {"@SUBID", SUBID?.ToUpper() },
                {"@CARDID", CARDID?.ToUpper() },
                {"@PERSON", PERSON },
                {"@EFFDT", EFFDT },
                {"@TRMDT", TRMDT.GetValueOrDefault() },
                {"@FLEX1", FLEX1?.ToUpper() },
                {"@FLEX2", FLEX2?.ToUpper() },
                {"@RELCD", RELCD?.ToUpper() },
                {"@ELGOVER", ELGOVER?.ToUpper() },
                {"@FNAME", FNAME?.ToUpper() },
                {"@MNAME", MNAME?.ToUpper() },
                {"@LNAME", LNAME?.ToUpper() },
                {"@ADDR", ADDR?.ToUpper() },
                {"@ADDR2", ADDR2?.ToUpper() },
                {"@CITY", CITY?.ToUpper() },
                {"@STATE", STATE?.ToUpper() },
                {"@ZIP", ZIP?.ToUpper() },
                {"@ZIP4", ZIP4?.ToUpper() },
                {"@DOB", DOB },
                {"@SEX", SEX?.ToUpper() },
                {"@ELGCD", ELGCD?.ToUpper() },
                {"@EMPCD", EMPCD?.ToUpper() },
                {"@CRDDT", CRDDT },
                {"@SYSID", SYSID?.ToUpper() },
                {"@LSTDTCARD", LSTDTCARD },
                {"@NOUPDATE", NOUPDATE?.ToUpper() },
                {"@NDCUPDATE", NDCUPDATE?.ToUpper() },
                {"@LASTUPDT", LASTUPDT },
                {"@MBRSINCE", MBRSINCE },
                {"@PHYID", PHYID?.ToUpper() },
                {"@OLDPERSON", OLDPERSON?.ToUpper() },
                {"@FLEX3", FLEX3?.ToUpper() },
                {"@OTHERID", OTHERID?.ToUpper() },
                {"@DEPCODE", DEPCODE?.ToUpper() },
                {"@MAINT", MAINT?.ToUpper() },
                {"@ACCUM", ACCUM },
                {"@PATSTAT", PATSTAT?.ToUpper() },
                {"@ENRCOPAYM", ENRCOPAYM?.ToUpper() },
                {"@ENRCOPAYR", ENRCOPAYR?.ToUpper() },
                {"@PHYSREQ", PHYSREQ?.ToUpper() },
                {"@USEELM", USEELM?.ToUpper() },
                {"@ACCMETH", ACCMETH?.ToUpper() },
                {"@CARDID2", CARDID2?.ToUpper() },
                {"@COB", COB?.ToUpper() },
                {"@JOURNAL", JOURNAL?.ToUpper() },
                {"@ADDEDBY", ADDEDBY?.ToUpper() },
                {"@PMGID", PMGID?.ToUpper() },
                {"@PHONE", PHONE },
                {"@MEDICARE", MEDICARE?.ToUpper() },
                {"@PPNREQENR", PPNREQENR?.ToUpper() },
                {"@PPNID", PPNID?.ToUpper() },
                {"@HICN", HICN?.ToUpper() },
                {"@RXBIN", RXBIN?.ToUpper() },
                {"@RXPCN", RXPCN?.ToUpper() },
                {"@RXGROUP", RXGROUP?.ToUpper() },
                {"@RXID", RXID?.ToUpper() },
                {"@TRELIG", TRELIG?.ToUpper() },
                {"@PHYQUAL", PHYQUAL?.ToUpper() },
                {"@MMEDAYMAX", MMEDAYMAX },
                {"@ALLOWGOVT", ALLOWGOVT.ToUpper() },
                {"@CHANGEDBY", ".NET DAL" },
                {"@DATE", DateTime.Now },
                {"@TIME", DateTime.Now.ToString("HH:mm:ss") },
                {"@USERNAME", USERNAME }
            };

            DataHelper.ExecuteReaderWithTransaction("apiPBM_APSLOG_insert", CommandType.StoredProcedure, parameters, reader =>
            {
                logSysid = reader.GetStringorDefault("APSLOGSYSID");
            });

            return logSysid;
        }

        /// <summary>
        /// Logs a suspension record to the database.  To be used prior to updating the suspension record.
        /// </summary>
        /// <param name="TABLENAME">String representing the name of the table the record to be suspended is from</param>
        /// <param name="LINKKEY">String representing the key used to link the suspension record to the table</param>
        /// <param name="CUTOFFDATE">Nullable DateTime representing date of the suspension</param>
        /// <param name="CUTOFFMETH">Single-character string representing the method of suspension</param>
        /// <param name="JOURNAL">String representing the whether to journal the change</param>
        /// <param name="SYSID">String representing the system identifier of the suspension record being logged</param>
        /// <param name="USERNAME">String representing the name of the user logging the suspension record</param>
        /// <returns>None</returns>
        public void LogSuspension(string TABLENAME, string LINKKEY, DateTime? CUTOFFDATE, string CUTOFFMETH, string JOURNAL, string SYSID, string USERNAME)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@TABLENAME", TABLENAME?.ToUpper() },
                {"@LINKKEY", LINKKEY?.ToUpper() },
                {"@CUTOFFDATE", CUTOFFDATE },
                {"@CUTOFFMETH", CUTOFFMETH?.ToUpper() },
                {"@JOURNAL", JOURNAL?.ToUpper() },
                {"@SYSID", SYSID?.ToUpper() },
                {"@CHANGEDBY", ".NET DAL" },
                {"@DATE", DateTime.Now },
                {"@TIME", DateTime.Now.ToString("HH:mm:ss") },
                {"@USERNAME", USERNAME }
            };

            DataHelper.ExecuteNonQuery("apiPBM_APSSUSL_insert", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        /// Logs a benefit year accumulator record to the database.  To be used prior to updating the benefit year accumulator record.
        /// </summary>
        /// <param name="PLNID">String representing the PLNID of the benefit year accumulator being logged.</param>
        /// <param name="ENRID">String representing the ENRID of the benefit year accumulator being logged.</param>
        /// <param name="SUBID">String representing the SUBID of the benefit year accumulator being logged.</param>
        /// <param name="ENRAMT">String representing the ENRAMT of the benefit year accumulator being logged.</param>
        /// <param name="YTDRX">String representing the YTDRX of the benefit year accumulator being logged.</param>
        /// <param name="YTDDOLLAR">String representing the YTDDOLLAR of the benefit year accumulator being logged.</param>
        /// <param name="PLANTYPE">String representing the PLANTYPE of the benefit year accumulator being logged.</param>
        /// <param name="EFFDT">String representing the EFFDT of the benefit year accumulator being logged.</param>
        /// <param name="TRMDT">String representing the TRMDT of the benefit year accumulator being logged.</param>
        /// <param name="BROKERYTD">String representing the BROKERYTD of the benefit year accumulator being logged.</param>
        /// <param name="SMOKINGYTD">String representing the SMOKINGYTD of the benefit year accumulator being logged.</param>
        /// <param name="SMOKINGLT">String representing the SMOKINGLT of the benefit year accumulator being logged.</param>
        /// <param name="COPAY">String representing the COPAY of the benefit year accumulator being logged.</param>
        /// <param name="PRODSEL">String representing the PRODSEL of the benefit year accumulator being logged.</param>
        /// <param name="DEDUCT">String representing the DEDUCT of the benefit year accumulator being logged.</param>
        /// <param name="DEDMETDT">String representing the DEDMETDT of the benefit year accumulator being logged.</param>
        /// <param name="EXCEEDMAX">String representing the EXCEEDMAX of the benefit year accumulator being logged.</param>
        /// <param name="MAXMETDT">String representing the MAXMETDT of the benefit year accumulator being logged.</param>
        /// <param name="OOPMETDT">String representing the OOPMETDT of the benefit year accumulator being logged.</param>
        /// <param name="LIFEMAX">String representing the LIFEMAX of the benefit year accumulator being logged.</param>
        /// <param name="FERYTDMAX">String representing the FERYTDMAX of the benefit year accumulator being logged.</param>
        /// <param name="FERLTMAX">String representing the FERLTMAX of the benefit year accumulator being logged.</param>
        /// <param name="OCYTD">String representing the OCYTD of the benefit year accumulator being logged.</param>
        /// <param name="OCLIFE">String representing the OCLIFE of the benefit year accumulator being logged.</param>
        /// <param name="ICYTD">String representing the ICYTD of the benefit year accumulator being logged.</param>
        /// <param name="ICLIFE">String representing the ICLIFE of the benefit year accumulator being logged.</param>
        /// <param name="JOURNAL">String representing the JOURNAL of the benefit year accumulator being logged.</param>
        /// <param name="SYSID">String representing the SYSID of the benefit year accumulator being logged.</param>
        /// <param name="TIER">String representing the TIER of the benefit year accumulator being logged.</param>
        /// <param name="NPDEDACC">String representing the NPDEDACC of the benefit year accumulator being logged.</param>
        /// <param name="NPOOPACC">String representing the NPOOPACC of the benefit year accumulator being logged.</param>
        /// <param name="NPMAXACC">String representing the NPMAXACC of the benefit year accumulator being logged.</param>
        /// <param name="QTR4DEDACC">String representing the QTR4DEDACC of the benefit year accumulator being logged.</param>
        /// <param name="QTR4OOPACC">String representing the QTR4OOPACC of the benefit year accumulator being logged.</param>
        /// <param name="QTR4MAXACC">String representing the QTR4MAXACC of the benefit year accumulator being logged.</param>
        /// <param name="MEDDEDACC">String representing the MEDDEDACC of the benefit year accumulator being logged.</param>
        /// <param name="MEDOOPACC">String representing the MEDOOPACC of the benefit year accumulator being logged.</param>
        /// <param name="MEDMAXACC">String representing the MEDMAXACC of the benefit year accumulator being logged.</param>
        /// <param name="DIAPHRDT">String representing the DIAPHRDT of the benefit year accumulator being logged.</param>
        /// <param name="NPMEDMAX">String representing the NPMEDMAX of the benefit year accumulator being logged.</param>
        /// <param name="NPMEDOOP">String representing the NPMEDOOP of the benefit year accumulator being logged.</param>
        /// <param name="NPMEDDED">String representing the NPMEDDED of the benefit year accumulator being logged.</param>
        /// <param name="LASTCLAIM">String representing the LASTCLAIM of the benefit year accumulator being logged.</param>
        /// <param name="OTHERID">String representing the OTHERID of the benefit year accumulator being logged.</param>
        /// <param name="GHYTDMAX">String representing the GHYTDMAX of the benefit year accumulator being logged.</param>
        /// <param name="GHLTMAX">String representing the GHLTMAX of the benefit year accumulator being logged.</param>
        /// <param name="CPYSUBS">String representing the CPYSUBS of the benefit year accumulator being logged.</param>
        /// <param name="DEDSUBS">String representing the DEDSUBS of the benefit year accumulator being logged.</param>
        /// <param name="TROOP">String representing the TROOP of the benefit year accumulator being logged.</param>
        /// <param name="TOTPRC">String representing the TOTPRC of the benefit year accumulator being logged.</param>
        /// <param name="GAP_TROOP">String representing the GAP_TROOP of the benefit year accumulator being logged.</param>
        /// <param name="ENRADJ">String representing the ENRADJ of the benefit year accumulator being logged.</param>
        /// <param name="CLASS">String representing the CLASS of the benefit year accumulator being logged.</param>
        /// <param name="BYATROOP">String representing the BYATROOP of the benefit year accumulator being logged.</param>
        /// <param name="PARTBDED">String representing the PARTBDED of the benefit year accumulator being logged.</param>
        /// <param name="PARTBOOP">String representing the PARTBOOP of the benefit year accumulator being logged.</param>
        /// <param name="PARTBMAX">String representing the PARTBMAX of the benefit year accumulator being logged.</param>
        /// <param name="BDEDMETDT">String representing the BDEDMETDT of the benefit year accumulator being logged.</param>
        /// <param name="BOOPMETDT">String representing the BOOPMETDT of the benefit year accumulator being logged.</param>
        /// <param name="BMAXMETDT">String representing the BMAXMETDT of the benefit year accumulator being logged.</param>
        /// <param name="TROOPIC">String representing the TROOPIC of the benefit year accumulator being logged.</param>
        /// <param name="TOTPRCIC">String representing the TOTPRCIC of the benefit year accumulator being logged.</param>
        /// <param name="CPYSUBSIC">String representing the CPYSUBSIC of the benefit year accumulator being logged.</param>
        /// <param name="GAPCPYSUB">String representing the GAPCPYSUB of the benefit year accumulator being logged.</param>
        /// <param name="GAPTOTPRC">String representing the GAPTOTPRC of the benefit year accumulator being logged.</param>
        /// <param name="TEQFEE">String representing the TEQFEE of the benefit year accumulator being logged.</param>
        /// <param name="SPECDED">String representing the SPECDED of the benefit year accumulator being logged.</param>
        /// <param name="SPECOOP">String representing the SPECOOP of the benefit year accumulator being logged.</param>
        /// <param name="SPECDOLLAR">String representing the SPECDOLLAR of the benefit year accumulator being logged.</param>
        /// <param name="USERNAME">String representing the USERNAME of the benefit year accumulator being logged.</param>
        /// <returns><see cref="string" /> representing the system identifier of the log record that was written</returns>
        public string LogAccumulator(string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
                                  string PLANTYPE, DateTime EFFDT, DateTime TRMDT, double BROKERYTD, double SMOKINGYTD, double SMOKINGLT,
                                  double COPAY, double PRODSEL, double DEDUCT, DateTime DEDMETDT, double EXCEEDMAX, DateTime MAXMETDT,
                                  DateTime OOPMETDT, double LIFEMAX, double FERYTDMAX, double FERLTMAX, double OCYTD, double OCLIFE, double ICYTD,
                                  double ICLIFE, string JOURNAL, string SYSID, string TIER, double NPDEDACC, double NPOOPACC, double NPMAXACC,
                                  double QTR4DEDACC, double QTR4OOPACC, double QTR4MAXACC, double MEDDEDACC, double MEDOOPACC, double MEDMAXACC,
                                  DateTime DIAPHRDT, double NPMEDMAX, double NPMEDOOP, double NPMEDDED, DateTime LASTCLAIM, string OTHERID,
                                  double GHYTDMAX, double GHLTMAX, double CPYSUBS, double DEDSUBS, double TROOP, double TOTPRC, double GAP_TROOP,
                                  double ENRADJ, string CLASS, double BYATROOP, double PARTBDED, double PARTBOOP, double PARTBMAX, DateTime BDEDMETDT,
                                  DateTime BOOPMETDT, DateTime BMAXMETDT, double TROOPIC, double TOTPRCIC, double CPYSUBSIC, double GAPCPYSUB,
                                  double GAPTOTPRC, double TEQFEE, double SPECDED, double SPECOOP, double SPECDOLLAR, string USERNAME)
        {
            string logSysid = "";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID", PLNID?.ToUpper() },
                {"@ENRID", ENRID?.ToUpper() },
                {"@SUBID", SUBID?.ToUpper() },
                {"@ENRAMT", ENRAMT },
                {"@YTDRX", YTDRX },
                {"@YTDDOLLAR", YTDDOLLAR },
                {"@PLANTYPE", PLANTYPE?.ToUpper() },
                {"@EFFDT", EFFDT },
                {"@TRMDT", TRMDT },
                {"@BROKERYTD", BROKERYTD },
                {"@SMOKINGYTD", SMOKINGYTD },
                {"@SMOKINGLT", SMOKINGLT },
                {"@COPAY", COPAY },
                {"@PRODSEL", PRODSEL },
                {"@DEDUCT", DEDUCT },
                {"@DEDMETDT", DEDMETDT },
                {"@EXCEEDMAX", EXCEEDMAX },
                {"@MAXMETDT", MAXMETDT },
                {"@OOPMETDT", OOPMETDT },
                {"@LIFEMAX", LIFEMAX },
                {"@FERYTDMAX", FERYTDMAX },
                {"@FERLTMAX", FERLTMAX },
                {"@OCYTD", OCYTD },
                {"@OCLIFE", OCLIFE },
                {"@ICYTD", ICYTD },
                {"@ICLIFE", ICLIFE },
                {"@JOURNAL", JOURNAL?.ToUpper() },
                {"@SYSID", SYSID?.ToUpper() },
                {"@TIER", TIER?.ToUpper() },
                {"@NPDEDACC", NPDEDACC },
                {"@NPOOPACC", NPOOPACC },
                {"@NPMAXACC", NPMAXACC },
                {"@QTR4DEDACC", QTR4DEDACC },
                {"@QTR4OOPACC", QTR4OOPACC },
                {"@QTR4MAXACC", QTR4MAXACC },
                {"@MEDDEDACC", MEDDEDACC },
                {"@MEDOOPACC", MEDOOPACC },
                {"@MEDMAXACC", MEDMAXACC },
                {"@DIAPHRDT", DIAPHRDT },
                {"@NPMEDMAX", NPMEDMAX },
                {"@NPMEDOOP", NPMEDOOP },
                {"@NPMEDDED", NPMEDDED },
                {"@LASTCLAIM", LASTCLAIM },
                {"@OTHERID", OTHERID?.ToUpper() },
                {"@GHYTDMAX", GHYTDMAX },
                {"@GHLTMAX", GHLTMAX },
                {"@CPYSUBS", CPYSUBS },
                {"@DEDSUBS", DEDSUBS },
                {"@TROOP", TROOP },
                {"@TOTPRC", TOTPRC },
                {"@GAP_TROOP", GAP_TROOP },
                {"@ENRADJ", ENRADJ },
                {"@CLASS", CLASS?.ToUpper() },
                {"@BYATROOP", BYATROOP },
                {"@PARTBDED", PARTBDED },
                {"@PARTBOOP", PARTBOOP },
                {"@PARTBMAX", PARTBMAX },
                {"@BDEDMETDT", BDEDMETDT },
                {"@BOOPMETDT",BOOPMETDT  },
                {"@BMAXMETDT", BMAXMETDT },
                {"@TROOPIC", TROOPIC },
                {"@TOTPRCIC", TOTPRCIC },
                {"@CPYSUBSIC", CPYSUBSIC },
                {"@GAPCPYSUB", GAPCPYSUB },
                {"@GAPTOTPRC", GAPTOTPRC },
                {"@TEQFEE", TEQFEE },
                {"@SPECDED", SPECDED },
                {"@SPECOOP", SPECOOP },
                {"@SPECDOLLAR", SPECDOLLAR },
                {"@CHANGEDBY", ".NET DAL" },
                {"@DATE", DateTime.Now },
                {"@TIME", DateTime.Now.ToString("HH:mm:ss") },
                {"@USERNAME", USERNAME }
            };

            DataHelper.ExecuteReaderWithTransaction("apiPBM_BYALOG_insert", CommandType.StoredProcedure, parameters, reader =>
            {
                logSysid = reader.GetStringorDefault("BYALOGSYSID");
            });

            return logSysid;
        }

        /// <summary>
        /// Inserts a history benefit year accumulator record in the database.
        /// </summary>
        /// <param name="PLNID">String representing the PLNID of the history benefit year accumulator.</param>
        /// <param name="ENRID">String representing the ENRID of the history benefit year accumulator.</param>
        /// <param name="SUBID">String representing the SUBID of the history benefit year accumulator.</param>
        /// <param name="ENRAMT">String representing the ENRAMT of the history benefit year accumulator.</param>
        /// <param name="YTDRX">String representing the YTDRX of the history benefit year accumulator.</param>
        /// <param name="YTDDOLLAR">String representing the YTDDOLLAR of the history benefit year accumulator.</param>
        /// <param name="PLANTYPE">String representing the PLANTYPE of the history benefit year accumulator.</param>
        /// <param name="EFFDT">String representing the EFFDT of the history benefit year accumulator.</param>
        /// <param name="TRMDT">String representing the TRMDT of the history benefit year accumulator.</param>
        /// <param name="BROKERYTD">String representing the BROKERYTD of the history benefit year accumulator.</param>
        /// <param name="SMOKINGYTD">String representing the SMOKINGYTD of the history benefit year accumulator.</param>
        /// <param name="SMOKINGLT">String representing the SMOKINGLT of the history benefit year accumulator.</param>
        /// <param name="COPAY">String representing the COPAY of the history benefit year accumulator.</param>
        /// <param name="PRODSEL">String representing the PRODSEL of the history benefit year accumulator.</param>
        /// <param name="DEDUCT">String representing the DEDUCT of the history benefit year accumulator.</param>
        /// <param name="DEDMETDT">String representing the DEDMETDT of the history benefit year accumulator.</param>
        /// <param name="EXCEEDMAX">String representing the EXCEEDMAX of the history benefit year accumulator.</param>
        /// <param name="MAXMETDT">String representing the MAXMETDT of the history benefit year accumulator.</param>
        /// <param name="OOPMETDT">String representing the OOPMETDT of the history benefit year accumulator.</param>
        /// <param name="LIFEMAX">String representing the LIFEMAX of the history benefit year accumulator.</param>
        /// <param name="FERYTDMAX">String representing the FERYTDMAX of the history benefit year accumulator.</param>
        /// <param name="FERLTMAX">String representing the FERLTMAX of the history benefit year accumulator.</param>
        /// <param name="OCYTD">String representing the OCYTD of the history benefit year accumulator.</param>
        /// <param name="OCLIFE">String representing the OCLIFE of the history benefit year accumulator.</param>
        /// <param name="ICYTD">String representing the ICYTD of the history benefit year accumulator.</param>
        /// <param name="ICLIFE">String representing the ICLIFE of the history benefit year accumulator.</param>
        /// <param name="JOURNAL">String representing the JOURNAL of the history benefit year accumulator.</param>
        /// <param name="SYSID">String representing the SYSID of the history benefit year accumulator.</param>
        /// <param name="TIER">String representing the TIER of the history benefit year accumulator.</param>
        /// <param name="NPDEDACC">String representing the NPDEDACC of the history benefit year accumulator.</param>
        /// <param name="NPOOPACC">String representing the NPOOPACC of the history benefit year accumulator.</param>
        /// <param name="NPMAXACC">String representing the NPMAXACC of the history benefit year accumulator.</param>
        /// <param name="QTR4DEDACC">String representing the QTR4DEDACC of the history benefit year accumulator.</param>
        /// <param name="QTR4OOPACC">String representing the QTR4OOPACC of the history benefit year accumulator.</param>
        /// <param name="QTR4MAXACC">String representing the QTR4MAXACC of the history benefit year accumulator.</param>
        /// <param name="MEDDEDACC">String representing the MEDDEDACC of the history benefit year accumulator.</param>
        /// <param name="MEDOOPACC">String representing the MEDOOPACC of the history benefit year accumulator.</param>
        /// <param name="MEDMAXACC">String representing the MEDMAXACC of the history benefit year accumulator.</param>
        /// <param name="DIAPHRDT">String representing the DIAPHRDT of the history benefit year accumulator.</param>
        /// <param name="NPMEDMAX">String representing the NPMEDMAX of the history benefit year accumulator.</param>
        /// <param name="NPMEDOOP">String representing the NPMEDOOP of the history benefit year accumulator.</param>
        /// <param name="NPMEDDED">String representing the NPMEDDED of the history benefit year accumulator.</param>
        /// <param name="LASTCLAIM">String representing the LASTCLAIM of the history benefit year accumulator.</param>
        /// <param name="OTHERID">String representing the OTHERID of the history benefit year accumulator.</param>
        /// <param name="GHYTDMAX">String representing the GHYTDMAX of the history benefit year accumulator.</param>
        /// <param name="GHLTMAX">String representing the GHLTMAX of the history benefit year accumulator.</param>
        /// <param name="CPYSUBS">String representing the CPYSUBS of the history benefit year accumulator.</param>
        /// <param name="DEDSUBS">String representing the DEDSUBS of the history benefit year accumulator.</param>
        /// <param name="TROOP">String representing the TROOP of the history benefit year accumulator.</param>
        /// <param name="TOTPRC">String representing the TOTPRC of the history benefit year accumulator.</param>
        /// <param name="GAP_TROOP">String representing the GAP_TROOP of the history benefit year accumulator.</param>
        /// <param name="ENRADJ">String representing the ENRADJ of the history benefit year accumulator.</param>
        /// <param name="CLASS">String representing the CLASS of the history benefit year accumulator.</param>
        /// <param name="BYATROOP">String representing the BYATROOP of the history benefit year accumulator.</param>
        /// <param name="PARTBDED">String representing the PARTBDED of the history benefit year accumulator.</param>
        /// <param name="PARTBOOP">String representing the PARTBOOP of the history benefit year accumulator.</param>
        /// <param name="PARTBMAX">String representing the PARTBMAX of the history benefit year accumulator.</param>
        /// <param name="BDEDMETDT">String representing the BDEDMETDT of the history benefit year accumulator.</param>
        /// <param name="BOOPMETDT">String representing the BOOPMETDT of the history benefit year accumulator.</param>
        /// <param name="BMAXMETDT">String representing the BMAXMETDT of the history benefit year accumulator.</param>
        /// <param name="TROOPIC">String representing the TROOPIC of the history benefit year accumulator.</param>
        /// <param name="TOTPRCIC">String representing the TOTPRCIC of the history benefit year accumulator.</param>
        /// <param name="CPYSUBSIC">String representing the CPYSUBSIC of the history benefit year accumulator.</param>
        /// <param name="GAPCPYSUB">String representing the GAPCPYSUB of the history benefit year accumulator.</param>
        /// <param name="GAPTOTPRC">String representing the GAPTOTPRC of the history benefit year accumulator.</param>
        /// <param name="TEQFEE">String representing the TEQFEE of the history benefit year accumulator.</param>
        /// <param name="SPECDED">String representing the SPECDED of the history benefit year accumulator.</param>
        /// <param name="SPECOOP">String representing the SPECOOP of the history benefit year accumulator.</param>
        /// <param name="SPECDOLLAR">String representing the SPECDOLLAR of the history benefit year accumulator.</param>
        /// <param name="USERNAME">String representing the USERNAME of the history benefit year accumulator.</param>
        /// <returns><see cref="string" /> representing the system identifier of the history record that was written</returns>
        public string SaveHistoryAccumulator(string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
                                  string PLANTYPE, DateTime EFFDT, DateTime TRMDT, double BROKERYTD, double SMOKINGYTD, double SMOKINGLT,
                                  double COPAY, double PRODSEL, double DEDUCT, DateTime DEDMETDT, double EXCEEDMAX, DateTime MAXMETDT,
                                  DateTime OOPMETDT, double LIFEMAX, double FERYTDMAX, double FERLTMAX, double OCYTD, double OCLIFE, double ICYTD,
                                  double ICLIFE, string JOURNAL, string SYSID, string TIER, double NPDEDACC, double NPOOPACC, double NPMAXACC,
                                  double QTR4DEDACC, double QTR4OOPACC, double QTR4MAXACC, double MEDDEDACC, double MEDOOPACC, double MEDMAXACC,
                                  DateTime DIAPHRDT, double NPMEDMAX, double NPMEDOOP, double NPMEDDED, DateTime LASTCLAIM, string OTHERID,
                                  double GHYTDMAX, double GHLTMAX, double CPYSUBS, double DEDSUBS, double TROOP, double TOTPRC, double GAP_TROOP,
                                  double ENRADJ, string CLASS, double BYATROOP, double PARTBDED, double PARTBOOP, double PARTBMAX, DateTime BDEDMETDT,
                                  DateTime BOOPMETDT, DateTime BMAXMETDT, double TROOPIC, double TOTPRCIC, double CPYSUBSIC, double GAPCPYSUB,
                                  double GAPTOTPRC, double TEQFEE, double SPECDED, double SPECOOP, double SPECDOLLAR, string USERNAME)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID", PLNID?.ToUpper() },
                {"@ENRID", ENRID?.ToUpper() },
                {"@SUBID", SUBID?.ToUpper() },
                {"@ENRAMT", ENRAMT },
                {"@YTDRX", YTDRX },
                {"@YTDDOLLAR", YTDDOLLAR },
                {"@PLANTYPE", PLANTYPE?.ToUpper() },
                {"@EFFDT", EFFDT },
                {"@TRMDT", TRMDT },
                {"@BROKERYTD", BROKERYTD },
                {"@SMOKINGYTD", SMOKINGYTD },
                {"@SMOKINGLT", SMOKINGLT },
                {"@COPAY", COPAY },
                {"@PRODSEL", PRODSEL },
                {"@DEDUCT", DEDUCT },
                {"@DEDMETDT", DEDMETDT },
                {"@EXCEEDMAX", EXCEEDMAX },
                {"@MAXMETDT", MAXMETDT },
                {"@OOPMETDT", OOPMETDT },
                {"@LIFEMAX", LIFEMAX },
                {"@FERYTDMAX", FERYTDMAX },
                {"@FERLTMAX", FERLTMAX },
                {"@OCYTD", OCYTD },
                {"@OCLIFE", OCLIFE },
                {"@ICYTD", ICYTD },
                {"@ICLIFE", ICLIFE },
                {"@JOURNAL", JOURNAL?.ToUpper() },
                {"@SYSID", SYSID?.ToUpper() },
                {"@TIER", TIER?.ToUpper() },
                {"@NPDEDACC", NPDEDACC },
                {"@NPOOPACC", NPOOPACC },
                {"@NPMAXACC", NPMAXACC },
                {"@QTR4DEDACC", QTR4DEDACC },
                {"@QTR4OOPACC", QTR4OOPACC },
                {"@QTR4MAXACC", QTR4MAXACC },
                {"@MEDDEDACC", MEDDEDACC },
                {"@MEDOOPACC", MEDOOPACC },
                {"@MEDMAXACC", MEDMAXACC },
                {"@DIAPHRDT", DIAPHRDT },
                {"@NPMEDMAX", NPMEDMAX },
                {"@NPMEDOOP", NPMEDOOP },
                {"@NPMEDDED", NPMEDDED },
                {"@LASTCLAIM", LASTCLAIM },
                {"@OTHERID", OTHERID?.ToUpper() },
                {"@GHYTDMAX", GHYTDMAX },
                {"@GHLTMAX", GHLTMAX },
                {"@CPYSUBS", CPYSUBS },
                {"@DEDSUBS", DEDSUBS },
                {"@TROOP", TROOP },
                {"@TOTPRC", TOTPRC },
                {"@GAP_TROOP", GAP_TROOP },
                {"@ENRADJ", ENRADJ },
                {"@CLASS", CLASS?.ToUpper() },
                {"@BYATROOP", BYATROOP },
                {"@PARTBDED", PARTBDED },
                {"@PARTBOOP", PARTBOOP },
                {"@PARTBMAX", PARTBMAX },
                {"@BDEDMETDT", BDEDMETDT },
                {"@BOOPMETDT",BOOPMETDT  },
                {"@BMAXMETDT", BMAXMETDT },
                {"@TROOPIC", TROOPIC },
                {"@TOTPRCIC", TOTPRCIC },
                {"@CPYSUBSIC", CPYSUBSIC },
                {"@GAPCPYSUB", GAPCPYSUB },
                {"@GAPTOTPRC", GAPTOTPRC },
                {"@TEQFEE", TEQFEE },
                {"@SPECDED", SPECDED },
                {"@SPECOOP", SPECOOP },
                {"@SPECDOLLAR", SPECDOLLAR },
                {"@CHANGEDBY", ".NET DAL" },
                {"@DATE", DateTime.Now },
                {"@TIME", DateTime.Now.ToString("HH:mm:ss") },
                {"@USERNAME", USERNAME }
            };

            DataHelper.ExecuteNonQuery("apiPBM_APSBYH_insert", CommandType.StoredProcedure, parameters);

            return SYSID;
        }

        /// <summary>
        /// Insert or update a member in the database
        /// </summary>
        /// <param name="ENRID">String representing the ENRID of the member</param>
        /// <param name="PLNID">String representing the PLNID of the member</param>
        /// <param name="SUBID">String representing the SUBID of the member</param>
        /// <param name="CARDID">String representing the CARDID of the member</param>
        /// <param name="PERSON">String representing the PERSON of the member</param>
        /// <param name="EFFDT">DateTime representing the EFFDT of the member</param>
        /// <param name="TRMDT">DateTime representing the TRMDT of the member</param>
        /// <param name="FLEX1">String representing the FLEX1 of the member</param>
        /// <param name="FLEX2">String representing the FLEX2 of the member</param>
        /// <param name="RELCD">String representing the RELCD of the member</param>
        /// <param name="ELGOVER">String representing the ELGOVER of the member</param>
        /// <param name="FNAME">String representing the FNAME of the member</param>
        /// <param name="MNAME">String representing the MNAME of the member</param>
        /// <param name="LNAME">String representing the LNAME of the member</param>
        /// <param name="ADDR">String representing the ADDR of the member</param>
        /// <param name="ADDR2">String representing the ADDR2 of the member</param>
        /// <param name="CITY">String representing the CITY of the member</param>
        /// <param name="STATE">String representing the STATE of the member</param>
        /// <param name="ZIP">String representing the ZIP of the member</param>
        /// <param name="ZIP4">String representing the ZIP4 of the member</param>
        /// <param name="DOB">DateTime representing the DOB of the member</param>
        /// <param name="SEX">String representing the SEX of the member</param>
        /// <param name="ELGCD">String representing the ELGCD of the member</param>
        /// <param name="EMPCD">String representing the EMPCD of the member</param>
        /// <param name="CRDDT">String representing the CRDDT of the member</param>
        /// <param name="SYSID">String representing the SYSID of the member</param>
        /// <param name="LSTDTCARD">DateTime representing the LSTDTCARD of the member</param>
        /// <param name="NOUPDATE">String representing the NOUPDATE of the member</param>
        /// <param name="NDCUPDATE">String representing the NDCUPDATE of the member</param>
        /// <param name="LASTUPDT">DateTime representing the LASTUPDT of the member</param>
        /// <param name="MBRSINCE">DateTime representing the MBRSINCE of the member</param>
        /// <param name="PHYID">String representing the PHYID of the member</param>
        /// <param name="OLDPERSON">String representing the OLDPERSON of the member</param>
        /// <param name="FLEX3">String representing the FLEX3 of the member</param>
        /// <param name="OTHERID">String representing the OTHERID of the member</param>
        /// <param name="DEPCODE">String representing the DEPCODE of the member</param>
        /// <param name="MAINT">String representing the MAINT of the member</param>
        /// <param name="ACCUM">Double representing the ACCUM of the member</param>
        /// <param name="PATSTAT">String representing the PATSTAT of the member</param>
        /// <param name="ENRCOPAYM">String representing the ENRCOPAYM of the member</param>
        /// <param name="ENRCOPAYR">String representing the ENRCOPAYR of the member</param>
        /// <param name="PHYSREQ">String representing the PHYSREQ of the member</param>
        /// <param name="USEELM">String representing the USEELM of the member</param>
        /// <param name="ACCMETH">String representing the ACCMETH of the member</param>
        /// <param name="CARDID2">String representing the CARDID2 of the member</param>
        /// <param name="COB">String representing the COB of the member</param>
        /// <param name="JOURNAL">String representing the JOURNAL of the member</param>
        /// <param name="ADDEDBY">String representing the ADDEDBY of the member</param>
        /// <param name="PMGID">String representing the PMGID of the member</param>
        /// <param name="PHONE">String representing the PHONE of the member</param>
        /// <param name="MEDICARE">String representing the MEDICARE of the member</param>
        /// <param name="PPNREQENR">String representing the PPNREQENR of the member</param>
        /// <param name="PPNID">String representing the PPNID of the member</param>
        /// <param name="HICN">String representing the HICN of the member</param>
        /// <param name="RXBIN">String representing the RXBIN of the member</param>
        /// <param name="RXPCN">String representing the RXPCN of the member</param>
        /// <param name="RXGROUP">String representing the RXGROUP of the member</param>
        /// <param name="RXID">String representing the RXID of the member</param>
        /// <param name="TRELIG">String representing the TRELIG of the member</param>
        /// <param name="PHYQUAL">String representing the PHYQUAL of the member</param>
        /// <param name="MMEDAYMAX">Double representing the MMEDAYMAX of the member</param>
        /// <param name="ALLOWGOVT">Double representing the ALLOWGOVT of the member</param>
        /// <returns><see cref="string" /> representing the system identifier of the member that was inserted or updated.</returns>
        public string SaveMember(string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                               string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                               string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                               string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                               string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                               string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                               string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                               string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                               int? MMEDAYMAX, string ALLOWGOVT)
        {
            string memberSysid = "";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID", ENRID?.ToUpper() },
                {"@PLNID", PLNID?.ToUpper() },
                {"@SUBID", SUBID?.ToUpper() },
                {"@CARDID", CARDID?.ToUpper() },
                {"@PERSON", PERSON },
                {"@EFFDT", EFFDT },
                {"@TRMDT", TRMDT.GetValueOrDefault() },
                {"@FLEX1", FLEX1?.ToUpper() },
                {"@FLEX2", FLEX2?.ToUpper() },
                {"@RELCD", RELCD?.ToUpper() },
                {"@ELGOVER", ELGOVER?.ToUpper() },
                {"@FNAME", FNAME?.ToUpper() },
                {"@MNAME", MNAME?.ToUpper() },
                {"@LNAME", LNAME?.ToUpper() },
                {"@ADDR", ADDR?.ToUpper() },
                {"@ADDR2", ADDR2?.ToUpper() },
                {"@CITY", CITY?.ToUpper() },
                {"@STATE", STATE?.ToUpper() },
                {"@ZIP", ZIP?.ToUpper() },
                {"@ZIP4", ZIP4?.ToUpper() },
                {"@DOB", DOB },
                {"@SEX", SEX?.ToUpper() },
                {"@ELGCD", ELGCD?.ToUpper() },
                {"@EMPCD", EMPCD?.ToUpper() },
                {"@CRDDT", CRDDT },
                {"@SYSID", SYSID?.ToUpper() },
                {"@LSTDTCARD", LSTDTCARD },
                {"@NOUPDATE", NOUPDATE?.ToUpper() },
                {"@NDCUPDATE", NDCUPDATE?.ToUpper() },
                {"@LASTUPDT", LASTUPDT },
                {"@MBRSINCE", MBRSINCE },
                {"@PHYID", PHYID?.ToUpper() },
                {"@OLDPERSON", OLDPERSON?.ToUpper() },
                {"@FLEX3", FLEX3?.ToUpper() },
                {"@OTHERID", OTHERID?.ToUpper() },
                {"@DEPCODE", DEPCODE?.ToUpper() },
                {"@MAINT", MAINT?.ToUpper() },
                {"@ACCUM", ACCUM },
                {"@PATSTAT", PATSTAT?.ToUpper() },
                {"@ENRCOPAYM", ENRCOPAYM?.ToUpper() },
                {"@ENRCOPAYR", ENRCOPAYR?.ToUpper() },
                {"@PHYSREQ", PHYSREQ?.ToUpper() },
                {"@USEELM", USEELM?.ToUpper() },
                {"@ACCMETH", ACCMETH?.ToUpper() },
                {"@CARDID2", CARDID2?.ToUpper() },
                {"@COB", COB?.ToUpper() },
                {"@JOURNAL", JOURNAL?.ToUpper() },
                {"@ADDEDBY", ADDEDBY?.ToUpper() },
                {"@PMGID", PMGID?.ToUpper() },
                {"@PHONE", PHONE },
                {"@MEDICARE", MEDICARE?.ToUpper() },
                {"@PPNREQENR", PPNREQENR?.ToUpper() },
                {"@PPNID", PPNID?.ToUpper() },
                {"@HICN", HICN?.ToUpper() },
                {"@RXBIN", RXBIN?.ToUpper() },
                {"@RXPCN", RXPCN?.ToUpper() },
                {"@RXGROUP", RXGROUP?.ToUpper() },
                {"@RXID", RXID?.ToUpper() },
                {"@TRELIG", TRELIG?.ToUpper() },
                {"@PHYQUAL", PHYQUAL?.ToUpper() },
                {"@MMEDAYMAX", MMEDAYMAX },
                {"@ALLOWGOVT", ALLOWGOVT.ToUpper() }
            };

            DataHelper.ExecuteReaderWithTransaction("apiPBM_APSENR_save", CommandType.StoredProcedure, parameters, reader =>
            {
                memberSysid = reader.GetStringorDefault("APSENRSYSID");
            });

            return memberSysid;

        }


        /// <summary>
        /// Inserts or updates a suspension record.
        /// </summary>
        /// <param name="TABLENAME">String representing the name of the table the record to be suspended is from</param>
        /// <param name="LINKKEY">String representing the key used to link the suspension record to the table</param>
        /// <param name="CUTOFFDATE">Nullable DateTime representing date of the suspension</param>
        /// <param name="CUTOFFMETH">Single-character string representing the method of suspension</param>
        /// <param name="JOURNAL">String representing the whether to journal the change</param>
        /// <param name="SYSID">String representing the system identifier of the suspension record</param>
        /// <returns><see cref="string" /> representing the system identifier of the suspension record that was inserted or updated.</returns>
        public string SaveSuspension(string TABLENAME, string LINKKEY , DateTime? CUTOFFDATE, string CUTOFFMETH, string JOURNAL, string SYSID)
        {
            string sysid = "";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@TABLENAME", TABLENAME?.ToUpper() },
                {"@LINKKEY", LINKKEY?.ToUpper() },
                {"@CUTOFFDATE", CUTOFFDATE },
                {"@CUTOFFMETH", CUTOFFMETH?.ToUpper() },
                {"@JOURNAL", JOURNAL?.ToUpper() },
                {"@SYSID", SYSID?.ToUpper() }
            };

            DataHelper.ExecuteReaderWithTransaction("apiPBM_APSSUS_save", CommandType.StoredProcedure, parameters, reader =>
            {
                sysid = reader.GetStringorDefault("APSSUSSYSID");
            });

            return sysid;

        }

        /// <summary>
        /// Inserts or updates a benefit year accumulator record in the database.
        /// </summary>
        /// <param name="PLNID">String representing the PLNID of the benefit year accumulator.</param>
        /// <param name="ENRID">String representing the ENRID of the benefit year accumulator.</param>
        /// <param name="SUBID">String representing the SUBID of the benefit year accumulator.</param>
        /// <param name="ENRAMT">String representing the ENRAMT of the benefit year accumulator.</param>
        /// <param name="YTDRX">String representing the YTDRX of the benefit year accumulator.</param>
        /// <param name="YTDDOLLAR">String representing the YTDDOLLAR of the benefit year accumulator.</param>
        /// <param name="PLANTYPE">String representing the PLANTYPE of the benefit year accumulator.</param>
        /// <param name="EFFDT">String representing the EFFDT of the benefit year accumulator.</param>
        /// <param name="TRMDT">String representing the TRMDT of the benefit year accumulator.</param>
        /// <param name="BROKERYTD">String representing the BROKERYTD of the benefit year accumulator.</param>
        /// <param name="SMOKINGYTD">String representing the SMOKINGYTD of the benefit year accumulator.</param>
        /// <param name="SMOKINGLT">String representing the SMOKINGLT of the benefit year accumulator.</param>
        /// <param name="COPAY">String representing the COPAY of the benefit year accumulator.</param>
        /// <param name="PRODSEL">String representing the PRODSEL of the benefit year accumulator.</param>
        /// <param name="DEDUCT">String representing the DEDUCT of the benefit year accumulator.</param>
        /// <param name="DEDMETDT">String representing the DEDMETDT of the benefit year accumulator.</param>
        /// <param name="EXCEEDMAX">String representing the EXCEEDMAX of the benefit year accumulator.</param>
        /// <param name="MAXMETDT">String representing the MAXMETDT of the benefit year accumulator.</param>
        /// <param name="OOPMETDT">String representing the OOPMETDT of the benefit year accumulator.</param>
        /// <param name="LIFEMAX">String representing the LIFEMAX of the benefit year accumulator.</param>
        /// <param name="FERYTDMAX">String representing the FERYTDMAX of the benefit year accumulator.</param>
        /// <param name="FERLTMAX">String representing the FERLTMAX of the benefit year accumulator.</param>
        /// <param name="OCYTD">String representing the OCYTD of the benefit year accumulator.</param>
        /// <param name="OCLIFE">String representing the OCLIFE of the benefit year accumulator.</param>
        /// <param name="ICYTD">String representing the ICYTD of the benefit year accumulator.</param>
        /// <param name="ICLIFE">String representing the ICLIFE of the benefit year accumulator.</param>
        /// <param name="JOURNAL">String representing the JOURNAL of the benefit year accumulator.</param>
        /// <param name="SYSID">String representing the SYSID of the benefit year accumulator.</param>
        /// <param name="TIER">String representing the TIER of the benefit year accumulator.</param>
        /// <param name="NPDEDACC">String representing the NPDEDACC of the benefit year accumulator.</param>
        /// <param name="NPOOPACC">String representing the NPOOPACC of the benefit year accumulator.</param>
        /// <param name="NPMAXACC">String representing the NPMAXACC of the benefit year accumulator.</param>
        /// <param name="QTR4DEDACC">String representing the QTR4DEDACC of the benefit year accumulator.</param>
        /// <param name="QTR4OOPACC">String representing the QTR4OOPACC of the benefit year accumulator.</param>
        /// <param name="QTR4MAXACC">String representing the QTR4MAXACC of the benefit year accumulator.</param>
        /// <param name="MEDDEDACC">String representing the MEDDEDACC of the benefit year accumulator.</param>
        /// <param name="MEDOOPACC">String representing the MEDOOPACC of the benefit year accumulator.</param>
        /// <param name="MEDMAXACC">String representing the MEDMAXACC of the benefit year accumulator.</param>
        /// <param name="DIAPHRDT">String representing the DIAPHRDT of the benefit year accumulator.</param>
        /// <param name="NPMEDMAX">String representing the NPMEDMAX of the benefit year accumulator.</param>
        /// <param name="NPMEDOOP">String representing the NPMEDOOP of the benefit year accumulator.</param>
        /// <param name="NPMEDDED">String representing the NPMEDDED of the benefit year accumulator.</param>
        /// <param name="LASTCLAIM">String representing the LASTCLAIM of the benefit year accumulator.</param>
        /// <param name="OTHERID">String representing the OTHERID of the benefit year accumulator.</param>
        /// <param name="GHYTDMAX">String representing the GHYTDMAX of the benefit year accumulator.</param>
        /// <param name="GHLTMAX">String representing the GHLTMAX of the benefit year accumulator.</param>
        /// <param name="CPYSUBS">String representing the CPYSUBS of the benefit year accumulator.</param>
        /// <param name="DEDSUBS">String representing the DEDSUBS of the benefit year accumulator.</param>
        /// <param name="TROOP">String representing the TROOP of the benefit year accumulator.</param>
        /// <param name="TOTPRC">String representing the TOTPRC of the benefit year accumulator.</param>
        /// <param name="GAP_TROOP">String representing the GAP_TROOP of the benefit year accumulator.</param>
        /// <param name="ENRADJ">String representing the ENRADJ of the benefit year accumulator.</param>
        /// <param name="CLASS">String representing the CLASS of the benefit year accumulator.</param>
        /// <param name="BYATROOP">String representing the BYATROOP of the benefit year accumulator.</param>
        /// <param name="PARTBDED">String representing the PARTBDED of the benefit year accumulator.</param>
        /// <param name="PARTBOOP">String representing the PARTBOOP of the benefit year accumulator.</param>
        /// <param name="PARTBMAX">String representing the PARTBMAX of the benefit year accumulator.</param>
        /// <param name="BDEDMETDT">String representing the BDEDMETDT of the benefit year accumulator.</param>
        /// <param name="BOOPMETDT">String representing the BOOPMETDT of the benefit year accumulator.</param>
        /// <param name="BMAXMETDT">String representing the BMAXMETDT of the benefit year accumulator.</param>
        /// <param name="TROOPIC">String representing the TROOPIC of the benefit year accumulator.</param>
        /// <param name="TOTPRCIC">String representing the TOTPRCIC of the benefit year accumulator.</param>
        /// <param name="CPYSUBSIC">String representing the CPYSUBSIC of the benefit year accumulator.</param>
        /// <param name="GAPCPYSUB">String representing the GAPCPYSUB of the benefit year accumulator.</param>
        /// <param name="GAPTOTPRC">String representing the GAPTOTPRC of the benefit year accumulator.</param>
        /// <param name="TEQFEE">String representing the TEQFEE of the benefit year accumulator.</param>
        /// <param name="SPECDED">String representing the SPECDED of the benefit year accumulator.</param>
        /// <param name="SPECOOP">String representing the SPECOOP of the benefit year accumulator.</param>
        /// <param name="SPECDOLLAR">String representing the SPECDOLLAR of the benefit year accumulator.</param>
        /// <returns><see cref="string" /> representing the system identifier of the benefit year accumulator record that was inserted or updated</returns>
        public string SaveAccumulator(string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
                                  string PLANTYPE, DateTime EFFDT, DateTime TRMDT, double BROKERYTD, double SMOKINGYTD, double SMOKINGLT,
                                  double COPAY, double PRODSEL, double DEDUCT, DateTime DEDMETDT, double EXCEEDMAX, DateTime MAXMETDT,
                                  DateTime OOPMETDT, double LIFEMAX, double FERYTDMAX, double FERLTMAX, double OCYTD, double OCLIFE, double ICYTD,
                                  double ICLIFE, string JOURNAL, string SYSID, string TIER, double NPDEDACC, double NPOOPACC, double NPMAXACC,
                                  double QTR4DEDACC, double QTR4OOPACC, double QTR4MAXACC, double MEDDEDACC, double MEDOOPACC, double MEDMAXACC,
                                  DateTime DIAPHRDT, double NPMEDMAX, double NPMEDOOP, double NPMEDDED, DateTime LASTCLAIM, string OTHERID,
                                  double GHYTDMAX, double GHLTMAX, double CPYSUBS, double DEDSUBS, double TROOP, double TOTPRC, double GAP_TROOP,
                                  double ENRADJ, string CLASS, double BYATROOP, double PARTBDED, double PARTBOOP, double PARTBMAX, DateTime BDEDMETDT,
                                  DateTime BOOPMETDT, DateTime BMAXMETDT, double TROOPIC, double TOTPRCIC, double CPYSUBSIC, double GAPCPYSUB,
                                  double GAPTOTPRC, double TEQFEE, double SPECDED, double SPECOOP, double SPECDOLLAR)
        {
            string sysid = "";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID", PLNID?.ToUpper() },
                {"@ENRID", ENRID?.ToUpper() },
                {"@SUBID", SUBID?.ToUpper() },
                {"@ENRAMT", ENRAMT },
                {"@YTDRX", YTDRX },
                {"@YTDDOLLAR", YTDDOLLAR },
                {"@PLANTYPE", PLANTYPE?.ToUpper() },
                {"@EFFDT", EFFDT },
                {"@TRMDT", TRMDT },
                {"@BROKERYTD", BROKERYTD },
                {"@SMOKINGYTD", SMOKINGYTD },
                {"@SMOKINGLT", SMOKINGLT },
                {"@COPAY", COPAY },
                {"@PRODSEL", PRODSEL },
                {"@DEDUCT", DEDUCT },
                {"@DEDMETDT", DEDMETDT },
                {"@EXCEEDMAX", EXCEEDMAX },
                {"@MAXMETDT", MAXMETDT },
                {"@OOPMETDT", OOPMETDT },
                {"@LIFEMAX", LIFEMAX },
                {"@FERYTDMAX", FERYTDMAX },
                {"@FERLTMAX", FERLTMAX },
                {"@OCYTD", OCYTD },
                {"@OCLIFE", OCLIFE },
                {"@ICYTD", ICYTD },
                {"@ICLIFE", ICLIFE },
                {"@JOURNAL", JOURNAL?.ToUpper() },
                {"@SYSID", SYSID?.ToUpper() },
                {"@TIER", TIER?.ToUpper() },
                {"@NPDEDACC", NPDEDACC },
                {"@NPOOPACC", NPOOPACC },
                {"@NPMAXACC", NPMAXACC },
                {"@QTR4DEDACC", QTR4DEDACC },
                {"@QTR4OOPACC", QTR4OOPACC },
                {"@QTR4MAXACC", QTR4MAXACC },
                {"@MEDDEDACC", MEDDEDACC },
                {"@MEDOOPACC", MEDOOPACC },
                {"@MEDMAXACC", MEDMAXACC },
                {"@DIAPHRDT", DIAPHRDT },
                {"@NPMEDMAX", NPMEDMAX },
                {"@NPMEDOOP", NPMEDOOP },
                {"@NPMEDDED", NPMEDDED },
                {"@LASTCLAIM", LASTCLAIM },
                {"@OTHERID", OTHERID?.ToUpper() },
                {"@GHYTDMAX", GHYTDMAX },
                {"@GHLTMAX", GHLTMAX },
                {"@CPYSUBS", CPYSUBS },
                {"@DEDSUBS", DEDSUBS },
                {"@TROOP", TROOP },
                {"@TOTPRC", TOTPRC },
                {"@GAP_TROOP", GAP_TROOP },
                {"@ENRADJ", ENRADJ },
                {"@CLASS", CLASS?.ToUpper() },
                {"@BYATROOP", BYATROOP },
                {"@PARTBDED", PARTBDED },
                {"@PARTBOOP", PARTBOOP },
                {"@PARTBMAX", PARTBMAX },
                {"@BDEDMETDT", BDEDMETDT },
                {"@BOOPMETDT",BOOPMETDT  },
                {"@BMAXMETDT", BMAXMETDT },
                {"@TROOPIC", TROOPIC },
                {"@TOTPRCIC", TOTPRCIC },
                {"@CPYSUBSIC", CPYSUBSIC },
                {"@GAPCPYSUB", GAPCPYSUB },
                {"@GAPTOTPRC", GAPTOTPRC },
                {"@TEQFEE", TEQFEE },
                {"@SPECDED", SPECDED },
                {"@SPECOOP", SPECOOP },
                {"@SPECDOLLAR", SPECDOLLAR }
            };

            DataHelper.ExecuteReaderWithTransaction("apiPBM_APSBYA_save", CommandType.StoredProcedure, parameters, reader =>
            {
                sysid = reader.GetStringorDefault("APSBYASYSID");
            });

            return sysid;
        }

        /// <summary>
        /// Insert a history member in the database
        /// </summary>
        /// <param name="ENRID">String representing the ENRID of the member</param>
        /// <param name="PLNID">String representing the PLNID of the member</param>
        /// <param name="SUBID">String representing the SUBID of the member</param>
        /// <param name="CARDID">String representing the CARDID of the member</param>
        /// <param name="PERSON">String representing the PERSON of the member</param>
        /// <param name="EFFDT">DateTime representing the EFFDT of the member</param>
        /// <param name="TRMDT">DateTime representing the TRMDT of the member</param>
        /// <param name="FLEX1">String representing the FLEX1 of the member</param>
        /// <param name="FLEX2">String representing the FLEX2 of the member</param>
        /// <param name="RELCD">String representing the RELCD of the member</param>
        /// <param name="ELGOVER">String representing the ELGOVER of the member</param>
        /// <param name="FNAME">String representing the FNAME of the member</param>
        /// <param name="MNAME">String representing the MNAME of the member</param>
        /// <param name="LNAME">String representing the LNAME of the member</param>
        /// <param name="ADDR">String representing the ADDR of the member</param>
        /// <param name="ADDR2">String representing the ADDR2 of the member</param>
        /// <param name="CITY">String representing the CITY of the member</param>
        /// <param name="STATE">String representing the STATE of the member</param>
        /// <param name="ZIP">String representing the ZIP of the member</param>
        /// <param name="ZIP4">String representing the ZIP4 of the member</param>
        /// <param name="DOB">DateTime representing the DOB of the member</param>
        /// <param name="SEX">String representing the SEX of the member</param>
        /// <param name="ELGCD">String representing the ELGCD of the member</param>
        /// <param name="EMPCD">String representing the EMPCD of the member</param>
        /// <param name="CRDDT">String representing the CRDDT of the member</param>
        /// <param name="SYSID">String representing the SYSID of the member</param>
        /// <param name="LSTDTCARD">DateTime representing the LSTDTCARD of the member</param>
        /// <param name="NOUPDATE">String representing the NOUPDATE of the member</param>
        /// <param name="NDCUPDATE">String representing the NDCUPDATE of the member</param>
        /// <param name="LASTUPDT">DateTime representing the LASTUPDT of the member</param>
        /// <param name="MBRSINCE">DateTime representing the MBRSINCE of the member</param>
        /// <param name="PHYID">String representing the PHYID of the member</param>
        /// <param name="OLDPERSON">String representing the OLDPERSON of the member</param>
        /// <param name="FLEX3">String representing the FLEX3 of the member</param>
        /// <param name="OTHERID">String representing the OTHERID of the member</param>
        /// <param name="DEPCODE">String representing the DEPCODE of the member</param>
        /// <param name="MAINT">String representing the MAINT of the member</param>
        /// <param name="ACCUM">Double representing the ACCUM of the member</param>
        /// <param name="PATSTAT">String representing the PATSTAT of the member</param>
        /// <param name="ENRCOPAYM">String representing the ENRCOPAYM of the member</param>
        /// <param name="ENRCOPAYR">String representing the ENRCOPAYR of the member</param>
        /// <param name="PHYSREQ">String representing the PHYSREQ of the member</param>
        /// <param name="USEELM">String representing the USEELM of the member</param>
        /// <param name="ACCMETH">String representing the ACCMETH of the member</param>
        /// <param name="CARDID2">String representing the CARDID2 of the member</param>
        /// <param name="COB">String representing the COB of the member</param>
        /// <param name="JOURNAL">String representing the JOURNAL of the member</param>
        /// <param name="ADDEDBY">String representing the ADDEDBY of the member</param>
        /// <param name="PMGID">String representing the PMGID of the member</param>
        /// <param name="PHONE">String representing the PHONE of the member</param>
        /// <param name="MEDICARE">String representing the MEDICARE of the member</param>
        /// <param name="PPNREQENR">String representing the PPNREQENR of the member</param>
        /// <param name="PPNID">String representing the PPNID of the member</param>
        /// <param name="HICN">String representing the HICN of the member</param>
        /// <param name="RXBIN">String representing the RXBIN of the member</param>
        /// <param name="RXPCN">String representing the RXPCN of the member</param>
        /// <param name="RXGROUP">String representing the RXGROUP of the member</param>
        /// <param name="RXID">String representing the RXID of the member</param>
        /// <param name="TRELIG">String representing the TRELIG of the member</param>
        /// <param name="PHYQUAL">String representing the PHYQUAL of the member</param>
        /// <param name="MMEDAYMAX">Double representing the MMEDAYMAX of the member</param>
        /// <param name="ALLOWGOVT">Double representing the ALLOWGOVT of the member</param>
        /// <param name="USERNAME">Double representing the USERNAME of the account updating the member</param>
        /// <returns>None</returns>
        public string SaveHistoryMember(string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                               string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                               string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                               string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                               string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                               string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                               string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                               string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                               int? MMEDAYMAX, string ALLOWGOVT, string USERNAME)
        {
            List<MemberDTO> dto = new List<MemberDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID", ENRID?.ToUpper() },
                {"@PLNID", PLNID?.ToUpper() },
                {"@SUBID", SUBID?.ToUpper() },
                {"@CARDID", CARDID?.ToUpper() },
                {"@PERSON", PERSON },
                {"@EFFDT", EFFDT },
                {"@TRMDT", TRMDT.GetValueOrDefault() },
                {"@FLEX1", FLEX1?.ToUpper() },
                {"@FLEX2", FLEX2?.ToUpper() },
                {"@RELCD", RELCD?.ToUpper() },
                {"@ELGOVER", ELGOVER?.ToUpper() },
                {"@FNAME", FNAME?.ToUpper() },
                {"@MNAME", MNAME?.ToUpper() },
                {"@LNAME", LNAME?.ToUpper() },
                {"@ADDR", ADDR?.ToUpper() },
                {"@ADDR2", ADDR2?.ToUpper() },
                {"@CITY", CITY?.ToUpper() },
                {"@STATE", STATE?.ToUpper() },
                {"@ZIP", ZIP?.ToUpper() },
                {"@ZIP4", ZIP4?.ToUpper() },
                {"@DOB", DOB },
                {"@SEX", SEX?.ToUpper() },
                {"@ELGCD", ELGCD?.ToUpper() },
                {"@EMPCD", EMPCD?.ToUpper() },
                {"@CRDDT", CRDDT },
                {"@SYSID", SYSID?.ToUpper() },
                {"@LSTDTCARD", LSTDTCARD },
                {"@NOUPDATE", NOUPDATE?.ToUpper() },
                {"@NDCUPDATE", NDCUPDATE?.ToUpper() },
                {"@LASTUPDT", LASTUPDT },
                {"@MBRSINCE", MBRSINCE },
                {"@PHYID", PHYID?.ToUpper() },
                {"@OLDPERSON", OLDPERSON?.ToUpper() },
                {"@FLEX3", FLEX3?.ToUpper() },
                {"@OTHERID", OTHERID?.ToUpper() },
                {"@DEPCODE", DEPCODE?.ToUpper() },
                {"@MAINT", MAINT?.ToUpper() },
                {"@ACCUM", ACCUM },
                {"@PATSTAT", PATSTAT?.ToUpper() },
                {"@ENRCOPAYM", ENRCOPAYM?.ToUpper() },
                {"@ENRCOPAYR", ENRCOPAYR?.ToUpper() },
                {"@PHYSREQ", PHYSREQ?.ToUpper() },
                {"@USEELM", USEELM?.ToUpper() },
                {"@ACCMETH", ACCMETH?.ToUpper() },
                {"@CARDID2", CARDID2?.ToUpper() },
                {"@COB", COB?.ToUpper() },
                {"@JOURNAL", JOURNAL?.ToUpper() },
                {"@ADDEDBY", ADDEDBY?.ToUpper() },
                {"@PMGID", PMGID?.ToUpper() },
                {"@PHONE", PHONE },
                {"@MEDICARE", MEDICARE?.ToUpper() },
                {"@PPNREQENR", PPNREQENR?.ToUpper() },
                {"@PPNID", PPNID?.ToUpper() },
                {"@HICN", HICN?.ToUpper() },
                {"@RXBIN", RXBIN?.ToUpper() },
                {"@RXPCN", RXPCN?.ToUpper() },
                {"@RXGROUP", RXGROUP?.ToUpper() },
                {"@RXID", RXID?.ToUpper() },
                {"@TRELIG", TRELIG?.ToUpper() },
                {"@PHYQUAL", PHYQUAL?.ToUpper() },
                {"@MMEDAYMAX", MMEDAYMAX },
                {"@ALLOWGOVT", ALLOWGOVT.ToUpper() },
                {"@CHANGEDBY", ".NET DAL" },
                {"@DATE", DateTime.Now },
                {"@TIME", DateTime.Now.ToString("HH:mm:ss") },
                {"@USERNAME", USERNAME }
            };

            DataHelper.ExecuteNonQuery("apiPBM_APSENH_insert", CommandType.StoredProcedure, parameters);

            return SYSID;
        }

        /// <summary>
        /// Adds a log detail record documenting current and previous field values
        /// </summary>
        /// <param name="logSysid">String representing the system identifier of the log record the log detail corresponds to.</param>
        /// <param name="logTable">String representing the name of the table the log this detail corresponds to is stored in</param>
        /// <param name="fieldName">String representing the name of the field that was updated</param>
        /// <param name="oldValue">String representing the old value of the field</param>
        /// <param name="newValue">String representing the new value of the field</param>
        /// <returns>None</returns>
        public void InsertLogDetail(string logSysid, string logTable, string fieldName, string oldValue, string newValue)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LOGFILE",  logTable?.ToUpper()},
                {"@LOGSYSID",  logSysid?.ToUpper()},
                {"@FLDCHANGED",  fieldName?.ToUpper()},
                {"@OLDVAL",  oldValue},
                {"@NEWVAL",  newValue}
            };

            DataHelper.ExecuteNonQuery("apiPBM_LOGDET_insert", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        ///  Write an import record to the database.
        /// </summary>
        /// <param name="importID">Long representing the identifier of the import</param>
        /// <param name="transactionTypeID">Long representing the identifier of the transaction type</param>
        /// <param name="clientID">Long representing the identifier of the client</param>
        /// <param name="importStatusID">Long representing the identifier of the import status</param>
        /// <param name="preImportID">Long representing the identifier of the pre-import record</param>
        /// <param name="planID">String representing the plan identifier of the patient</param>
        /// <param name="patName">String representing the name of the patient</param>
        /// <param name="rawData">String representing the full eligibility import request in XML format</param>
        /// <param name="createdTime">DateTime representing the date and time the import record was created</param>
        /// <param name="completedTime">DateTime representing the date and time the import was completed</param>
        /// <param name="returnValue">String representing the return value of the import</param>
        /// <param name="warningMessage">String representing warnings generated while processing the import</param>
        /// <param name="errorMessage">String representing errors generated while processing the import</param>
        /// <param name="recordID">String representing the external record identifier of the import</param>
        /// <param name="recordAction">String representing the action being performed on the import record</param>
        /// <param name="insertAppUserID">Long representing the identifier of the inserting app user</param>
        /// <param name="updateAppUserID">Long representing the identifier of the updating app user</param>
        /// <returns><see cref="long" /> representing the identifier of the import that was saved.</returns>
        public long SaveImport(long importID, long? transactionTypeID, long clientID, long importStatusID, long? preImportID, string planID, string patName,
                                                      string rawData, DateTime? createdTime, DateTime completedTime, string returnValue, string warningMessage,
                                                      string errorMessage, string recordID, string recordAction, long insertAppUserID, long updateAppUserID)
        {
            long result = 0;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ImportID",  importID},
                {"@TransactionTypeID",  transactionTypeID},
                {"@ClientID",  clientID},
                {"@ImportStatusID",  importStatusID},
                {"@PreImportID",  preImportID},
                {"@PlanID",  planID},
                {"@PatName",  patName},
                {"@RawData",  rawData},
                {"@CreatedTime",  createdTime},
                {"@CompletedTime",  completedTime},
                {"@ReturnValue",  returnValue},
                {"@WarningMessage",  warningMessage},
                {"@ErrorMessage",  errorMessage},
                {"@RecIdentifier",  recordID},
                {"@RecordAction",  recordAction},
                {"@InsertAppUserID",  insertAppUserID},
                {"@UpdateAppUserID",  updateAppUserID}
            };

            DataHelper.ExecuteReader("apiPBM_Import_save", CommandType.StoredProcedure, parameters, reader =>
            {
                result = reader.GetInt64(0);
            });

            return result;
        }

        /// <summary>
        ///  Verifies whether a plan with the given plan ID exists.
        /// </summary>
        /// <param name="planID">String representing the identifier of the member plan</param>
        /// <returns><see cref="bool" /> representing whether a plan with the given plan ID exists</returns>
        public bool PlanExists(string planID)
        {
            bool exists = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID", planID}
            };

            DataHelper.ExecuteReader("apiPBM_APSPLN_exists", CommandType.StoredProcedure, parameters, reader =>
            {
                exists = reader.GetBooleanSafe("PlanExists");
            });

            return exists;
        }

        /// <summary>
        ///  Verifies whether a plan with the given plan ID is active on the given adjustment date.
        /// </summary>
        /// <param name="planID">String representing the identifier of the member plan</param>
        /// <param name="adjustmentDate">DateTime representing the date the adjustment is being made</param>
        /// <returns><see cref="bool" /> representing whether a plan with the given plan ID is in date on the given adjustment date</returns>
        public bool PlanInDate(string planID, DateTime adjustmentDate)
        {
            bool exists = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID", planID},
                {"@EFFDT", adjustmentDate}
            };

            DataHelper.ExecuteReader("apiPBM_APSPLN_inDate", CommandType.StoredProcedure, parameters, reader =>
            {
                exists = reader.GetBooleanSafe("PlanInDate");
            });

            return exists;
        }

        #endregion
    }
}
