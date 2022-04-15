namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.API.PBM.Messages.Shared;
    using ProCare.Common.Data;

    using ServiceStack;

    using System;
    using System.Data;

    using static ProCare.API.PBM.Messages.Shared.Enums;

    public class EpisodeClaimsHistoryDTO : ILoadFromDataReader
    {
        public string RxNumber { get; set; }
        public string PharmacyNPI { get; set; }
        public string PharmacyNCPDPID { get; set; }
        public string PharmacyName { get; set; }
        public string PhysicianNPI { get; set; }
        public string PhysicianDEA { get; set; }
        public string PhysicianNCPDPID { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianMiddleInitial { get; set; }
        public string PhysicianLastName { get; set; }
        public int ClaimStatusID { get; set; }
        public string ClaimStatus { get; set; }
        public string DrugName { get; set; }
        public string FillDate { get; set; }
        public string NDCProcDateTime { get; set; }
        public decimal Quantity { get; set; }
        public int DaySupply { get; set; }
        public decimal BillAmount { get; set; }
        public string TableName { get; set; }
        public string NDCREF { get; set; }
        public void LoadFromDataReader(IDataReader reader)
        {
            RxNumber = reader.GetStringorNull("RXNO");
            PharmacyNPI = reader.GetStringorNull("PHANPI");
            PharmacyNCPDPID = reader.GetStringorNull("PHAID");
            PharmacyName = reader.GetStringorNull("PharmacyName");
            PhysicianNPI = reader.GetStringorNull("PHYNPI");
            PhysicianDEA = reader.GetStringorNull("PHYDEA");
            PhysicianNCPDPID = reader.GetStringorNull("PHYID");
            PhysicianFirstName = reader.GetStringorNull("PhysicianFirstName");
            PhysicianMiddleInitial = reader.GetStringorNull("PhysicianMiddleInitial");
            PhysicianLastName = reader.GetStringorNull("PhysicianLastName");
            ClaimStatusID = reader.GetInt16orDefault("CLAIM_STATUS");
            ClaimStatus = ((Enums.ClaimStatus)ClaimStatusID).ToDescription();
            DrugName = reader.GetStringorNull("LN");
            FillDate = reader.GetDateTimeorDefault("FILLDT", DateTime.MinValue).ToString("MM/dd/yyyy");
            string batId = reader.GetStringorNull("BATID");
            string seconds = reader.GetStringorNull("SECONDS") ?? "00";
            NDCProcDateTime = $"{reader.GetDateTimeorDefault("NDCPROCDT", DateTime.MinValue).ToString("MM/dd/yyyy")} {batId.Substring(batId.Length - 4, 2)}:{batId.Substring(batId.Length - 2, 2)}:{seconds.Substring(0, 2)}";
            Quantity = reader.GetDecimalorDefault("Qty");
            DaySupply = reader.GetInt16orDefault("DaySup");
            BillAmount = reader.GetDecimalorDefault("Charge") + reader.GetDecimalorDefault("AdminFee");
            TableName = reader.GetStringorNull("TableName");
            NDCREF = reader.GetStringorDefault("NDCREF");
        }
    }
}
