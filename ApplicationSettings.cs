using ProCare.API.Core.Constants;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using ProCare.Common;

namespace ProCare.API.PBM
{
    public static class ApplicationSettings
    {
        #region Public Variables

        public static string CommonApiBaseUrl { get { return _commonApiBaseUrl; } }
        public static string CommonApiUsername { get { return _commonApiUsername; } }
        public static string CommonApiPassword { get { return _commonApiPassword; } }

        #endregion Public Variables

        #region Private Variables

        private static string _commonApiBaseUrl;
        private static string _commonApiUsername;
        private static string _commonApiPassword;
        private static IConfiguration _configuration;
        private static string _environment;
        private static string _encryptionKey;
        private const double _apiMethodTimeoutMilliseconds = 100000;

        #endregion Private Variables        

        #region Public Methods

        public static void LoadSettings(IConfiguration configuration)
        {
            string environment = configuration.GetValue<string>($"{JSONSetting.Section_Environment}");

            _environment = environment;
            _configuration = configuration;

            if (string.IsNullOrEmpty(environment))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.Section_Environment}' configuration");
            }

            string configurationsEncryptionKey = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_ConfigurationsEncryptionKey}:{JSONSetting.ConfigurationsEncryptionKey}");

            _encryptionKey = configurationsEncryptionKey;

            if (string.IsNullOrEmpty(configurationsEncryptionKey))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.ConfigurationsEncryptionKey}' configuration");
            }

            _commonApiBaseUrl = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_CommonApiBaseUrl}:{JSONSetting.CommonApiBaseUrl}");

            if (string.IsNullOrEmpty(_commonApiBaseUrl))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.CommonApiBaseUrl}' configuration");
            }

            bool isbaseURLEncrypted;
            if (!bool.TryParse(configuration.GetValue<string>($"{environment}:{JSONSetting.Section_CommonApiBaseUrl}:{JSONSetting.CommonApiBaseUrl_IsEncrypted}"), out isbaseURLEncrypted))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.CommonApiBaseUrl_IsEncrypted}' configuration");
            }

            if (isbaseURLEncrypted)
            {
                _commonApiBaseUrl = EncryptionHelper.RijndaelDecryptString(_commonApiBaseUrl, configurationsEncryptionKey);
            }

            _commonApiUsername = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_CommonApiUsername}:{JSONSetting.CommonApiUsername}");

            if (string.IsNullOrEmpty(_commonApiUsername))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.CommonApiUsername}' configuration");
            }

            bool isUsernameEncrypted;
            if (!bool.TryParse(configuration.GetValue<string>($"{environment}:{JSONSetting.Section_CommonApiUsername}:{JSONSetting.CommonApiUsername_IsEncrypted}"), out isUsernameEncrypted))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.CommonApiUsername_IsEncrypted}' configuration");
            }

            if (isUsernameEncrypted)
            {
                _commonApiUsername = EncryptionHelper.RijndaelDecryptString(_commonApiUsername, configurationsEncryptionKey);
            }

            _commonApiPassword = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_CommonApiPassword}:{JSONSetting.CommonApiPassword}");

            if (string.IsNullOrEmpty(_commonApiPassword))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.CommonApiPassword}' configuration");
            }

            bool isPasswordEncrypted;
            if (!bool.TryParse(configuration.GetValue<string>($"{environment}:{JSONSetting.Section_CommonApiPassword}:{JSONSetting.CommonApiPassword_IsEncrypted}"), out isPasswordEncrypted))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.CommonApiPassword_IsEncrypted}' configuration");
            }

            if (isPasswordEncrypted)
            {
                _commonApiPassword = EncryptionHelper.RijndaelDecryptString(_commonApiPassword, configurationsEncryptionKey);
            }
        }

        public static string GetKey(string sectionName, string keyName, string isEncryptedKeyName)
        {
            var credKey = _configuration.GetValue<string>(
                $"{_environment}:{sectionName}:{keyName}");

            if (string.IsNullOrEmpty(credKey))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{keyName}' configuration");
            }

            bool isKeyEncrypted;
            if (!bool.TryParse(_configuration.GetValue<string>($"{_environment}:{sectionName}:{isEncryptedKeyName}"), out isKeyEncrypted))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{isEncryptedKeyName}' configuration");
            }

            if (isKeyEncrypted)
            {
                credKey = EncryptionHelper.RijndaelDecryptString(credKey, _encryptionKey);
            }

            return credKey;
        }

        public static CancellationToken GetCancellationToken(double apiTimeoutMilliseconds = _apiMethodTimeoutMilliseconds)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(apiTimeoutMilliseconds));

            return cancellationTokenSource.Token;
        }

        public static string Decrypt(string encryptedValue)
        {
            return EncryptionHelper.RijndaelDecryptString(encryptedValue, _encryptionKey);
        }
        #endregion Public Methods
    }
}
