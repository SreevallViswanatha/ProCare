using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Services
{
    interface IRTM_API
    {
        Task<RTM_API_Response> SendRTMData(Dictionary<string, string> apiSettings,RTMRecordDTO rtmRecord, string clientCode);
    }
}
