using ProCare.API.PBM.Helpers;
using ProCare.API.PBM.Messages.Request;
using ProCare.API.PBM.Messages.Shared;
using ProCare.API.PBM.Repository.DataAccess.Member;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Repository
{
    public class MemberRepository : BasedbRepository, IMemberRepository
    {
        public MemberRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        public async Task<MemberTerminateDTO> TerminateMember(string connectionString, string planID, string memberEnrolleeID,
                                                              DateTime terminationDate, string username)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(connectionString));

            MemberTerminateDTO output = await adsHelper.TerminateMember(planID,
                                     memberEnrolleeID,
                                     terminationDate,
                                     username);

            return output;
        }

        /// <summary>
        /// Get details for current members with a given Member ID in the database.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberIdType">Enum representing the type of Member ID the client uses</param>
        /// <param name="organizationId">String representing the Organization ID to use in filtering members</param>
        /// <param name="groupId">String representing the Group ID to use in filtering members</param>
        /// <param name="planId">String representing the Plan ID to use in filtering members</param>
        /// <param name="memberId">String representing the Other ID to use in filtering members</param>
        /// <param name="person">Optional string representing the Person code to use in filtering members </param>
        /// <returns><see cref="List{MemberDetailsResultDTO}" /> representing the member details</returns>
        public async Task<List<MemberDetailsResultDTO>> GetMemberDetails(string adsConnectionString, string clientName, MemberIDType memberIdType,
                                                                         string organizationId, string groupId, string planId, string memberId, string person)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            List<MemberDetailsResultDTO> output = new List<MemberDetailsResultDTO>();

            //Which field Member ID is stored in varies by client
            switch (memberIdType)
            {
                case Enums.MemberIDType.OtherID:
                    {
                        var otherResult = await adsHelper.ReadMemberDetailsByOtherID(clientName, organizationId, groupId, planId, memberId, person)
                                                .ConfigureAwait(false);

                        var enhResultsOther = await adsHelper.ReadMemberDetailsByCardIDCardID2_APSENH(clientName, organizationId, groupId, planId, memberId, person)
                                                       .ConfigureAwait(false);

                        //Get potential APSENR IDs from APSENH
                        var enhEnrolleeInfo = enhResultsOther.Select(x => new { x.MemberDetail.MemberEnrolleeID, x.MemberDetail.PlanID }).Distinct().ToList();

                        //Filter to only records not already found in APSENR
                        if (enhEnrolleeInfo.Count() > 0)
                        {
                            var removeList = new[]
                            {
                            enhEnrolleeInfo.First()
                        }.ToList();

                            removeList.Clear();

                            enhEnrolleeInfo.ForEach(enrolleeInfo =>
                            {
                                int index = otherResult.FindIndex(x => x.MemberDetail.MemberEnrolleeID == enrolleeInfo.MemberEnrolleeID && x.MemberDetail.PlanID == enrolleeInfo.PlanID);
                                if (index > -1)
                                {
                                    removeList.Add(enrolleeInfo);
                                }
                            });

                            enhEnrolleeInfo.RemoveAll(x => removeList.Contains(x));
                        }

                        List<MemberDetailsResultDTO> enrResultsOther = new List<MemberDetailsResultDTO>();

                        //Get details for APSENR records looked up based off APSENH
                        enhEnrolleeInfo.ForEach(enrolleeInfo =>
                        {
                            enrResultsOther.AddRange(adsHelper.ReadMemberDetailsByEnrolleeIDPlanID(clientName, organizationId, groupId, enrolleeInfo.PlanID, enrolleeInfo.MemberEnrolleeID, person).Result);
                        });

                        output = otherResult;
                        output.AddRange(enhResultsOther);
                        output.AddRange(enrResultsOther);

                        break;
                    }
                case Enums.MemberIDType.CardIDCardID2:
                    var enrResult = await adsHelper.ReadMemberDetailsByCardIDCardID2_APSENR(clientName, organizationId, groupId, planId, memberId, person)
                                            .ConfigureAwait(false);

                    var enhResultCard = await adsHelper.ReadMemberDetailsByCardIDCardID2_APSENH(clientName, organizationId, groupId, planId, memberId, person)
                                                   .ConfigureAwait(false);

                    output = enrResult;

                    //Add only results that weren't found by previous search
                    output.AddRange(enhResultCard);

                    break;
            }

            return output;
        }

        /// <summary>
        /// Get diagnosis details for the member.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="enrolleeId">String representing the unique Member Enrollee ID to retireve diagnosis details for</param>
        /// <returns><see cref="List{MemberDetailsMemberDiagnosisDTO}" /> representing the member's diagnosis details</returns>
        public async Task<List<MemberDetailsMemberDiagnosisDTO>> GetMemberDiagnoses(string adsConnectionString, string clientName,
                                                                                    string enrolleeId)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            List<MemberDetailsMemberDiagnosisDTO> output = await adsHelper.ReadMemberDiagnoses(clientName, enrolleeId)
                                                                 .ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Search for members with a given Member ID or Name and DOB in the database.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberIdType">Enum representing the type of Member ID the client uses</param>
        /// <param name="memberId">String representing the Member ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Member ID search to perform (S for Starts With, E for Exact Match)</param>
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
        public async Task<Tuple<List<MemberSearchDTO>, int>> GetMemberSearchResults(string adsConnectionString, string clientName,
                                                                        MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                        string lastName, string lastNameOperator, string firstName,
                                                                        string firstNameOperator, DateTime? dateOfBirth,
                                                                        List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            List<MemberSearchDTO> output = new List<MemberSearchDTO>();
            int removedDuplicates = 0;

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            //Member ID search
            if (!string.IsNullOrWhiteSpace(memberId))
            {
                //Which field Member ID is stored in varies by client
                switch (memberIdType)
                {
                    case Enums.MemberIDType.OtherID:
                        {
                            List<MemberSearchDTO> otherResult = new List<MemberSearchDTO>();
                            List<MemberSearchDTO> enhResult = new List<MemberSearchDTO>();

                            //Full search
                            if (!string.IsNullOrWhiteSpace(lastName))
                            {
                                otherResult = limitedAccess ?
                                                    await adsHelper.ReadMemberSearchResultsByOtherIDNameDOB_LimitedAccess(
                                                        clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                        dateOfBirth, parentIDs, organizationIDs, groupIDs, planIDs).ConfigureAwait(false) :
                                                    await adsHelper.ReadMemberSearchResultsByOtherIDNameDOB(
                                                        clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                        dateOfBirth).ConfigureAwait(false);


                                enhResult = limitedAccess ?
                                                await adsHelper.ReadMemberSearchResultsByCardIDCardID2NameDOB_APSENH_LimitedAccess(
                                                      clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                      dateOfBirth, parentIDs, organizationIDs, groupIDs, planIDs).ConfigureAwait(false) :
                                                await adsHelper.ReadMemberSearchResultsByCardIDCardID2NameDOB_APSENH(
                                                      clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                      dateOfBirth).ConfigureAwait(false);
                            }
                            //Member ID only search
                            else
                            {
                                otherResult = limitedAccess ?
                                                    await adsHelper.ReadMemberSearchResultsByOtherID_LimitedAccess(clientName, memberId, memberIdOperator,
                                                                    parentIDs, organizationIDs, groupIDs, planIDs)
                                                                 .ConfigureAwait(false) :
                                                    await adsHelper.ReadMemberSearchResultsByOtherID(clientName, memberId, memberIdOperator)
                                                                 .ConfigureAwait(false);


                                enhResult = limitedAccess ?
                                                    await adsHelper.ReadMemberSearchResultsByCardIDCardID2_APSENH_LimitedAccess(clientName, memberId, memberIdOperator,
                                                                    parentIDs, organizationIDs, groupIDs, planIDs)
                                                               .ConfigureAwait(false) :
                                                    await adsHelper.ReadMemberSearchResultsByCardIDCardID2_APSENH(clientName, memberId, memberIdOperator)
                                                               .ConfigureAwait(false);
                            }

                            int totalCount = otherResult.Count + enhResult.Count;

                            output = otherResult;

                            //Add only results that weren't found by previous search
                            enhResult = enhResult.Where(x => !output.Select(y => y.MemberEnrolleeId).Contains(x.MemberEnrolleeId)).ToList();
                            output.AddRange(enhResult);

                            removedDuplicates = totalCount - output.Count;

                            break;
                        }
                    case Enums.MemberIDType.CardIDCardID2:
                        {
                            //Full search
                            if (!string.IsNullOrWhiteSpace(lastName))
                            {
                                output = limitedAccess ?
                                                await adsHelper.ReadMemberSearchResultsByCardIDCardID2NameDOB_APSENR_LimitedAccess(
                                                   clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                   dateOfBirth, parentIDs, organizationIDs, groupIDs, planIDs).ConfigureAwait(false) :
                                                await adsHelper.ReadMemberSearchResultsByCardIDCardID2NameDOB_APSENR(
                                                   clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                   dateOfBirth).ConfigureAwait(false);
                            }
                            //Member ID only search
                            else
                            {
                                output = limitedAccess ?
                                                await adsHelper.ReadMemberSearchResultsByCardIDCardID2_APSENR_LimitedAccess(clientName, memberId, memberIdOperator,
                                                                parentIDs, organizationIDs, groupIDs, planIDs)
                                                        .ConfigureAwait(false) :
                                                await adsHelper.ReadMemberSearchResultsByCardIDCardID2_APSENR(clientName, memberId, memberIdOperator)
                                                        .ConfigureAwait(false);
                            }
                        }
                        break;
                }
            }
            //Name and DOB search
            else
            {
                output = limitedAccess ?
                                await adsHelper.ReadMemberSearchResultsByNameDOB_LimitedAccess(clientName, lastName, lastNameOperator, firstName, firstNameOperator, dateOfBirth, memberIdType.ToString(),
                                                parentIDs, organizationIDs, groupIDs, planIDs)
                                        .ConfigureAwait(false) :
                                await adsHelper.ReadMemberSearchResultsByNameDOB(clientName, lastName, lastNameOperator, firstName, firstNameOperator, dateOfBirth, memberIdType.ToString())
                                        .ConfigureAwait(false);
            }

            return new Tuple<List<MemberSearchDTO>, int>(output, removedDuplicates);
        }

        /// <summary>
        ///  Get a count of search results when searching for members with a given Member ID or Name and DOB in the database.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="memberIdType">Enum representing the type of Member ID the client uses</param>
        /// <param name="memberId">String representing the Member ID to search</param>
        /// <param name="memberIdOperator">String representing the type of Member ID search to perform (S for Starts With, E for Exact Match)</param>
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
        public async Task<int> GetMemberSearchResultsCount(string adsConnectionString, string clientName, MemberIDType memberIdType,
                                                           string memberId, string memberIdOperator, string lastName, string lastNameOperator,
                                                           string firstName, string firstNameOperator, DateTime? dateOfBirth,
                                                           List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            int output = 0;

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            //Member ID search
            if (!string.IsNullOrWhiteSpace(memberId))
            {
                //Which field Member ID is stored in varies by client
                switch (memberIdType)
                {
                    case Enums.MemberIDType.OtherID:
                        {
                            int otherCount = 0;
                            int enhCount = 0;

                            //Full search
                            if (!string.IsNullOrWhiteSpace(lastName))
                            {
                                otherCount = limitedAccess ?
                                                    await adsHelper.ReadMemberSearchResultsCountByOtherIDNameDOB_LimitedAccess(
                                                           clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                           dateOfBirth, parentIDs, organizationIDs, groupIDs, planIDs).ConfigureAwait(false) :
                                                    await adsHelper.ReadMemberSearchResultsCountByOtherIDNameDOB(
                                                           clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                           dateOfBirth).ConfigureAwait(false);


                                enhCount = limitedAccess ?
                                                    await adsHelper.ReadMemberSearchResultsCountByCardIDCardID2NameDOB_APSENH_LimitedAccess(
                                                         clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                         dateOfBirth, parentIDs, organizationIDs, groupIDs, planIDs).ConfigureAwait(false) :
                                                    await adsHelper.ReadMemberSearchResultsCountByCardIDCardID2NameDOB_APSENH(
                                                         clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                         dateOfBirth).ConfigureAwait(false);
                            }
                            //Member ID only search
                            else
                            {

                                otherCount = limitedAccess ?
                                                    await adsHelper.ReadMemberSearchResultsCountByOtherID_LimitedAccess(clientName, memberId, memberIdOperator,
                                                                    parentIDs, organizationIDs, groupIDs, planIDs)
                                                                .ConfigureAwait(false) :
                                                    await adsHelper.ReadMemberSearchResultsCountByOtherID(clientName, memberId, memberIdOperator)
                                                                .ConfigureAwait(false);


                                enhCount = limitedAccess ?
                                                    await adsHelper.ReadMemberSearchResultsCountByCardIDCardID2_APSENH_LimitedAccess(clientName, memberId, memberIdOperator,
                                                                    parentIDs, organizationIDs, groupIDs, planIDs)
                                                              .ConfigureAwait(false) :
                                                    await adsHelper.ReadMemberSearchResultsCountByCardIDCardID2_APSENH(clientName, memberId, memberIdOperator)
                                                              .ConfigureAwait(false);
                            }

                            output = otherCount + enhCount;

                            break;
                        }
                    case Enums.MemberIDType.CardIDCardID2:
                        {
                            //Full search
                            if (!string.IsNullOrWhiteSpace(lastName))
                            {
                                output = limitedAccess ?
                                                await adsHelper.ReadMemberSearchResultsCountByCardIDCardID2NameDOB_APSENR_LimitedAccess(
                                                   clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                   dateOfBirth, parentIDs, organizationIDs, groupIDs, planIDs).ConfigureAwait(false) :
                                                await adsHelper.ReadMemberSearchResultsCountByCardIDCardID2NameDOB_APSENR(
                                                   clientName, memberId, memberIdOperator, lastName, lastNameOperator, firstName, firstNameOperator,
                                                   dateOfBirth).ConfigureAwait(false);
                            }
                            //Member ID only
                            else
                            {
                                output = limitedAccess ?
                                                await adsHelper.ReadMemberSearchResultsCountByCardIDCardID2_APSENR_LimitedAccess(clientName, memberId, memberIdOperator,
                                                                parentIDs, organizationIDs, groupIDs, planIDs)
                                                        .ConfigureAwait(false) :
                                                await adsHelper.ReadMemberSearchResultsCountByCardIDCardID2_APSENR(clientName, memberId, memberIdOperator)
                                                        .ConfigureAwait(false);
                            }

                            break;
                        }
                }
            }
            //Name and DOB only search
            else
            {
                output = limitedAccess ?
                            await adsHelper.ReadMemberSearchResultsCountByNameDOB_LimitedAccess(clientName, lastName, lastNameOperator, firstName, firstNameOperator, dateOfBirth,
                                            parentIDs, organizationIDs, groupIDs, planIDs)
                                        .ConfigureAwait(false) :
                            await adsHelper.ReadMemberSearchResultsCountByNameDOB(clientName, lastName, lastNameOperator, firstName, firstNameOperator, dateOfBirth)
                                        .ConfigureAwait(false);
            }

            return output;
        }

        public async Task<List<string>> GetEnrolleeIds(string connectionString, string planId, string memberId, MemberIDType memberTypeId)
        {
            var da = new MemberDataAccess(new AdsHelper(connectionString));

            return await da.GetEnrolleeIds(planId, memberId, memberTypeId).ConfigureAwait(false);
        }

        public async Task<List<string>> GetEnrolleeIdsByPerson(string connectionString, string planId, string memberId, MemberIDType memberTypeId, string person)
        {
            var da = new MemberDataAccess(new AdsHelper(connectionString));

            return await da.GetEnrolleeIdsByPerson(planId, memberId, memberTypeId, person).ConfigureAwait(false);
        }

        public async Task<bool> Exists(string connectionString, string planId, string enrolleeId)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(connectionString));

            return await adsHelper.MemberExists(planId, enrolleeId).ConfigureAwait(false);
        }
    }
}
