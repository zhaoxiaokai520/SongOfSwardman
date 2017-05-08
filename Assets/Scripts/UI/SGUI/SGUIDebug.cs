using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.SGUI
{
    class SGUIDebug
    {
        [Conditional("SGUI_DEBUG")]
        public static void Assert(bool condition, string message, string detailMessage)
        {
            System.Diagnostics.Debug.Assert(condition, message, detailMessage);
        }

        [Conditional("SGUI_DEBUG")]
        public static void Log(string logmsg)
        {
            UnityEngine.Debug.Log(logmsg);
        }

        [Conditional("SGUI_DEBUG")]
        public static void LogError(string logmsg)
        {
            UnityEngine.Debug.LogError(logmsg);
        }
    }
}
