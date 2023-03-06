namespace Ares
{
    public class AresEnableIf : System.Attribute
    {
        public readonly string condition;
        public AresEnableIf(string condition)
        {
            this.condition = condition;
        }
    }
}
