using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using PBMEnums = ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class MediSpanDrugDataAccess : DataAccessBase
    {
        public MediSpanDrugDataAccess(IDataAccessHelper dataHelper) : base(dataHelper)
        {

        }

        public async Task<List<MediSpanDrugDTO>> LookupDrugByNDC(string NDC)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "P_NDC", NDC }
            };

            return await GetStoredProcedureMediSpanDrugs("api_medfil_ReadByNDC", parameters).ConfigureAwait(false);
        }

        public async Task<List<MediSpanDrugDTO>> SearchMediSpan(string NDC, string DrugName, string GPI, string MSCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "P_NDC", NDC},
                { "P_DrugName", DrugName },
                { "P_GPI", GPI },
                { "P_MSCode", MSCode }
            };

            return await GetStoredProcedureMediSpanDrugs("api_medfil_search", parameters).ConfigureAwait(false);
        }

        public async Task<List<MediSpanDrugDTO>> GetStoredProcedureMediSpanDrugs(string storedProcName, Dictionary<string, object> parameters)
        {
            Task<List<MediSpanDrugDTO>> t = Task.Run(() =>
            {
                List<MediSpanDrugDTO> dbResults = new List<MediSpanDrugDTO>();
                DataHelper.ExecuteReader(storedProcName, CommandType.StoredProcedure, parameters, reader =>
                {
                    MediSpanDrugDTO dto = new MediSpanDrugDTO();
                    dto.LoadFromDataReader(reader);
                    dbResults.Add(dto);
                });

                return dbResults;
            });

            List<MediSpanDrugDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get Drug Monograph.
        /// </summary>
        /// <param name="productQualfier">String representing product identifier</param>
        /// <param name="productID">String representing the product id</param>
        /// <param name="languageCode">Enum representing the language code</param>
        /// <returns><see cref="List{MonographDTO}" /> representing the monograph details</returns>
        public async Task<MonographDTO> GetDrugMonograph(int productQualfier, string productID, PBMEnums.LanguageCode languageCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ProductQualfier",  productQualfier},
                {"@ProductID",productID },
                {"@LanguageCode", $"0{(int)languageCode}" },
            };

            Task<MonographDTO> t = Task.Run(() =>
            {
                MonographDTO dbResults = new MonographDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Monograph_byGpiNDC", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MonographDTO dto = new MonographDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults = dto;
                    });

                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw new TaskCanceledException($"Failed to get get drug monograph");
                }

                return dbResults;
            });

            MonographDTO results = await t.ConfigureAwait(false);

            return results;
        }
    }
}
