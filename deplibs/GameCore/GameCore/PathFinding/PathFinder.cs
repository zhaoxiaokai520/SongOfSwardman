using System;
using System.Runtime.InteropServices;

public class PathFinder
{
    //#region cpp bridge interface
    //[DllImport("GameCoreCpp.dll", EntryPoint = "ReqPath", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    //public static extern void ReqPath(VecInt2 from, VecInt2 goal);

    //[DllImport("GameCoreCpp.dll", EntryPoint = "ReqPath", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    //public static extern void ReqPath(VecInt2 from, VecInt2 center, int radius);

    //[DllImport("GameCoreCpp.dll", EntryPoint = "ReqPath", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    //public static extern void ReqPath(VecInt2 from, RECT goal);

    //[DllImport("GameCoreCpp.dll", EntryPoint = "ReqPathInvert", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    //public static extern void ReqPathInvert(VecInt2 from, VecInt2 center, int radius);

    //[DllImport("GameCoreCpp.dll", EntryPoint = "ReqPathInvert", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    //public static extern void ReqPathInvert(VecInt2 from, RECT goal);
    //#endregion

    //void RequestPath(VecInt2 from, VecInt2 goal)
    //{
    //    ReqPath(from, goal);
    //}
    ////in circle
    //void RequestPath(VecInt2 from, VecInt2 center, int radius)
    //{
    //    ReqPath(from, center, radius);
    //}
    ////square
    //void RequestPath(VecInt2 from, RECT rect)
    //{
    //    ReqPath(from, rect);
    //}
    ////inverted circle
    //void RequestPathInvert(VecInt2 from, VecInt2 center, int radius)
    //{
    //    ReqPathInvert(from, center, radius);
    //}

    ////inverted square
    //void RequestPathInvert(VecInt2 from, RECT rect)
    //{
    //    ReqPathInvert(from, rect);
    //}
}
