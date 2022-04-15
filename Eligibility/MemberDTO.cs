using ProCare.Common.Data;
using System;
using System.Data;
using ProCare.API.PBM.Repository.Helpers;

namespace ProCare.API.PBM.Repository.DTO.Eligibility
{
    public class MemberDTO : ILoadFromDataReader, IEquatable<MemberDTO>
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

        public bool Equals(MemberDTO other)
        {
            bool equal = true;

            if (!this.ENRID.ToUpper().Equals(other.ENRID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.PLNID.ToUpper().Equals(other.PLNID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.SUBID.ToUpper().Equals(other.SUBID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.CARDID.ToUpper().Equals(other.CARDID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.PERSON.ToUpper().Equals(other.PERSON.ToUpper()))
            {
                equal = false;
            }
            else if (EFFDT != other.EFFDT)
            {
                equal = false;
            }
            else if (!ComparisonHelper.NullableDateTimesAreEqual(TRMDT, other.TRMDT))
            {
                equal = false;
            }
            else if (!this.FLEX1.ToUpper().Equals(other.FLEX1.ToUpper()))
            {
                equal = false;
            }
            else if (!this.FLEX2.ToUpper().Equals(other.FLEX2.ToUpper()))
            {
                equal = false;
            }
            else if (!this.RELCD.ToUpper().Equals(other.RELCD.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ELGOVER.ToUpper().Equals(other.ELGOVER.ToUpper()))
            {
                equal = false;
            }
            else if (!this.FNAME.ToUpper().Equals(other.FNAME.ToUpper()))
            {
                equal = false;
            }
            else if (!this.MNAME.ToUpper().Equals(other.MNAME.ToUpper()))
            {
                equal = false;
            }
            else if (!this.LNAME.ToUpper().Equals(other.LNAME.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ADDR.ToUpper().Equals(other.ADDR.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ADDR2.ToUpper().Equals(other.ADDR2.ToUpper()))
            {
                equal = false;
            }
            else if (!this.CITY.ToUpper().Equals(other.CITY.ToUpper()))
            {
                equal = false;
            }
            else if (!this.STATE.ToUpper().Equals(other.STATE.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ZIP.ToUpper().Equals(other.ZIP.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ZIP4.ToUpper().Equals(other.ZIP4.ToUpper()))
            {
                equal = false;
            }
            else if (!ComparisonHelper.NullableDateTimesAreEqual(DOB, other.DOB))
            {
                equal = false;
            }
            else if (!this.SEX.ToUpper().Equals(other.SEX.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ELGCD.ToUpper().Equals(other.ELGCD.ToUpper()))
            {
                equal = false;
            }
            else if (!this.EMPCD.ToUpper().Equals(other.EMPCD.ToUpper()))
            {
                equal = false;
            }
            else if (CRDDT != other.CRDDT)
            {
                equal = false;
            }
            else if (!this.SYSID.ToUpper().Equals(other.SYSID.ToUpper()))
            {
                equal = false;
            }
            else if (LSTDTCARD != other.LSTDTCARD)
            {
                equal = false;
            }
            else if (!this.NOUPDATE.ToUpper().Equals(other.NOUPDATE.ToUpper()))
            {
                equal = false;
            }
            //else if (!this.NDCUPDATE.ToUpper().Equals(other.NDCUPDATE.ToUpper()))
            //{
            //    equal = false;
            //}
            //else if (LASTUPDT != other.LASTUPDT)
            //{
            //    equal = false;
            //}
            else if (MBRSINCE != other.MBRSINCE)
            {
                equal = false;
            }
            else if (!this.PHYID.ToUpper().Equals(other.PHYID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.OLDPERSON.ToUpper().Equals(other.OLDPERSON.ToUpper()))
            {
                equal = false;
            }
            else if (!this.FLEX3.ToUpper().Equals(other.FLEX3.ToUpper()))
            {
                equal = false;
            }
            else if (!this.OTHERID.ToUpper().Equals(other.OTHERID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.DEPCODE.ToUpper().Equals(other.DEPCODE.ToUpper()))
            {
                equal = false;
            }
            else if (!this.MAINT.ToUpper().Equals(other.MAINT.ToUpper()))
            {
                equal = false;
            }
            else if(!ComparisonHelper.NullableIntsAreEqual(ACCUM, other.ACCUM))
            {
                equal = false;
            }
            else if (!this.PATSTAT.ToUpper().Equals(other.PATSTAT.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ENRCOPAYM.ToUpper().Equals(other.ENRCOPAYM.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ENRCOPAYR.ToUpper().Equals(other.ENRCOPAYR.ToUpper()))
            {
                equal = false;
            }
            else if (!this.PHYSREQ.ToUpper().Equals(other.PHYSREQ.ToUpper()))
            {
                equal = false;
            }
            else if (!this.USEELM.ToUpper().Equals(other.USEELM.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ACCMETH.ToUpper().Equals(other.ACCMETH.ToUpper()))
            {
                equal = false;
            }
            else if (!this.CARDID2.ToUpper().Equals(other.CARDID2.ToUpper()))
            {
                equal = false;
            }
            else if (!this.COB.ToUpper().Equals(other.COB.ToUpper()))
            {
                equal = false;
            }
            else if (!this.JOURNAL.ToUpper().Equals(other.JOURNAL.ToUpper()))
            {
                equal = false;
            }
            else if (!this.ADDEDBY.ToUpper().Equals(other.ADDEDBY.ToUpper()))
            {
                equal = false;
            }
            else if (!this.PMGID.ToUpper().Equals(other.PMGID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.PHONE.ToUpper().Equals(other.PHONE.ToUpper()))
            {
                equal = false;
            }
            else if (!this.MEDICARE.ToUpper().Equals(other.MEDICARE.ToUpper()))
            {
                equal = false;
            }
            else if (!this.PPNREQENR.ToUpper().Equals(other.PPNREQENR.ToUpper()))
            {
                equal = false;
            }
            else if (!this.PPNID.ToUpper().Equals(other.PPNID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.HICN.ToUpper().Equals(other.HICN.ToUpper()))
            {
                equal = false;
            }
            else if (!this.RXBIN.ToUpper().Equals(other.RXBIN.ToUpper()))
            {
                equal = false;
            }
            else if (!this.RXPCN.ToUpper().Equals(other.RXPCN.ToUpper()))
            {
                equal = false;
            }
            else if (!this.RXGROUP.ToUpper().Equals(other.RXGROUP.ToUpper()))
            {
                equal = false;
            }
            else if (!this.RXID.ToUpper().Equals(other.RXID.ToUpper()))
            {
                equal = false;
            }
            else if (!this.TRELIG.ToUpper().Equals(other.TRELIG.ToUpper()))
            {
                equal = false;
            }
            else if (!this.PHYQUAL.ToUpper().Equals(other.PHYQUAL.ToUpper()))
            {
                equal = false;
            }
            else if (!ComparisonHelper.NullableIntsAreEqual(this.MMEDAYMAX, other.MMEDAYMAX))
            {
                equal = false;
            }
            else if (!this.ALLOWGOVT.ToUpper().Equals(other.ALLOWGOVT.ToUpper()))
            {
                equal = false;
            }

            return equal;
        }

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
            catch (Exception)
            {

            }
        }
    }
}
