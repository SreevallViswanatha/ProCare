using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class FeeScheduleDataAccess : DataAccessBase
    {
        #region Constructors

        public FeeScheduleDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        ///  Validates a user's login information and returns additional fields needed to look up enrollee information
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="username">String representing the Member Portal user's username</param>
        /// <param name="password">String representing the Member Portal user's encrypted password</param>
        /// <returns><see cref="FeeScheduleDTO" /> instance of the requested fee schedule</returns>
        public async Task<FeeScheduleDTO> GetByNDCREF(string ndcRef)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@NDCREF",  ndcRef}
            };

            Task<FeeScheduleDTO> t = Task.Run(() =>
            {
                FeeScheduleDTO dbResult = new FeeScheduleDTO();

                DataHelper.ExecuteReader("apiPBM_FeeSchedule_ByNDCREF", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            FeeScheduleDTO result = await t.ConfigureAwait(false);

            return result;
        }
        
        #endregion Public Methods
    }
}
