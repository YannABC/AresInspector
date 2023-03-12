using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using static Codice.Client.BaseCommands.WkStatus.Printers.StatusChangeInfo;

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

            //IntegerField f = null;
            //f.isDelayed = true;


            SetLabelSize(pf, size);
            SetDelayed(prop, pf);
            SetOnValueChanged(pf, context.target);
            return pf;
        }

        protected void SetOnValueChanged(PropertyField pf, object target)
        {
            if (member.onValueChanged == null) return;
            pf.RegisterCallback((SerializedPropertyChangeEvent evt) =>
            {
                member.onValueChanged.Invoke(target, null);
            });
        }

        void SetDelayed(SerializedProperty property, PropertyField pf)
        {
            if (!member.HasAttribute<ACDelayed>()) return;
            pf.RegisterCallback<AttachToPanelEvent>(e =>
            {
                if (property.propertyType == SerializedPropertyType.Float)
                {
                    if (property.type == "float")
                    {
                        var f = pf.Q<FloatField>();
                        if (f != null) f.isDelayed = true;
                    }
                    else if (property.type == "double")
                    {
                        var f = pf.Q<DoubleField>();
                        if (f != null) f.isDelayed = true;
                    }
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    if (property.type == "int")
                    {
                        var f = pf.Q<IntegerField>();
                        if (f != null) f.isDelayed = true;
                    }
                    else if (property.type == "long")
                    {
                        var f = pf.Q<LongField>();
                        if (f != null) f.isDelayed = true;
                    }
                }
                else if (property.propertyType == SerializedPropertyType.String)
                {
                    var f = pf.Q<TextField>();
                    if (f != null) f.isDelayed = true;
                }
            });
        }
    }
#endif
}
