using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GameCore
{
    public class test
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        delegate int testInterface();
#else
        [DllImport("GameCoreCpp", EntryPoint = "testInterface", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int testInterface();
#endif
        

        public int testDummy([DefaultValue(0.0f)]float p)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            Debug.Log("testDummy============1");
        return NativePluginHelper.Invoke<int, testInterface>(NativePluginHelper.nativeLibraryPtr);
#else
            Debug.Log("testDummy============2");
            return testInterface();
#endif

        }
    }
}
