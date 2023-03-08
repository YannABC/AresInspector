using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using static PlasticGui.LaunchDiffParameters;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public partial class ADField : AresDrawer
    {
        public ADField() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class ADField
    {
        protected override VisualElement CreateFieldGUI(AresContext context, string labelName, int size)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
            PropertyField pf = new PropertyField(prop, labelName);
            pf.style.flexGrow = 1;

            SetLabelSize(pf, size);
            SetOnValueChanged(pf, context.target);
            return pf;
        }
    }
#endif
}
