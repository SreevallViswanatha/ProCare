namespace ProCare.API.PBM.Repository.DTO
{
    public class DatasetEnrolleeIDDTO
    {
        public DatasetDTO Dataset { get; set; }
        public string EnrolleeID { get; set; }

        public DatasetEnrolleeIDDTO(DatasetDTO dataset, string enrolleeId)
        {
            Dataset = dataset;
            EnrolleeID = enrolleeId;
        }
    }
}
