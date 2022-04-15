using System.Data;
using ProCare.API.Core.Responses;
using ProCare.Common.Data;
using System;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsCopayOverrideDTO : ILoadFromDataReader
    {
        public string CopayID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            CopayID = reader.GetStringorDefault("CopayID");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            CopayID = reader.GetStringorDefault(prefix + "CopayID");
        }
    }
}