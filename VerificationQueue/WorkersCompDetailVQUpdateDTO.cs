namespace ProCare.API.PBM.Repository.DTO
{
    public class WorkersCompDetailVQUpdateDTO
    {
        public string ENRID { get; set; }

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

        public string InjuryDate { get; set; }

        public string CarrierID { get; set; }

        public string WorkerCompClaimID { get; set; }

        public string PIPCLAIM { get; set; }

        public int AppUserID { get; set; }
    }
}
