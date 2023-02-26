using System.Diagnostics;
using UnityEditor;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public partial class AresAttribute : System.Attribute
    {
    }

#if UNITY_EDITOR
    public partial class AresAttribute
    {
        public object target;   // MB or SO or serializable class
        //public SerializedObject serializedObject;

        public virtual void OnGUI() { }
    }
#endif

    //public interface IAresObject { }
}
