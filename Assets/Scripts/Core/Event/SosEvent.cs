using System;
using Assets.Scripts.UI.Base;

namespace Assets.Scripts.Core.Event
{
    //public delegate void OnTriggerBegin(int senderId, int state);
    //public delegate void OnTriggerGoing(int senderId, int state);
    //public delegate void OnTriggerEnd(int senderId, int state);

    // directional event
    public delegate void InputEvent(SosObject sender, SosEventArgs args);
    public delegate void OutputEvent(SosObject sender, SosEventArgs args);

    //broadcast consumable event
    // return: whether is consumed or not, if yes, no other subscriber can get it.
    public delegate bool ConsumableEvent(SosObject sender, SosEventArgs args);
    //public delegate void OpenEvent(int senderId, int state);

    public class SosEvent
    {
        //private int mFlag = 0;

        //public virtual void Trigger()
        //{

        //}

        ////trig area
        ////trig condition
        ////check condition
        //public virtual bool Check()
        //{
        //    return false;
        //}

        ////if a duration event, we update every frame
        //public virtual void Update()
        //{

        //}

        //public void SetDebug(bool isDebug)
        //{
        //    if (isDebug)
        //    {
        //        mFlag |= SosDefines.EventDebugBit;
        //    }
        //    else
        //    {
        //        mFlag &= ~SosDefines.EventDebugBit;
        //    }
        //}
    }

    public enum MapEventId
    {
        dummy = -1,
        talk = 1,
        action,
        trap,
        door,
        itemshop,
        equipshop,
    };

    public enum UIEventId
    {
        showItem = 1,
        showMenu,
        itemChanged,
    };

    public enum CoreEventId
    {
        frameSync = 1,
        frameWindow,
    };

    public class SosEventArgs : EventArgs
    {
        public MapEventId evt;
        public int idx;

        public SosEventArgs()
        {
            idx = 0;
            evt = MapEventId.dummy;
        }

        public static readonly SosEventArgs EmptyEvt;
    }
}