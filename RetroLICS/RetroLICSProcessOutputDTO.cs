using ProCare.API.PBM.Exceptions;
using System.Collections.Generic;

namespace ProCare.API.PBM.Repository.DTO
{
    public class RetroLICSProcessOutputDTO 
    {
        public string EnrolleeID { get; set; }
        public List<string> NonZeroAccums { get; set; }
        public bool MemberLocked { get; set; }
        public RetroLICSProcessingException ProcessingException { get; set; }
        public bool ReverseClaimsExist { get; set; }
        public bool MemberProcessingAlreadyLocked { get; set; }
    }

    public enum RetroLICSProcessingResultType
    {
        Unknown = 0,
        Success = 1,
        Success_RerunNeeded = 2,
        Failure_Reject = 3,
        Failure_ExpectedError = 4,
        Failure_UnexpectedError = 5
    }
}
