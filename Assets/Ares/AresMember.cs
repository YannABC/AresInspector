using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class AresMember : AresAttribute
    {
        //{{
        public int groupId; 
        public int prority;
        public string show;    
        public string enable;  
        public Color? color;
        //}}

        public int index; // for stable sort

        public UnityEngine.Object target;
        public System.Type type;//自己，或者 父类

        public AresMember(
            int groupId = 0,                 //所在group
            int prority = 0,                 //所在group中的排序，越小越排前面
            string show = "true",            //是否显示 true false or {custom_method_name}
            string enable = "true",          //是否可改 true false or {custom_method_name}
            Color? color = null              //颜色
            )
        {
            this.groupId = groupId;
            this.prority = prority;
            this.show = show;
            this.enable = enable;
            this.color = color;
        }
    }
}
