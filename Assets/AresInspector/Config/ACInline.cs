using System;
using System.Diagnostics;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACInline : System.Attribute
    {

    }
}