using System;
using System.Collections;
using System.Diagnostics;

namespace Ares
{
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ACLayout : System.Attribute
    {
        //{{
        public int groupId;
        public int order;
        //}}

        public ACLayout(
            int groupId = 0,             //所在group
            int order = 0                //所在group中的排序，越小越排前面
            )
        {
            this.groupId = groupId;
            this.order = order;
        }
    }
}
