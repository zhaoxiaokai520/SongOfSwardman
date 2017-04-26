using Assets.Scripts.Common;
using Assets.Scripts.Event;
using Assets.Scripts.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Core.Event
{
    //class that unbind event sender and receiver for frequently event trigger change
    // sender only know send event to some object
    // receive only receive some event to action with, and dont care how many sender make the event 
    // and how sender event composite the final event(and, or, xor)
    // event enable logic not mix with the receive and make receive code not CHANGE with event bind change
    //ep. door open by a tile trode, then designer want door open by a monster dead.
    class EventBinder
    {
        protected int _inputNum = 1;
        Dictionary<MapEventId, InputEvent[]> _inputEventDic = new Dictionary<MapEventId, InputEvent[]>();
        Dictionary<MapEventId, OutputEvent> _outputEventDic = new Dictionary<MapEventId, OutputEvent>();

        public void AddOutput(MapEventId evt, OutputEvent output)
        {
            if (_outputEventDic.ContainsKey(evt))
            {
                _outputEventDic[evt] += output;
            }
            else
            {
                _outputEventDic.Add(evt, output);
            }
        }

        public void RmvOutput(MapEventId evt, OutputEvent output)
        {
            if (_outputEventDic.ContainsKey(evt))
            {
                _outputEventDic[evt] -= output;
            }
        }

        public InputEvent CreateInput(SosObject sender, EventArgs args, MapEventId evt, int idx = 0)
        {
            if (idx >= _inputNum)
            {
                DebugHelper.LogError("EventBinder.AddInput error idx " + idx + " max = " + _inputNum);
                return null;
            }

            if (!_inputEventDic.ContainsKey(evt))
            {
                _inputEventDic.Add(evt, new InputEvent[_inputNum]);
            }

            InputEvent retEvent = new InputEvent((SosObject obj, SosEventArgs arg) => { });
            _inputEventDic[evt][idx] = retEvent;

            return retEvent;
        }

        public void RmvInput(MapEventId evt, InputEvent input, int idx = 0)
        {
            if (idx >= _inputNum)
            {
                DebugHelper.LogError("EventBinder.RmvInput error idx " + idx + " max = " + _inputNum);
                return;
            }

            if (_inputEventDic.ContainsKey(evt))
            {
                _inputEventDic[evt][idx] -= input;
            }
        }

        public void InputEventHandler(SosObject sender, SosEventArgs args)
        {

        }

        //NOTE:Must be called !!
        public void Recycle()
        {

        }
    }
}
