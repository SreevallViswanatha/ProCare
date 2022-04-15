using System.Data;
using ProCare.API.Core.Responses;
using ProCare.Common.Data;
using System;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsPlanInfoDTO : ILoadFromDataReader
    {
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string BrandCopayMethod { get; set; }
        public double BrandCopayAmount { get; set; }
        public int BrandCopayPercent { get; set; }
        public string GenericCopayMethod { get; set; }
        public double GenericCopayAmount { get; set; }
        public int GenericCopayPercent { get; set; }
        public string BrandEquivalentCopayMethod { get; set; }
        public double BrandEquivalentCopayAmount { get; set; }
        public int BrandEquivalentCopayPercent { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            EffectiveDate = reader.GetDateTimeorNull("EffectiveDate");
            TerminationDate = reader.GetDateTimeorNull("TerminationDate");
            BrandCopayMethod = reader.GetStringorDefault("BrandCopayMethod");
            BrandCopayAmount = reader.GetDoubleorDefault("BrandCopayAmount");
            BrandCopayPercent = reader.GetInt32orDefault("BrandCopayPercent");
            GenericCopayMethod = reader.GetStringorDefault("GenericCopayMethod");
            GenericCopayAmount = reader.GetDoubleorDefault("GenericCopayAmount");
            GenericCopayPercent = reader.GetInt32orDefault("GenericCopayPercent");
            BrandEquivalentCopayMethod = reader.GetStringorDefault("BrandEquivalentCopayMethod");
            BrandEquivalentCopayAmount = reader.GetDoubleorDefault("BrandEquivalentCopayAmount");
            BrandEquivalentCopayPercent = reader.GetInt32orDefault("BrandEquivalentCopayPercent");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            EffectiveDate = reader.GetDateTimeorNull(prefix + "EffectiveDate");
            TerminationDate = reader.GetDateTimeorNull(prefix + "TerminationDate");
            BrandCopayMethod = reader.GetStringorDefault(prefix + "BrandCopayMethod");
            BrandCopayAmount = reader.GetDoubleorDefault(prefix + "BrandCopayAmount");
            BrandCopayPercent = reader.GetInt32orDefault(prefix + "BrandCopayPercent");
            GenericCopayMethod = reader.GetStringorDefault(prefix + "GenericCopayMethod");
            GenericCopayAmount = reader.GetDoubleorDefault(prefix + "GenericCopayAmount");
            GenericCopayPercent = reader.GetInt32orDefault(prefix + "GenericCopayPercent");
            BrandEquivalentCopayMethod = reader.GetStringorDefault(prefix + "BrandEquivalentCopayMethod");
            BrandEquivalentCopayAmount = reader.GetDoubleorDefault(prefix + "BrandEquivalentCopayAmount");
            BrandEquivalentCopayPercent = reader.GetInt32orDefault(prefix + "BrandEquivalentCopayPercent");
        }
    }
}