using Assets.Scripts.Battle.Command;
using Assets.Scripts.UI.Mgr;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Battle.Core
{
    //command design pattern invoker
    //Store command for undo operation
    class BatCmdMgr : Singleton<BatCmdMgr>, IUpdateSub
    {
        //List<BatCmd> mCmdList = new List<BatCmd>();
        Queue<BatCmd> mCmdQueue = new Queue<BatCmd>();
        WaitForSeconds mSharedWaitForSeconds = new WaitForSeconds(0.001f);

        public void StoreCmd(BatCmd bc)
        {
            mCmdQueue.Enqueue(bc);
        }

        /// <summary>
        /// start to process commands
        /// </summary>
        public void StartProc(MonoBehaviour mb)
        {
            mb.StopCoroutine("ProcessCommand");
            mb.StartCoroutine("ProcessCommand");
        }

        /// <summary>
        /// stop processing commands
        /// </summary>
        public void StopProc(MonoBehaviour mb)
        {
            mb.StopCoroutine("ProcessCommand");
        }

        public void AddListener()
        {
            GameUpdateMgr.Register(this);
        }

        public void RmvListener()
        {
            GameUpdateMgr.Unregister(this);
        }

        public void UpdateSub(float delta)
        {
            //for (int i = 0; i < mCmdList.Count; i++)
            //{
            //    BatCmd cmd = mCmdList[i];
            //    if (null != cmd)
            //    {
            //        cmd.Do();
            //    }
            //}
        }

        IEnumerator ProcessCommand()
        {
            while (true)
            {
                if (mCmdQueue.Count > 0)
                {
                    BatCmd bc = mCmdQueue.Dequeue();
                    if (null != bc)
                    {
                        bc.Do();
                    }
                    Utility.DebugHelper.Log("proc cmd doing==========");
                    yield return null;
                }
                else
                {
                    Utility.DebugHelper.Log("proc cmd reset for 1ms");
                    //rest for a while
                    yield return mSharedWaitForSeconds;
                }
            }
        }
    }
}
