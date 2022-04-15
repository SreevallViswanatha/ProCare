using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class UserDataAccess : DataAccessBase
    {
        #region Constructors

        public UserDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods

        public List<AppUserInfoDTO> GetAppUserInfo(List<int> userIDs)
        {
            List<AppUserInfoDTO> taskDTOs = new List<AppUserInfoDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserIdList",  string.Join(',', userIDs.Select(x => x))}
            };
            DataHelper.ExecuteReader("apiPBM_Users_readAppUsersInfo", CommandType.StoredProcedure, parameters, reader =>
            {
                AppUserInfoDTO taskDTO = new AppUserInfoDTO();
                taskDTO.LoadFromDataReader(reader);
                taskDTOs.Add(taskDTO);
            });

            return taskDTOs;
        }

        public List<PermissionDTO> GetAppUserPermissions(List<int> userIDs)
        {
            List<PermissionDTO> taskDTOs = new List<PermissionDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserIdList",  string.Join(',', userIDs.Select(x => x))}
            };
            DataHelper.ExecuteReader("apiPBM_Users_PermissionsForAppUserList", CommandType.StoredProcedure, parameters, reader =>
            {
                PermissionDTO taskDTO = new PermissionDTO();
                taskDTO.LoadFromDataReader(reader);
                taskDTOs.Add(taskDTO);
            });

            return taskDTOs;
        }
        public List<UserPermissionDTO> GetUsersWithPermissionsList(string permissionIdList)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PermissionIdList",  permissionIdList}
            };

            List<UserPermissionDTO> userPermissionDTOs = new List<UserPermissionDTO>();

            DataHelper.ExecuteReader("apiPBM_Users_AppUsersWithPermissions", CommandType.StoredProcedure, parameters, reader =>
            {
                UserPermissionDTO dto = new UserPermissionDTO();
                dto.LoadFromDataReader(reader);
                userPermissionDTOs.Add(dto);
            });

            return userPermissionDTOs;
        }
        
        public List<PermissionDTO> GetPermissionsByIds(string permissionIdList)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PermissionIdList",  permissionIdList}
            };

            List<PermissionDTO> userPermissionDTOs = new List<PermissionDTO>();

            DataHelper.ExecuteReader("apiPBM_Permission_readByIDs", CommandType.StoredProcedure, parameters, reader =>
            {
                PermissionDTO dto = new PermissionDTO();
                dto.LoadFromDataReader(reader, true);
                userPermissionDTOs.Add(dto);
            });

            return userPermissionDTOs;
        }

        public List<UserLogoBinaryDTO> GetUserLogoBinaryInfo(int appUserID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppUserID",  appUserID}
            };

            List<UserLogoBinaryDTO> userLogoBinaryDTOs = new List<UserLogoBinaryDTO>();

            DataHelper.ExecuteReader("apiPBM_Users_readUserLogoBinary", CommandType.StoredProcedure, parameters, reader =>
            {
                UserLogoBinaryDTO dto = new UserLogoBinaryDTO();
                dto.LoadFromDataReader(reader);
                userLogoBinaryDTOs.Add(dto);
            });

            return userLogoBinaryDTOs;
        }

        public List<PlanUserDTO> GetUsersWithPlansList(string planIdList)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PlanIdList",  planIdList}
            };

            List<PlanUserDTO> planUserDTOs = new List<PlanUserDTO>();

            DataHelper.ExecuteReader("apiPBM_Users_AppUsersWithPlanIds", CommandType.StoredProcedure, parameters, reader =>
            {
                PlanUserDTO dto = new PlanUserDTO();
                dto.LoadFromDataReader(reader);
                planUserDTOs.Add(dto);
            });

            return planUserDTOs;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
