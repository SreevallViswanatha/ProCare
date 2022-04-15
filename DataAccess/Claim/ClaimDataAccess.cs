using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DataAccess.Claim
{
    public class ClaimDataAccess : DataAccessBase
    {
        public ClaimDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #region Public Methods

        #region Daily Claims


        /// <summary>
        /// Search for OtherID in APSENR and APSENH table
        /// </summary>
        /// <param name="otherID">String representing member card id number</param>
        /// <returns><see cref="string" />string representing table where member Id is found</returns>
        public async Task<string> ReadOtherIDExists(string otherId, string otherIdOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@OtherID",  otherId?.ToUpper()},
                {"@OtherIDOperator",  otherIdOperator?.ToUpper()},
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_OtherID_exists", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("IDSource");

                    });
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw new TaskCanceledException($"Failed to read OtherId exists.");
                }

                return dbResult;
            });

            string results = await t.ConfigureAwait(false);

            return results;
        }


        /// <summary>
        /// Search for daily claims by OtherID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadDailyClaimSearchResultsByOtherID(string clientName, string memberId, 
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byOtherID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for daily claims by OtherID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadDailyClaimSearchResultsByOtherID_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byOtherID_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for daily claims by CardID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadDailyClaimSearchResultsByCardIDCardID2(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byCardIDCardID2", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for daily claims by CardID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadDailyClaimSearchResultsByCardIDCardID2_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byCardIDCardID2_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for daily claims by CardID and FillDate in APSEHN table
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadDailyClaimSearchResultsByCardIDCardID2inAPSENH(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byCardIDCardID2_inAPSENH", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for daily claims by CardID and FillDate in APSEHN table
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadDailyClaimSearchResultsByCardIDCardID2inAPSENH_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byCardIDCardID2_inAPSENH_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }


        /// <summary>
        ///  Get a count of search results when searching for claims by OtherID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        /// 
        public async Task<int> ReadDailyClaimSearchResultsCountByOtherID(string clientName, string memberId,string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byOtherID_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });

                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get a count of search results when searching for claims by OtherID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        /// 
        public async Task<int> ReadDailyClaimSearchResultsCountByOtherID_LimitedAccess(string clientName, string memberId, string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                        List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
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
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byOtherID_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });

                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }


        /// <summary>
        ///  Get a count of search results when searching for claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        /// 
        public async Task<int> ReadDailyClaimSearchResultsCountByCardIDCardID2(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byCardIDCardID2_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get a count of search results when searching for claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        /// 
        public async Task<int> ReadDailyClaimSearchResultsCountByCardIDCardID2_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
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
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byCardIDCardID2_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get a count of search results when searching for claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        /// 
        public async Task<int> ReadDailyClaimSearchResultsCountByCardIDCardID2inAPSENH(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byCardIDCardID2_inAPSENH_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get a count of search results when searching for claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        /// 
        public async Task<int> ReadDailyClaimSearchResultsCountByCardIDCardID2inAPSENH_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
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
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byCardIDCardID2_inAPSENH_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get details for a daily paid claim in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="claimNumber">String representing the Claim Number of the claim to retrieve details for</param>
        /// <returns><see cref="PaidClaimDetailsDTO" /> representing the details of the claim</returns>
        public async Task<PaidClaimDetailsDTO> ReadPaidClaimDailyDetailsByNDCREF(string clientName, string claimNumber)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@NDCREF", claimNumber?.ToUpper()}
            };

            Task<PaidClaimDetailsDTO> t = Task.Run(() =>
            {
                PaidClaimDetailsDTO dto = new PaidClaimDetailsDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Daily_byNDCREF", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dto;
            });

            PaidClaimDetailsDTO results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        ///  Get the benefit accumulator summary for the member from the database.
        /// </summary>
        /// <param name="enrolleeId">String representing the unique Member Enrollee ID of the member</param>
        /// <param name="planId">String representing the Plan ID of the member</param>
        /// <param name="planEffectiveDate">DateTime representing the plan Effective Date to use as the start date for the summary</param>
        /// <returns><see cref="MemberAccumulatorDTO" /> representing the benefit accumulator summary for the member</returns>
        public async Task<MemberAccumulatorDTO> ReadMemberAccumulatorsByEnrIdPlnIdEffDt(string enrolleeId, string planId,
                                                                                  DateTime? planEffectiveDate)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID", enrolleeId?.ToUpper()},
                {"@PLNID", planId?.ToUpper()},
                {"@EFFDT", planEffectiveDate}
            };

            Task<MemberAccumulatorDTO> t = Task.Run(() =>
            {
                MemberAccumulatorDTO dto = new MemberAccumulatorDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_Accumulators_byEnrIdPlnIdEffDt", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dto.LoadFromDataReader(reader);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search load member accumulators.");
                }

                return dto;
            });

            MemberAccumulatorDTO results = await t.ConfigureAwait(false);

            return results;
        }

        #endregion

        #region Paid Claims

        /// <summary>
        /// Search for paid claims by OtherID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadPaidClaimSearchResultsByOtherID(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byOtherID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for paid claims by OtherID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadPaidClaimSearchResultsByOtherID_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byOtherID_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for paid claims by CardID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadPaidClaimSearchResultsByCardIDCardID2(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byCardIDCardID2", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for paid claims by CardID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadPaidClaimSearchResultsByCardIDCardID2_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byCardIDCardID2_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for paid claims by CardID and FillDate in APSENH
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadPaidClaimSearchResultsByCardIDCardID2inAPSENH(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byCardIDCardID2_inAPSENH", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for paid claims by CardID and FillDate in APSENH
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadPaidClaimSearchResultsByCardIDCardID2inAPSENH_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byCardIDCardID2_inAPSENH_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for paid claims by FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadPaidClaimSearchResultsByFillDate(string clientName, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byFillDate", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }


        /// <summary>
        /// Get a count of search results when searching for paid claims by FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadPaidClaimSearchResultsCountByOtherID(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byOtherID_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get a count of search results when searching for paid claims by FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadPaidClaimSearchResultsCountByOtherID_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
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
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byOtherID_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }


        /// <summary>
        /// Get a count of search results when searching for paid claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadPaidClaimSearchResultsCountByCardIDCardID2(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byCardIDCardID2_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get a count of search results when searching for paid claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadPaidClaimSearchResultsCountByCardIDCardID2_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
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
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byCardIDCardID2_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get a count of search results when searching for paid claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadPaidClaimSearchResultsCountByCardIDCardID2inAPSENH(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byCardIDCardID2_inAPSENH_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get a count of search results when searching for paid claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadPaidClaimSearchResultsCountByCardIDCardID2inAPSENH_LimitedAccess(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
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
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byCardIDCardID2_inAPSENH_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get a count of search results when searching for paid claims by FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadPaidClaimSearchResultsCountByFillDate(string clientName, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byFillDate_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get details for a historical paid claim in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="claimNumber">String representing the Claim Number of the claim to retrieve details for</param>
        /// <returns><see cref="PaidClaimDetailsDTO" /> representing the details of the claim</returns>
        public async Task<PaidClaimDetailsDTO> ReadPaidClaimHistoryDetailsByNDCREF(string clientName, string claimNumber)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@NDCREF", claimNumber?.ToUpper()}
            };

            Task<PaidClaimDetailsDTO> t = Task.Run(() =>
            {
                PaidClaimDetailsDTO dto = new PaidClaimDetailsDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Paid_byNDCREF", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dto;
            });

            PaidClaimDetailsDTO results = await t.ConfigureAwait(false);

            return results;
        }


        #endregion

        #region Rejected Claims

        /// <summary>
        /// Search for rejected claims by OtherID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadRejectedClaimSearchResultsByOtherID(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Rejected_byOtherID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }
        /// <summary>
        /// Search for rejected claims by CardID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadRejectedClaimSearchResultsByCardIDCardID2(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Rejected_byCardIDCardID2", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }
        /// <summary>
        /// Search for rejected claims by FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadRejectedClaimSearchResultsByFillDate(string clientName, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Rejected_byFillDate", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get a count of search results when searching for rejected claims by OtherID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadRejectedClaimSearchResultsCountByOtherID(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Rejected_byOtherID_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }
        /// <summary>
        /// Get a count of search results when searching for rejected claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadRejectedClaimSearchResultsCountByCardIDCardID2(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Rejected_byCardIDCardID2_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }
        /// <summary>
        /// Get a count of search results when searching for rejected claims by FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadRejectedClaimSearchResultsCountByFillDate(string clientName, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Rejected_byFillDate_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Reversed Claims

        /// <summary>
        /// Search for reversed claims by OtherID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadReversedClaimSearchResultsByOtherID(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Reversed_byOtherID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for reversed claims by CardID and FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadReversedClaimSearchResultsByCardIDCardID2(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Reversed_byCardIDCardID2", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for reversed claims by FillDate
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<List<ClaimSearchDTO>> ReadReversedClaimSearchResultsByFillDate(string clientName, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
            };

            Task<List<ClaimSearchDTO>> t = Task.Run(() =>
            {
                List<ClaimSearchDTO> dbResults = new List<ClaimSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Reversed_byFillDate", CommandType.StoredProcedure, parameters, reader =>
                    {
                        ClaimSearchDTO dto = new ClaimSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<ClaimSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }




        /// <summary>
        /// Get a count of search results when searching for reversed claims by OtherID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the OtherID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadReversedClaimSearchResultsCountByOtherID(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Reversed_byOtherID_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get a count of search results when searching for reversed claims by CardID and FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the CardID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadReversedClaimSearchResultsCountByCardIDCardID2(string clientName, string memberId,
                                                                                    string memberIdOperator, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Reversed_byCardIDCardID2_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get a count of search results when searching for reversed claims by FillDate in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="fillDateFrom">Date representing claim search beging date</param>
        /// <param name="fillDateTo">Date representing claim search end date</param>
        /// <returns><see cref="List{ClaimSearchDTO}" />List representing the results of the given search</returns>
        public async Task<int> ReadReversedClaimSearchResultsCountByFillDate(string clientName, DateTime fillDateFrom, DateTime fillDateTo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@FillDateFrom", fillDateFrom},
                {"@FillDateTo",  fillDateTo},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Claim_Reversed_byFillDate_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }
        #endregion

        #endregion
    }
}
