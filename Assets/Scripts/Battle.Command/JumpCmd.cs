using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //Concerete Command
    //internal composite command, not see in battle menu
    class JumpCmd : ICmd
    {
        int cmd_id;
        byte[] data;
        int len;//data length

        public void Do()
        {
            
        }

        public void Undo()
        {
            
        }
    }
}
