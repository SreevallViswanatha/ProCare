using ProCare.API.Claims.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace ProCare.API.Claim.Claims
{
    public class ReferenceStringSet
    {
        public string TransactionID { get; set; }
        public string BinNumber { get; set; }
        public string VersionNumber { get; set; }
        public string TransactionType { get; set; }
        public List<string> ReferenceStrings { get; set; }

        public string ReferenceHeader => ValidatorHelper.AlphanumericCharactersOnly(BinNumber + TransactionID + VersionNumber + TransactionType);
    }
}
