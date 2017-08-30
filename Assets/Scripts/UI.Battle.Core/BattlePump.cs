using Assets.Scripts.UI.Mgr;
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
