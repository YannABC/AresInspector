﻿using UnityEditor;
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
            if (member.IsFieldMember() || member.IsPropertyMember())
            {
                //SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
                //string labelName = member.GetLabelText(prop);
                //int width = member.GetLabelWidth();

                //不能自己画Label，自己画的不能drag
                root = CreateCustomGUI(context);
                SetLabelWidth(root);
            }
            else
            {
                root = CreateCustomGUI(context);
            }

            SetLabelColor(root);
            SetBackgroundColor(root);

            return root;
        }

        protected virtual VisualElement CreateCustomGUI(AresContext context)
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

            //ve.RegisterCallback((AttachToPanelEvent e)
            //触发后，子元素也不一定已经初始化
            //所以用DoUntil
            AresHelper.DoUntil(() =>
            {
                //if (ve.panel == null) return true;

                TextElement te = ve.Q<TextElement>();
                if (te != null)
                {
                    te.style.color = ac.color;
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        protected void SetLabelWidth(VisualElement ve)
        {
            if (ve == null) return;
            int width = 120;
            ACLabelWidth ac = member.GetACLabelWidth();
            if (ac != null)
            {
                width = ac.width;
            }
            //unity BaseFile  m_LabelBaseMinWidth = 120 写死了
            //设置了也会被自动设成120
            //只能监听改变的事件，每次重新设置一次
            ve.RegisterCallback((GeometryChangedEvent e) =>
            {
                var lbl = ve.Q<Label>();
                if (lbl != null)
                {
                    if (width > 0)
                    {
                        lbl.style.minWidth = width;
                        lbl.style.width = width;
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
