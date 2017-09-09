using Assets.Scripts.Battle.Action;

namespace Assets.Scripts.Battle.Command
{
    //battle command interface, base class
    class BatCmd
    {
        private BaseAction mCmdReceiver;

        public virtual void Do() { }
        public virtual void Undo() { }
    }
}
