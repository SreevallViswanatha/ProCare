using ProCare.API.Core.Constants;
using ProCare.Common;
using ServiceStack;
using ServiceStack.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ProCare.API.Common
{
    public class CustomBasicAuthProvider : BasicAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string username, string password)
        {
            bool output = false;

            try
            {
                // Attempt to retrieve the security configurations based on the given username.
                // On successful retrieval of the configurations, check if the given username and password match the credentials in the configuration.
                // If there's a match, return true otherwise return false            
                Dictionary<string, string> setting = getSecurityAccount(username);

                if (setting != null)
                {
                    if (setting.ContainsKey(ConfigSetttingKey.BasicAuth_Username) && setting.ContainsKey(ConfigSetttingKey.BasicAuth_Password))
                    {
                        output = setting[ConfigSetttingKey.BasicAuth_Username].ToLower() == username.ToLower() && setting[ConfigSetttingKey.BasicAuth_Password] == password;
                    }
                    else
                    {
                        throw new Exception("Security information on file is invalid");
                    }
                }
            }
            catch
            {
                throw new Exception("A failure occurred while attempting to authenticate credentials");
            }

            return output;
        }

        private Dictionary<string, string> getSecurityAccount(string username)
        {
            Dictionary<string, string> output = null;            

            using (SqlConnection connection = new SqlConnection(ApplicationSettings.ConfigurationsConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("usp_GetSettings", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SettingKey", ConfigSetttingKey.BasicAuth_CommonApiUsernamePrefix + username);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string settingValue = reader["SettingValue"].ToString();                        

                        if (Convert.ToBoolean(reader["IsEncrypted"]))
                        {
                            output = JsonConfigReader.Deserialize<Dictionary<string, string>>(EncryptionHelper.RijndaelDecryptString(settingValue, ApplicationSettings.ConfigurationsEncryptionKey));
                        }
                        else
                        {
                            output = JsonConfigReader.Deserialize<Dictionary<string, string>>(settingValue);
                        }
                    }
                }
            }

            return output;
        }
    }

    public class CustomAuthUserSession : AuthUserSession
    {
        private List<string> ExcludedRoutes { get; set; }

        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            try
            {
                // Retrieve security configurations based on the authenticated username            
                Dictionary<string, string> setting = getSecurityAccount(session.UserAuthName);

                if (setting != null)
                {
                    // Populate the permissions list in session based on the permissions set within the configuration
                    if (setting.ContainsKey(ConfigSetttingKey.BasicAuth_Permissions))
                    {
                        string permissions = setting[ConfigSetttingKey.BasicAuth_Permissions];
                        Permissions = string.IsNullOrEmpty(permissions) ? new List<string>() : permissions.Split(',').ToList();
                    }
                    else
                    {
                        Permissions = new List<string>();
                    }

                    // Populate the excluded routes list in session based on the excluded routes set within the configuration
                    if (setting.ContainsKey(ConfigSetttingKey.BasicAuth_ExcludedRoutes))
                    {
                        string excludedRoutes = setting[ConfigSetttingKey.BasicAuth_ExcludedRoutes];
                        ExcludedRoutes = string.IsNullOrEmpty(excludedRoutes) ? new List<string>() : excludedRoutes.Replace(" ", string.Empty).Split(',').ToList();
                    }
                    else
                    {
                        ExcludedRoutes = new List<string>();
                    }
                }
                else
                {
                    throw new Exception("Security permissions are unable to be retrieved");
                }

                // Call base method to Save Session and fire Auth/Session callbacks:
                base.OnAuthenticated(authService, session, tokens, authInfo);
            }
            catch
            {
                throw new Exception("A failure occurred while attempting load session security profile");
            }
        }

        public override bool HasPermission(string permission, IAuthRepository authRepo)
        {
            bool output = false;

            try
            {
                // In order to support excluding routes that a user account has access too, the permission parameter passed in assumes to be delimited by the '|' character.
                // The string preceding the delimiter represents the route that is being accessed.  The string after the delimeter represents the permission required to grant access.  
                // If no delimeter is found, then the given parameter is assumed to be the permission required to grant access.  
                // If a route is specified, then the route is checked against the excluded routes for the user account in session and if a match is found then access is denied.
                // If the route is not excluded, then the permission is checked against the permissions for the user account in session and if a match is found then access is granted.            
                if (!string.IsNullOrEmpty(permission))
                {
                    List<string> fields = permission.Split('|').ToList();
                    string route = null;
                    string permissionNeeded = null;

                    if (fields.Count == 2)
                    {
                        route = fields[0].Trim();
                        permissionNeeded = fields[1].Trim();
                    }
                    else
                    {
                        permissionNeeded = fields[0].Trim();
                    }

                    output = !ExcludedRoutes.Contains(route) && (this.Permissions.Any(x => x.Trim() == ProCare.API.Core.Constants.Permissions.AllRoutes || x.Trim() == permissionNeeded));
                }
            }
            catch
            {
                throw new Exception("A failure occurred while attempting authorize API access");
            }

            return output;
        }

        private Dictionary<string, string> getSecurityAccount(string username)
        {
            Dictionary<string, string> output = null;          

            using (SqlConnection connection = new SqlConnection(ApplicationSettings.ConfigurationsConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("usp_GetSettings", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SettingKey", ConfigSetttingKey.BasicAuth_CommonApiUsernamePrefix + username);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string settingValue = reader["SettingValue"].ToString();      
                        
                        if (Convert.ToBoolean(reader["IsEncrypted"]))
                        {
                            output = JsonConfigReader.Deserialize<Dictionary<string, string>>(EncryptionHelper.RijndaelDecryptString(settingValue, ApplicationSettings.ConfigurationsEncryptionKey));
                        }
                        else
                        {
                            output = JsonConfigReader.Deserialize<Dictionary<string, string>>(settingValue);
                        }
                    }
                }
            }

            return output;
        }
    }
}
