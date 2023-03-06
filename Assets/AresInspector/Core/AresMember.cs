using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;

namespace Ares
{
#if UNITY_EDITOR
    public class AresMember
    {
        public int index; // for stable sort
        public int order;
        public int groupId;
        public Type ancestor;//self or current base class
        public AresGroup group;// the group that contains the member

        public FieldInfo fieldInfo;
        public MethodInfo methodInfo;

        protected List<AresDrawer> m_Drawers;

        public void Init()
        {
            IEnumerable<AresDrawer> drawers = null;
            if (IsFieldMember())
            {
                drawers = fieldInfo.GetCustomAttributes<AresDrawer>();
            }
            else
            {
                drawers = methodInfo.GetCustomAttributes<AresDrawer>();
            }

            int drawnCount = drawers.Count(ad => !ad.isDecrator);
            m_Drawers = new List<AresDrawer>(drawers);
            if (drawnCount == 0)
            {
                if (IsFieldMember())
                {
                    m_Drawers.Add(new ADPropertyField());
                }
                else
                {
                    m_Drawers.Add(new ADButton());
                }
            }
            foreach (AresDrawer d in m_Drawers)
            {
                d.member = this;
            }
        }

        public VisualElement CreateGUI(AresContext context)
        {
            AresContext childContext = null;
            if (IsFieldMember())
            {
                childContext = new AresContext(context.FindProperty(fieldInfo.Name), fieldInfo);
            }
            else
            {
                childContext = new AresContext(context.target, methodInfo);
            }
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
                return m_Drawers[0].CreateGUI(childContext);
            }
        }

        public bool IsFieldMember() { return fieldInfo != null; }

        //protected virtual void OnDrawnCoun0() { }
        //protected virtual IEnumerable<AresDrawer> GetDrawers() { return null; }
        //protected virtual AresContext GetChildContext(AresContext context) { return null; }
#endif
    }
}
