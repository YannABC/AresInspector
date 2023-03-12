using System;
using System.Diagnostics;
using UnityEditor;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACLabelText : System.Attribute
    {
        public readonly string text;       // if null, use displayName. if "", then hide the label

        public ACLabelText(string text = null)
        {
            this.text = text;
        }
    }
}