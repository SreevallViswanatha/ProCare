using ProCare.API.PBM.Messages;
using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class ClaimActualMemberDTO : ILoadFromDataReader
    {
        public string EnrolleeID { get; set; }
        public string MemberIDOtherID { get; set; }
        public string MemberIDCardIDCardID2 { get; set; }
        public string ParentID { get; set; }
        public string OrganizationID { get; set; }
        public string GroupID { get; set; }
        public string PlanID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PersonCode { get; set; }
        public string RelationshipCode { get; set; }
        public AddressDTO Address { get; set; }
        public bool MultiBirthCode { get; set; }
        public string FamilyID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            EnrolleeID = reader.GetStringorDefault("EnrolleeID");
            MemberIDOtherID = reader.GetStringorDefault("IDOtherID");
            MemberIDCardIDCardID2 = reader.GetStringorDefault("IDCardIDCardID2");
            ParentID = reader.GetStringorDefault("ParentID");
            OrganizationID = reader.GetStringorDefault("OrganizationID");
            GroupID = reader.GetStringorDefault("GroupID");
            PlanID = reader.GetStringorDefault("PlanID");
            FirstName = reader.GetStringorDefault("FirstName");
            LastName = reader.GetStringorDefault("LastName");
            MiddleInitial = reader.GetStringorDefault("MiddleInitial");
            DateOfBirth = reader.GetDateTimeorNull("DateOfBirth");
            Gender = reader.GetStringorDefault("Gender");
            PersonCode = reader.GetStringorDefault("PersonCode");
            RelationshipCode = reader.GetStringorDefault("RelationshipCode");
            MultiBirthCode = reader.GetBooleanSafe("MultiBirthCode");
            FamilyID = reader.GetStringorDefault("FamilyID");

            Address = new AddressDTO();
            Address.LoadFromDataReader(reader);
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            EnrolleeID = reader.GetStringorDefault(prefix + "EnrolleeID");
            MemberIDOtherID = reader.GetStringorDefault(prefix + "IDOtherID");
            MemberIDCardIDCardID2 = reader.GetStringorDefault(prefix + "IDCardIDCardID2");
            ParentID = reader.GetStringorDefault(prefix + "ParentID");
            OrganizationID = reader.GetStringorDefault(prefix + "OrganizationID");
            GroupID = reader.GetStringorDefault(prefix + "GroupID");
            PlanID = reader.GetStringorDefault(prefix + "PlanID");
            FirstName = reader.GetStringorDefault(prefix + "FirstName");
            LastName = reader.GetStringorDefault(prefix + "LastName");
            MiddleInitial = reader.GetStringorDefault(prefix + "MiddleInitial");
            DateOfBirth = reader.GetDateTimeorNull(prefix + "DateOfBirth");
            Gender = reader.GetStringorDefault(prefix + "Gender");
            PersonCode = reader.GetStringorDefault(prefix + "PersonCode");
            RelationshipCode = reader.GetStringorDefault(prefix + "RelationshipCode");
            MultiBirthCode = reader.GetBooleanSafe(prefix + "MultiBirthCode");
            FamilyID = reader.GetStringorDefault(prefix + "FamilyID");

            Address = new AddressDTO();
            Address.LoadFromDataReaderWithPrefix(reader, prefix);
        }
    }
}
