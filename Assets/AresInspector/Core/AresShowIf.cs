namespace Ares
{
    public class AresShowIf : System.Attribute
    {
        public readonly string name;
        public AresShowIf(string name)
        {
            this.name = name;
        }
    }
}