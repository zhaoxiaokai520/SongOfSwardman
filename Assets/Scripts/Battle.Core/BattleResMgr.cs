using Assets.Scripts.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Battle.Core
{
    class BattleResMgr : Singleton<BattleResMgr>
    {
        public void LoadBatRes(int bat_res_id)
        {

        }

        public void LoadBatResAll()
        {
            string dir = Application.dataPath;
            AssetBundlMgr.GetInstance().DoLoad(dir, "");
        }

        public void UnloadBatRes(int bat_res_id)
        {

        }

        public void UnloadBatResAll()
        {

        }
    }
}
