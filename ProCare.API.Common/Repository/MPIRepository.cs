using ProCare.API.Common.Repository.DataAccess;
using ProCare.API.Common.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;

namespace ProCare.API.Common.Repository
{
    public class MPIRepository : BasedbRepository, IMPIRepository
    {
        public MPIRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        public List<MasterPatientIndexDTO> SelectByFNameLNameAddressDOBZip(MasterPatientIndexDTO dto)
        {
            var sqlHelper = new MPIDataAccess(DataHelper);
            return sqlHelper.SelectByFNameLNameAddressDOBZip(dto);
        }
    }
}
