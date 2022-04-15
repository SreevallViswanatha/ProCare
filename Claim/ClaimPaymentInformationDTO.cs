using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimPaymentInformationDTO : ILoadFromDataReader
    {
        public string TransactionNumber { get; set; }
        public DateTime? DatePosted { get; set; }
        public string CheckNumber { get; set; }
        public double CheckAmount { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            TransactionNumber = reader.GetStringorDefault("TransactionNumber");
            DatePosted = reader.GetDateTimeorNull("DatePosted");
            CheckNumber = reader.GetStringorDefault("CheckNumber");
            CheckAmount = reader.GetDoubleorDefault("CheckAmount");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            TransactionNumber = reader.GetStringorDefault(prefix + "TransactionNumber");
            DatePosted = reader.GetDateTimeorNull(prefix + "DatePosted");
            CheckNumber = reader.GetStringorDefault(prefix + "CheckNumber");
            CheckAmount = reader.GetDoubleorDefault(prefix + "CheckAmount");
        }
    }
}
