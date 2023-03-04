using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    // Attribute used to make a float or int variable in a script be restricted to a specific range.
    public partial class AresRange : AresDrawer
    {
        public readonly float min;
        public readonly float max;

        public AresRange(float min, float max) : base(false)
        {
            this.min = min;
            this.max = max;
        }
    }

#if UNITY_EDITOR
    public partial class AresRange
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            SerializedProperty property = context.property;

            if (property.propertyType == SerializedPropertyType.Float)
            {
                var slider = new Slider(property.displayName, min, max);
                slider.AddToClassList(Slider.alignedFieldUssClassName);
                slider.bindingPath = property.propertyPath;
                slider.showInputField = true;
                return slider;
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                var intSlider = new SliderInt(property.displayName, (int)min, (int)max);
                intSlider.AddToClassList(SliderInt.alignedFieldUssClassName);
                intSlider.bindingPath = property.propertyPath;
                intSlider.showInputField = true;
                return intSlider;
            }

            return new Label(L10n.Tr("Use Range with float or int."));
        }
    }
#endif
}
