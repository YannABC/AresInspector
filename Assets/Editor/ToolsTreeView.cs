using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tools
{
    /// <summary>
    /// 
    /// </summary>
    class ToolsTreeView : TreeView
    {
        Dictionary<int, ToolsTreeItem> m_Items = new Dictionary<int, ToolsTreeItem>();

        IList<TreeViewItem> m_SelectedItems = null;
        bool m_SelectChanged = false;

        VisualElement m_Container;

        public ToolsTreeView(TreeViewState treeViewState, VisualElement container)
            : base(treeViewState)
        {
            m_Container = container;
            BuildItems();
            Reload();

            SelectionChanged(GetSelection());
        }

        void BuildItems()
        {
            ToolsTreeItem item = new ToolsTreeItem()
            {
                id = 1,
                depth = 0,
                displayName = "myso",
                file = "UserSettings/myso.asset",
                //file = "Assets/Editor/_Temp/myso.asset",
                type = typeof(MySo)
            };
            m_Items.Add(item.id, item);

            ToolsTreeItem item2 = new ToolsTreeItem()
            {
                id = 2,
                depth = 0,
                displayName = "myso2",
                file = "Assets/Editor/_Temp/myso2.asset",
                //file = "Assets/Editor/_Temp/myso2.asset",
                type = typeof(MySo2)
            };
            m_Items.Add(item2.id, item2);
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            List<TreeViewItem> allItems = new List<TreeViewItem>();
            foreach (var kv in m_Items)
            {
                allItems.Add(kv.Value);
            }

            //List<TreeViewItem> allItems = new List<TreeViewItem>
            //{
            //    new TreeViewItem {id =  1, depth = 0, displayName = "Animals"},
            //    new TreeViewItem {id =  2, depth = 1, displayName = "Mammals"},
            //    new TreeViewItem {id =  3, depth = 2, displayName = "Tiger"},
            //    new TreeViewItem {id =  4, depth = 2, displayName = "Elephant"},
            //    new TreeViewItem {id =  5, depth = 2, displayName = "Okapi"},
            //    new TreeViewItem {id =  6, depth = 2, displayName = "Armadillo"},
            //    new TreeViewItem {id =  7, depth = 1, displayName = "Reptiles"},
            //    new TreeViewItem {id =  8, depth = 2, displayName = "Crocodile"},
            //    new TreeViewItem {id =  9, depth = 2, displayName = "Lizard"},
            //};

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
                VisualElement ve = (item as ToolsTreeItem).OnOpen();
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
                    (item as ToolsTreeItem).OnClose();
                }
                m_SelectedItems = null;
            }
        }
    }
}