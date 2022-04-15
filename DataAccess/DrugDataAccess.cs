using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.DTO.Drug;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using PBMEnums = ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class DrugDataAccess : DataAccessBase
    {
        public DrugDataAccess(IDataAccessHelper dataHelper) : base(dataHelper)
        {

        }

        public async Task<List<string>> SearchDrugNamebyPartialDrugName(string PartialDrugName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "SearchStr", PartialDrugName }
            };

            Task<List<string>> t = Task.Run(() =>
            {
                List<string> dbResults = new List<string>();
                DataHelper.ExecuteReader("apiPBM_Drug_FullDrugName_byPartialDrugName", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResults.Add(reader.GetStringorDefault("FullDrugName"));
                });

                return dbResults;
            });

            List<string> results = await t.ConfigureAwait(false);

            return results;
        }
        public async Task<List<DrugStrengthsDTO>> SearchDrugStrengthsByDrugName(int ClientID, string FRMID, string DrugRoute, bool ClosedFrm, bool PreferredOnly)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "ClientID", ClientID },
                { "FRMID", FRMID },
                { "DrugRoute", DrugRoute },
                { "ClosedFrm", ClosedFrm },
                { "PreferredOnly", PreferredOnly }
            };

            Task<List<DrugStrengthsDTO>> t = Task.Run(() =>
            {
                List<DrugStrengthsDTO> dbResults = new List<DrugStrengthsDTO>();
                DataHelper.ExecuteReader("apiPBM_Drug_DrugDetails_byDrugRoute", CommandType.StoredProcedure, parameters, reader =>
                {
                    DrugStrengthsDTO dto = new DrugStrengthsDTO();
                    dto.LoadFromDataReader(reader);
                    dbResults.Add(dto);
                });

                return dbResults;
            });

            List<DrugStrengthsDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        public async Task<PRXUserSSODetailsDTO> SearchPRXUserSSOAPSENRPLN(string Token)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Token", Token }
            };

            Task<PRXUserSSODetailsDTO> t = Task.Run(() =>
            {
                PRXUserSSODetailsDTO dbResults = new PRXUserSSODetailsDTO();
                DataHelper.ExecuteReader("apiPBM_PRXUserSSO_getByToken", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResults.LoadFromDataReader(reader);
                });

                return dbResults;
            });

            PRXUserSSODetailsDTO results = await t.ConfigureAwait(false);

            return results;
        }
    }
}
