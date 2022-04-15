using ProCare.API.PBM.Messages.Shared;
using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public interface IPbmRepository
    {
        Task<Tuple<List<MemberSearchDTO>, int>> GetMemberSearchResults(string adsConnectionString, string clientName, Enums.MemberIDType memberIdType,
                                                           string memberId, string memberIdOperator, string lastName, string lastNameOperator,
                                                           string firstName, string firstNameOperator, DateTime? dateOfBirth,
                                                           List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);

        Task<int> GetMemberSearchResultsCount(string adsConnectionString, string clientName, Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                              string lastName, string lastNameOperator, string firstName, string firstNameOperator,
                                              DateTime? dateOfBirth,
                                              List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);

        Task<List<MemberPhysicianLockDetailsResultDTO>> GetMemberPhysicianLockDetails_ByMember(string adsConnectionString, string clientName,
                                                           string planId, string enrolleeId, string person,
                                                           List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);


        Task<MemberPhysicianLockDetailsResultDTO> GetMemberPhysicianLockDetails_BySysid(string adsConnectionString, string clientName,
                                                        string sysid);

        Task<List<MemberDetailsResultDTO>> GetMemberDetails(string adsConnectionString, string clientNme, Enums.MemberIDType memberIdType,
                                                     string organizationId, string groupId, string planId, string memberId, string person);

        Task<List<MemberDetailsMemberDiagnosisDTO>> GetMemberDiagnoses(string adsConnectionString, string clientName, string enrolleeId);

        Task<Tuple<List<ClaimSearchDTO>, int>> GetDailyClaimSearchResults(string adsConnectionString, string clientName,
                                                                    Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                    DateTime fillDateFrom, DateTime fillDateTo, bool lookupAllEnrolleesForMember,
                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);

        Task<Tuple<List<ClaimSearchDTO>, int>> GetPaidClaimSearchResults(string adsConnectionString, string clientName,
                                                                   Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                   DateTime fillDateFrom, DateTime fillDateTo, bool lookupAllEnrolleesForMember,
                                                                   List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);

        Task<int> GetDailyClaimSearchResultsCount(string adsConnectionString, string clientName,
                                                                    Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                    DateTime fillDateFrom, DateTime fillDateTo, bool lookupAllEnrolleesForMember,
                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);

        Task<int> GetPaidClaimSearchResultsCount(string adsConnectionString, string clientName,
                                                                   Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                   DateTime fillDateFrom, DateTime fillDateTo, bool lookupAllEnrolleesForMember,
                                                                   List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);

        Task<PaidClaimDetailsDTO> GetPaidClaimDailyDetails(string adsConnectionString, string clientName, string claimNumber);

        Task<PaidClaimDetailsDTO> GetPaidClaimHistoryDetails(string adsConnectionString, string clientName, string claimNumber);

        Task<RuleTemplateDTO> GetDynamicPARuleTemplate(string adsConnectionString, string clientName, string dynamicPACode2, string productID);

        Task<List<string>> GetMemberEnrolleeId(string adsConnectionString, Enums.MemberIDType memberIdType, string planId, string memberId);

        Task<string> GetMemberEnrolleeIdWithPerson(string adsConnectionString, string planId, string enrolleeId, string person);

        Task<string> RuleExists(string adsConnectionString, string planId, string enrolleeId, string vendType, string codeType, string codes);

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

        Task<bool> MemberExists(string adsConnectionString, string planId, string enrolleeId);

        Task<string> LookupDSGID(string adsConnectionString, int daysSupplyMaximum);

        Task<string> MemberPhysicianLockExists(string adsConnectionString, string planId, string enrolleeId, string npi);

        Task<string> AddMemberPhysicianLock(string adsConnectionString, string planId, string enrolleeId, string npi, string dea, string physicianFirstName, string physicianLastName, DateTime effectiveDate, DateTime? terminationDate, string userId);
        
        Task<FeeScheduleDTO> GetFeeScheduleByNDCREF(string adsConnectionString, string ndcRef);
        
        void TerminateMemberPhysicianLock(string adsConnectionString, string sysId, DateTime? terminationDate, string userId);
        
        void ReinstateMemberPhysicianLock(string adsConnectionString, string sysId, DateTime? effectiveDate, string userId);

        Task<bool> TrySetMemberLockInStatus(string adsConnectionString, string planId, string enrolleeId, string lockInStatus);

        ClientSiteConfigurationDTO GetClientSiteConfiguration(int ClientSiteConfigurationID, string adsConnectionString);

        List<CodedEntityDTO> ReadCodedEntities(int codedEntityTypeID, string connectionString);
    }
}