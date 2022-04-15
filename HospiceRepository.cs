using ProCare.API.PBM.Repository.DataAccess;
using ProCare.Common.Data;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public class HospiceRepository : BasedbRepository, IHospiceRepository
    {
        #region Constructor

        public HospiceRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        ///  Saves a submitted import record.
        /// </summary>
        /// <param name="vendorID">Integer representing the ProCare identifier of the vendor submitting the import</param>
        /// <param name="importRecord">String representing the raw record to be imported</param>
        /// <returns><see cref="long" /> representing the identifier of the created import record</returns>
        public async Task<long> InsertImportRecord(int vendorID, string importRecord, int status, long? patientId)
        {
            var sqlHelper = new HospiceDataAccess(DataHelper);
            long output = await sqlHelper.InsertImportRecord(vendorID, importRecord, status, patientId).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  inserts a patient record into the pre-import table.
        /// </summary>
        /// <param name="importId">Integer representing the primary key of the record in the Import table</param>
        /// <param name="externalPatientId">Integer representing the vendor's patientId</param>
        /// <returns><see cref="long" /> representing the identifier of the created pre-import record</returns>
        public async Task<long> InsertPreImportRecord(long? importId, int externalPatientId)
        {
            var sqlHelper = new HospiceDataAccess(DataHelper);
            long output = await sqlHelper.InsertPreImportRecord(importId, externalPatientId).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///  inserts a pre-import patient record into the medication queue table.
        /// </summary>
        /// <param name="preImportPatientDataId">Long representing the patient added to the pre-import table</param>
        /// <returns><see cref="long" /> representing the identifier of the created pre-import record</returns>
        public async Task<long> InsertPreImportMedicationQueueRecord(long preImportPatientDataId)
        {
            var sqlHelper = new HospiceDataAccess(DataHelper);
            long output = await sqlHelper.InsertPreImportMedicationQueueRecord(preImportPatientDataId).ConfigureAwait(false);

            return output;
        }

        public async Task<long?> GetPatientIdByExternalId(int externalId)
        {
            var sqlHelper = new HospiceDataAccess(DataHelper);
            long? id = await sqlHelper.GetPatientIdByExternalId(externalId).ConfigureAwait(false);

            id = id == 0 ? null : id;

            return id;
        }
        #endregion Public Methods
    }
}
