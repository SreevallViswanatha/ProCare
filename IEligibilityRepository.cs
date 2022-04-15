using ProCare.API.PBM.Repository.DTO.Eligibility;
using System;
using System.Collections.Generic;

namespace ProCare.API.PBM.Repository
{
    public interface IEligibilityRepository
    {
        ClientWithConfigurationsDTO GetClientWithClientConfiguration(long clientID);

        ClientACPDTO GetClientACPConfiguration(long clientID);

        List<MemberDTO> GetMembersByPlanIDCardIDCardID2Person(string adsConnectionString, string planID, string cardID, string cardID2, string person);

        List<BenefitYearAccumulatorDTO> GetBenefitYearAccumulatorByPlanIDEnrolleeID(string adsConnectionString, string planID, string enrolleeID);

        List<SuspensionDTO> GetSuspensionByTableNameLinkKey(string adsConnectionString, string tableName, string linkKey);

        List<MemberDTO> GetHistoryMembersByPlanIDCardIDCardID2Person(string adsConnectionString, string planID, string cardID, string cardID2, string person);

        string LogMember(string adsConnectionString, string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                       string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                       string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                       string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                       string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                       string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                       string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                       string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                       int? MMEDAYMAX, string ALLOWGOVT, string USERNAME);

        void LogSuspension(string adsConnectionString, string TABLENAME, string LINKKEY, DateTime? CUTOFFDATE, string CUTOFFMETH, string JOURNAL, string SYSID,
                          string USERNAME);

        string LogAccumulator(string adsConnectionString, string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
                          string PLANTYPE, DateTime EFFDT, DateTime TRMDT, double BROKERYTD, double SMOKINGYTD, double SMOKINGLT,
                          double COPAY, double PRODSEL, double DEDUCT, DateTime DEDMETDT, double EXCEEDMAX, DateTime MAXMETDT,
                          DateTime OOPMETDT, double LIFEMAX, double FERYTDMAX, double FERLTMAX, double OCYTD, double OCLIFE, double ICYTD,
                          double ICLIFE, string JOURNAL, string SYSID, string TIER, double NPDEDACC, double NPOOPACC, double NPMAXACC,
                          double QTR4DEDACC, double QTR4OOPACC, double QTR4MAXACC, double MEDDEDACC, double MEDOOPACC, double MEDMAXACC,
                          DateTime DIAPHRDT, double NPMEDMAX, double NPMEDOOP, double NPMEDDED, DateTime LASTCLAIM, string OTHERID,
                          double GHYTDMAX, double GHLTMAX, double CPYSUBS, double DEDSUBS, double TROOP, double TOTPRC, double GAP_TROOP,
                          double ENRADJ, string CLASS, double BYATROOP, double PARTBDED, double PARTBOOP, double PARTBMAX, DateTime BDEDMETDT,
                          DateTime BOOPMETDT, DateTime BMAXMETDT, double TROOPIC, double TOTPRCIC, double CPYSUBSIC, double GAPCPYSUB,
                          double GAPTOTPRC, double TEQFEE, double SPECDED, double SPECOOP, double SPECDOLLAR, string USERNAME);

        string SaveHistoryAccumulator(string adsConnectionString, string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
                          string PLANTYPE, DateTime EFFDT, DateTime TRMDT, double BROKERYTD, double SMOKINGYTD, double SMOKINGLT,
                          double COPAY, double PRODSEL, double DEDUCT, DateTime DEDMETDT, double EXCEEDMAX, DateTime MAXMETDT,
                          DateTime OOPMETDT, double LIFEMAX, double FERYTDMAX, double FERLTMAX, double OCYTD, double OCLIFE, double ICYTD,
                          double ICLIFE, string JOURNAL, string SYSID, string TIER, double NPDEDACC, double NPOOPACC, double NPMAXACC,
                          double QTR4DEDACC, double QTR4OOPACC, double QTR4MAXACC, double MEDDEDACC, double MEDOOPACC, double MEDMAXACC,
                          DateTime DIAPHRDT, double NPMEDMAX, double NPMEDOOP, double NPMEDDED, DateTime LASTCLAIM, string OTHERID,
                          double GHYTDMAX, double GHLTMAX, double CPYSUBS, double DEDSUBS, double TROOP, double TOTPRC, double GAP_TROOP,
                          double ENRADJ, string CLASS, double BYATROOP, double PARTBDED, double PARTBOOP, double PARTBMAX, DateTime BDEDMETDT,
                          DateTime BOOPMETDT, DateTime BMAXMETDT, double TROOPIC, double TOTPRCIC, double CPYSUBSIC, double GAPCPYSUB,
                          double GAPTOTPRC, double TEQFEE, double SPECDED, double SPECOOP, double SPECDOLLAR, string USERNAME);

        string SaveMember(string adsConnectionString, string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                       string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                       string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                       string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                       string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                       string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                       string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                       string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                       int? MMEDAYMAX, string ALLOWGOVT);

        string SaveSuspension(string adsConnectionString, string TABLENAME, string LINKKEY, DateTime? CUTOFFDATE, string CUTOFFMETH, string JOURNAL, string SYSID);

        string SaveAccumulator(string adsConnectionString, string PLNID, string ENRID, string SUBID, double ENRAMT, int YTDRX, double YTDDOLLAR,
                          string PLANTYPE, DateTime EFFDT, DateTime TRMDT, double BROKERYTD, double SMOKINGYTD, double SMOKINGLT,
                          double COPAY, double PRODSEL, double DEDUCT, DateTime DEDMETDT, double EXCEEDMAX, DateTime MAXMETDT,
                          DateTime OOPMETDT, double LIFEMAX, double FERYTDMAX, double FERLTMAX, double OCYTD, double OCLIFE, double ICYTD,
                          double ICLIFE, string JOURNAL, string SYSID, string TIER, double NPDEDACC, double NPOOPACC, double NPMAXACC,
                          double QTR4DEDACC, double QTR4OOPACC, double QTR4MAXACC, double MEDDEDACC, double MEDOOPACC, double MEDMAXACC,
                          DateTime DIAPHRDT, double NPMEDMAX, double NPMEDOOP, double NPMEDDED, DateTime LASTCLAIM, string OTHERID,
                          double GHYTDMAX, double GHLTMAX, double CPYSUBS, double DEDSUBS, double TROOP, double TOTPRC, double GAP_TROOP,
                          double ENRADJ, string CLASS, double BYATROOP, double PARTBDED, double PARTBOOP, double PARTBMAX, DateTime BDEDMETDT,
                          DateTime BOOPMETDT, DateTime BMAXMETDT, double TROOPIC, double TOTPRCIC, double CPYSUBSIC, double GAPCPYSUB,
                          double GAPTOTPRC, double TEQFEE, double SPECDED, double SPECOOP, double SPECDOLLAR);

        string SaveHistoryMember(string adsConnectionString, string ENRID, string PLNID, string SUBID, string CARDID, string PERSON, DateTime EFFDT, DateTime? TRMDT, string FLEX1,
                       string FLEX2, string RELCD, string ELGOVER, string FNAME, string MNAME, string LNAME, string ADDR, string ADDR2,
                       string CITY, string STATE, string ZIP, string ZIP4, DateTime? DOB, string SEX, string ELGCD, string EMPCD, DateTime CRDDT,
                       string SYSID, DateTime LSTDTCARD, string NOUPDATE, string NDCUPDATE, DateTime LASTUPDT, DateTime MBRSINCE,
                       string PHYID, string OLDPERSON, string FLEX3, string OTHERID, string DEPCODE, string MAINT, int? ACCUM,
                       string PATSTAT, string ENRCOPAYM, string ENRCOPAYR, string PHYSREQ, string USEELM, string ACCMETH, string CARDID2,
                       string COB, string JOURNAL, string ADDEDBY, string PMGID, string PHONE, string MEDICARE, string PPNREQENR,
                       string PPNID, string HICN, string RXBIN, string RXPCN, string RXGROUP, string RXID, string TRELIG, string PHYQUAL,
                       int? MMEDAYMAX, string ALLOWGOVT, string USERNAME);

        void AddLogDetail(string adsConnectionString, string logSysid, string logTable, string fieldName, string oldValue, string newValue);

        long SaveImport(long importID, long? transactionTypeID, long clientID, long importStatusID, long? preImportID, string planID, string patName,
                       string rawData, DateTime? createdTime, DateTime completedTime, string returnValue, string warningMessage,
                       string errorMessage, string recordID, string recordAction, long insertAppUserID, long updateAppUserID);

        bool PlanExists(string adsConnectionString, string planID);

        bool PlanInDate(string adsConnectionString, string planID, DateTime adjustmentDate);

        long SaveACPImport(long importID, long? transactionTypeID, long clientID, long importStatusID, string rawData, DateTime createdTime,
                           DateTime completedTime, string returnValue, string warningMessage, string errorMessage, string recIdentifier,
                           string recordAction, string newBalance, long insertAppUserID, long updateAppUserID);

        void ReplaceAccumulator(string adsConnectionString, DateTime AdjDt, DateTime ACCUM_EFF, DateTime ACCUM_TRM, string PLNID, string ENRID,
                               double MEDDEDACC, double MEDOOPACC, double MEDMAXACC, string USERNAME);
        void UpdateAccumulator(string adsConnectionString, DateTime AdjDt, DateTime ACCUM_EFF, DateTime ACCUM_TRM, string PLNID, string ENRID,
                                double MEDDEDACC, double MEDOOPACC, double MEDMAXACC, string USERNAME);

        string GetNewBalance(string adsConnectionString, DateTime adjustmentDate, string planID, string enrID);

        ACPPlanFieldsDTO GetACPPlanFields(string adsConnectionString, string planID);
    }
}