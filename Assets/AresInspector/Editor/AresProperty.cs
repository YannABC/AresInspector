using UnityEditor;
using UnityEngine.UIElements;

namespace Ares
{
    public class AresProperty : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            System.Type type = fieldInfo.FieldType;
            if (type.IsArray)
            {
                type = type.GetElementType();
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

}