using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DataAccess.Prescriber
{
    public class PrescriberDataAccess : DataAccessBase
    {
        #region Constructors

        public PrescriberDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods

        /// <summary>
        ///  Get a count of search results when searching for prescribers with a given Prescriber ID in the database.
        /// </summary>
        /// <param name="prescriberId">String representing the Prescriber ID to search</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadPrescriberSearchResultsCountByPhynpi(string prescriberId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PHYNPI",  prescriberId}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_byPHYNPI_getCount", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for prescribers with a given Prescriber ID in the database.
        /// </summary>
        /// <param name="prescriberId">String representing the Prescriber ID to search</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadPrescriberSearchResultsCountByPhynpiActiveOnly(string prescriberId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PHYNPI",  prescriberId}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_byPHYNPI_getCount_activeOnly", CommandType.StoredProcedure, parameters, reader =>
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
        ///  Get a count of search results when searching for prescribers with a given Last Name and optional First Name in the database.
        /// </summary>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadPrescriberSearchResultsCountByName(string lastName, string lastNameOperator, string firstName,
                                                                      string firstNameOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_byName_getCount", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get result count from dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Get a count of search results when searching for prescribers with a given Last Name and optional First Name in the database.
        /// </summary>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> ReadPrescriberSearchResultsCountByNameActiveOnly(string lastName, string lastNameOperator, string firstName,
                                                                      string firstNameOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_byName_getCount_activeOnly", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetInt32orDefault("ResultCount");
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to get result count from dataset.");
                }

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Search for prescribers with a given Prescriber ID in the database.
        /// </summary>
        /// <param name="prescriberId">String representing the Prescriber ID to search</param>
        /// <returns><see cref="List{PrescriberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<PrescriberSearchDTO>> ReadPrescriberSearchResultsByPhynpi(string prescriberId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PHYNPI",  prescriberId}
            };

            Task<List<PrescriberSearchDTO>> t = Task.Run(() =>
            {
                List<PrescriberSearchDTO> dbResults = new List<PrescriberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_byPHYNPI", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PrescriberSearchDTO dto = new PrescriberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search dataset.");
                }

                return dbResults;
            });

            List<PrescriberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for prescribers with a given Prescriber ID in the database.
        /// </summary>
        /// <param name="prescriberId">String representing the Prescriber ID to search</param>
        /// <returns><see cref="List{PrescriberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<PrescriberSearchDTO>> ReadPrescriberSearchResultsByPhynpiActiveOnly(string prescriberId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PHYNPI",  prescriberId}
            };

            Task<List<PrescriberSearchDTO>> t = Task.Run(() =>
            {
                List<PrescriberSearchDTO> dbResults = new List<PrescriberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_byPHYNPI_activeOnly", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PrescriberSearchDTO dto = new PrescriberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search dataset.");
                }

                return dbResults;
            });

            List<PrescriberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for prescribers with a given Last Name and optional First Name in the database.
        /// </summary>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="List{PrescriberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<PrescriberSearchDTO>> ReadPrescriberSearchResultsByName(string lastName, string lastNameOperator, string firstName,
                                                                                       string firstNameOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()}
            };

            Task<List<PrescriberSearchDTO>> t = Task.Run(() =>
            {
                List<PrescriberSearchDTO> dbResults = new List<PrescriberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_byName", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PrescriberSearchDTO dto = new PrescriberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search dataset.");
                }

                return dbResults;
            });

            List<PrescriberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Search for prescribers with a given Last Name and optional First Name in the database.
        /// </summary>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <returns><see cref="List{PrescriberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<PrescriberSearchDTO>> ReadPrescriberSearchResultsByNameActiveOnly(string lastName, string lastNameOperator, string firstName,
                                                                                       string firstNameOperator)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@LastName",  lastName?.ToUpper()},
                {"@LastNameOperator",  lastNameOperator?.ToUpper()},
                {"@FirstName",  firstName?.ToUpper()},
                {"@FirstNameOperator",  firstNameOperator?.ToUpper()}
            };

            Task<List<PrescriberSearchDTO>> t = Task.Run(() =>
            {
                List<PrescriberSearchDTO> dbResults = new List<PrescriberSearchDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_byName_activeOnly", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PrescriberSearchDTO dto = new PrescriberSearchDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException($"Failed to search dataset.");
                }

                return dbResults;
            });

            List<PrescriberSearchDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get Prescriber Detail from database based on NPI Number.
        /// </summary>
        /// <param name="npi">String representing prescriber NPI Id.</param>
        /// <returns><see cref="List{PrescriberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<PrescriberDetailDTO> ReadPrescriberDetailbyNPI(string npi)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@NPI",  npi},
            };

            Task<PrescriberDetailDTO> t = Task.Run(() =>
            {
                PrescriberDetailDTO dbResults = new PrescriberDetailDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_Detail_byNPI", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PrescriberDetailDTO dto = new PrescriberDetailDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults = dto;
                    });
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw new TaskCanceledException($"Failed to retrieve prescriber detail.");
                }

                return dbResults;
            });

            PrescriberDetailDTO results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get Prescriber Detail from database based on DEA.
        /// </summary>
        /// <param name="npi">String representing prescriber DEA Id.</param>
        /// <returns><see cref="List{PrescriberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<PrescriberDetailDTO> ReadPrescriberDetailbyDEA(string dea)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@DEA",  dea?.ToUpper()},
            };

            Task<PrescriberDetailDTO> t = Task.Run(() =>
            {
                PrescriberDetailDTO dbResults = new PrescriberDetailDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_Detail_byDEA", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PrescriberDetailDTO dto = new PrescriberDetailDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults = dto;
                    });
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw new TaskCanceledException($"Failed to retrieve prescriber detail.");
                }

                return dbResults;
            });

            PrescriberDetailDTO results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get Prescriber Alternate Adresses from database based on NPI.
        /// </summary>
        /// <param name="npi">String representing prescriber NPI Id.</param>
        /// <returns><see cref="List{PrescriberAddressDTO}" /> representing the results of the given search</returns>
        public async Task<List<PrescriberAddressDTO>> ReadPrescriberAlternateAdressbyNPI(string npi)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@NPI",  npi},
            };

            Task<List<PrescriberAddressDTO>> t = Task.Run(() =>
            {
                List<PrescriberAddressDTO> dbResults = new List<PrescriberAddressDTO>();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_AlternateAdress_byNPI", CommandType.StoredProcedure, parameters, reader =>
                    {
                        PrescriberAddressDTO dto = new PrescriberAddressDTO();
                        dto.LoadFromDataReader(reader);
                        dbResults.Add(dto);

                    });
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw new TaskCanceledException($"Failed to retrieve prescriber detail.");
                }

                return dbResults;
            });

            List<PrescriberAddressDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Get Prescriber NPI based on DEA id.
        /// </summary>
        /// <param name="dea">String representing prescriber DEA Id.</param>
        /// <returns><see cref="{string}" /> Prescriber NPI number</returns>
        public async Task<string> ReadPrescriberNPI(string dea)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@DEA",  dea?.ToUpper()},
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Prescriber_getNPI_byDEA", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult = reader.GetStringorDefault("PHYNPI");

                    });
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw new TaskCanceledException($"Failed to retrieve prescriber detail.");
                }

                return dbResult;
            });

            string results = await t.ConfigureAwait(false);

            return results;
        }

        public async Task<PhysicianDTO> GetPhysician(string npi, string dea)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@PHYNPI", npi?.ToUpper() },
                { "@PHYDEA", dea?.ToUpper() }
            };

            Task<PhysicianDTO> t = Task.Run(() =>
            {
                PhysicianDTO dbResult = new PhysicianDTO();

                try
                {
                    DataHelper.ExecuteReader("apiPBM_Physician_byNpiDea", CommandType.StoredProcedure, parameters, reader =>
                    {
                        dbResult.LoadFromDataReader(reader);
                    });
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to lookup physician information.");
                }

                return dbResult;
            });

            PhysicianDTO result = await t.ConfigureAwait(false);

            return result;
        }

        public void AddPhysician(string npi, string dea, string firstName, string lastName, DateTime effectiveDate, DateTime? terminationDate, string userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@PHYNPI", npi?.ToUpper() },
                { "@PHYDEA", dea?.ToUpper() },
                { "@FNAME", firstName?.ToUpper() },
                { "@LNAME", lastName?.ToUpper() },
                { "@EFFDT", effectiveDate },
                { "@TRMDT", terminationDate },
                { "@USERNAME", userId }
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteNonQuery("apiPBM_Physician_insert", CommandType.StoredProcedure, parameters);
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to add physician.");
                }

                return dbResult;
            });
        }

        public void UpdatePhysician(string npi, string firstName, string lastName, DateTime effectiveDate, DateTime? terminationDate, string userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@PHYNPI", npi?.ToUpper() },
                { "@FNAME", firstName?.ToUpper() },
                { "@LNAME", lastName?.ToUpper() },
                { "@EFFDT", effectiveDate },
                { "@TRMDT", terminationDate },
                { "@USERNAME", userId }
            };

            Task<string> t = Task.Run(() =>
            {
                string dbResult = string.Empty;

                try
                {
                    DataHelper.ExecuteNonQuery("apiPBM_Physician_update", CommandType.StoredProcedure, parameters);
                }
                catch (Exception)
                {
                    throw new TaskCanceledException("Failed to update physician.");
                }

                return dbResult;
            });
        }

        #endregion
    }
}
