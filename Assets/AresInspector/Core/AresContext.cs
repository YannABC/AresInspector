using System.Reflection;
using UnityEditor;

namespace Ares
{
#if UNITY_EDITOR
    public class AresContext
    {
        public object target { get; private set; }// MB or SO or serializable class
        public string disPlayName
        {
            get
            {
                if (m_SerializedProperty != null)
                {
                    return m_SerializedProperty.displayName;
                }
                else
                {
                    return m_SerializedObject.targetObject.name;
                }
            }
        }

        SerializedObject m_SerializedObject;
        SerializedProperty m_SerializedProperty;

        public AresContext(SerializedObject serializedObject)
        {
            m_SerializedObject = serializedObject;
            target = serializedObject.targetObject;
        }

        public AresContext(SerializedProperty serializedProperty)
        {
            m_SerializedProperty = serializedProperty;
            target = m_SerializedProperty.GetTargetObjectOfProperty();
        }

        public SerializedProperty FindProperty(string name)
        {
            if (m_SerializedProperty != null)
            {
                return m_SerializedProperty.FindPropertyRelative(name);
            }
            else
            {
                return m_SerializedObject.FindProperty(name);
            }
        }
    }
#endif
}
