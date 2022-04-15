using System.Threading.Tasks;
using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using ProCare.API.PBM.Repository.DTO.MemberPortal;
using ProCare.Common.Data.SQL;
using System;
using ProCare.API.PBM.Repository.DTO.Drug;
using System.Collections.Generic;
using ProCare.API.PBM.Repository.DTO;

namespace ProCare.API.PBM.Repository
{
    public class MemberPortalRepository : BasedbRepository, IMemberPortalRepository
    {
        #region Constructor

        public MemberPortalRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }
        #endregion Constructor

        #region Public Methods
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
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            ValidUserLoginDTO output = await sqlHelper.Login(cardID, cardID2, person, enrolleeID, binNumber, clientID, domainID)
                                         .ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Read User Token
        /// </summary>
        /// <param name="token">String representing the token of the Member Portal User.</param>
        /// <returns></returns>
        public async Task<AutoLogonDTO> ReadUserSSO(string token)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            AutoLogonDTO output = await sqlHelper.ReadUserSSO(token).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        /// Get PRX user Details
        /// </summary>
        /// <param name="connectionString">String representing the connection string for the database.</param>
        /// <param name="domainID">Integer representing the Identifier of the Member Portal Domain</param>
        /// <param name="loginName">String representing the Member Portal login Name</param>       
        /// <returns></returns>
        public async Task<PRXUserDTO> GetPRXUserDetails(string connectionString, int domainID, string loginName)
        {
            IDataAccessHelper dh = new SQLHelper(connectionString);
            var sqlHelper = new MemberPortalDataAccess(dh);

            return await sqlHelper.GetPRXUserDetails(domainID, loginName).ConfigureAwait(false);
        }

        /// <summary>
        /// Get PRX user Details for UpdatePassword
        /// </summary>
        /// <param name="connectionString">String representing the connection string for the database.</param>
        /// <param name="domainID">Integer representing the Identifier of the Member Portal Domain</param>
        /// <param name="loginName">String representing the Member Portal login Name</param>
        /// <param name="oldPassword">String representing the Member Portal OldPassword</param>       
        /// <returns></returns>
        public async Task<PRXUserDTO> GetPRXUserDetailsforUpdatePassword(string connectionString, int domainID, string loginName, string oldPassword)
        {
            IDataAccessHelper dh = new SQLHelper(connectionString);
            var sqlHelper = new MemberPortalDataAccess(dh);

            return await sqlHelper.GetPRXUserDetailsWithPassword(domainID, loginName, oldPassword).ConfigureAwait(false);
        }

        /// <summary>
        /// Save/Update Member Favourite Medications. This is a single medication for a single member.
        /// </summary>
        /// <param name="connectionString">String representing the connection string used to connect the database.</param>
        /// <param name="UserId">Integer representing the identifier of the Member Portal user.</param>
        /// <param name="MemberMedicationID">Long representing identifier of Member Medication of the Member Portal user</param>
        /// <param name="MemberMedicationTypeID">Integer representing the MemberMedicationType of the MemberMedication, indicating whether the record is a medication saved from the search or custom-added by the user</param>
        /// <param name="EntityIdentifier">Long representing, depending on MedicationTypeID, the NDC of a medication (MedicationTypeID = 1) or the identifer of a custom favorite medication that was created by the user (MedicationTypeID = 2)</param>
        /// <param name="MedicationName">String representing the favorite member medication name, depending on MedicationTypeID, if (MedicationTypeID = 2) custom favorite medication name will be created</param>
        /// <returns><see cref="long" />Representing  the Member Medication ID after save/update</returns>
        public async Task<long> SaveMemberFavoriteMedication(string connectionString, int UserId, long? MemberMedicationID, int MemberMedicationTypeID, long? EntityIdentifier, string MedicationName)
        {
            IDataAccessHelper dh = new SQLHelper(connectionString);
            var sqlHelper = new MemberPortalDataAccess(dh);

            return await sqlHelper.SaveMemberFavoriteMedication(UserId, MemberMedicationID, MemberMedicationTypeID, EntityIdentifier, MedicationName).ConfigureAwait(false);
        }

        /// <summary>
        /// Check ndc value is valid or not
        /// </summary>
        /// <param name="connectionString">String representing the connection string used to connect the database.</param>
        /// <param name="NDC">String representing the NDC for a MemberFavoriteMedication</param>
        /// <returns><see cref="bool" />Representing the NDC value is exist or not</returns>
        public async Task<bool> NDCExists(string connectionString, string NDC)
        {
            IDataAccessHelper dh = new SQLHelper(connectionString);
            var sqlHelper = new MemberPortalDataAccess(dh);

            return await sqlHelper.NDCExists(NDC);
        }

        /// <summary>
        /// To check the token is valid or not
        /// </summary>
        /// <param name="connectionString">String representing the connection string used to connect the database.</param>
        /// <param name="Token">String representing the token for the PrxUSer.</param>
        /// <returns><see cref="PRXUserSSODetailsDTO" />Representing the User SSO Details</returns>
        public async Task<PRXUserSSODetailsDTO> GetPRXUserSSOAPSENRPLN(string connectionString, string Token)
        {
            IDataAccessHelper dh = new SQLHelper(connectionString);
            var sqlHelper = new MemberPortalDataAccess(dh);
            return await sqlHelper.SearchPRXUserSSOAPSENRPLN(Token).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Member Favourite Medications for particular MemberMedication ID and user ID
        /// </summary>
        /// <param name="connectionString">String representing the connection string used to connect the database.</param>
        /// <param name="MemberMedicationID">Long representing identifier of Member Medication of the Member Portal user</param>
        /// <param name="UserID">Integer representing the identifier of the Member Portal user.</param>
        /// <returns><see cref="MemberFavoriteMedicationDTO" />Representing the Member Favourite Medication</returns>
        public async Task<MemberFavoriteMedicationDTO> GetMemberFavoriteMedicationByMemberMedicationID(string connectionString, long MemberMedicationID, int UserID)
        {
            IDataAccessHelper dh = new SQLHelper(connectionString);
            var sqlHelper = new MemberPortalDataAccess(dh);
            return await sqlHelper.GetMemberFavoriteMedicationByMemberMedicationID(MemberMedicationID, UserID);
        }

        /// <summary>
        /// Get Member Favourite Medications for particular user ID
        /// </summary>
        /// <param name="connectionString">String representing the connection string used to connect the database.</param>
        /// <param name="UserId">Integer representing the identifier of the Member Portal user.</param>
        /// <returns><see cref="List{MemberFavoriteMedicationDTO}" />List of Member Favourite Medications</returns>
        public async Task<List<MemberFavoriteMedicationDTO>> GetMemberFavoriteMedicationByUserId(string connectionString, int UserId)
        {
            IDataAccessHelper dh = new SQLHelper(connectionString);
            var sqlHelper = new MemberPortalDataAccess(dh);
            return await sqlHelper.GetMemberFavoriteMedicationByUserId(UserId);

        }
        /// <summary>
        /// Delete Member Favorites for favorite medications
        /// </summary>
        /// <param name="userID">Integer representing the identifier of PrxUser for a given token</param>
        /// <param name="memberMedicationID"> Long representing identifier of a Member Favorite Medication of PrxUser.</param>
        /// <returns></returns>
        public async Task DeleteMemberFavoriteMedication(int userID, long memberMedicationID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            await sqlHelper.MemberFavoriteMedicationDelete(userID, memberMedicationID).ConfigureAwait(false);
        }

        /// <summary>
        ///  Deletes an existing Member Favorite Contact.
        /// </summary>
        /// <param name="userID">Integer representing the identifier of the User</param>
        /// <param name="memberContactID">Long representing the identifier of Member Favorite Contact</param>
        public async Task MemberFavoriteContactDelete(int userID, long memberContactID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            await sqlHelper.MemberFavoriteContactDelete(userID, memberContactID).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns if a record exists in APSPHA
        /// </summary>
        /// <param name="ClientID">Integer representing identifier of a PrxUser CleintId for a given Member Favorite Contact.</param>
        /// <param name="PHAID"> String representing identifier of a Member Portal User PHAID for a given Pharmacy.</param>
        /// <returns><see cref="bool" />Returns true if a Pharmacy record exists for the given Client</returns>
        public async Task<bool> PharmacyByClientIDPHAIDExists(int ClientID, string PHAID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            return await sqlHelper.PharmacyByClientIDPHAIDExists(ClientID, PHAID).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads UserID, MemberContactTypeID, EntityIdentifier, ContactName, ContactAddress, ContactPhoneNumber of a Member Portal User when a MemberContactID is provided.
        /// </summary>
        /// <param name="ClientID">Integer representing identifier of a Member Portal User CleintId for a given Member Favorite Contact.</param>
        /// <param name="MemberContactID"> Long representing identifier of a Member Favorite Contact for a Member Portal User.</param>
        /// <returns><see cref="Task{MemberFavoriteContactDTO}" />Returns Member Favorite Contact information when a MemberContactID and ClientID are provided as inputs.</returns>

        public async Task<MemberFavoriteContactDTO> GetMemberFavoriteContactByMemberContactID(int ClientID, long MemberContactID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            return await sqlHelper.GetMemberFavoriteContactByMemberContactID(ClientID, MemberContactID).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads MemberContactID, MemberContactTypeID, EntityIdentifier, ContactName, ContactAddress, ContactPhoneNumber of a Member Portal User when a UserID of a Member Favorite Contact is provided.
        /// </summary>
        /// <param name="ClientID">Integer representing identifier of a Member Portal User CleintId for a given Member Favorite Contact.</param>
        /// <param name="UserID"> Long representing identifier of a Member Favorite Contact for a Member Portal User.</param>
        /// <returns><see cref="Task{MemberFavoriteContactDTO}" />Returns Member Contact information when a UserID and ClientID are provided as inputs.</returns>

        public async Task<List<MemberFavoriteContactDTO>> GetMemberFavoriteContactsByUserID(int ClientID, int UserID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            return await sqlHelper.GetMemberFavoriteContactsByUserID(ClientID, UserID).ConfigureAwait(false);
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

        public async Task<long> SaveMemberFavoriteContact(int UserID, int MemberContactTypeID, string EntityIdentifier, string ContactName, string ContactAddress, string ContactPhone, long? MemberContactID)
        {
            var sqlHelper = new MemberPortalDataAccess(DataHelper);
            return await sqlHelper.MemberFavoritesContactSave(UserID, MemberContactTypeID, EntityIdentifier, ContactName, ContactAddress, ContactPhone, MemberContactID).ConfigureAwait(false);
        }
        #endregion Public Methods
    }
}
