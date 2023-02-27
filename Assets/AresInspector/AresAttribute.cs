using System.Diagnostics;
using UnityEditor;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    //register update
    public interface IAresObject { }
    public interface IAresObjectV : IAresObject { }  //Vertical
    public interface IAresObjectH : IAresObject { }  //Horizontal


    [Conditional("UNITY_EDITOR")]
    public partial class AresAttribute : System.Attribute
    {
    }

#if UNITY_EDITOR
    public partial class AresAttribute
    {
        public object target;   // MB or SO or serializable class

        public virtual void OnGUI() { }
    }
#endif
}
