using System;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Ares
{
    // Attribute used to make a float or int variable in a script be restricted to a specific range.
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public partial class ADRange : AresDrawer
    {
        public readonly float min;
        public readonly float max;

        public ADRange(float min, float max) : base(false)
        {
            this.min = min;
            this.max = max;
        }
    }

#if UNITY_EDITOR
    public partial class ADRange
    {
        protected override VisualElement CreateFieldGUI(AresContext context, string labelName, int size)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);

            if (prop.propertyType == SerializedPropertyType.Float)
            {
                var slider = new UnityEngine.UIElements.Slider(labelName, min, max);
                slider.style.flexGrow = 1;
                slider.bindingPath = prop.propertyPath;
                slider.showInputField = true;
                SetOnValueChanged(slider, context.target);
                return slider;
            }
            else if (prop.propertyType == SerializedPropertyType.Integer)
            {
                var slider = new SliderInt(labelName, (int)min, (int)max);
                slider.style.flexGrow = 1;
                slider.bindingPath = prop.propertyPath;
                slider.showInputField = true;
                SetOnValueChanged(slider, context.target);
                //slider.RegisterValueChangedCallback
                return slider;
            }

            return new Label(L10n.Tr("Use Range with float or int."));
        }
    }
#endif
}
