using Ares;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

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
            //Debug.Log("OnOpen6 " + displayName);
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
            if (m_Obj == null)
            {
                //When calling AssetDatabase.LoadAssetAtPath during a domain reload,
                //it may return null if the asset has not been loaded yet.
                //This is because the asset may not have been loaded back into memory yet.
                VisualElement root = new VisualElement();

                EditorApplication.CallbackFunction a = null;
                a = () =>
                {
                    m_Obj = isRelativeAssets ?
                    AssetDatabase.LoadAssetAtPath(file, type) :
                    InternalEditorUtility.LoadSerializedFileAndForget(file)[0];
                    if (m_Obj != null)
                    {
                        //Debug.Log($"delay add {type.Name} success");
                        root.Add(CreateGUI(m_Obj));
                        EditorApplication.delayCall -= a;
                    }
                    else
                    {
                        //Debug.Log($"delay add {type.Name} still null");
                    }
                };
                EditorApplication.delayCall += a;
                //Debug.Log("delay add " + type.Name);
                return root;
            }
            else
            {
                return CreateGUI(m_Obj);
            }
        }

        VisualElement CreateGUI(Object obj)
        {
            AresGroup group = AresGroup.Get(m_Obj.GetType());
            if (group != null)
            {
                VisualElement root = group.CreateGUI(new AresContext(new SerializedObject(m_Obj)));
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
            if (m_Obj != null)
            {
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