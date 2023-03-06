using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public partial class ADPropertyField : AresDrawer
    {
        public ADPropertyField() : base(false)
        {

        }
    }

#if UNITY_EDITOR
    public partial class ADPropertyField
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            PropertyField pf = new PropertyField(context.property);
            pf.style.flexGrow = 1;//尽量撑满，1个就100%，两个就各50%...
            return pf;
        }
    }
#endif
}
