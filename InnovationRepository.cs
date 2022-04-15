using System.Threading.Tasks;
using ProCare.API.PBM.Repository.DataAccess;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository
{
    public class InnovationRepository : BasedbRepository, IInnovationRepository
    {
        #region Constructor

        public InnovationRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        ///  Logs the request to the database.
        /// </summary>
        /// <param name="apiCallName">String representing the name of the API being called</param>
        /// <param name="requestJson">String representing the JSON request received</param>
        /// <returns><see cref="long" /> representing the identifier of the request</returns>
        public async Task<long> AddAdmitCCRequest(string apiCallName, string requestJson)
        {
            var sqlHelper = new InnovationDataAccess(DataHelper);
            long output = await sqlHelper.InsertAdmitCCRequest(apiCallName, requestJson).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Logs the response to the database.
        /// </summary>
        /// <param name="requestId">Long representing logging identifier of the request</param>
        /// <param name="responseJson">String representing the JSON response returned</param>
        /// <returns><see cref="long" /> representing the identifier of the response</returns>
        public async Task<long> AddAdmitCCResponse(long requestId, string responseJson)
        {
            var sqlHelper = new InnovationDataAccess(DataHelper);
            long output = await sqlHelper.InsertAdmitCCResponse(requestId, responseJson).ConfigureAwait(false);

            return output;
        }

        #endregion Public Methods
    }
}
