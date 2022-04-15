using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProCare.API.PBM.Messages.Shared;
using System.Collections.Generic;

namespace ProCare.API.PBM.Repository.DTO
{
    public class DatasetDTO
    {
        public string Name { get; set; }
        public string Path { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.MemberIDType MemberIdType { get; set; }
        public string PrxClientID { get; set; }
        public bool ShowClaimsForAllMemberPlans { get; set; }
        public List<string> ParentIDs { get; set; }
        public List<string> OrganizationIDs { get; set; }
        public List<string> GroupIDs { get; set; }
        public List<string> PlanIDs { get; set; }
        public bool ClientHasAccess { get; set; }

        public DatasetDTO()
        {
            ParentIDs = new List<string>();
            OrganizationIDs = new List<string>();
            GroupIDs = new List<string>();
            PlanIDs = new List<string>();
            ClientHasAccess = false;
        }
    }
}
