using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Services
{
    public class RTM_API_Response
    {
        public string ALTID { get; set; }
        public RTM_APIResponseStatus Status{ get; set; }
    }

    public enum RTM_APIResponseStatus
    {
        Sucess = 0,
        Fail = 888,
    }
}
