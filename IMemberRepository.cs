using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository
{
    public interface IMemberRepository
    {
        Task<bool> Exists(string connectionString, string planId, string enrolleeId);
        Task<List<string>> GetEnrolleeIds(string connectionString, string planId, string memberId, MemberIDType memberTypeId);
        Task<List<MemberDetailsResultDTO>> GetMemberDetails(string adsConnectionString, string clientName, MemberIDType memberIdType, string organizationId, string groupId, string planId, string memberId, string person);
        Task<List<MemberDetailsMemberDiagnosisDTO>> GetMemberDiagnoses(string adsConnectionString, string clientName, string enrolleeId);
        Task<Tuple<List<MemberSearchDTO>, int>> GetMemberSearchResults(string adsConnectionString, string clientName, MemberIDType memberIdType, string memberId, string memberIdOperator, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth, List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);
        Task<int> GetMemberSearchResultsCount(string adsConnectionString, string clientName, MemberIDType memberIdType, string memberId, string memberIdOperator, string lastName, string lastNameOperator, string firstName, string firstNameOperator, DateTime? dateOfBirth, List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs);
        Task<MemberTerminateDTO> TerminateMember(string connectionString, string planID, string memberEnrolleeID, DateTime terminationDate, string username);
        Task<List<string>> GetEnrolleeIdsByPerson(string connectionString, string planId, string memberId, MemberIDType memberTypeId, string person);
    }
}