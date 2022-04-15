using ProCare.API.Core.Responses;
using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public class RTMRepository : BasedbRepository, IRTMRepository
    {
        /// <inheritdoc />
        public RTMRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        public async Task ProcessSendRTMData(string adsConnectionString, List<string> masterRTMFields, List<string> masterMiscFields,
            List<string> clientRTMFields, List<string> clientMiscFields, string clientCode, string enableRecordLogging, Dictionary<string, string> clientApiSettings)
        {
            var dataAccessHelper = new RTMDataAccess(new AdsHelper(adsConnectionString));
            await dataAccessHelper.ProcessSendRTMData(adsConnectionString,masterRTMFields, masterMiscFields, clientRTMFields,
               clientMiscFields, clientCode, enableRecordLogging, clientApiSettings).ConfigureAwait(false);
        }

    }
}