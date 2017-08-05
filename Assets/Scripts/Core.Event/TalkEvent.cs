using System.Collections.Generic;

namespace Assets.Scripts.Core.Event
{
    //role dialog saying lines
    public class TalkEvent : SosEvent
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
        public void UpdateEvent()
        {

        }
    }
}
