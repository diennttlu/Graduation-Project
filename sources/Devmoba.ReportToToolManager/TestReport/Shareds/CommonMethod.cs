using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TestReport.Shareds
{
    public class CommonMethod
    {
        public static string GetAppName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        public static Guid? GetAppId()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(false).OfType<GuidAttribute>();
            if (attributes.Any())
            {
                return Guid.Parse(attributes.First().Value);
            }
            return null;
        }

        public static string GetAppVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetExeFilePath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        public static int GetProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }
    }
}
