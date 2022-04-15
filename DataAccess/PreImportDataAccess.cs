using ProCare.API.PBM.Repository.DTO.ScheduledTask;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class PreImportDataAccess : DataAccessBase
    {
        #region Constructors

        public PreImportDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods
        /// <summary>
        ///  Insert a batch of records into the PatientPagedData table.
        /// </summary>
        /// <param name="PatientPagedDataDataTable">DataTable representing the batch of PatientPagedData records to write to the table</param>
        /// <returns>None</returns>
        public List<long> BatchInsertPatientPagedDataRecords(DataTable PatientPagedDataDataTable)
        {
            List<long> dto = new List<long>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportPatientPagedDataInsertType",  PatientPagedDataDataTable}
            };

            DataHelper.ExecuteReader("spPreImport_PatientPagedData_batchInsert", CommandType.StoredProcedure, parameters, reader =>
            {
                dto.Add(reader.GetInt64(0));
            });

            return dto;
        }

        /// <summary>
        ///  PatientQueue a batch of PatientPagedData records for processing.
        /// </summary>
        /// <param name="recordTypeID">Integer representing the type of PatientPagedData records being queued for processing</param>
        /// <param name="PatientPagedDataRecordIDs">List of longs representing the PatientPagedData records to PatientQueue for processing into reference records</param>
        /// <returns>None</returns>
        public void BatchInsertPatientQueueRecords(int recordTypeID, List<long> PatientPagedDataRecordIDs)
        {
            var PatientQueueDt = new DataTable();
            PatientQueueDt.Columns.Add("RecordTypeID");
            PatientQueueDt.Columns.Add("PreImportPatientPagedDataID");
            PatientQueueDt.Columns.Add("ProcessStatusID");

            PatientPagedDataRecordIDs.ForEach(PatientPagedDataRecordID =>
            {
                PatientQueueDt.Rows.Add(recordTypeID, PatientPagedDataRecordID, 0);
            });

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportPatientQueueInsertType",  PatientQueueDt}
            };

            DataHelper.ExecuteNonQuery("spPreImport_PatientQueue_batchInsert", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        ///  Update the status of a batch of PatientPagedData records in the PatientQueue.
        /// </summary>
        /// <param name="PatientPagedDataRecords"><see cref="List{PreImportPatientQueueDTO}" /> representing the PatientPagedData records to update the status of in the PatientQueue</param>
        /// <returns>None</returns>
        public void BatchUpdatePatientQueueStatus(List<PreImportPatientQueueDTO> PatientQueue)
        {
            var PatientQueueDt = new DataTable();
            PatientQueueDt.Columns.Add("PreImportPatientQueueID");
            PatientQueueDt.Columns.Add("ProcessStatusID");

            PatientQueue.ForEach(PatientPagedDataRecord =>
            {
                PatientQueueDt.Rows.Add(PatientPagedDataRecord.PreImportPatientQueueID, PatientPagedDataRecord.ProcessStatusID);
            });

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportPatientQueueUpdateType",  PatientQueueDt}
            };

            DataHelper.ExecuteNonQuery("spPreImport_PatientQueue_batchUpdate", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        ///  Process a batch of reference records into the database if they are newer than the latest record we have for the record identifier
        /// </summary>
        /// <param name="referenceDataTable">DataTable representing batch of reference records to process into the database</param>
        /// <returns>None</returns>
        public void BatchProcessChanges(DataTable referenceDataTable)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportPatientDataInsertType",  referenceDataTable}
            };

            DataHelper.ExecuteNonQuery("spPreImport_PatientData_batchProcessChanges", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        ///  Pull a given number of PatientPagedData records from the batch for processing.
        /// </summary>
        /// <param name="recordTypeID">Integer representing the type of PatientPagedData records to pull from the PatientQueue for processing</param>
        /// <param name="vendorID">Long representing the identifier of the vendor to pull PatientPagedData records for</param>
        /// <param name="batchSize">Integer representing the number of PatientPagedData records to pull from the PatientQueue for processing</param>
        /// <returns><see cref="List{PreImportPatientQueueDTO}" /> representing the PatientPagedData records to be processed</returns>
        public List<PreImportPatientQueueDTO> ReadPatientQueueBatch(int recordTypeID, long vendorID, int batchSize)
        {
            List<PreImportPatientQueueDTO> output = new List<PreImportPatientQueueDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@RecordTypeID",  recordTypeID},
                {"@VendorID",  vendorID},
                {"@BatchSize",  batchSize}
            };

            DataHelper.ExecuteReader("spPreImport_PatientQueue_getBatch", CommandType.StoredProcedure, parameters, reader =>
            {
                PreImportPatientQueueDTO dto = new PreImportPatientQueueDTO();
                dto.LoadFromDataReader(reader);
                output.Add(dto);
            });

            return output;
        }
        public List<PreImportPatientDataDTO> ReadActivePatientsBatch(int batchSize)
        {
            List<PreImportPatientDataDTO> output = new List<PreImportPatientDataDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@BatchSize",  batchSize}
            };

            DataHelper.ExecuteReader("spPreImport_PatientData_getActivePatientsBatch", CommandType.StoredProcedure, parameters, reader =>
            {
                PreImportPatientDataDTO dto = new PreImportPatientDataDTO();
                dto.LoadFromDataReader(reader);
                output.Add(dto);
            });

            return output;
        }

        /// <summary>
        ///  Insert all active patients from PreImportPatientData into PreImportMedPatientQueue
        /// </summary>
        /// <param name="medLastPullTime">DateTime representing last time medication records were pulled</param>
        /// <param name="vendorID">Long representing the identifier of the vendor to pull records for</param>/// 
        /// <returns><see cref="List{long}" /> representing id of records added</returns>
        public List<long> BatchInsertMedPatientQueueRecords(DateTime? medLastPullTime, long vendorID)
        {
            List<long> dto = new List<long>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@VendorID",  vendorID}
            };

            if (medLastPullTime == null)
            {
                parameters.Add("@MedLastPullTime", DBNull.Value);
            }
            else
            {
                parameters.Add("@MedLastPullTime", medLastPullTime);
            }

            DataHelper.ExecuteReader("spPreImport_MedPatientQueue_insertBatch", CommandType.StoredProcedure, parameters, reader =>
            {
                dto.Add(reader.GetInt64(0));
            });

            return dto;
        }

        public void UpdateNetsmartMedQueue(long id, int statusId, long importId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@PreImportnetsmartSOTHMedQueueID",  id},
                {"@ProcessStatusID",  statusId},
                {"@ImportID",  importId}
            };

            DataHelper.ExecuteNonQuery("apiPBM_PreImport_PreImportNetsmartSOTHMedQueue_Update", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        ///  Pull a given number of PatientPagedData records from the batch for processing.
        /// </summary>
        /// <param name="vendorID">Long representing the identifier of the vendor to pull PatientPagedData records for</param>
        /// <param name="batchSize">Integer representing the number of MedPatientPagedData records to pull from the MedPatientQueue for processing</param>
        /// <returns><see cref="List{PreImportPatientQueueDTO}" /> representing the MedPatientPagedData records to be processed</returns>
        public List<PreImportMedPatientQueueDTO> ReadMedPatientQueueBatch(long vendorID, int batchSize)
        {
            List<PreImportMedPatientQueueDTO> output = new List<PreImportMedPatientQueueDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@VendorID",  vendorID},
                {"@BatchSize",  batchSize}
            };

            DataHelper.ExecuteReader("spPreImport_MedPatientQueue_getBatch", CommandType.StoredProcedure, parameters, reader =>
            {
                PreImportMedPatientQueueDTO dto = new PreImportMedPatientQueueDTO();
                dto.LoadFromDataReader(reader);
                output.Add(dto);
            });

            return output;
        }

        /// <summary>
        ///  Update Status of records being processed in MedPatientQueue
        /// </summary>
        /// <param name="status">Integer representing the status of the record being processed</param>
        /// <param name="PreImportMedPatientQueueDTO">List representing MedPatientQueue records</param>
        /// <returns>None</returns>
        public void BatchUpdateMedPatientQueueStatus(List<PreImportMedPatientQueueDTO> medPatientQueueDto)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("MedPatientQueueID");
            dataTable.Columns.Add("ProcessStatusID");

            medPatientQueueDto.ForEach(record =>
            {
                dataTable.Rows.Add(record.PreImportMedPatientQueueID, record.ProcessStatusID);
            });

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportMedPatientQueueUpdateType",  dataTable},
            };

            DataHelper.ExecuteNonQuery("spPreImport_MedPatientQueue_batchUpdate_status", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        ///  Insert medications raw data into PreImportMedPagedData
        /// </summary>
        /// <param name="pagedDataTable">DataTable representing the batch of records to be inserted</param>
        /// <returns><see cref="List{long}" />List representing id of records added</returns>
        public List<long> BatchInsertMedPagedData(DataTable pagedDataTable)
        {
            List<long> dto = new List<long>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportMedPagedDataInsertType",  pagedDataTable}
            };

            DataHelper.ExecuteReader("spPreImport_MedPagedData_batchInsert", CommandType.StoredProcedure, parameters, reader =>
            {
                dto.Add(reader.GetInt64(0));
            });

            return dto;
        }

        /// <summary>
        ///  Enqueue medications paged data
        /// </summary>
        /// <param name="MedPagedDataRecordIDs">List of Longs representing MedPagedDataRecordIDs</param>
        /// <returns>None</returns>
        public List<long> BatchInsertMedQueueRecords(List<long> medPagedDataRecordIDs)
        {
            List<long> dto = new List<long>();

            var dataTable = new DataTable();
            dataTable.Columns.Add("PreImportMedPagedDataID");

            medPagedDataRecordIDs.ForEach(PatientPagedDataRecordID =>
            {
                dataTable.Rows.Add(PatientPagedDataRecordID);
            });

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportMedQueueInsertType",dataTable}
            };

            DataHelper.ExecuteReader("spPreImport_MedQueue_batchInsert", CommandType.StoredProcedure, parameters, reader =>
                {
                    dto.Add(reader.GetInt64(0));
                });

            return dto;
        }

        public List<PreImportMedQueueDTO> ReadMedQueueBatch(int batchSize, long vendorID)
        {
            List<PreImportMedQueueDTO> output = new List<PreImportMedQueueDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@VendorID",  vendorID},
                {"@BatchSize",  batchSize}
            };

            DataHelper.ExecuteReader("spPreImport_MedQueue_getBatch", CommandType.StoredProcedure, parameters, reader =>
            {
                PreImportMedQueueDTO dto = new PreImportMedQueueDTO();
                dto.LoadFromDataReader(reader);
                output.Add(dto);

            });

            return output;
        }


        /// <summary>
        ///  Update MedQueue record status
        /// </summary>
        /// <param name="status">Integer representing the status of the record being processed.</param>
        /// <param name="medQueueRecordDto">List representing records ID.</param>
        /// <returns>None</returns>
        public void BatchUpdateMedQueueStatus(List<PreImportMedQueueDTO> medQueueRecordDto)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PreImportMedQueueID");
            dataTable.Columns.Add("ProcessStatusID");

            medQueueRecordDto.ForEach(dto =>
            {
                dataTable.Rows.Add(dto.PreImportMedQueueID, dto.ProcessStatusID);
            });

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportMedQueueUpdateType",  dataTable},
            };

            DataHelper.ExecuteNonQuery("spPreImport_MedQueue_batchUpdate_status", CommandType.StoredProcedure, parameters);
        }

        public void BatchUpdateNetsmartMedQueueStatus(List<PreImportNetsmartMedQueueDTO> dtoList)
        {
            var dt = new DataTable();
            dt.Columns.Add("PreImportNetsmartSOTHMedQueueID");
            dt.Columns.Add("ProcessStatusID");

            dtoList.ForEach(dto =>
            {
                dt.Rows.Add(dto.PreImportNetsmartSOTHMedQueueID, dto.ProcessStatusID);
            });

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportNetsmartMedQueueUpdateType",  dt},
            };

            DataHelper.ExecuteNonQuery("apiPBM_PreImport_NetsmartSOTHMedQueue_UpdateBatchStatus", CommandType.StoredProcedure, parameters);
        }

        public void BatchProcessMedData(DataTable medDataTable)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@tblPreImportMedDataInsertType",  medDataTable},
            };

            DataHelper.ExecuteNonQuery("spPreImport_MedData_batchProcessChanges", CommandType.StoredProcedure, parameters);
        }
        public DateTime? GetLastPullTimeOfEnqueuedPatientsForMedication(long vendorID)
        {
            DateTime? dto = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@VendorID",  vendorID},
            };

           DataHelper.ExecuteReader("spPreImport_DataSchedule_GetLastPullTimeOfEnqueuedPatientsForMedication", CommandType.StoredProcedure, parameters, reader =>
            {
                dto = reader.GetDateTimeorDefault("LastTriggerTime", DateTime.MinValue);
                if (dto == DateTime.MinValue)
                {
                    dto = null;
                }
            });
            return dto;
        }



        public List<PreImportNetsmartMedQueueDTO> ReadNetsmartMedQueueBatch(int batchSize)
        {
            List<PreImportNetsmartMedQueueDTO> output = new List<PreImportNetsmartMedQueueDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@TopRecords",  batchSize}
            };

            DataHelper.ExecuteReader("apiPBM_PreImport_NetsmartSOTHMedQueue_GetBatch", CommandType.StoredProcedure, parameters, reader =>
            {
                PreImportNetsmartMedQueueDTO dto = new PreImportNetsmartMedQueueDTO();
                dto.LoadFromDataReader(reader);
                output.Add(dto);

            });

            return output;
        }

        #endregion
    }
}
