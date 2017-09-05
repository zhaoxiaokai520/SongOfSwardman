using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //Concerete Command
    //Marvelous command, take spirit as fuel
    //Also include incantation, Ninjitsu
    class MavlCmd : ICmd
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
