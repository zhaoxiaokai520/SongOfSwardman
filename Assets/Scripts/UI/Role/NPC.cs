using Assets.Scripts.Event;
using Assets.Scripts.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.Role
{
    class NPC : SosObject
    {
        private void Awake()
        {
#if UNITY_EDITOR
            if (null == gameObject.GetComponent<TalkGizmos>())
            {
                TalkGizmos tg = gameObject.AddComponent<TalkGizmos>();
            }
#endif
        }

        private void Start()
        {
            SosEventMgr.instance.Subscribe(MapEventId.action, Talk);
        }

        private void OnDestroy()
        {

        }

        public bool Talk(SosObject sender, SosEventArgs args)
        {
            return true;
        }
    }
}
