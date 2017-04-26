using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Common;
using Assets.Scripts.Role;

namespace Assets.Scripts.Event
{
    //public delegate void OnTriggerBegin(int senderId, int state);
    //public delegate void OnTriggerGoing(int senderId, int state);
    //public delegate void OnTriggerEnd(int senderId, int state);

    public delegate void InputEvent(SosObject sender, SosEventArgs args);
    public delegate void OutputEvent(SosObject sender, SosEventArgs args);
    //public delegate void OpenEvent(int senderId, int state);

    public class SosEvent
    {
        private int mFlag = 0;

        public virtual void Trigger()
        {

        }

        //trig area
        //trig condition
        //check condition
        public virtual bool Check()
        {
            return false;
        }

        //if a duration event, we update every frame
        public virtual void Update()
        {

        }

        public void SetDebug(bool isDebug)
        {
            if (isDebug)
            {
                mFlag |= SosDefines.EventDebugBit;
            }
            else
            {
                mFlag &= ~SosDefines.EventDebugBit;
            }
        }
    }

    public enum MapEventId
    {
        talk = 1,
        trick,
        trap,
        door,
        itemshop,
        equipshop
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

        public static readonly SosEventArgs EmptyEvt;
    }
}