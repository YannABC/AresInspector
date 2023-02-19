using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UIElements;

namespace Tools
{
    class TestSplitWindow : EditorWindow
    {
        private void OnEnable()
        {
            var v = new TwoPaneSplitView(0, 150, TwoPaneSplitViewOrientation.Horizontal);
            v.Add(new VisualElement());
            v.Add(new VisualElement());
            rootVisualElement.Add(v);
        }

        [MenuItem("Tools/Split Window")]
        static void ShowWindow()
        {
            var window = GetWindow<TestSplitWindow>();
            window.titleContent = new GUIContent("Tools");
            window.Show();

            //OpenOrClose("Tools");
        }
    }
}