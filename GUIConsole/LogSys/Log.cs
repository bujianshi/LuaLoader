using System;
using System.Diagnostics;

namespace LuaHotLoader.LogSys
{
    class Log
    {
        public static Action<string> DelLogError;
        public static Action<string> DelLogWarn;
        public static Action<string> DelLogDebug;

        public static void LogE(string err)
        {
            if (DelLogError != null)
            {
                DelLogError(err);
            }
        }

        public static void LogW(string warn)
        {
            if (DelLogWarn != null)
            {
                DelLogWarn(warn);
            }
        }

        public static void LogD(string info)
        {
            if (DelLogDebug != null)
            {
                DelLogDebug(info);
            }
        }
    }
}
