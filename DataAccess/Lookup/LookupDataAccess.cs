using ProCare.API.PBM.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;
namespace ProCare.API.PBM.Repository.DataAccess
{
    public class LookupDataAccess : DataAccessBase
    {
        #region Constructors

        public LookupDataAccess(IDataAccessHelper dataHelper) : base(dataHelper) { }

        #endregion

        #region Public Methods
        public ClientSiteConfigurationDTO GetClientSiteConfiguration(int ClientSiteConfigurationID)
        {
            ClientSiteConfigurationDTO taskDTO = new ClientSiteConfigurationDTO();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SiteID", ClientSiteConfigurationID);
            DataHelper.ExecuteReader("apiPBM_Site_read", CommandType.StoredProcedure, parameters, reader =>
            {
                taskDTO.LoadFromDataReader(reader);
            });

            return taskDTO;
        }

        public List<CodedEntityDTO> ReadCodedEntities(int codedEntityTypeID)
        {
            List<CodedEntityDTO> taskDTOs = new List<CodedEntityDTO>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CodedEntityTypeID", codedEntityTypeID);

            DataHelper.ExecuteReader("apiPBM_CodedEntityList_read", CommandType.StoredProcedure, parameters, reader =>
            {
                CodedEntityDTO codedEntityDTO = new CodedEntityDTO();
                codedEntityDTO.LoadFromDataReader(reader);
                taskDTOs.Add(codedEntityDTO);
            });

            return taskDTOs;
        }
        #endregion
    }
}
