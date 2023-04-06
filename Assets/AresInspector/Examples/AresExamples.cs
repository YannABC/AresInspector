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
        List<AresTreeItem> _Items;
        public override List<AresTreeItem> GetTreeItems()
        {
            _Items = new List<AresTreeItem>();

            AddItem(1, 0, "Overview", null);

            #region Drawer
            {
                int id = 1000;
                AddItem(id++, 0, "Drawer", null);
                AddItem(id++, 1, "Button", typeof(ExampleButton));
                AddItem(id++, 1, "DropDown", typeof(ExampleDropDown));
                AddItem(id++, 1, "HelpBox", typeof(ExampleHelpBox));
                AddItem(id++, 1, "Slider", typeof(ExampleSlider));
                AddItem(id++, 1, "Space", typeof(ExampleSpace));
            }
            #endregion

            #region Config
            {
                int id = 2000;
                AddItem(id++, 0, "Config", null);
                AddItem(id++, 1, "Background Color", typeof(ExampleBgColor));
                AddItem(id++, 1, "Label Color", typeof(ExampleLabelColor));
                AddItem(id++, 1, "Label Text", typeof(ExampleLabelText));
                AddItem(id++, 1, "Label Width", typeof(ExampleLabelWidth));
                AddItem(id++, 1, "Delayed", typeof(ExampleDelayed));
                AddItem(id++, 1, "Enable If", typeof(ExampleEnableIf));
                AddItem(id++, 1, "Show If", typeof(ExampleShowIf));
                AddItem(id++, 1, "Button Font Size", typeof(ExampleFontSize));
                AddItem(id++, 1, "Inline", typeof(ExampleInline));
                AddItem(id++, 1, "Layout", typeof(ExampleLayout));
                AddItem(id++, 1, "On Value Changed", typeof(ExampleValueChanged));
                AddItem(id++, 1, "AssetsOnly", typeof(ExampleAssetsOnly));
            }
            #endregion

            #region Layout
            {
                int id = 3000;
                AddItem(id++, 0, "Layout", null);
                AddItem(id++, 1, "Layout Horizontal", typeof(ExampleLayoutHorizontal));
                AddItem(id++, 1, "Layout Vertical", typeof(ExampleLayoutVertical));
                AddItem(id++, 1, "Layout Nest", typeof(ExampleLayoutNest));
                AddItem(id++, 1, "Layout Adjust Order", typeof(ExampleAdjustOrder));
            }
            #endregion

            return _Items;
        }

        void AddItem(int id, int depth, string displayName, System.Type type)
        {
            AresTreeItem item = new AresTreeItem(id, depth, displayName);
            if (type != null)
            {
                item.type = type;
                item.file = $"Assets/AresInspector/Examples/_Temp/{type.Name}.asset";
            }
            _Items.Add(item);
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
            OpenOrClose("Ares Examples", "UserSettings/AresExamples.asset", typeof(AresExamples));
        }

        const string OverView = @"AresInspector 是 类似odin的编辑器，使用Attribute的方式来写编辑器。
和odin不同的是，AresInspector更加轻量，使用了纯UIToolkit(VisualEment),没有使用IMGUI。
包含<color=blue>Layout</color>,<color=blue>Drawer</color>和<color=blue>Config</color>.";
    }
}
#endif