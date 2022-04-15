namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.Common.Data;

    using System.Data;

    public class ErrorCodeDTO : ILoadFromDataReader
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ErrorCode = reader.GetStringorNull("ErrorCode");
            ErrorMessage = reader.GetStringorNull("ErrorMessage");
        }
    }
}
