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

        GameCore.Glue glue;

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

        private void Start()
        {
            NativePluginHelper.LoadNativeDll();
            AddListener();
            GameUpdateMgr.Register(this);
            GameCore.GlueBattle.SetBattleMap("test_map.xml");
            GameCore.GlueBattle.LoadBattle();
        }

		void OnDestory()
		{
            Debug.Log("BattlePump.OnDestory()");
            GameCore.GlueBattle.UnloadBattle();
            GameUpdateMgr.Unregister(this);
            RmvListener();
		}

        void OnApplicationQuit()
        {
            NativePluginHelper.OnAppQuit();
        }

        public void FixedUpdateSub(float delta)
		{
			
		}

        private void UpdateNative()
        {
            DebugHelper.Log("UpdateNative");
            //GameCore.Glue.GetInstance().UpdateSub(0);
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

        void AddListener()
        {
            GameUpdateMgr.Register(this);
            glue = GameCore.Glue.GetInstance();
            glue.AddListener(1, NativeCallback);
        }

        void RmvListener()
        {
            //TODO OPTIONAL:GameUpdateMgr instance is null when stop editor running, 
            //Object.FindObjectByType failed of MonoSingleton
            GameUpdateMgr.Unregister(this);
            glue.RemoveListener(1);
        }

        void NativeCallback(string data)
        {
            DebugHelper.Log("BattleBump receive native log :" + data);
        }
    }
}
