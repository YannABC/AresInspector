using UnityEditor;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
#if UNITY_EDITOR
    public class AresContext
    {
        public object target => m_Target;
        object m_Target;   // MB or SO or serializable class

        SerializedObject m_SerializedObject;
        SerializedProperty m_SerializedProperty;

        public AresContext(SerializedObject serializedObject)
        {
            m_SerializedObject = serializedObject;
            m_Target = serializedObject.targetObject;
        }

        public AresContext(SerializedProperty serializedProperty)
        {
            m_SerializedProperty = serializedProperty;
            m_Target = m_SerializedProperty.GetTargetObjectOfProperty();
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
