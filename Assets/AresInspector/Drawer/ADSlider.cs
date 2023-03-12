using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Ares
{
    // Attribute used to make a float or int variable in a script be restricted to a specific range.
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public partial class ADSlider : AresDrawer
    {
        public readonly float min;
        public readonly float max;

        public ADSlider(float min, float max) : base(false)
        {
            this.min = min;
            this.max = max;
        }
    }

#if UNITY_EDITOR
    public partial class ADSlider
    {
        protected override VisualElement CreateFieldGUI(AresContext context, string labelName, int size)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);

            if (prop.propertyType == SerializedPropertyType.Float)
            {
                Slider slider = new Slider(labelName, min, max);
                SetupSlider(slider, prop.propertyPath, context.target);
                return slider;
            }
            else if (prop.propertyType == SerializedPropertyType.Integer)
            {
                SliderInt slider = new SliderInt(labelName, (int)min, (int)max);
                SetupSlider(slider, prop.propertyPath, context.target);
                return slider;
            }

            return new Label(L10n.Tr("Use Range with float or int."));
        }

        protected void SetupSlider<T>(BaseSlider<T> slider, string path, object target) where T : IComparable<T>
        {
            slider.style.flexGrow = 1;
            slider.bindingPath = path;
            slider.showInputField = true;

            if (member.onValueChanged == null) return;
            slider.RegisterValueChangedCallback((ChangeEvent<T> evt) =>
            {
                member.onValueChanged.Invoke(target, null);
            });
        }
    }
#endif
}
