using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public class AresMethod : AresAttribute
    {
        public MethodInfo methodInfo;
        public AresMethod(
            string show = "true",  //是否显示，false or method
            int group = 0          //所在组id
            )
        {
        }
    }
}
