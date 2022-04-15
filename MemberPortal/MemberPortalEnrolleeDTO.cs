using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberPortalEnrolleeDTO : ILoadFromDataReader
    {
        public string PlanID { get; set; }
        public string EnrolleeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? EffectiveDate { get; set; }

        /// <inheritdoc />
        public void LoadFromDataReader(IDataReader reader)
        {
            PlanID = reader.GetStringorDefault("PLNID").Trim();
            EnrolleeID = reader.GetStringorDefault("ENRID").Trim();
            FirstName = reader.GetStringorDefault("FNAME").Trim();
            LastName = reader.GetStringorDefault("LNAME").Trim();
            DateOfBirth = reader.GetDateTimeorDefault("DOB", DateTime.MinValue);
            EffectiveDate = reader.GetDateTimeorNull("EFFDT");
        }
    }
}
