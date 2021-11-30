using System;

namespace Devmoba.ToolManager
{
    public static class CommonMethod
    {
        public static long GetTimestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}
