using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class HospiceDataAccess : DataAccessBase
    {
        #region Constructors

        public HospiceDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        ///  Saves a submitted import record.
        /// </summary>
        /// <param name="vendorID">Integer representing the ProCare identifier of the vendor submitting the import</param>
        /// <param name="importRecord">String representing the raw record to be imported</param>
        /// <returns><see cref="long" /> representing the identifier of the created import record</returns>
        public async Task<long> InsertImportRecord(int vendorID, string importRecord, int status, long? patientId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@VendorID",  vendorID},
                {"@RawData", importRecord },
                {"@ImportStatusID", status },
                {"@PatientID", patientId }
            };

            Task<long> t = Task.Run(() =>
            {
                long dbResult = 0;
                DataHelper.ExecuteReader("apiPBM_Hospice_Import_insert", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt64orDefault("ImportID");
                });

                return dbResult;
            });

            long result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Saves a submitted import record.
        /// </summary>
        /// <param name="vendorID">Integer representing the ProCare identifier of the vendor submitting the import</param>
        /// <param name="importRecord">String representing the raw record to be imported</param>
        /// <returns><see cref="long" /> representing the identifier of the created import record</returns>
        public async Task<long> InsertPreImportRecord(long? importId, int externalPatientId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ImportId",  importId},
                {"@ExternalPatientId", externalPatientId }
            };

            Task<long> t = Task.Run(() =>
            {
                long dbResult = 0;
                DataHelper.ExecuteReader("apiPBM_PreImport_PreImportNetsmartSOTHPatientData_Insert", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt64orDefault("PreImportNetSmartSOTHPatientDataID");
                });

                return dbResult;
            });

            long result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Saves a submitted pre-import patient into the medication queue.
        /// </summary>
        /// <param name="preImportPatientDataId">the patientDataId to insert</param>
        /// <returns><see cref="long" /> representing the identifier of the created import record</returns>
        public async Task<long> InsertPreImportMedicationQueueRecord(long preImportPatientDataId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PreImportNetsmartSOTHPatientDataID", preImportPatientDataId }
            };

            Task<long> t = Task.Run(() =>
            {
                long dbResult = 0;
                DataHelper.ExecuteReader("apiPBM_PreImport_PreImportNetsmartSOTHMedQueue_Insert", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt64orDefault("PreImportNetsmartSOTHMedQueueID");
                });

                return dbResult;
            });

            long result = await t.ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///  Saves a submitted pre-import patient into the medication queue.
        /// </summary>
        /// <param name="preImportPatientDataId">the patientDataId to insert</param>
        /// <returns><see cref="long" /> representing the identifier of the created import record</returns>
        public async Task<long> GetPatientIdByExternalId(long externalId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ExternalPatientID", externalId }
            };

            Task<long> t = Task.Run(() =>
            {
                long dbResult = 0;
                DataHelper.ExecuteReader("apiPBM_PreImport_NetsmartSOTH_GetPatientIdByExternalId", CommandType.StoredProcedure, parameters, reader =>
                {
                    dbResult = reader.GetInt64orDefault("PatientID");
                });

                return dbResult;
            });

            long result = await t.ConfigureAwait(false);

            return result;
        }

        #endregion Public Methods
    }
}
