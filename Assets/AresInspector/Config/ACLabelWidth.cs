using System;
using System.Diagnostics;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACLabelWidth : System.Attribute
    {
        public readonly int width;       // if 0, auto size

        public ACLabelWidth(int width = 120)
        {
            this.width = width;
        }
    }
}