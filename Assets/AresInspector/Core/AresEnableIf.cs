namespace Ares
{
    public class AresEnableIf : System.Attribute
    {
        public readonly string name;
        public AresEnableIf(string name)
        {
            this.name = name;
        }
    }
}
