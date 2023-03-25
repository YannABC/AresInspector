using System;
using System.Diagnostics;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class AresOnValueChanged : System.Attribute
    {
        public readonly string method;
        public AresOnValueChanged(string method)
        {
            this.method = method;
        }
    }
}