using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.DTO.Drug;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using ProCare.Common.Data.SQL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PBMEnums = ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository
{
    public class DrugRepository : BasedbRepository, IDrugRepository
    {

        public DrugRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        public async Task<List<DrugStrengthsDTO>> LookupDrugStrengthsByDrugName(int ClientID, string FRMID, string DrugRoute, bool ClosedFrm, bool PreferredOnly)
        {
            DrugDataAccess drugHelper = new DrugDataAccess(DataHelper);

            return await drugHelper.SearchDrugStrengthsByDrugName(ClientID,FRMID,DrugRoute,ClosedFrm,PreferredOnly).ConfigureAwait(false);
        }

        public async Task<PRXUserSSODetailsDTO> GetPRXUserSSOAPSENRPLN(string connectionString, string Token)
        {
           IDataAccessHelper dh = new SQLHelper(connectionString);
            DrugDataAccess drugHelper = new DrugDataAccess(dh);

            return await drugHelper.SearchPRXUserSSOAPSENRPLN(Token).ConfigureAwait(false);
        }

        public async Task<List<string>> LookupFullDrugNamesByPartialDrugName(string startsWith)
        {
            DrugDataAccess drugHelper = new DrugDataAccess(DataHelper);

            return await drugHelper.SearchDrugNamebyPartialDrugName(startsWith).ConfigureAwait(false);
        }

      
    }
}