using ProCare.API.PBM.Repository.DTO;
using System.Collections.Generic;

namespace ProCare.API.PBM.Repository
{
    public interface IPBMSchedulerTaskRepository
    {
        List<PBMSchedulerServiceTaskDTO> getPBMSchedulerServiceTaskItems();
    }
}
