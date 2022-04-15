using ProCare.API.PBM.Messages;
using ProCare.API.PBM.Messages.Shared;
using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberSearchDTO : ILoadFromDataReader
    {
        public bool Active { get; set; }
        public string MemberId { get; set; }
        public string Client { get; set; }
        public string OrganizationId { get; set; }
        public string GroupId { get; set; }
        public string PlanId { get; set; }
        public DateTime? PlanEffectiveDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MemberEnrolleeId { get; set; }
        public DateTime? MemberEffectiveDate { get; set; }
        public DateTime? MemberTerminationDate { get; set; }
        public string FamilyId { get; set; }
        public string RelationshipCode { get; set; }
        public AddressDTO Address { get; set; }
        public string Phone { get; set; }
        public string MedicareId { get; set; }
        public GroupEligibilityDTO GroupEligibility { get; set; }
        public AdditionalEligibilityDTO AdditionalEligibility { get; set; }
        public string OrgName { get; set; }
        public string GroupName { get; set; }
        public string PlanName { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            if (reader.ColumnExists("MemberID"))
            {
                MemberId = reader.GetStringorDefault("MemberID");
            }
            else
            {
                string memberIdType = reader.GetStringorDefault("MemberIDType");

                Enums.MemberIDType idType = Enum.Parse<Enums.MemberIDType>(memberIdType);

                switch (idType)
                {
                    case Enums.MemberIDType.CardIDCardID2:
                    {
                        MemberId = reader.GetStringorDefault("CardIDCardID2MemberID");
                        break;
                    }
                    case Enums.MemberIDType.OtherID:
                    {
                        MemberId = reader.GetStringorDefault("OtherIDMemberID");
                        break;
                    }
                }
            }
            
            OrganizationId = reader.GetStringorDefault("ORGID");
            GroupId = reader.GetStringorDefault("GRPID");
            PlanId = reader.GetStringorDefault("PLNID");
            PlanEffectiveDate = reader.GetDateTimeorDefault("PlanEffectiveDate", DateTime.MinValue);
            FirstName = reader.GetStringorDefault("FNAME");
            MiddleInitial = reader.GetStringorDefault("MNAME");
            LastName = reader.GetStringorDefault("LNAME");
            Gender = reader.GetStringorDefault("SEX");
            DateOfBirth = reader.GetDateTimeorDefault("DOB", DateTime.MinValue);
            MemberEnrolleeId = reader.GetStringorDefault("ENRID");
            MemberEffectiveDate = reader.GetDateTimeorNull("EFFDT");
            MemberTerminationDate = reader.GetDateTimeorNull("TRMDT");
            FamilyId = reader.GetStringorDefault("FamilyID");
            RelationshipCode = reader.GetStringorDefault("RELCD");
            Phone = reader.GetStringorDefault("PHONE");
            MedicareId = reader.GetStringorDefault("HICN");
            OrgName = reader.GetStringorDefault("OrgName");
            GroupName = reader.GetStringorDefault("GroupName");
            PlanName = reader.GetStringorDefault("PlanName");

            Active = (MemberEffectiveDate.GetValueOrDefault() <= DateTime.Today) &&
                     (MemberTerminationDate == null || MemberTerminationDate.GetValueOrDefault() > DateTime.Today);

            Address = new AddressDTO();
            Address.LoadFromDataReader(reader);

            GroupEligibility = new GroupEligibilityDTO();
            GroupEligibility.LoadFromDataReader(reader);

            AdditionalEligibility = new AdditionalEligibilityDTO();
            AdditionalEligibility.LoadFromDataReader(reader);
        }
    }
}
