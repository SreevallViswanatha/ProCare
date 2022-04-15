using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class PharmacyDataAccess : DataAccessBase
    {
        #region Constructors

        public PharmacyDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Lookup network information for a given pharmacy
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="pharmacyId">Enum representing the NCPDP ID of the pharmacy</param>
        /// <param name="distance">String representing the distance in miles that the pharmacy is from the searched zip code</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <returns><see cref="List{PharmacyNetworkDTO}" /> representing the pharmacies and any of their corresponding networks</returns>
        public async Task<List<PharmacyNetworkDTO>> ReadPharmacyNetworkResults(string clientName, string pharmacyId, double distance, string planId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PHAID",  pharmacyId?.ToUpper()},
                {"@PLNID",  planId?.ToUpper()}
            };

            Task<List<PharmacyNetworkDTO>> t = Task.Run(() =>
            {
                List<PharmacyNetworkDTO> dbResults = new List<PharmacyNetworkDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_getNetworkDetails", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PharmacyNetworkDTO dto = new PharmacyNetworkDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Pharmacy.Client = clientName;
                        dto.Pharmacy.Distance = distance;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<PharmacyNetworkDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Lookup pharmacies for a given plan in the current dataset
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <returns><see cref="List{string}" /> representing the NCPDP Pharmacy IDs of the pharmacies</returns>
        public async Task<List<string>> ReadPlanPharmacies(string clientName, string planId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID",  planId?.ToUpper()}
            };

            Task<List<string>> t = Task.Run(() =>
            {
                List<string> dbResults = new List<string>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_getPharmaciesForPlan", CommandType.StoredProcedure, parameters, reader =>
                    {
                        string phaId = reader.GetStringorDefault("PHAID");
                        dbResults.Add(phaId);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get plan pharmacies from the {clientName} dataset.");
                }

                return dbResults;
            });

            List<string> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        ///  Verifies whether a pharmacy with the given pharmacy ID exists.
        /// </summary>
        /// <param name="pharmacyID">String representing the identifier of the pharmacy</param>
        /// <returns><see cref="bool" /> representing whether a pharmacy with the given pharmacy ID exists</returns>
        public async Task<bool> PharmacyExists(string pharmacyID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PPNID", pharmacyID?.ToUpper()}
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbExists = false;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_exists", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbExists = reader.GetBooleanSafe("PharmacyExists");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to check whether pharmacy exists.");
                }

                return dbExists;
            });

            bool exists = await t.ConfigureAwait(false);

            return exists;
        }

        /// <summary>
        /// Get the count of results for pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="clientIdList">String representing list of Client IDs for all PRX clients being searched</param>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <param name="open24Hours">Optional boolean whether to filter the results to only pharmacies open 24 hours</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <returns><see cref="int" /> representing the count of pharmacies in the given radius from the zip code</returns>
        public async Task<int> ReadPharmacySearchResultsCount(string clientIdList, string zip, int withinMiles, bool? open24Hours)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ZipCode", zip},
                {"@Miles",  withinMiles},
                {"@24Hour",  open24Hours},
                {"@ClientIDList",  clientIdList}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_ByDistanceZip_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get result count.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
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
        /// <returns><see cref="int" /> representing the count of pharmacies in the given radius from the zip code</returns>
        public async Task<int> ReadPharmacySearchResultsCountForPlan(string clientName, string zip, int withinMiles, bool? open24Hours, bool? flex90, string planId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ZipCode", zip},
                {"@Miles",  withinMiles},
                {"@24Hour",  open24Hours},
                {"@IsFlex90",  flex90},
                {"@PlnId", planId?.ToUpper()}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_byDistance_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get result count from {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get the count of results for pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <param name="open24Hours">Optional boolean representing whether to filter the results to only pharmacies open 24 hours</param>
        ///  <param name="flex90">Optional boolean representing whether to filter the results to only pharmacies that are flex90</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the count of pharmacies in the given radius from the zip code</returns>
        public async Task<int> ReadPharmacySearchResultsCountForPlan_LimitedAccess(string clientName, string zip, int withinMiles, bool? open24Hours, bool? flex90, string planId,
                                                                        List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ZipCode", zip},
                {"@Miles",  withinMiles},
                {"@24Hour",  open24Hours},
                {"@IsFlex90",  flex90},
                {"@PlnId", planId?.ToUpper()},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_byDistance_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get result count from {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get a batch of pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="batchSize">Integer representing the maximum batch size to retrieve.</param>
        /// <param name="clientIdList">String representing list of Client IDs for all PRX clients being searched</param>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <param name="open24Hours">Optional boolean whether to filter the results to only pharmacies open 24 hours</param>
        /// <returns><see cref="List{PharmacyDistanceDTO}" /> representing the NCPDP Pharmacy IDs of the pharmacies and their distance in miles from the zip code</returns>
        public async Task<List<PharmacyDistanceDTO>> ReadPharmacySearchResultsWithinDistance(int batchSize, string clientIdList, string zip, int withinMiles,
                                                                                         bool? open24Hours)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@BatchSize", batchSize},
                {"@ZipCode", zip},
                {"@Miles",  withinMiles},
                {"@24Hour",  open24Hours},
                {"@ClientIDList",  clientIdList}
            };

            Task<List<PharmacyDistanceDTO>> t = Task.Run(() =>
            {
                List<PharmacyDistanceDTO> dbResults = new List<PharmacyDistanceDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_ByDistanceZip", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PharmacyDistanceDTO dto = new PharmacyDistanceDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search pharmacies.");
                }

                return dbResults;
            });

            List<PharmacyDistanceDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get a batch of pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <param name="open24Hours">Optional boolean representing whether to filter the results to only pharmacies open 24 hours</param>
        /// <param name="flex90">Optional boolean representing whether to filter the results to only pharmacies that are flex90</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <returns><see cref="List{PharmacyNetworkDTO}" /> representing the list of pharmacies, their distance in miles from the zip code, and network information</returns>
        public async Task<List<PharmacyNetworkDTO>> ReadPharmacySearchResultsWithinDistanceForPlan(string clientName, string zip, int withinMiles,
                                                                                         bool? open24Hours, bool? flex90, string planId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ZipCode", zip},
                {"@Miles",  withinMiles},
                {"@24Hour",  open24Hours},
                {"@IsFlex90",  flex90},
                {"@PlnID",  planId?.ToUpper()},
            };

            Task<List<PharmacyNetworkDTO>> t = Task.Run(() =>
            {
            List<PharmacyNetworkDTO> dbResults = new List<PharmacyNetworkDTO>();

            try
            {
                DataHelper.ExecuteReader("apiPBM_Pharmacy_byDistance", CommandType.StoredProcedure, parameters, reader =>
                {
                    PharmacyNetworkDTO dto = new PharmacyNetworkDTO();
                    dto.LoadFromDataReader(reader);
                    dto.Pharmacy.Client = clientName;
                    dbResults.Add(dto);
                });
            }
            catch (Exception)
            {
                throw new TaskCanceledException($"Failed to search {clientName} pharmacies.");
            }

            return dbResults;
            });

            List<PharmacyNetworkDTO> results = await t.ConfigureAwait(false);

           return results;
        }

        /// <summary>
        /// Get a batch of pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <param name="open24Hours">Optional boolean representing whether to filter the results to only pharmacies open 24 hours</param>
        /// <param name="planId">Optional string representing the ID of the plan to return results for</param>
        /// <param name="flex90">Optional boolean representing whether to filter the results to only pharmacies that are flexible over 90 day prescription</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{PharmacyNetworkDTO}" /> representing the list of pharmacies, their distance in miles from the zip code, and network information</returns>
        public async Task<List<PharmacyNetworkDTO>> ReadPharmacySearchResultsWithinDistanceForPlan_LimitedAccess(string clientName, string zip, int withinMiles,
                                                                                         bool? open24Hours, bool? flex90, string planId,
                                                                                         List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ZipCode", zip},
                {"@Miles",  withinMiles},
                {"@24Hour",  open24Hours},
                {"@IsFlex90",  flex90},
                {"@PlnID",  planId?.ToUpper()},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<List<PharmacyNetworkDTO>> t = Task.Run(() =>
            {
                List<PharmacyNetworkDTO> dbResults = new List<PharmacyNetworkDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_byDistance_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PharmacyNetworkDTO dto = new PharmacyNetworkDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Pharmacy.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} pharmacies.");
                }

                return dbResults;
            });

            List<PharmacyNetworkDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get a batch of pharmacies within a given radius of a zip code
        /// </summary>
        /// <param name="zip">String representing 5-character zip code being searched</param>
        /// <param name="withinMiles">Integer representing the radius in miles to search from the zip code</param>
        /// <returns><see cref="List{PharmacySearchDTO}" /> representing the list of pharmacies and their distance in miles from the zip code</returns>
        public async Task<List<PharmacySearchDTO>> ReadTop10PharmacySearchResultsWithinDistance(string zip, int withinMiles)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ZipCode", zip},
                {"@Miles",  withinMiles}
            };

            Task<List<PharmacySearchDTO>> t = Task.Run(() =>
            {
                List<PharmacySearchDTO> dbResults = new List<PharmacySearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Pharmacy_Top10_byDistance", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PharmacySearchDTO dto = new PharmacySearchDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to locate pharmacies.");
                }

                return dbResults;
            });

            List<PharmacySearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }
        #endregion
    }
}
