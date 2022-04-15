using System.Collections.Generic;
using System.Threading.Tasks;

using ProCare.API.PBM.Repository.DTO;
namespace ProCare.API.PBM.Repository
{
    public interface IVerificationQueueRepository
    {
        Task<List<EpisodeDTO>> GetUnverifiedEpisodeListFromAPSDLY(string connectionString);

        Task<List<EpisodeDTO>> GetUnverifiedEpisodeListFromClaimHist(string connectionString);

        Task<List<EpisodeDTO>> GetUnverifiedEpisodeListFromAPSSDB(string connectionString);

        
        void InsertNewEpisodes(string connectionString, List<EpisodeDTO> episodes);

        Task<EpisodeDetailsDTO> GetEpisodeDetailsFromAPSDLY(string connectionString, long episodeID);


        SelfAssignEpisodeDTO SelfAssignEpisode(string connectionString, long episodeID, int autoAssignmentMode, int assignedToAppUserID);

        void ReAssignEpisode(string connectionString, int episodeID, int? assignedToAppUserID, int appUserId);

        List<string> GetEpisodePlanIds(string connectionString, int appUserId, string planIdList);
        WorkersCompDetailDTO GetWorkersCompDetails(string connectionString, string enrID, string plnID);

        List<NoteDTO> GetNoteList(string connectionString, string p_File, string p_Key, string p_Type);

        List<EpisodeClaimsHistoryDTO> GetEpisodeClaims(string connectionString, string enrid, string planID);

        MemberDetailVQDTO GetMemberDetailByENRIDAndPlanID(string connectionString, string EnrID, string PlnID);

        void InsertNote(string connectionString, string p_File, string p_Key, string Type, string Description, string CreatedBy, string Note);

        bool AppUserExists(string connectionString, string CreatedBy);

        bool APSENRExists(string connectionString, string enrID, string planID);

        void DetectAndCreateEpisodes(string connectionString, int batchSize, int staleQueueItemDurationInSeconds, string planIDs);
        bool EpisodeExists(string connectionString, long episodeID);
        Task<EpisodeDetailsDTO> GetEpisodeDetailsFromClaimHist(string connectionString, long episodeID);
        Task<EpisodeDetailsDTO> GetEpisodeDetailsFromAPSSDB(string connectionString, long episodeID);
        List<ErrorCodeDTO> ValidateMemberPlanID(string enrID, string planID, string newPlanID, int appUserID);
        void UpdateMemberPlanID(string enrID, string planID, string newPlanID, string changeBy, int appUserID);

        List<ErrorCodeDTO> ValidateCardID(string connectionString, string cardID, string cardID2, string planID, string subID);
        
        void UpdateCardHolderID(string connectionString, string cardID, string cardID2, string planID, string subID, string changedBy, int appUserID);
        string GetUserNameByAppUserID(int appUserID);
        void UpdateMemberDetailVQ(MemberDetailVQUpdateDTO request, string userName);
        bool ENRWCExists(string enrID, string planID);
        void UpdateWorkersCompDetailVQ(WorkersCompDetailVQUpdateDTO request, string userName);

        Task CalculatePlanAccess(int appUserId, bool forceRebuild);
        Task CalculateGroupAccess(int appUserId, bool forceRebuild);
        Task CalculateOrgAccess(int appUserId, bool forceRebuild);
        Task CalculateParentAccess(int appUserId, bool forceRebuild);
        List<UserPlanDTO> GetUserPlanWithPlanName(int appUserId, string planName);
        List<UserPlanDTO> GetUserPlanWithGroupName(int appUserId, string groupName);
        List<UserPlanDTO> GetUserPlanWithPlanAddress(int appUserId, string planAddress);
        List<UserPlanDTO> GetUserPlanWithPlanState(int appUserId, string planState);

        bool AppUserIDExists(string connectionString, int appUserID);

        List<ChangeLogDetailDTO> GetChangeLogDetail(string eNRID);

        void UpdateEpisodeStatus(long episodeID, int episodeStatusID, string changedBy, string userName);

        long? GetNextEpisodeForUser(int assignedToAppUserID, long currentEpisodeID);
        EpisodeDetailsDTO GetEpisodeDetailsFromEpisode(long episodeID);

        List<long> ValidateEpisodeIDs(string episodeIDsList);

        void UpdateCarrierNote(string episodeIDsList, string carrierNote, string userName);

    }
}
