using System.Diagnostics;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public class AresEnableIf : System.Attribute
    {
        public readonly string condition;
        public AresEnableIf(string condition)
        {
            this.condition = condition;
        }
    }
}
