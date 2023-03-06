using System;
using UnityEditor;
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
        public override VisualElement CreateGUI(AresContext context)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);

            if (prop.propertyType == SerializedPropertyType.Float)
            {
                var slider = new Slider(prop.displayName, min, max);
                slider.AddToClassList(Slider.alignedFieldUssClassName);
                slider.bindingPath = prop.propertyPath;
                slider.showInputField = true;
                return slider;
            }
            else if (prop.propertyType == SerializedPropertyType.Integer)
            {
                var intSlider = new SliderInt(prop.displayName, (int)min, (int)max);
                intSlider.AddToClassList(SliderInt.alignedFieldUssClassName);
                intSlider.bindingPath = prop.propertyPath;
                intSlider.showInputField = true;
                return intSlider;
            }

            return new Label(L10n.Tr("Use Range with float or int."));
        }
    }
#endif
}
