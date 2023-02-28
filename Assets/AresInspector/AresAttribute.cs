using System.Diagnostics;
using UnityEditor;
using UnityEngine.UIElements;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    //register update
    public interface IAresObjectV { }  //Vertical
    public interface IAresObjectH { }  //Horizontal


    [Conditional("UNITY_EDITOR")]
    public partial class AresAttribute : System.Attribute
    {
    }

#if UNITY_EDITOR
    public partial class AresAttribute
    {
        public virtual VisualElement CreateGUI(AresContext context) { return null; }
    }
#endif
}
