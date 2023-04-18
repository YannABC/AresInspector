using System;
using System.Diagnostics;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public partial class ADRequired : AresDrawer
    {
        public readonly string info;
        public ADRequired(string info) : base(true)
        {
            this.info = info;
        }
    }

#if UNITY_EDITOR
    public partial class ADRequired
    {
        protected override VisualElement CreateCustomGUI(AresContext context)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
            if (prop == null) return null;
            if (prop.propertyType != SerializedPropertyType.ObjectReference) return null;

            HelpBox hb = new HelpBox(info, HelpBoxMessageType.Error);

            AresHelper.DoUntil(() =>
            {
                bool v = prop.objectReferenceValue != null;
                hb.style.display = v ? DisplayStyle.None : DisplayStyle.Flex;
                return false;
            }, int.MaxValue);

            hb.style.display = DisplayStyle.None;

            return hb;
        }
    }
#endif
}