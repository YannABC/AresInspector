using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Ares
{
    //base class for all drawers
    public abstract partial class AresDrawer : System.Attribute
    {
        public bool isDecrator;
        public AresDrawer(bool isDecrator = false)
        {
            this.isDecrator = isDecrator;
        }
    }

#if UNITY_EDITOR
    public abstract partial class AresDrawer
    {
        public AresMember member;
        public virtual VisualElement CreateGUI(AresContext context)
        {
            VisualElement root;
            if (member.IsFieldMember())
            {
                SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
                //string labelName = member.GetLabelText(prop);
                //int width = member.GetLabelWidth();

                //不能自己画Label，自己画的不能drag
                root = CreateFieldGUI(context);
                SetLabelWidth(root);
            }
            else
            {
                root = CreateMethodGUI(context);
            }

            SetLabelColor(root);
            SetBackgroundColor(root);

            return root;
        }

        protected virtual VisualElement CreateFieldGUI(AresContext context)
        {
            return null;
        }

        protected virtual VisualElement CreateMethodGUI(AresContext context)
        {
            return null;
        }

        protected void SetBackgroundColor(VisualElement ve)
        {
            if (ve == null) return;
            ACBgColor ac = member.GetACBackgroundColor();
            if (ac == null) return;
            ve.style.backgroundColor = ac.color;
        }

        protected void SetLabelColor(VisualElement ve)
        {
            if (ve == null) return;
            ACLabelColor ac = member.GetACLabelColor();
            if (ac == null) return;
            ve.RegisterCallback((AttachToPanelEvent e) =>
            {
                //Label , Button etc is TextElement
                TextElement te = ve.Q<TextElement>();
                if (te != null)
                {
                    te.style.color = ac.color;
                }
            });
        }

        protected void SetLabelWidth(VisualElement ve)
        {
            if (ve == null) return;
            ACLabelWidth ac = member.GetACLabelWidth();
            if (ac == null) return;
            //unity BaseFile  m_LabelBaseMinWidth = 120 写死了
            //设置了也会被自动设成120
            //只能监听改变的事件，每次重新设置一次
            ve.RegisterCallback((GeometryChangedEvent e) =>
            {
                var lbl = ve.Q<Label>();
                if (lbl != null)
                {
                    if (ac.width > 0)
                    {
                        lbl.style.minWidth = ac.width;
                        lbl.style.width = ac.width;
                    }
                    else
                    {
                        lbl.style.minWidth = StyleKeyword.Auto;
                        lbl.style.width = StyleKeyword.Auto;
                    }
                }
            });
        }
    }
#endif
}
