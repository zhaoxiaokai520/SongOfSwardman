using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //Macro Command as a Composite
    //has a sequence of commands to execute
    class MacroCmd : BatCmd
    {
        List<BatCmd> mCmds;

        public void AddCmd(BatCmd cmd)
        {
            mCmds.Add(cmd);
        }

        
        public void RmvCmd(int idx)
        {
            if (mCmds.Count > idx)
            {
                mCmds.RemoveAt(idx);
            }
        }

        public override void Do()
        {
            if (null != mCmds)
            {
                for (int i = 0; i < mCmds.Count; i++)
                {
                    if (null != mCmds[i])
                    {
                        mCmds[i].Do();
                    }
                }
            }
        }

        public override void Undo()
        {
            if (null != mCmds)
            {
                for (int i = 0; i < mCmds.Count; i++)
                {
                    if (null != mCmds[i])
                    {
                        mCmds[i].Undo();
                    }
                }
            }
        }
    }
}
