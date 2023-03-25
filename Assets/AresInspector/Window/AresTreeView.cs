using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ares
{
    /// <summary>
    /// 
    /// </summary>
    internal class AresTreeView : TreeView
    {
        IList<TreeViewItem> m_SelectedItems = null;
        bool m_SelectChanged = false;
        System.Func<List<AresTreeItem>> m_GetItems;
        VisualElement m_Container;

        public AresTreeView(TreeViewState treeViewState, VisualElement container, System.Func<List<AresTreeItem>> getItems)
            : base(treeViewState)
        {
            m_Container = container;
            m_GetItems = getItems;
            Reload();

            SelectionChanged(GetSelection());
        }


        protected override TreeViewItem BuildRoot()
        {
            List<AresTreeItem> items = m_GetItems.Invoke();

            List<TreeViewItem> allItems = new List<TreeViewItem>();

            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            //allItems.Add(root);
            allItems.AddRange(items);

            // Utility method that initializes the TreeViewItem.children and .parent for all items.
            SetupParentsAndChildrenFromDepths(root, allItems);

            // Return root of the tree
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
            m_Container.Clear();
            foreach (TreeViewItem item in m_SelectedItems)
            {
                VisualElement ve = (item as AresTreeItem).OnOpen();
                if (ve != null)
                {
                    m_Container.Add(ve);
                }
            }

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