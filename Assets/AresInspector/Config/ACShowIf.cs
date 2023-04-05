using System;
using System.Diagnostics;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Conditional("UNITY_EDITOR")]
    public class ACShowIf : System.Attribute
    {
        public readonly string condition;
        public ACShowIf(string condition)
        {
            this.condition = condition;
        }
    }
}