using System.Diagnostics;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public partial class AresField : AresMember
    {
        //{{
        public readonly string label;
        public readonly int labelSize;
        public readonly bool inline;
        //}}

        public AresField(
            string label = null,                    //label text  (null use property, empty not show)
            int labelSize = 0,                      //label size
            bool inline = false                     //inline                
            ) : base()
        {
            this.label = label;
            this.labelSize = labelSize;
            this.inline = inline;

            //AresLabel?
            //AresDropDown?
        }
    }

#if UNITY_EDITOR
    public partial class AresField
    {
        public FieldInfo fieldInfo;
        List<AresDrawer> m_Drawers;

        public override void Init()
        {
            IEnumerable<AresDrawer> drawers = fieldInfo.GetCustomAttributes<AresDrawer>();
            int drawnCount = drawers.Count(ad => !ad.isDecrator);
            m_Drawers = new List<AresDrawer>(drawers);
            if (drawnCount == 0)
            {
                m_Drawers.Add(new AresDrawer());
            }
            foreach (AresDrawer d in m_Drawers)
            {
                d.member = this;
            }
        }

        public override VisualElement CreateGUI(AresContext context)
        {
            AresContext childContext = new AresContext(context.FindProperty(fieldInfo.Name), fieldInfo);

            if (m_Drawers.Count > 1)
            {
                VisualElement root = new VisualElement();
                root.style.flexDirection = group.type == EAresGroupType.Horizontal ? FlexDirection.Row : FlexDirection.Column;
                root.style.flexGrow = 1;
                foreach (AresDrawer drawer in m_Drawers)
                {
                    VisualElement child = drawer.CreateGUI(childContext);
                    if (child != null)
                    {
                        root.Add(child);
                    }
                }
                return root;
            }
            else
            {
                return m_Drawers[0].CreateGUI(childContext);//AresDefault
            }
        }
    }
#endif
}
