using System.ComponentModel;
using System.Runtime.InteropServices;

namespace GameCore
{
    public class test
    {
        [DllImport("GameCoreCpp.dll", EntryPoint = "testInterface", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int testInterface();

        public bool testDummy([DefaultValue(0.0f)]float p)
        {
            testInterface();
            return false;
        }
    }
}
