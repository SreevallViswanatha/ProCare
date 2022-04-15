using ProCare.API.PBM.Repository.DTO.ScheduledTask;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.PBM.Repository
{
    public interface IPreImportRepository
    {
        List<long> BatchInsertPatientPagedDataRecords(DataTable PatientPagedDataDataTable);

        void BatchInsertPatientQueueRecords(int recordTypeID, List<long> PatientPagedDataRecordIDs);

        List<PreImportPatientQueueDTO> GetPatientQueueBatch(int recordTypeID, long vendorID, int batchSize);

        void BatchUpdatePatientQueueStatus(List<PreImportPatientQueueDTO> PatientPagedDataRecords);

        void BatchProcessChanges(DataTable referenceDataTable);

        List<PreImportPatientDataDTO> GetActivePatientsBatch(int batchSize);

        List<long> BatchInsertMedPatientQueueRecords(DateTime? medLastPullTime, long vendorID);

        void BatchUpdateMedPatientQueueStatus(List<PreImportMedPatientQueueDTO> MedPatientQueue);

        List<PreImportMedPatientQueueDTO> GetMedPatientQueueBatch(int batchSize, long vendorID);

        List<long> BatchInsertMedPagedData(DataTable pagedDataTable);

        List<long> BatchInsertMedQueueRecords(List<long> MedPagedDataRecordIDs);

        List<PreImportMedQueueDTO> GetMedQueueBatch(int batchSize, long vendorID);

        void BatchUpdateMedQueueStatus(List<PreImportMedQueueDTO> medQueueRecordIDs);

        void BatchProcessMedData(DataTable medDataTable);

        DateTime? GetLastPullTimeOfEnqueuedPatientsForMedication(long vendorID);
        List<PreImportNetsmartMedQueueDTO> GetNetsmartMedQueueBatch(int v);
        void BatchUpdateNetsmartMedQueueStatus(List<PreImportNetsmartMedQueueDTO> batch);
        void UpdateNetsmartMedQueue(long preImportNetsmartSOTHMedQueueID, int v, long importId);
    }
}
