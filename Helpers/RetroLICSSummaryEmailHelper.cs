using ProCare.API.Core.Constants;
using ProCare.API.PBM.Exceptions;
using ProCare.API.PBM.Repository.DTO;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProCare.API.PBM.Helpers
{
    public static class RetroLICSSummaryEmailHelper
    {
        #region Private Variables

        private const string FailureText = "Failure";
        private const string ErrorText = "Error";
        private const string RejectText = "Reject";
        private const string NeedsRerunText = "Needs Rerun";
        private const string SuccessText = "Success";
        private const string EmailStyling_BatchSummary = @"<style>
                                            p {
	                                            margin-bottom:5pt;
	                                            margin-top: 0pt;
	                                            font-family:'Calibri',sans-serif;
	                                            font-size:10.0pt;
                                            }

                                            table {
	                                            border-collapse:collapse;
	                                            border:none
                                            }

                                            td {
	                                            margin:0in;
	                                            font-family:'Calibri',sans-serif;
	                                            border:solid windowtext 1.0pt;
	                                            padding:2pt 5.4pt 2pt 5.4pt;
                                            }

                                            .failure-text {
	                                            color:red;
	                                            font-weight:bold;
                                            }

                                            .needs-rerun-text {
	                                            background:yellow;
	                                            mso-highlight:yellow;
	                                            font-weight:bold;
                                            }

                                            .medium-width-summary-data {
	                                            width:3in;
                                            }

                                            .large-width-summary-data {
	                                            width:5in;
                                            }

                                            #email-header-text {
	                                            font-size:14.0pt;
	                                            font-weight: bold;
                                            }

                                            #summary-header-row {
	                                            text-align:center;
	                                            font-weight:bold;
	                                            background:#FFF2CC;
                                            }
                                            </style>";
        #endregion Private Variables

        private static List<RetroLICSProcessingException.RetroLICSProcessingExceptionType> RejectExceptions =
            new List<RetroLICSProcessingException.RetroLICSProcessingExceptionType>
            {
                RetroLICSProcessingException.RetroLICSProcessingExceptionType.GapInEligibility,
                RetroLICSProcessingException.RetroLICSProcessingExceptionType.ClaimReversalFailed,
                RetroLICSProcessingException.RetroLICSProcessingExceptionType.ClaimSubmissionFailed
            };

        public static void SendRetroLICSSummaryEmail(string hostName, string smtpPort, string emailFrom, string emailCopyTo, string emailTo, string subject, RetroLICSProcessOutputDTO processingResults, DateTime startTime, bool isBatch)
        {
            RetroLICSProcessingException exception = processingResults.ProcessingException;
            List<string> nonZeroAccums = processingResults.NonZeroAccums;
            string enrolleeId = processingResults.EnrolleeID;

            string message =
                $"Retro LICS Processing for {(isBatch ? "" : "Enrollee ")}{enrolleeId} Completed<br/><br/>" +
                $"Ran from {startTime:MM/dd/yyyy HH:mm:ss} to {DateTime.Now:MM/dd/yyyy HH:mm:ss}<br/>" +
                $"Result: {(exception == null ? "SUCCESS" : "FAILURE")}";

            if (nonZeroAccums.Any())
            {
                message = message + GetWarningText(nonZeroAccums);
            }

            if (exception != null)
            {
                subject = string.Format(subject, "FAILURE");

                message = message + "<br/><br/>" +
                          $"Error Type: {exception.ExceptionType.ToString()}" + "<br/><br/>" +
                          $"Reason: {exception.GetBaseException().Message}" + "<br/><br/>" +
                          $"Inner Exception: {exception.ErrorReason}" + "<br/><br/>";
                if (!isBatch)
                {
                    message = message + $"Member Locked: {GetLockText(exception.MemberLocked, false)}";
                }

                if (exception.ExceptionType == RetroLICSProcessingException.RetroLICSProcessingExceptionType.UnexpectedError)
                {
                    message = message + "<br/><br/>" +
                              $"Stack Trace: {exception.GetBaseException().StackTrace}";
                }
                else if (exception.Data != null &&
                        (
                            exception.ExceptionType == RetroLICSProcessingException.RetroLICSProcessingExceptionType.NonZeroAccumulatorAfterReversals
                            || exception.ExceptionType == RetroLICSProcessingException.RetroLICSProcessingExceptionType.AccumulatorRebuildFailed
                        ))
                {
                    message = message + "<br/><br/>" +
                              $"Non-Zero Accumulators: {((List<string>)exception.Data).Join(", ")}";
                }

            }
            else
            {
                if (!processingResults.MemberProcessingAlreadyLocked && processingResults.ReverseClaimsExist)
                {
                    subject = string.Format(subject, "NEEDS RERUN");
                }
                else
                {
                    subject = string.Format(subject, "SUCCESS");
                }
            }

            EmailHelper.SendNotificationEmail(hostName, smtpPort, emailFrom, emailTo, message, subject, emailCopyTo);
        }

        public static void SendRetroLICSBatchSummaryEmail(string hostName, string smtpPort, string emailFrom, string emailCopyTo, string emailTo, string subject, List<RetroLICSProcessOutputDTO> processingResults, DateTime startTime, int? currentBatch, int? totalBatches)
        {
            string message = EmailStyling_BatchSummary;

            message = message + $@"<p id='email-header-text'>Retro LICS Processing Batch Summary {GetBatchText(currentBatch, totalBatches)}</p>
                                    <p>Records with {GetFailureText()} results require more attention to correct issues and reinitiate processing for those enrollees.</p>
                                    <p>Records with {GetNeedsRerunText()} results need to be reprocessed to ensure all claims and accumulators are up to date after manual changes.</p>";

            message = message + @"<table>
                                   <tr id='summary-header-row'>
                                      <td>Enrollee ID</td>
                                      <td>Result</td>
                                      <td>Lock Status</td>
                                      <td>Message</td>
                                      <td>Inner Exception</td>
                                   </tr>";

            processingResults.ForEach(result =>
            {
                RetroLICSProcessingResultType resultType = getResultType(result);

                message = message + $@"<tr>
                                          <td>{result.EnrolleeID}</td>
                                          <td>{GetResultText(resultType)}</td>
                                          <td>{GetLockText(result.MemberLocked, true)}</td>
                                          <td class='medium-width-summary-data'>{GetReasonText(result, resultType)}</td>
                                          <td class='large-width-summary-data'>{GetInnerExceptionText(result)}</td>
                                       </tr>";
            });

            message = message + "</table>";

            EmailHelper.SendNotificationEmail(hostName, smtpPort, emailFrom, emailTo, message, string.Format(subject, "BATCH"), emailCopyTo);
        }

        public static void SendBatchErrorEmail(string requestingUserEmail, DateTime startTime, RetroLICSProcessingException ex, Dictionary<string, string> retroLICSReprocessConfigs)
        {
            var x = new RetroLICSProcessOutputDTO
            {
                EnrolleeID = ex.EnrolleeID,
                ProcessingException = ex,
                NonZeroAccums = new List<string>(),
                MemberLocked = false
            };

            SendRetroLICSSummaryEmail(retroLICSReprocessConfigs[ConfigSetttingKey.SMTPServer],
                                                                  retroLICSReprocessConfigs[ConfigSetttingKey.SMTPPort],
                                                                  retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailFromName],
                                                                  retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailCopyToName],
                                                                  requestingUserEmail,
                                                                  retroLICSReprocessConfigs[ConfigSetttingKey.PBMAPI_RetroLICS_EmailSubjectTemplate],
                                                                  x,
                                                                  startTime,
                                                                  true);
        }

        private static RetroLICSProcessingResultType getResultType(RetroLICSProcessOutputDTO result)
        {
            RetroLICSProcessingResultType resultType;

            if (result.ProcessingException == null)
            {
                resultType = result.MemberProcessingAlreadyLocked ?
                    RetroLICSProcessingResultType.Success_RerunNeeded :
                    RetroLICSProcessingResultType.Success;
            }
            else if (RejectExceptions.Contains(result.ProcessingException.ExceptionType))
            {
                resultType = RetroLICSProcessingResultType.Failure_Reject;
            }
            else if (result.ProcessingException.ExceptionType ==
                     RetroLICSProcessingException.RetroLICSProcessingExceptionType.UnexpectedError)
            {
                resultType = RetroLICSProcessingResultType.Failure_UnexpectedError;
            }
            else
            {
                resultType = RetroLICSProcessingResultType.Failure_ExpectedError;
            }

            return resultType;
        }

        public static string GetLockText(bool memberIsLocked, bool useFormatting)
        {
            string lockText = memberIsLocked ? "Locked" : "Unlocked";

            if (useFormatting && memberIsLocked)
            {
                lockText = GetRedText(lockText);
            }
            return lockText;
        }

        public static string GetReasonText(RetroLICSProcessOutputDTO result, RetroLICSProcessingResultType resultType)
        {
            string reasonText = "";

            switch (resultType)
            {
                case RetroLICSProcessingResultType.Success:
                case RetroLICSProcessingResultType.Success_RerunNeeded:
                    {
                        if (result.NonZeroAccums.Any())
                        {
                            reasonText = GetWarningText(result.NonZeroAccums);
                        }

                        break;
                    }
                case RetroLICSProcessingResultType.Failure_Reject:
                case RetroLICSProcessingResultType.Failure_ExpectedError:
                    {
                        reasonText = result.ProcessingException.ExceptionType + " - " +
                                     result.ProcessingException.GetBaseException().Message;
                        break;
                    }
                default:
                    {
                        reasonText = result.ProcessingException.GetBaseException().Message;
                        break;
                    }
            }

            return reasonText;
        }

        public static string GetInnerExceptionText(RetroLICSProcessOutputDTO result)
        {
            string innerExceptionText = "";

            if (result.ProcessingException != null)
            {
                innerExceptionText = result.ProcessingException.ErrorReason;
            }

            return innerExceptionText;
        }

        public static string GetWarningText(List<string> nonZeroAccums)
        {
            return "<br/>WARNING: Process had to force accumulators to zero.<br/>" +
                   $"Non-Zero Accumulators: {nonZeroAccums.Join(", ")}";
        }

        public static string GetResultText(RetroLICSProcessingResultType resultType)
        {
            string resultText;

            switch (resultType)

            {
                case RetroLICSProcessingResultType.Success:
                    {
                        resultText = GetSuccessText();
                        break;
                    }
                case RetroLICSProcessingResultType.Success_RerunNeeded:
                    {
                        resultText = GetNeedsRerunText();
                        break;
                    }
                case RetroLICSProcessingResultType.Failure_Reject:
                    {
                        resultText = GetRejectText();
                        break;
                    }
                default:
                    {
                        resultText = GetErrorText();
                        break;
                    }
            }

            return resultText;
        }

        public static string GetSuccessText()
        {
            return SuccessText;
        }

        public static string GetRejectText()
        {
            return GetFailureText(RejectText);
        }

        public static string GetErrorText()
        {
            return GetFailureText(ErrorText);
        }

        public static string GetFailureText(string failureType = "")
        {
            string failureText = FailureText;
            if (!string.IsNullOrWhiteSpace(failureType))

            {
                failureText = failureText + $" - {failureType}";
            }
            return GetRedText(failureText);
        }

        public static string GetNeedsRerunText()
        {
            return GetHighlightedText(NeedsRerunText);
        }

        public static string GetBatchText(int? currentBatch, int? totalBatches)
        {
            string batchText = "";

            if (currentBatch.HasValue && totalBatches.HasValue)
            {
                batchText = $"(Batch {currentBatch.Value} of {totalBatches.Value})";
            }

            return batchText;
        }

        public static string GetHighlightedText(string text)
        {
            return getClassText("needs-rerun-text", text);
        }

        public static string GetRedText(string text)
        {
            return getClassText("failure-text", text);
        }

        private static string getClassText(string className, string text)
        {
            return $"<span class='{className}'>{text}</span>";
        }
    }
}
