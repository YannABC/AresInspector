using System;
using System.Drawing;
using UnityEngine.UIElements;

namespace Ares
{
    /// <summary>
    /// 按钮
    /// 可以与各种AC*配合
    /// </summary>
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
        protected override VisualElement CreateCustomGUI(AresContext context)
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
