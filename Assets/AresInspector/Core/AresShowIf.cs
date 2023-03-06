using System;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AresShowIf : System.Attribute
    {
        public readonly string name;
        public AresShowIf(string name)
        {
            this.name = name;
        }
    }
}