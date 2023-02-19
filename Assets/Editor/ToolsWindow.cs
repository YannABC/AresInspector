using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Tools
{
    class ToolsWindow : SavableWindow<ToolsWindow>
    {
        [SerializeField] float m_SplitterWidth = 150;//分隔符宽度

        [SerializeField] TreeViewState m_TreeState;
        ToolsTreeView m_TreeView;

        TwoPaneSplitView m_Splitter;

        void OnEnable()
        {
            SetupTreeView();
            SetupSplitter();
        }

        void SetupTreeView()
        {
            if (m_TreeState == null)
                m_TreeState = new TreeViewState();

            m_TreeView = new ToolsTreeView(m_TreeState);
        }

        void DrawTreeView()
        {
            m_TreeView.OnGUI(new Rect(0, 0, m_SplitterWidth, position.height));
        }

        void SetupSplitter()
        {
            m_Splitter = new TwoPaneSplitView(0, m_SplitterWidth, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(m_Splitter);

            VisualElement left = new VisualElement();
            m_Splitter.Add(left);

            ToolbarSearchField search = new ToolbarSearchField();
            left.Add(search);
            search.style.position = Position.Absolute;
            search.style.left = 0;
            search.style.width = m_SplitterWidth - 8;
            search.style.top = 0;
            search.style.height = 18;

            IMGUIContainer tree = new IMGUIContainer(DrawTreeView);
            left.Add(tree);
            tree.style.position = Position.Absolute;
            tree.style.left = 0;
            tree.style.right = 0;
            tree.style.top = 20;
            tree.style.bottom = 0;

            m_Splitter.Add(new VisualElement());

            left.RegisterCallback<GeometryChangedEvent>((evt) =>
            {
                m_SplitterWidth = left.style.width.value.value;
                search.style.width = m_SplitterWidth - 8;
            });
        }

        void OnGUI()
        {
            //Debug.Log("ONGUI");
            //m_TreeView.OnGUI(new Rect(0, 0, m_SplitterWidth, position.height));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        // Add menu named "My Window" to the Window menu
        [MenuItem("Tools/Open or Close Tools %t")]
        static void ShowWindow()
        {
            OpenOrClose("Tools");
        }
    }
}