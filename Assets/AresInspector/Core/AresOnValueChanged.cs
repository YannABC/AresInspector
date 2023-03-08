using System;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class AresOnValueChanged : System.Attribute
    {
        public readonly string method;
        public AresOnValueChanged(string method)
        {
            this.method = method;
        }
    }
}