using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberRuleDTO : ILoadFromDataReader
    {
        public string CLIENTNAME { get; set; }
        public string PLNID { get; set; }
        public string ENRID { get; set; }
        public string CODES { get; set; }
        public string EPA_ID { get; set; }
        public string DESC { get; set; }
        public string TYPE { get; set; }
        public DateTime? EFFDT { get; set; }
        public DateTime? TRMDT { get; set; }
        public string CODETYPE { get; set; }
        public string VENDTYPE { get; set; }
        public string PADENIED { get; set; }
        public string SEX { get; set; }
        public string MAGEMETH { get; set; }
        public string FAGEMETH { get; set; }
        public int? MAGELO { get; set; }
        public int? MAGEHI { get; set; }
        public int? FAGELO { get; set; }
        public int? FAGEHI { get; set; }
        public string APPLYACC { get; set; }
        public string BAPPACC { get; set; }
        public string DSGID { get; set; }
        public string DSGID2 { get; set; }
        public string CALCREFILL { get; set; }
        public int? REFILLDAYS { get; set; }
        public string REFILLMETH { get; set; }
        public int? REFILLPCT { get; set; }
        public int? MAXREFILLS { get; set; }
        public int? MAXREFMNT { get; set; }
        public string PENALTY { get; set; }
        public string DESI { get; set; }
        public string PHYLIMIT { get; set; }
        public string GI_GPI { get; set; }
        public string PPGID { get; set; }
        public string PHARMACYID { get; set; }
        public string INCLUDEEXCLUDE { get; set; }
        public string INCCOMP { get; set; }
        public string BRANDDISC { get; set; }
        public string GENONLY { get; set; }
        public string DRUGCLASS { get; set; }
        public string DRUGTYPE { get; set; }
        public string DRUGSTAT { get; set; }
        public string MAINTIND { get; set; }
        public int? COMPMAX { get; set; }
        public int? HIDOLLAR { get; set; }
        public double? QTYPERDYS { get; set; }
        public int? QTYDYLMT { get; set; }
        public string MULTISOURCECODE { get; set; }
        public string COPLVLASSN { get; set; }
        public string OVRRJTADI { get; set; }
        public string OVRRJTAGE { get; set; }
        public string OVRRJTADD { get; set; }
        public string OVRRJTDDC { get; set; }
        public string OVRRJTDOT { get; set; }
        public string OVRRJTDUP { get; set; }
        public string OVRRJTIAT { get; set; }
        public string OVRRJTMMA { get; set; }
        public string OVRRJTLAC { get; set; }
        public string OVRRJTPRG { get; set; }
        public string ACTIVE { get; set; }
        public string NOTE { get; set; }
        public string VENDORPANUMBER { get; set; }
        public string PAIDMSG { get; set; }
        public string REASON { get; set; }
        public string USERNAME { get; set; }
        public string CHANGEDBY { get; set; }

        public string JOURNAL { get; set; }
        public string SYSID { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            try
            {
                ENRID = reader.GetStringorDefault("ENRID");
            }
            catch (Exception) { }

            CODES = reader.GetStringorDefault("CODES");
            //LN60 = reader.GetStringorDefault("LN60");
            EPA_ID = reader.GetStringorDefault("EPA_ID");
            DESC = reader.GetStringorDefault("DESC");
            TYPE = reader.GetStringorDefault("TYPE");
            EFFDT = reader.GetDateTimeorNull("EFFDT");
            TRMDT = reader.GetDateTimeorNull("TRMDT");
            CODETYPE = reader.GetStringorDefault("CODETYPE");
            VENDTYPE = reader.GetStringorDefault("VENDTYPE");
            PADENIED = reader.GetStringorDefault("PADENIED");
            SEX = reader.GetStringorDefault("SEX");
            MAGEMETH = reader.GetStringorDefault("MAGEMETH");
            FAGEMETH = reader.GetStringorDefault("FAGEMETH");
            MAGELO = reader.GetInt32orNull("MAGELO");
            MAGEHI = reader.GetInt32orNull("MAGEHI");
            FAGELO = reader.GetInt32orNull("FAGELO");
            FAGEHI = reader.GetInt32orNull("FAGEHI");
            APPLYACC = reader.GetStringorDefault("APPLYACC");
            BAPPACC = reader.GetStringorDefault("BAPPACC");
            DSGID = reader.GetStringorDefault("DSGID");
            DSGID2 = reader.GetStringorDefault("DSGID2");
            CALCREFILL = reader.GetStringorDefault("CALCREFILL");
            REFILLDAYS = reader.GetInt32orNull("REFILLDAYS");
            REFILLMETH = reader.GetStringorDefault("REFILLMETH");
            REFILLPCT = reader.GetInt32orNull("REFILLPCT");
            MAXREFILLS = reader.GetInt32orNull("MAXREFILLS");
            MAXREFMNT = reader.GetInt32orNull("MAXREFMNT");
            PENALTY = reader.GetStringorDefault("PENALTY");
            DESI = reader.GetStringorDefault("DESI");
            PHYLIMIT = reader.GetStringorDefault("PHYLIMIT");
            GI_GPI = reader.GetStringorDefault("GI_GPI");
            PPGID = reader.GetStringorDefault("PPGID");
            //PPNID = reader.GetStringorDefault("PPNID");
            //PPNREQRUL = reader.GetStringorDefault("PPNREQRUL");
            INCCOMP = reader.GetStringorDefault("INCCOMP");
            BRANDDISC = reader.GetStringorDefault("BRANDDISC");
            GENONLY = reader.GetStringorDefault("GENONLY");
            DRUGCLASS = reader.GetStringorDefault("DRUGCLASS");
            DRUGTYPE = reader.GetStringorDefault("DRUGTYPE");
            DRUGSTAT = reader.GetStringorDefault("DRUGSTAT");
            MAINTIND = reader.GetStringorDefault("MAINTIND");
            COMPMAX = reader.GetInt32orNull("COMPMAX");
            HIDOLLAR = reader.GetInt32orNull("HIDOLLAR");
            QTYPERDYS = reader.GetDoubleorNull("QTYPERDYS");
            QTYDYLMT = reader.GetInt32orNull("QTYDYLMT");
            //COPAYGCI = reader.GetStringorDefault("COPAYGCI");
            COPLVLASSN = reader.GetStringorDefault("COPLVLASSN");
            OVRRJTADI = reader.GetStringorDefault("OVRRJTADI");
            OVRRJTAGE = reader.GetStringorDefault("OVRRJTAGE");
            OVRRJTADD = reader.GetStringorDefault("OVRRJTADD");
            OVRRJTDDC = reader.GetStringorDefault("OVRRJTDDC");
            OVRRJTDOT = reader.GetStringorDefault("OVRRJTDOT");
            OVRRJTDUP = reader.GetStringorDefault("OVRRJTDUP");
            OVRRJTIAT = reader.GetStringorDefault("OVRRJTIAT");
            OVRRJTMMA = reader.GetStringorDefault("OVRRJTMMA");
            OVRRJTLAC = reader.GetStringorDefault("OVRRJTLAC");
            OVRRJTPRG = reader.GetStringorDefault("OVRRJTPRG");
            SYSID = reader.GetStringorDefault("SYSID");
            JOURNAL = reader.GetStringorDefault("JOURNAL");
            ACTIVE = reader.GetStringorDefault("ACTIVE");
            REASON = reader.GetStringorDefault("REASON");
        }
    }
}
