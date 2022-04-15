using ProCare.API.Common.Repository.DTO;
using System.Collections.Generic;

namespace ProCare.API.Common.Repository
{
    public interface IMPIRepository
    {
        List<MasterPatientIndexDTO> SelectByFNameLNameAddressDOBZip(MasterPatientIndexDTO dto);
    }
}
