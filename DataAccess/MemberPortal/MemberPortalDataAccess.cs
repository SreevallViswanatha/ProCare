using System;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ProCare.API.PBM.Repository.DTO.MemberPortal;
using ProCare.API.PBM.Repository.DTO.Drug;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class MemberPortalDataAccess : DataAccessBase
    {
        #region Constructors

        public MemberPortalDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

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
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  clientID},
                {"@LoginName",  username},
                {"@Password",  password}
            };

            Task<MemberPortalUserDTO> t = Task.Run(() =>
            {
                MemberPortalUserDTO dbResult = new MemberPortalUserDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_login", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            MemberPortalUserDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Looks up enrollee information needed to create a login token by ClientID and EnrolleeID
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="enrolleeID">String representing the ProCare identifier of the enrollee</param>
        /// <returns><see cref="List{MemberPortalSSOValidationDTO}" /> representing the list of corresponding enrollees</returns>
        public async Task<List<MemberPortalSSOValidationDTO>> GetMembers(int clientID, string enrolleeID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  clientID},
                {"@ENRID",  enrolleeID}
            };

            Task<List<MemberPortalSSOValidationDTO>> t = Task.Run(() =>
            {
                List<MemberPortalSSOValidationDTO> dbResult = new List<MemberPortalSSOValidationDTO>();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_getMemberSSOValidationFields_byClientIDENRID", CommandType.StoredProcedure, parameters, reader =>
                {
                    MemberPortalSSOValidationDTO dto = new MemberPortalSSOValidationDTO();
                    dto.LoadFromDataReader(reader);
                    dbResult.Add(dto);
                });

                return dbResult;
            });

            List<MemberPortalSSOValidationDTO> result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Generates a new login token or returns the current login token for the user.
        /// </summary>
        /// <param name="cardID">String representing first 9 characters of the enrollee's card identifier</param>
        /// <param name="cardID2">String representing additional characters of the enrollee's card identifier</param>
        /// <param name="person">String representing the enrollee's person identifier</param>
        /// <param name="enrolleeID">String representing the ProCare identifier of the enrollee</param>
        /// <param name="binNumber">String representing the enrollee's BIN number</param>
        /// <param name="clientID">Integer representing the ProCare identifier of the client associated with the enrollee</param>
        /// <param name="domainID">Integer representing the ProCare identifier of the domain associated with the enrollee</param>
        /// <returns><see cref="ValidUserLoginDTO" /> representing the card information and current login token for the user</returns>
        public async Task<ValidUserLoginDTO> Login(string cardID, string cardID2, string person, string enrolleeID,
                                                     string binNumber, int clientID, int domainID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@CardID",  cardID},
                {"@CardID2",  cardID2},
                {"@Person",  person},
                {"@ENRID",  enrolleeID},
                {"@BIN",  binNumber},
                {"@ClientID",  clientID},
                {"@DomainID",  domainID}
            };

            Task<ValidUserLoginDTO> t = Task.Run(() =>
            {
                ValidUserLoginDTO dbResult = new ValidUserLoginDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_PRXUserSSO_insert", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            ValidUserLoginDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Read User Token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AutoLogonDTO> ReadUserSSO(string token)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@Token",  token}
            };

            Task<AutoLogonDTO> t = Task.Run(() =>
            {
                AutoLogonDTO dbResult = new AutoLogonDTO();

                DataHelper.ExecuteReader("mp_PRXUserSSO_Get", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            AutoLogonDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Looks up information for the client and default domain associated with the BIN
        /// </summary>
        /// <param name="binNumber">String representing the BIN to lookup</param>
        /// <returns><see cref="ClientInfoDTO" /> representing the associated client and default domain</returns>
        public async Task<ClientInfoDTO> LookupClientInfo(string binNumber)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@BinNumber",  binNumber}
            };

            Task<ClientInfoDTO> t = Task.Run(() =>
            {
                ClientInfoDTO dbResult = new ClientInfoDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_getClientInformationByBinNumber", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            ClientInfoDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Lookup the User Access Info
        /// </summary>
        /// <param name="enrolleeID"></param>
        /// <returns></returns>
        public async Task<UserInfoDTO> LookupUserInfo(string enrolleeID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EnrolleeID",  enrolleeID}
            };

            Task<UserInfoDTO> t = Task.Run(() =>
            {
                UserInfoDTO dbResult = new UserInfoDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupUser", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            UserInfoDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Lookup the User Access Info filter by UserID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<UserInfoDTO> LookupUserInfo(int userID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserID",  userID}
            };

            Task<UserInfoDTO> t = Task.Run(() =>
            {
                UserInfoDTO dbResult = new UserInfoDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupUserByUser", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            UserInfoDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Insert Log and Add 1 LogIn Count.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task LoginSuccessful(int userID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserID",  userID}
            };

            Task t = Task.Run(() =>
            {
                DataHelper.ExecuteNonQuery("apiPBM_MemberPortal_loginSuccessful", CommandType.StoredProcedure, parameters);
            });

            await t.ConfigureAwait(false);
        }

        /// <summary>
        ///  Looks up the DomainID associated with the domain name
        /// </summary>
        /// <param name="domainName">String representing the BIN to lookup</param>
        /// <returns><see cref="int" /> representing the associated DomainID</returns>
        public async Task<int> LookupDomainID(string domainName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@DomainName",  domainName}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupDomainID", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt32orDefault("DomainID");
                });

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Looks up the ClientID associated with the domain name
        /// </summary>
        /// <param name="domainName">String representing the BIN to lookup</param>
        /// <returns><see cref="int" /> representing the associated ClientID</returns>
        public async Task<int> LookupClientID(string domainName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@DomainName",  domainName}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupClientID", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt32orDefault("ClientID");
                });

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Verifies whether online access is allowed for the BIN
        /// </summary>
        /// <param name="binNumber">String representing the BIN to verify</param>
        /// <returns><see cref="bool" /> representing whether online access is allowed for the BIN</returns>
        public async Task<bool> BINAllowsOnlineAccess(string binNumber)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@BinNumber",  binNumber}
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbResult = false;

                DataHelper.ExecuteReader("apiPBM_MemberPortal_verifyBINAllowsAccess", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetBooleanSafe("AllowAccess");
                });

                return dbResult;
            });

            bool result = await t.ConfigureAwait(false);

            return result;
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
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@CardID",  cardID},
                {"@DOB",  dateOfBirth},
                {"@Sex",  gender},
                {"@ClientID",  clientID},
                {"@FilterPARID",  parentID},
                {"@FilterORGID",  orgID},
                {"@FilterGRPID",  groupID},
                {"@FilterCLASS",  classID}
            };

            Task<List<MemberPortalEnrolleeDTO>> t = Task.Run(() =>
            {
                List<MemberPortalEnrolleeDTO> dbResult = new List<MemberPortalEnrolleeDTO>();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupEnrollee", CommandType.StoredProcedure, parameters, reader =>
                {
                    MemberPortalEnrolleeDTO dto = new MemberPortalEnrolleeDTO();
                    dto.LoadFromDataReader(reader);
                    dbResult.Add(dto);
                });

                return dbResult;
            });

            List<MemberPortalEnrolleeDTO> result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get the Enrolle Info filter by EnrolleeID.
        /// </summary>
        /// <param name="enrolleeID"></param>
        /// <returns></returns>
        public async Task<List<MemberPortalEnrolleeDTO>> LookupEnrollee(string enrolleeID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EnrolleeID",  enrolleeID}
            };

            Task<List<MemberPortalEnrolleeDTO>> t = Task.Run(() =>
            {
                List<MemberPortalEnrolleeDTO> dbResult = new List<MemberPortalEnrolleeDTO>();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupEnrolleeByID", CommandType.StoredProcedure, parameters, reader =>
                {
                    MemberPortalEnrolleeDTO dto = new MemberPortalEnrolleeDTO();
                    dto.LoadFromDataReader(reader);
                    dbResult.Add(dto);
                });

                return dbResult;
            });

            List<MemberPortalEnrolleeDTO> result = await t.ConfigureAwait(false);

            return result;
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
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  clientID},
                {"@PLNID",  planID},
                {"@TYPE",  planType},
            };

            Task<PlanLookupDTO> t = Task.Run(() =>
            {
                PlanLookupDTO dbResult = new PlanLookupDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupPlan", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            PlanLookupDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Lookup a group by ID
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="groupID">String representing the identifier of the group</param>
        /// <returns><see cref="GroupLookupDTO" /> representing the matching group</returns>
        public async Task<GroupLookupDTO> LookupGroup(int clientID, string groupID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  clientID},
                {"@GRPID",  groupID}
            };

            Task<GroupLookupDTO> t = Task.Run(() =>
            {
                GroupLookupDTO dbResult = new GroupLookupDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupGroup", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            GroupLookupDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Lookup a organization by ID
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="orgID">String representing the identifier of the organization</param>
        /// <returns><see cref="OrgLookupDTO" /> representing the matching organization</returns>
        public async Task<OrgLookupDTO> LookupOrg(int clientID, string orgID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  clientID},
                {"@ORGID",  orgID}
            };

            Task<OrgLookupDTO> t = Task.Run(() =>
            {
                OrgLookupDTO dbResult = new OrgLookupDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupOrg", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            OrgLookupDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Lookup a parent by ID
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="parentID">String representing the identifier of the parent</param>
        /// <returns><see cref="ParentLookupDTO" /> representing the matching parent</returns>
        public async Task<ParentLookupDTO> LookupParent(int clientID, string parentID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  clientID},
                {"@PARID",  parentID}
            };

            Task<ParentLookupDTO> t = Task.Run(() =>
            {
                ParentLookupDTO dbResult = new ParentLookupDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_lookupParent", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            ParentLookupDTO result = await t.ConfigureAwait(false);

            return result;
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
        public async Task<int> InsertMember(string emailAddress, string password, string username, string binNumber, string enrolleeID, string question,
                                         string answer, int clientID, int domainID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@EmailAddress",  emailAddress},
                {"@Password",  password},
                {"@LoginName",  username},
                {"@ENRID",  enrolleeID},
                {"@Question",  question},
                {"@Answer",  answer},
                {"@ClientID",  clientID},
                {"@BinNumber",  binNumber},
                {"@DomainID",  domainID}
            };

            Task<int> t = Task.Run(() =>
            {
                int dbResult = 0;

                DataHelper.ExecuteReader("apiPBM_MemberPortal_User_insert", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt32orDefault("UserID");
                });

                return dbResult;
            });

            int result = await t.ConfigureAwait(false);

            return result;
        }

        /*
        public async Task<bool> MemberPasswordChange(int userID, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserID",  userID},
                {"@Password",  password}
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbResult = false;

                //Update User Password
                DataHelper.ExecuteReader("apiPBM_MemberPortal_PasswordChange", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetBooleanSafe("IsUpdated");
                });

                return dbResult;
            });

            bool result = await t.ConfigureAwait(false);

            return result;
        }

        public async Task<bool> MemberPasswordReset(int userID, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserID",  userID},
                {"@Password",  password}
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbResult = false;

                //Update User Password
                DataHelper.ExecuteReader("apiPBM_MemberPortal_PasswordReset", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetBooleanSafe("IsUpdated");
                });

                return dbResult;
            });

            bool result = await t.ConfigureAwait(false);

            return result;
        }*/

        /// <summary>
        ///  Updates an existing password for Member in MemberPortal
        /// </summary>
        /// <param name="@UserID">Integer representing the identifier of the member</param>
        /// <param name="@Password">String representing the new password of the of the member</param>
        /// <returns><see cref="bool" /> representing whether the password was updated or not</returns>
        public async Task<bool> MemberUpdatePassword(int userID, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserID",  userID},
                {"@Password",  password}
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbResult = false;

                //Update User Password
                DataHelper.ExecuteReader("apiPBM_MemberPortal_UpdatePassword", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetBooleanSafe("IsUpdated");
                });

                return dbResult;
            });

            bool result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Verify whether an enrollee has previously been registered for Member Portal access
        /// </summary>
        /// <param name="clientID">Integer representing the identifier of the client</param>
        /// <param name="enrolleeID">String representing the ProCare identifier of the enrollee</param>
        /// <returns><see cref="bool" /> representing whether the enrollee has previously registered</returns>
        public async Task<bool> EnrolleePreviouslyRegistered(int clientID, string enrolleeID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  clientID},
                {"@ENRID",  enrolleeID}
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbResult = false;

                DataHelper.ExecuteReader("apiPBM_MemberPortal_verifyPreviouslyRegistered", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetBooleanSafe("PreviouslyRegistered");
                });

                return dbResult;
            });

            bool result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Verify whether a Member Portal username is available for registration
        /// </summary>
        /// <param name="username">String representing the username of the Member Portal user</param>
        /// <returns><see cref="bool" /> representing whether the username is available</returns>
        public async Task<bool> UsernameAvailable(string username)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@Username",  username}
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbResult = false;

                DataHelper.ExecuteReader("apiPBM_MemberPortal_verifyUsernameAvailable", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetBooleanSafe("UsernameAvailable");
                });

                return dbResult;
            });

            bool result = await t.ConfigureAwait(false);

            return result;
        }
        /// <summary>
        /// Get PRX user Details
        /// </summary>
        /// <param name="domainID">Integer representing the Identifier of the Member Portal Domain</param>
        /// <param name="loginName">String representing the Member Portal login Name</param>       
        /// <returns></returns>
        public async Task<PRXUserDTO> GetPRXUserDetails(int domainID, string loginName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@DomainID",  domainID},
                {"@LoginName",  loginName},

            };

            Task<PRXUserDTO> t = Task.Run(() =>
            {
                PRXUserDTO dbResult = new PRXUserDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_getLoginDetails", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            PRXUserDTO result = await t.ConfigureAwait(false);

            return result;
        }
        /// <summary>
        /// Get PRX user Details for UpdatePassword
        /// </summary>
        /// <param name="domainID">Integer representing the Identifier of the Member Portal Domain</param>
        /// <param name="loginName">String representing the Member Portal login Name</param>
        /// <param name="oldPassword">String representing the Member Portal OldPassword</param>       
        /// <returns></returns>
        public async Task<PRXUserDTO> GetPRXUserDetailsWithPassword(int domainID, string loginName, string oldPassword)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@DomainID",  domainID},
                {"@LoginName",  loginName},
                {"@OldPassword",  oldPassword},
            };

            Task<PRXUserDTO> t = Task.Run(() =>
            {
                PRXUserDTO dbResult = new PRXUserDTO();

                DataHelper.ExecuteReader("apiPBM_MemberPortal_getLoginDetailsWithPassword", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            PRXUserDTO result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Save/Update Member Favourite Medications. This is a single medication for a single member.
        /// </summary>
        /// <param name="UserId">Integer representing the identifier of the Member Portal user.</param>
        /// <param name="MemberMedicationID">Long representing identifier of Member Medication of the Member Portal user</param>
        /// <param name="MemberMedicationTypeID">Integer representing the MemberMedicationType of the MemberMedication, indicating whether the record is a medication saved from the search or custom-added by the user</param>
        /// <param name="EntityIdentifier">Long representing, depending on MedicationTypeID, the NDC of a medication (MedicationTypeID = 1) or the identifer of a custom favorite medication that was created by the user (MedicationTypeID = 2)</param>
        /// <param name="MedicationName">String representing the favorite member medication name, depending on MedicationTypeID, if (MedicationTypeID = 2) custom favorite medication name will be created</param>
        /// <returns><see cref="long" />Representing  the Member Medication ID after save/update</returns>


        public async Task<long> SaveMemberFavoriteMedication(int UserId, long? MemberMedicationID, int MemberMedicationTypeID, long? EntityIdentifier, string MedicationName)
        {

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberMedicationID",  MemberMedicationID},
                {"@UserId",  UserId},
                {"@MemberMedicationTypeID",  MemberMedicationTypeID},
                {"@EntityIdentifier",  EntityIdentifier},
                {"@MedicationName",  MedicationName},
            };

            Task<long> t = Task.Run(() =>
            {
                long dbResult = 0;
                DataHelper.ExecuteReader("apiPBM_MemberFavorites_Medication_save", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt32orDefault("MemberMedicationID");

                });
                return dbResult;
            });
            long result = await t.ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// To check the token is valid or not
        /// </summary>
        /// <param name="Token">String representing the token for the PrxUSer.</param>
        /// <returns><see cref="PRXUserSSODetailsDTO" />Representing the User SSO Details</returns>

        public async Task<PRXUserSSODetailsDTO> SearchPRXUserSSOAPSENRPLN(string Token)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Token", Token }
            };

            Task<PRXUserSSODetailsDTO> t = Task.Run(() =>
            {
                PRXUserSSODetailsDTO dbResults = new PRXUserSSODetailsDTO();
                DataHelper.ExecuteReader("apiPBM_PRXUserSSO_getByToken", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResults.LoadFromDataReader(reader);
                });

                return dbResults;
            });

            PRXUserSSODetailsDTO results = await t.ConfigureAwait(false);

            return results;
        }
        /// <summary>
        /// Check ndc value is valid or not
        /// </summary>
        /// <param name="NDC">String representing the NDC for a MemberFavoriteMedication</param>
        /// <returns><see cref="bool" />Representing the NDC value is exist or not</returns>

        public async Task<bool> NDCExists(string NDC)
        {
            string strNDC = string.Format("{0:00000000000}", Convert.ToInt64(NDC));

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@NDC",  strNDC }
            };

            Task<bool> t = Task.Run(() =>
            {
                bool dbResult = false;
                DataHelper.ExecuteReader("apiPBM_RNDC14_read_byNDC", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetBooleanSafe("RecordExists");
                });

                return dbResult;
            });

            bool result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get Member Favourite Medications for particular MemberMedication ID and user ID
        /// </summary>
        /// <param name="MemberMedicationID">Long representing identifier of Member Medication of the Member Portal user</param>
        /// <param name="UserID">Integer representing the identifier of the Member Portal user.</param>
        /// <returns><see cref="MemberFavoriteMedicationDTO" />Representing the Member Favourite Medication</returns>
        public async Task<MemberFavoriteMedicationDTO> GetMemberFavoriteMedicationByMemberMedicationID(long MemberMedicationID, int UserID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@MemberMedicationID",  MemberMedicationID},
                {"@UserID",  UserID},
            };

            Task<MemberFavoriteMedicationDTO> t = Task.Run(() =>
            {
                MemberFavoriteMedicationDTO dbResult = new MemberFavoriteMedicationDTO();
                DataHelper.ExecuteReader("apiPBM_MemberFavorites_Medication_read_byMemberMedicationID", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });

                return dbResult;
            });

            MemberFavoriteMedicationDTO result = await t.ConfigureAwait(false);

            return result;
        }
        /// <summary>
        /// Get Member Favourite Medications for particular user ID
        /// </summary>
        /// <param name="UserId">Integer representing the identifier of the Member Portal user.</param>
        /// <returns><see cref="List{MemberFavoriteMedicationDTO}" />List of Member Favourite Medications</returns>
        public async Task<List<MemberFavoriteMedicationDTO>> GetMemberFavoriteMedicationByUserId(int UserId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserId",  UserId}
            };

            Task<List<MemberFavoriteMedicationDTO>> t = Task.Run(() =>
        {
            List<MemberFavoriteMedicationDTO> dbResult = new List<MemberFavoriteMedicationDTO>();
            DataHelper.ExecuteReader("apiPBM_MemberFavorites_Medication_read_byUserID", CommandType.StoredProcedure, parameters, reader =>
            {
                MemberFavoriteMedicationDTO memberMedications = new MemberFavoriteMedicationDTO();
                memberMedications.LoadFromDataReader(reader);
                dbResult.Add(memberMedications);
            });

            return dbResult;
        });
            List<MemberFavoriteMedicationDTO> result = await t.ConfigureAwait(false);
            return result;
        }

        /// <summary>
        ///  Deletes an existing Member Favorite Contact.
        /// </summary>
        /// <param name="userID">Integer representing the identifier of the User</param>
        /// <param name="memberContactID">Long representing the identifier of Member Favorite Contact</param>
        public async Task MemberFavoriteContactDelete(int userID, long memberContactID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserID",  userID},
                {"@MemberContactID",  memberContactID}
            };

            Task t = Task.Run(() =>
            {
                DataHelper.ExecuteNonQuery("apiPBM_MemberFavorites_Contact_delete", CommandType.StoredProcedure, parameters);
            });
            await t.ConfigureAwait(false);
        }


        /// <summary>
        ///  Deletes an existing MemberFavorite Medication.
        /// </summary>
        /// <param name="userID">Integer representing the identifier of the client</param>
        /// <param name="memberMedicationID">Long representing the identifier of Member Favorite Medication</param>
        public async Task MemberFavoriteMedicationDelete(int userID, long memberMedicationID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserID",  userID},
                {"@MemberMedicationID",  memberMedicationID}
            };

            Task t = Task.Run(() =>
            {
                DataHelper.ExecuteReader("apiPBM_MemberFavorites_Medication_delete", CommandType.StoredProcedure, parameters, reader =>
                {
                    reader.GetBooleanSafe("IsDeleted");
                });
            });

            await t.ConfigureAwait(false);

        }

        /// <summary>
        /// Returns if a record exists in APSPHA
        /// </summary>
        /// <param name="ClientID">Integer representing identifier of a PrxUser CleintId for a given Member Favorite Contact.</param>
        /// <param name="PHAID"> String representing identifier of a PrxUser PHAID for a given Pharmacy.</param>
        /// <returns><see cref="bool" />Returns true if a Pharmacy record exists in APSPHA table.</returns>
        public async Task<bool> PharmacyByClientIDPHAIDExists(int ClientID, string PHAID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  ClientID},
                {"@PHAID",  PHAID}
            };

            Task<bool> t = Task.Run(() =>
            {
                var dbResult = false;
                DataHelper.ExecuteReader("apiPBM_Pharmacy_Exists_byClientIDPHAID", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt32orDefault("RecordExists") == 1;
                });
                return dbResult;
            });

            var result = await t.ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// Reads UserID, MemberContactTypeID, EntityIdentifier, ContactName, ContactAddress, ContactPhoneNumber of a PrxUser when a MemberContactID is provided.
        /// </summary>
        /// <param name="ClientID">Integer representing identifier of a PrxUser CleintId for a given Member Favorite Contact.</param>
        /// <param name="MemberContactID"> Long representing identifier of a Member Favorite Contact for a Member Portal User.</param>
        /// <returns><see cref="Task{MemberFavoriteContactDTO}" />Returns Member Favorite Contact Information when a MemberContactID and ClientID are provided as inputs.</returns>
        public async Task<MemberFavoriteContactDTO> GetMemberFavoriteContactByMemberContactID(int ClientID, long MemberContactID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  ClientID},
                {"@MemberContactID",  MemberContactID}
            };

            Task<MemberFavoriteContactDTO> t = Task.Run(() =>
            {
                MemberFavoriteContactDTO dbResult = new MemberFavoriteContactDTO();
                DataHelper.ExecuteReader("apiPBM_MemberFavorites_Contact_read_byMemberContactID", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult.LoadFromDataReader(reader);
                });
                return dbResult;
            });

            MemberFavoriteContactDTO result = await t.ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Get Member Favourite Medications for particular user ID
        /// </summary>
        /// <param name="ClientID">Integer representing identifier of a PrxUser CleintId for a given Member Favorite Contact.</param>
        /// <param name="UserID">Integer representing the identifier of the Member Portal user.</param>
        /// <returns><see cref="List{MemberFavoriteContactDTO}" />List of Member Favourite Contacts</returns>
        public async Task<List<MemberFavoriteContactDTO>> GetMemberFavoriteContactsByUserID(int ClientID, long UserID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ClientID",  ClientID},
                {"@UserId",  UserID}
            };

            Task<List<MemberFavoriteContactDTO>> t = Task.Run(() =>
            {
                List<MemberFavoriteContactDTO> dbResult = new List<MemberFavoriteContactDTO>();
                DataHelper.ExecuteReader("apiPBM_MemberFavorites_Contact_read_byUserID", CommandType.StoredProcedure, parameters, reader =>
                {
                    MemberFavoriteContactDTO memberMedications = new MemberFavoriteContactDTO();
                    memberMedications.LoadFromDataReader(reader);
                    dbResult.Add(memberMedications);
                });

                return dbResult;
            });
            List<MemberFavoriteContactDTO> result = await t.ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// Reads MemberContactID, MemberContactTypeID, EntityIdentifier, ContactName, ContactAddress, ContactPhoneNumber of a PrxUser when a UserID of a Member Favorite Contact is provided.
        /// </summary>
        /// <param name="UserID"> Long representing identifier of a Member Favorite Contact for a Member Portal User.</param>
        /// <param name="MemberContactTypeID"> Integer representing identifier of a Member Favorite Contact Type for a Member Portal User.</param>
        /// <param name="EntityIdentifier"> String representing Entity identifier of a Member Favorite Contact for a Member Portal User.</param>
        /// <param name="ContactName"> String representing Name of a Member Favorite Contact for a Member Portal User.</param>
        /// <param name="ContactAddress"> String representing Address of a Member Favorite Contact for a Member Portal User.</param>
        /// <param name="ContactPhone"> String representing Phone of a Member Favorite Contact for a Member Portal User.</param>
        /// <param name="MemberContactID"> Long representing identifier of a Member Favorite Contact for a Member Portal User.</param>
        /// <returns><see cref="Task{MemberFavoriteContactSaveDTO}" />Saves new or updates an existing Member Contact information when a UserID, MemberContactTypeID, EntityIdentifier, ContactName, ClientID, ContactAddress, ContactPhone, MemberContactID are provided as inputs.</returns>


        public async Task<long> MemberFavoritesContactSave(int UserID, int MemberContactTypeID, string EntityIdentifier, string ContactName, string ContactAddress, string ContactPhone, long? MemberContactID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@UserID",  UserID},
                {"@MemberContactTypeID",  MemberContactTypeID},
                {"@EntityIdentifier",  EntityIdentifier},
                {"@ContactName",  ContactName},
                {"@ContactAddress",  ContactAddress},
                {"@ContactPhone",  ContactPhone},
                {"@MemberContactID",  MemberContactID}
            };

            Task<long> t = Task.Run(() =>
            {
                long dbResult = 0;
                DataHelper.ExecuteReader("apiPBM_MemberFavorites_Contact_save", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt64orDefault("MemberContactID");
                });
                return dbResult;
            });

            var result = await t.ConfigureAwait(false);
            return result;
        }


        #endregion Public Methods
    }
}
