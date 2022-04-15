using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.DTO.Drug;
using System.Collections.Generic;
using System.Threading.Tasks;
using PBMEnums = ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository
{
    public interface IDrugRepository
    {
        Task<List<string>> LookupFullDrugNamesByPartialDrugName(string startsWith);
        Task<List<DrugStrengthsDTO>> LookupDrugStrengthsByDrugName(int ClientID, string FRMID, string DrugRoute, bool ClosedFrm, bool PreferredOnly);
        Task<PRXUserSSODetailsDTO> GetPRXUserSSOAPSENRPLN(string connectionString, string Token);
    }
}