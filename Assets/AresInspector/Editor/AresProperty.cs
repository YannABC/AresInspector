using Ares;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[CustomPropertyDrawer(typeof(IAresObjectV), true)]
public class AresPropertyV : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        //Debug.LogWarning("CreatePropertyGUI V " + fieldInfo.FieldType.Name);
        return AresHelper.GetGroup(fieldInfo.FieldType).CreateGUI(new AresContext(property));
    }
}

[CustomPropertyDrawer(typeof(IAresObjectH), true)]
public class AresPropertyH : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        //Debug.LogWarning("CreatePropertyGUI H " + fieldInfo.FieldType.Name);
        return AresHelper.GetGroup(fieldInfo.FieldType).CreateGUI(new AresContext(property));
    }
}