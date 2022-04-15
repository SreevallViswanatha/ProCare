using ProCare.API.Common.Repository.DTO;
using System;

namespace ProCare.API.Common.Repository
{
    public interface ILogRepository
    {
        void LogApiMessage(int apiTypeID, Guid id, string methodName, int messageTypeID, string xmlMessage, string jsonMessage, string clientIPAddress = null,
            string identifier1 = null, string identifier2 = null, string identifier3 = null, string identifier4 = null, DateTime? MessageTimeStamp = null);

        void LogWarningMessage(int Source, string message, string Properties = "");
        void LogInfoMessage(int Source, string message, string Properties = "");
        void LogExceptionMessage(bool isCritical, int source, string message, string stackTrace, string properties = "", string methodSource = "");

        void LogAppEventResponseTime(int appSourceID, int appEventTypeID, string appEventName, string appFeatureName,
            int responseTime, string miscField1, string miscField2, string miscField3, string properties);
    }
}
