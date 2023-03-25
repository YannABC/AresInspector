using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Ares
{
    public abstract class AresTreeWindow<T> : AresWindow<T> where T : EditorWindow
    {
        [SerializeField]
        float m_SplitterPos = 150;//分隔符宽度

        [SerializeField]
        TreeViewState m_TreeState;

        AresTreeView m_TreeView;

        TwoPaneSplitView m_Splitter;

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
            VisualElement right = new VisualElement();
            m_Splitter.Add(right);
            right.style.position = Position.Absolute;
            right.style.left = m_SplitterPos + 8;
            right.style.right = 0;
            right.style.top = 0;

            m_TreeView = new AresTreeView(m_TreeState, right, GetITreetems);

            left.RegisterCallback((GeometryChangedEvent evt) =>
            {
                m_SplitterPos = left.style.width.value.value;
                search.style.width = m_SplitterPos - 8;
                right.style.left = m_SplitterPos + 8;
            });
        }

        void DrawTreeView()
        {
            m_TreeView?.OnGUI(new Rect(0, 0, m_SplitterPos, position.height));
        }

        protected virtual List<AresTreeItem> GetITreetems()
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