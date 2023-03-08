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
            if (member.IsFieldMember())
            {
                SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
                string labelName = GetLabel(prop);
                int size = GetLabelSize();

                //不能自己画Label，自己画的不能drag
                return CreateFieldGUI(context, labelName, size);
            }
            else
            {
                return CreateMethodGUI(context);
            }
        }

        protected virtual VisualElement CreateFieldGUI(AresContext context, string labelName, int size)
        {
            return null;
        }

        protected virtual VisualElement CreateMethodGUI(AresContext context)
        {
            return null;
        }

        protected void SetLabelSize(VisualElement ve, int size)
        {
            ve.RegisterCallback((GeometryChangedEvent e) =>
            {
                var lbl = ve.Q<Label>();
                if (lbl != null)
                {
                    if (size > 0)
                    {
                        lbl.style.minWidth = size;
                        lbl.style.width = size;
                    }
                    else
                    {
                        lbl.style.minWidth = StyleKeyword.Auto;
                        lbl.style.width = StyleKeyword.Auto;
                    }
                }
            });
        }

        protected void SetOnValueChanged(VisualElement ve, object target)
        {
            if (ve == null) return;
            if (member.onValueChanged == null) return;
            ve.RegisterCallback((SerializedPropertyChangeEvent evt) =>
            {
                member.onValueChanged.Invoke(target, null);

                //if (evt.target == this)
                //{
                //    callback(evt);
                //}
            });
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
                size = 120;//unity default
            }
            return size;
        }
    }
#endif
}
