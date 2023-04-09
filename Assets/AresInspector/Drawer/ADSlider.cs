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
        protected override VisualElement CreateCustomGUI(AresContext context)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
            string labelText = member.GetLabelText(prop.displayName);

            if (prop.propertyType == SerializedPropertyType.Float)
            {
                Slider slider = new Slider(labelText, min, max);
                SetupSlider(slider, prop, context.target);
                return slider;
            }
            else if (prop.propertyType == SerializedPropertyType.Integer)
            {
                SliderInt slider = new SliderInt(labelText, (int)min, (int)max);
                SetupSlider(slider, prop, context.target);
                return slider;
            }

            return new Label(L10n.Tr("Use Range with float or int."));
        }

        protected void SetupSlider<T>(BaseSlider<T> slider, SerializedProperty prop, object target) where T : IComparable<T>
        {
            slider.style.flexGrow = 1;
            slider.showInputField = true;

            slider.bindingPath = prop.propertyPath;
            slider.Bind(prop.serializedObject);

            SetDelayed(slider);

            if (member.onValueChanged != null)
            {
                slider.RegisterValueChangedCallback((ChangeEvent<T> evt) =>
                {
                    member.onValueChanged.Invoke(target, null);
                });
            }
        }

        void SetDelayed<T>(BaseSlider<T> slider) where T : IComparable<T>
        {
            if (!member.HasAttribute<ACDelayed>()) return;
            AresHelper.DoUntil(() =>
            {
                TextField tf = slider.Q<TextField>();
                if (tf != null)
                {
                    tf.isDelayed = true;
                    return true;
                }
                return false;
            });
        }
    }
#endif
}
