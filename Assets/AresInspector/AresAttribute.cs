using System.Diagnostics;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public partial class AresAttribute : System.Attribute
    {
    }

#if UNITY_EDITOR
    public partial class AresAttribute
    {
        public UnityObject target;
        public SerializedObject serializedObject;

        public virtual void OnGUI() { }
    }
#endif
}
