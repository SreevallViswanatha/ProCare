using FluentValidation;
using log4net;
using ProCare.API.Claims.Claims;
using ProCare.API.Claims.Helpers;
using ProCare.API.Claims.Messages.Request;
using ProCare.API.Claims.Messages.Response;
using ProCare.API.Claims.PayerSheets;
using ProCare.API.Claims.Validators;
using ProCare.API.Core;
using ProCare.NCPDP.Telecom;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProCare.API.Core.Constants;
using ProCare.API.Core.Requests;
using static ProCare.API.Core.Requests.Enums;
using ProCare.API.Core.Responses;
using ProCare.NCPDP.Telecom.Extensions;
using ProCare.API.Claim.Claims;

namespace ProCare.API.Claims
{
    /// <summary>
    /// Common Services are services that are not specific to a certain LOB (PBM/Pharmacy).
    /// Settings are a good example of services that are provided here.
    /// </summary>
    public class ClaimServices : Service
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly object transactionCounterLock = new object();
        private static int transactionCounter = 0;

        #region Claims Processing

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ClaimSubmissionRequest + "|" + ApiRoutes.ClaimSubmissionRequest)]
        public async Task<ClaimSubmissionResponse> Post(ClaimSubmissionRequest request)
        {
            //DateTime processingStartTime = DateTime.Now;
            ClaimSubmissionResponse response = null;
            //DateTime startTime = DateTime.Now;
            //List<double> msTimes = new List<double>();

            response = await processRequest(request, ApiRoutes.ClaimSubmissionRequest, async () =>
            {
                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                response = new ClaimSubmissionResponse();
                string requestLoggingName = "Claim Submission";

                Transmission Ncpdp = new Transmission();
                ClaimHelper helper = new ClaimHelper();
                TransmissionValidator validator = new TransmissionValidator();


                bool isCareMark = helper.IsCareMark(request.Header.BinNumber);
                bool isIlliniosMedicaid = helper.IsIllinoisMedicaid(request.Header.BinNumber, request.Header.ProcessorControlNumber);
                bool isExpressScripts = helper.IsExpressScripts(request.Header.BinNumber);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                if (isCareMark)
                {
                    validator.ValidateAndThrow(request, GetClaimSubmissionValidationString("CareMark", request));
                }
                else if (isExpressScripts)
                {
                    validator.ValidateAndThrow(request, GetClaimSubmissionValidationString("ExpressScripts", request));
                }
                else
                {
                    validator.ValidateAndThrow(request);
                }

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //mapping request into NCPDP 
                Ncpdp = helper.MapSubmissionRequestClaimToNcpdp(request);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                if (isCareMark)
                {
                    CareMarkPayerSheet caremark = new CareMarkPayerSheet();
                    caremark.ValidateClaimSubmissionRequest(Ncpdp);
                }
                else if (isIlliniosMedicaid)
                {
                    DefaultPayerSheet illMedicaid = new DefaultPayerSheet();
                    illMedicaid.ValidateClaimSubmissionRequest(Ncpdp);
                }
                else if (isExpressScripts)
                {
                    ExpressScriptsPayerSheet expressScripts = new ExpressScriptsPayerSheet();
                    expressScripts.ValidateClaimSubmissionRequest(Ncpdp);
                }
                else
                {
                    DefaultPayerSheet payersheet = new DefaultPayerSheet();
                    payersheet.ValidateClaimSubmissionRequest(Ncpdp);
                }

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //convert NCPDP request into claim string
                string claim = Ncpdp.ToString();

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //Log claim string
                await logNCPDPInfo($"{requestLoggingName} NCPDP Request", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "NCPDPRequestString", claim } }).ConfigureAwait(false);
                Request.Items.Add(Logging.Text_RespIdentifier1, $"See AppLog '{requestLoggingName} NCPDP Request'");

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //Creates switcher response
                Tuple<string, List<double>> resp = await Switcher.Submit(claim, getReferenceStringSets(Ncpdp)).ConfigureAwait(false);
                string switcherResponse = resp.Item1;
                //msTimes.AddRange(resp.Item2);

                //startTime = DateTime.Now;

                //Map switcher response into NCPDP response
                TransmissionResponse transmissionResponse = new TransmissionResponse(switcherResponse);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                // map NCPDP Transmission Response back into the ClaimSubmission Response
                response = helper.MapClaimSubmissionResponseFromNcpdp<ClaimSubmissionResponse>(transmissionResponse);
                response.NcpdpRequestString = claim;
                response.NcpdpResponseString = switcherResponse;

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //Log switcher request and response strings
                await logNCPDPInfo($"{requestLoggingName} NCPDP Response", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "NCPDPResponseString", switcherResponse } }).ConfigureAwait(false);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                Request.Items.Add(Logging.Text_RespIdentifier2, $"See AppLog '{requestLoggingName} NCPDP Response'");
                Request.Items.Add(Logging.Text_RespIdentifier3, ((TransactionResponseStatusShort)response.Status.TransactionResponseStatus).ToString());
                //Request.Items.Add(Logging.Text_RespIdentifier4, $"Send AppLog '{requestLoggingName} Timings'");

                //DateTime processingEndTime = DateTime.Now;

                //Log additional timings
                //await logNCPDPInfo($"{requestLoggingName} Timings", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "Processing Start", processingStartTime.ToString() }, { "Processing End", processingEndTime.ToString() }, { "Timings", msTimes.Join(",") }, { "Total of Times Logged", msTimes.Sum().ToString() }, { "Processing Time Logged", (processingEndTime - processingStartTime).TotalMilliseconds.ToString() } }).ConfigureAwait(false);

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.TestClaimSubmissionRequest + "|" + ApiRoutes.TestClaimSubmissionRequest)]
        public async Task<TestClaimSubmissionResponse> Post(TestClaimSubmissionRequest request)
        {
            TestClaimSubmissionResponse response = null;
            request.Header.SoftwareId = "KEYED";

            response = await processRequest(request, ApiRoutes.TestClaimSubmissionRequest, async () =>
            {
                response = await submitClaim<TestClaimSubmissionResponse>(request, "Test Claim").ConfigureAwait(false);

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        private async Task<T> submitClaim<T>(ClaimSubmissionRequest request, string requestLoggingName)
            where T : ClaimSubmissionResponse, new()
        {
            ClaimSubmissionResponse response = new ClaimSubmissionResponse();

            Transmission Ncpdp = new Transmission();
            ClaimHelper helper = new ClaimHelper();
            TransmissionValidator validator = new TransmissionValidator();


            bool isCareMark = helper.IsCareMark(request.Header.BinNumber);
            bool isIlliniosMedicaid = helper.IsIllinoisMedicaid(request.Header.BinNumber, request.Header.ProcessorControlNumber);
            bool isExpressScripts = helper.IsExpressScripts(request.Header.BinNumber);

            if (isCareMark)
            {
                validator.ValidateAndThrow(request, GetClaimSubmissionValidationString("CareMark", request));
            }
            else if (isExpressScripts)
            {
                validator.ValidateAndThrow(request, GetClaimSubmissionValidationString("ExpressScripts", request));
            }
            else
            {
                validator.ValidateAndThrow(request);
            }

            //mapping request into NCPDP 
            Ncpdp = helper.MapSubmissionRequestClaimToNcpdp(request);

            if (isCareMark)
            {
                CareMarkPayerSheet caremark = new CareMarkPayerSheet();
                caremark.ValidateClaimSubmissionRequest(Ncpdp);
            }
            else if (isIlliniosMedicaid)
            {
                DefaultPayerSheet illMedicaid = new DefaultPayerSheet();
                illMedicaid.ValidateClaimSubmissionRequest(Ncpdp);
            }
            else if (isExpressScripts)
            {
                ExpressScriptsPayerSheet expressScripts = new ExpressScriptsPayerSheet();
                expressScripts.ValidateClaimSubmissionRequest(Ncpdp);
            }
            else
            {
                DefaultPayerSheet payersheet = new DefaultPayerSheet();
                payersheet.ValidateClaimSubmissionRequest(Ncpdp);
            }

            //convert NCPDP request into claim string
            string claim = Ncpdp.ToString();

            //Log claim string
            await logNCPDPInfo($"{requestLoggingName} NCPDP Request", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "NCPDPRequestString", claim } }).ConfigureAwait(false);
            Request.Items.Add(Logging.Text_RespIdentifier1, $"See AppLog '{requestLoggingName} NCPDP Request'");

            //Creates switcher response
            Tuple<string, List<double>> resp = await Switcher.Submit(claim, getReferenceStringSets(Ncpdp)).ConfigureAwait(false);
            string switcherResponse = resp.Item1;

            //Map switcher response into NCPDP response
            TransmissionResponse transmissionResponse = new TransmissionResponse(switcherResponse);

            // map NCPDP Transmission Response back into the ClaimSubmission Response
            response = helper.MapClaimSubmissionResponseFromNcpdp<T>(transmissionResponse);
            response.NcpdpRequestString = claim;
            response.NcpdpResponseString = switcherResponse;

            //Log switcher request and response strings
            await logNCPDPInfo($"{requestLoggingName} NCPDP Response", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "NCPDPResponseString", switcherResponse } }).ConfigureAwait(false);

            Request.Items.Add(Logging.Text_RespIdentifier2, $"See AppLog '{requestLoggingName} NCPDP Response'");
            Request.Items.Add(Logging.Text_RespIdentifier3, ((TransactionResponseStatusShort)response.Status.TransactionResponseStatus).ToString());
            //Request.Items.Add(Logging.Text_RespIdentifier4, $"Send Time: {sendTime}, Recv Time: {recvTime}");

            return response as T;
        }

        private string GetClaimSubmissionValidationString(string payerName, ClaimSubmissionRequest request)
        {
            StringBuilder validationBuilder = new StringBuilder();

            validationBuilder.AppendFormat("{0},default", payerName);

            if (!request.Pharmacy.ToString().Equals(new NCPDP.Telecom.Request.RequestPharmacy().ToString()))
            {
                validationBuilder.AppendFormat("{0}Pharmacy", payerName);
            }

            if (!request.DUR.ToString().Equals(new NCPDP.Telecom.Request.RequestDrugUtilizationReview().ToString()))
            {
                validationBuilder.AppendFormat("{0}DUR", payerName);
            }

            if (!request.Compound.ToString().Equals(new NCPDP.Telecom.Request.RequestCompound().ToString()))
            {
                validationBuilder.AppendFormat("{0}Compound", payerName);
            }

            if (!request.Clinical.ToString().Equals(new NCPDP.Telecom.Request.RequestClinical().ToString()))
            {
                validationBuilder.AppendFormat("{0}Clinical", payerName);
            }

            return validationBuilder.ToString();
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ClaimReversalRequest + "|" + ApiRoutes.ClaimReversalRequest)]
        public async Task<ClaimReversalResponse> Post(ClaimReversalRequest request)
        {
            //DateTime processingStartTime = DateTime.Now;
            ClaimReversalResponse response = null;
            //DateTime startTime = DateTime.Now;
            //List<double> msTimes = new List<double>();

            response = await processRequest(request, ApiRoutes.ClaimReversalRequest, async () =>
            {
                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                response = new ClaimReversalResponse();

                Transmission Ncpdp = new Transmission();
                ClaimHelper helper = new ClaimHelper();
                ClaimReversalValidator validator = new ClaimReversalValidator();

                bool isCareMark = helper.IsCareMark(request.Header.BinNumber);
                bool isIlliniosMedicaid = helper.IsIllinoisMedicaid(request.Header.BinNumber, request.Header.ProcessorControlNumber);
                bool isExpressScripts = helper.IsExpressScripts(request.Header.BinNumber);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                if (isCareMark)
                {
                    validator.ValidateAndThrow(request, "CareMark,default");
                }
                else if (isExpressScripts)
                {
                    validator.ValidateAndThrow(request, "ExpressScripts,default");
                }
                else
                {
                    validator.ValidateAndThrow(request);
                }

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //mapping request into NCPDP 
                Ncpdp = helper.MapClaimReversalRequestClaimToNcpdp(request);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                if (isCareMark)
                {
                    CareMarkPayerSheet caremark = new CareMarkPayerSheet();
                    caremark.ValidateClaimReversalRequest(Ncpdp);
                }
                else if (isIlliniosMedicaid)
                {
                    DefaultPayerSheet illMedicaid = new DefaultPayerSheet();
                    illMedicaid.ValidateClaimReversalRequest(Ncpdp);
                }
                else if (isExpressScripts)
                {
                    ExpressScriptsPayerSheet expressScripts = new ExpressScriptsPayerSheet();
                    expressScripts.ValidateClaimReversalRequest(Ncpdp);
                }
                else
                {
                    DefaultPayerSheet payersheet = new DefaultPayerSheet();
                    payersheet.ValidateClaimReversalRequest(Ncpdp);
                }

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //convert NCPDP request into claim string
                string claim = Ncpdp.ToString();

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //Log claim string
                await logNCPDPInfo("Claim Reversal NCPDP Request", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "NCPDPRequestString", claim } }).ConfigureAwait(false);
                Request.Items.Add(Logging.Text_RespIdentifier1, "See AppLog 'Claim Reversal NCPDP Request'");

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //Creates switcher response
                Tuple<string, List<double>> resp = await Switcher.Submit(claim, getReferenceStringSets(Ncpdp)).ConfigureAwait(false);
                string switcherResponse = resp.Item1;
                //msTimes.AddRange(resp.Item2);

                //startTime = DateTime.Now;

                //Map switcher response into NCPDP response
                TransmissionResponse transmissionResponse = new TransmissionResponse(switcherResponse);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                // map NCPDP Transmission Response back into the ClaimSubmission Response
                response = helper.MapClaimReversalResponseFromNcpdp(transmissionResponse);
                response.NcpdpRequestString = claim;
                response.NcpdpResponseString = switcherResponse;

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //Log switcher request and response strings
                await logNCPDPInfo("Claim Reversal NCPDP Response", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "NCPDPResponseString", switcherResponse } }).ConfigureAwait(false);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                Request.Items.Add(Logging.Text_RespIdentifier2, "See AppLog 'Claim Reversal NCPDP Response'");
                Request.Items.Add(Logging.Text_RespIdentifier3, ((TransactionResponseStatusShort)response.Status.TransactionResponseStatus).ToString());
                //Request.Items.Add(Logging.Text_RespIdentifier4, $"Send AppLog 'Claim Reversal Timings'");

                DateTime processingEndTime = DateTime.Now;

                //Log additional timings
                //await logNCPDPInfo($"Claim Reversal Timings", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "Processing Start", processingStartTime.ToString() }, { "Processing End", processingEndTime.ToString() }, { "Timings", msTimes.Join(",") }, { "Total of Times Logged", msTimes.Sum().ToString() }, { "Processing Time Logged", (processingEndTime - processingStartTime).TotalMilliseconds.ToString() } }).ConfigureAwait(false);

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        [Authenticate]
        [RequiresAnyPermission(ApiRoutes.ClaimEligibilityRequest + "|" + ApiRoutes.ClaimEligibilityRequest)]
        public async Task<ClaimEligibilityResponse> Post(ClaimEligibilityRequest request)
        {
            //DateTime processingStartTime = DateTime.Now;

            ClaimEligibilityResponse response = null;
            //DateTime startTime = DateTime.Now;
            //List<double> msTimes = new List<double>();

            response = await processRequest(request, ApiRoutes.ClaimEligibilityRequest, async () =>
            {
                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                response = new ClaimEligibilityResponse();

                ClaimHelper helper = new ClaimHelper();
                ClaimEligibilityValidator validator = new ClaimEligibilityValidator();
                Transmission Ncpdp = new Transmission();

                bool isCareMark = helper.IsCareMark(request.Header.BinNumber);
                bool isIlliniosMedicaid = helper.IsIllinoisMedicaid(request.Header.BinNumber, request.Header.ProcessorControlNumber);
                bool isExpressScripts = helper.IsExpressScripts(request.Header.BinNumber);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                if (isCareMark)
                {
                    validator.ValidateAndThrow(request, "CareMark,default");
                }
                else if (isExpressScripts)
                {
                    validator.ValidateAndThrow(request, "ExpressScripts,default");
                }
                else
                {
                    validator.ValidateAndThrow(request);
                }

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //mapping request into NCPDP 
                Ncpdp = helper.MapClaimEligibilityRequestClaimToNcpdp(request);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                if (isCareMark)
                {
                    CareMarkPayerSheet caremark = new CareMarkPayerSheet();
                    caremark.ValidateClaimEligibilityRequest(Ncpdp);
                }
                else if (isIlliniosMedicaid)
                {
                    DefaultPayerSheet illMedicaid = new DefaultPayerSheet();
                    illMedicaid.ValidateClaimEligibilityRequest(Ncpdp);
                }
                else if (isExpressScripts)
                {
                    ExpressScriptsPayerSheet expressScripts = new ExpressScriptsPayerSheet();
                    expressScripts.ValidateClaimEligibilityRequest(Ncpdp);
                }
                else
                {
                    DefaultPayerSheet payersheet = new DefaultPayerSheet();
                    payersheet.ValidateClaimEligibilityRequest(Ncpdp);
                }

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //convert NCPDP request into claim string
                string claim = Ncpdp.ToString();

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;


                //Log claim string
                await logNCPDPInfo("Claim Eligibility NCPDP Request", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "NCPDPRequestString", claim } }).ConfigureAwait(false);
                Request.Items.Add(Logging.Text_RespIdentifier1, "See AppLog 'Claim Eligibility NCPDP Request'");

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //Creates switcher response
                Tuple<string, List<double>> resp = await Switcher.Submit(claim, getReferenceStringSets(Ncpdp)).ConfigureAwait(false);
                string switcherResponse = resp.Item1;
                //msTimes.AddRange(resp.Item2);

                //startTime = DateTime.Now;

                //Map switcher response into NCPDP response
                TransmissionResponse transmissionResponse = new TransmissionResponse(switcherResponse);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                // map NCPDP Transmission Response back into the ClaimSubmission Response
                response = helper.MapClaimEligibilityResponseFromNcpdp(transmissionResponse);
                response.NcpdpRequestString = claim;
                response.NcpdpResponseString = switcherResponse;

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //Log switcher request and response strings
                await logNCPDPInfo("Claim Eligibility NCPDP Response", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "NCPDPResponseString", switcherResponse } }).ConfigureAwait(false);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                Request.Items.Add(Logging.Text_RespIdentifier2, "See AppLog 'Claim Eligibility NCPDP Response'");
                Request.Items.Add(Logging.Text_RespIdentifier3, ((TransactionResponseStatusShort)response.Status.TransactionResponseStatus).ToString());
                //Request.Items.Add(Logging.Text_RespIdentifier4, $"See AppLog 'Claim Eligibility Timings'");

                DateTime processingEndTime = DateTime.Now;

                //Log additional timings
                //await logNCPDPInfo($"Claim Eligibility Timings", new Dictionary<string, string> { { "APIMessageGUID", Request.Items["ApiMessageID"].ToString() }, { "Processing Start", processingStartTime.ToString() }, { "Processing End", processingEndTime.ToString() }, { "Timings", msTimes.Join(",") }, { "Total of Times Logged", msTimes.Sum().ToString() }, { "Processing Time Logged", (processingEndTime - processingStartTime).TotalMilliseconds.ToString() } }).ConfigureAwait(false);

                return response;
            }).ConfigureAwait(false);

            return response;
        }

        #endregion ClaimsProcessing

        #region Private Methods
        private async Task<T2> processRequest<T1, T2>(T1 request, string apiRoute, Func<Task<T2>> actionToCallIfValid = null)
            where T1 : BaseRequest
            where T2 : BaseResponse
        {
            T2 response;
            try
            {
                if (this.Request.Items.ContainsKey("IsValidRequest") && Convert.ToBoolean(this.Request.Items["IsValidRequest"]))
                {
                    response = await actionToCallIfValid().ConfigureAwait(false);
                }
                else
                {
                    response = (T2)((ServiceStack.Host.NetCore.NetCoreResponse)this.Response).Dto;
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> exceptionIdentifiers = new Dictionary<string, string> { { "ApiRequestID", Request.Items["ApiMessageID"].ToString() } };

                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
                await commonApiHelper.LogException(false, Enums.ApplicationSource.ClaimAPI, ex.Message, ex.StackTrace, exceptionIdentifiers, apiRoute).ConfigureAwait(false);
 
                if (ex is ValidationException)
                {
                    throw ex = new ArgumentException(ex.Message);
                }
                else
                {
                    throw ex = ex is ArgumentException || ex is TaskCanceledException || ex is OperationCanceledException ? ex : new Exception(FieldDescriptions.UnhandledExceptionError);
                }
            }

            return response;
        }

        private async Task logNCPDPInfo(string message, Dictionary<string, string> properties)
        {
            CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);
            await commonApiHelper.LogInfo(ApplicationSource.ClaimAPI, message, properties).ConfigureAwait(false);
        }

        private ReferenceStringSet getReferenceStringSets(Transmission ncpdp)
        {
            ReferenceStringSet referenceStringSet = new ReferenceStringSet
            {
                TransactionID = getTransactionID(),
                BinNumber = ncpdp.Header.BinNumber,
                VersionNumber = ncpdp.Header.VersionNumber.ToOutputString(),
                TransactionType = NCPDP.Telecom.Extensions.StringExtensions.TransactionCodeToString(ncpdp.Header.TransactionCode),
                ReferenceStrings = new List<string>
                {
                    ncpdp.Header.DateOfService.ToNCPDPDateString()
                }
            };

            if(ncpdp.Header.TransactionCode != TransactionCode.EligibilityVerification)
            {   
                try
                {
                    if (!string.IsNullOrWhiteSpace(ncpdp.Header.ServiceProviderId))
                    {
                        referenceStringSet.ReferenceStrings.Add(ValidatorHelper.FormatReferenceStringSet(ncpdp.Header.ServiceProviderId));
                    }
                }
                catch (Exception) { }
            }

            //if(ncpdp.Header.TransactionCode == TransactionCode.Billing)
            //try
            //{
            //    if (!string.IsNullOrWhiteSpace(ncpdp.CurrentTransaction.Claim.PrescriptionNumber))
            //    {
            //        referenceStringSet.ReferenceStrings.Add(ValidatorHelper.FormatReferenceStringSet(ncpdp.CurrentTransaction.Claim.PrescriptionNumber));
            //    }
            //}
            //catch (Exception) { }

            return referenceStringSet;
        }

        private static string getTransactionID()
        {
            string transactionID = "";
            lock (transactionCounterLock)
            {
                transactionCounter++;
                if (transactionCounter > 9999)
                {
                    transactionCounter = 1;
                }

                transactionID = transactionCounter.ToString("0000");
            }

            return transactionID;
        }
        #endregion Private Methods
    }
}
