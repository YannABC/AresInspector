using System;
using System.Diagnostics;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACEnableIf : System.Attribute
    {
        public readonly string condition;
        public ACEnableIf(string condition)
        {
            this.condition = condition;
        }
    }
}
