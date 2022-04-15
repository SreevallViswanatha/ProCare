namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.Common.Data;

    public class AddressExtendedDTO : AddressDTO, ILoadFromDataReader
    {
        public string StateFull { get; set; }
    }
}
