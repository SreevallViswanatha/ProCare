using Encrypt2;
using ProCare.API.Claim.Claims;
using ProCare.API.Claims.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProCare.API.Claims.Claims
{
    public class Switcher
    {
        private const string CstrHeader = "POETS3/ICMW{0}";
        private const char SOH = (char)(0x01);
        private const char STX = (char)(0x02);
        private const char ETX = (char)(0x03);
        private const char EOH = (char)(0x04);
        private static Encoding switcherEncoding = CodePagesEncodingProvider.Instance.GetEncoding(1252);

        private static PRXCrypto crypt = null;
        private static readonly object cryptoLock = new object();
        private static readonly object switcherLock = new object();
        //private static readonly object keyOneLock = new object();
        //private static readonly object keyTwoLock = new object();
        private static readonly object responseListLock = new object();
        private static TcpClient switcherObj = null;
        private static Socket sock = SwitcherInstance.Client;

        //private static byte[] keyOne = null;
        //private static int? keyTwo = null;
        private static List<SwitcherResponse> unmatchedSwitcherResponses = new List<SwitcherResponse>();

        public static PRXCrypto CryptInstance
        {
            get
            {
                if (crypt == null)
                {
                    lock (cryptoLock)
                    {
                        if (crypt == null)
                        {
                            crypt = new PRXCrypto();
                        }
                    }
                }

                return crypt;
            }
            set => crypt = value;
        }

        public static TcpClient SwitcherInstance
        {
            get
            {
                if (switcherObj == null)
                {
                    lock (switcherLock)
                    {
                        if (switcherObj == null)
                        {
                            switcherObj = OpenSwitcher();
                            sock = SwitcherInstance.Client;
                        }
                    }
                }

                return switcherObj;
            }
            set => switcherObj = value;
        }

        //public static byte[] KeyOne
        //{
        //    get
        //    {
        //        SetKeyOne(false);

        //        return keyOne;
        //    }
        //    set => keyOne = value;
        //}

        //public static int KeyTwo
        //{
        //    get
        //    {
        //        SetKeyTwo(false);

        //        return keyTwo.Value;
        //    }
        //    set => keyTwo = value;
        //}

        //private static void SetKeyOne(bool forceReset)
        //{
        //    if (keyOne == null || forceReset)
        //    {
        //        lock (keyOneLock)
        //        {
        //            if (keyOne == null || forceReset)
        //            {
        //                //Generate Key1 if we don't have one, this is shared across all claim sends
        //                string cInitiateString = string.Empty;
        //                cInitiateString += (char)(0x02);
        //                cInitiateString += (char)(0x1F);

        //                string cResponseString = "";

        //                lock (switcherLock)
        //                {
        //                    SendToSwitcher(sock, cInitiateString);
        //                    cResponseString = ReadKeyFromSwitcher(sock).Result;
        //                }

        //                if ((cResponseString.Length > 0) && ((cResponseString[0] == SOH) && (cResponseString[cResponseString.Length - 1] == EOH)))
        //                {
        //                    // --------------------------------------------------------
        //                    // Build 1st key to send to Switcher
        //                    // --------------------------------------------------------
        //                    keyOne = new byte[sock.ReceiveBufferSize];
        //                    cResponseString += "\0";

        //                    //int keyLength = CryptInstance.initkey1(switcherEncoding.GetBytes(cResponseString), ref keyOne);
        //                    //if (keyLength < 0)
        //                    //{
        //                    //    throw new ApplicationException("Exception calling Communication support dll, InitKey1");
        //                    //}
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void SetKeyTwo(bool forceReset)
        //{
        //    if (keyTwo == null || forceReset)
        //    {
        //        lock (keyTwoLock)
        //        {
        //            if (keyTwo == null || forceReset)
        //            {
        //                string cResponseString = switcherEncoding.GetString(KeyOne);

        //                lock (switcherLock)
        //                {
        //                    SendToSwitcher(sock, cResponseString);
        //                    cResponseString = ReadKeyFromSwitcher(sock).Result;
        //                }

        //                if ((cResponseString.Length > 0) && ((cResponseString[0] == SOH) && (cResponseString[cResponseString.Length - 1] == EOH)))
        //                {
        //                    // --------------------------------------------------------
        //                    // Build 2nd key to send to Switcher
        //                    // --------------------------------------------------------

        //                    cResponseString += "\0";

        //                    //keyTwo = CryptInstance.initkey2(switcherEncoding.GetBytes(cResponseString));
        //                    //if (keyTwo < 0)
        //                    //{
        //                    //    throw new ApplicationException("Exception calling Communication support dll, InitKey2");
        //                    //}
        //                }
        //            }
        //        }
        //    }
        //}

        public static async Task<Tuple<string, List<double>>> Submit(string claim, ReferenceStringSet referenceStringSet)
        {
            //DateTime startTime = DateTime.Now;
            List<double> msTimes = new List<double>();
            Guid fileGUID = Guid.NewGuid();
            Tuple<string, List<double>> resp = new Tuple<string, List<double>>("", new List<double>());

            ReconnectSwitcherIfDisconnected();

            //using (StreamWriter fileOut = new StreamWriter(String.Format(ApplicationSettings.ClaimLogPathAndFileFormat, DateTime.Now.ToString("yyMMdd-hhmm_"), fileGUID.ToString())))
            //{
                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                string strSwitcherRequest = BuildSwitcherRequest(claim, referenceStringSet.TransactionID);

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //fileOut.Write(strSwitcherRequest);
                //fileOut.WriteLine();

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                string strSwitcherResponse = string.Empty;
                try
                {

                    //SetKeyOne(false);
                    //SetKeyTwo(false);

                    //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                    //startTime = DateTime.Now;

                    resp = await SendSwitcherMessage(strSwitcherRequest, referenceStringSet).ConfigureAwait(false);
                    strSwitcherResponse = resp.Item1;

                    //msTimes.AddRange(resp.Item2);
                    //startTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    ReconnectSwitcherIfDisconnected();

                    throw ex;
                }

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                //fileOut.Write(strSwitcherResponse);
                //fileOut.WriteLine();

                //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
                //startTime = DateTime.Now;

                resp = new Tuple<string, List<double>>(resp.Item1, msTimes);

                return resp;
            //}
        }

        private static void ReconnectSwitcherIfDisconnected()
        {
            if (!SocketIsConnected() || !(SwitcherInstance?.Connected ?? false) || !(SwitcherInstance?.Client?.Connected ?? false))
            {
                lock (switcherLock)
                {
                    if (!SocketIsConnected() || !(SwitcherInstance?.Connected ?? false) || !(SwitcherInstance?.Client?.Connected ?? false))
                    {
                        try
                        {
                            SwitcherInstance.Close();
                        }
                        catch (Exception) { }

                        SwitcherInstance = OpenSwitcher();

                        sock = SwitcherInstance.Client;

                        //SetKeyOne(true);
                        //SetKeyTwo(true);
                    }
                }
            }
        }

        private static bool SocketIsConnected()
        {
            bool connected = false;

            try
            {
                connected = !(sock.Poll(1, SelectMode.SelectRead) && sock.Available == 0);
            }
            catch (SocketException){ }

            return connected;
        }

        public static string BuildSwitcherRequest(string strNCPDPRequest, string transactionId)
        {
            // ---------------------------------------------------------------------
            // Build the ProCare Rx Switcher Request from the NCPDP Request.
            // ---------------------------------------------------------------------
            string strHeader = String.Format(CstrHeader, transactionId);
            string strSwitcherRequest = STX + strHeader + strNCPDPRequest + ETX;
            return strSwitcherRequest;
        }

        public static async Task<Tuple<string, List<double>>> SendSwitcherMessage(string strSwitcherRequest, ReferenceStringSet referenceStringSet)
        {
            // ---------------------------------------------------------------------
            // Transmit a ProCare Rx Switcher Request to the ProCare Rx Switcher.
            // ---------------------------------------------------------------------
            DateTime startTime = DateTime.Now;
            List<double> msTimes = new List<double>();

            string result = string.Empty;

            // --------------------------------------------------------
            // Encrypt Claim
            // --------------------------------------------------------
            string claimString = strSwitcherRequest;
            claimString += "\0";

            int keyLength = 0;

            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
            //startTime = DateTime.Now;

            var keyBuffer = switcherEncoding.GetBytes(claimString);

            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
            //startTime = DateTime.Now;

            // --------------------------------------------------------------
            // rebuild the claim string (from EncryptionArray array) to send to switcher
            // -------------------------------------------------------------
            string encryptedClaim = "";
            string cResponseString = "";

            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
            //startTime = DateTime.Now;

            encryptedClaim = switcherEncoding.GetString(keyBuffer);

            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
            //startTime = DateTime.Now;

            // -------------------------------------------------------------
            // Send/Transmit the actual claim to switcher
            // ------------------------------------------------------------

            
            SendToSwitcher(sock, encryptedClaim);

            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
            //startTime = DateTime.Now;

            // ------------------------------------------------------------
            // Read the Response from Switcher & Online
            // ------------------------------------------------------------
            cResponseString = await FindSwitcherResponse(sock, referenceStringSet).ConfigureAwait(false);
            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
            //startTime = DateTime.Now;

            // -----------------------------------------------------------
            // Decrypt the response from Switcher & Online
            // -----------------------------------------------------------
            keyBuffer = switcherEncoding.GetBytes(cResponseString);

            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
            //startTime = DateTime.Now;


            int endingDelimiter = switcherEncoding.GetString(keyBuffer).IndexOf(ETX);

            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);
            //startTime = DateTime.Now;

            if (endingDelimiter > 0)
            {
                result = switcherEncoding.GetString(keyBuffer).Substring(0, endingDelimiter);
            }

            //msTimes.Add((DateTime.Now - startTime).TotalMilliseconds);

            return new Tuple<string, List<double>>(result, msTimes);
        }

        private static TcpClient OpenSwitcher()
        {
            SwitcherSettings settings = new SwitcherSettings();

            string switcherHost = settings.SwitcherUrl; //** Need URL - add value to SwitcherSettings class below;

            if (string.IsNullOrWhiteSpace(switcherHost))
            {
                throw new ApplicationException("Missing Switcher Host Configuration");
            }

            int switcherPort = settings.SwitcherPort;  //** Need Port - add  value to SwitcherSettings class below;

            if (switcherPort == 0)
            {
                throw new ApplicationException("Invalid Switcher Port Configuration");
            }

            TcpClient switcher = new TcpClient();

            switcher.Connect(switcherHost, switcherPort);

            switcher.Client.NoDelay = settings.NoDelay;
            switcher.SendTimeout = settings.SendTimeout;
            switcher.ReceiveTimeout = settings.ReceiveTimeout;
            switcher.ReceiveBufferSize = settings.ReceiveBufferSize;
            switcher.SendBufferSize = settings.SendBufferSize;

            return switcher;
        }

        private static void SendToSwitcher(Socket sock, string message)
        {
            Byte[] abytSwitcherRequestBuffer;
            try
            {
                if (!message.EndsWith(EOH.ToString()))
                {
                    message += EOH;
                }
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                abytSwitcherRequestBuffer = switcherEncoding.GetBytes(message);
                sock.Send(abytSwitcherRequestBuffer, abytSwitcherRequestBuffer.Length, SocketFlags.None);
            }
            finally
            {
                abytSwitcherRequestBuffer = null;
            }
        }

        public static async Task<string> FindSwitcherResponse(Socket sock, ReferenceStringSet referenceStringSet)
        {
            string result = "";

            CancellationToken token = getCancellationToken(TimeSpan.FromMilliseconds(SwitcherInstance.ReceiveTimeout));
            Task<string> t = PollForResponse(sock, referenceStringSet, token);
            t.Wait(token);

            if (!t.IsCanceled)
            {
                result = t.Result;
            }

            return ValidateSwitcherResponse(result);
        }

        public static async Task<string> ProcessSingleSwitcherResponse(Socket sock, ReferenceStringSet referenceStringSet, bool checkAvailable)
        {
            //Get switcher response
            string result = await Task.Run(() => GetFullSwitcherResponse(sock, checkAvailable)).ConfigureAwait(false);

            //Response is not for this request? Add to response list and wait for our response to finish processing
            if (!string.IsNullOrWhiteSpace(result) && !ResponseMatchesRequest(result, referenceStringSet))
            {
                AddResponseToUnmatchedResponseList(result);
                result = "";
            }

            return result;
        }

        public static void AddResponseToUnmatchedResponseList(string result)
        {
            PurgeOldUnmatchedResponses();
            AddToUnmatchedResponseList(result);
        }

        public static bool ResponseMatchesRequest(string result, ReferenceStringSet referenceStringSet)
        {
            string formattedResponse = ValidatorHelper.FormatSwitcherResponseForReferenceStringSetCompare(result, referenceStringSet);

            bool responseMatches = formattedResponse.StartsWith(referenceStringSet.ReferenceHeader);

            if(responseMatches)
            {
                responseMatches = referenceStringSet.ReferenceStrings.All(x => formattedResponse.Contains(x));
            }

            return responseMatches;
        }

        public static void PurgeOldUnmatchedResponses()
        {
            lock (responseListLock)
            {
                //Remove responses more than two minutes old to prevent failed requests from hanging around indefinitely
                unmatchedSwitcherResponses.RemoveAll(x => (DateTime.Now - x.Timestamp).Seconds > ApplicationSettings.UnmatchedResponseHistorySeconds);
            }
        }

        public static void AddToUnmatchedResponseList(string nonMatchingResult)
        {
            lock (responseListLock)
            {
                unmatchedSwitcherResponses.Add(new SwitcherResponse(nonMatchingResult, DateTime.Now));
            }
        }

        public static async Task<string> PollForResponse(Socket sock, ReferenceStringSet referenceStringSet, CancellationToken token)
        {
            string result = await ProcessSingleSwitcherResponse(sock, referenceStringSet, true).ConfigureAwait(false);
            bool responseFound = !string.IsNullOrWhiteSpace(result);

            while (!responseFound)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(ApplicationSettings.ResponsePollDelayMilliseconds)).ConfigureAwait(false);

                    //Check the list of responses that were already retrieved from the switcher
                    if (!token.IsCancellationRequested)
                    {
                        result = CheckResponsesList(referenceStringSet);
                        responseFound = !string.IsNullOrWhiteSpace(result);
                    }

                    //If the response was not in the response list
                    //Or the response list has not had any new responses added in a few seconds
                    //Try to get another response from the switcher
                    if (!responseFound && !token.IsCancellationRequested)
                    {
                        result = await ProcessSingleSwitcherResponse(sock, referenceStringSet, true).ConfigureAwait(false);
                        responseFound = !string.IsNullOrWhiteSpace(result);
                    }

                    //Exit if polling task was canceled
                    if(token.IsCancellationRequested)
                    {
                        responseFound = true;
                    }
                }
                catch (Exception) { }
            }

            return result;
        }

        public static string CheckResponsesList(ReferenceStringSet referenceStringSet)
        {
            string result = "";


            SwitcherResponse matchedResponse = null;

            try
            {
                matchedResponse = unmatchedSwitcherResponses
                                        .Where(x => ResponseMatchesRequest(x.Value, referenceStringSet))
                                        .ToList().FirstOrDefault();
            }
            catch (Exception) { }

            if (matchedResponse != null && !string.IsNullOrWhiteSpace(matchedResponse.Value))
            {
                lock (responseListLock)
                {
                    if (unmatchedSwitcherResponses.Contains(matchedResponse))
                    {
                        result = matchedResponse.Value;
                        unmatchedSwitcherResponses.Remove(matchedResponse);
                    }
                }
            }

            return result;
        }

        public static string GetFullSwitcherResponse(Socket sock, bool checkAvailable)
        {
            string result = string.Empty;
            byte[] abytSwitcherResponseBuffer = new byte[sock.ReceiveBufferSize];

            //Receive response
            if (!checkAvailable || sock.Available > 0)
            {
                try
                {
                    lock (switcherLock)
                    {
                        if (!checkAvailable || sock.Available > 0)
                        {
                            while (result.IndexOf(EOH) < 0 && result.IndexOf(ETX) < 0 && result.LastIndexOf(STX) <= result.IndexOf(ETX))
                            {
                                sock.Receive(abytSwitcherResponseBuffer, abytSwitcherResponseBuffer.Length, SocketFlags.None);
                                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                                result += switcherEncoding.GetString(abytSwitcherResponseBuffer);
                            }
                        }
                    }

                    List<string> results = result.Split(ETX).ToList();
                    result = results.First() + ETX;

                    //It's possible to receive more than one response
                    //If that happens, we want to make sure that all responses received are received by the correct requests
                    int responseCount = results.Count(x => x.Contains(STX));
                    if (responseCount > 1)
                    {
                        var additionalResponses = results.Skip(1).Take(responseCount - 1).Select(x => new SwitcherResponse(x + ETX, DateTime.Now));
                        lock(responseListLock)
                        {
                            unmatchedSwitcherResponses.AddRange(additionalResponses);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    abytSwitcherResponseBuffer = null;
                }
            }

            return result;
        }

        public static string ValidateSwitcherResponse(string switcherResponse)
        {
            string result = switcherResponse;

            if (switcherResponse.Length > 0)
            {
                int indx = switcherResponse.IndexOf(SOH);
                if (indx > 0)
                {
                    result = switcherResponse.Substring(indx, switcherResponse.Length - indx);
                }

                indx = result.IndexOf(EOH);

                if (indx > 0)
                {
                    result = result.Substring(0, indx + 1);
                }

                if (!result.EndsWith(EOH.ToString()))
                {
                    result += EOH;
                }
            }
            return result;
        }

        public static CancellationToken getCancellationToken(TimeSpan timespan)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(timespan);

            return cancellationTokenSource.Token;
        }
    }

    public class SwitcherSettings
    {
        public string SwitcherUrl { get; set; }
        public int SwitcherPort { get; set; }
        public bool DontFragment { get; set; } = true;
        public bool NoDelay { get; set; } = true;
        public int SendTimeout { get; set; } = 15000;
        public int ReceiveTimeout { get; set; } = 30000;
        public int ReceiveBufferSize { get; set; } = 3001;
        public int SendBufferSize { get; set; } = 3001;

        public SwitcherSettings()
        {
            SwitcherUrl = ApplicationSettings.SwitcherHost;
            SwitcherPort = ApplicationSettings.SwitcherPort;
            SendTimeout = ApplicationSettings.SwitcherSendTimeoutMilliseconds;
            ReceiveTimeout = ApplicationSettings.SwitcherReceiveTimeoutMilliseconds;
        }
    }
}
