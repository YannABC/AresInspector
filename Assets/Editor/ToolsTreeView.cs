using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace Tools
{
    /// <summary>
    /// 未解决问题
    /// 1. 开着，重新reload,打不开窗口
    /// 2. treeviewitem ctrl不能取消
    /// 3. splitter有问题
    /// </summary>
    class ToolsTreeView : TreeView
    {
        Dictionary<int, ToolsTreeItem> m_Items = new Dictionary<int, ToolsTreeItem>();

        IList<TreeViewItem> m_Selected = null;

        public ToolsTreeView(TreeViewState treeViewState)
            : base(treeViewState)
        {
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
                file = "Assets/Editor/_Temp/myso.asset",
                type = typeof(MySo)
            };
            m_Items.Add(item.id, item);

            ToolsTreeItem item2 = new ToolsTreeItem()
            {
                id = 2,
                depth = 0,
                displayName = "myso2",
                file = "Assets/Editor/_Temp/myso2.asset",
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

        internal void DrawContent()
        {
            if (m_Selected == null) return;
            foreach (var item in m_Selected)
            {
                (item as ToolsTreeItem).OnDraw();
            }
        }

        internal void OnDisable()
        {
            ClearSelectedItems();
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            ClearSelectedItems();

            m_Selected = FindRows(selectedIds);
            if (m_Selected == null) return;
            foreach (var item in m_Selected)
            {
                (item as ToolsTreeItem).OnOpen();
            }
        }

        void ClearSelectedItems()
        {
            if (m_Selected == null) return;
            foreach (var item in m_Selected)
            {
                (item as ToolsTreeItem).OnClose();
            }
            m_Selected = null;
        }
    }
}