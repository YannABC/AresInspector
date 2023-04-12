using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public partial class ADFlagsEnum : AresDrawer
    {
        public readonly bool hasAllButton;
        public ADFlagsEnum(bool hasAllButton = false) : base(false)
        {
            this.hasAllButton = hasAllButton;
        }
    }

#if UNITY_EDITOR
    public partial class ADFlagsEnum
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

            int cur = (int)fieldInfo.GetValue(context.target);
            int all = 0;
            //Debug.Log("cur:" + cur);

            Type enumType = fieldInfo.FieldType;
            Array enumValues = Enum.GetValues(enumType);
            foreach (var value in enumValues)
            {
                int v = (int)value;
                all |= v;

                ToolbarToggle button = new ToolbarToggle();
                button.RegisterValueChangedCallback((evt) =>
                {
                    if (evt.newValue)
                    {
                        cur |= v;
                        button.value = true;
                    }
                    else
                    {
                        cur &= ~v;
                        button.value = false;
                    }
                    fieldInfo.SetValue(context.target, cur);
                    prop.serializedObject.ApplyModifiedProperties();

                });
                button.text = value.ToString();
                button.style.flexGrow = 1;
                button.style.borderLeftWidth = 0;
                toolbar.Add(button);

                button.SetValueWithoutNotify((cur & v) != 0);
            }
            //All
            if (hasAllButton)
            {
                Button button = new Button();
                button.clicked += () =>
                {
                    if (cur == all)
                    {
                        cur = 0;
                    }
                    else
                    {
                        cur = all;
                    }

                    foreach (var c in toolbar.Children())
                    {
                        ToolbarToggle tt = c as ToolbarToggle;
                        tt.SetValueWithoutNotify(cur != 0);
                    }
                    fieldInfo.SetValue(context.target, cur);
                    prop.serializedObject.ApplyModifiedProperties();
                };
                button.text = "All";
                button.style.flexGrow = 0;
                //button.style.borderLeftWidth = 0;
                button.style.width = 30;
                toolbar.Add(button);
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
