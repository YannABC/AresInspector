using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace Ares.Examples
{
    public class AresExamples : AresTreeWindow
    {
        public override List<AresTreeItem> GetTreeItems()
        {
            List<AresTreeItem> items = new List<AresTreeItem>();

            items.Add(new AresTreeItem(1, 0, "Overview"));

            #region Layout
            {
                int id = 1000;
                items.Add(new AresTreeItem(id++, 0, "Layout"));

                items.Add(new AresTreeItem(id++, 1, "Layout Horizontal")
                {
                    file = "Assets/AresInspector/Examples/_Temp/LayoutH.asset",
                    type = typeof(LayoutH)
                });

                items.Add(new AresTreeItem(id++, 1, "Layout Vertical")
                {
                    file = "Assets/AresInspector/Examples/_Temp/LayoutV.asset",
                    type = typeof(LayoutV)
                });

                items.Add(new AresTreeItem(id++, 1, "Layout Nest")
                {
                    file = "Assets/AresInspector/Examples/_Temp/LayoutNest.asset",
                    type = typeof(LayoutNest)
                });

                items.Add(new AresTreeItem(id++, 1, "Layout Adjust Order")
                {
                    file = "Assets/AresInspector/Examples/_Temp/LayoutAdjustOrder.asset",
                    type = typeof(LayoutAdjustOrder)
                });
            }
            #endregion

            #region Drawer
            {
                int id = 2000;
                items.Add(new AresTreeItem(id++, 0, "Drawer"));

                items.Add(new AresTreeItem(id++, 1, "Button")
                {
                    file = "Assets/AresInspector/Examples/_Temp/DrawerButton.asset",
                    type = typeof(DrawerButton)
                });
            }
            #endregion

            return items;
        }

        protected override VisualElement CreateTreeItemGUI(AresTreeItem ati)
        {
            if (ati.id == 1)
            {
                return new HelpBox(OverView, HelpBoxMessageType.None);
            }
            return base.CreateTreeItemGUI(ati);
        }

        [MenuItem("Ares/Examples %t")]
        static void ShowWindow()
        {
            OpenOrClose("Ares Examples", "UserSettings/AresExamples/asset", typeof(AresExamples));
        }

        const string OverView = @"AresInspector 是 类似odin的编辑器，使用Attribute的方式来写编辑器。
和odin不同的是，AresInspector更加轻量，使用了纯UIToolkit(VisualEment),没有使用IMGUI。
包含<color=blue>Layout</color>,<color=blue>Drawer</color>和<color=blue>Config</color>.";
    }
}
#endif