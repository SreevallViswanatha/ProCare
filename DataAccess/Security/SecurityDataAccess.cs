using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class SecurityDataAccess : DataAccessBase
    {
        #region Constructors

        public SecurityDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        ///  Retrieves the Hospice Name and ID corresponding to the provided Hospice Security Code if valid.
        /// </summary>
        /// <param name="hospiceSecurityCode">String representing the Hospice Security Code to validate</param>
        /// <returns><see cref="HospiceLookupDTO" /> representing the hospice details</returns>
        public async Task<HospiceLookupDTO> LookupHospiceByHospiceSecurityCode(string hospiceSecurityCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@HSC",  hospiceSecurityCode}
            };

            Task<HospiceLookupDTO> t = Task.Run(() =>
            {
                HospiceLookupDTO dbResult = new HospiceLookupDTO();
                DataHelper.ExecuteReader("apiPBM_Hospice_ByHSC", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            HospiceLookupDTO result = await t.ConfigureAwait(false);

            return result;
            ;
        }
        #endregion Public Methods
    }
}
