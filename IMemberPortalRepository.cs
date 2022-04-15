using System.Collections.Generic;
using System.Threading.Tasks;
using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.DTO.Drug;
using ProCare.API.PBM.Repository.DTO.MemberPortal;


namespace ProCare.API.PBM.Repository
{
    public interface IMemberPortalRepository
    {
        Task<ValidUserLoginDTO> Login(string cardID, string cardID2, string person, string enrolleeID,
                                        string binNumber, int clientID, int domainID);

        Task<AutoLogonDTO> ReadUserSSO(string token);

        Task<PRXUserDTO> GetPRXUserDetailsforUpdatePassword(string connectionString, int domainID, string loginName, string oldPassword);

        Task<PRXUserDTO> GetPRXUserDetails(string connectionString, int domainID, string loginName);
        Task DeleteMemberFavoriteMedication(int userid, long memberMedicationID);
        
        Task<long> SaveMemberFavoriteMedication(string connectionString, int UserId, long? MemberMedicationID, int MemberMedicationTypeID, long? EntityIdentifier, string MedicationName);
        Task<bool> NDCExists(string connectionString, string NDC);
        Task<MemberFavoriteMedicationDTO> GetMemberFavoriteMedicationByMemberMedicationID(string connectionString, long MemberMedicationID,int UserID);
        Task<List<MemberFavoriteMedicationDTO>> GetMemberFavoriteMedicationByUserId(string connectionString, int UserId);
        Task<PRXUserSSODetailsDTO> GetPRXUserSSOAPSENRPLN(string connectionString, string Token);
        Task MemberFavoriteContactDelete(int userID, long memberContactID);
        Task<MemberFavoriteContactDTO> GetMemberFavoriteContactByMemberContactID(int ClientID, long MemberContactID);
        Task<bool> PharmacyByClientIDPHAIDExists(int ClientID, string PHAID);
        Task<List<MemberFavoriteContactDTO>> GetMemberFavoriteContactsByUserID(int ClientID, int UserID);
        Task<long> SaveMemberFavoriteContact(int UserID, int MemberContactTypeID, string EntityIdentifier, string ContactName, string ContactAddress, string ContactPhone, long? MemberContactID);

    }
}
