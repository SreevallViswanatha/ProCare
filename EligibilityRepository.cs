using ProCare.API.PBM.Repository.DataAccess.Eligibility;
using ProCare.API.PBM.Repository.DTO.Eligibility;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using System;
using System.Collections.Generic;

namespace ProCare.API.PBM.Repository
{
    public class EligibilityRepository : BasedbRepository, IEligibilityRepository
    {
        /// <inheritdoc />

        #region Constructor

        public EligibilityRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }
        #endregion

        #region Public Methods

        /// <summary>
        ///  Read data for a Client and its ClientConfiguration from the database.
        /// </summary>
        /// <param name="clientID">string representing note entity identifier</param>
        /// <returns><see cref="ClientWithConfigurationsDTO" /> representing the client and its configuration details</returns>
        public ClientWithConfigurationsDTO GetClientWithClientConfiguration(long clientID)
        {
            var sqlHelper = new EligibilityDataAccess(DataHelper);
            ClientWithConfigurationsDTO output = sqlHelper.ReadClientWithClientConfiguration(clientID);

            return output;
        }

        /// <summary>
        ///  Read data for a Client and its ClientConfiguration from the database.
        /// </summary>
        /// <param name="clientID">string representing note entity identifier</param>
        /// <returns><see cref="ClientACPDTO" /> representing the client and its configuration details</returns>
        public ClientACPDTO GetClientACPConfiguration(long clientID)
        {
            var sqlHelper = new EligibilityDataAccess(DataHelper);
            ClientACPDTO output = sqlHelper.ReadClientACPConfiguration(clientID);

            return output;
        }

        /// <summary>
        /// Read a list of members with matching Plan ID, CardID, CardID2, and Person from the database.
        /// </summary>
        /// <param name="planID">String representing the history member plan identifier</param>
        /// <param name="cardID">String representing first 9 digits of the history member card identifier</param>
        /// <param name="cardID2">String representing additional digits of the history member card identifier</param>
        /// <param name="person">String representing history member person identifier</param>
        /// <returns><see cref="List{MemberDTO}" /> representing the history members with matching PlanID, CardID, CardID2, and Person</returns>
        public List<MemberDTO> GetMembersByPlanIDCardIDCardID2Person(string adsConnectionString, string planID, string cardID, string cardID2, string person)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            List<MemberDTO> output = adsHelper.ReadMemberByPlanIDCardIDCardID2Person(planID, cardID, cardID2, person);

            return output;
        }

        /// <summary>
        /// Read a list of benefit year accumulators with matching Plan ID and Enrollee ID from the database.
        /// </summary>
        /// <param name="planID">String representing the history member plan identifier</param>
        /// <param name="enrolleeID">String representing the history member plan identifier</param>
        /// <returns><see cref="List{BenefitYearAccumulatorDTO}" /> representing the benefit year accumulators with matching PlanID and EnrolleeID</returns>
        public List<BenefitYearAccumulatorDTO> GetBenefitYearAccumulatorByPlanIDEnrolleeID(string adsConnectionString, string planID, string enrolleeID)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            List<BenefitYearAccumulatorDTO> output = adsHelper.ReadBenefitYearAccumulatorByPlanIDEnrolleeID(planID, enrolleeID);

            return output;
        }

        /// <summary>
        /// Read a list of suspension records with a matching link key for the given table from the database.
        /// </summary>
        /// <param name="tableName">String representing the name of the table the suspended record is from</param>
        /// <param name="linkKey">String representing the key value used to look up data in the table</param>
        /// <returns><see cref="List{SuspensionDTO}" /> representing the suspension records for the given table name and link key</returns>
        public List<SuspensionDTO> GetSuspensionByTableNameLinkKey(string adsConnectionString, string tableName, string linkKey)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            List<SuspensionDTO> output = adsHelper.ReadSuspensionByTableNameLinkKey(tableName, linkKey);

            return output;
        }

        /// <summary>
        /// Read a list of history members with matching Plan ID, CardID, CardID2, and Person from the database.
        /// </summary>
        /// <param name="planID">String representing the history member plan identifier</param>
        /// <param name="cardID">String representing first 9 digits of the history member card identifier</param>
        /// <param name="cardID2">String representing additional digits of the history member card identifier</param>
        /// <param name="person">String representing history member person identifier</param>
        /// <returns><see cref="List{MemberDTO}" /> representing the history members with matching PlanID, CardID, CardID2, and Person</returns>
        public List<MemberDTO> GetHistoryMembersByPlanIDCardIDCardID2Person(string adsConnectionString, string planID, string cardID, string cardID2, string person)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            List<MemberDTO> output = adsHelper.ReadHistoryMemberByPlanIDCardIDCardID2Person(planID, cardID, cardID2, person);

            return output;
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
        public string LogMember(string adsConnectionString, string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                               string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                               string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                               string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                               string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                               string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                               string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                               string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                               int? MMEDAYMAX, string ALLOWGOVT, string USERNAME)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            return adsHelper.LogMember(ENRID, PLNID, SUBID, CARDID, PERSON, EFFDT, TRMDT, FLEX1, FLEX2, RELCD, ELGOVER, FNAME, MNAME, LNAME, ADDR, ADDR2,
                                 CITY, STATE, ZIP, ZIP4, DOB, SEX, ELGCD, EMPCD, CRDDT, SYSID, LSTDTCARD, NOUPDATE, NDCUPDATE, LASTUPDT, MBRSINCE,
                                 PHYID, OLDPERSON, FLEX3, OTHERID, DEPCODE, MAINT, ACCUM, PATSTAT, ENRCOPAYM, ENRCOPAYR, PHYSREQ, USEELM, ACCMETH,
                                 CARDID2, COB, JOURNAL, ADDEDBY, PMGID, PHONE, MEDICARE, PPNREQENR, PPNID, HICN, RXBIN, RXPCN, RXGROUP, RXID, TRELIG,
                                 PHYQUAL, MMEDAYMAX, ALLOWGOVT, USERNAME);
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
        public void LogSuspension(string adsConnectionString, string TABLENAME, string LINKKEY, DateTime? CUTOFFDATE, string CUTOFFMETH, string JOURNAL, string SYSID,
                                  string USERNAME)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            adsHelper.LogSuspension(TABLENAME, LINKKEY, CUTOFFDATE, CUTOFFMETH, JOURNAL, SYSID, USERNAME);
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
        public string LogAccumulator(string adsConnectionString, string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
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
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            return adsHelper.LogAccumulator(PLNID, ENRID, SUBID, ENRAMT, YTDRX, YTDDOLLAR, PLANTYPE, EFFDT, TRMDT, BROKERYTD, SMOKINGYTD, SMOKINGLT,
                                         COPAY, PRODSEL, DEDUCT, DEDMETDT, EXCEEDMAX, MAXMETDT, OOPMETDT, LIFEMAX, FERYTDMAX, FERLTMAX, OCYTD, OCLIFE,
                                         ICYTD, ICLIFE, JOURNAL, SYSID, TIER, NPDEDACC, NPOOPACC, NPMAXACC, QTR4DEDACC, QTR4OOPACC, QTR4MAXACC,
                                         MEDDEDACC, MEDOOPACC, MEDMAXACC, DIAPHRDT, NPMEDMAX, NPMEDOOP, NPMEDDED, LASTCLAIM, OTHERID, GHYTDMAX,
                                         GHLTMAX, CPYSUBS, DEDSUBS, TROOP, TOTPRC, GAP_TROOP, ENRADJ, CLASS, BYATROOP, PARTBDED, PARTBOOP, PARTBMAX,
                                         BDEDMETDT, BOOPMETDT, BMAXMETDT, TROOPIC, TOTPRCIC, CPYSUBSIC, GAPCPYSUB, GAPTOTPRC, TEQFEE, SPECDED,
                                         SPECOOP, SPECDOLLAR, USERNAME);
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
        public string SaveHistoryAccumulator(string adsConnectionString, string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
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
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            return adsHelper.SaveHistoryAccumulator(PLNID, ENRID, SUBID, ENRAMT, YTDRX, YTDDOLLAR, PLANTYPE, EFFDT, TRMDT, BROKERYTD, SMOKINGYTD, SMOKINGLT,
                                         COPAY, PRODSEL, DEDUCT, DEDMETDT, EXCEEDMAX, MAXMETDT, OOPMETDT, LIFEMAX, FERYTDMAX, FERLTMAX, OCYTD, OCLIFE,
                                         ICYTD, ICLIFE, JOURNAL, SYSID, TIER, NPDEDACC, NPOOPACC, NPMAXACC, QTR4DEDACC, QTR4OOPACC, QTR4MAXACC,
                                         MEDDEDACC, MEDOOPACC, MEDMAXACC, DIAPHRDT, NPMEDMAX, NPMEDOOP, NPMEDDED, LASTCLAIM, OTHERID, GHYTDMAX,
                                         GHLTMAX, CPYSUBS, DEDSUBS, TROOP, TOTPRC, GAP_TROOP, ENRADJ, CLASS, BYATROOP, PARTBDED, PARTBOOP, PARTBMAX,
                                         BDEDMETDT, BOOPMETDT, BMAXMETDT, TROOPIC, TOTPRCIC, CPYSUBSIC, GAPCPYSUB, GAPTOTPRC, TEQFEE, SPECDED,
                                         SPECOOP, SPECDOLLAR, USERNAME);
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
        public string SaveMember(string adsConnectionString, string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                               string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                               string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                               string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                               string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                               string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                               string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                               string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                               int? MMEDAYMAX, string ALLOWGOVT)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            return adsHelper.SaveMember(ENRID, PLNID, SUBID, CARDID, PERSON, EFFDT, TRMDT, FLEX1, FLEX2, RELCD, ELGOVER, FNAME, MNAME, LNAME, ADDR, ADDR2,
                                 CITY, STATE, ZIP, ZIP4, DOB, SEX, ELGCD, EMPCD, CRDDT, SYSID, LSTDTCARD, NOUPDATE, NDCUPDATE, LASTUPDT, MBRSINCE,
                                 PHYID, OLDPERSON, FLEX3, OTHERID, DEPCODE, MAINT, ACCUM, PATSTAT, ENRCOPAYM, ENRCOPAYR, PHYSREQ, USEELM, ACCMETH,
                                 CARDID2, COB, JOURNAL, ADDEDBY, PMGID, PHONE, MEDICARE, PPNREQENR, PPNID, HICN, RXBIN, RXPCN, RXGROUP, RXID, TRELIG,
                                 PHYQUAL, MMEDAYMAX, ALLOWGOVT);
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
        public string SaveSuspension(string adsConnectionString, string TABLENAME, string LINKKEY, DateTime? CUTOFFDATE, string CUTOFFMETH, string JOURNAL, string SYSID)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            return adsHelper.SaveSuspension(TABLENAME, LINKKEY, CUTOFFDATE, CUTOFFMETH, JOURNAL, SYSID);
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
        public string SaveAccumulator(string adsConnectionString, string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
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
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            return adsHelper.SaveAccumulator(PLNID, ENRID, SUBID, ENRAMT, YTDRX, YTDDOLLAR, PLANTYPE, EFFDT, TRMDT, BROKERYTD, SMOKINGYTD, SMOKINGLT,
                                         COPAY, PRODSEL, DEDUCT, DEDMETDT, EXCEEDMAX, MAXMETDT, OOPMETDT, LIFEMAX, FERYTDMAX, FERLTMAX, OCYTD, OCLIFE,
                                         ICYTD, ICLIFE, JOURNAL, SYSID, TIER, NPDEDACC, NPOOPACC, NPMAXACC, QTR4DEDACC, QTR4OOPACC, QTR4MAXACC,
                                         MEDDEDACC, MEDOOPACC, MEDMAXACC, DIAPHRDT, NPMEDMAX, NPMEDOOP, NPMEDDED, LASTCLAIM, OTHERID, GHYTDMAX,
                                         GHLTMAX, CPYSUBS, DEDSUBS, TROOP, TOTPRC, GAP_TROOP, ENRADJ, CLASS, BYATROOP, PARTBDED, PARTBOOP, PARTBMAX,
                                         BDEDMETDT, BOOPMETDT, BMAXMETDT, TROOPIC, TOTPRCIC, CPYSUBSIC, GAPCPYSUB, GAPTOTPRC, TEQFEE, SPECDED,
                                         SPECOOP, SPECDOLLAR);
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
        public string SaveHistoryMember(string adsConnectionString, string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                               string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                               string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                               string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                               string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                               string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                               string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                               string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                               int? MMEDAYMAX, string ALLOWGOVT, string USERNAME)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            return adsHelper.SaveHistoryMember(ENRID, PLNID, SUBID, CARDID, PERSON, EFFDT, TRMDT, FLEX1, FLEX2, RELCD, ELGOVER, FNAME, MNAME, LNAME, ADDR, ADDR2,
                                 CITY, STATE, ZIP, ZIP4, DOB, SEX, ELGCD, EMPCD, CRDDT, SYSID, LSTDTCARD, NOUPDATE, NDCUPDATE, LASTUPDT, MBRSINCE,
                                 PHYID, OLDPERSON, FLEX3, OTHERID, DEPCODE, MAINT, ACCUM, PATSTAT, ENRCOPAYM, ENRCOPAYR, PHYSREQ, USEELM, ACCMETH,
                                 CARDID2, COB, JOURNAL, ADDEDBY, PMGID, PHONE, MEDICARE, PPNREQENR, PPNID, HICN, RXBIN, RXPCN, RXGROUP, RXID, TRELIG,
                                 PHYQUAL, MMEDAYMAX, ALLOWGOVT, USERNAME);
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
        public void AddLogDetail(string adsConnectionString, string logSysid, string logTable, string fieldName, string oldValue, string newValue)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            adsHelper.InsertLogDetail(logSysid, logTable, fieldName, oldValue, newValue);
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
            var sqlHelper = new EligibilityDataAccess(DataHelper);
            return sqlHelper.SaveImport(importID, transactionTypeID, clientID, importStatusID, preImportID, planID, patName, rawData, createdTime, completedTime,
                                 returnValue, warningMessage, errorMessage, recordID, recordAction, insertAppUserID, updateAppUserID);
        }

        /// <summary>
        ///  Verifies whether a plan with the given plan ID exists.
        /// </summary>
        /// <param name="planID">String representing the identifier of the member plan</param>
        /// <returns><see cref="bool" /> representing whether a plan with the given plan ID exists</returns>
        public bool PlanExists(string adsConnectionString, string planID)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            bool output = adsHelper.PlanExists(planID);
            return output;
        }

        /// <summary>
        ///  Verifies whether a plan with the given plan ID is active on the given adjustment date.
        /// </summary>
        /// <param name="planID">String representing the identifier of the member plan</param>
        /// <param name="adjustmentDate">DateTime representing the date the adjustment is being made</param>
        /// <returns><see cref="bool" /> representing whether a plan with the given plan ID is in date on the given adjustment date</returns>
        public bool PlanInDate(string adsConnectionString, string planID, DateTime adjustmentDate)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            bool output = adsHelper.PlanInDate(planID, adjustmentDate);
            return output;
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
        public long SaveACPImport(long importID, long? transactionTypeID, long clientID, long importStatusID, string rawData, DateTime createdTime,
                                  DateTime completedTime, string returnValue, string warningMessage, string errorMessage, string recIdentifier,
                                  string recordAction, string newBalance, long insertAppUserID, long updateAppUserID)
        {
            var sqlHelper = new EligibilityDataAccess(DataHelper);
            return sqlHelper.SaveACPImport(importID, transactionTypeID, clientID, importStatusID, rawData, createdTime, completedTime, returnValue,
                                           warningMessage, errorMessage, recIdentifier, recordAction, newBalance, insertAppUserID, updateAppUserID);
        }

        /// <summary>
        /// Gets plan fields needed to perform an ACP update.
        /// </summary>
        /// <param name="planID">String representing the identifier of the plan</param>
        /// <returns><see cref="ACPPlanFieldsDTO" /> representing plan fields needed to perform the ACP update</returns>
        public ACPPlanFieldsDTO GetACPPlanFields(string adsConnectionString, string planID)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            ACPPlanFieldsDTO output = adsHelper.GetACPPlanFields(planID);

            return output;
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
        public void ReplaceAccumulator(string adsConnectionString, DateTime adjustmentDate, DateTime effectiveDate, DateTime termDate, string planId,
                                       string enrId, double medicalDeductible, double medicalOutOfPocketAmount, double medicalMaximumBenefitAmount,
                                       string userName)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            adsHelper.ReplaceAccumulator(adjustmentDate, effectiveDate, termDate, planId, enrId, medicalDeductible, medicalOutOfPocketAmount,
                                         medicalMaximumBenefitAmount, userName);
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
        public void UpdateAccumulator(string adsConnectionString, DateTime adjustmentDate, DateTime effectiveDate, DateTime termDate, string planId,
                                      string enrId, double medicalDeductible, double medicalOutOfPocketAmount, double medicalMaximumBenefitAmount,
                                      string userName)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            adsHelper.UpdateAccumulator(adjustmentDate, effectiveDate, termDate, planId, enrId, medicalDeductible, medicalOutOfPocketAmount,
                                        medicalMaximumBenefitAmount, userName);
        }

        /// <summary>
        /// Gets new balance after performing an ACP update.
        /// </summary>
        /// <param name="adjustmentDate">DateTime representing the date of the adjustment</param>
        /// <param name="planId">String representing the identifier of the plan the adjustment was made on</param>
        /// <param name="enrId">String representing the identifier of the enrollee the adjustment was made on</param>
        /// <returns><see cref="string" /> representing the new balance after performing the ACP update</returns>
        public string GetNewBalance(string adsConnectionString, DateTime adjustmentDate, string planID, string enrID)
        {
            var adsHelper = new EligibilityDataAccess(new AdsHelper(adsConnectionString));
            string output = adsHelper.ReadNewBalance(adjustmentDate, planID, enrID);

            return output;
        }
        #endregion
    }
}