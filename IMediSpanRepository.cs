using ProCare.API.PBM.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using PBMEnums = ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository
{
    public interface IMediSpanRepository
    {
        Task<List<MediSpanDrugDTO>> LookupDrugByNDC(string connectionString, string NDC);
        Task<List<MediSpanDrugDTO>> SearchMedifil(string connectionString, string NDC, string DrugName, string GPI, string MSCode);
        Task<MonographDTO> GetDrugMonograph(int productQualfier, string productID, PBMEnums.LanguageCode languageCode);
    }
}