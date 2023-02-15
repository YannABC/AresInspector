using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;
using System.IO;

class SimpleTreeViewEditor : EditorWindow/*, ISerializationCallbackReceiver*/
{
    // SerializeField is used to ensure the view state is written to the window 
    // layout file. This means that the state survives restarting Unity as long as the window
    // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
    //[SerializeField] TreeViewState m_TreeViewState;


    SimpleTreeViewEditorSettings _Settings;

    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    SimpleTreeView m_SimpleTreeView;

    UnityEditor.Editor editor;

    //public void OnBeforeSerialize()
    //{
    //    Debug.Log("OnBeforeSerialize");
    //}

    //public void OnAfterDeserialize()
    //{
    //    Debug.Log("OnAfterDeserialized      ");
    //}

    void OnEnable()
    {
        string settingsFile = "Assets/Editor/SimpleTreeViewEditorSettings.asset";
        if (!File.Exists(settingsFile))
        {
            SimpleTreeViewEditorSettings s = ScriptableObject.CreateInstance<SimpleTreeViewEditorSettings>();
            AssetDatabase.CreateAsset(s, settingsFile);
        }
        _Settings = AssetDatabase.LoadAssetAtPath<SimpleTreeViewEditorSettings>(settingsFile);
        _Settings.aa++;

        // Check whether there is already a serialized view state (state 
        // that survived assembly reloading)
        if (_Settings.TreeState == null)
            _Settings.TreeState = new TreeViewState();

        m_SimpleTreeView = new SimpleTreeView(_Settings.TreeState);

        //IList<int>  lst = m_SimpleTreeView.GetSelection();


        MySo obj = AssetDatabase.LoadAssetAtPath<MySo>("Assets/Editor/SoInst.asset");
        editor = Editor.CreateEditor(obj);


        //MySo newScriptableObject = ScriptableObject.CreateInstance<MySo>();
        //AssetDatabase.CreateAsset(newScriptableObject, "Assets/Editor/SO.asset");
    }

    void OnDisable()
    {
        AssetDatabase.SaveAssetIfDirty(_Settings);
        DestroyImmediate(this.editor);
    }

    private void OnDestroy()
    {
        
    }

    void OnGUI()
    {
        //m_TreeViewState.Serialize(this);
        m_SimpleTreeView.OnGUI(new Rect(0, 0, 150, position.height));
        GUILayout.BeginArea(new Rect(150,0,position.width - 150, position.height));
        //EditorGUILayout.LabelField("aaaa");


        //editor.OnInspectorGUI();

        SerializedObject obj = editor.serializedObject;

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
    [MenuItem("TreeView Examples/Simple Tree Window")]
    static void ShowWindow()
    {
        // Get existing open window or if none, make a new one:
        var window = GetWindow<SimpleTreeViewEditor>();
        window.titleContent = new GUIContent("My Window");
        window.Show();
    }
}