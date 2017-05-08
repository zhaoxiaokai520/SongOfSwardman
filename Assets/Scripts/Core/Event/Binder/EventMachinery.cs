using Assets.Scripts.UI.Base;
using Assets.Scripts.Core.Event;
using System.Collections.Generic;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Core.Event
{
    //class that unbind event sender and receiver for frequently event trigger change
    // sender only know send event to some object
    // receive only receive some event to action with, and dont care how many sender make the event 
    // and how sender event composite the final event(and, or, xor)
    // event enable logic not mix with the receive and make receive code not CHANGE with event bind change
    //ep. door open by a tile trode, then designer want door open by a monster dead.
    class EventMachinery
    {
        protected int _inputNum = 1;
        Dictionary<MapEventId, MapEventId> _linkDic = new Dictionary<MapEventId, MapEventId>();
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

        public InputEvent CreateInput(SosObject sender, SosEventArgs args, MapEventId evt)
        {
            if (args.idx < 0 || args.idx >= _inputNum)
            {
                DebugHelper.LogError("EventBinder.AddInput error idx " + args.idx + " max = " + _inputNum);
                return null;
            }

            if (!_inputEventDic.ContainsKey(evt))
            {
                _inputEventDic.Add(evt, new InputEvent[_inputNum]);
            }

            //InputEvent retEvent = new InputEvent(InputEventHandler);
            _inputEventDic[evt][args.idx] = InputEventHandler;

            return InputEventHandler;
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

        public void LinkTo(MapEventId input, MapEventId output)
        {
            _linkDic[input] = output;
        }

        public void InputEventHandler(SosObject sender, SosEventArgs args)
        {
            MapEventId outEvt = _linkDic[args.evt];
            if (_outputEventDic.ContainsKey(outEvt))
            {
                _outputEventDic[outEvt](sender, args);
            }
        }

        //NOTE:Must be called !!
        public void Recycle()
        {
            //clear output
            Dictionary<MapEventId, OutputEvent>.Enumerator iter = _outputEventDic.GetEnumerator();
            while (iter.MoveNext())
            {
                _outputEventDic[iter.Current.Key] = null;
            }

            iter.Dispose();
            _outputEventDic.Clear();

            //clear input
            Dictionary<MapEventId, InputEvent[]>.Enumerator iter2 = _inputEventDic.GetEnumerator();
            while (iter2.MoveNext())
            {
                InputEvent[] arr = _inputEventDic[iter2.Current.Key];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = null;
                }
            }

            iter2.Dispose();
            _inputEventDic.Clear();
        }
    }
}
