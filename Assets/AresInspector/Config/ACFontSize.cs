using System;
using System.Diagnostics;

namespace Ares
{
    [System.AttributeUsage(/*AttributeTargets.Field |*/ AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACFontSize : System.Attribute
    {
        public readonly int size;

        public ACFontSize(int size = 12)
        {
            this.size = size;
        }
    }
}