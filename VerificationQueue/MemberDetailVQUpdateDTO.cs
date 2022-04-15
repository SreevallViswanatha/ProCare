namespace ProCare.API.PBM.Repository.DTO
{
    using ProCare.API.PBM.Messages.Response;

    public class MemberDetailVQUpdateDTO
    {
        public string ENRID { get; set; }
        public string PlanID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public int GenderID { get; set; }
        public string MemberPhoneNumber { get; set; }
        public AddressExtended MemberAddress { get; set; }
        public string EffectiveDate { get; set; }
        public string TermDate { get; set; }
        public int AppUserID { get; set; }
    }
}
