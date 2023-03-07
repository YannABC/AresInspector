using UnityEditor;
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
            if (member.IsFieldMember())
            {
                SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
                string labelName = GetLabel(prop);
                int size = GetLabelSize();

                if (labelName == "")
                {
                    return CreateFieldGUI(context);
                }
                else
                {
                    VisualElement root = new VisualElement();
                    root.style.flexDirection = FlexDirection.Row;
                    root.style.flexGrow = 1;

                    Label label = new Label(labelName);
                    if (size > 0) label.style.width = size;
                    root.Add(label);

                    VisualElement child = CreateFieldGUI(context);
                    if (child != null)
                    {
                        root.Add(child);
                    }

                    return root;
                }
            }
            else
            {
                return CreateMethodGUI(context);
            }
        }

        protected virtual VisualElement CreateFieldGUI(AresContext context)
        {
            return null;
        }

        protected virtual VisualElement CreateMethodGUI(AresContext context)
        {
            return null;
        }


        protected string GetLabel(SerializedProperty prop)
        {
            ACLabel acl = member.label;
            string labelName;
            if (acl != null)
            {
                labelName = acl.label ?? prop.displayName;
            }
            else
            {
                labelName = prop.displayName;
            }
            return labelName;
        }

        protected int GetLabelSize()
        {
            ACLabel acl = member.label;
            int size;
            if (acl != null)
            {
                size = acl.size;
            }
            else
            {
                size = 120;
            }
            return size;
        }
    }
#endif
}
