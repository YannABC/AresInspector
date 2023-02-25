using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEditor;

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
            bool inline = false                    //inline                
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

        public override void OnGUI()
        {
            if (property == null)
            {
                property = serializedObject.FindProperty(fieldInfo.Name);
            }
            SerializedProperty sp = property;

            if (sp == null)
            {
                UnityEngine.Debug.LogError(fieldInfo.Name + " sp == null");
                return;
            }

            bool visible = IsVisible();
            if (!visible) return;

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
                string displayName = label == null ? sp.displayName : label;
                EditorGUILayout.PropertyField(sp, new GUIContent(displayName), includeChildren: true);

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
    }
#endif
}
