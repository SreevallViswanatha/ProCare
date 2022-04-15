using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberPortalLookupDTO : ILoadFromDataReader
    {
        public bool AllowsWebAccess { get; set; }
        public bool TestMode { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            List<string> validWebAccessValues = new List<string>{"Y", "T"};
            string webAccess = reader.GetStringorDefault("WEBACCESS").ToUpper().Trim();
            AllowsWebAccess = validWebAccessValues.Contains(webAccess);
            TestMode = webAccess.Equals("T");
        }
    }
}
