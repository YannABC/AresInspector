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

        protected override VisualElement CreateFooterGUI(AresTreeItem ati)
        {
            if (string.IsNullOrEmpty(ati.file)) return null;
            VisualElement root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;

            Button btnAsset;
            if (ati.file.StartsWith("Assets/"))
            {
                btnAsset = new Button(() =>
                {
                    Object obj = AssetDatabase.LoadAssetAtPath(ati.file, ati.type);
                    Selection.activeObject = obj;
                });
            }
            else
            {
                btnAsset = new Button(() =>
                {
                    string dir = Path.GetDirectoryName(ati.file);
                    Application.OpenURL(dir);
                });
            }
            btnAsset.text = "Goto Asset";
            root.Add(btnAsset);

            Button btnScript = new Button(() =>
            {
                Debug.Log("goto scprit");
                string path = FindClassFile(ati.type.Name);
                Application.OpenURL(path);
            });
            btnScript.text = "Goto Script";
            root.Add(btnScript);

            Label lbl = new Label(ati.file);
            lbl.style.unityTextAlign = TextAnchor.MiddleLeft;
            root.Add(lbl);

            return root;
        }

        protected string FindClassFile(string name)
        {
            DirectoryInfo direction = new DirectoryInfo(Application.dataPath);
            FileInfo[] files = direction.GetFiles("*.cs", SearchOption.AllDirectories);

            string fullPath = "";
            string targetFileName = name + ".cs";
            for (int i = 0; i < files.Length; i++)
            {
                if (targetFileName == files[i].Name)
                {
                    fullPath = files[i].FullName;
                    break;
                }
            }
            return fullPath;
        }

        //protected override VisualElement CreateHeaderGUI(AresTreeItem ati)
        //{
        //    Label lbl = new Label("header");
        //    return lbl;
        //}

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