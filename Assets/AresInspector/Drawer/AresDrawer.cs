using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    //base class for field decrator or display
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
