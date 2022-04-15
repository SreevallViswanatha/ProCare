using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimRejectInformationDTO : ILoadFromDataReader
    {
        public ClaimNcpdpRejectCodeDTO Reject1 { get; set; }
        public ClaimNcpdpRejectCodeDTO Reject2 { get; set; }
        public ClaimNcpdpRejectCodeDTO Reject3 { get; set; }
        public ClaimNcpdpRejectCodeDTO Reject4 { get; set; }
        public ClaimNcpdpRejectCodeDTO Reject5 { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            Reject1 = new ClaimNcpdpRejectCodeDTO();
            Reject1.LoadFromDataReaderWithPrefix(reader, "Reject1");

            Reject2 = new ClaimNcpdpRejectCodeDTO();
            Reject2.LoadFromDataReaderWithPrefix(reader, "Reject2");

            Reject3 = new ClaimNcpdpRejectCodeDTO();
            Reject3.LoadFromDataReaderWithPrefix(reader, "Reject3");

            Reject4 = new ClaimNcpdpRejectCodeDTO();
            Reject4.LoadFromDataReaderWithPrefix(reader, "Reject4");

            Reject5 = new ClaimNcpdpRejectCodeDTO();
            Reject5.LoadFromDataReaderWithPrefix(reader, "Reject5");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            Reject1 = new ClaimNcpdpRejectCodeDTO();
            Reject1.LoadFromDataReaderWithPrefix(reader, prefix + "Reject1");

            Reject2 = new ClaimNcpdpRejectCodeDTO();
            Reject2.LoadFromDataReaderWithPrefix(reader, prefix + "Reject2");

            Reject3 = new ClaimNcpdpRejectCodeDTO();
            Reject3.LoadFromDataReaderWithPrefix(reader, prefix + "Reject3");

            Reject4 = new ClaimNcpdpRejectCodeDTO();
            Reject4.LoadFromDataReaderWithPrefix(reader, prefix + "Reject4");

            Reject5 = new ClaimNcpdpRejectCodeDTO();
            Reject5.LoadFromDataReaderWithPrefix(reader, prefix + "Reject5");
        }
    }
}
