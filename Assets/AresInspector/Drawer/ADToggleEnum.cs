using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public partial class ADToggleEnum : AresDrawer
    {
        public ADToggleEnum() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class ADToggleEnum
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
            string name = ObjectNames.NicifyVariableName(member.fieldInfo.Name);
            string labelText = member.GetLabelText(name);

            FieldInfo fieldInfo = member.fieldInfo;

            VisualElement ve = new VisualElement();
            ve.style.flexDirection = FlexDirection.Row;
            ve.style.flexGrow = 1;

            Label label = new Label(labelText);
            SetLabel(label);
            ve.Add(label);

            var toolbar = new Toolbar();
            toolbar.style.flexGrow = 1;
            toolbar.style.marginLeft = 0;
            toolbar.style.borderLeftWidth = 1;
            toolbar.style.paddingLeft = 3;
            toolbar.style.paddingRight = 3;
            toolbar.style.borderRightWidth = 1;
            toolbar.style.marginRight = 0;
            ve.Add(toolbar);

            string cur = fieldInfo.GetValue(context.target).ToString();
            //Debug.Log("cur:" + cur);

            Type enumType = fieldInfo.FieldType;
            Array enumValues = Enum.GetValues(enumType);
            foreach (var value in enumValues)
            {
                ToolbarToggle button = new ToolbarToggle();
                button.RegisterValueChangedCallback((evt) =>
                {
                    if (evt.newValue)
                    {
                        foreach (var c in toolbar.Children())
                        {
                            ToolbarToggle tt = c as ToolbarToggle;
                            tt.SetValueWithoutNotify(tt == button);
                        }
                        //how to support undo?
                        //Undo.RecordObject(prop.serializedObject.targetObject, "set enum");
                        fieldInfo.SetValue(context.target, value);
                        //prop.serializedObject.Update();
                        //prop.intValue = (int)value;
                        prop.serializedObject.ApplyModifiedProperties();
                        //Undo.FlushUndoRecordObjects();
                    }
                    else
                    {
                        button.value = true;
                    }

                });
                button.text = value.ToString();
                button.style.flexGrow = 1;
                button.style.borderLeftWidth = 0;
                toolbar.Add(button);

                button.SetValueWithoutNotify(cur == value.ToString());
            }
            return ve;
        }

        protected void SetLabel(Label lbl)
        {
            int width = 120;
            ACLabelWidth ac = member.GetACLabelWidth();
            if (ac != null)
            {
                width = ac.width;
            }
            if (width > 0)
            {
                lbl.style.minWidth = width;
                lbl.style.width = width;
            }
            else
            {
                lbl.style.minWidth = StyleKeyword.Auto;
                lbl.style.width = StyleKeyword.Auto;
            }
            lbl.style.marginLeft = 3;
            lbl.style.marginRight = 2;
            //lbl.style.height = 18;
            lbl.style.paddingTop = 2;
        }
    }
#endif
}
