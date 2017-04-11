using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Role;

namespace Assets.Scripts.Event
{
    public class SosEventMgr : Singleton<SosEventMgr>
    {
        public enum SosEventType { TALK = 1, TRICK, TRAP, DOOR, ITEM_SHOP, EQUIP_SHOP};

        Dictionary<string, SosObject> mTargetDic = new Dictionary<string, SosObject>();

        public void AddEvent(SosEvent evt)
        {

        }

        public void RmvEvent()
        {

        }

        public void RegisterEvent(SosEventType etype, string targetId, SosObject target)
        {
            if (etype.Equals(SosEventType.TALK))
            {
                if (!mTargetDic.ContainsKey(targetId))
                {
                    mTargetDic.Add(targetId, target);
                }
            }
        }

        public void RmvTarget(SosEventType etype, string targetId)
        {
            if (etype.Equals(SosEventType.TALK))
            {
                if (mTargetDic.ContainsKey(targetId))
                {
                    mTargetDic.Remove(targetId);
                }
            }
        }

        public ICollection<SosObject> GetTargetList()
        {
            return mTargetDic.Values;
        }
    }
}
