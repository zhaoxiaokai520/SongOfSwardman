using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //battle command interface
    abstract class BatCmd
    {
        public abstract void Do();
        public abstract void Undo();
    }
}
