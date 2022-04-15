using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public interface IInnovationRepository
    {
        Task<long> AddAdmitCCRequest(string apiCallName, string requestJson);

        Task<long> AddAdmitCCResponse(long requestId, string responseJson);
    }
}
