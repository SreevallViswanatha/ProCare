using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;



namespace ProCare.API.Claims.Claims
{
    public class ClaimDataAccess : DataAccessBase
    {
        public ClaimDataAccess(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        public string GetSoftwareIdByAnsiBin(string AnsiBin, string ProcessorControlNumber)
        {
            string result = string.Empty;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@ANSIBin", AnsiBin},
                {"@PCN", ProcessorControlNumber}
            };

            DataHelper.ExecuteReader("apiClaim_ClientConfig_readByANSIBin", CommandType.StoredProcedure, parameters,
                                     reader => { result = reader.GetStringorDefault("SoftwareID"); });

            return result;
        }
    }
}
