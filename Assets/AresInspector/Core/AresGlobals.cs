using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace Ares
{
    public static class AresGlobals
    {
        static bool EditorOnly() { return !Application.isPlaying; }
        static bool RuntimeOnly() { return Application.isPlaying; }
    }
}
#endif