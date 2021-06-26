using System.Collections.Generic;

namespace Devmoba.ToolClient.Services
{
    public static class Console
    {
        public static List<string> Result = new List<string>();

        public static void Log(object value)
        {
            Result.Add(value.ToString());
        }
    }
}
