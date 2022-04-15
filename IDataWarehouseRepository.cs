using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public interface IDataWarehouseRepository
    {
        Task<MemberPortalUserDTO> LookupUser(int clientID, string username, string password);

        Task<List<MemberPortalSSOValidationDTO>> GetMembers(int clientID, string enrolleeID);

        Task<ClientInfoDTO> LookupClientInfo(string binNumber);

        Task<UserInfoDTO> LookupUserInfo(string enrolleeID);

        Task<UserInfoDTO> LookupUserInfo(int userID);

        Task LoginSuccessful(int userID);

        Task<List<MemberPortalEnrolleeDTO>> LookupEnrollee(int clientID, int domainID, string cardID, DateTime dateOfBirth, string gender,
                                                           string parentID, string orgID, string groupID, string classID);
        
        Task<List<MemberPortalEnrolleeDTO>> LookupEnrollee(string enrolleeID);

        Task<int> LookupDomainID(string domainName);

        Task<int> LookupClientID(string domainName);

        Task<bool> BINAllowsOnlineAccess(string binNumber);

        Task<PlanLookupDTO> LookupPlan(int clientID, string planID, string planType);

        Task<GroupLookupDTO> LookupGroup(int clientID, string groupID);

        Task<OrgLookupDTO> LookupOrg(int clientID, string orgID);

        Task<ParentLookupDTO> LookupParent(int clientID, string parentID);

        Task<int> AddMember(string emailAddress, string password, string username, string binNumber, string enrolleeID, string question,
                            string answer, int clientID, int domainID);



        Task<bool> UpdatePassword(int userid, string password);

        Task<bool> EnrolleePreviouslyRegistered(int clientID, string enrolleeID);

        Task<bool> UsernameAvailable(string username);
    }
}
