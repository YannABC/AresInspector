using System;
using System.Diagnostics;
using UnityEditor;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACInline : System.Attribute
    {

    }
}