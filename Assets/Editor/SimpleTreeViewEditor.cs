using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine.UIElements;
using System;

class SimpleTreeViewWindow : EditorWindow
{
    // SerializeField is used to ensure the view state is written to the window 
    // layout file. This means that the state survives restarting Unity as long as the window
    // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
    [SerializeField] TreeViewState m_TreeViewState;

    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    SimpleTreeView m_SimpleTreeView;

    UnityEditor.Editor editor;

    void OnEnable()
    {
        // Check whether there is already a serialized view state (state 
        // that survived assembly reloading)
        if (m_TreeViewState == null)
            m_TreeViewState = new TreeViewState();

        m_SimpleTreeView = new SimpleTreeView(m_TreeViewState);

        MySo obj = AssetDatabase.LoadAssetAtPath<MySo>("Assets/SoInst.asset");
        editor = UnityEditor.Editor.CreateEditor(obj);
    }

    void OnGUI()
    {
        m_SimpleTreeView.OnGUI(new Rect(0, 0, 150, position.height));
        GUILayout.BeginArea(new Rect(150,0,position.width - 150, position.height));
        //EditorGUILayout.LabelField("aaaa");


        editor.OnInspectorGUI();
        GUILayout.EndArea();
    }

    // Add menu named "My Window" to the Window menu
    [MenuItem("TreeView Examples/Simple Tree Window")]
    static void ShowWindow()
    {
        // Get existing open window or if none, make a new one:
        var window = GetWindow<SimpleTreeViewWindow>();
        window.titleContent = new GUIContent("My Window");
        window.Show();
    }
}