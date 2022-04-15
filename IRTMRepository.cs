using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public interface IRTMRepository
    {
        Task ProcessSendRTMData(string adsConnectionString, List<string> masterRTMFields, List<string> masterMiscFields, 
            List<string> clientRTMFields, List<string> clientMiscFields, string clientCode, string enableRecordLogging, Dictionary<string, string> clientApiSettings);
    }
}