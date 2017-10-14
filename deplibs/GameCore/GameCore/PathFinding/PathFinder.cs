using System;
using System.Runtime.InteropServices;

public class PathFinder
{
    #region cpp bridge interface
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    delegate void ReqPathP(VecInt2 from, VecInt2 goal);
    delegate void ReqPathC(VecInt2 from, VecInt2 center, int radius);
    delegate void ReqPathR(VecInt2 from, RECT goal);
    delegate void ReqPathInvertC(VecInt2 from, VecInt2 center, int radius);
    delegate void ReqPathInvertR(VecInt2 from, RECT goal);
#else
    [DllImport("GameCoreCpp", EntryPoint = "ReqPathP", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ReqPathP(VecInt2 from, VecInt2 goal);

    [DllImport("GameCoreCpp", EntryPoint = "ReqPathC", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ReqPathC(VecInt2 from, VecInt2 center, int radius);

    [DllImport("GameCoreCpp", EntryPoint = "ReqPathR", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ReqPathR(VecInt2 from, RECT goal);

    [DllImport("GameCoreCpp", EntryPoint = "ReqPathInvertC", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ReqPathInvertC(VecInt2 from, VecInt2 center, int radius);

    [DllImport("GameCoreCpp", EntryPoint = "ReqPathInvertR", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ReqPathInvertR(VecInt2 from, RECT goal);
#endif
    #endregion

    public static void RequestPath(VecInt2 from, VecInt2 goal)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        NativePluginHelper.Invoke<ReqPathP>(NativePluginHelper.nativeLibraryPtr, from, goal);
#else
        ReqPathP(from, goal);
#endif

    }
    //in circle
    public static void RequestPath(VecInt2 from, VecInt2 center, int radius)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        NativePluginHelper.Invoke<ReqPathC>(NativePluginHelper.nativeLibraryPtr, from, center, radius);
#else
        ReqPathC(from, center, radius);
#endif

    }
    //square
    public static void RequestPath(VecInt2 from, RECT rect)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        NativePluginHelper.Invoke<ReqPathR>(NativePluginHelper.nativeLibraryPtr, from, rect);
#else
        ReqPathR(from, rect);
#endif

    }
    //inverted circle
    public static void RequestPathInvert(VecInt2 from, VecInt2 center, int radius)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        NativePluginHelper.Invoke<ReqPathInvertC>(NativePluginHelper.nativeLibraryPtr, from, center, radius);
#else
        ReqPathInvertC(from, center, radius);
#endif

    }

    //inverted square
    public static void RequestPathInvert(VecInt2 from, RECT rect)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        NativePluginHelper.Invoke<ReqPathInvertR>(NativePluginHelper.nativeLibraryPtr, from, rect);
#else
        ReqPathInvertR(from, rect);
#endif
    }
}
