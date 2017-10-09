using Assets.Scripts.UI.Mgr;
using Assets.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Core
{
    //game director, game framework, drive game loop to run
    // the lowest level of game application hierarchy
    class BattlePump : MonoBehaviour, IUpdateSub, ILateUpdateSub
    {
        private static BattlePump _sInstance = null;
        private bool _initialized = false;

        public static BattlePump Instance()
        {
            return _sInstance;
        }

        private void Awake()
        {
            if (_sInstance == null)
            {
                _sInstance = this;
                //GameObject.DontDestroyOnLoad(s_instance);
            }
            InvokeRepeating("UpdateNative", 0.0f, 1f);
        }

        private void UpdateNative()
        {
            DebugHelper.Log("UpdateNative");
            GameCore.Glue.GetInstance().UpdateSub(0);
        }

        private void Start()
        {
			GameUpdateMgr.GetInstance().Register(this);
        }

		void OnDestory()
		{
			GameUpdateMgr.instance.Unregister(this);
		}

		public void FixedUpdateSub(float delta)
		{
			
		}

        public void UpdateSub(float delta)
        {
            if (_initialized)
            {

            }
        }

        public void LateUpdateSub(float delta)
        {

        }

    }
}
