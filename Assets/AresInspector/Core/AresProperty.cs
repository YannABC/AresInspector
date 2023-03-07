using System.Collections.Generic;
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
                return group.CreateGUI(new AresContext(property));
            }
        }
    }

    [CustomPropertyDrawer(typeof(IAresObjectV), true)]
    public class AresPropertyV : AresProperty { }

    [CustomPropertyDrawer(typeof(IAresObjectH), true)]
    public class AresPropertyH : AresProperty { }
#endif
}