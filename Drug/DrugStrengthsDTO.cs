using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{

    public class DrugStrengthsDTO : ILoadFromDataReader
    {
        public string DrugID { get; set; }
        public string DrugName { get; set; }
        public string DrugType { get; set; }
        public string NDC { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            DrugID = reader.GetStringorDefault("DrugID");
            DrugName = reader.GetStringorDefault("DrugName");
            DrugType = reader.GetStringorDefault("DrugType");
            NDC = reader.GetStringorDefault("NDC");
        }
    }
}
