using System;

namespace ProCare.API.PBM.Exceptions
{
    public class RetroLICSProcessingException : Exception
    {
        public enum RetroLICSProcessingExceptionType
        {
            NoException = 0,
            FailedToLoadClaims = 1,
            FailedToGetEligibility = 2,
            GapInEligibility = 3,
            ClaimReversalFailed = 4,
            ClaimSubmissionFailed = 5,
            NonZeroAccumulatorAfterReversals = 6,
            AccumulatorRebuildFailed = 7,
            UnexpectedError = 8,
            VerificationFailureByReprocGroupingCheck = 9,
            VerificationFailureByStatusCheck = 10,
            APSRLMNotFound = 11,
            APSRLMNotLocked = 12,
            InvalidHostURL = 13
        }

        public RetroLICSProcessingExceptionType ExceptionType { get; set; }
        public string EnrolleeID { get; set; }
        public string ErrorReason { get; set; }
        public object Data { get; set; }
        public bool MemberLocked { get; set; }

        public RetroLICSProcessingException(RetroLICSProcessingExceptionType exceptionType, string enrolleeId, string errorReason, string message, object data = null, bool memberLocked = false) : base(message)
        {
            ExceptionType = exceptionType;
            EnrolleeID = enrolleeId;
            ErrorReason = errorReason;
            Data = data;
            MemberLocked = memberLocked;        }
    }
}
