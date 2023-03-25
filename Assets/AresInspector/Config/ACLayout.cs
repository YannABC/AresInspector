using System.Collections;
using System.Diagnostics;

namespace Ares
{
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
