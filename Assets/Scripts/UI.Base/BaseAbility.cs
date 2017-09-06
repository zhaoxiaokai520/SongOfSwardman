using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.UI.Mgr;

namespace Assets.Scripts.UI.Base
{
    //attack, defend, cast something, use item
    //front shake, back shake
    class BaseAbility : MonoBehaviour, IUpdateSub
    {
        void Awake()
        {

        }

        void Start()
        {
			GameUpdateMgr.GetInstance().Register (this);
        }

        void OnDestory()
        {
			GameUpdateMgr.instance.Unregister (this);
        }

        public void Move(Vector3 delta)
        {

        }

        public void Stop()
        {

        }

        public void Pause()
        {

        }

        public void Clean()
        {

        }

		public void UpdateSub(float delta)
        {

        }
    }
}
