using System.Diagnostics;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public partial class AresAttribute : System.Attribute
    {
    }

#if UNITY_EDITOR
    public partial class AresAttribute
    {
        public UnityEngine.Object target;
        public UnityEditor.SerializedObject serializedObject;

        public virtual void OnGUI() { }
    }
#endif
}
