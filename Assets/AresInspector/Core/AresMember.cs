using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public abstract partial class AresMember : AresAttribute
    {
        //{{
        public int groupId;
        public int order;
        public string visible;
        public string enable;
        public Color? color;
        //}}

        public AresMember(
            int groupId = 0,                 //所在group
            int order = 0,                 //所在group中的排序，越小越排前面
            string visible = "true",         //是否显示 true false or {custom_method_name}
            string enable = "true",          //是否可改 true false or {custom_method_name}
            Color? color = null              //颜色
            )
        {
            this.groupId = groupId;
            this.order = order;
            this.visible = visible;
            this.enable = enable;
            this.color = color;
        }
    }

#if UNITY_EDITOR
    public abstract partial class AresMember
    {
        public int index; // for stable sort
        public Type ancestor;//self or current base class
        public AresGroup group;// the group that contains the member

        public virtual void Init() { }

        public bool IsVisible(object target)
        {
            return Is(visible, target);
        }

        public bool IsEnable(object target)
        {
            return Is(enable, target);
        }

        bool Is(string key, object target)
        {
            bool ret = true;
            if (key == "true")
            {
                ret = true;
            }
            else if (key == "false")
            {
                ret = false;
            }
            else if (!string.IsNullOrEmpty(key))
            {
                MethodInfo mi = ancestor.GetMethodInfo(key);

                if (mi == null)
                {
                    UnityEngine.Debug.LogError(key + " method not found");
                }
                if (mi != null)
                {
                    ret = (bool)mi.Invoke(target, null);
                }
            }
            return ret;
        }
#endif
    }
}
