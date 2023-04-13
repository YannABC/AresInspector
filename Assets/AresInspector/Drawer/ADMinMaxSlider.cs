using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public partial class ADMinMaxSlider : AresDrawer
    {
        public readonly float min;
        public readonly float max;

        public ADMinMaxSlider(float min, float max) : base(false)
        {
            this.min = min;
            this.max = max;
        }

        public ADMinMaxSlider(int min, int max) : base(false)
        {
            this.min = min;
            this.max = max;
        }
    }

#if UNITY_EDITOR
    public partial class ADMinMaxSlider
    {
        protected override VisualElement CreateCustomGUI(AresContext context)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
            string labelText = member.GetLabelText(prop.displayName);

            var ve = new VisualElement();
            ve.style.flexGrow = 1;
            ve.style.flexDirection = FlexDirection.Row;
            ve.Add(new Label(labelText));

            if (prop.propertyType == SerializedPropertyType.Vector2)
            {
                Vector2 cur = prop.vector2Value;

                int width = 50;
                var f1 = new FloatField(null, 7);
                f1.style.width = width;
                f1.isDelayed = true;
                f1.value = cur.x;
                f1.RegisterValueChangedCallback((evt) =>
                {
                    Vector2 v = prop.vector2Value;
                    v.x = evt.newValue;
                    prop.vector2Value = v;
                    prop.serializedObject.ApplyModifiedProperties();
                });
                ve.Add(f1);

                MinMaxSlider slider = new MinMaxSlider();
                slider.highLimit = max;
                slider.lowLimit = min;
                slider.maxValue = cur.y;
                slider.minValue = cur.x;
                SetupSlider(slider, prop, context.target);

                ve.Add(slider);

                var f2 = new FloatField(null, 7);
                f2.style.width = width;
                f2.isDelayed = true;
                f2.RegisterValueChangedCallback((evt) =>
                {
                    Vector2 v = prop.vector2Value;
                    v.y = evt.newValue;
                    prop.vector2Value = v;
                    prop.serializedObject.ApplyModifiedProperties();

                });
                ve.Add(f2);

                slider.RegisterValueChangedCallback((evt) =>
                {
                    Vector2 v = evt.newValue;
                    f1.SetValueWithoutNotify(v.x);
                    f2.SetValueWithoutNotify(v.y);
                    prop.vector2Value = v;
                    prop.serializedObject.ApplyModifiedProperties();
                });
            }
            else if (prop.propertyType == SerializedPropertyType.Vector2Int)
            {
                Vector2Int cur = prop.vector2IntValue;

                int width = 50;
                var f1 = new IntegerField(null, 7);
                f1.style.width = width;
                f1.isDelayed = true;
                f1.value = cur.x;
                f1.RegisterValueChangedCallback((evt) =>
                {
                    Vector2Int v = prop.vector2IntValue;
                    v.x = evt.newValue;
                    prop.vector2IntValue = v;
                    prop.serializedObject.ApplyModifiedProperties();
                });
                ve.Add(f1);

                MinMaxSlider slider = new MinMaxSlider();
                slider.highLimit = max;
                slider.lowLimit = min;
                slider.maxValue = cur.y;
                slider.minValue = cur.x;
                SetupSlider(slider, prop, context.target);

                ve.Add(slider);

                var f2 = new IntegerField(null, 7);
                f2.style.width = width;
                f2.isDelayed = true;
                f2.value = cur.y;
                f2.RegisterValueChangedCallback((evt) =>
                {
                    Vector2Int v = prop.vector2IntValue;
                    v.y = evt.newValue;
                    prop.vector2IntValue = v;
                    prop.serializedObject.ApplyModifiedProperties();

                });
                ve.Add(f2);

                slider.RegisterValueChangedCallback((evt) =>
                {
                    Vector2 v = evt.newValue;

                    Vector2Int newV = new Vector2Int((int)v.x, (int)v.y);
                    slider.SetValueWithoutNotify(new Vector2(newV.x, newV.y));
                    f1.SetValueWithoutNotify(newV.x);
                    f2.SetValueWithoutNotify(newV.y);
                    prop.vector2IntValue = newV;
                    prop.serializedObject.ApplyModifiedProperties();
                });
            }

            return ve;
        }

        protected void SetupSlider(MinMaxSlider slider, SerializedProperty prop, object target)
        {
            slider.style.flexGrow = 1;
            //slider.bindingPath = prop.propertyPath;
            //slider.Bind(prop.serializedObject);

            if (member.onValueChanged != null)
            {
                slider.RegisterValueChangedCallback((evt) =>
                {
                    member.onValueChanged.Invoke(target, null);
                });
            }
        }
    }
#endif
}
