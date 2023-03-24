using Ares;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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
        Object m_Obj;

        bool isRelativeAssets => file.StartsWith("Assets/");

        internal VisualElement OnOpen()
        {
            //Debug.Log("OnOpen " + displayName);
            if (!File.Exists(file))
            {
                string dir = Path.GetDirectoryName(file);
                Directory.CreateDirectory(dir);

                m_Obj = ScriptableObject.CreateInstance(type);
                if (isRelativeAssets)
                {
                    AssetDatabase.CreateAsset(m_Obj, file);
                }
                else
                {
                    InternalEditorUtility.SaveToSerializedFileAndForget(
                        new Object[] { m_Obj }, file, allowTextSerialization: true);
                }
            }

            {
                m_Obj = isRelativeAssets ?
                    AssetDatabase.LoadAssetAtPath(file, type) :
                    InternalEditorUtility.LoadSerializedFileAndForget(file)[0];
            }

            AresGroup group = AresGroup.Get(m_Obj.GetType());
            if (group != null)
            {
                VisualElement root = group.CreateGUI(new AresContext(new SerializedObject(m_Obj)));


                //EditorApplication.update += () =>
                //{
                //    root.MarkDirtyRepaint();
                //};
                return root;
            }
            else
            {
                m_ObjEditor = Editor.CreateEditor(m_Obj, typeof(AresEditor));
                return new IMGUIContainer(DrawEditor);
            }
        }

        void DrawEditor()
        {
            m_ObjEditor?.OnInspectorGUI();
        }

        internal void OnClose()
        {
            //Debug.Log("OnClose " + displayName);
            if (isRelativeAssets)
            {
                EditorUtility.SetDirty(m_Obj);
                AssetDatabase.SaveAssetIfDirty(m_Obj);
            }
            else
            {
                InternalEditorUtility.SaveToSerializedFileAndForget(
                    new Object[] { m_Obj }, file, allowTextSerialization: true);
            }

            if (m_ObjEditor != null)
            {
                Object.DestroyImmediate(m_ObjEditor);
                m_ObjEditor = null;
            }
        }

        //internal void OnDraw()
        //{
        //    //Debug.Log("OnDraw " + displayName);
        //    m_ObjEditor.OnInspectorGUI();
        //}
    }
}