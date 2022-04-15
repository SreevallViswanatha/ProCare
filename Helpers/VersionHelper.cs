using System;
using System.Diagnostics;
using System.Reflection;

namespace ProCare.API.PBM.Helpers
{
    public class VersionHelper
    {
        public static string GetVersion()
        {
            Assembly fileAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileAssembly.Location);
            string productVersion = String.Format("[{0} - {1}]", fvi.FileVersion, (fvi.ProductVersion == "1.0.0.0" ? "Private Dev Build" : fvi.ProductVersion.Trim()));
            return productVersion;
        }
    }
}
