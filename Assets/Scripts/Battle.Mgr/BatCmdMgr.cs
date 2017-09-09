using Assets.Scripts.Battle.Command;
using Assets.Scripts.UI.Mgr;
using System.Collections.Generic;

namespace Assets.Scripts.Battle.Mgr
{
    //command design pattern invoker
    //Store command for undo operation
    class BatCmdMgr : Singleton<BatCmdMgr>, IUpdateSub
    {
        List<BatCmd> mCmdList = new List<BatCmd>();

        public void AddListener()
        {
            GameUpdateMgr.GetInstance().Register(this);
        }

        public void RmvListener()
        {
            GameUpdateMgr.instance.Unregister(this);
        }

        public void UpdateSub(float delta)
        {
            for (int i = 0; i < mCmdList.Count; i++)
            {
                BatCmd cmd = mCmdList[i];
                if (null != cmd)
                {
                    cmd.Do();
                }
            }
        }
    }
}
