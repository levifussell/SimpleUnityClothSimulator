using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Timer
{
    public delegate void FunctionToTime();

    public static void TimeThis(FunctionToTime f, string name)
    {
        Stopwatch sw = Stopwatch.StartNew();
        f();
        sw.Stop();
        UnityEngine.Debug.Log(name + ": " + sw.ElapsedMilliseconds + "ms");
    }
}
