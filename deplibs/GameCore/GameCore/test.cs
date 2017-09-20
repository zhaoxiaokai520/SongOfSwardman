using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class test
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool testInterface([DefaultValue(0.0f)]float p);
    }
}
