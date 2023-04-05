using System;
using System.Diagnostics;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACValueChanged : System.Attribute
    {
        public readonly string method;
        public ACValueChanged(string method)
        {
            this.method = method;
        }
    }
}