using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DTO
{
    public class RetroLICSProcessingConfigDTO
    {
        public int WaitSeconds { get; set; }
        public string ClaimFilePath { get; set; }
        public bool SyncAccumulators { get; set; }
        public bool ErrorOnNonZeroAccums { get; set; }
        public bool DeleteClaimFilesOnCompletion { get; set; }
        public Dictionary<string, string> ClaimApiCredentials { get; set; }
        public int ConcurrentThreadCount { get; set; }
        public DateTime StartDate { get; set; }
        public bool WriteLastCompletionTime { get; set; }
        public bool BeginAfterLastCompletionTime { get; set; }
    }
}
