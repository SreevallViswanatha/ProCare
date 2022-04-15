namespace ProCare.API.PBM.Repository.DTO
{
    using System.Data;
    using ProCare.Common.Data;

    public class CodedEntityDTO
    {
        public int? ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int? SortOrderID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ID = reader.GetInt32orNull("ID");
            Code = reader.GetStringOrNullAsEmptyString("Code");
            Description = reader.GetStringOrNullAsEmptyString("Description");
            IsActive = reader.GetBooleanSafe("IsActive");
            SortOrderID = reader.GetInt32orNull("SortOrderID");
        }
    }
}
