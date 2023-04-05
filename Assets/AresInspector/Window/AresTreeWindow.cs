using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

#if UNITY_EDITOR
namespace Ares
{
    public abstract class AresTreeWindow : AresWindow
    {
        [SerializeField]
        float m_SplitterPos = 150;//分隔符宽度

        [SerializeField]
        TreeViewState m_TreeState;

        AresTreeView m_TreeView;
        IMGUIContainer m_IMGUITree;

        TwoPaneSplitView m_Splitter;
        protected ScrollView m_ScrollView;
        protected VisualElement m_Header;
        protected VisualElement m_Footer;

        void OnEnable()
        {
            if (m_TreeState == null)
                m_TreeState = new TreeViewState();

            m_Splitter = new TwoPaneSplitView(0, m_SplitterPos, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(m_Splitter);

            //左侧
            VisualElement left = new VisualElement();
            left.style.flexGrow = 1;
            left.style.flexDirection = FlexDirection.Column;
            m_Splitter.Add(left);

            //左侧 - 搜索栏
            ToolbarSearchField search = new ToolbarSearchField();
            search.style.flexGrow = 0;
            search.style.width = StyleKeyword.Auto;
            left.Add(search);
            search.RegisterValueChangedCallback((ChangeEvent<string> s) =>
            {
                m_TreeView.searchString = s.newValue;
            });

            //左侧 - 树状
            m_IMGUITree = new IMGUIContainer(DrawTreeView);
            m_IMGUITree.style.flexGrow = 1;
            left.Add(m_IMGUITree);

            //右侧
            VisualElement right = new VisualElement();
            m_Splitter.Add(right);

            m_Header = new VisualElement();
            m_Header.name = "header";
            right.style.flexGrow = 0;
            right.Add(m_Header);

            m_ScrollView = new ScrollView();
            m_ScrollView.style.flexGrow = 1;
            right.Add(m_ScrollView);

            m_Footer = new VisualElement();
            m_Footer.name = "footer";
            right.style.flexGrow = 0;
            right.Add(m_Footer);

            m_TreeView = new AresTreeView(m_TreeState, this);

            left.RegisterCallback((GeometryChangedEvent evt) =>
            {
                m_SplitterPos = left.style.width.value.value;
                //if (m_SplitterPos < 10)
                //{
                //    m_SplitterPos = 10;
                //    //left.style.width = m_SplitterPos;
                //}
            });
        }


        void DrawTreeView()
        {
            m_TreeView?.OnGUI(new Rect(0, 0, m_SplitterPos, m_IMGUITree.worldBound.height));
        }

        public virtual List<AresTreeItem> GetTreeItems()
        {
            return null;
        }

        public virtual void OnSelectionChanged(IList<TreeViewItem> items)
        {
            m_Header.Clear();
            m_Footer.Clear();
            m_ScrollView.Clear();
            if (items.Count == 1)
            {
                AresTreeItem ati = items[0] as AresTreeItem;
                VisualElement ve = CreateHeaderGUI(ati);
                if (ve != null)
                {
                    m_Header.Add(ve);
                }
            }
            foreach (TreeViewItem item in items)
            {
                AresTreeItem ati = item as AresTreeItem;
                VisualElement ve = CreateTreeItemGUI(ati);
                if (ve != null)
                {
                    m_ScrollView.Add(ve);
                }
            }
            if (items.Count == 1)
            {
                AresTreeItem ati = items[0] as AresTreeItem;
                VisualElement ve = CreateFooterGUI(ati);
                if (ve != null)
                {
                    m_Footer.Add(ve);
                }
            }
        }

        protected virtual VisualElement CreateTreeItemGUI(AresTreeItem ati)
        {
            return ati.OnOpen();
        }
        protected virtual VisualElement CreateHeaderGUI(AresTreeItem ati)
        {
            return null;
        }
        protected virtual VisualElement CreateFooterGUI(AresTreeItem ati)
        {
            return null;
        }

        protected override void OnDisable()
        {
            m_TreeView.OnDisable();
            base.OnDisable();
        }
    }
}
#endif