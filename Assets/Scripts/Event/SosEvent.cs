using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Common;

namespace Assets.Scripts.Event
{
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
}
