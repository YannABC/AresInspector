using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

namespace Tools
{
    /// <summary>
    /// EditorWindow 默认只在自身没关闭情况下，关闭unity时才会serialize自己, 关闭自身时不会serialize自己
    /// SavableEditorWindow 在关闭自身时会自动序列化public变量和SerializeField的变量
    /// </summary>
    class SavableEditorWindow<T> : EditorWindow where T : EditorWindow
    {
        static T _Inst;
        static string _File = $"UserSettings/{typeof(T).Name}.asset";

        protected static T OpenOrClose(string title)
        {
            if (_Inst == null)
            {
                string dir = Path.GetDirectoryName(_File);
                Directory.CreateDirectory(dir);

                T t;
                if (File.Exists(_File))
                {
                    //t = AssetDatabase.LoadAssetAtPath<T>(_File);
                    t = InternalEditorUtility.LoadSerializedFileAndForget(_File)[0] as T;
                }
                else
                {
                    t = ScriptableObject.CreateInstance<T>();
                    t.hideFlags = HideFlags.None;
                    t.titleContent = new GUIContent(title);
                }
                
                t.Show();

                _Inst = t;
                return t;
            }
            else
            {
                _Inst.Close();
                _Inst = null;
                return null;
            }
        }

        protected virtual void OnDisable()
        {
            //AssetDatabase.CreateAsset(t, _File);
            InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { this }, _File, true);
        }

        //static T GetOrCreateSo<T>(string file) where T : ScriptableObject
        //{
        //    if (!File.Exists(file))
        //    {
        //        var t = ScriptableObject.CreateInstance<T>();
        //        t.hideFlags = HideFlags.None;
        //        AssetDatabase.CreateAsset(t, file);
        //        EditorUtility.SetDirty(t);
        //        AssetDatabase.SaveAssetIfDirty(t);
        //        return t;
        //    }
        //    else
        //    {
        //        var t = AssetDatabase.LoadAssetAtPath<T>(file);
        //        return t;
        //    }
        //}
    }
}