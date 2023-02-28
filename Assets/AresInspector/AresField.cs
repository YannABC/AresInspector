﻿using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public partial class AresField : AresMember
    {
        //{{
        public string label;
        public int labelSize;
        public bool inline;
        //}}

        public AresField(
            string label = null,                    //label text  (null use property, empty not show)
            int labelSize = 0,                      //label size
            bool inline = false                     //inline                
            ) : base()
        {
            this.label = label;
            this.labelSize = labelSize;
            this.inline = inline;

            //AresLabel?
            //AresDropDown?
        }
    }

#if UNITY_EDITOR
    public partial class AresField
    {
        public FieldInfo fieldInfo;

        public override VisualElement CreateGUI(AresContext context)
        {
            PropertyField pf = new PropertyField(context.FindProperty(fieldInfo.Name));
            return pf;
        }
    }
#endif
}
