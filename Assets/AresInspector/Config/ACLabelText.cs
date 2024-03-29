﻿using System;
using System.Diagnostics;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
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