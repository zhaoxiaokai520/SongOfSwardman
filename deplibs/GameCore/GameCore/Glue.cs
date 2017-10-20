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

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        delegate void AddCallback(int code, CallBack cb);
        delegate void RmvCallback(int code);
        delegate void UpdateNative(int turnLength);
        delegate void OnTimerNative();
#else
        [DllImport("GameCoreCpp")]
        static extern void AddCallback(int code, CallBack cb);
        [DllImport("GameCoreCpp")]
        static extern void RmvCallback(int code);
        [DllImport("GameCoreCpp")]
        static extern void UpdateNative(int turnLength);
        [DllImport("GameCoreCpp")]
        static extern void OnTimerNative();
#endif


        public delegate void CallBack(string data);

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
            Debug.Log("AddListener=========");
            //List<IListener> slot = FetchOrCreateSlot(code);
            //if (l != null && !slot.Contains(l))
            //    slot.Add(l);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            NativePluginHelper.Invoke<AddCallback>(NativePluginHelper.nativeLibraryPtr, code, l);
#else
            AddCallback(code, l);
#endif
        }

        public void RemoveListener(int code)
        {
            Debug.Log("RemoveListener============");
            //List<IListener> slot = FetchOrCreateSlot(code);
            //if (slot.Contains(l))
            //    slot.Remove(l);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            NativePluginHelper.Invoke<RmvCallback>(NativePluginHelper.nativeLibraryPtr, code);
#else
            RmvCallback(code, l);
#endif
        }

        public void UpdateSub(int turnLength)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            NativePluginHelper.Invoke<UpdateNative>(NativePluginHelper.nativeLibraryPtr, turnLength);
#else
            UpdateNative(turnLength);
#endif
        }

        public void OnTimer()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            NativePluginHelper.Invoke<OnTimerNative>(NativePluginHelper.nativeLibraryPtr);
#else
            OnTimerNative();
#endif
        }
    }
}
