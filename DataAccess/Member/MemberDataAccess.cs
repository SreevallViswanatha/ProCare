using System;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository.DataAccess.Member
{
    public class MemberDataAccess : DataAccessBase
    {
        #region Constructors

        public MemberDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods

        /// <summary>
        ///  Get a count of search results when searching for members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Other ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByOtherID(string clientName, string memberId, string memberIdOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byOtherID_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Other ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByOtherID_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
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
                    DataHelper.ExecuteReader("apiPBM_Member_byOtherID_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Other ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByOtherIDNameDOB(string clientName, string memberId, string memberIdOperator,
                                                                            string lastName, string lastNameOperator, string firstName,
                                                                            string firstNameOperator, DateTime? dateOfBirth)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byOtherIDNameDOB_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Other ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByOtherIDNameDOB_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                            string lastName, string lastNameOperator, string firstName,
                                                                            string firstNameOperator, DateTime? dateOfBirth,
                                                                            List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byOtherIDNameDOB_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for current members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByCardIDCardID2_APSENR(string clientName, string memberId, string memberIdOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2_APSENR_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for current members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByCardIDCardID2_APSENR_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                                                List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2_APSENR_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for current members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByCardIDCardID2NameDOB_APSENR(string clientName, string memberId, string memberIdOperator,
                                                                                         string lastName, string lastNameOperator, string firstName,
                                                                                         string firstNameOperator, DateTime? dateOfBirth)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2NameDOB_APSENR_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for current members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByCardIDCardID2NameDOB_APSENR_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                                         string lastName, string lastNameOperator, string firstName,
                                                                                         string firstNameOperator, DateTime? dateOfBirth,
                                                                                         List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2NameDOB_APSENR_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for historical members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByCardIDCardID2_APSENH(string clientName, string memberId, string memberIdOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2_APSENH_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for historical members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByCardIDCardID2_APSENH_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                                                List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2_APSENH_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
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

        public async Task<List<string>> GetEnrolleeIds(string planId, string memberId, MemberIDType memberTypeId)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"@PlanID",  planId?.ToUpper()},
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberTypeID", memberTypeId }
            };

            Task<List<string>> t = Task.Run(() =>
            {
                var dbResults = new List<string>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_EnrolleeIdList_ByPlanIDMemberID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResults.Add(reader.GetStringorDefault("ENRID"));
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to retrieve list of Enrollee Ids for member: {memberId}.");
                }

                return dbResults;
            });

            var result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<List<string>> GetEnrolleeIdsByPerson(string planId, string memberId, MemberIDType memberTypeId, string person)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"@PlanID",  planId?.ToUpper()},
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberTypeID", memberTypeId },
                {"@Person", person }
            };

            Task<List<string>> t = Task.Run(() =>
            {
                var dbResults = new List<string>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_EnrolleeIdList_ByPlanIDMemberIDPerson", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResults.Add(reader.GetStringorDefault("ENRID"));
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to retrieve list of Enrollee Ids for member: {memberId}.");
                }

                return dbResults;
            });

            var result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get a count of search results when searching for historical members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByCardIDCardID2NameDOB_APSENH(string clientName, string memberId, string memberIdOperator,
                                                                                         string lastName, string lastNameOperator, string firstName,
                                                                                         string firstNameOperator, DateTime? dateOfBirth)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2NameDOB_APSENH_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for historical members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByCardIDCardID2NameDOB_APSENH_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                                         string lastName, string lastNameOperator, string firstName,
                                                                                         string firstNameOperator, DateTime? dateOfBirth,
                                                                                         List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2NameDOB_APSENH_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for members with a given Last Name, First Name, and Date Of Birth in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByNameDOB(string clientName, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byNameDOB_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for members with a given Last Name, First Name, and Date Of Birth in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadMemberSearchResultsCountByNameDOB_LimitedAccess(string clientName, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth,
                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byNameDOB_limitedAccess_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        /// Search for members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Other ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByOtherID(string clientName, string memberId, string memberIdOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()}
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byOtherID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Other ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByOtherID_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                                                List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byOtherID_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Other ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByOtherIDNameDOB(string clientName, string memberId, string memberIdOperator, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth}
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byOtherIDNameDOB", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Other ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Other ID search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByOtherIDNameDOB_LimitedAccess(string clientName, string memberId, string memberIdOperator, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth,
                                                                                                        List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byOtherIDNameDOB_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for current members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByCardIDCardID2_APSENR(string clientName, string memberId, string memberIdOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()}
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2_APSENR", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for current members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByCardIDCardID2_APSENR_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                                                            List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2_APSENR_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for current members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByCardIDCardID2NameDOB_APSENR(string clientName, string memberId, string memberIdOperator, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth}
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2NameDOB_APSENR", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for current members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByCardIDCardID2NameDOB_APSENR_LimitedAccess(string clientName, string memberId, string memberIdOperator, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth,
                                                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2NameDOB_APSENR_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for historical members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByCardIDCardID2_APSENH(string clientName, string memberId, string memberIdOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()}
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2_APSENH", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for historical members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByCardIDCardID2_APSENH_LimitedAccess(string clientName, string memberId, string memberIdOperator,
                                                                                                                List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2_APSENH_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for historical members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByCardIDCardID2NameDOB_APSENH(string clientName, string memberId, string memberIdOperator, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth}
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2NameDOB_APSENH", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for historical members with a given Card ID and Card ID2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberId">String representing the Card ID and Card ID2 to search</param>
        /// <param name="memberIdOperator">String representing the type of Card ID and Card ID2 search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByCardIDCardID2NameDOB_APSENH_LimitedAccess(string clientName, string memberId, string memberIdOperator, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth,
                                                                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberID",  memberId?.ToUpper()},
                {"@MemberIDOperator",  memberIdOperator?.ToUpper()},
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byCardIDCardID2NameDOB_APSENH_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for members with a given Last Name, First Name, and Date Of Birth in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="memberIdType">String representing the type of Member ID the client uses</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByNameDOB(string clientName, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth, string memberIdType)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@MemberIDType", memberIdType }
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byNameDOB", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for members with a given Last Name, First Name, and Date Of Birth in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="dateOfBirth">Optional date representing the date of birth to search</param>
        /// <param name="memberIdType">String representing the type of Member ID the client uses</param>
        /// <param name="parentIDs">Optional list of strings representing the parentIDs the client can access</param>
        /// <param name="organizationIDs">Optional list of strings representing the organizationIDs the client can access</param>
        /// <param name="groupIDs">Optional list of strings representing the groupIDs the client can access</param>
        /// <param name="planIDs">Optional list of strings representing the planIDs the client can access</param>
        /// <returns><see cref="List{MemberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<MemberSearchDTO>> ReadMemberSearchResultsByNameDOB_LimitedAccess(string clientName, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth, string memberIdType,
                                                                                                List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()},
                {"@DateOfBirth",  dateOfBirth},
                {"@MemberIDType", memberIdType },
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null},
            };

            Task<List<MemberSearchDTO>> t = Task.Run(() =>
            {
                List<MemberSearchDTO> dbResults = new List<MemberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_byNameDOB_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberSearchDTO dto = new MemberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get details for current members with a given Other ID in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="organizationId">String representing the Organization ID to use in filtering members</param>
        /// <param name="groupId">String representing the Group ID to use in filtering members</param>
        /// <param name="planId">String representing the Plan ID to use in filtering members</param>
        /// <param name="memberId">String representing the Other ID to use in filtering members</param>
        /// <param name="person">Optional string representing the Person code to use in filtering members </param>
        /// <returns><see cref="List{MemberDetailsResultDTO}" /> representing the member details</returns>
        public async Task<List<MemberDetailsResultDTO>> ReadMemberDetailsByOtherID(string clientName, string organizationId, string groupId, string planId,
                                                                                                string memberId, string person)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ORGID",  organizationId?.ToUpper()},
                {"@GRPID",  groupId?.ToUpper()},
                {"@PLNID",  planId?.ToUpper()},
                {"@MemberID",  memberId?.ToUpper()},
                {"@Person",  person}
            };

            Task<List<MemberDetailsResultDTO>> t = Task.Run(() =>
            {
                List<MemberDetailsResultDTO> dbResults = new List<MemberDetailsResultDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_Detail_byOtherID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberDetailsResultDTO dto = new MemberDetailsResultDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dto.TableName = "APSENR";
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get details from {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberDetailsResultDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get details for current members with a given Card ID and Card ID 2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="organizationId">String representing the Organization ID to use in filtering members</param>
        /// <param name="groupId">String representing the Group ID to use in filtering members</param>
        /// <param name="planId">String representing the Plan ID to use in filtering members</param>
        /// <param name="memberId">String representing the Card ID and Card ID 2 to use in filtering members</param>
        /// <param name="person">Optional string representing the Person code to use in filtering members </param>
        /// <returns><see cref="List{MemberDetailsResultDTO}" /> representing the member details</returns>
        public async Task<List<MemberDetailsResultDTO>> ReadMemberDetailsByCardIDCardID2_APSENR(string clientName, string organizationId, string groupId, string planId,
                                                                          string memberId, string person)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ORGID",  organizationId?.ToUpper()},
                {"@GRPID",  groupId?.ToUpper()},
                {"@PLNID",  planId?.ToUpper()},
                {"@MemberID",  memberId?.ToUpper()},
                {"@Person",  person}
            };

            Task<List<MemberDetailsResultDTO>> t = Task.Run(() =>
            {
                List<MemberDetailsResultDTO> dbResults = new List<MemberDetailsResultDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_Detail_byCardIDCardID2_APSENR", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberDetailsResultDTO dto = new MemberDetailsResultDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dto.TableName = "APSENR";
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get details from {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberDetailsResultDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get details for historical members with a given Card ID and Card ID 2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="organizationId">String representing the Organization ID to use in filtering members</param>
        /// <param name="groupId">String representing the Group ID to use in filtering members</param>
        /// <param name="planId">String representing the Plan ID to use in filtering members</param>
        /// <param name="memberId">String representing the Card ID and Card ID 2 to use in filtering members</param>
        /// <param name="person">Optional string representing the Person code to use in filtering members </param>
        /// <returns><see cref="List{MemberDetailsResultDTO}" /> representing the member details</returns>
        public async Task<List<MemberDetailsResultDTO>> ReadMemberDetailsByCardIDCardID2_APSENH(string clientName, string organizationId, string groupId, string planId,
                                                                                                string memberId, string person)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ORGID",  organizationId?.ToUpper()},
                {"@GRPID",  groupId?.ToUpper()},
                {"@PLNID",  planId?.ToUpper()},
                {"@MemberID",  memberId?.ToUpper()},
                {"@Person",  person}
            };

            Task<List<MemberDetailsResultDTO>> t = Task.Run(() =>
            {
                List<MemberDetailsResultDTO> dbResults = new List<MemberDetailsResultDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_Detail_byCardIDCardID2_APSENH", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberDetailsResultDTO dto = new MemberDetailsResultDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dto.TableName = "APSENH";
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get details from {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberDetailsResultDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get details for historical members with a given Card ID and Card ID 2 in the database.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="organizationId">String representing the Organization ID to use in filtering members</param>
        /// <param name="groupId">String representing the Group ID to use in filtering members</param>
        /// <param name="planId">String representing the Plan ID to use in filtering members</param>
        /// <param name="memberId">String representing the Card ID and Card ID 2 to use in filtering members</param>
        /// <param name="person">Optional string representing the Person code to use in filtering members </param>
        /// <returns><see cref="List{MemberDetailsResultDTO}" /> representing the member details</returns>
        public async Task<List<MemberDetailsResultDTO>> ReadMemberDetailsByEnrolleeIDPlanID(string clientName, string organizationId, string groupId, string planId,
                                                                                                string enrolleeId, string person)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ORGID",  organizationId?.ToUpper()},
                {"@GRPID",  groupId?.ToUpper()},
                {"@PLNID",  planId?.ToUpper()},
                {"@ENRID",  enrolleeId?.ToUpper()},
                {"@Person",  person}
            };

            Task<List<MemberDetailsResultDTO>> t = Task.Run(() =>
            {
                List<MemberDetailsResultDTO> dbResults = new List<MemberDetailsResultDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_Detail_byENRIDPLNID_APSENR", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberDetailsResultDTO dto = new MemberDetailsResultDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dto.TableName = "APSENR";
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get details from {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberDetailsResultDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get diagnosis details for the member.
        /// </summary>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="enrolleeId">String representing the unique Member Enrollee ID to retireve diagnosis details for</param>
        /// <returns><see cref="List{MemberDetailsMemberDiagnosisDTO}" /> representing the member's diagnosis details</returns>
        public async Task<List<MemberDetailsMemberDiagnosisDTO>> ReadMemberDiagnoses(string clientName, string enrolleeId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ENRID",  enrolleeId?.ToUpper()}
            };

            Task<List<MemberDetailsMemberDiagnosisDTO>> t = Task.Run(() =>
            {
                List<MemberDetailsMemberDiagnosisDTO> dbResults = new List<MemberDetailsMemberDiagnosisDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_Diagnoses_byENRID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberDetailsMemberDiagnosisDTO dto = new MemberDetailsMemberDiagnosisDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dto.MemberEnrolleeID = enrolleeId;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get diagnoses from {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberDetailsMemberDiagnosisDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        public async Task<List<MemberPhysicianLockDetailsResultDTO>> ReadMemberPhysicianLockDetailsResults_ByMember(string clientName, string planId, string enrolleeId, string person)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID",  planId?.ToUpper()},
                {"@ENRID",  enrolleeId?.ToUpper()},
                {"@Person",  person}
            };

            Task<List<MemberPhysicianLockDetailsResultDTO>> t = Task.Run(() =>
            {
                List<MemberPhysicianLockDetailsResultDTO> dbResults = new List<MemberPhysicianLockDetailsResultDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_PhysicianLock_byPlanIDEnrolleeIDPerson", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberPhysicianLockDetailsResultDTO dto = new MemberPhysicianLockDetailsResultDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get details from {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberPhysicianLockDetailsResultDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        public async Task<MemberPhysicianLockDetailsResultDTO> GetMemberPhysicianLock(string planId, string enrolleeId, string npi)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@PLNID", planId?.ToUpper() },
                { "@ENRID", enrolleeId?.ToUpper() },
                { "@PHYNPI", npi?.ToUpper() }
            };

            Task<MemberPhysicianLockDetailsResultDTO> t = Task.Run(() =>
            {
                MemberPhysicianLockDetailsResultDTO dbResult = new MemberPhysicianLockDetailsResultDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_PhysicianLock_byPlanIDEnrolleeIDPhysicianNPI", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberPhysicianLockDetailsResultDTO dto = new MemberPhysicianLockDetailsResultDTO();
                        dto.LoadFromDataReader(reader);
                        //dto.Client = clientName;
                        dbResult = dto;
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to get member-physician lock.");
                }

                return dbResult;
            });

            var result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<string> CheckMemberPhysicianLockExists(string planId, string enrolleeId, string npi)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@PLNID", planId?.ToUpper() },
                { "@ENRID", enrolleeId?.ToUpper() },
                { "@PHYNPI", npi?.ToUpper() }
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_PhysicianLock_exists", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("MemberPhysicianLockExists");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to validate whether member physician lock already exists.");
                }

                return dbResult;
            });

            string result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<string> AddMemberPhysicianLock(string planId, string enrolleeId, string npi, string dea, string physicianFirstName, string physicianLastName, DateTime effectiveDate, DateTime? terminationDate, string userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@PLNID", planId?.ToUpper() },
                { "@ENRID", enrolleeId?.ToUpper() },
                { "@PHYNPI", npi?.ToUpper() },
                { "@PHYDEA", dea?.ToUpper() },
                { "@FNAME", physicianFirstName?.ToUpper() },
                { "@LNAME", physicianLastName?.ToUpper() },
                { "@EFFDT", effectiveDate },
                { "@TRMDT", terminationDate },
                { "@USERNAME", userId }
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_PhysicianLock_insert", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("SYSID");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to add member-physician lock.");
                }

                return dbResult;
            });

            string result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task ReinstateMemberPhysicianLock(string sysId,
                                                       DateTime? effectiveDate,
                                                       string userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@AuditUserGuid", userId },
                { "@SysId", sysId },
                { "@NewEffDate", effectiveDate }
            };

            await Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteNonQueryWithTransaction("usp_Reinstate_MemberPhysician", CommandType.StoredProcedure, parameters);
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to reinstate member-physician lock.");
                }
            });
        }

        public async Task<MemberTerminateDTO> TerminateMember(string planId, string enrolleeId,
                                                              DateTime? terminationDate, string username)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@UserName", username },
                { "@PlanId", planId },
                { "@EnrolleeId", enrolleeId },
                { "@TermDate", terminationDate },
            };

            Task<List<MemberTerminateDTO>> t = Task.Run(() =>
            {
                List<MemberTerminateDTO> dbResults = new List<MemberTerminateDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_Terminate", CommandType.StoredProcedure, parameters, reader =>
                    {
                        var dto = new MemberTerminateDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);
                    });
                }
                catch (Exception ex)
                {
                    string message = "Failed to terminate member.";

                    //TODO: LOG ANY FAILURES
                    if (ex.Message.Contains("DataValidationException"))
                    {
                        var start = ex.Message.IndexOf("($START)") + "($START)".Length;
                        var end = ex.Message.IndexOf("($END)") - start;
                        message = ex.Message.Substring(start, end);
                    }

                    throw new TaskCanceledException(message);
                }

                return dbResults;
            });

            var results = await t.ConfigureAwait(false);

            return results.FirstOrDefault();
        }

        public async Task TerminateMemberPhysicianLock(string sysId,
                                                       DateTime? terminationDate,
                                                       string userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@AuditUserGuid", userId },
                { "@SysId", sysId },
                { "@TermDate", terminationDate }
            };

            await Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteNonQueryWithTransaction("usp_Terminate_MemberPhysician", CommandType.StoredProcedure, parameters);
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to terminate member-physician lock.");
                }
            });
        }

        public async Task<List<MemberPhysicianLockDetailsResultDTO>> ReadMemberPhysicianLockDetailsResults_ByMember_LimitedAccess(string clientName, string planId, string enrolleeId, string person,
                                                                                                List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID",  planId?.ToUpper()},
                {"@ENRID",  enrolleeId?.ToUpper()},
                {"@Person",  person},
                {"@PARIDList",  parentIDs.Any() ? string.Join(",", parentIDs) : null},
                {"@ORGIDList",  organizationIDs.Any() ? string.Join(",", organizationIDs) : null},
                {"@GRPIDList",  groupIDs.Any() ? string.Join(",", groupIDs) : null},
                {"@PLNIDList",  planIDs.Any() ? string.Join(",", planIDs) : null}
            };

            Task<List<MemberPhysicianLockDetailsResultDTO>> t = Task.Run(() =>
            {
                List<MemberPhysicianLockDetailsResultDTO> dbResults = new List<MemberPhysicianLockDetailsResultDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_PhysicianLock_byPlanIDEnrolleeIDPerson_limitedAccess", CommandType.StoredProcedure, parameters, reader =>
                    {
                        MemberPhysicianLockDetailsResultDTO dto = new MemberPhysicianLockDetailsResultDTO();
                        dto.LoadFromDataReader(reader);
                        dto.Client = clientName;
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get details from {clientName} dataset.");
                }

                return dbResults;
            });

            List<MemberPhysicianLockDetailsResultDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        public async Task<MemberPhysicianLockDetailsResultDTO> ReadMemberPhysicianLockDetailsResults_BySysid(string clientName, string sysid)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@SYSID",  sysid?.ToUpper()}
            };

            Task<MemberPhysicianLockDetailsResultDTO> t = Task.Run(() =>
            {
                MemberPhysicianLockDetailsResultDTO dbResult = new MemberPhysicianLockDetailsResultDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_PhysicianLock_bySysid", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult.LoadFromDataReader(reader);
                        dbResult.Client = clientName;
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get details from {clientName} dataset.");
                }

                return dbResult;
            });

            MemberPhysicianLockDetailsResultDTO result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<List<string>> ReadMemberEnrolleeIDByOtherID(string planId, string memberId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PlanID",  planId?.ToUpper()},
                {"@MemberID",  memberId?.ToUpper()}
            };

            Task<List<string>> t = Task.Run(() =>
            {
                List<string> dbResults = new List<string>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_EnrolleeID_byOtherID", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResults.Add(reader.GetStringorDefault("ENRID"));
                });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to validate Member Enrollee ID for Member ID {memberId}.");
                }

                return dbResults;
            });

            List<string> result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<List<string>> ReadMemberEnrolleeIDByCardIDCardID2_APSENR(string planId, string memberId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PlanID",  planId?.ToUpper()},
                {"@MemberID",  memberId?.ToUpper()}
            };

            Task<List<string>> t = Task.Run(() =>
            {
                List<string> dbResults = new List<string>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_EnrolleeID_byCardIDCardID2_APSENR", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResults.Add(reader.GetStringorDefault("ENRID"));
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to validate Member Enrollee ID for Member ID {memberId}.");
                }

                return dbResults;
            });

            List<string> result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<string> ReadMemberEnrolleeIDByPlanIDFamilyMemberEnrolleeIDPerson(string planId, string enrolleeId, string person)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PlanID",  planId?.ToUpper()},
                {"@FamilyMemberEnrolleeID",  enrolleeId?.ToUpper()},
                {"@Person",  person?.ToUpper()}
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = "";

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_EnrolleeID_byPlanIDFamilyMemberEnrolleeIDPerson", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("ENRID");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to find Member with appropriate Person Code.");
                }

                return dbResult;
            });

            string result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<List<string>> ReadMemberEnrolleeIDByCardIDCardID2_APSENH(string planId, string memberId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PlanID",  planId?.ToUpper()},
                {"@MemberID",  memberId?.ToUpper()}
            };

            Task<List<string>> t = Task.Run(() =>
            {
                List<string> dbResults = new List<string>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_EnrolleeID_byCardIDCardID2_APSENH", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResults.Add(reader.GetStringorDefault("ENRID"));
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to validate Member Enrollee ID for Member ID {memberId}.");
                }

                return dbResults;
            });

            List<string> result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Verifies whether a member with the given plan ID and ProCare enrollee ID exists.
        /// </summary>
        /// <param name="planId">String representing the identifier of the plan</param>
        /// <param name="enrolleeId">String representing the ProCare identifier of the enrollee</param>
        /// <returns><see cref="bool" /> representing whether a member with the given plan ID and ProCare enrollee ID exists</returns>
        public async Task<bool> MemberExists(string planId, string enrolleeId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID", planId?.ToUpper()},
                {"@ENRID", enrolleeId?.ToUpper()}
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbExists = false;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_exists_byPLNIDENRID", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbExists = reader.GetBooleanSafe("MemberExists");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to check whether member exists.");
                }

                return dbExists;
            });

            bool exists = await t.ConfigureAwait(false);

            return exists;
        }

        public async Task<bool> TrySetMemberLockInStatus(string planId, string enrolleeId, string lockInStatus)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PLNID", planId?.ToUpper()},
                {"@ENRID", enrolleeId?.ToUpper()},
                {"@PHYSREQ", lockInStatus?.ToUpper()},
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbExists = false;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Member_PhysicianLock_trySetMemberLockInStatus", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbExists = reader.GetBooleanSafe("WasAbleToSetRecord");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to check whether member exists.");
                }

                return dbExists;
            });

            bool exists = await t.ConfigureAwait(false);

            return exists;
        }

        #endregion
    }
}
