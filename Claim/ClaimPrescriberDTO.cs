using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimPrescriberDTO : ILoadFromDataReader
    {
        public string SubmittedPrescriberID { get; set; }
        public string SubmittedPrescriberIDQualifier { get; set; }
        public string State { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            SubmittedPrescriberID = reader.GetStringorDefault("SubmittedPrescriberID");
            SubmittedPrescriberIDQualifier = reader.GetStringorDefault("SubmittedPrescriberIDQualifier");
            State = reader.GetStringorDefault("State");
            FirstName = reader.GetStringorDefault("FirstName");
            LastName = reader.GetStringorDefault("LastName");
            Phone = reader.GetStringorDefault("Phone");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            SubmittedPrescriberID = reader.GetStringorDefault(prefix + "SubmittedPrescriberID");
            SubmittedPrescriberIDQualifier = reader.GetStringorDefault(prefix + "SubmittedPrescriberIDQualifier");
            State = reader.GetStringorDefault(prefix + "State");
            FirstName = reader.GetStringorDefault(prefix + "FirstName");
            LastName = reader.GetStringorDefault(prefix + "LastName");
            Phone = reader.GetStringorDefault(prefix + "Phone");
        }
    }
}
