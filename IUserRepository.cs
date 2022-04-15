using System.Collections.Generic;
using System.Threading.Tasks;

using ProCare.API.PBM.Repository.DTO;
namespace ProCare.API.PBM.Repository
{
    public interface IUserRepository
    {
        List<AppUserInfoDTO> GetAppUserInfo(string connectionString, List<int> userIDs);
        List<PermissionDTO> GetAppUserPermissions(string connectionString, List<int> userIDs);
        List<UserPermissionDTO> GetUsersWithPermissionsList(string connectionString, string permissionIdList);
        List<PermissionDTO> GetPermissionsByIds(string connectionString, string permissionIdList);
        List<UserLogoBinaryDTO> GetUserLogoBinaryInfo(string connectionString, int AppUserID);
        List<PlanUserDTO> GetUsersWithPlansList(string connectionString, string planIdList);
    }
}
