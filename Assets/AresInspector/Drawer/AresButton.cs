using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    // Attribute used to make a float or int variable in a script be restricted to a specific range.
    public partial class AresButton : AresDrawer
    {
        public AresButton() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class AresButton
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            AresMethod am = member as AresMethod;
            Button btn = new Button(() =>
            {
                am.methodInfo.Invoke(context.target, null);
            });
            btn.text = am.methodInfo.Name;
            btn.style.flexGrow = 1;
            return btn;
        }
    }
#endif
}
