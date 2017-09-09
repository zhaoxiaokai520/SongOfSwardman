using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Action
{
    //command design pattern
    //base macro action receiver as a composite pattern
    //
    class BaseMacroAction : BaseAction
    {
        public override void DoAction() { }
        public override void UndoAction() { }
    }
}
