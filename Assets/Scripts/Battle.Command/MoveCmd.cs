using Assets.Scripts.Battle.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //Concerete Command
    //use for drive role action or ui action
    //independ what action source is
    class MoveCmd : MacroCmd
    {
        MoveAction mAction;

        public MoveCmd(MoveAction ma)
        {
            mAction = ma;
        }
        public override void Do()
        {
            
        }

        public override void Undo()
        {
            
        }
    }
}
