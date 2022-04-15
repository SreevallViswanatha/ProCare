using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberDetailsMemberDTO : ILoadFromDataReader
    {
        public string ParentID { get; set; }
        public string OrganizationID { get; set; }
        public string GroupID { get; set; }
        public string PlanID { get; set; }
        public string MemberID { get; set; }
        public string MemberEnrolleeID { get; set; }
        public string PersonCode { get; set; }
        public string RelationshipCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool MultiBirthCode { get; set; }
        public string Flex1 { get; set; }
        public string Flex2 { get; set; }
        public string Flex3 { get; set; }
        public string FamilyID { get; set; }
        public DateTime? OriginalFromDate { get; set; }
        public string Phone { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            ParentID = reader.GetStringorDefault("ParentID");
            OrganizationID = reader.GetStringorDefault("OrganizationID");
            GroupID = reader.GetStringorDefault("GroupID");
            PlanID = reader.GetStringorDefault("PlanID");
            MemberID = reader.GetStringorDefault("MemberID");
            MemberEnrolleeID = reader.GetStringorDefault("MemberEnrolleeID");
            PersonCode = reader.GetStringorDefault("PersonCode");
            RelationshipCode = reader.GetStringorDefault("RelationshipCode");
            LastName = reader.GetStringorDefault("LastName");
            FirstName = reader.GetStringorDefault("FirstName");
            MiddleInitial = reader.GetStringorDefault("MiddleInitial");
            Gender = reader.GetStringorDefault("Gender");
            DateOfBirth = reader.GetDateTimeorNull("DateOfBirth");
            MultiBirthCode = reader.GetBooleanSafe("MultiBirthCode");
            Flex1 = reader.GetStringorDefault("Flex1");
            Flex2 = reader.GetStringorDefault("Flex2");
            Flex3 = reader.GetStringorDefault("Flex3");
            FamilyID = reader.GetStringorDefault("FamilyID");
            OriginalFromDate = reader.GetDateTimeorNull("OriginalFromDate");
            Phone = reader.GetStringorDefault("Phone");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            ParentID = reader.GetStringorDefault(prefix + "ParentID");
            OrganizationID = reader.GetStringorDefault(prefix + "OrganizationID");
            GroupID = reader.GetStringorDefault(prefix + "GroupID");
            PlanID = reader.GetStringorDefault(prefix + "PlanID");
            MemberID = reader.GetStringorDefault(prefix + "MemberID");
            MemberEnrolleeID = reader.GetStringorDefault(prefix + "MemberEnrolleeID");
            PersonCode = reader.GetStringorDefault(prefix + "PersonCode");
            RelationshipCode = reader.GetStringorDefault(prefix + "RelationshipCode");
            LastName = reader.GetStringorDefault(prefix + "LastName");
            FirstName = reader.GetStringorDefault(prefix + "FirstName");
            MiddleInitial = reader.GetStringorDefault(prefix + "MiddleInitial");
            Gender = reader.GetStringorDefault(prefix + "Gender");
            DateOfBirth = reader.GetDateTimeorNull(prefix + "DateOfBirth");
            MultiBirthCode = reader.GetBooleanSafe(prefix + "MultiBirthCode");
            Flex1 = reader.GetStringorDefault(prefix + "Flex1");
            Flex2 = reader.GetStringorDefault(prefix + "Flex2");
            Flex3 = reader.GetStringorDefault(prefix + "Flex3");
            FamilyID = reader.GetStringorDefault(prefix + "FamilyID");
            OriginalFromDate = reader.GetDateTimeorNull(prefix + "OriginalFromDate");
            Phone = reader.GetStringorDefault(prefix + "Phone");
        }
    }
}