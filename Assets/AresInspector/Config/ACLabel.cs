using System;
using System.Diagnostics;
using UnityEditor;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public partial class ACLabel : System.Attribute
    {
        public readonly string label;       // if null, use displayName. if "", then hide the label
        public readonly int size;           // if has label and size == 0, size is auto

        public ACLabel(string label = null, int size = 120)
        {
            this.label = label;
            this.size = size;

            if (this.label == "")
            {
                this.size = 0;
            }
        }
    }
}