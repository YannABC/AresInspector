using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.IO;
using Unity.VisualScripting;

[System.Serializable]
class SimpleTreeViewEditor : EditorWindow/*, ISerializationCallbackReceiver*/
{
    // SerializeField is used to ensure the view state is written to the window 
    // layout file. This means that the state survives restarting Unity as long as the window
    // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
    //[SerializeField] TreeViewState m_TreeViewState;

    [SerializeField] public int bb;


    SimpleTreeViewEditorSettings _Settings;

    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    SimpleTreeView m_SimpleTreeView;

    UnityEditor.Editor editor;

    //public void OnBeforeSerialize()
    //{
    //    Debug.Log("OnBeforeSerialize");//关闭unity才会serialize, 关闭window不会
    //}

    //public void OnAfterDeserialize()
    //{
    //    Debug.Log("OnAfterDeserialized ");//打开unity才会deserialize
    //}

    void OnEnable()
    {
        string settingsFile = "Assets/Editor/SimpleTreeViewEditorSettings.asset";
        if (!File.Exists(settingsFile))
        {
            _Settings = ScriptableObject.CreateInstance<SimpleTreeViewEditorSettings>();
            AssetDatabase.CreateAsset(_Settings, settingsFile);
        }
        else
        {
            _Settings = AssetDatabase.LoadAssetAtPath<SimpleTreeViewEditorSettings>(settingsFile);
        }
        _Settings.aa++;

        Debug.Log("bb:" + bb);
        bb++;

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
        //EditorUtility.SetDirty(this);
        //AssetDatabase.SaveAssetIfDirty(this);

        EditorUtility.SetDirty(_Settings);
        AssetDatabase.SaveAssetIfDirty(_Settings);
        DestroyImmediate(this.editor);
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        //DestroyImmediate(this, false);
    }

    void OnGUI()
    {
        m_SimpleTreeView.OnGUI(new Rect(0, 0, 150, position.height));
        GUILayout.BeginArea(new Rect(150, 0, position.width - 150, position.height));

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
        //var window = GetWindow<SimpleTreeViewEditor>();
        //window.titleContent = new GUIContent("My Window");
        //window.Show();

        //SimpleTreeViewEditor editorWindow = ScriptableObject.CreateInstance<SimpleTreeViewEditor>();
        //editorWindow.titleContent = new GUIContent("My Window");

        //editorWindow.Show();

        //MySo obj = AssetDatabase.LoadAssetAtPath<MySo>("Assets/Editor/SoInst.asset");

        //Assets/Editor/_Temp/SimpleTreeViewEditor.asset
        //Object t = AssetDatabase.LoadAssetAtPath<Object>("Assets/Editor/_Temp/SimpleTreeViewEditor.asset");
        //Object t = AssetDatabase.LoadAssetAtPath<Object>("Assets/SimpleTreeViewEditor.asset");

        //Debug.Log("t is " + ((t == null)? "Null":t.ToString()));

        GetOrCreateSo<MySo>("Assets/MySo.asset");
        ShowEditorWindow<SimpleTreeViewEditor>("Tools");
    }

    static T GetOrCreateSo<T>(string file) where T : ScriptableObject
    {
        if (!File.Exists(file))
        {
            var t = ScriptableObject.CreateInstance<T>();
            t.hideFlags = HideFlags.None;
            AssetDatabase.CreateAsset(t, file);
            EditorUtility.SetDirty(t);
            AssetDatabase.SaveAssetIfDirty(t);
            return t;
        }
        else
        {
            var t = AssetDatabase.LoadAssetAtPath<T>(file);
            return t;
        }
    }

    static T ShowEditorWindow<T>(string title) where T : EditorWindow
    {
        //试试不放在Assets下
        string file = $"Assets/Editor/_Temp/{typeof(T).Name}.asset";
        Directory.CreateDirectory("Assets/Editor/_Temp");
        var t = GetOrCreateSo<T>(file);
        t = Object.Instantiate(t);//复制一份，否则关闭时会被destroy
        if (t != null)
        {
            t.titleContent = new GUIContent(title);
            t.Show();
        }
        return t;
    }
}