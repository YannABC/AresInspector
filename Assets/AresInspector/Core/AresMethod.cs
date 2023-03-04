using System.Collections;
using System.Diagnostics;
using System.Reflection;
using UnityEngine.UIElements;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public partial class AresMethod : AresMember
    {
        //{{
        public EAresButtonSize buttonSize;
        //}}

        public AresMethod(
            EAresButtonSize size = EAresButtonSize.Small     // 大小

            ) : base()
        {
            this.buttonSize = size;
        }
    }

    public enum EAresButtonSize
    {
        Small = 20,
        Medium = 40,
        Big = 60,
    }

#if UNITY_EDITOR
    public partial class AresMethod
    {
        public MethodInfo methodInfo;

        public override VisualElement CreateGUI(AresContext context)
        {
            Button btn = new Button(() =>
            {
                methodInfo.Invoke(context.target, null);
            });
            btn.text = methodInfo.Name;
            btn.style.flexGrow = 1;
            return btn;
        }
    }
#endif
}
