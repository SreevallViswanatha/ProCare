
namespace ProCare.API.PBM
{
    public class Constants
    {
        public const int stateOfTheHeart = 28;
        public const string ApiError = "Error While Calling API";
        public const string ConnectionInfoNotFound = "Connection string information could not be found";
        public const int ChunkSize = 250;
        public const string VerificationQueue = "VQ";
    }
    public static class Brightree
    {
        public const string MaxConcurrentEnqueueThreadCount = "MaxConcurrentEnqueueThreadCount";
        public const string APIPageSize = "APIPageSize";
        public const string BaseURL = "BaseURL";
        public const string DatabaseAuditPageSize = "DatabaseAuditPageSize";
        public const string ActivePatientsBatchSize = "ActivePatientsBatchSize";
        public const string AccessTokenRoute = "AccessTokenRoute";
        public const string SecurityClientID = "SecurityClientID";
        public const string SecuritySecret = "SecuritySecret";
        public const string ProcessingBatchSize = "ProcessingBatchSize";
        
    }

    public static class Header
    {
        public const string VendorID = "VendorID";
    }
}
