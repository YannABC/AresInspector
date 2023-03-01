using Ares;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class AresProperty : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        System.Type type = fieldInfo.FieldType;
        if (type.IsArray)
        {
            type = type.GetElementType();
        }
        AresGroup group = AresHelper.GetGroup(type);
        if (group == null)
        {
            return null;
        }
        else
        {
            return group.CreateGUI(new AresContext(property));
        }
    }
}

[CustomPropertyDrawer(typeof(IAresObjectV), true)]
public class AresPropertyV : AresProperty { }

[CustomPropertyDrawer(typeof(IAresObjectH), true)]
public class AresPropertyH : AresProperty { }