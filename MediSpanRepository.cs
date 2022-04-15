using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PBMEnums = ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository
{
    public class MediSpanRepository : BasedbRepository, IMediSpanRepository
    {
        private Dictionary<string, string> eProcareConnections;

        public MediSpanRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        /// <inheritdoc />
        public MediSpanRepository(IDataAccessHelper dataHelper, Dictionary<string, string> eProcareConnections) : base(dataHelper)
        {
            //lookup connections case-insensitive
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            this.eProcareConnections = new Dictionary<string, string>(eProcareConnections, comparer);
        }

        public async Task<List<MediSpanDrugDTO>> LookupDrugByNDC(string connectionString, string NDC)
        {
            IDataAccessHelper dh = new AdsHelper(connectionString);
            MediSpanDrugDataAccess mediSpanHelper = new MediSpanDrugDataAccess(dh);

            return await mediSpanHelper.LookupDrugByNDC(NDC).ConfigureAwait(false);
        }

        public async Task<List<MediSpanDrugDTO>> SearchMedifil(string connectionString, string NDC, string DrugName, string GPI, string MSCode)
        {
            List<MediSpanDrugDTO> results = new List<MediSpanDrugDTO>();

            string searchNDC = NDC;
            string searchDrugName = DrugName;
            string searchGPI = GPI;
            string searchMSCode = MSCode;

            //5 character minimum to search by NDC
            if (!String.IsNullOrEmpty(searchNDC) && searchNDC.Length < 5)
            {
                searchNDC = null;
            }

            //3 character minimum to search by DrugName
            if (!String.IsNullOrEmpty(searchDrugName) && searchDrugName.Length < 3)
            {
                searchDrugName = null;
            }

            //MSCode will either be null or only one character
            if (!String.IsNullOrEmpty(searchMSCode))
            {
                searchMSCode = searchMSCode.Trim().Substring(0, 1);
            }

            //only call proc if valid conditions for search have been met
            if (!String.IsNullOrEmpty(searchNDC) || !String.IsNullOrEmpty(searchDrugName))
            {
                IDataAccessHelper dh = new AdsHelper(connectionString);
                MediSpanDrugDataAccess mediSpanHelper = new MediSpanDrugDataAccess(dh);
                results.AddRange(await mediSpanHelper.SearchMediSpan(searchNDC, searchDrugName, searchGPI, searchMSCode).ConfigureAwait(false));
            }

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
            var sqlHelper = new MediSpanDrugDataAccess(DataHelper);

            MonographDTO output = new MonographDTO();

            output = await sqlHelper.GetDrugMonograph(productQualfier, productID, languageCode).ConfigureAwait(false);

            return output;
        }

    }
}