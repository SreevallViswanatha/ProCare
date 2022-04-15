using ProCare.API.Common.Repository.DataAccess;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;

namespace ProCare.API.Common.Repository
{
    public class LogRepository : BasedbRepository, ILogRepository
    {
        #region Constructors

        public LogRepository(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Logs an Api message
        /// </summary>
        /// <param name="apiTypeID">Integer representing the identifier of the Api Type</param>
        /// <param name="messageIdentifier">Guid representing the message identifier</param>
        /// <param name="methodName">String representing the name of the Api method</param>
        /// <param name="messageTypeID">Integer representing the identifier of the Api Message Type</param>
        /// <param name="xmlMessage">String representing the message in XML format</param>
        /// <param name="jsonMessage">String representing the message in Json format</param>
        /// <param name="clientIPAddress">Optional String representing the client IP address</param>
        /// <param name="identifier1">Optional String representing the first identifier</param>
        /// <param name="identifier2">Optional String representing the second identifier</param>
        /// <param name="identifier3">Optional String representing the third identifier</param>
        /// <param name="identifier4">Optional String representing the fourth identifier</param>
        public void LogApiMessage(int apiTypeID, Guid messageIdentifier, string methodName, int messageTypeID, string xmlMessage, string jsonMessage, string clientIPAddress = null,
            string identifier1 = null, string identifier2 = null, string identifier3 = null, string identifier4 = null, DateTime? MessageTimeStamp = null)
        {
            var sqlHelper = new LogDataAccess(DataHelper);
            sqlHelper.InsertApiMessage(apiTypeID, messageIdentifier, methodName, messageTypeID, xmlMessage, jsonMessage, clientIPAddress, identifier1, identifier2, identifier3, identifier4, MessageTimeStamp);
        }

        /// <summary>
        /// Record a warning message to the ApplicationLog
        /// </summary>
        /// <param name="Source">Integer representing the Application requesting the log</param>
        /// <param name="message">String representing the warning message to record</param>
        /// <param name="Properites">Optional string holding details in Json format</param>
        public void LogWarningMessage(int Source, string Message, string Properties = "")
        {
            var sqlHelper = new LogDataAccess(DataHelper);
            sqlHelper.InsertWarningMessage(Source, Message, Properties);
        }

        /// <summary>
        /// Record an informational message to the ApplicationLog
        /// </summary>
        /// <param name="Source">Integer representing the Application requesting the log</param>
        /// <param name="Message">String representing the warning message to record</param>
        /// <param name="Properties">Optional string holding details in Json format</param>
        public void LogInfoMessage(int Source, string Message, string Properties = "")
        {
            var sqlHelper = new LogDataAccess(DataHelper);
            sqlHelper.InsertInfoMessage(Source, Message, Properties);
        }

        /// <summary>
        /// Record an exception message to the ApplicationLog
        /// </summary>
        /// <param name="isCritical">Bool indicating is the exception was critical</param>
        /// <param name="Source">Integer representing the Application requesting the log</param>
        /// <param name="Message">String representing the warning message to record</param>
        /// <param name="Properties">Optional string holding details in Json format</param>
        /// <param name="MethodSource">String representing a method name</param>
        /// <param name="StackTrace">String representing an exception stack track</param>
        public void LogExceptionMessage(bool isCritical, int Source, string Message, string StackTrace, string Properties = "", string MethodSource = "")
        {
            var sqlHelper = new LogDataAccess(DataHelper);
            sqlHelper.InsertExceptionMessage(isCritical, Source, Message, StackTrace, Properties, MethodSource);
        }

        /// <summary>
        /// Logs an application event response time
        /// </summary>
        /// <param name="appSourceID">Integer representing the application source identifier</param>
        /// <param name="appEventTypeID">Integer representing the application event type identifier</param>
        /// <param name="apoEventName">String representing the application method name or stored procedure name</param>
        /// <param name="appFeatureName">String representing the application feature name</param>
        /// <param name="responseTime">Integer representing the application event response time</param>
        /// <param name="miscField1">String representing the first miscellaneous field</param>
        /// <param name="miscField2">String representing the second miscellaneous field</param>
        /// <param name="miscField3">String representing the third miscellaneous field</param>
        /// <param name="properties">Optional Json string representing the additional message details</param>
        public void LogAppEventResponseTime(int appSourceID, int appEventTypeID, string appEventName, string appFeatureName,
            int responseTime, string miscField1, string miscField2, string miscField3, string properties)
        {
            var sqlHelper = new LogDataAccess(DataHelper);
            sqlHelper.InsertAppEventResponseTime(appSourceID, appEventTypeID, appEventName, appFeatureName, responseTime, miscField1, miscField2, miscField3, properties);
        }
        #endregion Public Methods
    }
}
