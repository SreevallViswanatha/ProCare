using Funq;
using ProCare.API.Common.Repository;
using ProCare.API.Common.RequestValidator;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ProCare.Common;
using ProCare.Common.Data.SQL;
using ServiceStack;
using ServiceStack.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace ProCare.API.Common
{
    public class AppHost : ProCareAppHost
    {
        #region Constructors

        public AppHost() : base("ProCare.API.Common", typeof(CommonServices).Assembly) { }

        #endregion Constructors

        #region Public Methods

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            base.Configure(container);
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false)
            });

            Plugins.Add(new AuthFeature(() => new CustomAuthUserSession(),
                new ServiceStack.Auth.IAuthProvider[] {
                    new CustomBasicAuthProvider()
                }));

            // Register repositories
            Dictionary<string, string> connections = getConnectionStrings();
            container.Register<IConfigurationRepository>(c => new ConfigurationRepository(new SQLHelper(connections[ConfigSetttingKey.ConnectionStrings_ApplicationConfiguration])));
            container.Register<ILogRepository>(c => new LogRepository(new SQLHelper(connections[ConfigSetttingKey.ConnectionStrings_ApplicationLog])));
            container.Register<IMPIRepository>(c => new MPIRepository(new SQLHelper(connections[ConfigSetttingKey.ConnectionStrings_MasterPatientIndex])));
            container.RegisterValidator(typeof(AppEventResponseTimeLoggingRequestValidator));
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets a set of configured connection strings
        /// </summary>
        /// <returns>Dictionary(string, string) representing the set of configured connection strings</returns>
        private Dictionary<string, string> getConnectionStrings()
        {
            Dictionary<string, string> output = null;

            using (SqlConnection connection = new SqlConnection(ApplicationSettings.ConfigurationsConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("usp_GetSettings", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SettingKey", ConfigSetttingKey.ConnectionStrings);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string settingValue = reader["SettingValue"].ToString();

                        if(Convert.ToBoolean(reader["IsEncrypted"]))
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

        #endregion Private Methods
    }
}
