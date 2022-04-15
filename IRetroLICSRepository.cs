using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;

namespace ProCare.API.PBM.Repository
{
    public interface IRetroLICSRepository
    {
        string CheckForPausedMemberClaimsProcessing(string adsConnectionString, string cardId, string cardId2, string enrId);
        string PauseClaimsProcessing(string adsConnectionString, string cardId, string cardId2, string enrId);
        void ResumeClaimsProcessing(string adsConnectionString, string sysid);
        RetroLICSMemberDTO GetRetroLICSRecord(string adsConnectionString, string enrId);
        List<RetroLICSMemberDTO> GetRetroLICSBatch(string adsConnectionString);
        bool TryLockRetroLICSRecord(string adsConnectionString, string sysid, string enrId);
        bool TryLockRetroLICSBatch(string adsConnectionString);
        List<RetroLICSClaimDTO> GetClaimsToReprocess(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate);
        List<RetroLICSClaimDTO> GetClaimsToReprocess_FromLastCompletionTime(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate, DateTime lastCompletionTime);
        List<RetroLICSClaimDTO> GetClaimsForStatusVerification(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate);
        List<RetroLICSClaimDTO> GetClaimsForStatusVerification_FromLastCompletionTime(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate, DateTime lastCompletionTime);
        void SyncAccumulatorAdjustments(string adsConnectionString, string enrId, DateTime startDate);
        bool MonthHasAccumUpdates(string adsConnectionString, string enrId, DateTime startDate);
        void ForceAccumulatorsToZero(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate);
        bool VerifyAccumulatorsAreZero(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate);
        List<RetroLICSAccumulatorDTO> GetAccumulatorValues(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate);
        List<CompoundInfoDTO> GetClaimCompoundInfo(string adsConnectionString, string tableName, string ndcref);
        void MarkRetroLICSMemberAsProcessed(string adsConnectionString, string sysid, string newPlnId, DateTime? effDt, DateTime? trmDt, string errorMessage, string errorType);
        bool LastActionWasAccumAdjustment(string adsConnectionString, string enrId, string rxno, DateTime? fillDate, int accumsSinceZeroOut);
        DateTime? GetEnrolleeLastFullReprocCompletionTime(string adsConnectionString, string enrId, int benefitYear);
        void UpdateEnrolleeLastFullReprocCompletionTime(string adsConnectionString, string enrId, int benefitYear, DateTime lastCompletionDate);
    }
}
