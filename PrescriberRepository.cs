using ProCare.API.PBM.Repository.DataAccess.Prescriber;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository
{
    public class PrescriberRepository : BasedbRepository, IPrescriberRepository
    {
        /// <inheritdoc />
        public PrescriberRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        /// <summary>
        /// Search for prescribers with a given Prescriber ID or Last Name and optional First Name in the database.
        /// </summary>
        /// <param name="prescriberId">String representing the Prescriber ID to search</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="includeDeactivatedPrescribers">Boolean representing whether to include results for inactive prescribers</param>
        /// <returns><see cref="List{PrescriberSearchDTO}" /> representing the results of the given search</returns>
        public async Task<List<PrescriberSearchDTO>> GetPrescriberSearchResults(string prescriberId, string lastName, string lastNameOperator,
                                                                                string firstName, string firstNameOperator, bool includeDeactivatedPrescribers)
        {
            var adsHelper = new PrescriberDataAccess(DataHelper);
            List<PrescriberSearchDTO> output = new List<PrescriberSearchDTO>();

            //Prescriber ID search
            if (!string.IsNullOrWhiteSpace(prescriberId))
            {
                if (includeDeactivatedPrescribers)
                {
                    output = await adsHelper.ReadPrescriberSearchResultsByPhynpi(prescriberId)
                                            .ConfigureAwait(false);
                }
                else
                {
                    output = await adsHelper.ReadPrescriberSearchResultsByPhynpiActiveOnly(prescriberId)
                                            .ConfigureAwait(false);
                }
            }
            //Name search
            else
            {
                if (includeDeactivatedPrescribers)
                {
                    output = await adsHelper.ReadPrescriberSearchResultsByName(lastName, lastNameOperator, firstName, firstNameOperator)
                                            .ConfigureAwait(false);
                }
                else
                {
                    output = await adsHelper.ReadPrescriberSearchResultsByNameActiveOnly(lastName, lastNameOperator, firstName, firstNameOperator)
                                            .ConfigureAwait(false);
                }

               
            }

            return output;
        }

        /// <summary>
        ///  Get a count of search results when searching for prescribers with a given Prescriber ID or Last Name and optional First Name in the database.
        /// </summary>
        /// <param name="prescriberId">String representing the Prescriber ID to search</param>
        /// <param name="lastName">String representing the Last Name to search</param>
        /// <param name="lastNameOperator">String representing the type of Last Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="firstName">Optional string representing the First Name to search</param>
        /// <param name="firstNameOperator">Optional string representing the type of First Name search to perform (S for Starts With, E for Exact Match)</param>
        /// <param name="includeDeactivatedPrescribers">Boolean representing whether to include results for inactive prescribers</param>
        /// <returns><see cref="int" /> representing the number of results for the given search</returns>
        public async Task<int> GetPrescriberSearchResultsCount(string prescriberId, string lastName, string lastNameOperator, string firstName,
                                                               string firstNameOperator, bool includeDeactivatedPrescribers)
        {
            var adsHelper = new PrescriberDataAccess(DataHelper);
            int output = 0;

            //Prescriber ID search
            if (!string.IsNullOrWhiteSpace(prescriberId))
            {
                if (includeDeactivatedPrescribers)
                {
                    output = await adsHelper.ReadPrescriberSearchResultsCountByPhynpi(prescriberId)
                                            .ConfigureAwait(false);
                }
                else
                {
                    output = await adsHelper.ReadPrescriberSearchResultsCountByPhynpiActiveOnly(prescriberId)
                                            .ConfigureAwait(false);
                }
            }
            //Name search
            else
            {
                if (includeDeactivatedPrescribers)
                {
                    output = await adsHelper.ReadPrescriberSearchResultsCountByName(lastName, lastNameOperator, firstName, firstNameOperator)
                                            .ConfigureAwait(false);
                }
                else
                {
                    output = await adsHelper.ReadPrescriberSearchResultsCountByNameActiveOnly(lastName, lastNameOperator, firstName, firstNameOperator)
                                            .ConfigureAwait(false);
                }
            }

            return output;
        }

        /// <summary>
        /// Get Prescriber Details based on NPI.
        /// </summary>
        /// <param name="npi">String representing prescriber NPI Id.</param>
        /// <returns><see cref="{PrescriberDetailDTO}" /> Prescriber Details</returns>
        public async Task<PrescriberDetailDTO> GetPrescriberDetails(string prescriberId, PhysicianQualifier qualifier)
        {
            var adsHelper = new PrescriberDataAccess(DataHelper);
            PrescriberDetailDTO output = new PrescriberDetailDTO();

            if (qualifier == PhysicianQualifier.NPI)
            {
                output = await adsHelper.ReadPrescriberDetailbyNPI(prescriberId).ConfigureAwait(false);
            }
            else
            {
                output = await adsHelper.ReadPrescriberDetailbyDEA(prescriberId).ConfigureAwait(false);
            }

            return output;
        }


        /// <summary>
        /// Get Prescriber ALternate addresses based on NPI.
        /// </summary>
        /// <param name="npi">String representing prescriber NPI Id.</param>
        /// <returns><see cref="{PrescriberAddressDTO}" /> Prescriber Addresses</returns>
        public async Task<List<PrescriberAddressDTO>> GetPrescriberAlternateAddress(string npi,PhysicianQualifier qualifier)
        {
            var adsHelper = new PrescriberDataAccess(DataHelper);

            List<PrescriberAddressDTO> output = new List<PrescriberAddressDTO>();
            if (qualifier == PhysicianQualifier.NPI)
            {
                output = await adsHelper.ReadPrescriberAlternateAdressbyNPI(npi).ConfigureAwait(false);
            }

            return output;
        }

        /// <summary>
        /// Get Prescriber NPI based on DEA id.
        /// </summary>
        /// <param name="dea">String representing prescriber DEA Id.</param>
        /// <returns><see cref="{string}" /> Prescriber NPI number</returns>
        public async Task<string> GetPrescriberNPI(string dea)
        {
            var adsHelper = new PrescriberDataAccess(DataHelper);

            string output = string.Empty;

            output = await adsHelper.ReadPrescriberNPI(dea).ConfigureAwait(false);

            return output;

        }

        public async Task<PhysicianDTO> GetPhysician(string adsConnectionString, string npi, string dea)
        {
            var adsHelper = new PrescriberDataAccess(new AdsHelper(adsConnectionString));
            PhysicianDTO physician = await adsHelper.GetPhysician(npi, dea).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(physician.NPI) && string.IsNullOrWhiteSpace(physician.DEA))
            {
                throw new ArgumentException($"Invalid physician: A physician using NPI {npi} or DEA {dea} could not be found; contact account manager to add to Physician File");
            }
            else if (!adsStringsEqual(npi, physician.NPI) |
                    !adsStringsEqual(dea, physician.DEA))
            {
                throw new ArgumentException($"Invalid physician: Conflict when using NPI {npi} with DEA {dea}");
            }

            return physician;
        }

        private bool adsStringsEqual(string item1, string item2)
        {
            return formatAdsString(item1).Equals(formatAdsString(item2));
        }

        private string formatAdsString(string item)
        {
            return (item ?? "").ToUpper().Trim();
        }


        public void AddPhysician(string adsConnectionString, string npi, string dea, string firstName, string lastName, DateTime effectiveDate, DateTime? terminationDate, string userId)
        {
            var adsHelper = new PrescriberDataAccess(new AdsHelper(adsConnectionString));
            adsHelper.AddPhysician(npi, dea, firstName, lastName, effectiveDate, terminationDate, userId);
        }
        public void UpdatePhysician(string adsConnectionString, string npi, string firstName, string lastName, DateTime effectiveDate, DateTime? terminationDate, string userId)
        {
            var adsHelper = new PrescriberDataAccess(new AdsHelper(adsConnectionString));
            adsHelper.UpdatePhysician(npi, firstName, lastName, effectiveDate, terminationDate, userId);
        }
    }
}