using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.Eligibility
{
    public class ACPPlanFieldsDTO : ILoadFromDataReader
    {
        public int AccumulatorPeriod { get; set; }

        public string Oxford { get; set; }

        public DateTime AnnDate { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            AccumulatorPeriod = reader.GetInt32orDefault("ACCPERIOD");
            Oxford = reader.GetStringorDefault("OXFORD");
            AnnDate = reader.GetDateTimeorDefault("ANNDT", DateTime.MinValue);
        }
    }
}
