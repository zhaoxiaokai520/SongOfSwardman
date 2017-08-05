using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Base;

namespace Assets.Scripts.Core.Event
{
    public class SosEventMgr : Singleton<SosEventMgr>
    {
        //public enum SosEventType { TALK = 1, TRICK, TRAP, DOOR, ITEM_SHOP, EQUIP_SHOP};

        //Dictionary<string, SosObject> mTargetDic = new Dictionary<string, SosObject>();
        public delegate void SosEventCallback(object sender, SosEvent e);

        // handle sessions
        private Dictionary<int, Session> _sessionSlots = new Dictionary<int, Session>();
        private SosEventWorker<MapEventId> _mapEventWorker = new SosEventWorker<MapEventId>();
        private SosEventWorker<UIEventId> _uiEventWorker = new SosEventWorker<UIEventId>();
        private SosEventWorker<CoreEventId> _coreEventWorker = new SosEventWorker<CoreEventId>();
        //private Dictionary<MapEventId, ConsumableEvent> _eventDic = new Dictionary<MapEventId, ConsumableEvent>();
        //private Session _recentSession = null;

        //can not convert from T to enum, so we have to copy function prototype
        //Map event
        public void Subscribe (MapEventId evt, ConsumableEvent listener)
        {
            _mapEventWorker.Subscribe(evt, listener);
        }

        public void Unsubscribe(MapEventId evt, ConsumableEvent listener)
        {
            //_eventDic[evt] -= listener;
            _mapEventWorker.Unsubscribe(evt, listener);
        }

        public void Publish(MapEventId evt, SosObject sender, SosEventArgs args)
        {
            _mapEventWorker.Publish(evt, sender, args);
        }

        //UI event
        public void Subscribe(UIEventId evt, ConsumableEvent listener)
        {
            _uiEventWorker.Subscribe(evt, listener);
        }

        public void Unsubscribe(UIEventId evt, ConsumableEvent listener)
        {
            //_eventDic[evt] -= listener;
            _uiEventWorker.Unsubscribe(evt, listener);
        }

        public void Publish(UIEventId evt, SosObject sender, SosEventArgs args)
        {
            _uiEventWorker.Publish(evt, sender, args);
        }

        //Core event
        public void Subscribe(CoreEventId evt, ConsumableEvent listener)
        {
            _coreEventWorker.Subscribe(evt, listener);
        }

        public void Unsubscribe(CoreEventId evt, ConsumableEvent listener)
        {
            _coreEventWorker.Unsubscribe(evt, listener);
        }

        public void Publish(CoreEventId evt, SosObject sender, SosEventArgs args)
        {
            _coreEventWorker.Publish(evt, sender, args);
        }

        public void Recycle()
        {
            _mapEventWorker.Recycle();
            _uiEventWorker.Recycle();
            _coreEventWorker.Recycle();
        }

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

        private class SosEventWorker<T> where T : IComparable
        {
            private Dictionary<T, ConsumableEvent> _eventDic = new Dictionary<T, ConsumableEvent>();

            public void Subscribe(T evt, ConsumableEvent listener)
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

            public void Unsubscribe(T evt, ConsumableEvent listener)
            {
                _eventDic[evt] -= listener;
            }

            public void Publish(T evt, SosObject sender, SosEventArgs args)
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
                Dictionary<T, ConsumableEvent>.Enumerator iter = _eventDic.GetEnumerator();
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
        }
    }
}
