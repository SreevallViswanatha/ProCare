using ProCare.API.Common.Repository.DTO;
using ProCare.Common.Data;
using System.Collections.Generic;
using System.Data;

namespace ProCare.API.Common.Repository.DataAccess
{
    public class MPIDataAccess : DataAccessBase
    {
        public MPIDataAccess(IDataAccessHelper dataHelper) : base(dataHelper)
        {
        }

        public List<MasterPatientIndexDTO> SelectByFNameLNameAddressDOBZip(MasterPatientIndexDTO dto)
        {
            string sql =
                @"select mpi.prxid, 
                    clnt.ClientName, 
                    mpids.PatientID, 
                    mpids.CardID, 
                    mpi.firstname, 
                    mpi.lastname, 
                    mpi.dateofbirth, 
                    addr.address, 
                    addr.city, 
                    addr.state, 
                    addr.zipcode, 
                    phn.PhoneNumber
                    from dbo.mpi mpi
                    join dbo.MPI_DataSet mpids on mpi.PrxID = mpids.PrxID
                    join dbo.mpi_address mpiadd on mpi.prxid = mpiadd.prxid
                    join dbo.address addr on mpiadd.addressid = addr.addressid
                    join dbo.MPI_PhoneNumber mpiphn on mpi.PrxID = mpiphn.PrxID
                    join dbo.PhoneNumber phn on mpiphn.PhoneNumberID = phn.PhoneNumberID and phn.PhoneNumberTypeID = 1
                    join dbo.DataSet ds on mpids.DataSetID = ds.DataSetID
                    join dbo.client clnt on ds.ClientID = clnt.ClientID
                    where mpi.firstname = @FirstName
                    and mpi.lastname = @LastName
                    and mpi.DateOfBirth = @DOB
                    and addr.Address = @Addr
                    and addr.ZipCode = @Zip";

            List<MasterPatientIndexDTO> results = new List<MasterPatientIndexDTO>();

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@FirstName", dto.FirstName},
                {"@LastName", dto.LastName},
                {"@DOB", dto.DateOfBirth},
                {"@Addr", dto.Address},
                {"@Zip", dto.ZipCode},
                
            };

            DataHelper.ExecuteReader(sql, CommandType.Text, parameters, reader =>
            {
                var response = new MasterPatientIndexDTO();
                response.LoadFromDataReader(reader);
                results.Add(response);                
            });

            return results;
        }

    }
}
