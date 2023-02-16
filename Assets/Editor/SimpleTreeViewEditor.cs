using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.IO;
using Unity.VisualScripting;

namespace Tools
{
    class SimpleTreeViewEditor : SavableEditorWindow<SimpleTreeViewEditor>
    {
        public int bb;                             //auto serialize
        [SerializeField] TreeViewState _TreeState; //serializable

        SimpleTreeView _TreeView;  // not serialize
        Editor _ObjEditor;

        void OnEnable()
        {

            Debug.Log("bb:" + bb);
            bb++;

            if (_TreeState == null)
                _TreeState = new TreeViewState();

            _TreeView = new SimpleTreeView(_TreeState);

            //IList<int>  lst = m_SimpleTreeView.GetSelection();


            MySo obj = AssetDatabase.LoadAssetAtPath<MySo>("Assets/Editor/SoInst.asset");
            _ObjEditor = Editor.CreateEditor(obj);


            //MySo newScriptableObject = ScriptableObject.CreateInstance<MySo>();
            //AssetDatabase.CreateAsset(newScriptableObject, "Assets/Editor/SO.asset");
        }

        void OnGUI()
        {
            _TreeView.OnGUI(new Rect(0, 0, 150, position.height));
            GUILayout.BeginArea(new Rect(150, 0, position.width - 150, position.height));

            SerializedObject obj = _ObjEditor.serializedObject;

            EditorGUI.BeginChangeCheck();
            obj.UpdateIfRequiredOrScript();
            SerializedProperty iterator = obj.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                //using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                //{
                //    EditorGUILayout.PropertyField(iterator, true);
                //}

                if (iterator.propertyPath == "m_Script") continue;
                EditorGUILayout.PropertyField(iterator, true);

                enterChildren = false;
            }

            obj.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();

            GUILayout.EndArea();
        }

        // Add menu named "My Window" to the Window menu
        [MenuItem("TreeView Examples/Simple Tree Window %t")]
        static void ShowWindow()
        {
            //var window = GetWindow<SimpleTreeViewEditor>();
            //window.titleContent = new GUIContent("Tools");
            //window.Show();

            OpenOrClose("Tools");
        }
    }
}