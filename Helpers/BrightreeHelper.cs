using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProCare.API.Core;
using ProCare.API.PBM.Messages.Response;
using ServiceStack;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.PBM.Repository.Helpers
{
    public static class BrightreeHelper
    {
        #region Private Variables

        private static readonly HttpClient _httpClient = new HttpClient();

        #endregion

        #region Public Methods
        /// <summary>
        /// This method get data using OAuth2 accessToken
        /// </summary>
        /// <returns></returns>
        public static async Task<PreImportResponse> ApiGet(string url, Dictionary<string, string> parameters, string accessToken)
        {
            Guid guid = Guid.NewGuid();
            string methodName = "PBMApi/Brightree/" + url?.Substring(33);

            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            // Prepare the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url + createQueryString(parameters));
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // log request
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Request, parameters).ConfigureAwait(false); ;

            HttpResponseMessage responseTask = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var jsonString = await responseTask.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (jsonString.Equals("\"Authorization has been denied for this request: 0030\""))
            {
                throw new AuthenticationException(jsonString);
            }

            PreImportResponse  apiResponse = JsonConvert.DeserializeObject<Messages.Response.PreImportResponse>(jsonString);

            // log response                
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Response, apiResponse).ConfigureAwait(false);


            return apiResponse;
        }

        public static async Task<List<BusinessUnitResponse>> ApiGetBusinessUnitGuids(string url, Dictionary<string, string> parameters, string accessToken)
        {
            Guid guid = Guid.NewGuid();
            string methodName = "PBMApi/Brightree/" + url?.Substring(33);

            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            // Prepare the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url + createQueryString(parameters));
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // log request
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Request, parameters).ConfigureAwait(false); ;

            //Request data
            HttpResponseMessage responseTask = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var jsonString = await responseTask.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (jsonString.Equals("\"Authorization has been denied for this request: 0030\""))
            {
                throw new AuthenticationException(jsonString);
            }

            List<BusinessUnitResponse> apiResponse = JsonConvert.DeserializeObject<List<BusinessUnitResponse>>(jsonString);

            // log response                
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Response, apiResponse).ConfigureAwait(false);


            return apiResponse;
        }

        /// <summary>
        /// This method get data using OAuth2 accessToken
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// This method uses the OAuth2 Client Credentials to get an Access Token to provide Authorization to the APIs.
        /// </summary>
        /// <returns></returns>
        public static async Task<AccessTokenResponse> GetAccessToken(string url, List<KeyValuePair<string, string>> requestData, string clientId, string clientSecret)
        {
            string credentials = $"{clientId}:{clientSecret}";
            Guid guid = Guid.NewGuid();
            string methodName = "PBMApi/Brightree/" + url?.Substring(33);

            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

            FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

            // Prepare the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));
            request.Content = requestBody;

            // log request
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Request, requestData.ToDictionary(x => x.Key, x => x.Value)).ConfigureAwait(false);

            //Request Token
            HttpResponseMessage response =  await _httpClient.SendAsync(request).ConfigureAwait(false);

            string jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            AccessTokenResponse apiResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(jsonString);

            // log response                
            await commonApiHelper.LogApiMessage(ApiType.PBMApi, guid, methodName, ApiMessageType.Response, apiResponse).ConfigureAwait(false);

            return apiResponse;
        }

        public static List<List<string>> GroupResponseItemsForDatabasePagedData(string[] items, int pagedDataPageSize)
        {
            return items
                   .Select((str, index) => new { str, index })
                   .GroupBy(x => x.index / pagedDataPageSize)
                   .Select(g => g.Select(x => x.str).ToList())
                   .ToList();
        }

        #endregion

        #region Private Methods

        private static string createQueryString(Dictionary<string, string> parameters)
        {
            string output;

            StringBuilder queryString = new StringBuilder();
            queryString.Append("/");

            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                if (kvp.Key.StartsWith("?"))
                {
                    queryString.Append($"{kvp.Key}{kvp.Value}");
                }
                else
                {
                    queryString.Append($"{kvp.Value}/");
                }
            }

            output = queryString.ToString();

            return output;
        }
        #endregion Private Methods
    }
}
