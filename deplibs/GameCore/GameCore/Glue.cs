using System;
using System.Collections.Generic;
using AOT;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GameCore
{
    public class Glue :Singleton<Glue>
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Parameter
        {
            public int a;
            public int b;
        }

        [DllImport("GameCoreCpp")]
        static extern void AddCallback(int code, CallBack cb);
        [DllImport("GameCoreCpp")]
        static extern void RmvCallback(int code, CallBack cb);
        [DllImport("GameCoreCpp")]
        static extern void UpdateNative(int turnLength);

        public delegate void CallBack();

        [MonoPInvokeCallback(typeof(CallBack))]
        static void CallBackFunc(IntPtr param)
        {
            var p = (Parameter)Marshal.PtrToStructure(param, typeof(Parameter));
            Debug.Log("a:" + p.a + " b:" + p.b);
        }

        public interface IListener
        {
            void OnNotifyGameStatus();
        }

        private List<IListener> FetchOrCreateSlot(int code)
        {
            List<IListener> slot = null;
            //if (_listenerSlots.ContainsKey(code))
            //    slot = _listenerSlots[code];
            //else
            //{
            //    slot = new List<IListener>();
            //    _listenerSlots.Add(code, slot);
            //}
            return slot;
        }

        public void AddListener(int code, CallBack l)
        {
            //List<IListener> slot = FetchOrCreateSlot(code);
            //if (l != null && !slot.Contains(l))
            //    slot.Add(l);
            AddCallback(code, l);
        }

        public void RemoveListener(int code, CallBack l)
        {
            //List<IListener> slot = FetchOrCreateSlot(code);
            //if (slot.Contains(l))
            //    slot.Remove(l);
            RmvCallback(code, l);
        }

        public void UpdateSub(int turnLength)
        {
            UpdateNative(turnLength);
        }
    }
}
