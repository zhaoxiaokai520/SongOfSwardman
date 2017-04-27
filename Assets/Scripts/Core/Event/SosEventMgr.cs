using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Role;

namespace Assets.Scripts.Event
{
    public class SosEventMgr : Singleton<SosEventMgr>
    {
        //public enum SosEventType { TALK = 1, TRICK, TRAP, DOOR, ITEM_SHOP, EQUIP_SHOP};

        //Dictionary<string, SosObject> mTargetDic = new Dictionary<string, SosObject>();
        public delegate void SosEventCallback(object sender, SosEvent e);

        // handle sessions
        private Dictionary<int, Session> _sessionSlots = new Dictionary<int, Session>();
        private Dictionary<MapEventId, ConsumableEvent> _eventDic = new Dictionary<MapEventId, ConsumableEvent>();
        //private Session _recentSession = null;

        public void Subscribe(MapEventId evt, ConsumableEvent listener)
        {
            if (!_eventDic.ContainsKey(evt))
            {
                _eventDic.Add(evt, listener);
            }
            else
            {
                _eventDic[evt] += listener;
            }
        }

        public void Unsubscribe(MapEventId evt, ConsumableEvent listener)
        {
            _eventDic[evt] -= listener;
        }

        public void Publish(MapEventId evt, SosObject sender, SosEventArgs args)
        {
            if (_eventDic.ContainsKey(evt) && null != _eventDic[evt])
            {
                Delegate[] cbs = _eventDic[evt].GetInvocationList();
                ConsumableEvent cb = null;
                for (int ii = 0; ii < cbs.Length; ++ii)
                {
                    cb = (ConsumableEvent)cbs[ii];
                    if (null != cb && cb(sender, args))
                    {
                        //consume event and other listener can not receive it
                        break;
                    }
                }
            }
        }

        public void Recycle()
        {
            //clear input
            Dictionary<MapEventId, ConsumableEvent>.Enumerator iter = _eventDic.GetEnumerator();
            while (iter.MoveNext())
            {
                ConsumableEvent evts = _eventDic[iter.Current.Key];
                if (evts != null)
                {
                    Delegate[] cbs = evts.GetInvocationList();
                    for (int ii = 0; ii < cbs.Length; ++ii)
                    {
                        evts -= (ConsumableEvent)cbs[ii];
                    }
                }
            }

            iter.Dispose();
            _eventDic.Clear();
        }

        //public void RegisterEvent(SosEventType etype, string targetId, SosObject target)
        //{
        //    if (etype.Equals(SosEventType.TALK))
        //    {
        //        if (!mTargetDic.ContainsKey(targetId))
        //        {
        //            mTargetDic.Add(targetId, target);
        //        }
        //    }
        //}

        //public void RmvTarget(SosEventType etype, string targetId)
        //{
        //    if (etype.Equals(SosEventType.TALK))
        //    {
        //        if (mTargetDic.ContainsKey(targetId))
        //        {
        //            mTargetDic.Remove(targetId);
        //        }
        //    }
        //}

        //public ICollection<SosObject> GetTargetList()
        //{
        //    return mTargetDic.Values;
        //}

        // implement unity event
        private class Session : AbstractSmartObj
        {
            private int _sessionId = -1;
            public int sessionId
            {
                get
                {
                    return _sessionId;
                }
            }

            public Session()
            {
                _sessionId = System.Guid.NewGuid().GetHashCode();
            }

            private event SosEventCallback evts;

            public void AddListener(SosEventCallback cb)
            {
                evts += cb;
            }

            public void RemoveListener(SosEventCallback cb)
            {
                evts -= cb;
            }

            public void RemoveAllListeners()
            {
                if (evts != null)
                {
                    Delegate[] cbs = evts.GetInvocationList();
                    for (int ii = 0; ii < cbs.Length; ++ii)
                    {
                        evts -= (SosEventCallback)cbs[ii];
                    }
                }
            }

            public void Invoke(object sender, SosEvent e)
            {
                if (evts != null)
                    evts(sender, e);
            }

            public override void OnRelease()
            {
                RemoveAllListeners();
            }
        }
    }
}
