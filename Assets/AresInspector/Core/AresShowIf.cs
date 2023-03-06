using System;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AresShowIf : System.Attribute
    {
        public readonly string condition;
        public AresShowIf(string condition)
        {
            this.condition = condition;
        }
    }
}