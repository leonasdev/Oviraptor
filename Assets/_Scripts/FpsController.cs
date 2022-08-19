using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Application.targetFrameRate = -1;
    }
}
