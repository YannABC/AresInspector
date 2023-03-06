using UnityEngine.UIElements;

namespace Ares
{
    public class AresValidateIf : AresDrawer
    {
        public readonly string name;
        public readonly string info;
        public AresValidateIf(string name, string info)
        {
            this.name = name;
            this.info = info;
        }

        public override VisualElement CreateGUI(AresContext context)
        {
            return null;
        }
    }
}