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
        AresShowIf m_ShowIf;
        AresEnableIf m_EnableIf;

        public void Init()
        {
            IEnumerable<AresDrawer> drawers = GetDrawers();

            int drawnCount = drawers.Count(ad => !ad.isDecrator);
            m_Drawers = new List<AresDrawer>(drawers);
            if (drawnCount == 0)
            {
                OnDrawnCoun0();
            }
            foreach (AresDrawer d in m_Drawers)
            {
                d.member = this;
            }

            GetShowIf();
            GetEnableIf();
        }

        public VisualElement CreateGUI(AresContext context)
        {
            AresContext childContext = context;

            VisualElement root = null;
            if (m_Drawers.Count > 1)
            {
                root = new VisualElement();
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
            }
            else
            {
                root = m_Drawers[0].CreateGUI(childContext);
            }

            if (m_ShowIf != null)
            {
                AresControls.RegisterShowIf(root, context.target, m_ShowIf.condition);
            }

            if (m_EnableIf != null)
            {
                AresControls.RegisterEnableIf(root, context.target, m_EnableIf.condition);
            }

            return root;
        }

        public bool IsFieldMember() { return fieldInfo != null; }

        void OnDrawnCoun0()
        {
            if (IsFieldMember())
            {
                m_Drawers.Add(new ADField());
            }
            else
            {
                m_Drawers.Add(new ADButton());
            }
        }

        IEnumerable<AresDrawer> GetDrawers()
        {
            if (IsFieldMember())
            {
                return fieldInfo.GetCustomAttributes<AresDrawer>();
            }
            else
            {
                return methodInfo.GetCustomAttributes<AresDrawer>();
            }
        }

        //AresContext GetChildContext(AresContext context)
        //{
        //    if (IsFieldMember())
        //    {
        //        return new AresContext(context.FindProperty(fieldInfo.Name), fieldInfo);
        //    }
        //    else
        //    {
        //        return new AresContext(context.target, methodInfo);
        //    }
        //}

        void GetShowIf()
        {
            if (IsFieldMember())
            {
                m_ShowIf = fieldInfo.GetCustomAttribute<AresShowIf>();
            }
            else
            {
                m_ShowIf = methodInfo.GetCustomAttribute<AresShowIf>();
            }
        }
        void GetEnableIf()
        {
            if (IsFieldMember())
            {
                m_EnableIf = fieldInfo.GetCustomAttribute<AresEnableIf>();
            }
            else
            {
                m_EnableIf = methodInfo.GetCustomAttribute<AresEnableIf>();
            }
        }
#endif
    }
}
