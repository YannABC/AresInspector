using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Tools
{
    class ToolsTreeItem : TreeViewItem
    {
        public string file;
        public System.Type type;

        Editor m_ObjEditor;

        internal void OnOpen()
        {
            //Debug.Log("OnOpen " + displayName);

            if (File.Exists(file))
            {
                Object obj = AssetDatabase.LoadAssetAtPath(file, type);
                m_ObjEditor = Editor.CreateEditor(obj);
            }
            else
            {
                string dir = Path.GetDirectoryName(file);
                Directory.CreateDirectory(dir);

                Object obj = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(obj, file);
                m_ObjEditor = Editor.CreateEditor(obj);
            }
        }

        internal void OnClose()
        {
            //Debug.Log("OnClose " + displayName);
            Object.DestroyImmediate(m_ObjEditor);
            m_ObjEditor = null;
        }

        internal void OnDraw()
        {
            //Debug.Log("OnDraw " + displayName);
            m_ObjEditor.OnInspectorGUI();
        }
    }
}