using System.Reflection;
using UnityEditor;

namespace Ares
{
#if UNITY_EDITOR
    public class AresContext
    {
        public object target { get; private set; }// MB or SO or serializable class
        public object fieldValue { get; private set; }// MB or SO or serializable class

        public SerializedProperty property { get; private set; }
        public FieldInfo fieldInfo { get; private set; }
        public MethodInfo methodInfo { get; private set; }

        SerializedObject m_SerializedObject;

        public AresContext(SerializedObject serializedObject)
        {
            m_SerializedObject = serializedObject;
            target = serializedObject.targetObject;
            fieldValue = null;
        }

        public AresContext(SerializedProperty serializedProperty, FieldInfo fieldInfo)
        {
            property = serializedProperty;
            this.fieldInfo = fieldInfo;
            target = property.GetTargetObjectOfProperty();
        }

        public AresContext(object target, MethodInfo methodInfo)
        {
            this.target = target;
            this.methodInfo = methodInfo;
        }

        public SerializedProperty FindProperty(string name)
        {
            if (property != null)
            {
                return property.FindPropertyRelative(name);
            }
            else
            {
                return m_SerializedObject.FindProperty(name);
            }
        }
    }
#endif
}
