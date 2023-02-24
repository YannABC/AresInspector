using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public class AresField : AresMember
    {
        //{{
        public string label;
        public int labelSize;
        public bool inline;
        public string[] popupList;
        public string popupGetter;
        //}}

        public FieldInfo fieldInfo;

#if UNITY_EDITOR
        public UnityEditor.SerializedProperty property;
#endif

        public AresField(
            string label = null,                    //label text
            int labelSize = 0,                      //label size
            bool inline = false,                    //inline
            string[] popupList = null,               //pop up list
            string popupGetter = null               //pop up method
            ) : base()
        {
            this.label = label;
            this.labelSize = labelSize;
            this.inline = inline;
            this.popupList = popupList;
            this.popupGetter = popupGetter;
        }
    }
}
