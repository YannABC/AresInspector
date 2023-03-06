using UnityEngine.UIElements;

namespace Ares
{
    // Attribute used to make a float or int variable in a script be restricted to a specific range.
    public partial class ADButton : AresDrawer
    {
        public ADButton() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class ADButton
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            Button btn = new Button(() =>
            {
                member.methodInfo.Invoke(context.target, null);
            });
            btn.text = member.methodInfo.Name;
            btn.style.flexGrow = 1;
            return btn;
        }
    }
#endif
}
