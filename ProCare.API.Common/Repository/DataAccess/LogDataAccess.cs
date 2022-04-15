using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.Common.Repository.DataAccess
{
    public class LogDataAccess : DataAccessBase
    {
        #region Constructors

        public LogDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Inserts an Api message to the database
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
        public void InsertApiMessage(int apiTypeID, Guid messageIdentifier, string methodName, int messageTypeID, string xmlMessage, string jsonMessage, string clientIPAddress = null,
            string identifier1 = null, string identifier2 = null, string identifier3 = null, string identifier4 = null, DateTime? MessageTimeStamp = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@APITypeID",  apiTypeID },
                {"@APIMessageTypeID", messageTypeID},
                {"@APIMessageGuid",  messageIdentifier},
                {"@APIMethodName",  methodName},
                {"@ClientIPAddress",  clientIPAddress },
                {"@Identifier1",  identifier1},
                {"@Identifier2",  identifier2},
                {"@Identifier3",  identifier3},
                {"@Identifier4",  identifier4 },
                {"@JSONMessage",  jsonMessage},
                {"@XMLMessage",  xmlMessage},
                {"@MessageTimeStamp", MessageTimeStamp }
            };

            DataHelper.ExecuteNonQuery("log_InsertAPIMessage", CommandType.StoredProcedure, parameters);
        }

        public void InsertLogMessage(int apiTypeID, Guid messageIdentifier, string methodName, int messageTypeID, string xmlMessage, string jsonMessage, string clientIPAddress = null,
    string identifier1 = null, string identifier2 = null, string identifier3 = null, string identifier4 = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@APITypeID",  apiTypeID },
                {"@APIMessageTypeID", messageTypeID},
                {"@APIMessageGuid",  messageIdentifier},
                {"@APIMethodName",  methodName},
                {"@ClientIPAddress",  clientIPAddress },
                {"@Identifier1",  identifier1},
                {"@Identifier2",  identifier2},
                {"@Identifier3",  identifier3},
                {"@Identifier4",  identifier4 },
                {"@JSONMessage",  jsonMessage},
                {"@XMLMessage",  xmlMessage}
            };

            DataHelper.ExecuteNonQuery("log_InsertAPIMessage", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        /// Record a warning message to the ApplicationLog
        /// </summary>
        /// <param name="Source">Integer representing the Application requesting the log</param>
        /// <param name="message">String representing the warning message to record</param>
        /// <param name="Properites">Optional string holding details in Json format</param>
        public void InsertWarningMessage(int Source, string Message, string Properties = "")
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ApplicationSourceID",  Source},
                {"@Message", Message},
                {"@Properties",  Properties}
            };

            DataHelper.ExecuteNonQuery("Insert_AppLogWarning", CommandType.StoredProcedure, parameters);

        }

        /// <summary>
        /// Record an informational message to the ApplicationLog
        /// </summary>
        /// <param name="Source">Integer representing the Application requesting the log</param>
        /// <param name="Message">String representing the warning message to record</param>
        /// <param name="Properties">Optional string holding details in Json format</param>
        public void InsertInfoMessage(int Source, string Message, string Properties = "")
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ApplicationSourceID",  Source},
                {"@Message", Message},
                {"@Properties",  Properties}
            };

            DataHelper.ExecuteNonQuery("Insert_AppLogInformation", CommandType.StoredProcedure, parameters);
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
        public void InsertExceptionMessage(bool isCritical, int Source, string Message, string StackTrace, string Properties = "", string MethodSource = "")
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@ApplicationSourceID",  Source},
                {"@MessageLevelID", isCritical ? 1 :2 },
                {"@Message", Message},
                {"@StackTrace", StackTrace },
                {"@Properties",  Properties},
                {"@MethodSource", MethodSource }
            };

            DataHelper.ExecuteNonQuery("Insert_AppLogException", CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        /// Inserts an application response time to the ApplicationLog
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
        public void InsertAppEventResponseTime(int appSourceID, int appEventTypeID, string appEventName, string appFeatureName,
            int responseTime, string miscField1, string miscField2, string miscField3, string properties)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@AppSourceID", appSourceID },
                {"@AppEventTypeID", appEventTypeID },
                {"@AppEventName", appEventName },
                {"@AppFeature", appFeatureName },
                {"@ResponseTimeMs",  responseTime},
                {"@MiscField1",  miscField1},
                {"@MiscField2",  miscField2},
                {"@MiscField3",  miscField3},
                {"@Properties",  properties}
            };

            DataHelper.ExecuteNonQuery("Insert_AppEventResponseTime", CommandType.StoredProcedure, parameters);
        }
        #endregion Public Methods
    }
}
