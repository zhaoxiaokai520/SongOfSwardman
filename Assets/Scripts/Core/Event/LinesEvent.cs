using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Event
{
    //role dialog saying lines
    public class LinesEvent : SosEvent
    {
        List<string> lines = new List<string>();

        public void Trigger()
        {

        }

        //check condition
        public bool Check()
        {
            return false;
        }

        //if a duration event, we update every frame
        public void Update()
        {

        }
    }
}
