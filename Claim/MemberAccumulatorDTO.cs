using ProCare.Common.Data;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class MemberAccumulatorDTO : ILoadFromDataReader
    {
        public double DEDUCT { get; set; }
        public double SUB_DEDUCT { get; set; }
        public double INDDEDUCT { get; set; }
        public double FAMDEDUCT { get; set; }
        public double DEDMETDT { get; set; }
        public double COPAY { get; set; }
        public double SUB_COPAY { get; set; }
        public double INDPOCKET { get; set; }
        public double FAMPOCKET { get; set; }
        public double OOPMETDT { get; set; }
        public double YTDDOLLAR { get; set; }
        public double SUB_YTDDOLLAR { get; set; }
        public double INDBENEFIT { get; set; }
        public double FAMBENEFIT { get; set; }
        public double MAXMETDT { get; set; }
        public double YTDRX { get; set; }
        public double SUB_YTDRX { get; set; }
        public double INDMAXRX { get; set; }
        public double FAMMAXRX { get; set; }
        public double SPECDED { get; set; }
        public double SUB_SPECDED { get; set; }
        public double ISPECDED { get; set; }
        public double FSPECDED { get; set; }
        public double SPECOOP { get; set; }
        public double SUB_SPECOOP { get; set; }
        public double ISPECOOP { get; set; }
        public double FSPECOOP { get; set; }
        public double SPECDOLLAR { get; set; }
        public double SUB_SPECDOLLAR { get; set; }
        public double ISPECBENE { get; set; }
        public double FSPECBENE { get; set; }
        public double SMOKINGYTD { get; set; }
        public double SMOKINGLT { get; set; }
        public double SMKYTDMAX { get; set; }
        public double SMKLTMAX { get; set; }
        public double GHYTDMAX { get; set; }
        public double GHLTMAX { get; set; }
        public double PLN_GHYTDMAX { get; set; }
        public double PLN_GHLTMAX { get; set; }
        public double FERYTDMAX { get; set; }
        public double FERLTMAX { get; set; }
        public double PLN_FERYTDMAX { get; set; }
        public double PLN_FERLTMAX { get; set; }
        public double OCYTD { get; set; }
        public double OCLIFE { get; set; }
        public double PLN_OCYTD { get; set; }
        public double PLN_OCLIFE { get; set; }
        public double ENRAMT { get; set; }
        public double SUB_ENRAMT { get; set; }
        public double LIFETIME_DEDUCT { get; set; }
        public double LIFETIME_COPAY { get; set; }
        public double LIFETIME_YTDDOLLAR { get; set; }
        public double LIFETIME_YTDRX { get; set; }
        public double LIFETIME_SPECDED { get; set; }
        public double LIFETIME_SPECOOP { get; set; }
        public double LIFETIME_SPECDOLLAR { get; set; }
        public double LIFETIME_ENRAMT { get; set; }
        public double LIFETIME_SUB_DEDUCT { get; set; }
        public double LIFETIME_SUB_COPAY { get; set; }
        public double LIFETIME_SUB_YTDDOLLAR { get; set; }
        public double LIFETIME_SUB_YTDRX { get; set; }
        public double LIFETIME_SUB_SPECDED { get; set; }
        public double LIFETIME_SUB_SPECOOP { get; set; }
        public double LIFETIME_SUB_SPECDOLLAR { get; set; }
        public double LIFETIME_SUB_ENRAMT { get; set; }

        public void LoadFromDataReader(IDataReader reader)
        {
            DEDUCT = reader.GetDoubleorDefault("DEDUCT");
            SUB_DEDUCT = reader.GetDoubleorDefault("SUB_DEDUCT");
            INDDEDUCT = reader.GetDoubleorDefault("INDDEDUCT");
            FAMDEDUCT = reader.GetDoubleorDefault("FAMDEDUCT");
            DEDMETDT = reader.GetDoubleorDefault("DEDMETDT");
            COPAY = reader.GetDoubleorDefault("COPAY");
            SUB_COPAY = reader.GetDoubleorDefault("SUB_COPAY");
            INDPOCKET = reader.GetDoubleorDefault("INDPOCKET");
            FAMPOCKET = reader.GetDoubleorDefault("FAMPOCKET");
            OOPMETDT = reader.GetDoubleorDefault("OOPMETDT");
            YTDDOLLAR = reader.GetDoubleorDefault("YTDDOLLAR");
            SUB_YTDDOLLAR = reader.GetDoubleorDefault("SUB_YTDDOLLAR");
            INDBENEFIT = reader.GetDoubleorDefault("INDBENEFIT");
            FAMBENEFIT = reader.GetDoubleorDefault("FAMBENEFIT");
            MAXMETDT = reader.GetDoubleorDefault("MAXMETDT");
            YTDRX = reader.GetDoubleorDefault("YTDRX");
            SUB_YTDRX = reader.GetDoubleorDefault("SUB_YTDRX");
            INDMAXRX = reader.GetDoubleorDefault("INDMAXRX");
            FAMMAXRX = reader.GetDoubleorDefault("FAMMAXRX");
            SPECDED = reader.GetDoubleorDefault("SPECDED");
            SUB_SPECDED = reader.GetDoubleorDefault("SUB_SPECDED");
            ISPECDED = reader.GetDoubleorDefault("ISPECDED");
            FSPECDED = reader.GetDoubleorDefault("FSPECDED");
            SPECOOP = reader.GetDoubleorDefault("SPECOOP");
            SUB_SPECOOP = reader.GetDoubleorDefault("SUB_SPECOOP");
            ISPECOOP = reader.GetDoubleorDefault("ISPECOOP");
            FSPECOOP = reader.GetDoubleorDefault("FSPECOOP");
            SPECDOLLAR = reader.GetDoubleorDefault("SPECDOLLAR");
            SUB_SPECDOLLAR = reader.GetDoubleorDefault("SUB_SPECDOLLAR");
            ISPECBENE = reader.GetDoubleorDefault("ISPECBENE");
            FSPECBENE = reader.GetDoubleorDefault("FSPECBENE");
            SMOKINGYTD = reader.GetDoubleorDefault("SMOKINGYTD");
            SMOKINGLT = reader.GetDoubleorDefault("SMOKINGLT");
            SMKYTDMAX = reader.GetDoubleorDefault("SMKYTDMAX");
            SMKLTMAX = reader.GetDoubleorDefault("SMKLTMAX");
            GHYTDMAX = reader.GetDoubleorDefault("GHYTDMAX");
            GHLTMAX = reader.GetDoubleorDefault("GHLTMAX");
            PLN_GHYTDMAX = reader.GetDoubleorDefault("PLN_GHYTDMAX");
            PLN_GHLTMAX = reader.GetDoubleorDefault("PLN_GHLTMAX");
            FERYTDMAX = reader.GetDoubleorDefault("FERYTDMAX");
            FERLTMAX = reader.GetDoubleorDefault("FERLTMAX");
            PLN_FERYTDMAX = reader.GetDoubleorDefault("PLN_FERYTDMAX");
            PLN_FERLTMAX = reader.GetDoubleorDefault("PLN_FERLTMAX");
            OCYTD = reader.GetDoubleorDefault("OCYTD");
            OCLIFE = reader.GetDoubleorDefault("OCLIFE");
            PLN_OCYTD = reader.GetDoubleorDefault("PLN_OCYTD");
            PLN_OCLIFE = reader.GetDoubleorDefault("PLN_OCLIFE");
            ENRAMT = reader.GetDoubleorDefault("ENRAMT");
            SUB_ENRAMT = reader.GetDoubleorDefault("SUB_ENRAMT");
            LIFETIME_DEDUCT = reader.GetDoubleorDefault("LIFETIME_DEDUCT");
            LIFETIME_COPAY = reader.GetDoubleorDefault("LIFETIME_COPAY");
            LIFETIME_YTDDOLLAR = reader.GetDoubleorDefault("LIFETIME_YTDDOLLAR");
            LIFETIME_YTDRX = reader.GetDoubleorDefault("LIFETIME_YTDRX");
            LIFETIME_SPECDED = reader.GetDoubleorDefault("LIFETIME_SPECDED");
            LIFETIME_SPECOOP = reader.GetDoubleorDefault("LIFETIME_SPECOOP");
            LIFETIME_SPECDOLLAR = reader.GetDoubleorDefault("LIFETIME_SPECDOLLAR");
            LIFETIME_ENRAMT = reader.GetDoubleorDefault("LIFETIME_ENRAMT");
            LIFETIME_SUB_DEDUCT = reader.GetDoubleorDefault("LIFETIME_SUB_DEDUCT");
            LIFETIME_SUB_COPAY = reader.GetDoubleorDefault("LIFETIME_SUB_COPAY");
            LIFETIME_SUB_YTDDOLLAR = reader.GetDoubleorDefault("LIFETIME_SUB_YTDDOLLAR");
            LIFETIME_SUB_YTDRX = reader.GetDoubleorDefault("LIFETIME_SUB_YTDRX");
            LIFETIME_SUB_SPECDED = reader.GetDoubleorDefault("LIFETIME_SUB_SPECDED");
            LIFETIME_SUB_SPECOOP = reader.GetDoubleorDefault("LIFETIME_SUB_SPECOOP");
            LIFETIME_SUB_SPECDOLLAR = reader.GetDoubleorDefault("LIFETIME_SUB_SPECDOLLAR");
            LIFETIME_SUB_ENRAMT = reader.GetDoubleorDefault("LIFETIME_SUB_ENRAMT");
        }

        public void LoadFromDataReaderWithPrefix(IDataReader reader, string prefix)
        {
            DEDUCT = reader.GetDoubleorDefault(prefix + "DEDUCT");
            SUB_DEDUCT = reader.GetDoubleorDefault(prefix + "SUB_DEDUCT");
            INDDEDUCT = reader.GetDoubleorDefault(prefix + "INDDEDUCT");
            FAMDEDUCT = reader.GetDoubleorDefault(prefix + "FAMDEDUCT");
            DEDMETDT = reader.GetDoubleorDefault(prefix + "DEDMETDT");
            COPAY = reader.GetDoubleorDefault(prefix + "COPAY");
            SUB_COPAY = reader.GetDoubleorDefault(prefix + "SUB_COPAY");
            INDPOCKET = reader.GetDoubleorDefault(prefix + "INDPOCKET");
            FAMPOCKET = reader.GetDoubleorDefault(prefix + "FAMPOCKET");
            OOPMETDT = reader.GetDoubleorDefault(prefix + "OOPMETDT");
            YTDDOLLAR = reader.GetDoubleorDefault(prefix + "YTDDOLLAR");
            SUB_YTDDOLLAR = reader.GetDoubleorDefault(prefix + "SUB_YTDDOLLAR");
            INDBENEFIT = reader.GetDoubleorDefault(prefix + "INDBENEFIT");
            FAMBENEFIT = reader.GetDoubleorDefault(prefix + "FAMBENEFIT");
            MAXMETDT = reader.GetDoubleorDefault(prefix + "MAXMETDT");
            YTDRX = reader.GetDoubleorDefault(prefix + "YTDRX");
            SUB_YTDRX = reader.GetDoubleorDefault(prefix + "SUB_YTDRX");
            INDMAXRX = reader.GetDoubleorDefault(prefix + "INDMAXRX");
            FAMMAXRX = reader.GetDoubleorDefault(prefix + "FAMMAXRX");
            SPECDED = reader.GetDoubleorDefault(prefix + "SPECDED");
            SUB_SPECDED = reader.GetDoubleorDefault(prefix + "SUB_SPECDED");
            ISPECDED = reader.GetDoubleorDefault(prefix + "ISPECDED");
            FSPECDED = reader.GetDoubleorDefault(prefix + "FSPECDED");
            SPECOOP = reader.GetDoubleorDefault(prefix + "SPECOOP");
            SUB_SPECOOP = reader.GetDoubleorDefault(prefix + "SUB_SPECOOP");
            ISPECOOP = reader.GetDoubleorDefault(prefix + "ISPECOOP");
            FSPECOOP = reader.GetDoubleorDefault(prefix + "FSPECOOP");
            SPECDOLLAR = reader.GetDoubleorDefault(prefix + "SPECDOLLAR");
            SUB_SPECDOLLAR = reader.GetDoubleorDefault(prefix + "SUB_SPECDOLLAR");
            ISPECBENE = reader.GetDoubleorDefault(prefix + "ISPECBENE");
            FSPECBENE = reader.GetDoubleorDefault(prefix + "FSPECBENE");
            SMOKINGYTD = reader.GetDoubleorDefault(prefix + "SMOKINGYTD");
            SMOKINGLT = reader.GetDoubleorDefault(prefix + "SMOKINGLT");
            SMKYTDMAX = reader.GetDoubleorDefault(prefix + "SMKYTDMAX");
            SMKLTMAX = reader.GetDoubleorDefault(prefix + "SMKLTMAX");
            GHYTDMAX = reader.GetDoubleorDefault(prefix + "GHYTDMAX");
            GHLTMAX = reader.GetDoubleorDefault(prefix + "GHLTMAX");
            PLN_GHYTDMAX = reader.GetDoubleorDefault(prefix + "PLN_GHYTDMAX");
            PLN_GHLTMAX = reader.GetDoubleorDefault(prefix + "PLN_GHLTMAX");
            FERYTDMAX = reader.GetDoubleorDefault(prefix + "FERYTDMAX");
            FERLTMAX = reader.GetDoubleorDefault(prefix + "FERLTMAX");
            PLN_FERYTDMAX = reader.GetDoubleorDefault(prefix + "PLN_FERYTDMAX");
            PLN_FERLTMAX = reader.GetDoubleorDefault(prefix + "PLN_FERLTMAX");
            OCYTD = reader.GetDoubleorDefault(prefix + "OCYTD");
            OCLIFE = reader.GetDoubleorDefault(prefix + "OCLIFE");
            PLN_OCYTD = reader.GetDoubleorDefault(prefix + "PLN_OCYTD");
            PLN_OCLIFE = reader.GetDoubleorDefault(prefix + "PLN_OCLIFE");
            ENRAMT = reader.GetDoubleorDefault(prefix + "ENRAMT");
            SUB_ENRAMT = reader.GetDoubleorDefault(prefix + "SUB_ENRAMT");
            LIFETIME_DEDUCT = reader.GetDoubleorDefault(prefix + "LIFETIME_DEDUCT");
            LIFETIME_COPAY = reader.GetDoubleorDefault(prefix + "LIFETIME_COPAY");
            LIFETIME_YTDDOLLAR = reader.GetDoubleorDefault(prefix + "LIFETIME_YTDDOLLAR");
            LIFETIME_YTDRX = reader.GetDoubleorDefault(prefix + "LIFETIME_YTDRX");
            LIFETIME_SPECDED = reader.GetDoubleorDefault(prefix + "LIFETIME_SPECDED");
            LIFETIME_SPECOOP = reader.GetDoubleorDefault(prefix + "LIFETIME_SPECOOP");
            LIFETIME_SPECDOLLAR = reader.GetDoubleorDefault(prefix + "LIFETIME_SPECDOLLAR");
            LIFETIME_ENRAMT = reader.GetDoubleorDefault(prefix + "LIFETIME_ENRAMT");
            LIFETIME_SUB_DEDUCT = reader.GetDoubleorDefault(prefix + "LIFETIME_SUB_DEDUCT");
            LIFETIME_SUB_COPAY = reader.GetDoubleorDefault(prefix + "LIFETIME_SUB_COPAY");
            LIFETIME_SUB_YTDDOLLAR = reader.GetDoubleorDefault(prefix + "LIFETIME_SUB_YTDDOLLAR");
            LIFETIME_SUB_YTDRX = reader.GetDoubleorDefault(prefix + "LIFETIME_SUB_YTDRX");
            LIFETIME_SUB_SPECDED = reader.GetDoubleorDefault(prefix + "LIFETIME_SUB_SPECDED");
            LIFETIME_SUB_SPECOOP = reader.GetDoubleorDefault(prefix + "LIFETIME_SUB_SPECOOP");
            LIFETIME_SUB_SPECDOLLAR = reader.GetDoubleorDefault(prefix + "LIFETIME_SUB_SPECDOLLAR");
            LIFETIME_SUB_ENRAMT = reader.GetDoubleorDefault(prefix + "LIFETIME_SUB_ENRAMT");
        }
    }
}
