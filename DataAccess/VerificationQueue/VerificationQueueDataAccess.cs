using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class VerificationQueueDataAccess : DataAccessBase
    {
        #region Constructors

        public VerificationQueueDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods

        public async Task<List<EpisodeDTO>> GetUnverifiedEpisodeListFromAPSDLY()
        {
            Task<List<EpisodeDTO>> t = Task.Run(() =>
            {
                List<EpisodeDTO> taskDTOs = new List<EpisodeDTO>();
                DataHelper.ExecuteReader("apiPBM_Episode_readUnverifiedEpisodesFromAPSDLY", CommandType.StoredProcedure, null, reader =>
                {
                    EpisodeDTO taskDTO = new EpisodeDTO();
                    taskDTO.LoadFromDataReader(reader);
                    taskDTOs.Add(taskDTO);
                });
                return taskDTOs;
            });
            List<EpisodeDTO> results = await t.ConfigureAwait(false);
            return results;
        }

        public async Task<List<EpisodeDTO>> GetUnverifiedEpisodeListFromClaimHist()
        {
            Task<List<EpisodeDTO>> t = Task.Run(() =>
            {
                List<EpisodeDTO> taskDTOs = new List<EpisodeDTO>();
                DataHelper.ExecuteReader("apiPBM_Episode_readUnverifiedEpisodesFromClaimHist", CommandType.StoredProcedure, null, reader =>
                {
                    EpisodeDTO taskDTO = new EpisodeDTO();
                    taskDTO.LoadFromDataReader(reader);
                    taskDTOs.Add(taskDTO);
                });
                return taskDTOs;
            });
            List<EpisodeDTO> results = await t.ConfigureAwait(false);
            return results;
        }

        public async Task<List<EpisodeDTO>> GetUnverifiedEpisodeListFromAPSSDB()
        {
            Task<List<EpisodeDTO>> t = Task.Run(() =>
            {
                List<EpisodeDTO> taskDTOs = new List<EpisodeDTO>();
                DataHelper.ExecuteReader("apiPBM_Episode_readUnverifiedEpisodesFromAPSSDB", CommandType.StoredProcedure, null, reader =>
                {
                    EpisodeDTO taskDTO = new EpisodeDTO();
                    taskDTO.LoadFromDataReader(reader);
                    taskDTOs.Add(taskDTO);
                });
                return taskDTOs;
            });
            List<EpisodeDTO> results = await t.ConfigureAwait(false);
            return results;
        }

        public void InsertNewEpisodes(List<EpisodeDTO> episodes)
        {
            if (episodes?.Count > 0)
            {
                DataTable episodesDT = createEpisodeDataTable(episodes);
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@Episodes",  episodesDT},
                };
                DataHelper.ExecuteReader("apiPBM_Episode_insertNewEpisodes", CommandType.StoredProcedure, parameters);
            }
        }

        public async Task<EpisodeDetailsDTO> GetEpisodeDetailsFromAPSDLY(long episodeID)
        {
            EpisodeDetailsDTO result = new EpisodeDetailsDTO();

            Task<EpisodeDetailsDTO> task = Task.Run(() =>
            {
                EpisodeDetailsDTO episodeDetails = new EpisodeDetailsDTO();

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@EpisodeID",  episodeID}
                };

                DataHelper.ExecuteReader("apiPBM_Episode_readFromAPSDLY", CommandType.StoredProcedure, parameters, reader =>
                {
                    episodeDetails.LoadFromDataReader(reader);
                });

                return episodeDetails;
            });

            result = await task.ConfigureAwait(false);

            return result;
        }

        public SelfAssignEpisodeDTO SelfAssignEpisode(long episodeID, int autoAssignmentMode, int assignedToAppUserID)
        {
            SelfAssignEpisodeDTO selfAssignEpisodeDTO = new SelfAssignEpisodeDTO();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EpisodeID",  episodeID},
                {"@AutoAssignmentMode",  autoAssignmentMode},
                {"@AssignedToAppUserID",  assignedToAppUserID},

            };

            DataHelper.ExecuteReader("apiPBM_Episode_selfAssignEpisode", CommandType.StoredProcedure, parameters, reader =>
            {
                selfAssignEpisodeDTO.LoadFromDataReader(reader);
            });

            return selfAssignEpisodeDTO;
        }

        public void ReAssignEpisode(int episodeID, int? assignedToAppUserID, int appUserId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EpisodeID",  episodeID},
                {"@AssignedToAppUserID",  assignedToAppUserID},
                {"@AppUserID",  appUserId},
            };
            DataHelper.ExecuteReader("apiPBM_Episode_reassignEpisode", CommandType.StoredProcedure, parameters);
        }

        public List<string> GetEpisodePlanIds(int appUserId, string planIdList)
        {
            List<string> planids = new List<string>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserId",  appUserId},
                {"@PlanIdList",  planIdList}
            };

            DataHelper.ExecuteReader("apiPBM_Users_readPlanAccess", CommandType.StoredProcedure, parameters, reader =>
            {
                string planid = reader.GetStringorDefault("PLNID");
                planids.Add(planid);
            });

            return planids;
        }

        public WorkersCompDetailDTO GetWorkersCompDetails(string enrID, string plnID)
        {
            WorkersCompDetailDTO workersCompDetailDTO = new WorkersCompDetailDTO();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID",  enrID},
                {"@PLNID",  plnID}
            };

            DataHelper.ExecuteReader("apiPBM_WorkersComp_readByENRID", CommandType.StoredProcedure, parameters, reader =>
            {
                workersCompDetailDTO.LoadFromDataReader(reader);
            });

            return workersCompDetailDTO;
        }

        public List<NoteDTO> GetNoteList(string p_File, string p_Key, string p_Type)
        {
            List<NoteDTO> taskDTOs = new List<NoteDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@P_File",  p_File},
                {"@P_Key",  p_Key},
                {"@P_Type",  p_Type}
            };

            DataHelper.ExecuteReader("apiPBM_APSNTS_read", CommandType.StoredProcedure, parameters, reader =>
            {
                NoteDTO taskDTO = new NoteDTO();
                taskDTO.LoadFromDataReader(reader);
                taskDTOs.Add(taskDTO);
            });

            return taskDTOs;
        }


        public List<EpisodeClaimsHistoryDTO> GetEpisodeClaims(string enrid, string planID)
        {
            List<EpisodeClaimsHistoryDTO> episodeClaimsHistoryDTOs = new List<EpisodeClaimsHistoryDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID",  enrid},
                {"@PLNID",  planID}
            };

            DataHelper.ExecuteReader("apiPBM_EpisodeClaimHistory_readByENRID", CommandType.StoredProcedure, parameters, reader =>
            {
                EpisodeClaimsHistoryDTO episodeClaimsHistoryDTO = new EpisodeClaimsHistoryDTO();
                episodeClaimsHistoryDTO.LoadFromDataReader(reader);
                episodeClaimsHistoryDTOs.Add(episodeClaimsHistoryDTO);
            });

            return episodeClaimsHistoryDTOs;
        }

        public MemberDetailVQDTO GetMemberDetailByENRIDAndPlanID(string enrId, string plnId)
        {
            MemberDetailVQDTO memberDetailVQDTO = new MemberDetailVQDTO();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID", enrId },
                {"@PLNID", plnId }
            };

            DataHelper.ExecuteReader("apiPBM_Member_readByENRID", CommandType.StoredProcedure, parameters, reader =>
            {
                memberDetailVQDTO.LoadFromDataReader(reader);
            });

            return memberDetailVQDTO;
        }

        public void InsertNote(string p_File, string p_Key, string type, string description, string createdBy, string note)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@P_File",  p_File},
                {"@P_Key",  p_Key},
                {"@P_Type",  type},
                {"@P_Desc",  description},
                {"@Note",  note},
                {"@CreatedBy",  createdBy}
            };
            DataHelper.ExecuteReader("apiPBM_APSNTS_Insert", CommandType.StoredProcedure, parameters);
        }

        public bool AppUserExists(string createdBy)
        {
            bool? exists = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@CreatedBy",  createdBy}
            };
            DataHelper.ExecuteReader("apiPBM_APPUSER_IsExist", CommandType.StoredProcedure, parameters, reader =>
            {
                exists = reader.GetBooleanorNull("IsExist");

            });
            return exists.HasValue && exists.Value;

        }

        public bool APSENRExists(string enrID, string planID)
        {
            bool? exists = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID",  enrID},
                {"@PLNID", planID }
            };
            DataHelper.ExecuteReader("apiPBM_APSENR_exists", CommandType.StoredProcedure, parameters, reader =>
            {
                exists = reader.GetBooleanorNull("IsExist");

            });
            return exists.HasValue && exists.Value;
        }

        public bool ENRWCExists(string enrID, string planID)
        {
            bool? isAPSENR = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID",  enrID},
                {"@PLNID", planID }
            };
            DataHelper.ExecuteReader("apiPBM_ENRWC_exists", CommandType.StoredProcedure, parameters, reader =>
            {
                isAPSENR = reader.GetBooleanorNull("exists");

            });

            return isAPSENR.HasValue && isAPSENR.Value;
        }

        public void DetectAndCreateEpisodes(int batchSize, int staleQueueItemDurationInSeconds, string planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@BatchSize",  batchSize},
                {"@StaleQueueItemDurationInSeconds",  staleQueueItemDurationInSeconds},
                {"@PlanIDFilter",  planIDs}
            };

            DataHelper.ExecuteReader("apiPBM_Episode_detectAndCreateEpisodes", CommandType.StoredProcedure, parameters);
        }

        public bool EpisodeExists(long episodeID)
        {
            bool? output = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EpisodeID",  episodeID}
            };
            DataHelper.ExecuteReader("apiPBM_Episode_exists", CommandType.StoredProcedure, parameters, reader =>
            {
                output = reader.GetBooleanorNull("EpisodeExists");

            });
            return output.HasValue && output.Value;
        }

        public async Task<EpisodeDetailsDTO> GetEpisodeDetailsFromClaimHist(long episodeID)
        {
            EpisodeDetailsDTO result = new EpisodeDetailsDTO();

            Task<EpisodeDetailsDTO> task = Task.Run(() =>
            {
                EpisodeDetailsDTO episodeDetails = new EpisodeDetailsDTO();

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@EpisodeID",  episodeID}
                };

                DataHelper.ExecuteReader("apiPBM_Episode_readFromClaimHist", CommandType.StoredProcedure, parameters, reader =>
                {
                    episodeDetails.LoadFromDataReader(reader);
                });

                return episodeDetails;
            });

            result = await task.ConfigureAwait(false);

            return result;
        }

        public async Task<EpisodeDetailsDTO> GetEpisodeDetailsFromAPSSDB(long episodeID)
        {
            EpisodeDetailsDTO result = new EpisodeDetailsDTO();

            Task<EpisodeDetailsDTO> task = Task.Run(() =>
            {
                EpisodeDetailsDTO episodeDetails = new EpisodeDetailsDTO();

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@EpisodeID",  episodeID}
                };

                DataHelper.ExecuteReader("apiPBM_Episode_readFromAPSSDB", CommandType.StoredProcedure, parameters, reader =>
                {
                    episodeDetails.LoadFromDataReader(reader);
                });

                return episodeDetails;
            });

            result = await task.ConfigureAwait(false);

            return result;
        }

        public List<ErrorCodeDTO> ValidateMemberPlanID(string enrID, string planID, string newPlanID, int appUserID)
        {
            List<ErrorCodeDTO> output = new List<ErrorCodeDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID",  enrID},
                {"@PLNID",  planID},
                {"@NewPlanID",  newPlanID},
                {"@AppUserID",  appUserID}
            };

            DataHelper.ExecuteReader("apiPBM_PlanIDUpdate_validation", CommandType.StoredProcedure, parameters, reader =>
            {
                ErrorCodeDTO errorCodeDTO = new ErrorCodeDTO();
                errorCodeDTO.LoadFromDataReader(reader);
                output.Add(errorCodeDTO);
            });

            return output;
        }

        public void MemberPlanIDUpdate(string enrID, string planID, string newPlanID, string changeBy, int appUserID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID",  enrID},
                {"@PlanID",  planID},
                {"@NewPlanID",  newPlanID},
                {"@ChangedBy",  changeBy},
                {"@AppUserID",  appUserID}
            };

            DataHelper.ExecuteNonQuery("apiPBM_MemberPlanID_update", CommandType.StoredProcedure, parameters);
        }

        public List<ErrorCodeDTO> ValidateCardID(string cardID, string cardID2, string planID, string subID)
        {
            List<ErrorCodeDTO> errors = new List<ErrorCodeDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@SubID",  subID},
                {"@PLNID",  planID},
                {"@CARDID",  cardID},
                {"@CARDID2",  cardID2}
            };
            DataHelper.ExecuteReader("apiPBM_CardID_validate", CommandType.StoredProcedure, parameters, reader =>
            {
                ErrorCodeDTO errorCodeDTO = new ErrorCodeDTO();
                errorCodeDTO.LoadFromDataReader(reader);

                errors.Add(errorCodeDTO);

            });
            return errors;
        }

        public void UpdateCardHolderID(string cardID, string cardID2, string planID, string subID, string changedBy, int appUserID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@SubID",  subID},
                {"@CARDID",  cardID},
                {"@CARDID2",  cardID2},
                {"@CHANGEDBY",  changedBy},
                {"@AppUserID",  appUserID}
            };
            DataHelper.ExecuteReader("apiPBM_CardHolderID_update", CommandType.StoredProcedure, parameters);
        }

        public string GetUserNameByAppUserID(int appUserID)
        {
            string userName = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserID",  appUserID}
            };
            DataHelper.ExecuteReader("apiPBM_APPUSER_Read", CommandType.StoredProcedure, parameters, reader =>
            {
                userName = reader.GetStringorNull("LogonId");

            });

            return userName;
        }

        public void UpdateMemberDetailVQ(MemberDetailVQUpdateDTO request, string userName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID", request.ENRID},
                {"@PLNID", request.PlanID},
                {"@FNAME", request.FirstName},
                {"@MNAME", request.MiddleInitial},
                {"@LNAME", request.LastName},
                {"@ADDR", request.MemberAddress?.Address1},
                {"@ADDR2", request.MemberAddress?.Address2},
                {"@CITY", request.MemberAddress?.City},
                {"@STATE", request.MemberAddress?.State},
                {"@ZIP", request.MemberAddress?.Zip1},
                {"@DOB", request.DOB},
                {"@SEX", Enum.GetName(typeof(GenderID), request.GenderID)},
                {"@PHONE", request.MemberPhoneNumber},
                {"@EFFDT", request.EffectiveDate},
                {"@TRMDT", request.TermDate},
                {"@UserName", userName}
            };
            DataHelper.ExecuteReader("apiPBM_MemberDetailVQ_Update", CommandType.StoredProcedure, parameters);
        }

        public void UpdateWorkersCompDetailVQ(WorkersCompDetailVQUpdateDTO request, string userName)
        {
            DateTime injuryDate = Convert.ToDateTime(request.InjuryDate);

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID", request.ENRID},
                {"@PLNID", request.PlanID},
                {"@EMPNAME", request.EmployerName},
                {"@EMPADDR1", request.EmployerAddress?.Address1},
                {"@EMPADDR2", request.EmployerAddress?.Address2},
                {"@EMPCITY", request.EmployerAddress?.City},
                {"@EMPSTATE", request.EmployerAddress?.State},
                {"@EMPZIP", request.EmployerAddress?.Zip1},
                {"@EMPPHONE", request.EmployerPhone},
                {"@ADJSTNAME", request.AdjusterName},
                {"@ADJSTEMAIL", request.AdjusterEmail},
                {"@ADJSTPHONE", request.AdjusterPhone},
                {"@ADJSTEXTN", request.AdjusterPhoneExt},
                {"@ADJSTFAX", request.AdjusterFax},
                {"@JURIS", request.Jurisdiction},
                {"@CLMSTATUS", request.ClaimStatusCode},
                {"@INJURDT", injuryDate},
                {"@CARRIER_ID", request.CarrierID},
                {"@WCCLAIMID", request.WorkerCompClaimID},
                {"@PIPCLAIM", request.PIPCLAIM},
                {"@UserName", userName}
            };
            DataHelper.ExecuteReader("apiPBM_WorkersCompDetailVQ_update", CommandType.StoredProcedure, parameters);
        }


        public async Task CalculatePlanAccess(int appUserId, bool forceRebuild)
        {
            Task task = Task.Run(() =>
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@AppUserId", appUserId},
                    {"@ForceRebuild", forceRebuild}
                };

                DataHelper.ExecuteReader("apiPBM_CalculatePlanAccess", CommandType.StoredProcedure, parameters);
            });

            await task.ConfigureAwait(false);
        }

        public async Task CalculateGroupAccess(int appUserId, bool forceRebuild)
        {
            Task task = Task.Run(() =>
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@AppUserId", appUserId},
                    {"@ForceRebuild", forceRebuild}
                };

                DataHelper.ExecuteReader("apiPBM_CalculateGroupAccess", CommandType.StoredProcedure, parameters);
            });

            await task.ConfigureAwait(false);
        }

        public async Task CalculateOrgAccess(int appUserId, bool forceRebuild)
        {
            Task task = Task.Run(() =>
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@AppUserId", appUserId},
                    {"@ForceRebuild", forceRebuild}
                };

                DataHelper.ExecuteReader("apiPBM_CalculateOrgAccess", CommandType.StoredProcedure, parameters);
            });

            await task.ConfigureAwait(false);
        }

        public async Task CalculateParentAccess(int appUserId, bool forceRebuild)
        {
            Task task = Task.Run(() =>
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@AppUserId", appUserId},
                    {"@ForceRebuild", forceRebuild}
                };

                DataHelper.ExecuteReader("apiPBM_CalculateParentAccess", CommandType.StoredProcedure, parameters);
            });

            await task.ConfigureAwait(false);
        }

        public List<UserPlanDTO> GetUserPlanWithPlanName(int appUserId, string planName)
        {
            List<UserPlanDTO> userPlans = new List<UserPlanDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserID",  appUserId},
                {"@PlanName",  planName}

            };
            DataHelper.ExecuteReader("apiPBM_APSPLN_searchWithPlanName", CommandType.StoredProcedure, parameters, reader =>
            {
                UserPlanDTO userPlan = new UserPlanDTO();
                userPlan.LoadFromDataReader(reader);
                userPlans.Add(userPlan);
            });
            return userPlans;
        }

        public List<UserPlanDTO> GetUserPlanWithGroupName(int appUserId, string groupName)
        {
            List<UserPlanDTO> userPlans = new List<UserPlanDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserID",  appUserId},
                {"@GroupName",  groupName}
            };
            DataHelper.ExecuteReader("apiPBM_APSPLN_searchWithGroupName", CommandType.StoredProcedure, parameters, reader =>
            {
                UserPlanDTO userPlan = new UserPlanDTO();
                userPlan.LoadFromDataReader(reader);
                userPlans.Add(userPlan);
            });
            return userPlans;
        }

        public List<UserPlanDTO> GetUserPlanWithPlanAddress(int appUserId, string planAddress)
        {
            List<UserPlanDTO> userPlans = new List<UserPlanDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserID",  appUserId},
                {"@PlanAddress",  planAddress}
            };
            DataHelper.ExecuteReader("apiPBM_APSPLN_searchWithPlanAddress", CommandType.StoredProcedure, parameters, reader =>
            {
                UserPlanDTO userPlan = new UserPlanDTO();
                userPlan.LoadFromDataReader(reader);
                userPlans.Add(userPlan);
            });
            return userPlans;
        }

        public List<UserPlanDTO> GetUserPlanWithPlanState(int appUserId, string planState)
        {
            List<UserPlanDTO> userPlans = new List<UserPlanDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserID",  appUserId},
                {"@PlanState",  planState}
            };
            DataHelper.ExecuteReader("apiPBM_APSPLN_searchWithPlanState", CommandType.StoredProcedure, parameters, reader =>
            {
                UserPlanDTO userPlan = new UserPlanDTO();
                userPlan.LoadFromDataReader(reader);
                userPlans.Add(userPlan);
            });
            return userPlans;
        }

        public bool AppUserIDExists(int appUserID)
        {
            bool? output = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserID",  appUserID}
            };
            DataHelper.ExecuteReader("apiPBM_AppUser_IDexists", CommandType.StoredProcedure, parameters, reader =>
            {
                output = reader.GetBooleanorNull("AppUserIDExists");

            });
            return output.HasValue && output.Value;
        }

        public List<ChangeLogDetailDTO> GetChangeLogDetail(string eNRID)
        {
            List<ChangeLogDetailDTO> changeLogDetails = new List<ChangeLogDetailDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID",  eNRID}
            };
            DataHelper.ExecuteReader("apiPBM_VQChangeLog_read", CommandType.StoredProcedure, parameters, reader =>
            {
                ChangeLogDetailDTO changeLogDetail = new ChangeLogDetailDTO();
                changeLogDetail.LoadFromDataReader(reader);
                changeLogDetails.Add(changeLogDetail);
            });
            return changeLogDetails;
        }

        public void UpdateEpisodeStatus(long episodeID, int episodeStatusID, string changedBy, string userName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EpisodeID",  episodeID},
                {"@EpisodeStatusID",  episodeStatusID},
                {"@ChangedBy",  changedBy},
                {"@UserName",  userName}
            };

            DataHelper.ExecuteNonQuery("apiPBM_Episode_updateStatus", CommandType.StoredProcedure, parameters);
        }

        public long? GetNextEpisodeForUser(int assignedToAppUserID, long currentEpisodeID)
        {
            long? episodeid = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AssignedToAppUserID",  assignedToAppUserID},
                {"@CurrentEpisodeID",  currentEpisodeID}
            };
            DataHelper.ExecuteReader("apiPBM_Episode_getNext", CommandType.StoredProcedure, parameters, reader =>
            {
                episodeid = reader.GetInt64orNull("NextEpisodeID");
            });

            return episodeid;
        }

        public EpisodeDetailsDTO GetEpisodeDetailsFromEpisode(long episodeID)
        {
            EpisodeDetailsDTO result = new EpisodeDetailsDTO();


            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EpisodeID",  episodeID}
            };

            DataHelper.ExecuteReader("apiPBM_Episode_read", CommandType.StoredProcedure, parameters, reader =>
            {
                result.LoadFromDataReader(reader);
            });


            return result;
        }
        
        public List<long> ValidateEpisodeIDs(string episodeIDsList)
        {
            List<long> output = new List<long>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EpisodeIDs",  episodeIDsList}
            };
            DataHelper.ExecuteReader("apiPBM_Episode_validateIDList", CommandType.StoredProcedure, parameters, reader =>
            {
                long value = reader.GetInt64orDefault("EpisodeID");
                output.Add(value);

            });
            return output;
        }
       
        public void UpdateCarrierNote(string episodeIDsList, string carrierNote, string userName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EpisodeIDs",  episodeIDsList},
                {"@CarrierNote", carrierNote },
                {"@ChangedBy", "VQ" },
                {"@UserName", userName }
            };
            DataHelper.ExecuteReader("apiPBM_Episode_updateCarrierNoteByList", CommandType.StoredProcedure, parameters);
        }

        #endregion

        #region Private Methods

        private static DataTable createEpisodeDataTable(List<EpisodeDTO> episodes)
        {
            DataTable table = new DataTable();
            table.Columns.Add("EnrID", typeof(string));
            table.Columns.Add("CardholderID", typeof(string));
            table.Columns.Add("CardholderName", typeof(string));
            table.Columns.Add("EmpName", typeof(string));
            table.Columns.Add("PharmacyName", typeof(string));
            table.Columns.Add("PharmacyNetwork", typeof(string));
            table.Columns.Add("AccountManager", typeof(string));
            table.Columns.Add("PlnID", typeof(string));
            table.Columns.Add("NDCREF", typeof(string));
            table.Columns.Add("NDCProcDateTime", typeof(DateTime));

            foreach (EpisodeDTO dto in episodes)
            {
                DataRow dataRow = table.NewRow();
                dataRow[0] = dto.EnrID;
                dataRow[1] = dto.CardholderID;
                dataRow[2] = dto.CardholderName;
                dataRow[3] = dto.EmpName;
                dataRow[4] = dto.PharmacyName;
                dataRow[5] = dto.PharmacyNetwork;
                dataRow[6] = dto.AccountManager;
                dataRow[7] = dto.PlanID;
                dataRow[8] = dto.NDCREF;
                dataRow[9] = dto.NDCProcDateTime;

                table.Rows.Add(dataRow);
            }
            return table;
        }

        #endregion
    }
}
