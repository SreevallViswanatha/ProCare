using System;
using System.Data;
using ProCare.API.Core.Responses;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsIDCardInfoDTO : ILoadFromDataReader
    {
        public DateTime? CardDate { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            CardDate = reader.GetDateTimeorNull("CardDate");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            CardDate = reader.GetDateTimeorNull(prefix + "CardDate");
        }
    }
}