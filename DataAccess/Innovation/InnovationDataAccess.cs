using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class InnovationDataAccess : DataAccessBase
    {
        #region Constructors

        public InnovationDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        ///  Logs the request to the database.
        /// </summary>
        /// <param name="apiCallName">String representing the name of the API being called</param>
        /// <param name="requestJson">String representing the JSON request received</param>
        /// <returns><see cref="long" /> representing the identifier of the request</returns>
        public async Task<long> InsertAdmitCCRequest(string apiCallName, string requestJson)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@APICallName",  apiCallName},
                {"@RequestJSON",  requestJson}
            };

            Task<long> t = Task.Run(() =>
            {
                long dbResult = 0L;
                DataHelper.ExecuteReader("pbmAPI_AdmitCCRequest_insert", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt64orDefault("RequestID");
                });

                return dbResult;
            });

            long result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Logs the response to the database.
        /// </summary>
        /// <param name="requestId">Long representing logging identifier of the request</param>
        /// <param name="responseJson">String representing the JSON response returned</param>
        /// <returns><see cref="long" /> representing the identifier of the response</returns>
        public async Task<long> InsertAdmitCCResponse(long requestId, string responseJson)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@RequestID",  requestId},
                {"@ResponseJSON",  responseJson}
            };

            Task<long> t = Task.Run(() =>
            {
                long dbResult = 0L;
                DataHelper.ExecuteReader("pbmAPI_AdmitCCResponse_insert", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt64orDefault("ResponseID");
                });

                return dbResult;
            });

            long result = await t.ConfigureAwait(false);

            return result;
        }
        #endregion Public Methods
    }
}
