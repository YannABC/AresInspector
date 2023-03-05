using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;

namespace Ares
{
    //[Conditional("UNITY_EDITOR")]
    //public partial class AresMethod : AresMember
    //{
    //    //{{
    //    public EAresButtonSize buttonSize;
    //    //}}

    //    public AresMethod(
    //        EAresButtonSize size = EAresButtonSize.Small     // 大小

    //        ) : base()
    //    {
    //        this.buttonSize = size;
    //    }
    //}

    //public enum EAresButtonSize
    //{
    //    Small = 20,
    //    Medium = 40,
    //    Big = 60,
    //}

#if UNITY_EDITOR
    public class AresMethod : AresMember
    {
        public MethodInfo methodInfo;
        List<AresDrawer> m_Drawers;

        public override void Init()
        {
            IEnumerable<AresDrawer> drawers = methodInfo.GetCustomAttributes<AresDrawer>();
            int drawnCount = drawers.Count(ad => !ad.isDecrator);
            m_Drawers = new List<AresDrawer>(drawers);
            if (drawnCount == 0)
            {
                m_Drawers.Add(new AresButton());
            }
            foreach (AresDrawer d in m_Drawers)
            {
                d.member = this;
            }
        }

        public override VisualElement CreateGUI(AresContext context)
        {
            AresContext childContext = new AresContext(context.target, methodInfo);

            if (m_Drawers.Count > 1)
            {
                VisualElement root = new VisualElement();
                root.style.flexDirection = group.type == EAresGroupType.Horizontal ? FlexDirection.Row : FlexDirection.Column;
                root.style.flexGrow = 1;
                foreach (AresDrawer drawer in m_Drawers)
                {
                    VisualElement child = drawer.CreateGUI(childContext);
                    if (child != null)
                    {
                        root.Add(child);
                    }
                }
                return root;
            }
            else
            {
                return m_Drawers[0].CreateGUI(childContext);//AresButton
            }
        }
    }
#endif
}
