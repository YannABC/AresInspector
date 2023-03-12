using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace Ares
{
#if UNITY_EDITOR
    public class AresProperty : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            System.Type type = fieldInfo.FieldType;
            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                type = type.GetGenericArguments()[0];
            }
            AresGroup group = AresGroup.Get(type);
            if (group == null)
            {
                return null;
            }
            else
            {
                VisualElement ve = group.CreateGUI(new AresContext(property));
                if (ve == null) return ve;
                if (fieldInfo.GetCustomAttribute<ACInline>() == null)
                {
                    Foldout foldout = new Foldout();
                    foldout.text = property.displayName;
                    foldout.Add(ve);
                    return foldout;
                }
                else
                {
                    return ve;
                }
            }
        }
    }

    [CustomPropertyDrawer(typeof(IAresObjectV), true)]
    public class AresPropertyV : AresProperty { }

    [CustomPropertyDrawer(typeof(IAresObjectH), true)]
    public class AresPropertyH : AresProperty { }
#endif
}