#define SOS_DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Common
{
    class DebugHelper
    {
        [Conditional("SOS_DEBUG")]
        public static void Assert(bool condition, string message, string detailMessage)
        {
            System.Diagnostics.Debug.Assert(condition, message, detailMessage);
        }

        [Conditional("SOS_DEBUG")]
        public static void Log(string logmsg)
        {
            UnityEngine.Debug.Log(logmsg);
        }

        [Conditional("SOS_DEBUG")]
        public static void LogError(string logmsg)
        {
            UnityEngine.Debug.LogError(logmsg);
        }

        [Conditional("SOS_DEBUG")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            UnityEngine.Debug.DrawLine(start, end, color);
        }

        [Conditional("SOS_DEBUG")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            UnityEngine.Debug.DrawLine(start, end, color, duration);
        }

        [Conditional("SOS_DEBUG")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color,  float duration, bool depthTest)
        {
            UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
        }
    }
}
