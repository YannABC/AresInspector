using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace Ares
{
    /// <summary>
    /// 
    /// </summary>
    internal class AresTreeView : TreeView
    {
        IList<TreeViewItem> m_SelectedItems = null;
        bool m_SelectChanged = false;

        AresTreeWindow m_Window;

        public AresTreeView(TreeViewState treeViewState, AresTreeWindow window)
            : base(treeViewState)
        {
            m_Window = window;
            Reload();

            SelectionChanged(GetSelection());
        }

        protected override TreeViewItem BuildRoot()
        {
            List<AresTreeItem> items = m_Window.GetTreeItems();

            List<TreeViewItem> allItems = new List<TreeViewItem>();

            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            allItems.AddRange(items);

            SetupParentsAndChildrenFromDepths(root, allItems);

            return root;
        }

        //解决 ctrl+click不能 deselect的问题
        protected override void SingleClickedItem(int id)
        {
            if (ControlIsDown() && !m_SelectChanged)
            {
                List<int> ids = new List<int>(GetSelection());
                ids.Remove(id);
                SetSelection(ids, TreeViewSelectionOptions.FireSelectionChanged);
            }

            m_SelectChanged = false;
        }

        bool ControlIsDown()
        {
            //return Event.current.control;
            return EditorGUI.actionKey;// control on windows,  cmd on mac
        }

        internal void OnDisable()
        {
            ClearSelectedItems();
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            ClearSelectedItems();

            m_SelectedItems = FindRows(selectedIds);

            m_Window.OnSelectionChanged(m_SelectedItems);

            m_SelectChanged = true;
        }

        void ClearSelectedItems()
        {
            if (m_SelectedItems != null)
            {
                foreach (TreeViewItem item in m_SelectedItems)
                {
                    (item as AresTreeItem).OnClose();
                }
                m_SelectedItems = null;
            }
        }
    }
}
#endif