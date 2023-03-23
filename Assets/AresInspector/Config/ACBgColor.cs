using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACBgColor : System.Attribute
    {
        public readonly Color color;

        public ACBgColor(float r, float g, float b, float a = 1f)
        {
            this.color = new Color(r, g, b, a);
        }
    }
}