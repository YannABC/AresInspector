using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
namespace Ares
{
    public class AresExamples : AresTreeWindow
    {
        public override List<AresTreeItem> GetTreeItems()
        {
            List<AresTreeItem> items = new List<AresTreeItem>();

            AresTreeItem item1000 = new AresTreeItem()
            {
                id = 1000,
                depth = 0,
                displayName = "Layout",
            };
            items.Add(item1000);

            AresTreeItem item1001 = new AresTreeItem()
            {
                id = 1001,
                depth = 1,
                displayName = "Layout Horizontal",
                file = "Assets/AresInspector/Examples/_Temp/LayoutH.asset",
                type = typeof(LayoutH)
            };
            items.Add(item1001);

            AresTreeItem item1002 = new AresTreeItem()
            {
                id = 1002,
                depth = 1,
                displayName = "Layout Vertical",
                file = "Assets/AresInspector/Examples/_Temp/LayoutV.asset",
                type = typeof(LayoutV)
            };
            items.Add(item1002);

            AresTreeItem item2000 = new AresTreeItem()
            {
                id = 2000,
                depth = 0,
                displayName = "Drawer",
            };
            items.Add(item2000);

            AresTreeItem item2001 = new AresTreeItem()
            {
                id = 2001,
                depth = 1,
                displayName = "Button",
                file = "Assets/AresInspector/Examples/_Temp/DrawerButton.asset",
                type = typeof(DrawerButton)
            };
            items.Add(item2001);

            return items;
        }

        [MenuItem("Ares/Examples %t")]
        static void ShowWindow()
        {
            OpenOrClose("Ares Examples", "UserSettings/AresExamples/asset", typeof(AresExamples));
        }
    }
}
#endif