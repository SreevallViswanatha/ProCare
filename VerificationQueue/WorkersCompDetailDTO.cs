namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.API.Core.Helpers;
    using ProCare.Common.Data;

    using System.Data;

    public class WorkersCompDetailDTO : ILoadFromDataReader
    {
        public string PlanID { get; set; }
        public string EmployerName { get; set; }
        public AddressExtendedDTO EmployerAddress { get; set; }
        public string EmployerPhone { get; set; }
        public string AdjusterName { get; set; }
        public string AdjusterEmail { get; set; }
        public string AdjusterPhone { get; set; }
        public string AdjusterPhoneExt { get; set; }
        public string AdjusterFax { get; set; }
        public string Jurisdiction { get; set; }
        public string ClaimStatusCode { get; set; }
        public string ClaimStatusDescription { get; set; }
        public string InjuryDate { get; set; }
        public string CarrierID { get; set; }
        public string WorkerCompClaimID { get; set; }
        public string PIPClaim { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PlanID = reader.GetStringorNull("PLNID");
            EmployerName = reader.GetStringorNull("EMPNAME");
            EmployerPhone = reader.GetStringorNull("EMPPHONE");
            AdjusterName = reader.GetStringorNull("ADJSTNAME");
            AdjusterEmail = reader.GetStringorNull("ADJSTEMAIL");
            AdjusterPhone = reader.GetStringorNull("ADJSTPHONE");
            AdjusterPhoneExt = reader.GetStringorNull("ADJSTEXTN");
            AdjusterFax = reader.GetStringorNull("ADJSTFAX");
            Jurisdiction = reader.GetStringorNull("JURIS");
            ClaimStatusCode = reader.GetStringorNull("CLMSTATUS");
            ClaimStatusDescription = reader.GetStringorNull("ClaimStatusDesc");
            InjuryDate = DateTimeHelper.ConvertDateToString(reader.GetDateTimeorNull("INJURDT"));
            CarrierID = reader.GetStringorNull("CARRIER_ID");
            WorkerCompClaimID = reader.GetStringorNull("WCCLAIMID");
            PIPClaim = reader.GetStringorNull("PIPCLAIM");

            EmployerAddress = new AddressExtendedDTO();
            EmployerAddress.LoadFromDataReaderWithPrefix(reader, "EMP");
            EmployerAddress.StateFull = reader.GetStringorNull("EmpStateFull");
        }
    }
}
