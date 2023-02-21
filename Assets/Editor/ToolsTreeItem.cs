using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// file 可以在如下目录
    /// 1. Assets/              游戏数据，运行时用的到
    /// 2. UserSettings/        用户数据，不受版本控制
    /// 3. ProjectSettings/     项目数据，受版本控制
    /// </summary>
    class ToolsTreeItem : TreeViewItem
    {
        public string file;
        public System.Type type;

        Editor m_ObjEditor;

        bool IsRelativeAssets => file.StartsWith("Assets/");

        internal void OnOpen()
        {
            //Debug.Log("OnOpen " + displayName);
            if (!File.Exists(file))
            {
                string dir = Path.GetDirectoryName(file);
                Directory.CreateDirectory(dir);

                Object obj = ScriptableObject.CreateInstance(type);
                if (IsRelativeAssets)
                {
                    AssetDatabase.CreateAsset(obj, file);
                }
                else
                {
                    InternalEditorUtility.SaveToSerializedFileAndForget(
                        new Object[] { obj }, file, allowTextSerialization: true);
                }
            }

            {
                Object obj = IsRelativeAssets ?
                    AssetDatabase.LoadAssetAtPath(file, type) :
                    InternalEditorUtility.LoadSerializedFileAndForget(file)[0];
                m_ObjEditor = Editor.CreateEditor(obj);
            }
        }

        internal void OnClose()
        {
            //Debug.Log("OnClose " + displayName);
            if (!IsRelativeAssets)
            {
                InternalEditorUtility.SaveToSerializedFileAndForget(
                    new Object[] { m_ObjEditor.target }, file, allowTextSerialization: true);
            }
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