using System;
using System.Collections.Generic;
using AOT;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GameCore
{
    public class GlueBattle :Singleton<GlueBattle>
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        delegate void SetBattleMapNative(string mapPath);
        delegate void LoadBattleNative();
        delegate void UnloadBattleNative();
#else
        [DllImport("GameCoreCpp")]
        static extern void LoadBattleNative();
        [DllImport("GameCoreCpp")]
        static extern void UnloadBattleNative();
        [DllImport("GameCoreCpp")]
        static extern float SetBattleMapNative(string mapPath);
#endif
        public static void SetBattleMap(string mapPath)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            NativePluginHelper.Invoke<SetBattleMapNative>(NativePluginHelper.nativeLibraryPtr, mapPath);
#else
            SetBattleMapNative(mapPath);
#endif
        }

        public static void LoadBattle()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            NativePluginHelper.Invoke<LoadBattleNative>(NativePluginHelper.nativeLibraryPtr);
#else
            LoadBattleNative();
#endif
        }

        public static void UnloadBattle()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            NativePluginHelper.Invoke<UnloadBattleNative>(NativePluginHelper.nativeLibraryPtr);
#else
            UnloadBattleNative();
#endif
        }
    }
}
