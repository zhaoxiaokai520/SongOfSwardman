using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //battle command interface
    interface ICmd
    {
        //int cmd_id;
        //byte[] data;
        //int len;//data length
        void Do();
        void Undo();
    }
}
