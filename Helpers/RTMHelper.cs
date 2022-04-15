using Newtonsoft.Json;
using ProCare.API.Core;
using ProCare.API.Core.Requests;
using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Services;
using ProCare.Common;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ProCare.API.Core.Requests.Enums;

namespace ProCare.API.PBM.Repository.Helpers
{
    public class RTMHelper
    {
        private const double _apiMethodTimeoutMilliseconds = 100000;

        #region Public Methods


        public static async Task<RTM_API_Response> Send_RTMRecord(Dictionary<string, string> clientApiSettings,string clientCode, RTMRecordDTO rtmRecord)
        {
            IRTM_API api = null;

            switch (clientCode)
            {
                case "CBR":
                    api = new RTM_API_CodeBrokers();
                    break;
                case "TMG":
                    api = new RTM_API_TMG();
                    break;
                default:
                    break;
            }

            var apiResponse = await api.SendRTMData(clientApiSettings,rtmRecord,clientCode).ConfigureAwait(false); ;
            return apiResponse;
        }

        public static async Task Log_RTMRecord(string enableRecordLoggin, RTMRecordDTO rtmRecord, Guid readBatchguid)
        {
            if (enableRecordLoggin == "True")
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                await commonApiHelper.LogApiMessage(Enums.ApiType.PBMApi, readBatchguid, ApiRoutes.ProcessSendRTMDataTask, Enums.ApiMessageType.Request,
                    JsonConvert.SerializeObject(rtmRecord)
                    ).ConfigureAwait(false);
            }
        }

        public static void Cleaunp_RTMRecord(ref RTMRecordDTO rtmRecord)
        {
            foreach (var field in rtmRecord.Fields)
            {
                switch (field.Name)
                {
                    case "RXNO":
                        if (!string.IsNullOrEmpty((string)field.Value))
                        {
                            field.Value = (field.Value?.ToString()).PadLeft(12, '0');
                        }
                        break;
                    case "ALTID":
                        field.Value = "0";
                        break;
                    case "PAT_SSN":

                        //--If pat_ssn is empty then use the CARDID value
                        if (string.IsNullOrEmpty((string)field.Value))
                        {
                            field.Value = (string)rtmRecord.Fields.Where(x => x.Name == "CARDID").Select(x => x.Value).FirstOrDefault();
                        }
                        break;

                    case "FLEX1":

                        if (string.IsNullOrEmpty((string)field.Value))
                        {
                            field.Value = "0";
                        }
                        break;

                    case "CHARGE":

                        decimal marginDec = (decimal)rtmRecord.Fields.Where(x => x.Name == "MARGIN").Select(x => x.Value)?.FirstOrDefault();
                        decimal scvfeeDec = (decimal)rtmRecord.Fields.Where(x => x.Name == "SVCFEE").Select(x => x.Value)?.FirstOrDefault();
                        decimal chargeDec = (decimal)field.Value;

                        field.Value = chargeDec + scvfeeDec + marginDec;

                        break;

                    case "ENRAMT":
                    case "DEDUCT":
                    case "COPAY":

                        StringBuilder decimalPattern = new StringBuilder("{0:00000000}");
                        string formatString = decimalPattern.ToString(0, decimalPattern.Length);
                        string fieldValue = field.Value?.ToString();

                        if (fieldValue?.Contains(".") == true)
                        {
                            decimal fieldDecimal = fieldValue.Substring(fieldValue.IndexOf(".")).ToDecimal();

                            fieldDecimal *= 100;

                            string tmpValue = String.Format(formatString, fieldDecimal);

                            if (fieldDecimal >= 0)
                            {
                                field.Value = tmpValue.Substring(0, tmpValue.Length - 1) + GetOverPunchForValue(tmpValue[tmpValue.Length - 1], true);
                            }
                            else
                            {
                                field.Value = tmpValue.Substring(1, tmpValue.Length - 2) + GetOverPunchForValue(tmpValue[tmpValue.Length - 1], false);
                            }
                        }

                        break;

                    case "FILLDT":
                    case "DOB":
                    case "NDCPROCDT":

                        if (!(field.Value == DBNull.Value))
                        {
                            field.Value = String.Format("{0:yyyyMMdd}", field.Value);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static RTMRecordDTO FilterClientFields(ref RTMRecordDTO rtmRecord)
        {
            RTMRecordDTO clientRecord = new RTMRecordDTO();

            rtmRecord.Fields.ForEach(field =>
            {
                if (field.IncludeField == true)
                {
                    clientRecord.Fields.Add(field);
                }
            });

            rtmRecord = clientRecord;
            return rtmRecord;
        }

        #endregion

        #region "Private Methods"

        private static char GetOverPunchForValue(char chrValue, bool blnIsPositive)
        {
            char chrResult = '0';

            switch (chrValue)
            {
                case '0':
                    chrResult = blnIsPositive ? '{' : '}';
                    break;
                case '1':
                    chrResult = blnIsPositive ? 'A' : 'J';
                    break;
                case '2':
                    chrResult = blnIsPositive ? 'B' : 'K';
                    break;
                case '3':
                    chrResult = blnIsPositive ? 'C' : 'L';
                    break;
                case '4':
                    chrResult = blnIsPositive ? 'D' : 'M';
                    break;
                case '5':
                    chrResult = blnIsPositive ? 'E' : 'N';
                    break;
                case '6':
                    chrResult = blnIsPositive ? 'F' : 'O';
                    break;
                case '7':
                    chrResult = blnIsPositive ? 'G' : 'P';
                    break;
                case '8':
                    chrResult = blnIsPositive ? 'H' : 'Q';
                    break;
                case '9':
                    chrResult = blnIsPositive ? 'I' : 'R';
                    break;
            }
            return chrResult;
        }

        #endregion
    }
}
