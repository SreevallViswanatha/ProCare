using System.Threading.Tasks;
using ProCare.API.PBM.Repository.DTO;

namespace ProCare.API.PBM.Repository
{
    public interface ISecurityRepository
    {
        Task<HospiceLookupDTO> LookupHospiceByHospiceSecurityCode(string hospiceSecurityCode);
    }
}
