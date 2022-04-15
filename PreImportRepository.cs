using ProCare.API.PBM.Repository.DataAccess;
using ProCare.API.PBM.Repository.DTO.ScheduledTask;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.PBM.Repository
{
    public class PreImportRepository : BasedbRepository, IPreImportRepository
    {
        private PreImportDataAccess SqlHelper { get; set; }

        #region Constructor

        public PreImportRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
            SqlHelper = new PreImportDataAccess(dataHelper);
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///  Insert a batch of records into the PatientPagedData table.
        /// </summary>
        /// <param name="PatientPagedDataDataTable">DataTable representing the batch of PatientPagedData records to write to the table</param>
        /// <returns>None</returns>
        public List<long> BatchInsertPatientPagedDataRecords(DataTable PatientPagedDataDataTable)
        {
            return SqlHelper.BatchInsertPatientPagedDataRecords(PatientPagedDataDataTable);
        }

        /// <summary>
        ///  PatientQueue a batch of PatientPagedData records for processing.
        /// </summary>
        /// <param name="recordTypeID">Integer representing the type of PatientPagedData records being queued for processing</param>
        /// <param name="PatientPagedDataRecordIDs">List of longs representing the PatientPagedData records to PatientQueue for processing into reference records</param>
        /// <returns>None</returns>
        public void BatchInsertPatientQueueRecords(int recordTypeID, List<long> PatientPagedDataRecordIDs)
        {
            SqlHelper.BatchInsertPatientQueueRecords(recordTypeID, PatientPagedDataRecordIDs);
        }

        /// <summary>
        ///  Pull a given number of PatientPagedData records from the batch for processing.
        /// </summary>
        /// <param name="recordTypeID">Integer representing the type of PatientPagedData records to pull from the PatientQueue for processing</param>
        /// <param name="vendorID">Long representing the identifier of the vendor to pull PatientPagedData records for</param>
        /// <param name="batchSize">Integer representing the number of PatientPagedData records to pull from the PatientQueue for processing</param>
        /// <returns><see cref="List{PreImportPatientQueueDTO}" /> representing the PatientPagedData records to be processed</returns>
        public List<PreImportPatientQueueDTO> GetPatientQueueBatch(int recordTypeID, long vendorID, int batchSize)
        {
            return SqlHelper.ReadPatientQueueBatch(recordTypeID, vendorID, batchSize);
        }

        /// <summary>
        ///  Update the status of a batch of PatientPagedData records in the PatientQueue.
        /// </summary>
        /// <param name="PatientPagedDataRecords"><see cref="List{PreImportPatientQueueDTO}" /> representing the PatientPagedData records to update the status of in the PatientQueue</param>
        /// <returns>None</returns>
        public void BatchUpdatePatientQueueStatus(List<PreImportPatientQueueDTO> PatientPagedDataRecords)
        {
            SqlHelper.BatchUpdatePatientQueueStatus(PatientPagedDataRecords);
        }

        /// <summary>
        ///  Process a batch of reference records into the database if they are newer than the latest record we have for the record identifier
        /// </summary>
        /// <param name="referenceDataTable">DataTable representing batch of reference records to process into the database</param>
        /// <returns>None</returns>
        public void BatchProcessChanges(DataTable referenceDataTable)
        {
            SqlHelper.BatchProcessChanges(referenceDataTable);
        }

        public List<PreImportPatientDataDTO> GetActivePatientsBatch(int batchSize)
        {
            return SqlHelper.ReadActivePatientsBatch(batchSize);
        }

        /// <summary>
        ///  Insert all active patients from PreImportPatientData into PreImportMedPatientQueue
        /// </summary>
        /// <param name="medLastPullTime">DateTime representing last time medication records were pulled</param>
        /// <param name="vendorID">Long representing the identifier of the vendor to pull records for</param>/// 
        /// <returns><see cref="List{long}" />List representing id of records added</returns>
        public List<long> BatchInsertMedPatientQueueRecords(DateTime? medLastPullTime, long vendorID)
        {
            return SqlHelper.BatchInsertMedPatientQueueRecords(medLastPullTime, vendorID);
        }

        /// <summary>
        ///  Pull a given number of PatientPagedData records from the batch for processing.
        /// </summary>
        /// <param name="vendorID">Long representing the identifier of the vendor to pull MedPatientPagedData records for</param>
        /// <param name="batchSize">Integer representing the number of MedPatientPagedData records to pull from the MedPatientQueue for processing</param>
        /// <returns><see cref="List{PreImportPatientQueueDTO}" /> representing the MedPatientPagedData records to be processed</returns>
        public List<PreImportMedPatientQueueDTO> GetMedPatientQueueBatch(int batchSize, long vendorID)
        {
            return SqlHelper.ReadMedPatientQueueBatch(vendorID, batchSize);
        }

        /// <summary>
        ///  Update Status of records being processed in MedPatientQueue
        /// </summary>
        /// <param name="status">Integer representing the status of the record being processed</param>
        /// <param name="medPatientQueueDto">List representing a MedPatientQueue record</param>
        public void BatchUpdateMedPatientQueueStatus(List<PreImportMedPatientQueueDTO> medPatientQueue)
        {
            SqlHelper.BatchUpdateMedPatientQueueStatus(medPatientQueue);
        }

        /// <summary>
        ///  Insert medications raw data into PreImportMedPagedData
        /// </summary>
        /// <param name="pagedDataTable">DataTable representing the batch of records to be inserted</param>
        /// <returns><see cref="List{long}" />List representing id of records added</returns>
        public List<long> BatchInsertMedPagedData(DataTable pagedDataTable)
        {
            return SqlHelper.BatchInsertMedPagedData(pagedDataTable);
        }

        public List<long> BatchInsertMedQueueRecords(List<long> medPagedDataRecordIDs)
        {
            return SqlHelper.BatchInsertMedQueueRecords(medPagedDataRecordIDs);
        }

        public List<PreImportMedQueueDTO> GetMedQueueBatch(int batchSize, long vendorID)
        {
            return SqlHelper.ReadMedQueueBatch(batchSize, vendorID);
        }

        public void BatchUpdateMedQueueStatus(List<PreImportMedQueueDTO> medQueueRecordIDs)
        {
            SqlHelper.BatchUpdateMedQueueStatus(medQueueRecordIDs);
        }

        public void BatchUpdateNetsmartMedQueueStatus(List<PreImportNetsmartMedQueueDTO> medQueueRecordIDs)
        {
            SqlHelper.BatchUpdateNetsmartMedQueueStatus(medQueueRecordIDs);
        }

        public void UpdateNetsmartMedQueue(long id, int statusId, long importId)
        {
            SqlHelper.UpdateNetsmartMedQueue(id, statusId, importId);
        }

        public void BatchProcessMedData(DataTable medDataTable)
        {
            SqlHelper.BatchProcessMedData(medDataTable);
        }

        /// <summary>
        ///   Get last trigger time patients were pulled for medications
        /// </summary>
        /// <param name="vendorID">Long representing the identifier of the vendor to pull MedPatientPagedData records for</param>
        /// <returns><see cref="DateTime" /> representing DateTime patient was last pulled</returns>
        public DateTime? GetLastPullTimeOfEnqueuedPatientsForMedication(long vendorID)
        {
            return SqlHelper.GetLastPullTimeOfEnqueuedPatientsForMedication(vendorID);
        }

        public List<PreImportNetsmartMedQueueDTO> GetNetsmartMedQueueBatch(int batchSize)
        {
            return SqlHelper.ReadNetsmartMedQueueBatch(batchSize);
        }
        #endregion
    }
}
