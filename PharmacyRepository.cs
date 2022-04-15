using ProCare.API.PBM.Helpers;
using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public class PharmacyRepository : BasedbRepository, IPharmacyRepository
    {
        /// <inheritdoc />
        public PharmacyRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        /// <summary>
        /// Lookup network information for a given pharmacy
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="pharmacyId">Enum representing the NCPDP ID of the pharmacy</param>
        /// <param name="distance">String representing the distance in miles that the pharmacy is from the searched zip code</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <returns><see cref="List{PharmacyNetworkDTO}" /> representing the pharmacies and any of their corresponding networks</returns>
        public async Task<List<PharmacyNetworkDTO>> GetPharmacyNetworkResults(string adsConnectionString, string clientName, string pharmacyId,
                                                                              double distance, string planId)
        {
            var adsHelper = new PharmacyDataAccess(new AdsHelper(adsConnectionString));
            List<PharmacyNetworkDTO> output = new List<PharmacyNetworkDTO>();

            output = await adsHelper.ReadPharmacyNetworkResults(clientName, pharmacyId, distance, planId)
                                      .ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Lookup pharmacies for a given plan in the current dataset
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <returns><see cref="List{string}" /> representing the NCPDP Pharmacy IDs of the pharmacies</returns>
        public async Task<List<string>> GetPlanPharmacies(string adsConnectionString, string clientName, string planId)
        {
            var adsHelper = new PharmacyDataAccess(new AdsHelper(adsConnectionString));
            List<string> output = new List<string>();

            output = await adsHelper.ReadPlanPharmacies(clientName, planId)
                                      .ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Get the count of results for pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <param name="open24Hours">Optional boolean representing whether to filter the results to only pharmacies open 24 hours</param>
        /// <param name="flex90">Optional boolean representing whether to filter the results to only pharmacies that are flex90</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the count of pharmacies in the given radius from the zip code</returns>
        public async Task<int> GetPharmacySearchResultsCount(string adsConnectionString, string clientName, string zip, int withinMiles, bool? open24Hours, bool? flex90, string planId,
                                                            List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            int output = 0;

            var adsHelper = new PharmacyDataAccess(new AdsHelper(adsConnectionString));

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            output = limitedAccess ?
                            await adsHelper.ReadPharmacySearchResultsCountForPlan_LimitedAccess(clientName, zip, withinMiles, open24Hours, flex90, planId,
                                                                            parentIDs, organizationIDs, groupIDs, planIDs).ConfigureAwait(false) :
                            await adsHelper.ReadPharmacySearchResultsCountForPlan(clientName, zip, withinMiles, open24Hours, flex90, planId
                                                                            ).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Get a batch of pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <param name="open24Hours">Optional boolean representing whether to filter the results to only pharmacies open 24 hours</param>
        /// <param name="flex90">Optional boolean representing whether to filter the results to only pharmacies flex90</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{PharmacyNetworkDTO}" /> representing the list of pharmacies, their distance in miles from the zip code, and network information</returns>
        public async Task<List<PharmacyNetworkDTO>> GetPharmacySearchResultsWithinDistance(string adsConnectionString, string clientName,string zip, int withinMiles,
                                                                                            bool? open24Hours, bool? flex90, string planId,
                                                                                            List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            List<PharmacyNetworkDTO> output = new List<PharmacyNetworkDTO>();

            var adsHelper = new PharmacyDataAccess(new AdsHelper(adsConnectionString));

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            output = limitedAccess ?
                            await adsHelper.ReadPharmacySearchResultsWithinDistanceForPlan_LimitedAccess(clientName, zip, withinMiles, open24Hours, flex90, planId,
                                                                                    parentIDs, organizationIDs, groupIDs, planIDs).ConfigureAwait(false) :
                            await adsHelper.ReadPharmacySearchResultsWithinDistanceForPlan(clientName, zip, withinMiles, open24Hours, flex90, planId
                                                                                    ).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Get a batch of pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <returns><see cref="List{PharmacySearchDTO}" /> representing the list of pharmacies and their distance in miles from the zip code</returns>
        public async Task<List<PharmacySearchDTO>> GetTop10WithinDistance(string zip, int withinMiles)
        {
            var adsHelper = new PharmacyDataAccess(DataHelper);

            List<PharmacySearchDTO> output = await adsHelper.ReadTop10PharmacySearchResultsWithinDistance(zip, withinMiles)
                                    .ConfigureAwait(false);

            return output;
        }

        public async Task<bool> PharmacyExists(string adsConnectionString, string pharmacyID)
        {
            var adsHelper = new PharmacyDataAccess(new AdsHelper(adsConnectionString));

            bool output = await adsHelper.PharmacyExists(pharmacyID)
                                    .ConfigureAwait(false);

            return output;
        }
    }
}