using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Reflection;

namespace Ares
{
    /// <summary>
    /// 没有序列化，又想显示在编辑器中时使用
    /// 如私有field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public partial class ADUnserializedField : AresDrawer
    {
        public ADUnserializedField() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class ADUnserializedField
    {
        protected override VisualElement CreateCustomGUI(AresContext context)
        {
            FieldInfo fieldInfo = member.fieldInfo;

            string labelText = member.GetLabelText(ObjectNames.NicifyVariableName(fieldInfo.Name));

            VisualElement ve;
            if (fieldInfo.FieldType == typeof(int))
            {
                var field = new IntegerField(labelText);
                field.SetValueWithoutNotify((int)fieldInfo.GetValue(context.target));
                field.RegisterValueChangedCallback((evt) =>
                {
                    fieldInfo.SetValue(context.target, (int)evt.newValue);
                });
                AresHelper.DoUntil(() =>
                {
                    field.SetValueWithoutNotify((int)fieldInfo.GetValue(context.target));
                    return false;
                }, int.MaxValue);
                ve = field;
            }
            else if (fieldInfo.FieldType == typeof(float))
            {
                var field = new FloatField(labelText);
                field.SetValueWithoutNotify((float)fieldInfo.GetValue(context.target));
                field.RegisterValueChangedCallback((evt) =>
                {
                    fieldInfo.SetValue(context.target, (float)evt.newValue);
                });
                AresHelper.DoUntil(() =>
                {
                    field.SetValueWithoutNotify((float)fieldInfo.GetValue(context.target));
                    return false;
                }, int.MaxValue);
                ve = field;
            }
            else if (fieldInfo.FieldType == typeof(long))
            {
                var field = new LongField(labelText);
                field.SetValueWithoutNotify((long)fieldInfo.GetValue(context.target));
                field.RegisterValueChangedCallback((evt) =>
                {
                    fieldInfo.SetValue(context.target, evt.newValue);
                });
                AresHelper.DoUntil(() =>
                {
                    field.SetValueWithoutNotify((long)fieldInfo.GetValue(context.target));
                    return false;
                }, int.MaxValue);
                ve = field;
            }
            else if (fieldInfo.FieldType == typeof(bool))
            {
                var field = new Toggle(labelText);
                field.SetValueWithoutNotify((bool)fieldInfo.GetValue(context.target));
                field.RegisterValueChangedCallback((evt) =>
                {
                    fieldInfo.SetValue(context.target, evt.newValue);
                });
                AresHelper.DoUntil(() =>
                {
                    field.SetValueWithoutNotify((bool)fieldInfo.GetValue(context.target));
                    return false;
                }, int.MaxValue);
                ve = field;
            }
            else
            {
                ve = null;
            }

            if (ve != null)
            {
                SetReadOnly(ve);
            }
            else
            {
                ve = new Label("unknown type " + fieldInfo.FieldType);
            }

            return ve;
        }

        void SetReadOnly(VisualElement ve)
        {
            if (!member.HasAttribute<ACReadOnly>()) return;
            ve.SetEnabled(false);
        }
    }
#endif
}
