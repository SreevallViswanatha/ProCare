using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using ProCare.API.Core.Constants;
using ProCare.Common;

namespace ProCare.API.Claims
{
    public static class ApplicationSettings
    {
        #region Public Variables

        public static string CommonApiBaseUrl { get { return _commonApiBaseUrl; } }
        public static string CommonApiUsername { get { return _commonApiUsername; } }
        public static string CommonApiPassword { get { return _commonApiPassword; } }
        public static string SwitcherHost { get { return _switcherHost; } }
        public static int SwitcherPort { get { return _switcherPort; } }
        public static string ClaimLogPathAndFileFormat {  get {  return _claimLogPathAndFileFormat; } }
        public static int ResponsePollDelayMilliseconds { get { return _responsePollDelayMilliseconds; } }
        public static int UnmatchedResponseHistorySeconds { get { return _unmatchedResponseHistorySeconds; } }
        public static int SwitcherSendTimeoutMilliseconds { get { return _switcherSendTimeoutMilliseconds; } }
        public static int SwitcherReceiveTimeoutMilliseconds { get { return _switcherReceiveTimeoutMilliseconds; } }

        #endregion Public Variables

        #region Private Variables

        private static string _commonApiBaseUrl;
        private static string _commonApiUsername;
        private static string _commonApiPassword;
        private static string _switcherHost;
        private static int _switcherPort;
        private static string _claimLogPathAndFileFormat;
        private const double _apiMethodTimeoutMilliseconds = 100000;
        public static int _responsePollDelayMilliseconds;
        public static int _unmatchedResponseHistorySeconds;
        public static int _switcherSendTimeoutMilliseconds;
        public static int _switcherReceiveTimeoutMilliseconds;

        #endregion Private Variables        

        #region Public Methods

        public static void LoadSettings(IConfiguration configuration)
        {
            string environment = configuration.GetValue<string>($"{JSONSetting.Section_Environment}");

            if (string.IsNullOrEmpty(environment))
            {
                throw new Exception($"Failure occurred when attempting to retrieve the '{JSONSetting.Section_Environment}' configuration");
            }

            string configurationsEncryptionKey = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_ConfigurationsEncryptionKey}:{JSONSetting.ConfigurationsEncryptionKey}");

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

            _switcherHost = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_SwitcherHost}:{JSONSetting.SwitcherHost}");
            _switcherPort = configuration.GetValue<int>($"{environment}:{JSONSetting.Section_SwitcherPort}:{JSONSetting.SwitcherPort}");

            _claimLogPathAndFileFormat = configuration.GetValue<string>($"{environment}:{JSONSetting.Section_ClaimLogPathAndFileFormat}:{JSONSetting.ClaimLogPathAndFileFormat}");

            _responsePollDelayMilliseconds = configuration.GetValue<int>($"{environment}:{JSONSetting.Section_Times}:{JSONSetting.ResponsePollDelayMilliseconds}");
            _unmatchedResponseHistorySeconds = configuration.GetValue<int>($"{environment}:{JSONSetting.Section_Times}:{JSONSetting.UnmatchedResponseHistorySeconds}");
            _switcherSendTimeoutMilliseconds = configuration.GetValue<int>($"{environment}:{JSONSetting.Section_Times}:{JSONSetting.SwitcherSendTimeoutMilliseconds}");
            _switcherReceiveTimeoutMilliseconds = configuration.GetValue<int>($"{environment}:{JSONSetting.Section_Times}:{JSONSetting.SwitcherReceiveTimeoutMilliseconds}");
        }

        public static CancellationToken GetCancellationToken(double apiTimeoutMilliseconds = _apiMethodTimeoutMilliseconds)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(apiTimeoutMilliseconds));

            return cancellationTokenSource.Token;
        }
        #endregion Public Methods
    }
}
