using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Action
{
    //command design pattern
    //base action receiver
    //
    class BaseAction
    {
        public virtual void DoAction() { }
        public virtual void UndoAction() { }
    }
}
