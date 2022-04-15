using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class RetroLICSAccumulatorDTO : ILoadFromDataReader
    {
        public int YTDRX { get; set; }
        public double ENRAMT { get; set; }
        public double YTDDOLLAR { get; set; }
        public double BROKERYTD { get; set; }
        public double SMOKINGYTD { get; set; }
        public double SMOKINGLT { get; set; }
        public double COPAY { get; set; }
        public double PRODSEL { get; set; }
        public double TEQFEE { get; set; }
        public double DEDUCT { get; set; }
        public double EXCEEDMAX { get; set; }
        public double LIFEMAX { get; set; }
        public double FERYTDMAX { get; set; }
        public double FERLTMAX { get; set; }
        public double OCYTD { get; set; }
        public double OCLIFE { get; set; }
        public double ICYTD { get; set; }
        public double ICLIFE { get; set; }
        public double SPECDED { get; set; }
        public double SPECOOP { get; set; }
        public double SPECDOLLAR { get; set; }
        public double QTR4DEDACC { get; set; }
        public double QTR4OOPACC { get; set; }
        public double QTR4MAXACC { get; set; }
        public double MEDDEDACC { get; set; }
        public double MEDOOPACC { get; set; }
        public double MEDMAXACC { get; set; }
        public double NPMEDMAX { get; set; }
        public double NPMEDOOP { get; set; }
        public double NPMEDDED { get; set; }
        public double GHYTDMAX { get; set; }
        public double GHLTMAX { get; set; }
        public double CPYSUBS { get; set; }
        public double DEDSUBS { get; set; }
        public double TROOP { get; set; }
        public double TOTPRC { get; set; }
        public double GAP_TROOP { get; set; }
        public double ENRADJ { get; set; }
        public double BYATROOP { get; set; }
        public double PARTBDED { get; set; }
        public double PARTBOOP { get; set; }
        public double PARTBMAX { get; set; }
        public double TROOPIC { get; set; }
        public double TOTPRCIC { get; set; }
        public double CPYSUBSIC { get; set; }
        public double GAPCPYSUB { get; set; }
        public double GAPTOTPRC { get; set; }
        public double NPDEDACC { get; set; }
        public double NPMAXACC { get; set; }
        public double NPOOPACC { get; set; }
        public double TOTPRCCAT { get; set; }
        public double TOTPRCDED { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            YTDRX = reader.GetInt32orDefault("YTDRX");
            ENRAMT = reader.GetDoubleorDefault("ENRAMT");
            YTDDOLLAR = reader.GetDoubleorDefault("YTDDOLLAR");
            BROKERYTD = reader.GetDoubleorDefault("BROKERYTD");
            SMOKINGYTD = reader.GetDoubleorDefault("SMOKINGYTD");
            SMOKINGLT = reader.GetDoubleorDefault("SMOKINGLT");
            COPAY = reader.GetDoubleorDefault("COPAY");
            PRODSEL = reader.GetDoubleorDefault("PRODSEL");
            TEQFEE = reader.GetDoubleorDefault("TEQFEE");
            DEDUCT = reader.GetDoubleorDefault("DEDUCT");
            EXCEEDMAX = reader.GetDoubleorDefault("EXCEEDMAX");
            LIFEMAX = reader.GetDoubleorDefault("LIFEMAX");
            FERYTDMAX = reader.GetDoubleorDefault("FERYTDMAX");
            FERLTMAX = reader.GetDoubleorDefault("FERLTMAX");
            OCYTD = reader.GetDoubleorDefault("OCYTD");
            OCLIFE = reader.GetDoubleorDefault("OCLIFE");
            ICYTD = reader.GetDoubleorDefault("ICYTD");
            ICLIFE = reader.GetDoubleorDefault("ICLIFE");
            SPECDED = reader.GetDoubleorDefault("SPECDED");
            SPECOOP = reader.GetDoubleorDefault("SPECOOP");
            SPECDOLLAR = reader.GetDoubleorDefault("SPECDOLLAR");
            QTR4DEDACC = reader.GetDoubleorDefault("QTR4DEDACC");
            QTR4OOPACC = reader.GetDoubleorDefault("QTR4OOPACC");
            QTR4MAXACC = reader.GetDoubleorDefault("QTR4MAXACC");
            MEDDEDACC = reader.GetDoubleorDefault("MEDDEDACC");
            MEDOOPACC = reader.GetDoubleorDefault("MEDOOPACC");
            MEDMAXACC = reader.GetDoubleorDefault("MEDMAXACC");
            NPMEDMAX = reader.GetDoubleorDefault("NPMEDMAX");
            NPMEDOOP = reader.GetDoubleorDefault("NPMEDOOP");
            NPMEDDED = reader.GetDoubleorDefault("NPMEDDED");
            GHYTDMAX = reader.GetDoubleorDefault("GHYTDMAX");
            GHLTMAX = reader.GetDoubleorDefault("GHLTMAX");
            CPYSUBS = reader.GetDoubleorDefault("CPYSUBS");
            DEDSUBS = reader.GetDoubleorDefault("DEDSUBS");
            TROOP = reader.GetDoubleorDefault("TROOP");
            TOTPRC = reader.GetDoubleorDefault("TOTPRC");
            GAP_TROOP = reader.GetDoubleorDefault("GAP_TROOP");
            ENRADJ = reader.GetDoubleorDefault("ENRADJ");
            BYATROOP = reader.GetDoubleorDefault("BYATROOP");
            PARTBDED = reader.GetDoubleorDefault("PARTBDED");
            PARTBOOP = reader.GetDoubleorDefault("PARTBOOP");
            PARTBMAX = reader.GetDoubleorDefault("PARTBMAX");
            TROOPIC = reader.GetDoubleorDefault("TROOPIC");
            TOTPRCIC = reader.GetDoubleorDefault("TOTPRCIC");
            CPYSUBSIC = reader.GetDoubleorDefault("CPYSUBSIC");
            GAPCPYSUB = reader.GetDoubleorDefault("GAPCPYSUB");
            GAPTOTPRC = reader.GetDoubleorDefault("GAPTOTPRC");
            NPDEDACC = reader.GetDoubleorDefault("NPDEDACC");
            NPMAXACC = reader.GetDoubleorDefault("NPMAXACC");
            NPOOPACC = reader.GetDoubleorDefault("NPOOPACC");
            TOTPRCCAT = reader.GetDoubleorDefault("TOTPRCCAT");
            TOTPRCDED = reader.GetDoubleorDefault("TOTPRCDED");
        }
    }
}
