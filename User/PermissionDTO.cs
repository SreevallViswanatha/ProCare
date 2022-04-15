using ProCare.Common.Data;

using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PermissionDTO
    {
        public int AppUserID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int PermissionID { get; set; }
        public string PermissionName { get; set; }
        public int GrantLevel { get; set; }
        public string AssignmentSource { get; set; }

        public void LoadFromDataReader(IDataReader reader, bool isOnlyPermissionDetails = false)
        {
            CategoryID = reader.GetInt32orDefault("PermCategoryID");
            CategoryName = reader.GetStringorDefault("PermCategoryName");
            PermissionID = reader.GetInt32orDefault("PermissionId");
            PermissionName = reader.GetStringorDefault("PermissionName");

            if (!isOnlyPermissionDetails)
            {
                AppUserID = reader.GetInt32orDefault("AppUserId");
                GrantLevel = reader.GetInt32orDefault("GrantLevel");
                AssignmentSource = reader.GetStringorDefault("AssignmentSource");
            }
        }
    }
}
