using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using ProCare.Common.Data.ADS;

namespace ProCare.API.PBM.Repository
{
    public class RetroLICSRepository : BasedbRepository, IRetroLICSRepository
    {
        #region Constructor

        public RetroLICSRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        ///  Checks for an existing pause on claim processing for the enrollee
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="cardId">String representing the first nine characters of the enrollee's CardID</param>
        /// <param name="cardId2">String representing the last nine characters of the enrollee's CardID</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <returns><see cref="string" /> representing the identifier of the current claim processing pause record</returns>
        public string CheckForPausedMemberClaimsProcessing(string adsConnectionString, string cardId, string cardId2, string enrId)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.CheckForExistingClaimsProcessingPause(cardId, cardId2, enrId);
        }

        /// <summary>
        ///  Pauses claim processing for the enrollee
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="cardId">String representing the first nine characters of the enrollee's CardID</param>
        /// <param name="cardId2">String representing the last nine characters of the enrollee's CardID</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <returns><see cref="string" /> representing the identifier of the claim processing pause record</returns>
        public string PauseClaimsProcessing(string adsConnectionString, string cardId, string cardId2, string enrId)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.PauseClaimsProcessing(cardId, cardId2, enrId);
        }

        /// <summary>
        ///  Resumes claim processing for the enrollee
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="sysid">String representing the identifier of the claims processing pause record</param>
        public void ResumeClaimsProcessing(string adsConnectionString, string sysid)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            adsHelper.ResumeClaimsProcessing(sysid);
        }

        /// <summary>
        ///  Get the details of the Retro LICS Reprocessing request for the enrollee
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <returns><see cref="RetroLICSMemberDTO" /> representing the details of the member to be reprocessed</returns>
        public RetroLICSMemberDTO GetRetroLICSRecord(string adsConnectionString, string enrId)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadRetroLICSRecord(enrId);
        }

        /// <summary>
        ///  Get the details of the Retro LICS Reprocessing requests for the batch
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <returns><see cref="List{RetroLICSMemberDTO}" /> representing the list of details for the members to be reprocessed</returns>
        public List<RetroLICSMemberDTO> GetRetroLICSBatch(string adsConnectionString)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadRetroLICSBatch();
        }

        /// <summary>
        ///  Attempt to lock the Retro LICS record for processing
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="sysid">String representing the identifier of the Retro LICS record</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <returns><see cref="bool" /> representing whether the record was able to be locked for processing</returns>
        public bool TryLockRetroLICSRecord(string adsConnectionString, string sysid, string enrId)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.TryLockRetroLICSRecord(sysid, enrId);
        }

        /// <summary>
        ///  Attempt to lock unprocessed Retro LICS records for processing
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <returns><see cref="bool" /> representing whether records were able to be locked for processing</returns>
        public bool TryLockRetroLICSBatch(string adsConnectionString)
        {
            var adsHelper = new RetroLICSDataAccess(new AdsHelper(adsConnectionString));
            return adsHelper.TryLockRetroLICSBatch();
        }

        /// <summary>
        ///  Gets the claims to reprocess for the enrollee
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the earliest fill date to retrieve claims for</param>
        /// <param name="endDate">DateTime representing the latest fill date to retrieve claims for</param>
        /// <returns><see cref="List{RetroLICSClaimDTO}" /> representing the list of claims to reprocess</returns>
        public List<RetroLICSClaimDTO> GetClaimsToReprocess(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadRetroLICSClaimsToReprocess(enrId, startDate, endDate);
        }

        public List<RetroLICSClaimDTO> GetClaimsToReprocess_FromLastCompletionTime(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate, DateTime lastCompletionTime)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadRetroLICSClaimsToReprocess_FromLastCompletionTime(enrId, startDate, endDate, lastCompletionTime);
        }

        /// <summary>
        ///  Gets claims for the enrollee with status 5 for completed run verification
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the earliest fill date to retrieve claims for</param>
        /// <param name="endDate">DateTime representing the latest fill date to retrieve claims for</param>
        /// <returns><see cref="List{RetroLICSClaimDTO}" /> representing the list of claims with status 5</returns>
        public List<RetroLICSClaimDTO> GetClaimsForStatusVerification(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadRetroLICSClaimsForStatusVerification(enrId, startDate, endDate);
        }

        public List<RetroLICSClaimDTO> GetClaimsForStatusVerification_FromLastCompletionTime(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate, DateTime lastCompletionTime)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadRetroLICSClaimsForStatusVerification_FromLastCompletionTime(enrId, startDate, endDate, lastCompletionTime);
        }

        /// <summary>
        ///  Gets the benefit year accumulator values for the enrollee
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the first date to retrieve accumulators for</param>
        /// <returns><see cref="List{RetroLICSAccumulatorDTO}" /> representing the list of accumulator records found for the benefit year</returns>
        public List<RetroLICSAccumulatorDTO> GetAccumulatorValues(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadAccumulatorValues(enrId, startDate, endDate);
        }

        /// <summary>
        ///  Gets the compound info if the product is a compound
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="tableName">String representing the name of the table the original claim was loaded from</param>
        /// <param name="ndcref">String representing the Procare identifier of the claim</param>
        /// <returns><see cref="List{CompoundInfoDTO}" /> representing the list of compound ingredients found for the product</returns>
        public List<CompoundInfoDTO> GetClaimCompoundInfo(string adsConnectionString, string tableName, string ndcref)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadClaimCompoundInfo(tableName, ndcref);
        }

        /// <summary>
        ///  Force all accumulator values for the enrollee to zero for the benefit year
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the first date to begin zeroing accumulators for</param>
        public void ForceAccumulatorsToZero(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            adsHelper.ZeroAccumulatorValues(enrId, startDate, endDate);
        }

        /// <summary>
        ///  Verifies that the benefit year accumulator values for the enrollee are zero
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the first date to retrieve accumulators for</param>
        /// <returns><see cref="bool" /> representing whether all non-adjustment accumulators found for the benefit year are zero</returns>
        public bool VerifyAccumulatorsAreZero(string adsConnectionString, string enrId, DateTime startDate, DateTime endDate)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.VerifyAccumulatorsAreZero(enrId, startDate, endDate);
        }

        /// <summary>
        ///  Syncs an enrollee's accumulator adjustments for a given month prior to resubmitting a reprocessed claim
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the month to sync accumulator adjustments for</param>
        public void SyncAccumulatorAdjustments(string adsConnectionString, string enrId, DateTime startDate)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            adsHelper.SyncAccumulatorAdjustments(enrId, startDate);
        }

        /// <summary>
        ///  Syncs an enrollee's accumulator adjustments for a given month prior to resubmitting a reprocessed claim
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the month to sync accumulator adjustments for</param>
        /// <returns><see cref="bool" /> representing whether accumulator adjustments exist for the enrollee for the given month</returns>
        public bool MonthHasAccumUpdates(string adsConnectionString, string enrId, DateTime startDate)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.MonthHasAccumUpdates(enrId, startDate);
        }

        /// <summary>
        ///  Marks the record requesting Retro LICS Reprocessing for an enrollee as completed
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="sysid">String representing the identifier of the Retro LICS record</param>
        /// <param name="newPlnId">String representing the identifier of the most recent plan for the enrollee</param>
        /// <param name="effDt">DateTime representing the first date reprocessed</param>
        /// <param name="trmDt">Optional DateTime representing the term date of the most recent plan for the enrollee</param>
        /// <param name="errorMessage">String representing the error that caused processing to complete unsuccessfully</param>
        /// <param name="errorType">String representing the type of processing failure that occurred (R for Reject, E for Error)</param>
        public void MarkRetroLICSMemberAsProcessed(string adsConnectionString, string sysid, string newPlnId, DateTime? effDt, DateTime? trmDt, string errorMessage, string errorType)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            adsHelper.SetRetroLICSRecordProcessed(sysid, newPlnId, effDt, trmDt, errorMessage, errorType);
        }

        public DateTime? GetEnrolleeLastFullReprocCompletionTime(string adsConnectionString, string enrId, int benefitYear)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.ReadEnrolleeLastFullReprocCompletionTime(enrId, benefitYear);
        }
        public void UpdateEnrolleeLastFullReprocCompletionTime(string adsConnectionString, string enrId, int benefitYear, DateTime lastCompletionDate)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            adsHelper.SetEnrolleeLastFullReprocCompletionTime(enrId, benefitYear, lastCompletionDate);
        }

        /// <summary>
        ///  Checks whether an accumulator adjustment was made after the last claims in the previous month
        /// </summary>
        /// <param name="adsConnectionString">String representing the connection string to use if overriding the default</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="rxno">String representing the Rx Number of the claim</param>
        /// <param name="fillDate">DateTime representing the date the claim was originally filled</param>
        /// <returns><see cref="bool" /> representing whether an accumulator adjustment was made after the last submission of the given claim</returns>
        public bool LastActionWasAccumAdjustment(string adsConnectionString, string enrId, string rxno, DateTime? fillDate, int accumsSinceZeroOut)
        {
            var adsHelper = new RetroLICSDataAccess(getHelper(adsConnectionString));
            return adsHelper.LastActionWasAccumAdjustment(enrId, rxno, fillDate, accumsSinceZeroOut);
        }

        //Gets an IDataAccessHelper with the overridden connection string
        //Or returns the default helper if no connection string is provided
        private IDataAccessHelper getHelper(string adsConnectionString)
        {
            return MultipleConnectionsHelper.GetAdsHelper(adsConnectionString, DataHelper);
        }
        #endregion Public Methods
    }
}
