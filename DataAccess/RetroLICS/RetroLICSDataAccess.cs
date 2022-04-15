using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class RetroLICSDataAccess : DataAccessBase
    {
        #region Constructors

        public RetroLICSDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        ///  Checks for an existing pause on claim processing for the enrollee
        /// </summary>
        /// <param name="cardId">String representing the first nine characters of the enrollee's CardID</param>
        /// <param name="cardId2">String representing the last nine characters of the enrollee's CardID</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <returns><see cref="string" /> representing the identifier of the current claim processing pause record</returns>
        public string CheckForExistingClaimsProcessingPause(string cardId, string cardId2, string enrId)
        {
            string sysid = "";

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@CARDID",  cardId},
                    {"@CARDID2",  cardId2},
                    {"@ENRID",  enrId}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_checkForExistingPause", CommandType.StoredProcedure, parameters, reader =>
                {
                    sysid = reader.GetStringorDefault("SYSID");
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return sysid;
        }

        /// <summary>
        ///  Pauses claim processing for the enrollee
        /// </summary>
        /// <param name="cardId">String representing the first nine characters of the enrollee's CardID</param>
        /// <param name="cardId2">String representing the last nine characters of the enrollee's CardID</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <returns><see cref="string" /> representing the identifier of the claim processing pause record</returns>
        public string PauseClaimsProcessing(string cardId, string cardId2, string enrId)
        {
            string sysid = "";

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@CARDID",  cardId},
                    {"@CARDID2",  cardId2},
                    {"@ENRID",  enrId},
                    {"@USERNAME",  "PBMAPI"}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_pause", CommandType.StoredProcedure, parameters, reader =>
                {
                    sysid = reader.GetStringorDefault("SYSID");
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return sysid;
        }

        /// <summary>
        ///  Resumes claim processing for the enrollee
        /// </summary>
        /// <param name="sysid">String representing the identifier of the claims processing pause record</param>
        public void ResumeClaimsProcessing(string sysid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@SYSID",  sysid}
                };

                DataHelper.ExecuteNonQuery("apiPBM_MemberClaimsProcessing_resume_bySYSID", CommandType.StoredProcedure, parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///  Get the details of the Retro LICS Reprocessing request for the enrollee
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <returns><see cref="RetroLICSMemberDTO" /> representing the details of the member to be reprocessed</returns>
        public RetroLICSMemberDTO ReadRetroLICSRecord(string enrId)
        {
            RetroLICSMemberDTO dto = new RetroLICSMemberDTO();

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID", enrId}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_getRetroLICSRecord", CommandType.StoredProcedure, parameters,
                                         reader => { dto.LoadFromDataReader(reader); });
            }
            catch (Exception e)
            {
                throw e;
            }

            return dto;
        }

        /// <summary>
        ///  Get the details of the Retro LICS Reprocessing requests for the batch
        /// </summary>
        /// <returns><see cref="List{RetroLICSMemberDTO}" /> representing the list of details for the members to be reprocessed</returns>
        public List<RetroLICSMemberDTO> ReadRetroLICSBatch()
        {
            List<RetroLICSMemberDTO> output = new List<RetroLICSMemberDTO>();

            try
            {
                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_getRetroLICSBatch", CommandType.StoredProcedure, null, reader =>
                {
                    RetroLICSMemberDTO dto = new RetroLICSMemberDTO();
                    dto.LoadFromDataReader(reader);
                    output.Add(dto);
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return output;
        }

        /// <summary>
        ///  Attempt to lock the Retro LICS record for processing
        /// </summary>
        /// <param name="sysid">String representing the identifier of the Retro LICS record</param>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <returns><see cref="bool" /> representing the whether the record was able to be locked for processing</returns>
        public bool TryLockRetroLICSRecord(string sysid, string enrId)
        {
            bool result = false;

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@SYSID",  sysid},
                    {"@ENRID",  enrId}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_tryLockRecordForProcessing_bySYSID", CommandType.StoredProcedure, parameters, reader =>
                {
                    result = reader.GetBooleanSafe("WasAbleToLockRecord");
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        ///  Attempt to lock unprocessed Retro LICS records for processing
        /// </summary>
        /// <returns><see cref="bool" /> representing whether records were able to be locked for processing</returns>
        public bool TryLockRetroLICSBatch()
        {
            bool result = false;

            try
            {
                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_tryLockBatchForProcessing", CommandType.StoredProcedure, null, reader =>
                {
                    result = reader.GetBooleanSafe("WasAbleToLockBatch");
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        ///  Gets the claims to reprocess for the enrollee
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the earliest fill date to retrieve claims for</param>
        /// <param name="endDate">DateTime representing the latest fill date to retrieve claims for</param>
        /// <returns><see cref="List{RetroLICSClaimDTO}" /> representing the list of claims to reprocess</returns>
        public List<RetroLICSClaimDTO> ReadRetroLICSClaimsToReprocess(string enrId, DateTime startDate, DateTime endDate)
        {
            List<RetroLICSClaimDTO> output = new List<RetroLICSClaimDTO>();

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE", endDate}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_getClaimsToReprocess", CommandType.StoredProcedure, parameters, reader =>
                {
                    RetroLICSClaimDTO dto = new RetroLICSClaimDTO();
                    dto.LoadFromDataReader(reader);
                    output.Add(dto);
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return output;
        }

        public List<RetroLICSClaimDTO> ReadRetroLICSClaimsToReprocess_FromLastCompletionTime(string enrId, DateTime startDate, DateTime endDate, DateTime lastCompletionTime)
        {
            List<RetroLICSClaimDTO> output = new List<RetroLICSClaimDTO>();

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE", endDate},
                    {"@LastCompletionTime", lastCompletionTime.Date}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_getClaimsToReprocess_fromLastCompletionTime", CommandType.StoredProcedure, parameters, reader =>
                {
                    RetroLICSClaimDTO dto = new RetroLICSClaimDTO();
                    dto.LoadFromDataReader(reader);
                    output.Add(dto);
                });

                if(output.Any())
                {
                    //This accounts for the time part of the timestamp, which is a headache to do in ADS
                    output = output.Where(x => x.ProcessingTime > lastCompletionTime).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return output;
        }

        /// <summary>
        ///  Gets claims for the enrollee with status 5 for completed run verification
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the earliest fill date to retrieve claims for</param>
        /// <param name="endDate">DateTime representing the latest fill date to retrieve claims for</param>
        /// <returns><see cref="List{RetroLICSClaimDTO}" /> representing the list of claims with status 5</returns>
        public List<RetroLICSClaimDTO> ReadRetroLICSClaimsForStatusVerification(string enrId, DateTime startDate, DateTime endDate)
        {
            List<RetroLICSClaimDTO> output = new List<RetroLICSClaimDTO>();

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE", endDate}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_getClaimsByStatus", CommandType.StoredProcedure, parameters, reader =>
                {
                    RetroLICSClaimDTO dto = new RetroLICSClaimDTO();
                    dto.LoadFromDataReaderForStatusVerification(reader);
                    output.Add(dto);
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return output;
        }

        public List<RetroLICSClaimDTO> ReadRetroLICSClaimsForStatusVerification_FromLastCompletionTime(string enrId, DateTime startDate, DateTime endDate, DateTime lastCompletionTime)
        {
            List<RetroLICSClaimDTO> output = new List<RetroLICSClaimDTO>();

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE", endDate},
                    {"@LastCompletionTime", lastCompletionTime.Date}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_getClaimsByStatus_fromLastCompletionTime", CommandType.StoredProcedure, parameters, reader =>
                {
                    RetroLICSClaimDTO dto = new RetroLICSClaimDTO();
                    dto.LoadFromDataReaderForStatusVerification(reader);
                    output.Add(dto);
                });

                if (output.Any())
                {
                    //This accounts for the time part of the timestamp, which is a headache to do in ADS
                    output = output.Where(x => x.ProcessingTime > lastCompletionTime).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return output;
        }

        /// <summary>
        ///  Gets the benefit year accumulator values for the enrollee
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the first date to retrieve accumulators for</param>
        /// <returns><see cref="List{RetroLICSAccumulatorDTO}" /> representing the list of accumulator records found for the benefit year</returns>
        public List<RetroLICSAccumulatorDTO> ReadAccumulatorValues(string enrId, DateTime startDate, DateTime endDate)
        {
            List<RetroLICSAccumulatorDTO> output = new List<RetroLICSAccumulatorDTO>();

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE",  endDate}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_getAccumulators", CommandType.StoredProcedure, parameters, reader =>
                {
                    RetroLICSAccumulatorDTO dto = new RetroLICSAccumulatorDTO();
                    dto.LoadFromDataReader(reader);
                    output.Add(dto);
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return output;
        }

        /// <summary>
        ///  Gets the compound info if the product is a compound
        /// </summary>
        /// <param name="tableName">String representing the name of the table the original claim was loaded from</param>
        /// <param name="ndcref">String representing the Procare identifier of the claim</param>
        /// <returns><see cref="List{CompoundInfoDTO}" /> representing the list of compound ingredients found for the product</returns>
        public List<CompoundInfoDTO> ReadClaimCompoundInfo(string tableName, string ndcref)
        {
            List<CompoundInfoDTO> output = new List<CompoundInfoDTO>();
            string sprocName = $"apiPBM_MemberClaimsProcessing_getClaimCompoundDetails_{tableName}";

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@NDCREF",  ndcref}
                };

                

                DataHelper.ExecuteReader(sprocName, CommandType.StoredProcedure, parameters, reader =>
                {
                    CompoundInfoDTO dto = new CompoundInfoDTO();
                    dto.LoadFromDataReader(reader);
                    output.Add(dto);
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return output;
        }

        /// <summary>
        ///  Force all accumulator values for the enrollee to zero for the benefit year
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the first date to begin zeroing accumulators for</param>
        public void ZeroAccumulatorValues(string enrId, DateTime startDate, DateTime endDate)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE",  endDate}
                };

                DataHelper.ExecuteNonQuery("apiPBM_MemberClaimsProcessing_forceZeroOutAccumulators", CommandType.StoredProcedure, parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///  Verifies that the benefit year accumulator values for the enrollee are zero
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the first date to retrieve accumulators for</param>
        /// <returns><see cref="bool" /> representing whether all non-adjustment accumulators found for the benefit year are zero</returns>
        public bool VerifyAccumulatorsAreZero(string enrId, DateTime startDate, DateTime endDate)
        {
            bool result = false;

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE",  endDate}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_verifyZeroAccumulators", CommandType.StoredProcedure, parameters, reader =>
                {
                    result = reader.GetBooleanSafe("AllAccumsZero");
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        ///  Checks whether an accumulator adjustment was made after the last claims in the previous month
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="rxno">String representing the Rx Number of the claim</param>
        /// <param name="fillDate">DateTime representing the date the claim was originally filled</param>
        /// <returns><see cref="bool" /> representing whether an accumulator adjustment was made after the last submission of the given claim</returns>
        public bool LastActionWasAccumAdjustment(string enrId, string rxno, DateTime? fillDate, int accumsSinceZeroOut)
        {
            bool result = false;

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@RXNO",  rxno},
                    {"@FILLDT",  fillDate},
                    {"@AccumCount", accumsSinceZeroOut }
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_lastActionWasAccumAdjustment", CommandType.StoredProcedure, parameters, reader =>
                {
                    result = reader.GetBooleanSafe("LastActionWasAccumAdjustment");
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        ///  Syncs an enrollee's accumulator adjustments for a given month prior to resubmitting a reprocessed claim
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the month to sync accumulator adjustments for</param>
        public void SyncAccumulatorAdjustments(string enrId, DateTime startDate)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE",  startDate.AddMonths(1).AddDays(-1)}
                };

                DataHelper.ExecuteNonQuery("apiPBM_MemberClaimsProcessing_revertMonthlyAccumulatorAdjustment", CommandType.StoredProcedure, parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///  Syncs an enrollee's accumulator adjustments for a given month prior to resubmitting a reprocessed claim
        /// </summary>
        /// <param name="enrId">String representing the enrollee's ProCare identifier</param>
        /// <param name="startDate">DateTime representing the month to sync accumulator adjustments for</param>
        /// <returns><see cref="bool" /> representing whether accumulator adjustments exist for the enrollee for the given month</returns>
        public bool MonthHasAccumUpdates(string enrId, DateTime startDate)
        {
            bool result = false;

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@STARTDATE",  startDate},
                    {"@ENDDATE",  startDate.AddMonths(1).AddDays(-1)}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_monthHasAccumUpdates", CommandType.StoredProcedure, parameters, reader =>
                {
                    result = reader.GetBooleanSafe("MonthHasAccumUpdates");
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        ///  Marks the record requesting Retro LICS Reprocessing for an enrollee as completed
        /// </summary>
        /// <param name="sysid">String representing the identifier of the Retro LICS record</param>
        /// <param name="newPlnId">String representing the identifier of the most recent plan for the enrollee</param>
        /// <param name="effDt">DateTime representing the first date reprocessed</param>
        /// <param name="trmDt">Optional DateTime representing the term date of the most recent plan for the enrollee</param>
        /// <param name="errorMessage">String representing the error that caused processing to complete unsuccessfully</param>
        /// <param name="errorType">String representing the type of processing failure that occurred (R for Reject, E for Error)</param>
        public void SetRetroLICSRecordProcessed(string sysid, string newPlnId, DateTime? effDt, DateTime? trmDt, string errorMessage, string errorType)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@SYSID",  sysid},
                    {"@NEWPLNID",  newPlnId},
                    {"@EFFDT",  effDt},
                    {"@TRMDT",  trmDt},
                    {"@MESSAGE",  errorMessage},
                    {"@LICSPROC", errorType }
                };

                DataHelper.ExecuteNonQuery("apiPBM_MemberClaimsProcessing_setRetroLICSRecordProcessed_bySYSID", CommandType.StoredProcedure, parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DateTime? ReadEnrolleeLastFullReprocCompletionTime(string enrId, int benefitYear)
        {
            DateTime? result = null;

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@BenefitYear",  benefitYear}
                };

                DataHelper.ExecuteReader("apiPBM_MemberClaimsProcessing_getPreviousReprocCompletionTime", CommandType.StoredProcedure, parameters, reader =>
                {
                    result = reader.GetDateTimeorNull("CompletionTime");
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public void SetEnrolleeLastFullReprocCompletionTime(string enrId, int benefitYear, DateTime lastCompletionTime)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@ENRID",  enrId},
                    {"@BenefitYear",  benefitYear},
                    {"@CompletionTime",  lastCompletionTime}
                };

                DataHelper.ExecuteNonQuery("apiPBM_MemberClaimsProcessing_setReprocCompletionTime", CommandType.StoredProcedure, parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion Public Methods
    }
}
