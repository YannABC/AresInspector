using System;
using UnityEngine.UIElements;

namespace Ares
{
    // Attribute used to make a horizontal or vertical space
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public partial class ADSpace : AresDrawer
    {
        public readonly float size;

        public ADSpace(float size = 8) : base(true)
        {
            // By default uses 8 pixels which corresponds to EditorGUILayout.Space()
            // which reserves 6 pixels, plus the usual 2 pixels caused by the neighboring margin.
            // (Why not 2 pixels for margin both below and above?
            // Because one of those is already accounted for when the space is not there.)
            this.size = size;
        }
    }

#if UNITY_EDITOR
    public partial class ADSpace
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            bool horizontal = member.group.type == EAresGroupType.Horizontal;
            VisualElement ve = new VisualElement();
            //ve.style.flexDirection = horizontal ? FlexDirection.Row : FlexDirection.Column;
            if (horizontal)
            {
                ve.style.width = size;
            }
            else
            {
                ve.style.height = size;
            }

            ve.style.flexGrow = 1;
            return ve;
        }
    }
#endif
}
