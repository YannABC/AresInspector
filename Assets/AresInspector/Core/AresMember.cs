using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
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

        public FieldInfo fieldInfo;
        public PropertyInfo propertyInfo;
        public MethodInfo methodInfo;

        protected List<AresDrawer> m_Drawers;
        ACShowIf m_ShowIf;
        ACEnableIf m_EnableIf;
        ACLabelText m_LabelText;
        ACLabelWidth m_LabelWidth;
        ACLabelColor m_LabelColor;
        ACFontSize m_FontSize;
        ACBgColor m_BackgrondColor;

        public MethodInfo onValueChanged => m_OnValueChanged;
        MethodInfo m_OnValueChanged;

        public ACLabelWidth GetACLabelWidth() { return m_LabelWidth; }
        public ACFontSize GetACFontSize() { return m_FontSize; }
        public ACLabelColor GetACLabelColor() { return m_LabelColor; }
        public ACBgColor GetACBackgroundColor() { return m_BackgrondColor; }

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

            if (IsFieldMember())
            {
                m_ShowIf = fieldInfo.GetCustomAttribute<ACShowIf>();
                m_EnableIf = fieldInfo.GetCustomAttribute<ACEnableIf>();
                m_LabelText = fieldInfo.GetCustomAttribute<ACLabelText>();
                m_LabelWidth = fieldInfo.GetCustomAttribute<ACLabelWidth>();
                m_LabelColor = fieldInfo.GetCustomAttribute<ACLabelColor>();
                m_BackgrondColor = fieldInfo.GetCustomAttribute<ACBgColor>();

                ACValueChanged aovc = fieldInfo.GetCustomAttribute<ACValueChanged>();
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
            else if (IsPropertyMember())
            {
                m_EnableIf = propertyInfo.GetCustomAttribute<ACEnableIf>();
                m_LabelText = propertyInfo.GetCustomAttribute<ACLabelText>();
                m_LabelWidth = propertyInfo.GetCustomAttribute<ACLabelWidth>();
                m_LabelColor = propertyInfo.GetCustomAttribute<ACLabelColor>();
                m_BackgrondColor = propertyInfo.GetCustomAttribute<ACBgColor>();
            }
            else
            {
                m_ShowIf = methodInfo.GetCustomAttribute<ACShowIf>();
                m_EnableIf = methodInfo.GetCustomAttribute<ACEnableIf>();
                m_LabelText = methodInfo.GetCustomAttribute<ACLabelText>();
                m_FontSize = methodInfo.GetCustomAttribute<ACFontSize>();
                m_LabelColor = methodInfo.GetCustomAttribute<ACLabelColor>();
                m_BackgrondColor = methodInfo.GetCustomAttribute<ACBgColor>();
            }
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
        public bool IsPropertyMember() { return propertyInfo != null; }
        public bool IsMethodMember() { return methodInfo != null; }

        void OnDrawnCoun0()
        {
            if (IsFieldMember())
            {
                m_Drawers.Add(new ADField());
            }
            else if (IsMethodMember())
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
            else if (IsPropertyMember())
            {
                return propertyInfo.GetCustomAttributes<AresDrawer>();
            }
            else
            {
                return methodInfo.GetCustomAttributes<AresDrawer>();
            }
        }

        public bool HasAttribute(System.Type attr)
        {
            return GetAttribute(attr) != null;
        }

        public bool HasAttribute<T>() where T : Attribute
        {
            return HasAttribute(typeof(T));
        }

        public System.Attribute GetAttribute(System.Type attr)
        {
            if (IsFieldMember())
            {
                return fieldInfo.GetCustomAttribute(attr);
            }
            else if (IsPropertyMember())
            {
                return propertyInfo.GetCustomAttribute(attr);
            }
            else
            {
                return methodInfo.GetCustomAttribute(attr);
            }
        }

        public T GetAttribute<T>() where T : Attribute
        {
            return GetAttribute(typeof(T)) as T;
        }

        public string GetLabelText(string defaultName)
        {
            if (IsFieldMember() || IsPropertyMember())
            {
                string labelName;
                if (m_LabelText != null)
                {
                    labelName = m_LabelText.text ?? defaultName;
                }
                else
                {
                    labelName = defaultName;
                }
                return labelName;
            }
            else
            {
                return null;
            }
        }

        public string GetButtonText()
        {
            if (m_LabelText != null)
            {
                return string.IsNullOrEmpty(m_LabelText.text) ? methodInfo.Name : m_LabelText.text;
            }
            else
            {
                return methodInfo.Name;
            }
        }
#endif
    }
}
