using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public class AresGroup : AresAttribute
    {
        public List<AresAttribute> attrs = new List<AresAttribute>();

        public AresGroup(
            string show = "true",  //是否显示，false or method
            int group = 0          //所在组id
            )
        {
        }
    }
}
