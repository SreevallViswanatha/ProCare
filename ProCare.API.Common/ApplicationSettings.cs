using Microsoft.Extensions.Configuration;
using ProCare.API.Core.Constants;
using ProCare.Common;
using System;
using System.Configuration;

namespace ProCare.API.Common
{
    public static class ApplicationSettings
    {
        #region Public Variables

        public static string ConfigurationsConnectionString { get { return _configurationsConnectionString; } }
        public static string ConfigurationsEncryptionKey { get { return _configurationsEncryptionKey; } }

        #endregion Public Variables

        #region Private Variables

        private static string _configurationsConnectionString;
        private static string _configurationsEncryptionKey;

        #endregion Private Variables        

        #region Public Methods

        public static void LoadSettings(IConfiguration configuration)
        {
            // Load configurations
            string environment = configuration.GetValue<string>($"{JSONSetting.Section_Environment}");

            if (string.IsNullOrEmpty(environment))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.Section_Environment}' configuration");
            }
            
            _configurationsEncryptionKey = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_ConfigurationsEncryptionKey}:{JSONSetting.ConfigurationsEncryptionKey}");

            if (string.IsNullOrEmpty(_configurationsEncryptionKey))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.ConfigurationsEncryptionKey}' configuration");
            }

            _configurationsConnectionString = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_ConfigurationsConnectionString}:{JSONSetting.ConfigurationsConnectionString}");
            
            if (string.IsNullOrEmpty(_configurationsConnectionString))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.ConfigurationsConnectionString}' configuration");
            }

            bool isConnectionStringEncrypted;
            if (!bool.TryParse(configuration.GetValue<string>($"{environment}:{JSONSetting.Section_ConfigurationsConnectionString}:{JSONSetting.ConfigurationsConnectionString_IsEncrypted}"), out isConnectionStringEncrypted))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.ConfigurationsConnectionString_IsEncrypted}' configuration");
            }

            if(isConnectionStringEncrypted)
            {
                _configurationsConnectionString = EncryptionHelper.RijndaelDecryptString(_configurationsConnectionString, _configurationsEncryptionKey);
            }
        }

        #endregion Public Methods
    }
}
