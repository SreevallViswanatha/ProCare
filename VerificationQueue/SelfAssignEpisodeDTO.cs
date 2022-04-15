namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.Common.Data;
    using System.Data;

    public class SelfAssignEpisodeDTO : ILoadFromDataReader
    {
        public int? AssignedToAppUserID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            AssignedToAppUserID = reader.GetInt32orNull("AssignedToAppUserID");
        }
    }
}
