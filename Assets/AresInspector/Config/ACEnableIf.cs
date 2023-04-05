using System.Diagnostics;

namespace Ares
{
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
