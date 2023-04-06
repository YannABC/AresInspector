using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Ares
{
    /// <summary>
    /// Field 默认 drawer, 不需要手动添加
    /// </summary>
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
        protected override VisualElement CreateCustomGUI(AresContext context)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
            string labelText = member.GetLabelText(prop);
            PropertyField pf = new PropertyField(prop, labelText);
            pf.style.flexGrow = 1;

            pf.Bind(prop.serializedObject);//需要Bind，否则在TreeView中自己创建的，不显示IntegerField等

            SetAssetsOnly(pf);
            SetDelayed(prop, pf);
            SetOnValueChanged(pf, context.target);
            return pf;
        }

        protected void SetOnValueChanged(PropertyField pf, object target)
        {
            if (member.onValueChanged != null)
            {
                pf.RegisterCallback((SerializedPropertyChangeEvent evt) =>
                {
                    member.onValueChanged.Invoke(target, null);
                });
            }
        }

        void SetAssetsOnly(PropertyField pf)
        {
            if (!member.HasAttribute<ACAssetsOnly>()) return;

            AresHelper.DoUntil(() =>
            {
                var f = pf.Q<ObjectField>();
                if (f != null)
                {
                    f.allowSceneObjects = false;
                    return true;
                }
                return false;
            });
        }

        void SetDelayed(SerializedProperty property, PropertyField pf)
        {
            if (!member.HasAttribute<ACDelayed>()) return;
            AresHelper.DoUntil(() =>
            {
                if (property.propertyType == SerializedPropertyType.Float)
                {
                    if (property.type == "float")
                    {
                        var f = pf.Q<FloatField>();
                        if (f != null)
                        {
                            f.isDelayed = true;
                            return true;
                        }
                    }
                    else if (property.type == "double")
                    {
                        var f = pf.Q<DoubleField>();
                        if (f != null)
                        {
                            f.isDelayed = true;
                            return true;
                        }
                    }
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    if (property.type == "int")
                    {
                        var f = pf.Q<IntegerField>();
                        if (f != null)
                        {
                            f.isDelayed = true;
                            return true;
                        }
                    }
                    else if (property.type == "long")
                    {
                        var f = pf.Q<LongField>();
                        if (f != null)
                        {
                            f.isDelayed = true;
                            return true;
                        }
                    }
                }
                else if (property.propertyType == SerializedPropertyType.String)
                {
                    var f = pf.Q<TextField>();
                    if (f != null)
                    {
                        f.isDelayed = true;
                        return true;
                    }
                }
                return false;
            });
        }
    }
#endif
}
