using UnityEngine.UIElements;

namespace Ares
{
    //base class for all drawers
    public abstract partial class AresDrawer : System.Attribute
    {
        public bool isDecrator;
        public AresDrawer(bool isDecrator = false)
        {
            this.isDecrator = isDecrator;
        }
    }

#if UNITY_EDITOR
    public abstract partial class AresDrawer
    {
        public AresMember member;
        public virtual VisualElement CreateGUI(AresContext context)
        {
            return null;
        }
    }
#endif
}
