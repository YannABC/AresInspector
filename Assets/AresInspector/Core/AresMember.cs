using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
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
        public AresGroup group;// the group  that contains the member

        public ACLabel label => m_Label;

        public FieldInfo fieldInfo;
        public MethodInfo methodInfo;

        protected List<AresDrawer> m_Drawers;
        AresShowIf m_ShowIf;
        AresEnableIf m_EnableIf;
        ACLabel m_Label;

        public MethodInfo onValueChanged => m_OnValueChanged;
        MethodInfo m_OnValueChanged;

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
            GetLabel();
            GetOnValueChanged();
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

        void GetLabel()
        {
            if (IsFieldMember())
            {
                m_Label = fieldInfo.GetCustomAttribute<ACLabel>();
            }
        }

        void GetOnValueChanged()
        {
            if (IsFieldMember())
            {
                AresOnValueChanged aovc = fieldInfo.GetCustomAttribute<AresOnValueChanged>();
                if (aovc != null)
                {
                    m_OnValueChanged = ancestor.GetMethod(aovc.method,
                    BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (m_OnValueChanged == null)
                    {
                        UnityEngine.Debug.LogError(aovc.method + " method not found");
                    }
                }
            }
        }
#endif
    }
}
