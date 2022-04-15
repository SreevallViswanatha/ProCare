using Advantage.Data.Provider;
using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.Helpers;
using ProCare.API.PBM.Services;
using ProCare.Common.Data;
using ProCare.Common.Data.ADS;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DataAccess
{
    public class RTMDataAccess : DataAccessBase
    {

        #region Fields

        private static ILogger _log;

        #endregion

        #region Constructors

        public RTMDataAccess(IDataAccessHelper dataHelper) : base(dataHelper)
        {
            _log = Log.ForContext<AdsHelper>();
        }

        #endregion

        #region Public Methods

        public async Task ProcessSendRTMData(string adsConnectionString, List<string> masterRTMFields, List<string> masterMiscFields,
            List<string> clientRTMFields, List<string> clientMiscFields, string clientCode, string enableRecordLoggin, Dictionary<string, string> clientApiSettings)
        {
            string commandText = "APSRTM";
            using (_log.BeginTimedOperation(description: "ExecuteReader", identifier: commandText, warnIfExceeds: TimeSpan.FromMilliseconds(500), level: LogEventLevel.Debug, levelExceeds: LogEventLevel.Warning))
            {
                using (LogContext.PushProperty("SQLCommand", commandText))
                {
                    try
                    {
                        using (AdsConnection connection = DataHelper.CreateDbConnection() as AdsConnection)
                        {
                            using (AdsCommand command = DataHelper.CreateDbCommand(connection, commandText) as AdsCommand)
                            {
                                //--OPEN APSRTM table 
                                command.CommandType = CommandType.TableDirect;

                                using (AdsExtendedReader reader = command.ExecuteExtendedReader())
                                {
                                    reader.ActiveIndex = "ERRCD";
                                    reader.SetRange(new object[] { 990 }, new object[] { 999 });
                                    Guid readBatchguid = Guid.NewGuid();

                                    //--READ one row at a time
                                    while (reader.Read())
                                    {
                                        string errCode = reader["ERRCD"]?.ToString();

                                        //--Only pull records if error code is 990 or 999
                                        if (errCode == "990" || errCode == "999")
                                        {
                                            var ndcref = (string)reader["NDCREF"];
                                            var rtmRecord = new RTMRecordDTO();

                                            //--Get fields from APSRTM
                                            rtmRecord.LoadFromDataReader(reader, masterRTMFields, clientRTMFields);

                                            //---Get miscellaneous fields from other tables
                                            GetMiscFields(masterMiscFields, clientMiscFields, ndcref, rtmRecord);

                                            //--Get fields enabled for the client 
                                            RTMHelper.FilterClientFields(ref rtmRecord);

                                            //--Format data or set default values
                                            RTMHelper.Cleaunp_RTMRecord(ref rtmRecord);

                                            await RTMHelper.Log_RTMRecord(enableRecordLoggin, rtmRecord, readBatchguid).ConfigureAwait(false);

                                            //--Post RTM Record to external API
                                            var apiResponse =  await RTMHelper.Send_RTMRecord(clientApiSettings, clientCode,  rtmRecord).ConfigureAwait(false);

                                            //--Update RTM Record status based on API response status
                                            try
                                            {
                                                reader.LockRecord();
                                                try
                                                {
                                                    reader.SetInt32(reader.GetOrdinal("ERRCD"), (int)apiResponse.Status);
                                                }  
                                                catch (Exception e)
                                                {
                                                    _log.Error(e, $"Error updating record in APSRTM table..NCDCREF:{ndcref}");
                                                }
                                                finally
                                                {
                                                    try
                                                    {
                                                        reader.UnlockRecord();
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        _log.Error(e, $"Error unlocking record in APSRTM table..NCRREF:{ndcref}");
                                                    }
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                _log.Error(e, $"Error locking or updating record in APSRTM table..NCRREF:{ndcref}");
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error(e, "Error sending RTM data");
                    }

                }
            }
        }

        #endregion


        #region "Private Methods"

        public void GetMiscFields(List<string> masterMiscFields, List<string> clientMiscFields, string ndcref, RTMRecordDTO dto)
        {
            if (!string.IsNullOrEmpty(ndcref))
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "@NDCREF", ndcref }
                };

                DataHelper.ExecuteReader("apiPBM_RTM_byNDCREF", CommandType.StoredProcedure, parameters, reader2 =>
                {
                    dto.LoadFromDataReader(reader2, masterMiscFields, clientMiscFields);
                });

                //--Create empty fields if no record found in other tables
                if(!dto.Fields.Any(c => clientMiscFields.Contains(c.Name)))
                {
                    dto.LoadEmptyFields(masterMiscFields, clientMiscFields);
                }
            }

        }
        #endregion
    }
}
