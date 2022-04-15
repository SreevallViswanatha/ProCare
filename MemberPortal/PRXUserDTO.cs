using ProCare.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.DTO.MemberPortal
{
    public class PRXUserDTO : ILoadFromDataReader
    {

        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public int ClientID { get; set; }
        public string ENRID { get; set; }      
        public string FNAME { get; set; }
        public string LNAME { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            UserID = reader.GetInt32orDefault("UserID");
            ClientID = reader.GetInt32orDefault("ClientID");
            EmailAddress = reader.GetStringorDefault("EmailAddress");
            LoginName = reader.GetStringorDefault("LoginName");
            Name = reader.GetStringorDefault("Name");
            ENRID = reader.GetStringorDefault("ENRID");           
            FNAME = reader.GetStringorDefault("FNAME");
            LNAME = reader.GetStringorDefault("LNAME");
        }
    }
}
