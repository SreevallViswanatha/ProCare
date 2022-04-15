using ProCare.Common.Data;
using System;
using System.Data;
using ProCare.API.PBM.Messages.Shared;
using System.Collections.Generic;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimSearchDTO : ILoadFromDataReader
    {
        public string Client { get; set; }
        public string ClaimNumber { get; set; }
        public string ClaimStatus { get; set; }
        public string CLAIM_SOURCE { get; set; }
        public string ClaimOriginationFlag { get; set; }
        public string RxNumber { get; set; }
        public string OrganizationID { get; set; }
        public string GroupID { get; set; }
        public string PlanID { get; set; }
        public string CARDID { get; set; }
        public string CARDID2 { get; set; }
        public string ENR_OTHERID { get; set; }
        public string SubmittedFirstName { get; set; }
        public string SubmittedLastName { get; set; }
        public string ActualFirstName { get; set; }
        public string ActualLastName { get; set; }
        public DateTime? ActualDateOfBirth { get; set; }
        public DateTime? FillDate { get; set; }
        public DateTime? RxDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string ProductID { get; set; }
        public string ProductIDQualifier { get; set; }
        public string ProductNameAbbreviation { get; set; }
        public string ProductNameExtension { get; set; }
        public string PrescriberIDQualifier { get; set; }
        public string PrescriberID { get; set; }
        public string PrescriberLastName { get; set; }
        public string PrescriberState { get; set; }
        public string NCPDPPharmacyID { get; set; }
        public string NPIPharmacyID { get; set; }
        public string PharmacyName { get; set; }
        public string DaysSupply { get; set; }
        public string QuantityDispensed { get; set; }
        public string PatientPayAmount { get; set; }
        public string PlanPayAmount { get; set; }
        public string SalesTax { get; set; }
        public string TotalPaidAmount { get; set; }
        public string ProductSelectionAmount { get; set; }
        public string DeductibleAmount { get; set; }
        public string AmountExceedingPlanMaxium { get; set; }
        public string CopayAmount { get; set; }
        public string PriorAuthorizationNumber { get; set; }
        public string REJCODE1 { get; set; }
        public string REJCODE2 { get; set; }
        public string REJCODE3 { get; set; }
        public string REJCODE4 { get; set; }
        public string REJCODE5 { get; set; }
        public string REJCODE1_DESC { get; set; }
        public string REJCODE2_DESC { get; set; }
        public string REJCODE3_DESC { get; set; }
        public string REJCODE4_DESC { get; set; }
        public string REJCODE5_DESC { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ClaimNumber = reader.GetStringorDefault("NDCREF");
            ClaimOriginationFlag = reader.GetStringorDefault("RXORIGIN");
            RxNumber = reader.GetStringorDefault("RXNO");
            OrganizationID = reader.GetStringorDefault("ORGID");
            GroupID = reader.GetStringorDefault("GRPID");
            PlanID = reader.GetStringorDefault("PLNID");
            CARDID = reader.GetStringorDefault("CARDID");
            CARDID2 = reader.GetStringorDefault("CARDID2");
            ActualFirstName = reader.GetStringorDefault("ENR_FNAME");
            ActualLastName = reader.GetStringorDefault("ENR_LNAME");
            ActualDateOfBirth = reader.GetDateTimeorNull("ENR_DOB");
            FillDate = reader.GetDateTimeorNull("FILLDT");
            SubmittedDate = reader.GetDateTimeorNull("NDCPROCDT"); 
            ProductID = reader.GetStringorDefault("NDC");
            ProductNameAbbreviation = reader.GetStringorDefault("BN");
            ProductNameExtension = reader.GetStringorDefault("LN60");
            PrescriberIDQualifier = reader.GetStringorDefault("PHYQUAL");
            PrescriberID = reader.GetStringorDefault("PHYID");
            PrescriberLastName = reader.GetStringorDefault("PHY_LNAME");
            PrescriberState = reader.GetStringorDefault("PHY_STATE");
            NCPDPPharmacyID = reader.GetStringorDefault("PHAID");
            NPIPharmacyID = reader.GetStringorDefault("PHANPI");
            PharmacyName = reader.GetStringorDefault("PHANAME");
            DaysSupply = reader.GetStringorDefault("DAYSUP");
            QuantityDispensed = reader.GetStringorDefault("QTY");
            PatientPayAmount = reader.GetStringorDefault("ENRAMT");
            PlanPayAmount = reader.GetStringorDefault("CHARGE");
            SalesTax = reader.GetStringorDefault("TAX");
            TotalPaidAmount = reader.GetStringorDefault("TOTPRC");
            ProductSelectionAmount = reader.GetStringorDefault("PRODSEL");
            DeductibleAmount = reader.GetStringorDefault("DEDUCT");
            AmountExceedingPlanMaxium = reader.GetStringorDefault("EXCEEDMAX");
            CopayAmount = reader.GetStringorDefault("COPAY");
            PriorAuthorizationNumber = reader.GetStringorDefault("PAUTHNO");

            if (reader.ColumnExists("FNAME"))
            {
                SubmittedFirstName = reader.GetStringorDefault("FNAME");
            }
            if (reader.ColumnExists("LNAME"))
            {
                SubmittedLastName = reader.GetStringorDefault("LNAME");
            }
            if (reader.ColumnExists("RXDATE"))
            {
                RxDate = reader.GetDateTimeorNull("RXDATE");
            }
            if (reader.ColumnExists("CLAIM_SOURCE"))
            {
                CLAIM_SOURCE = reader.GetStringorDefault("CLAIM_SOURCE");
            }
            if (reader.ColumnExists("CLAIM_STATUS"))
            {
                ClaimStatus = reader.GetStringorDefault("CLAIM_STATUS");
            }
            if (reader.ColumnExists("REJCODE1"))
            {
                REJCODE1 = reader.GetStringorDefault("REJCODE1");
                REJCODE1_DESC = reader.GetStringorDefault("REJCODE1_DESC");
            }
            if (reader.ColumnExists("REJCODE2"))
            {
                REJCODE2 = reader.GetStringorDefault("REJCODE2");
                REJCODE2_DESC = reader.GetStringorDefault("REJCODE2_DESC");
            }
            if (reader.ColumnExists("REJCODE3"))
            {
                REJCODE3 = reader.GetStringorDefault("REJCODE3");
                REJCODE3_DESC = reader.GetStringorDefault("REJCODE3_DESC");
            }
            if (reader.ColumnExists("REJCODE4"))
            {
                REJCODE4 = reader.GetStringorDefault("REJCODE4");
                REJCODE4_DESC = reader.GetStringorDefault("REJCODE4_DESC");
            }
            if (reader.ColumnExists("REJCODE5"))
            {
                REJCODE5 = reader.GetStringorDefault("REJCODE5");
                REJCODE5_DESC = reader.GetStringorDefault("REJCODE5_DESC");
            }
        }
    }
}
