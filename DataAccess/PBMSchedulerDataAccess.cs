using ProCare.API.PBM.Repository.DTO;
using ProCare.API.PBM.Repository.DTO.ScheduledTask;
using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.PBM.Repository
{
    public class PBMSchedulerDataAccess : DataAccessBase
    {
        #region Constructors

        public PBMSchedulerDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods
        public List<PBMSchedulerServiceTaskDTO> getPBMSchedulerServiceTaskItems()
        {
            List<PBMSchedulerServiceTaskDTO> taskDTOs = new List<PBMSchedulerServiceTaskDTO>();

            DataHelper.ExecuteReader("apiPBM_PBMSchedulerServiceTask_read", CommandType.StoredProcedure, null, reader =>
            {
                PBMSchedulerServiceTaskDTO taskDTO = new PBMSchedulerServiceTaskDTO();
                taskDTO.LoadFromDataReader(reader);
                taskDTOs.Add(taskDTO);
            });

            return taskDTOs;
        }
        #endregion
    }
}
