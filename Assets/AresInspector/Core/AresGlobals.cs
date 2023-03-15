using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
public static class AresGlobals
{
    static bool EditorOnly() { return !Application.isPlaying; }
    static bool RuntimeOnly() { return Application.isPlaying; }
}
#endif