using Newtonsoft.Json;
using ProCare.API.Core;
using ProCare.API.PBM.Messages.Response;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.PBM.Repository.Helpers
{
    public static class NetsmartHelper
    {
        #region Private Variables

        private static readonly HttpClient _httpClient = new HttpClient();

        private static CommonApiHelper _commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

        #endregion

        #region Public Methods
        /// <summary>
        /// This method get data using OAuth2 accessToken
        /// </summary>
        /// <returns></returns>
        public static async Task<string> ApiGet(string url, Dictionary<string, string> parameters, string accessToken)
        {
            var guid = Guid.NewGuid();
            var methodName = "PBMApi/Netsmart/" + url?.Substring(33);

            // Prepare the request
            var request = GetRequestMessage(url, parameters, accessToken);

            // Log request
            await _commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Request, parameters).ConfigureAwait(false);

            // Send request
            HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            // Check response
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = $"An error occurred when calling the Netsmart Api.  Class: {nameof(NetsmartHelper)}  Method: {nameof(ApiGet)}  Response Status: {response.StatusCode}";
                await _commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Response, message).ConfigureAwait(false);
                throw new ApplicationException(message);
            }

            // Extract content
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Log response                
            await _commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Response, json).ConfigureAwait(false);

            var deserializedResponse = JsonConvert.DeserializeObject(json).ToString();

            return deserializedResponse;
        }

        private static HttpRequestMessage GetRequestMessage(string url, Dictionary<string, string> parameters, string accessToken)
        {
            var requestUri = $"{url}{createQueryString(parameters)}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return request;
        }

        /// <summary>
        /// This method uses the OAuth2 Client Credentials to get an Access Token to provide Authorization to the APIs.
        /// </summary>
        /// <returns></returns>
        public static async Task<AccessTokenResponse> GetAccessToken(List<KeyValuePair<string, string>> config)
        {
            // extract parameters
            var parameters = GetOAuthParameters(config);

            // build uri
            var uri = $"{config.Single(x => x.Key == "BaseUrl.OAuthToken").Value}{config.Single(x => x.Key == "Routes.OAuthToken").Value}{createQueryString(parameters)}";

            // log request
            Guid guid = Guid.NewGuid();
            
            string methodName = "PBMApi/Netsmart" + uri?.Substring(32);
            
            await _commonApiHelper.LogApiMessage(ApiType.Netsmart, guid, methodName, ApiMessageType.Request, config.ToDictionary(x => x.Key, x => x.Value)).ConfigureAwait(false);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                //Request Token
                var json = client.GetStringAsync(new Uri(uri)).Result;

                AccessTokenResponse token = JsonConvert.DeserializeObject<AccessTokenResponse>(json);

                // log response                
                await _commonApiHelper.LogApiMessage(ApiType.Netsmart, guid, methodName, ApiMessageType.Response, token).ConfigureAwait(false);

                return token;
            }
        }

        private static Dictionary<string, string> GetOAuthParameters(List<KeyValuePair<string,string>> config)
        {
            var searchString = "OAuthToken.QueryParams.";
            var parameters = config.Where(x => x.Key.StartsWithIgnoreCase(searchString))
                                .ToDictionary(p => p.Key.Substring(searchString.Length), p => p.Value);


            return parameters;
        }

        #region Private Methods

        private static string createQueryString(Dictionary<string, string> parameters)
        {
            string output;

            StringBuilder queryString = new StringBuilder();
            queryString.Append("?");

            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                queryString.Append($"{kvp.Key}={kvp.Value}&");
            }

            output = queryString.ToString().Substring(0,queryString.Length - 1);

            return output;
        }

        #endregion Private Methods
    }
}
#endregion