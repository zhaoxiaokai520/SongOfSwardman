using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //Concerete Command
    //Spell command, take magic power as fuel
    class SpellCmd : ICmd
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
