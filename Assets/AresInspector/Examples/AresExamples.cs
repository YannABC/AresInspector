using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Ares
{
    public class AresExamples : AresTreeWindow<AresExamples>
    {
        protected override List<AresTreeItem> GetITreetems()
        {
            List<AresTreeItem> items = new List<AresTreeItem>();

            AresTreeItem item = new AresTreeItem()
            {
                id = 1,
                depth = 0,
                displayName = "myso",
                file = "UserSettings/myso.asset",
                //file = "Assets/Editor/_Temp/myso.asset",
                type = typeof(MySo)
            };
            items.Add(item);

            AresTreeItem item3 = new AresTreeItem()
            {
                id = 3,
                depth = 1,
                displayName = "myso3",
                file = "Assets/Editor/_Temp/myso2.asset",
                //file = "Assets/Editor/_Temp/myso2.asset",
                type = typeof(MySo2)
            };
            items.Add(item3);

            AresTreeItem item2 = new AresTreeItem()
            {
                id = 2,
                depth = 0,
                displayName = "myso2",
                file = "Assets/Editor/_Temp/myso2.asset",
                //file = "Assets/Editor/_Temp/myso2.asset",
                type = typeof(MySo2)
            };
            items.Add(item2);

            return items;
        }

        [MenuItem("Ares/Examples %t")]
        static void ShowWindow()
        {
            OpenOrClose("Ares Examples");
        }
    }
}