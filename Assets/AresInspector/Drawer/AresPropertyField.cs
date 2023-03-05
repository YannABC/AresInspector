using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    // Attribute used to make a float or int variable in a script be restricted to a specific range.
    public partial class AresPropertyField : AresDrawer
    {
        public AresPropertyField() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class AresPropertyField
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            PropertyField pf = new PropertyField(context.property);
            pf.style.flexGrow = 1;//尽量撑满，1个就100%，两个就各50%...
            return pf;
        }
    }
#endif
}
