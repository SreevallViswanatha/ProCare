using ProCare.API.PBM.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public interface IPharmacyRepository
    {
        Task<List<PharmacyNetworkDTO>> GetPharmacyNetworkResults(string adsConnectionString, string clientName, string pharmacyId, double distance, string planId);

        Task<List<string>> GetPlanPharmacies(string adsConnectionString, string clientName, string planId);

        Task<int> GetPharmacySearchResultsCount(string adsConnectionString, string clientName, string zip, int withinMiles, bool? open24Hours, bool? flex90, string planId,
                                                List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);

        Task<List<PharmacyNetworkDTO>> GetPharmacySearchResultsWithinDistance(string adsConnectionString, string clientName, string zip, int withinMiles,
                                                                               bool? open24Hours, bool? flex90, string planId, 
                                                                               List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);

        Task<List<PharmacySearchDTO>> GetTop10WithinDistance(string zip, int withinMiles);


        Task<bool> PharmacyExists(string adsConnectionString, string pharmacyID);
    }
}