using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.PackageManager.UI;

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

        TwoPaneSplitView m_Splitter;
        protected VisualElement m_Right;

        void OnEnable()
        {
            if (m_TreeState == null)
                m_TreeState = new TreeViewState();

            m_Splitter = new TwoPaneSplitView(0, m_SplitterPos, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(m_Splitter);

            //左侧
            VisualElement left = new VisualElement();
            m_Splitter.Add(left);

            //左侧 - 搜索栏
            ToolbarSearchField search = new ToolbarSearchField();
            left.Add(search);
            search.style.position = Position.Absolute;
            search.style.left = 0;
            search.style.width = m_SplitterPos - 8;
            search.style.top = 0;
            search.style.height = 18;
            search.RegisterValueChangedCallback((ChangeEvent<string> s) =>
            {
                m_TreeView.searchString = s.newValue;
            });

            //左侧 - 树状
            IMGUIContainer tree = new IMGUIContainer(DrawTreeView);
            left.Add(tree);
            tree.style.position = Position.Absolute;
            tree.style.left = 0;
            tree.style.right = 0;
            tree.style.top = 20;
            tree.style.bottom = 0;

            //右侧
            m_Right = new VisualElement();
            m_Splitter.Add(m_Right);
            m_Right.style.position = Position.Absolute;
            m_Right.style.left = m_SplitterPos + 8;
            m_Right.style.right = 0;
            m_Right.style.top = 0;

            m_TreeView = new AresTreeView(m_TreeState, this);

            left.RegisterCallback((GeometryChangedEvent evt) =>
            {
                m_SplitterPos = left.style.width.value.value;
                search.style.width = m_SplitterPos - 8;
                m_Right.style.left = m_SplitterPos + 8;
            });
        }


        void DrawTreeView()
        {
            m_TreeView?.OnGUI(new Rect(0, 0, m_SplitterPos, position.height));
        }

        public virtual List<AresTreeItem> GetTreeItems()
        {
            return null;
        }

        public virtual void OnSelectionChanged(IList<TreeViewItem> items)
        {
            m_Right.Clear();
            foreach (TreeViewItem item in items)
            {
                AresTreeItem ati = item as AresTreeItem;
                VisualElement ve = ati.OnOpen();
                if (ve != null)
                {
                    m_Right.Add(ve);
                }
            }
        }

        protected override void OnDisable()
        {
            m_TreeView.OnDisable();
            base.OnDisable();
        }
    }
}
#endif