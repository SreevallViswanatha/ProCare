using ProCare.API.PBM.Messages.Shared;
using ProCare.API.PBM.Repository.DataAccess.Claim;
using ProCare.API.PBM.Repository.DataAccess.Member;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProCare.API.PBM.Repository.DataAccess.Rule;
using ProCare.API.PBM.Helpers;
using ProCare.API.PBM.Repository.DataAccess;
using ProCare.Common.Data.SQL;

namespace ProCare.API.PBM.Repository
{
    public class PbmRepository : BasedbRepository, IPbmRepository
    {
        /// <inheritdoc />
        public PbmRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }


        #region Member
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
                                                                        Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
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
        public async Task<int> GetMemberSearchResultsCount(string adsConnectionString, string clientName, Enums.MemberIDType memberIdType,
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

        public async Task<string> MemberPhysicianLockExists(string adsConnectionString, string planId, string enrolleeId, string npi)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            string output = await adsHelper.CheckMemberPhysicianLockExists(planId, enrolleeId, npi).ConfigureAwait(false);

            return output;
        }

        public async Task<List<MemberPhysicianLockDetailsResultDTO>> GetMemberPhysicianLockDetails_ByMember(string adsConnectionString, string clientName,
                                                           string planId, string enrolleeId, string person,
                                                           List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            List<MemberPhysicianLockDetailsResultDTO> output = new List<MemberPhysicianLockDetailsResultDTO>();

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            output = limitedAccess ?
                        await adsHelper.ReadMemberPhysicianLockDetailsResults_ByMember_LimitedAccess(clientName, planId, enrolleeId, person,
                                        parentIDs, organizationIDs, groupIDs, planIDs)
                                .ConfigureAwait(false) :
                        await adsHelper.ReadMemberPhysicianLockDetailsResults_ByMember(clientName, planId, enrolleeId, person)
                                .ConfigureAwait(false);

            return new List<MemberPhysicianLockDetailsResultDTO>(output);
        }

        public async Task<MemberPhysicianLockDetailsResultDTO> GetMemberPhysicianLockDetails_BySysid(string adsConnectionString, string clientName,
                                                        string sysid)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            MemberPhysicianLockDetailsResultDTO output = new MemberPhysicianLockDetailsResultDTO();

            output = await adsHelper.ReadMemberPhysicianLockDetailsResults_BySysid(clientName, sysid)
                                .ConfigureAwait(false);

            return output;
        }

        public async Task<string> AddMemberPhysicianLock(string adsConnectionString, string planId, string enrolleeId, string npi, string dea, string physicianFirstName, string physicianLastName, DateTime effectiveDate, DateTime? terminationDate, string userId)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            string output =  await adsHelper.AddMemberPhysicianLock(planId, enrolleeId, npi, dea, physicianFirstName, physicianLastName, effectiveDate, terminationDate, userId).ConfigureAwait(false);

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
        public async Task<List<MemberDetailsResultDTO>> GetMemberDetails(string adsConnectionString, string clientName, Enums.MemberIDType memberIdType,
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
                        var enhEnrolleeInfo = enhResultsOther.Select(x => new {x.MemberDetail.MemberEnrolleeID, x.MemberDetail.PlanID}).Distinct().ToList();

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

        public async void TerminateMemberPhysicianLock(string adsConnectionString, string sysId, DateTime? terminationDate, string userId)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            await adsHelper.TerminateMemberPhysicianLock(sysId, terminationDate, userId);
        }

        public async void ReinstateMemberPhysicianLock(string adsConnectionString, string sysId, DateTime? effectiveDate, string userId)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            await adsHelper.ReinstateMemberPhysicianLock(sysId, effectiveDate, userId);
        }

        public async Task<List<string>> GetMemberEnrolleeId(string adsConnectionString, Enums.MemberIDType memberIdType, string planId, string memberId)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            List<string> output = new List<string>();

            //Which field Member ID is stored in varies by client
            switch (memberIdType)
            {
                case Enums.MemberIDType.OtherID:
                {
                    var otherResult = await adsHelper.ReadMemberEnrolleeIDByOtherID(planId, memberId)
                                                     .ConfigureAwait(false);


                    var enhResult = await adsHelper.ReadMemberEnrolleeIDByCardIDCardID2_APSENH(planId, memberId)
                                                   .ConfigureAwait(false);

                    //Add only results that weren't found by previous search
                    output = otherResult.Concat(enhResult).Distinct().ToList();

                    break;
                }
                case Enums.MemberIDType.CardIDCardID2:
                    output = await adsHelper.ReadMemberEnrolleeIDByCardIDCardID2_APSENR(planId, memberId)
                                            .ConfigureAwait(false);
                    break;
            }

            return output;
        }

        public async Task<string> GetMemberEnrolleeIdWithPerson(string adsConnectionString, string planId, string enrolleeId, string person)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));
            string output = await adsHelper.ReadMemberEnrolleeIDByPlanIDFamilyMemberEnrolleeIDPerson(planId, enrolleeId, person)
                                                         .ConfigureAwait(false);
            return output;
        }

        /// <summary>
        ///  Verifies whether a member with the given plan ID and ProCare enrollee ID exists.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="planId">String representing the identifier of the plan</param>
        /// <param name="enrolleeId">String representing the ProCare identifier of the enrollee</param>
        /// <returns><see cref="bool" /> representing whether a member with the given plan ID and ProCare enrollee ID exists</returns>
        public async Task<bool> MemberExists(string adsConnectionString, string planId, string enrolleeId)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));

            bool output = await adsHelper.MemberExists(planId, enrolleeId)
                                         .ConfigureAwait(false);

            return output;
        }

        public async Task<bool> TrySetMemberLockInStatus(string adsConnectionString, string planId, string enrolleeId, string lockInStatus)
        {
            var adsHelper = new MemberDataAccess(new AdsHelper(adsConnectionString));

            bool output = await adsHelper.TrySetMemberLockInStatus(planId, enrolleeId, lockInStatus)
                                         .ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Looks up a DSGID by its DaysAmt and MaintDays values.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="daysSupplyMaximum">Integer representing the DaysAmt and MaintDays to use to lookup the DSGID</param>
        /// <returns><see cref="string" /> representing the DSGID that was looked up</returns>
        public async Task<string> LookupDSGID(string adsConnectionString, int daysSupplyMaximum)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));

            string output = await adsHelper.LookupDSGID(daysSupplyMaximum)
                                         .ConfigureAwait(false);

            return output;
        }

        #endregion Member

        #region Rule

        /// <summary>
        ///  Check whether a rule already exists in the database.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="planId">String representing the identifier of the plan</param>
        /// <param name="enrolleeId">String representing the ProCare identifier of the enrollee</param>
        /// <param name="vendType">String representing the vend type of the plan</param>
        /// <param name="codeType">String representing the code type of the product code (NDC, GCN, or GNN)</param>
        /// <param name="codes">String representing the submitted product code after being translated to the code type on the rule</param>
        /// <returns><see cref="string" /> representing the system identifier of the rule if found</returns>
        public async Task<string> RuleExists(string adsConnectionString, string planId, string enrolleeId, string vendType, string codeType,
                                           string codes)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            string output = await adsHelper.CheckRuleExists(planId, enrolleeId, vendType, codeType, codes).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Get the default template for the DynamicPACode2 and ProductID
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="dynamicPACode2">String representing the identifier of the rule template to be used</param>
        /// <param name="productID">String representing the submitted product code</param>
        /// <returns><see cref="string" /> representing the system identifier of the rule if found</returns>
        public async Task<RuleTemplateDTO> GetDynamicPARuleTemplate(string adsConnectionString, string clientName, string dynamicPACode2, string productID)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            RuleTemplateDTO output = await adsHelper.ReadDynamicPARuleTemplate(clientName, dynamicPACode2, productID).ConfigureAwait(false);

            return output;
        }

        public async Task<string> AddMemberRule_DynamicPA(string adsConnectionString, string clientName, string PLNID, string memberId, string enrolleeId, string CODES,
                                                string EPA_ID, string DESC, string TYPE, DateTime? EFFDT, DateTime? TRMDT, string CODETYPE, string VENDTYPE,
                                                string PADENIED, string SEX, string MAGEMETH, string FAGEMETH, int? MAGELO, int? MAGEHI,
                                                int? FAGELO, int? FAGEHI, string APPLYACC, string BAPPACC, string DSGID, string DSGID2,
                                                string CALCREFILL, int? REFILLDAYS, string REFILLMETH, int? REFILLPCT, int? MAXREFILLS,
                                                int? MAXREFMNT, string PENALTY, string DESI, string PHYLIMIT, string GI_GPI, string PPGID,
                                                string PPNID, string PPNREQRUL, string INCCOMP, string BRANDDISC, string GENONLY, string DRUGCLASS,
                                                string DRUGTYPE, string DRUGSTAT, string MAINTIND, int? COMPMAX, int? HIDOLLAR, double? QTYPERDYS,
                                                int? QTYDYLMT, string COPAYGCI, string COPLVLASSN, string OVRRJTADI, string OVRRJTAGE,
                                                string OVRRJTADD, string OVRRJTDDC, string OVRRJTDOT, string OVRRJTDUP, string OVRRJTIAT,
                                                string OVRRJTMMA, string OVRRJTLAC, string OVRRJTPRG, string ACTIVE, string Note, string pharmacyID,
                                                string vendorPANumber, string paidMsg, string REASON, string USERNAME, string CHANGEDBY, bool ignorePlan)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            string output = await adsHelper.InsertMemberRule_DynamicPA(clientName, PLNID, memberId, enrolleeId, CODES, EPA_ID, DESC, TYPE, EFFDT, TRMDT, CODETYPE, VENDTYPE,
                                                             PADENIED, SEX, MAGEMETH, FAGEMETH, MAGELO, MAGEHI, FAGELO, FAGEHI, APPLYACC, BAPPACC,
                                                             DSGID, DSGID2, CALCREFILL, REFILLDAYS, REFILLMETH, REFILLPCT, MAXREFILLS, MAXREFMNT,
                                                             PENALTY, DESI, PHYLIMIT, GI_GPI, PPGID, PPNID, PPNREQRUL, INCCOMP, BRANDDISC, GENONLY,
                                                             DRUGCLASS, DRUGTYPE, DRUGSTAT, MAINTIND, COMPMAX, HIDOLLAR, QTYPERDYS, QTYDYLMT,
                                                             COPAYGCI, COPLVLASSN, OVRRJTADI, OVRRJTAGE, OVRRJTADD, OVRRJTDDC, OVRRJTDOT, OVRRJTDUP,
                                                             OVRRJTIAT, OVRRJTMMA, OVRRJTLAC, OVRRJTPRG, ACTIVE, Note, pharmacyID, vendorPANumber, paidMsg, REASON,
                                                             USERNAME, CHANGEDBY, ignorePlan)
                                           .ConfigureAwait(false);

            return output;
        }

        public async Task<string> UpdateMemberRule_DynamicPA(string adsConnectionString, string clientName, string SYSID, string PLNID, string memberId, string enrolleeId, string CODES,
                                        string EPA_ID, string DESC, string TYPE, DateTime? EFFDT, DateTime? TRMDT, string CODETYPE, string VENDTYPE,
                                        string PADENIED, string SEX, string MAGEMETH, string FAGEMETH, int? MAGELO, int? MAGEHI,
                                        int? FAGELO, int? FAGEHI, string APPLYACC, string BAPPACC, string DSGID, string DSGID2,
                                        string CALCREFILL, int? REFILLDAYS, string REFILLMETH, int? REFILLPCT, int? MAXREFILLS,
                                        int? MAXREFMNT, string PENALTY, string DESI, string PHYLIMIT, string GI_GPI, string PPGID,
                                        string PPNID, string PPNREQRUL, string INCCOMP, string BRANDDISC, string GENONLY, string DRUGCLASS,
                                        string DRUGTYPE, string DRUGSTAT, string MAINTIND, int? COMPMAX, int? HIDOLLAR, double? QTYPERDYS,
                                        int? QTYDYLMT, string COPAYGCI, string COPLVLASSN, string OVRRJTADI, string OVRRJTAGE,
                                        string OVRRJTADD, string OVRRJTDDC, string OVRRJTDOT, string OVRRJTDUP, string OVRRJTIAT,
                                        string OVRRJTMMA, string OVRRJTLAC, string OVRRJTPRG, string ACTIVE, string Note, string pharmacyID,
                                        string vendorPANumber, string paidMsg, string REASON, string USERNAME, string CHANGEDBY, bool ignorePlan)
        {
            var adsHelper = new RuleDataAccess(new AdsHelper(adsConnectionString));
            string output = await adsHelper.UpdateMemberRule_DynamicPA(clientName, SYSID, PLNID, memberId, enrolleeId, CODES, EPA_ID, DESC, TYPE, EFFDT, TRMDT, CODETYPE, VENDTYPE,
                                                             PADENIED, SEX, MAGEMETH, FAGEMETH, MAGELO, MAGEHI, FAGELO, FAGEHI, APPLYACC, BAPPACC,
                                                             DSGID, DSGID2, CALCREFILL, REFILLDAYS, REFILLMETH, REFILLPCT, MAXREFILLS, MAXREFMNT,
                                                             PENALTY, DESI, PHYLIMIT, GI_GPI, PPGID, PPNID, PPNREQRUL, INCCOMP, BRANDDISC, GENONLY,
                                                             DRUGCLASS, DRUGTYPE, DRUGSTAT, MAINTIND, COMPMAX, HIDOLLAR, QTYPERDYS, QTYDYLMT,
                                                             COPAYGCI, COPLVLASSN, OVRRJTADI, OVRRJTAGE, OVRRJTADD, OVRRJTDDC, OVRRJTDOT, OVRRJTDUP,
                                                             OVRRJTIAT, OVRRJTMMA, OVRRJTLAC, OVRRJTPRG, ACTIVE, Note, pharmacyID, vendorPANumber, paidMsg, REASON,
                                                             USERNAME, CHANGEDBY, ignorePlan)
                                           .ConfigureAwait(false);

            return output;
        }
        #endregion Rule

        #region Claim

        #region Claim Search
        public async Task<Tuple<List<ClaimSearchDTO>, int>> GetDailyClaimSearchResults(string adsConnectionString, string clientName,
                                                                            Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                            DateTime fillDateFrom, DateTime fillDateTo, bool lookupAllEnrolleesForMember,
                                                                            List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            var adsHelper = new ClaimDataAccess(new AdsHelper(adsConnectionString));
            List<ClaimSearchDTO> output = new List<ClaimSearchDTO>();
            int removedDuplicates = 0;

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            if (!string.IsNullOrWhiteSpace(memberId))
            {
                switch (memberIdType)
                {
                    case Enums.MemberIDType.OtherID:

                        if (!lookupAllEnrolleesForMember)
                        {
                            string otherIdExists = await adsHelper.ReadOtherIDExists(memberId, memberIdOperator).ConfigureAwait(false);

                            if (otherIdExists == "APSENR")
                            {
                                output = limitedAccess ? 
                                                await adsHelper.ReadDailyClaimSearchResultsByOtherID_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                                parentIDs, organizationIDs, groupIDs, planIDs)
                                               .ConfigureAwait(false) :
                                               await adsHelper.ReadDailyClaimSearchResultsByOtherID(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                               .ConfigureAwait(false);
                            }

                            if (otherIdExists == "APSENH")
                            {
                                output = limitedAccess ? 
                                                await adsHelper.ReadDailyClaimSearchResultsByCardIDCardID2inAPSENH_LimitedAccess(
                                                   clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                                   parentIDs, organizationIDs, groupIDs, planIDs)
                                               .ConfigureAwait(false) :
                                               await adsHelper.ReadDailyClaimSearchResultsByCardIDCardID2inAPSENH(
                                                   clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                               .ConfigureAwait(false);
                            }
                        }
                        else
                        {
                            List<ClaimSearchDTO> otherResult = limitedAccess ? 
                                            await adsHelper
                                           .ReadDailyClaimSearchResultsByOtherID_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                           parentIDs, organizationIDs, groupIDs, planIDs)
                                           .ConfigureAwait(false) :
                                           await adsHelper
                                           .ReadDailyClaimSearchResultsByOtherID(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                           .ConfigureAwait(false);

                            List<ClaimSearchDTO> enhResult = limitedAccess ?
                                            await adsHelper.ReadDailyClaimSearchResultsByCardIDCardID2inAPSENH_LimitedAccess(
                                               clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                               parentIDs, organizationIDs, groupIDs, planIDs)
                                           .ConfigureAwait(false) :
                                           await adsHelper.ReadDailyClaimSearchResultsByCardIDCardID2inAPSENH(
                                               clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                           .ConfigureAwait(false);

                            int totalCount = otherResult.Count + enhResult.Count;

                            output = otherResult;

                            //Add only results that weren't found by previous search
                            enhResult = enhResult.Where(x => !output.Select(y => y.ClaimNumber).Contains(x.ClaimNumber)).ToList();
                            output.AddRange(enhResult);

                            removedDuplicates = totalCount - output.Count;

                        }

                        break;

                    case Enums.MemberIDType.CardIDCardID2:

                        output = limitedAccess ? 
                                        await adsHelper.ReadDailyClaimSearchResultsByCardIDCardID2_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                        parentIDs, organizationIDs, groupIDs, planIDs)
                                                                                           .ConfigureAwait(false) :
                                        await adsHelper.ReadDailyClaimSearchResultsByCardIDCardID2(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                                                                           .ConfigureAwait(false);
                        break;
                }
            }


            return new Tuple<List<ClaimSearchDTO>, int>(output, removedDuplicates);
        }

        public async Task<Tuple<List<ClaimSearchDTO>, int>> GetPaidClaimSearchResults(string adsConnectionString, string clientName,
                                                                          Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                          DateTime fillDateFrom, DateTime fillDateTo, bool lookupAllEnrolleesForMember,
                                                                          List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            var adsHelper = new ClaimDataAccess(new AdsHelper(adsConnectionString));
            List<ClaimSearchDTO> output = new List<ClaimSearchDTO>();
            int removedDuplicates = 0;

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            //Member ID search
            if (!string.IsNullOrWhiteSpace(memberId))
            {
                switch (memberIdType)
                {
                    case Enums.MemberIDType.OtherID:

                        if (!lookupAllEnrolleesForMember)
                        {
                            string otherIdExists = await adsHelper.ReadOtherIDExists(memberId, memberIdOperator).ConfigureAwait(false);

                            if (otherIdExists == "APSENR")
                            {
                                output = limitedAccess ? 
                                                await adsHelper.ReadPaidClaimSearchResultsByOtherID_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                                parentIDs, organizationIDs, groupIDs, planIDs)
                                               .ConfigureAwait(false) :
                                               await adsHelper.ReadPaidClaimSearchResultsByOtherID(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                               .ConfigureAwait(false);
                            }

                            if (otherIdExists == "APSENH")
                            {
                                output = limitedAccess ? 
                                                await adsHelper.ReadPaidClaimSearchResultsByCardIDCardID2inAPSENH_LimitedAccess(
                                                   clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                                   parentIDs, organizationIDs, groupIDs, planIDs)
                                               .ConfigureAwait(false) :
                                               await adsHelper.ReadPaidClaimSearchResultsByCardIDCardID2inAPSENH(
                                                   clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                               .ConfigureAwait(false);
                            }
                        }
                        else
                        {
                            List<ClaimSearchDTO> otherResult = limitedAccess ? 
                                            await adsHelper.ReadPaidClaimSearchResultsByOtherID_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                            parentIDs, organizationIDs, groupIDs, planIDs)
                                           .ConfigureAwait(false) :
                                           await adsHelper.ReadPaidClaimSearchResultsByOtherID(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                           .ConfigureAwait(false);

                            List<ClaimSearchDTO> enhResult = limitedAccess ? 
                                            await adsHelper.ReadPaidClaimSearchResultsByCardIDCardID2inAPSENH_LimitedAccess(
                                               clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                               parentIDs, organizationIDs, groupIDs, planIDs)
                                           .ConfigureAwait(false) :
                                           await adsHelper.ReadPaidClaimSearchResultsByCardIDCardID2inAPSENH(
                                               clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                           .ConfigureAwait(false);

                            int totalCount = otherResult.Count + enhResult.Count;

                            output = otherResult;

                            //Add only results that weren't found by previous search
                            enhResult = enhResult.Where(x => !output.Select(y => y.ClaimNumber).Contains(x.ClaimNumber)).ToList();
                            output.AddRange(enhResult);

                            removedDuplicates = totalCount - output.Count;
                        }

                        break;
                    case Enums.MemberIDType.CardIDCardID2:

                        output = limitedAccess ? 
                                        await adsHelper.ReadPaidClaimSearchResultsByCardIDCardID2_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                        parentIDs, organizationIDs, groupIDs, planIDs)
                                                .ConfigureAwait(false) :
                                        await adsHelper.ReadPaidClaimSearchResultsByCardIDCardID2(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                                .ConfigureAwait(false);

                        break;
                }
            }

            return new Tuple<List<ClaimSearchDTO>, int>(output, removedDuplicates);
        }

        public async Task<int> GetDailyClaimSearchResultsCount(string adsConnectionString, string clientName,
                                                                    Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                    DateTime fillDateFrom, DateTime fillDateTo, bool lookupAllEnrolleesForMember,
                                                                    List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            var adsHelper = new ClaimDataAccess(new AdsHelper(adsConnectionString));
            int output = 0;

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            //FillDate and Member ID search
            if (!string.IsNullOrWhiteSpace(memberId))
            {
                switch (memberIdType)
                {
                    case Enums.MemberIDType.OtherID:

                        if (!lookupAllEnrolleesForMember)
                        {
                            string otherIdExists = await adsHelper.ReadOtherIDExists(memberId, memberIdOperator).ConfigureAwait(false);

                            if (otherIdExists == "APSENR")
                            {
                                output = limitedAccess ? 
                                                await adsHelper.ReadDailyClaimSearchResultsCountByOtherID_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom,
                                                                                          fillDateTo,
                                                                                          parentIDs, organizationIDs, groupIDs, planIDs)
                                               .ConfigureAwait(false) :
                                               await adsHelper.ReadDailyClaimSearchResultsCountByOtherID(clientName, memberId, memberIdOperator, fillDateFrom,
                                                                                          fillDateTo)
                                               .ConfigureAwait(false);
                            }

                            if (otherIdExists == "APSENH")
                            {
                                output = limitedAccess ? 
                                                await adsHelper.ReadDailyClaimSearchResultsCountByCardIDCardID2inAPSENH_LimitedAccess(
                                                   clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                                   parentIDs, organizationIDs, groupIDs, planIDs)
                                               .ConfigureAwait(false) :
                                               await adsHelper.ReadDailyClaimSearchResultsCountByCardIDCardID2inAPSENH(
                                                   clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                               .ConfigureAwait(false);
                            }
                        }
                        else
                        {
                            int otherResult = limitedAccess ? 
                                            await adsHelper.ReadDailyClaimSearchResultsCountByOtherID_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom,
                                                                                      fillDateTo,
                                                                                      parentIDs, organizationIDs, groupIDs, planIDs)
                                           .ConfigureAwait(false) :
                                           await adsHelper.ReadDailyClaimSearchResultsCountByOtherID(clientName, memberId, memberIdOperator, fillDateFrom,
                                                                                      fillDateTo)
                                           .ConfigureAwait(false);

                            int enhResult = limitedAccess ? 
                                            await adsHelper.ReadDailyClaimSearchResultsCountByCardIDCardID2inAPSENH_LimitedAccess(
                                               clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                               parentIDs, organizationIDs, groupIDs, planIDs)
                                           .ConfigureAwait(false) :
                                           await adsHelper.ReadDailyClaimSearchResultsCountByCardIDCardID2inAPSENH(
                                               clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                           .ConfigureAwait(false);

                            output = otherResult + enhResult;
                        }

                        break;
                    case Enums.MemberIDType.CardIDCardID2:

                        output = limitedAccess ? 
                                        await adsHelper.ReadDailyClaimSearchResultsCountByCardIDCardID2_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                        parentIDs, organizationIDs, groupIDs, planIDs)
                                                .ConfigureAwait(false) :
                                        await adsHelper.ReadDailyClaimSearchResultsCountByCardIDCardID2(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                                .ConfigureAwait(false);
                        break;
                }
            }

            return output;
        }

        public async Task<int> GetPaidClaimSearchResultsCount(string adsConnectionString, string clientName,
                                                                          Enums.MemberIDType memberIdType, string memberId, string memberIdOperator,
                                                                          DateTime fillDateFrom, DateTime fillDateTo, bool lookupAllEnrolleesForMember,
                                                                          List<string> parentIDs, List<string> organizationIDs, List<string> groupIDs, List<string> planIDs)
        {
            var adsHelper = new ClaimDataAccess(new AdsHelper(adsConnectionString));
            int output = 0;

            bool limitedAccess = ClientConfigHelper.UseLimitedAccess(parentIDs, organizationIDs, groupIDs, planIDs);

            //FillDate and Member ID search
            if (!string.IsNullOrWhiteSpace(memberId))
            {
                switch (memberIdType)
                {
                    case Enums.MemberIDType.OtherID:

                        if (!lookupAllEnrolleesForMember)
                        {
                            string otherIdExists = await adsHelper.ReadOtherIDExists(memberId, memberIdOperator).ConfigureAwait(false);

                            if (otherIdExists == "APSENR")
                            {
                                output = limitedAccess ? 
                                                await adsHelper.ReadPaidClaimSearchResultsCountByOtherID_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom,
                                                                                         fillDateTo,
                                                                                         parentIDs, organizationIDs, groupIDs, planIDs)
                                               .ConfigureAwait(false) :
                                               await adsHelper.ReadPaidClaimSearchResultsCountByOtherID(clientName, memberId, memberIdOperator, fillDateFrom,
                                                                                         fillDateTo)
                                               .ConfigureAwait(false);
                            }

                            if (otherIdExists == "APSENH")
                            {
                                output = limitedAccess ? 
                                                await adsHelper.ReadPaidClaimSearchResultsCountByCardIDCardID2inAPSENH_LimitedAccess(
                                                   clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                                   parentIDs, organizationIDs, groupIDs, planIDs)
                                               .ConfigureAwait(false) :
                                               await adsHelper.ReadPaidClaimSearchResultsCountByCardIDCardID2inAPSENH(
                                                   clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                               .ConfigureAwait(false);
                            }
                        }
                        else
                        {
                            int otherResult = limitedAccess ? 
                                                    await adsHelper.ReadPaidClaimSearchResultsCountByOtherID_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom,
                                                                                              fillDateTo,
                                                                                              parentIDs, organizationIDs, groupIDs, planIDs)
                                                    .ConfigureAwait(false) :
                                                    await adsHelper.ReadPaidClaimSearchResultsCountByOtherID(clientName, memberId, memberIdOperator, fillDateFrom,
                                                                                              fillDateTo)
                                                    .ConfigureAwait(false);

                            int enhResult = limitedAccess ? 
                                                  await adsHelper.ReadPaidClaimSearchResultsCountByCardIDCardID2inAPSENH_LimitedAccess(
                                                      clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                                      parentIDs, organizationIDs, groupIDs, planIDs)
                                                  .ConfigureAwait(false) :
                                                  await adsHelper.ReadPaidClaimSearchResultsCountByCardIDCardID2inAPSENH(
                                                      clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                                  .ConfigureAwait(false);

                            output = otherResult + enhResult;
                        }

                        break;

                    case Enums.MemberIDType.CardIDCardID2:

                        output = limitedAccess ? 
                                        await adsHelper.ReadPaidClaimSearchResultsCountByCardIDCardID2_LimitedAccess(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo,
                                        parentIDs, organizationIDs, groupIDs, planIDs)
                                                .ConfigureAwait(false) :
                                        await adsHelper.ReadPaidClaimSearchResultsCountByCardIDCardID2(clientName, memberId, memberIdOperator, fillDateFrom, fillDateTo)
                                                .ConfigureAwait(false);
                        break;
                }
            }

            return output;
        }
        #endregion Claim Search

        #region Claim Details

        /// <summary>
        ///  Get details for a daily paid claim in the database.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="claimNumber">String representing the Claim Number of the claim to retrieve details for</param>
        /// <returns><see cref="PaidClaimDetailsDTO" /> representing the details of the claim</returns>
        public async Task<PaidClaimDetailsDTO> GetPaidClaimDailyDetails(string adsConnectionString, string clientName, string claimNumber)
        {
            var adsHelper = new ClaimDataAccess(new AdsHelper(adsConnectionString));
            PaidClaimDetailsDTO output = await adsHelper.ReadPaidClaimDailyDetailsByNDCREF(clientName, claimNumber)
                                    .ConfigureAwait(false);

            if (output.Claim != null)
            {
                MemberAccumulatorDTO accums = await adsHelper.ReadMemberAccumulatorsByEnrIdPlnIdEffDt(
                    output.ActualMember.EnrolleeID, output.ActualMember.PlanID,
                    output.PlanInformation.PlanEffectiveDate);

                output.IndividualAccumulationInformation.Benefit = accums.YTDDOLLAR;
                output.FamilyAccumulationInformation.Benefit = accums.SUB_YTDDOLLAR;
            }

            return output;
        }

        /// <summary>
        ///  Get details for a historical paid claim in the database.
        /// </summary>
        /// <param name="adsConnectionString">String representing the client dataset to query</param>
        /// <param name="clientName">String representing the name of the client dataset being searched</param>
        /// <param name="claimNumber">String representing the Claim Number of the claim to retrieve details for</param>
        /// <returns><see cref="PaidClaimDetailsDTO" /> representing the details of the claim</returns>
        public async Task<PaidClaimDetailsDTO> GetPaidClaimHistoryDetails(string adsConnectionString, string clientName, string claimNumber)
        {
            var adsHelper = new ClaimDataAccess(new AdsHelper(adsConnectionString));
            PaidClaimDetailsDTO output = await adsHelper.ReadPaidClaimHistoryDetailsByNDCREF(clientName, claimNumber)
                                                              .ConfigureAwait(false);

            if (output.Claim != null)
            {
                MemberAccumulatorDTO accums = await adsHelper.ReadMemberAccumulatorsByEnrIdPlnIdEffDt(
                    output.ActualMember.EnrolleeID, output.ActualMember.PlanID,
                    output.PlanInformation.PlanEffectiveDate);

                output.IndividualAccumulationInformation.Benefit = accums.YTDDOLLAR;
                output.FamilyAccumulationInformation.Benefit = accums.SUB_YTDDOLLAR;
            }

            return output;
        }

        #endregion Claim Details

        #endregion

        #region "Fee Schedule"

        public async Task<FeeScheduleDTO> GetFeeScheduleByNDCREF(string adsConnectionString, string ndcRef)
        {
            var adsHelper = new FeeScheduleDataAccess(new AdsHelper(adsConnectionString));
            var dto = await adsHelper.GetByNDCREF(ndcRef);
            return dto;
        }

        #endregion

        #region Lookup
        public ClientSiteConfigurationDTO GetClientSiteConfiguration(int ClientSiteConfigurationID, string adsConnectionString)
        {
            var lookupDataAccess = new LookupDataAccess(new AdsHelper(adsConnectionString));
            ClientSiteConfigurationDTO results = lookupDataAccess.GetClientSiteConfiguration(ClientSiteConfigurationID);
            return results;
        }

        public List<CodedEntityDTO> ReadCodedEntities(int codedEntityTypeID, string connectionString)
        {
            var lookupDataAccess = new LookupDataAccess(new AdsHelper(connectionString));
            List <CodedEntityDTO> results = lookupDataAccess.ReadCodedEntities(codedEntityTypeID);
            return results;
        }
        #endregion

    }
}