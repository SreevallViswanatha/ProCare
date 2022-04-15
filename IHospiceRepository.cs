using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public interface IHospiceRepository
    {
        Task<long> InsertImportRecord(int vendorID, string importRecord, int status = 1, long? patientId = null);

        Task<long> InsertPreImportRecord(long? importId, int externalPatientId);

        Task<long> InsertPreImportMedicationQueueRecord(long preImportPatientDataId);

        Task<long?> GetPatientIdByExternalId(int externalId);
    }
}
