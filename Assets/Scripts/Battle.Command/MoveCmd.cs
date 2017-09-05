using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //Concerete Command
    //use for drive role action or ui action
    //independ what action source is
    class MoveCmd : ICmd
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
