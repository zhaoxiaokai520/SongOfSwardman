using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.UI.Mgr;

namespace Assets.Scripts.UI.Base
{
    class ActorMover : MonoBehaviour, IUpdateSub
    {
        void Awake()
        {

        }

        void Start()
        {
			UpdateGameMgr.instance.Register (this);
        }

        void OnDestory()
        {
			UpdateGameMgr.instance.Unregister (this);
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
