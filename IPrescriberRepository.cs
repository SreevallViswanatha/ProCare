using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository
{
    public interface IPrescriberRepository
    {
        Task<List<PrescriberSearchDTO>> GetPrescriberSearchResults(string memberId, string lastName, string lastNameOperator,
                                                           string firstName, string firstNameOperator, bool includeDeactivatedPrescribers);

        Task<int> GetPrescriberSearchResultsCount(string memberId,
                                              string lastName, string lastNameOperator, string firstName, string firstNameOperator, bool includeDeactivatedPrescribers);
        Task<PrescriberDetailDTO> GetPrescriberDetails(string PrescriberId, PhysicianQualifier qualifier);

        Task<List<PrescriberAddressDTO>> GetPrescriberAlternateAddress(string PrescriberId, PhysicianQualifier qualifier);

        Task<string> GetPrescriberNPI(string dea);

        Task<PhysicianDTO> GetPhysician(string adsConnectionString, string npi, string dea);


        void AddPhysician(string adsConnectionString, string npi, string dea, string firstName, string lastName, DateTime effectiveDate, DateTime? terminationDate, string userId);
        
        void UpdatePhysician(string adsConnectionString, string npi, string firstName, string lastName, DateTime effectiveDate, DateTime? terminationDate, string userId);
    }
}
