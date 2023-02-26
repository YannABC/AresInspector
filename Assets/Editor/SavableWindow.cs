using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

namespace Tools
{
    /// <summary>
    /// EditorWindow 默认只在自身没关闭情况下，关闭unity时才会serialize自己, 关闭自身时不会serialize自己
    /// SavableWindow 在关闭自身时会自动序列化public变量和SerializeField的变量
    /// </summary>
    class SavableWindow<T> : EditorWindow where T : EditorWindow
    {
        static string s_File = $"UserSettings/{typeof(T).Name}.asset";

        protected static T OpenOrClose(string title)
        {
            T inst = GetWindow();
            if (!inst)
            {
                string dir = Path.GetDirectoryName(s_File);
                Directory.CreateDirectory(dir);

                T t;
                if (File.Exists(s_File))
                {
                    //t = AssetDatabase.LoadAssetAtPath<T>(_File);
                    t = InternalEditorUtility.LoadSerializedFileAndForget(s_File)[0] as T;
                }
                else
                {
                    t = ScriptableObject.CreateInstance<T>();
                    t.hideFlags = HideFlags.None;
                    t.titleContent = new GUIContent(title);
                }

                t.Show();

                return t;
            }
            else
            {
                inst.Close();
                return null;
            }
        }

        static T GetWindow()
        {
            UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(T));
            EditorWindow editorWindow = ((array.Length != 0) ? ((EditorWindow)array[0]) : null);
            return editorWindow as T;
        }

        protected virtual void OnDisable()
        {
            //AssetDatabase.CreateAsset(t, _File);
            InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { this }, s_File, true);
        }
    }
}