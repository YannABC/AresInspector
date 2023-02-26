﻿//using Ares;
//using UnityEditor;
//using UnityEngine;

//[CustomPropertyDrawer(typeof(object), true)]
//public class AresCustomDrawer : PropertyDrawer
//{
//    // Draw the property inside the given rect
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        object target = property.GetTargetObjectOfProperty();
//        System.Type type = target.GetType();
//        bool hasAres = ReflectionUtility.HasAres(type);
//        if (hasAres)
//        {
//            AresGroup g = new AresGroup(0, 0, EAresGroupType.Vertical) { target = target };
//            g.Init(property);

//            EditorGUI.BeginProperty(position, label, property);
//            g.OnGUI();
//            EditorGUI.EndProperty();
//        }
//        else
//        {
//            base.OnGUI(position, property, label);
//        }
//        //property.
//        //if (GUILayout.Button("aaaa"))
//        //{
//        //    Debug.Log("aaaa");
//        //}
//        //Debug.LogWarning("++++++++++++++++");
//        //base.OnGUI(position, property, label);
//        // Using BeginProperty / EndProperty on the parent property means that
//        // prefab override logic works on the entire property.
//        //EditorGUI.BeginProperty(position, label, property);

//        //// Draw label
//        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//        //// Don't make child fields be indented
//        //var indent = EditorGUI.indentLevel;
//        //EditorGUI.indentLevel = 0;

//        //// Calculate rects
//        //var amountRect = new Rect(position.x, position.y, 30, position.height);
//        //var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
//        //var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

//        //// Draw fields - pass GUIContent.none to each so they are drawn without labels
//        //EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
//        //EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
//        //EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

//        //// Set indent back to what it was
//        //EditorGUI.indentLevel = indent;

//        //EditorGUI.EndProperty();
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return EditorGUIUtility.singleLineHeight * 2;
//    }
//}