using System;
using UnityEngine.UIElements;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public partial class ADButton : AresDrawer
    {
        public ADButton() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class ADButton
    {
        protected override VisualElement CreateMethodGUI(AresContext context)
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
