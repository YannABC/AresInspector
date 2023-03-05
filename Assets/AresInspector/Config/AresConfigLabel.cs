using System;
using System.Diagnostics;
using UnityEditor;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public partial class AresConfigLabel : System.Attribute
    {
        public readonly string label;        //label text  (null use property.displayName, empty not show)
        public readonly int size;

        public AresConfigLabel(string label = null, int size = 0)
        {
            this.label = label;
            this.size = size;
        }
    }
}
