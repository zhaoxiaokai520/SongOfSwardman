using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    class DebugHelper
    {
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR"), Conditional("SOS_LOG_DEBUG")]
        public static void Assert(bool condition, string message, string detailMessage)
        {
            System.Diagnostics.Debug.Assert(condition, message, detailMessage);
        }

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR"), Conditional("SOS_LOG_DEBUG")]
        public static void Log(string logmsg)
        {
            UnityEngine.Debug.Log(logmsg);
        }

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR"), Conditional("SOS_LOG_DEBUG")]
        public static void LogError(string logmsg)
        {
            UnityEngine.Debug.LogError(logmsg);
        }

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR"), Conditional("SOS_LOG_DEBUG")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            UnityEngine.Debug.DrawLine(start, end, color);
        }

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR"), Conditional("SOS_LOG_DEBUG")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            UnityEngine.Debug.DrawLine(start, end, color, duration);
        }

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR"), Conditional("SOS_LOG_DEBUG")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color,  float duration, bool depthTest)
        {
            UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
        }
    }
}
