using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

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
            string label = null,                    //label text  (null use property, empty not show)
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
        //public SerializedProperty property;

        //public override void OnGUI()
        //{
        //    bool visible = IsVisible();
        //    if (!visible) return;

        //    //if (m_Group != null)
        //    //{
        //    //    //int indent = EditorGUI.indentLevel;
        //    //    //EditorGUI.indentLevel++;
        //    //    m_Group.OnGUI();
        //    //    //EditorGUI.indentLevel = indent;
        //    //    return;
        //    //}

        //    // Validate
        //    //ValidatorAttribute[] validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
        //    //foreach (var validatorAttribute in validatorAttributes)
        //    //{
        //    //    validatorAttribute.GetValidator().ValidateProperty(property);
        //    //}

        //    // Check if enabled and draw
        //    EditorGUI.BeginChangeCheck();
        //    bool enabled = true;

        //    using (new EditorGUI.DisabledScope(disabled: !enabled))
        //    {
        //        string displayName = label == null ? property.displayName : label;
        //        EditorGUILayout.PropertyField(property, new GUIContent(displayName), includeChildren: true);

        //        //Rect rect = EditorGUILayout.GetControlRect();

        //        //int index = EditorGUI.Popup(rect, 1, new string[] { "1", "2", "3" });


        //        //EditorGUILayout.PropertyField(sp, GUIContent.none, includeChildren: true);

        //        //EditorGUI.IntSlider(EditorGUILayout.GetControlRect(), sp, 2, 10, label);

        //    }

        //    // Call OnValueChanged callbacks
        //    if (EditorGUI.EndChangeCheck())
        //    {

        //    }
        //}

        public override VisualElement CreateUI(AresContext context)
        {
            PropertyField pf = new PropertyField(context.FindProperty(fieldInfo.Name));
            return pf;
        }
    }
#endif
}
