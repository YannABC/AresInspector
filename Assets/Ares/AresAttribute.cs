using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public class AresAttribute : System.Attribute
    {
        public Object target;
        public System.Type type;
        public int prority = 0;//越小越排前面
        public int group = 0;
    }
}
