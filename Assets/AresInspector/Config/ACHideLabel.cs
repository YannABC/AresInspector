using System;
using System.Diagnostics;
using UnityEditor;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public partial class ACLabelSize : System.Attribute
    {
        public readonly int size;

        public ACLabelSize(int size = 0)
        {
            this.size = size;
        }
    }
}
