using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ProCare.Common.Data;

namespace ProCare.API.PBM.Repository.DTO.Eligibility
{
    public class BenefitYearAccumulatorDTO : ILoadFromDataReader
    {
        public string PLNID { get; set; }

        public string ENRID { get; set; }

        public string SUBID { get; set; }

        public double ENRAMT { get; set; }

        public int YTDRX { get; set; }

        public double YTDDOLLAR { get; set; }

        public string PLANTYPE { get; set; }

        public DateTime EFFDT { get; set; }

        public DateTime TRMDT { get; set; }

        public double BROKERYTD { get; set; }

        public double SMOKINGYTD { get; set; }

        public double SMOKINGLT { get; set; }

        public double COPAY { get; set; }

        public double PRODSEL { get; set; }

        public double DEDUCT { get; set; }

        public DateTime DEDMETDT { get; set; }

        public double EXCEEDMAX { get; set; }

        public DateTime MAXMETDT { get; set; }

        public DateTime OOPMETDT { get; set; }

        public double LIFEMAX { get; set; }

        public double FERYTDMAX { get; set; }

        public double FERLTMAX { get; set; }

        public double OCYTD { get; set; }

        public double OCLIFE { get; set; }

        public double ICYTD { get; set; }

        public double ICLIFE { get; set; }

        public string JOURNAL { get; set; }

        public string SYSID { get; set; }

        public string TIER { get; set; }

        public double NPDEDACC { get; set; }

        public double NPOOPACC { get; set; }

        public double NPMAXACC { get; set; }

        public double QTR4DEDACC { get; set; }

        public double QTR4OOPACC { get; set; }

        public double QTR4MAXACC { get; set; }

        public double MEDDEDACC { get; set; }

        public double MEDOOPACC { get; set; }

        public double MEDMAXACC { get; set; }

        public DateTime DIAPHRDT { get; set; }

        public double NPMEDMAX { get; set; }

        public double NPMEDOOP { get; set; }

        public double NPMEDDED { get; set; }

        public DateTime LASTCLAIM { get; set; }

        public string OTHERID { get; set; }

        public double GHYTDMAX { get; set; }

        public double GHLTMAX { get; set; }

        public double CPYSUBS { get; set; }

        public double DEDSUBS { get; set; }

        public double TROOP { get; set; }

        public double TOTPRC { get; set; }

        public double GAP_TROOP { get; set; }

        public double ENRADJ { get; set; }

        public string CLASS { get; set; }

        public double BYATROOP { get; set; }

        public double PARTBDED { get; set; }

        public double PARTBOOP { get; set; }

        public double PARTBMAX { get; set; }

        public DateTime BDEDMETDT { get; set; }

        public DateTime BOOPMETDT { get; set; }

        public DateTime BMAXMETDT { get; set; }

        public double TROOPIC { get; set; }

        public double TOTPRCIC { get; set; }

        public double CPYSUBSIC { get; set; }

        public double GAPCPYSUB { get; set; }

        public double GAPTOTPRC { get; set; }

        public double TEQFEE { get; set; }

        public double SPECDED { get; set; }

        public double SPECOOP { get; set; }

        public double SPECDOLLAR { get; set; }

        public string CHANGEDBY { get; set; }

        public DateTime DATE { get; set; }

        public string TIME { get; set; }

        public string USERNAME { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            PLNID = reader.GetStringorDefault("PLNID");
            ENRID = reader.GetStringorDefault("ENRID");
            SUBID = reader.GetStringorDefault("SUBID");
            ENRAMT = reader.GetDoubleorDefault("ENRAMT");
            YTDRX = reader.GetInt32orDefault("YTDRX");
            YTDDOLLAR = reader.GetDoubleorDefault("YTDDOLLAR");
            PLANTYPE = reader.GetStringorDefault("PLANTYPE");
            EFFDT = reader.GetDateTimeorDefault("EFFDT", DateTime.MinValue);
            TRMDT = reader.GetDateTimeorDefault("TRMDT", DateTime.MinValue);
            BROKERYTD = reader.GetDoubleorDefault("BROKERYTD");
            SMOKINGYTD = reader.GetDoubleorDefault("SMOKINGYTD");
            SMOKINGLT = reader.GetDoubleorDefault("SMOKINGLT");
            COPAY = reader.GetDoubleorDefault("COPAY");
            PRODSEL = reader.GetDoubleorDefault("PRODSEL");
            DEDUCT = reader.GetDoubleorDefault("DEDUCT");
            DEDMETDT = reader.GetDateTimeorDefault("DEDMETDT", DateTime.MinValue);
            EXCEEDMAX = reader.GetDoubleorDefault("EXCEEDMAX");
            MAXMETDT = reader.GetDateTimeorDefault("MAXMETDT", DateTime.MinValue);
            OOPMETDT = reader.GetDateTimeorDefault("OOPMETDT", DateTime.MinValue);
            LIFEMAX = reader.GetDoubleorDefault("LIFEMAX");
            FERYTDMAX = reader.GetDoubleorDefault("FERYTDMAX");
            FERLTMAX = reader.GetDoubleorDefault("FERLTMAX");
            OCYTD = reader.GetDoubleorDefault("OCYTD");
            OCLIFE = reader.GetDoubleorDefault("OCLIFE");
            ICYTD = reader.GetDoubleorDefault("ICYTD");
            ICLIFE = reader.GetDoubleorDefault("ICLIFE");
            JOURNAL = reader.GetStringorDefault("JOURNAL");
            SYSID = reader.GetStringorDefault("SYSID");
            TIER = reader.GetStringorDefault("TIER");
            NPDEDACC = reader.GetDoubleorDefault("NPDEDACC");
            NPOOPACC = reader.GetDoubleorDefault("NPOOPACC");
            NPMAXACC = reader.GetDoubleorDefault("NPMAXACC");
            QTR4DEDACC = reader.GetDoubleorDefault("QTR4DEDACC");
            QTR4OOPACC = reader.GetDoubleorDefault("QTR4OOPACC");
            QTR4MAXACC = reader.GetDoubleorDefault("QTR4MAXACC");
            MEDDEDACC = reader.GetDoubleorDefault("MEDDEDACC");
            MEDOOPACC = reader.GetDoubleorDefault("MEDOOPACC");
            MEDMAXACC = reader.GetDoubleorDefault("MEDMAXACC");
            DIAPHRDT = reader.GetDateTimeorDefault("DIAPHRDT", DateTime.MinValue);
            NPMEDMAX = reader.GetDoubleorDefault("NPMEDMAX");
            NPMEDOOP = reader.GetDoubleorDefault("NPMEDOOP");
            NPMEDDED = reader.GetDoubleorDefault("NPMEDDED");
            LASTCLAIM = reader.GetDateTimeorDefault("LASTCLAIM", DateTime.MinValue);
            OTHERID = reader.GetStringorDefault("OTHERID");
            GHYTDMAX = reader.GetDoubleorDefault("GHYTDMAX");
            GHLTMAX = reader.GetDoubleorDefault("GHLTMAX");
            CPYSUBS = reader.GetDoubleorDefault("CPYSUBS");
            DEDSUBS = reader.GetDoubleorDefault("DEDSUBS");
            TROOP = reader.GetDoubleorDefault("TROOP");
            TOTPRC = reader.GetDoubleorDefault("TOTPRC");
            GAP_TROOP = reader.GetDoubleorDefault("GAP_TROOP");
            ENRADJ = reader.GetDoubleorDefault("ENRADJ");
            CLASS = reader.GetStringorDefault("CLASS");
            BYATROOP = reader.GetDoubleorDefault("BYATROOP");
            PARTBDED = reader.GetDoubleorDefault("PARTBDED");
            PARTBOOP = reader.GetDoubleorDefault("PARTBOOP");
            PARTBMAX = reader.GetDoubleorDefault("PARTBMAX");
            BDEDMETDT = reader.GetDateTimeorDefault("BDEDMETDT", DateTime.MinValue);
            BOOPMETDT = reader.GetDateTimeorDefault("BOOPMETDT", DateTime.MinValue);
            BMAXMETDT = reader.GetDateTimeorDefault("BMAXMETDT", DateTime.MinValue);
            TROOPIC = reader.GetDoubleorDefault("TROOPIC");
            TOTPRCIC = reader.GetDoubleorDefault("TOTPRCIC");
            CPYSUBSIC = reader.GetDoubleorDefault("CPYSUBSIC");
            GAPCPYSUB = reader.GetDoubleorDefault("GAPCPYSUB");
            GAPTOTPRC = reader.GetDoubleorDefault("GAPTOTPRC");
            TEQFEE = reader.GetDoubleorDefault("TEQFEE");
            SPECDED = reader.GetDoubleorDefault("SPECDED");
            SPECOOP = reader.GetDoubleorDefault("SPECOOP");
            SPECDOLLAR = reader.GetDoubleorDefault("SPECDOLLAR");
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
