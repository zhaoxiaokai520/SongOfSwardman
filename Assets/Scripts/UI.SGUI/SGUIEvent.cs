using System;
using Assets.Scripts.UI.Base;

namespace Assets.SGUI
{
    // directional pipe event
    public delegate void InputEvent(SosObject sender, SGUIEventArgs args);
    public delegate void OutputEvent(SosObject sender, SGUIEventArgs args);

    //broadcast consumable event
    // return: whether is consumed or not, if yes, no other subscriber can get it.
    public delegate bool ConsumableEvent(SosObject sender, SGUIEventArgs args);
    //public delegate void OpenEvent(int senderId, int state);

    public class SGUIEvent
    {
        public int eventId;
    }

    public enum SGUIEventId
    {
        frameSync = 1,
        frameWindow,
        maxid,
    };

    public class SGUIEventArgs : EventArgs
    {
        public int idx;
        public uint heroId;
        public int selectIndex;
        public int tag;
        public int tag2;
        public int tag3;
        public string tagStr;
        public uint tagUInt;
        public int skillSlotId;
        public int sliderValue;

        public SGUIEventArgs()
        {
            idx = 0;
        }

        public static readonly SGUIEventArgs EmptyEvt;
    }
}