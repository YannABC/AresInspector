using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public partial class AresField : AresMember
    {
        //{{
        public string label;
        public int labelSize;
        public bool inline;
        //}}

        public AresField(
            bool showLabel = true,                  //show label
            string label = null,                    //label text
            int labelSize = 0,                      //label size
            bool inline = false                     //inline                
            ) : base()
        {
            this.label = label;
            this.labelSize = labelSize;
            this.inline = inline;

            //AresLabel?
            //AresDropDown?
        }
    }

#if UNITY_EDITOR
    public partial class AresField
    {
        public FieldInfo fieldInfo;
        public SerializedProperty property;

        AresGroup m_Group;//内嵌类

        public override void OnGUI()
        {
            bool visible = IsVisible();
            if (!visible) return;

            if (m_Group != null)
            {
                //int indent = EditorGUI.indentLevel;
                //EditorGUI.indentLevel++;
                m_Group.OnGUI();
                //EditorGUI.indentLevel = indent;
                return;
            }

            // Validate
            //ValidatorAttribute[] validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
            //foreach (var validatorAttribute in validatorAttributes)
            //{
            //    validatorAttribute.GetValidator().ValidateProperty(property);
            //}

            // Check if enabled and draw
            EditorGUI.BeginChangeCheck();
            bool enabled = true;

            using (new EditorGUI.DisabledScope(disabled: !enabled))
            {
                string displayName = label == null ? property.displayName : label;
                EditorGUILayout.PropertyField(property, new GUIContent(displayName), includeChildren: true);

                //Rect rect = EditorGUILayout.GetControlRect();

                //int index = EditorGUI.Popup(rect, 1, new string[] { "1", "2", "3" });


                //EditorGUILayout.PropertyField(sp, GUIContent.none, includeChildren: true);

                //EditorGUI.IntSlider(EditorGUILayout.GetControlRect(), sp, 2, 10, label);

            }

            // Call OnValueChanged callbacks
            if (EditorGUI.EndChangeCheck())
            {

            }
        }

        public override void Init()
        {
            System.Type type = fieldInfo.FieldType;

            if (type.IsClass)
            {
                object obj = property.GetTargetObjectOfProperty();
                if (obj == null)
                {
                    // array  can be null
                    return;
                }

                m_Group = obj.GetType().GetCustomAttributes<AresGroup>().
                    Where(ag => ag.id == 0).FirstOrDefault();
                if (m_Group == null) return;

                m_Group.Init(obj, property);
            }
        }
    }
#endif
}
