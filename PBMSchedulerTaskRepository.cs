using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository
{
    public class PBMSchedulerTaskRepository : BasedbRepository, IPBMSchedulerTaskRepository
    {
        #region Constructor

        public PBMSchedulerTaskRepository(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }
        #endregion Constructor

        public List<PBMSchedulerServiceTaskDTO> getPBMSchedulerServiceTaskItems()
        {
            var sqlHelper = new PBMSchedulerDataAccess(DataHelper);
            return sqlHelper.getPBMSchedulerServiceTaskItems();
        }
    }
}
