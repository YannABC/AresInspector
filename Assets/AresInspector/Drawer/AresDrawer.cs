using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    //base class for field decrator or display
    public partial class AresDrawer : System.Attribute
    {
        public bool isDecrator;
        public AresDrawer(bool isDecrator = false)
        {
            this.isDecrator = isDecrator;
        }
    }

#if UNITY_EDITOR
    public partial class AresDrawer
    {
        public AresMember member;
        public virtual VisualElement CreateGUI(AresContext context)
        {
            PropertyField pf = new PropertyField(context.property);
            pf.style.flexGrow = 1;//尽量撑满，1个就100%，两个就各50%...
            return pf;
        }
    }
#endif
}
