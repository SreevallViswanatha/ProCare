using System.Threading.Tasks;
using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository
{
    public class SecurityRepository : BasedbRepository, ISecurityRepository
    {
        #region Constructor

        public SecurityRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        ///  Retrieves the Hospice Name and ID corresponding to the provided Hospice Security Code if valid.
        /// </summary>
        /// <param name="hospiceSecurityCode">String representing the Hospice Security Code to validate</param>
        /// <returns><see cref="HospiceLookupDTO" /> representing the hospice details</returns>
        public async Task<HospiceLookupDTO> LookupHospiceByHospiceSecurityCode(string hospiceSecurityCode)
        {
            var sqlHelper = new SecurityDataAccess(DataHelper);
            HospiceLookupDTO output = await sqlHelper.LookupHospiceByHospiceSecurityCode(hospiceSecurityCode).ConfigureAwait(false);

            return output;
        }
        #endregion Public Methods
    }
}
