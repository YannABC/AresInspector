using System;
using UnityEditor;
using UnityEngine.UIElements;
//using UnityObject = UnityEngine.Object;

namespace Ares
{
    // Attribute used to make a horizontal space
    [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public partial class AresSpace : AresDrawer
    {
        public readonly float size;

        public AresSpace(float size = 8) : base(true)
        {
            // By default uses 8 pixels which corresponds to EditorGUILayout.Space()
            // which reserves 6 pixels, plus the usual 2 pixels caused by the neighboring margin.
            // (Why not 2 pixels for margin both below and above?
            // Because one of those is already accounted for when the space is not there.)
            this.size = size;
        }
    }

#if UNITY_EDITOR
    public partial class AresSpace
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
