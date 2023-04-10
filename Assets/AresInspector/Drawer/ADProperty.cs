using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Reflection;

namespace Ares
{
    /// <summary>
    /// unity是不会序列化property的，我们也不想支持property的序列化，因为感觉没必要
    /// ADProperty不是为了序列化的，只是需要显示在编辑器的时候才加
    /// 可以在编辑器中修改，运行时能生效，但不会被序列化
    /// 目前只支持int float long bool，需要其他的自己扩展
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public partial class ADProperty : AresDrawer
    {
        public ADProperty() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class ADProperty
    {
        protected override VisualElement CreateCustomGUI(AresContext context)
        {
            PropertyInfo propertyInfo = member.propertyInfo;

            string labelText = member.GetLabelText(ObjectNames.NicifyVariableName(propertyInfo.Name));

            VisualElement ve;
            if (propertyInfo.PropertyType == typeof(int))
            {
                var field = new IntegerField(labelText);
                field.SetValueWithoutNotify((int)propertyInfo.GetValue(context.target));
                field.RegisterValueChangedCallback((evt) =>
                {
                    propertyInfo.SetValue(context.target, (int)evt.newValue);
                });
                AresHelper.DoUntil(() =>
                {
                    field.SetValueWithoutNotify((int)propertyInfo.GetValue(context.target));
                    return false;
                }, int.MaxValue);
                ve = field;
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                var field = new FloatField(labelText);
                field.SetValueWithoutNotify((float)propertyInfo.GetValue(context.target));
                field.RegisterValueChangedCallback((evt) =>
                {
                    propertyInfo.SetValue(context.target, (float)evt.newValue);
                });
                AresHelper.DoUntil(() =>
                {
                    field.SetValueWithoutNotify((float)propertyInfo.GetValue(context.target));
                    return false;
                }, int.MaxValue);
                ve = field;
            }
            else if (propertyInfo.PropertyType == typeof(long))
            {
                var field = new LongField(labelText);
                field.SetValueWithoutNotify((long)propertyInfo.GetValue(context.target));
                field.RegisterValueChangedCallback((evt) =>
                {
                    propertyInfo.SetValue(context.target, evt.newValue);
                });
                AresHelper.DoUntil(() =>
                {
                    field.SetValueWithoutNotify((long)propertyInfo.GetValue(context.target));
                    return false;
                }, int.MaxValue);
                ve = field;
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                var field = new Toggle(labelText);
                field.SetValueWithoutNotify((bool)propertyInfo.GetValue(context.target));
                field.RegisterValueChangedCallback((evt) =>
                {
                    propertyInfo.SetValue(context.target, evt.newValue);
                });
                AresHelper.DoUntil(() =>
                {
                    field.SetValueWithoutNotify((bool)propertyInfo.GetValue(context.target));
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
                ve = new Label("unknown type " + propertyInfo.PropertyType);
            }

            return ve;
        }

        void SetReadOnly(VisualElement ve)
        {
            if (!member.propertyInfo.CanWrite || member.HasAttribute<ACReadOnly>())
            {
                ve.SetEnabled(false);
            }
        }
    }
#endif
}
