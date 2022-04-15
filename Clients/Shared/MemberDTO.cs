using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO.Clients.Shared
{
    public class MemberDTO : ILoadFromDataReader
    {
        public string ENRID { get; set; }

        public string PLNID { get; set; }

        public string SUBID { get; set; }

        public string CARDID { get; set; }

        public string PERSON { get; set; }

        public DateTime EFFDT { get; set; }

        public DateTime? TRMDT { get; set; }

        public string FLEX1 { get; set; }

        public string FLEX2 { get; set; }

        public string RELCD { get; set; }

        public string ELGOVER { get; set; }

        public string FNAME { get; set; }

        public string MNAME { get; set; }

        public string LNAME { get; set; }

        public string ADDR { get; set; }

        public string ADDR2 { get; set; }

        public string CITY { get; set; }

        public string STATE { get; set; }

        public string ZIP { get; set; }

        public string ZIP4 { get; set; }

        public DateTime? DOB { get; set; }

        public string SEX { get; set; }

        public string ELGCD { get; set; }

        public string EMPCD { get; set; }

        public DateTime CRDDT { get; set; }

        public string SYSID { get; set; }

        public DateTime LSTDTCARD { get; set; }

        public string NOUPDATE { get; set; }

        public string NDCUPDATE { get; set; }

        public DateTime LASTUPDT { get; set; }

        public DateTime MBRSINCE { get; set; }

        public string PHYID { get; set; }

        public string OLDPERSON { get; set; }

        public string FLEX3 { get; set; }

        public string OTHERID { get; set; }

        public string DEPCODE { get; set; }

        public string MAINT { get; set; }

        public int? ACCUM { get; set; }

        public string PATSTAT { get; set; }

        public string ENRCOPAYM { get; set; }

        public string ENRCOPAYR { get; set; }

        public string PHYSREQ { get; set; }

        public string USEELM { get; set; }

        public string ACCMETH { get; set; }

        public string CARDID2 { get; set; }

        public string COB { get; set; }

        public string JOURNAL { get; set; }

        public string ADDEDBY { get; set; }

        public string PMGID { get; set; }

        public string PHONE { get; set; }

        public string MEDICARE { get; set; }

        public string PPNREQENR { get; set; }

        public string PPNID { get; set; }

        public string HICN { get; set; }

        public string RXBIN { get; set; }

        public string RXPCN { get; set; }

        public string RXGROUP { get; set; }

        public string RXID { get; set; }

        public string TRELIG { get; set; }

        public string PHYQUAL { get; set; }

        public int? MMEDAYMAX { get; set; }

        public string ALLOWGOVT { get; set; }

        public string CHANGEDBY { get; set; }

        public DateTime DATE { get; set; }

        public string TIME { get; set; }

        public string USERNAME { get; set; }


        public void LoadFromDataReader(IDataReader reader)
        {
            ENRID = reader.GetStringorDefault("ENRID");
            PLNID = reader.GetStringorDefault("PLNID");
            SUBID = reader.GetStringorDefault("SUBID");
            CARDID = reader.GetStringorDefault("CARDID");
            PERSON = reader.GetStringorDefault("PERSON");
            EFFDT = reader.GetDateTimeorDefault("EFFDT", DateTime.MinValue);
            TRMDT = reader.GetDateTimeorNull("TRMDT");
            FLEX1 = reader.GetStringorDefault("FLEX1");
            FLEX2 = reader.GetStringorDefault("FLEX2");
            RELCD = reader.GetStringorDefault("RELCD");
            ELGOVER = reader.GetStringorDefault("ELGOVER");
            FNAME = reader.GetStringorDefault("FNAME");
            MNAME = reader.GetStringorDefault("MNAME");
            LNAME = reader.GetStringorDefault("LNAME");
            ADDR = reader.GetStringorDefault("ADDR");
            ADDR2 = reader.GetStringorDefault("ADDR2");
            CITY = reader.GetStringorDefault("CITY");
            STATE = reader.GetStringorDefault("STATE");
            ZIP = reader.GetStringorDefault("ZIP");
            ZIP4 = reader.GetStringorDefault("ZIP4");
            DOB = reader.GetDateTimeorDefault("DOB", DateTime.MinValue);
            SEX = reader.GetStringorDefault("SEX");
            ELGCD = reader.GetStringorDefault("ELGCD");
            EMPCD = reader.GetStringorDefault("EMPCD");
            CRDDT = reader.GetDateTimeorDefault("CRDDT", DateTime.MinValue);
            SYSID = reader.GetStringorDefault("SYSID");
            LSTDTCARD = reader.GetDateTimeorDefault("LSTDTCARD", DateTime.MinValue);
            NOUPDATE = reader.GetStringorDefault("NOUPDATE");
            NDCUPDATE = reader.GetStringorDefault("NDCUPDATE");
            LASTUPDT = reader.GetDateTimeorDefault("LASTUPDT", DateTime.MinValue);
            MBRSINCE = reader.GetDateTimeorDefault("MBRSINCE", DateTime.MinValue);
            PHYID = reader.GetStringorDefault("PHYID");
            OLDPERSON = reader.GetStringorDefault("OLDPERSON");
            FLEX3 = reader.GetStringorDefault("FLEX3");
            OTHERID = reader.GetStringorDefault("OTHERID");
            DEPCODE = reader.GetStringorDefault("DEPCODE");
            MAINT = reader.GetStringorDefault("MAINT");
            ACCUM = reader.GetInt32orDefault("ACCUM");
            PATSTAT = reader.GetStringorDefault("PATSTAT");
            ENRCOPAYM = reader.GetStringorDefault("ENRCOPAYM");
            ENRCOPAYR = reader.GetStringorDefault("ENRCOPAYR");
            PHYSREQ = reader.GetStringorDefault("PHYSREQ");
            USEELM = reader.GetStringorDefault("USEELM");
            ACCMETH = reader.GetStringorDefault("ACCMETH");
            CARDID2 = reader.GetStringorDefault("CARDID2");
            COB = reader.GetStringorDefault("COB");
            JOURNAL = reader.GetStringorDefault("JOURNAL");
            ADDEDBY = reader.GetStringorDefault("ADDEDBY");
            PMGID = reader.GetStringorDefault("PMGID");
            PHONE = reader.GetStringorDefault("PHONE");
            MEDICARE = reader.GetStringorDefault("MEDICARE");
            PPNREQENR = reader.GetStringorDefault("PPNREQENR");
            PPNID = reader.GetStringorDefault("PPNID");
            HICN = reader.GetStringorDefault("HICN");
            RXBIN = reader.GetStringorDefault("RXBIN");
            RXPCN = reader.GetStringorDefault("RXPCN");
            RXGROUP = reader.GetStringorDefault("RXGROUP");
            RXID = reader.GetStringorDefault("RXID");
            TRELIG = reader.GetStringorDefault("TRELIG");
            PHYQUAL = reader.GetStringorDefault("PHYQUAL");
            MMEDAYMAX = reader.GetInt32orNull("MMEDAYMAX");
            ALLOWGOVT = reader.GetStringorDefault("ALLOWGOVT");
            try
            {
                CHANGEDBY = reader.GetStringorDefault("CHANGEDBY");
                DATE = reader.GetDateTimeorDefault("DATE", DateTime.MinValue);
                TIME = reader.GetStringorDefault("TIME");
                USERNAME = reader.GetStringorDefault("USERNAME");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
