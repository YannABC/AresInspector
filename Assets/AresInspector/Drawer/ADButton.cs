using System;
using System.Drawing;
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

            btn.style.flexGrow = 1;
            btn.text = member.GetLabelText();
            SetBtnSize(btn);

            return btn;
        }

        void SetBtnSize(Button btn)
        {
            ACFontSize ac = member.GetACFontSize();
            if (ac == null) return;
            btn.style.fontSize = ac.size;
        }
    }
#endif
}
