using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DTO.MemberPortal
{
    public class MemberFavoriteMedicationDTO
    {
        public int MemberMedicationID { get; set; }
        public int UserID { get; set; }
        public int MemberMedicationTypeID { get; set; }
        public long EntityIdentifier { get; set; }
        public string MedicationName { get; set; }
        public void LoadFromDataReader(IDataReader reader)
        {
            try
            {
                UserID = reader.GetInt32orDefault("UserID");
            }
            catch (Exception)
            {
            }
            MemberMedicationID = reader.GetInt32orDefault("MemberMedicationID");
            MemberMedicationTypeID = reader.GetInt32orDefault("MemberMedicationTypeID");
            EntityIdentifier = reader.GetInt32orDefault("EntityIdentifier");
            MedicationName = reader.GetStringorDefault("MedicationName");
        }
    }
}
