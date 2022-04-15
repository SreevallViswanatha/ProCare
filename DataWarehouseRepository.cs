using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public class DataWarehouseRepository : BasedbRepository, IDataWarehouseRepository
    {
        #region Constructor

        public DataWarehouseRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        ///  Validates a user's login information and returns additional fields needed to look up enrollee information
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="username">String representing the Member Portal user's username</param>
        /// <param name="password">String representing the Member Portal user's encrypted password</param>
        /// <returns><see cref="MemberPortalUserDTO" /> representing the additional fields used to look up enrollee information on valid login</returns>
        public async Task<MemberPortalUserDTO> LookupUser(int clientID, string username, string password)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            MemberPortalUserDTO output = await sqlHelper.LookupUser(clientID, username, password).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Looks up enrollee information needed to create a login token by ClientID and EnrolleeID
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="enrolleeID">String representing the ProCare identifier of the enrollee</param>
        /// <returns><see cref="List{MemberPortalSSOValidationDTO}" /> representing the list of corresponding enrollees</returns>
        public async Task<List<MemberPortalSSOValidationDTO>> GetMembers(int clientID, string enrolleeID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            List<MemberPortalSSOValidationDTO> output = await sqlHelper.GetMembers(clientID, enrolleeID).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Looks up information for the client and default domain associated with the BIN
        /// </summary>
        /// <param name="binNumber">String representing the BIN to lookup</param>
        /// <returns><see cref="ClientInfoDTO" /> representing the associated client and default domain</returns>
        public async Task<ClientInfoDTO> LookupClientInfo(string binNumber)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            ClientInfoDTO output = await sqlHelper.LookupClientInfo(binNumber).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Look up User Access Info filter with EnrolleeID.
        /// </summary>
        /// <param name="enrolleeID"></param>
        /// <returns></returns>
        public async Task<UserInfoDTO> LookupUserInfo(string enrolleeID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            UserInfoDTO output = await sqlHelper.LookupUserInfo(enrolleeID).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Look up User Access Info filter with UserID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<UserInfoDTO> LookupUserInfo(int userID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            UserInfoDTO output = await sqlHelper.LookupUserInfo(userID).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Insert Log and Add 1 LogIn Count.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task LoginSuccessful(int userID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            await sqlHelper.LoginSuccessful(userID).ConfigureAwait(false);
        }

        /// <summary>
        ///  Looks up the DomainID associated with the domain name
        /// </summary>
        /// <param name="domainName">String representing the BIN to lookup</param>
        /// <returns><see cref="int" /> representing the associated DomainID</returns>
        public async Task<int> LookupDomainID(string domainName)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            int output = await sqlHelper.LookupDomainID(domainName).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Looks up the ClientID associated with the domain name
        /// </summary>
        /// <param name="domainName">String representing the BIN to lookup</param>
        /// <returns><see cref="int" /> representing the associated ClientID</returns>
        public async Task<int> LookupClientID(string domainName)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            int output = await sqlHelper.LookupClientID(domainName).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Verifies whether online access is allowed for the BIN
        /// </summary>
        /// <param name="binNumber">String representing the BIN to verify</param>
        /// <returns><see cref="bool" /> representing whether online access is allowed for the BIN</returns>
        public async Task<bool> BINAllowsOnlineAccess(string binNumber)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            bool output = await sqlHelper.BINAllowsOnlineAccess(binNumber).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Get a list of enrollees by enrollee details
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="domainID">Integer representing the identifier of the domain</param>
        /// <param name="cardID">String representing the enrollee identifier filter to use</param>
        /// <param name="dateOfBirth">DateTinme representing the enrollee date of birth filter to use</param>
        /// <param name="gender">String representing the sex filter to use</param>
        /// <param name="parentID">String representing the identifier of the Parent filter to use</param>
        /// <param name="orgID">String representing the identifier of the Organization filter to use</param>
        /// <param name="groupID">String representing the identifier of the Group filter to use</param>
        /// <param name="classID">String representing the identifier of the Class filter to use</param>
        /// <returns><see cref="List{MemberPortalEnrolleeDTO}" /> representing the list of enrollees matching the provided values</returns>
        public async Task<List<MemberPortalEnrolleeDTO>> LookupEnrollee(int clientID, int domainID, string cardID, DateTime dateOfBirth,
                                                                        string gender, string parentID, string orgID, string groupID, string classID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            List<MemberPortalEnrolleeDTO> output = await sqlHelper
                                                         .LookupEnrollee(clientID, domainID, cardID, dateOfBirth, gender, parentID, orgID, groupID,
                                                                         classID).ConfigureAwait(false);

            return output;
        }

        public async Task<List<MemberPortalEnrolleeDTO>> LookupEnrollee(string enrolleeID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            List<MemberPortalEnrolleeDTO> output = await sqlHelper.LookupEnrollee(enrolleeID).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Lookup a plan by plan ID and type
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="planID">String representing the identifier of the plan</param>
        /// <param name="planType">String representing the mail or retail type of the plan</param>
        /// <returns><see cref="PlanLookupDTO" /> representing the matching plan</returns>
        public async Task<PlanLookupDTO> LookupPlan(int clientID, string planID, string planType)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            PlanLookupDTO output = await sqlHelper.LookupPlan(clientID, planID, planType).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Lookup a group by ID
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="groupID">String representing the identifier of the group</param>
        /// <returns><see cref="GroupLookupDTO" /> representing the matching group</returns>
        public async Task<GroupLookupDTO> LookupGroup(int clientID, string groupID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            GroupLookupDTO output = await sqlHelper.LookupGroup(clientID, groupID).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Lookup a organization by ID
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="orgID">String representing the identifier of the organization</param>
        /// <returns><see cref="OrgLookupDTO" /> representing the matching organization</returns>
        public async Task<OrgLookupDTO> LookupOrg(int clientID, string orgID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            OrgLookupDTO output = await sqlHelper.LookupOrg(clientID, orgID).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Lookup a parent by ID
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="parentID">String representing the identifier of the parent</param>
        /// <returns><see cref="ParentLookupDTO" /> representing the matching parent</returns>
        public async Task<ParentLookupDTO> LookupParent(int clientID, string parentID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            ParentLookupDTO output = await sqlHelper.LookupParent(clientID, parentID).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Register a new Member Portal member
        /// </summary>
        /// <param name="emailAddress">String representing the users's email address</param>
        /// <param name="password">String representing the user's encrypted Member Portal password</param>
        /// <param name="username">String representing the users's Member Portal username</param>
        /// <param name="binNumber">String representing the user's BIN number</param>
        /// <param name="enrolleeID">String representing the ProCare identifier of the enrollee</param>
        /// <param name="question">String representing the user's Member Portal security question</param>
        /// <param name="answer">String representing the user's Member Portal security answer</param>
        /// <param name="clientID">Integer representing the ProCare identifier of the client associated with the enrollee</param>
        /// <param name="domainID">Integer representing the ProCare identifier of the domain associated with the enrollee</param>
        /// <returns><see cref="int" /> representing the identifier of the new Member Portal member</returns>
        public async Task<int> AddMember(string emailAddress, string password, string username, string binNumber, string enrolleeID, string question,
                                         string answer, int clientID, int domainID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            int output = await sqlHelper.InsertMember(emailAddress, password, username, binNumber, enrolleeID, question, answer, clientID, domainID)
                                        .ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Change the User Password Manually.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePassword(int userID, string password)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            bool output = await sqlHelper.MemberUpdatePassword(userID, password).ConfigureAwait(false);

            return output;
        }       

        /// <summary>
        ///  Verify whether an enrollee has previously been registered for Member Portal access
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="enrolleeID">String representing the ProCare identifier of the enrollee</param>
        /// <returns><see cref="bool" /> representing whether the enrollee has previously registered</returns>
        public async Task<bool> EnrolleePreviouslyRegistered(int clientID, string enrolleeID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            bool output = await sqlHelper.EnrolleePreviouslyRegistered(clientID, enrolleeID)
                                        .ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  Verify whether a Member Portal username is available for registration
        /// </summary>
        /// <param name="username">String representing the username of the Member Portal user</param>
        /// <returns><see cref="bool" /> representing whether the username is available</returns>
        public async Task<bool> UsernameAvailable(string username)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            bool output = await sqlHelper.UsernameAvailable(username)
                                         .ConfigureAwait(false);

            return output;
        }
        #endregion Public Methods
    }
}
