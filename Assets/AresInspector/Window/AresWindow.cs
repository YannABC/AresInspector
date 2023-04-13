using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

#if UNITY_EDITOR
namespace Ares
{
    /// <summary>
    /// EditorWindow 默认只在自身没关闭情况下，关闭unity时才会serialize自己, 关闭自身时不会serialize自己
    /// AresWindow 在关闭自身时会自动序列化public变量和SerializeField的变量
    /// </summary>
    public class AresWindow : EditorWindow
    {
        //static string  s_File = $"UserSettings/{typeof(T).Name}.asset";

        string m_File;

        protected static AresWindow OpenOrClose(string title, string file, System.Type type)
        {
            AresWindow inst = GetAresWindow(type);
            if (!inst)
            {
                string dir = Path.GetDirectoryName(file);
                Directory.CreateDirectory(dir);

                AresWindow t;
                if (File.Exists(file))
                {
                    //t = AssetDatabase.LoadAssetAtPath<T>(_File);
                    t = InternalEditorUtility.LoadSerializedFileAndForget(file)[0] as AresWindow;
                }
                else
                {
                    t = ScriptableObject.CreateInstance(type) as AresWindow;
                    t.hideFlags = HideFlags.None;
                    t.titleContent = new GUIContent(title);
                }
                t.m_File = file;
                t.Show();

                return t;
            }
            else
            {
                inst.Close();
                return null;
            }
        }

        static AresWindow GetAresWindow(System.Type type)
        {
            UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(type);
            EditorWindow editorWindow = ((array.Length != 0) ? ((EditorWindow)array[0]) : null);
            return editorWindow as AresWindow;
        }

        protected virtual void OnDisable()
        {
            //AssetDatabase.CreateAsset(t, _File);
            InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { this }, m_File, true);
        }
    }
}
#endif